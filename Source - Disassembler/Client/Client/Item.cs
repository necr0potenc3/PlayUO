namespace Client
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Runtime.InteropServices;

    public class Item : IComparable, IPoint3D, IEntity, IMessageOwner, IPoint2D, IAnimationOwner
    {
        private short m_Amount;
        private BBMessageBody m_BBBody;
        private BBMessageHeader m_BBHeader;
        private int m_BookIconX = 0x19;
        private int m_BookIconY = 0x19;
        private int m_BottomY;
        private int m_Circle;
        private IContainer m_Container;
        private short m_ContainerX;
        private short m_ContainerY;
        private ArrayList m_CorpseEquip = new ArrayList(0);
        private int m_CorpseSerial;
        private byte m_Direction;
        private ArrayList m_Equip = new ArrayList(0);
        private object m_EquipParent;
        private Frames m_fAnimationPool;
        private static Queue m_FindItem_Queue;
        private static Queue m_FindItems_Queue;
        private ItemFlags m_Flags = new ItemFlags();
        private IHue m_hAnimationPool;
        private ushort m_Hue;
        private int m_iAnimationPool;
        private short m_ID;
        private bool m_InWorld;
        private bool m_IsEquip;
        private bool m_IsMulti;
        private ArrayList m_Items = new ArrayList(0);
        private Mobile m_LastLooked;
        private int m_LastSpell = -1;
        public string m_LastText;
        private IHue m_LastTextHue;
        private int m_MessageFrame;
        private int m_MessageX;
        private int m_MessageY;
        private Client.Multi m_Multi;
        private DateTime m_NextQueryProps;
        private int m_OldMapX;
        private int m_OldMapY;
        private bool m_OpenSB;
        private Item m_Parent;
        private int m_Props;
        private bool m_QueueOpenSB;
        private int m_RealHue;
        private Client.RestoreInfo m_RestoreInfo;
        private int m_Revision = -1;
        private int m_Serial;
        private bool m_ShouldOverrideHue;
        private bool m_Sort;
        private int m_SpellbookGraphic;
        private int m_SpellbookOffset;
        private long m_SpellContained;
        private Texture m_tPool;
        private bool m_Traced = false;
        private bool m_TradeCheck1;
        private bool m_TradeCheck2;
        private VertexCache m_vCache;
        private bool m_Visible;
        private short m_X;
        private short m_Y;
        private short m_Z;

        public Item(int serial)
        {
            this.m_Serial = serial;
        }

        public void AddItem(Item toAdd)
        {
            if (toAdd != null)
            {
                object parent = this;
                bool flag = false;
                while (parent != null)
                {
                    if (!(parent is Item))
                    {
                        break;
                    }
                    Item item = (Item) parent;
                    if (((item.Container != null) && (item.Container.Gump is GContainer)) && ((GContainer) item.Container.Gump).m_TradeContainer)
                    {
                        flag = true;
                    }
                    if (flag)
                    {
                        break;
                    }
                    parent = item.Parent;
                }
                if (flag)
                {
                    toAdd.QueryProperties();
                }
                if (!this.m_Items.Contains(toAdd))
                {
                    if ((toAdd.Parent != null) && (toAdd.Parent != this))
                    {
                        toAdd.Parent.RemoveItem(toAdd);
                    }
                    this.m_Items.Add(toAdd);
                    toAdd.Parent = this;
                    if (this.m_Container != null)
                    {
                        this.m_Container.OnItemAdd(toAdd);
                    }
                }
                else if (this.m_Container != null)
                {
                    this.m_Container.OnItemRefresh(toAdd);
                }
                this.m_Sort = true;
            }
        }

        public void AddTextMessage(string Name, string Message, IFont Font, IHue Hue, bool unremovable)
        {
            this.m_LastTextHue = Hue;
            this.m_LastText = Message;
            string text = null;
            if (Name.Length > 0)
            {
                text = Name + ": " + Message;
            }
            else
            {
                text = Message;
            }
            if (Message.Length > 0)
            {
                Engine.AddToJournal(new JournalEntry(text, Hue, this.m_Serial));
                Message = Engine.WrapText(Message, 200, Font).TrimEnd(new char[0]);
                if (Message.Length > 0)
                {
                    MessageManager.AddMessage(new GDynamicMessage(unremovable, this, Message, Font, Hue));
                }
            }
        }

        public void AutoTrace(string name)
        {
            if (!this.m_Traced)
            {
                this.m_Traced = true;
            }
        }

        private bool CheckItemID(params int[] vals)
        {
            int num = this.m_ID & 0x3fff;
            bool flag = false;
            for (int i = 0; !flag && (i < vals.Length); i++)
            {
                flag = num == vals[i];
            }
            return flag;
        }

        Frames IAnimationOwner.GetOwnedFrames(IHue hue, int realID)
        {
            if (((this.m_iAnimationPool != realID) || (this.m_hAnimationPool != hue)) || this.m_fAnimationPool.Disposed)
            {
                this.m_fAnimationPool = hue.GetAnimation(realID);
                this.m_hAnimationPool = hue;
                this.m_iAnimationPool = realID;
            }
            return this.m_fAnimationPool;
        }

        public int CompareTo(object x)
        {
            if (x == null)
            {
                return 1;
            }
            if (x.GetType() != typeof(Item))
            {
                throw new ArgumentException();
            }
            Item item = (Item) x;
            if ((this.m_ContainerY < item.m_ContainerY) || ((this.m_ContainerY == item.m_ContainerY) && (this.m_ContainerX < item.m_ContainerX)))
            {
                return -1;
            }
            if ((this.m_ContainerY > item.m_ContainerY) || ((this.m_ContainerY == item.m_ContainerY) && (this.m_ContainerX > item.m_ContainerX)))
            {
                return 1;
            }
            return 0;
        }

        public bool ContainsItem(Item check)
        {
            for (Item item = check; item != null; item = item.Parent)
            {
                if (item == this)
                {
                    return true;
                }
            }
            return false;
        }

        public void Draw(Texture t, int x, int y)
        {
            if (this.m_vCache == null)
            {
                this.m_vCache = new VertexCache();
            }
            if (this.m_tPool != t)
            {
                this.m_tPool = t;
                this.m_vCache.Invalidate();
            }
            this.m_vCache.Draw(t, x, y);
        }

        public void DrawGame(Texture t, int x, int y)
        {
            if (this.m_vCache == null)
            {
                this.m_vCache = new VertexCache();
            }
            if (this.m_tPool != t)
            {
                this.m_tPool = t;
                this.m_vCache.Invalidate();
            }
            this.m_vCache.DrawGame(t, x, y);
        }

        public Item FindItem(IItemValidator check)
        {
            if (check == null)
            {
                throw new ArgumentNullException("check");
            }
            Queue queue = m_FindItem_Queue;
            if (queue == null)
            {
                queue = m_FindItem_Queue = new Queue();
            }
            else if (queue.Count > 0)
            {
                queue.Clear();
            }
            if (check.IsValid(this))
            {
                return this;
            }
            if (this.m_Items.Count > 0)
            {
                queue.Enqueue(this);
                while (queue.Count > 0)
                {
                    Item item = (Item) queue.Dequeue();
                    ArrayList items = item.m_Items;
                    int count = items.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Item item2 = (Item) items[i];
                        if (check.IsValid(item2))
                        {
                            return item2;
                        }
                        if (item2.m_Items.Count > 0)
                        {
                            queue.Enqueue(item2);
                        }
                    }
                }
            }
            return null;
        }

        public Item[] FindItems(IItemValidator check)
        {
            if (check == null)
            {
                throw new ArgumentNullException("check");
            }
            ArrayList dataStore = Engine.GetDataStore();
            Queue queue = m_FindItems_Queue;
            if (queue == null)
            {
                queue = m_FindItems_Queue = new Queue();
            }
            else if (queue.Count > 0)
            {
                queue.Clear();
            }
            if (check.IsValid(this))
            {
                dataStore.Add(this);
            }
            if (this.m_Items.Count > 0)
            {
                queue.Enqueue(this);
                while (queue.Count > 0)
                {
                    Item item = (Item) queue.Dequeue();
                    ArrayList items = item.m_Items;
                    int count = items.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Item item2 = (Item) items[i];
                        if (check.IsValid(item2))
                        {
                            dataStore.Add(item2);
                        }
                        if (item2.m_Items.Count > 0)
                        {
                            queue.Enqueue(item2);
                        }
                    }
                }
            }
            Item[] itemArray = (Item[]) dataStore.ToArray(typeof(Item));
            Engine.ReleaseDataStore(dataStore);
            return itemArray;
        }

        public void GetAbsoluteLocation(out int x, out int y, out int z)
        {
            object obj2 = this;
            while (obj2 != null)
            {
                object equipParent;
                if (obj2 is Item)
                {
                    if (((Item) obj2).EquipParent is Mobile)
                    {
                        equipParent = ((Item) obj2).EquipParent;
                    }
                    else
                    {
                        equipParent = ((Item) obj2).Parent;
                    }
                }
                else
                {
                    equipParent = null;
                }
                if (equipParent == null)
                {
                    break;
                }
                obj2 = equipParent;
            }
            if (obj2 is Item)
            {
                x = ((Item) obj2).X;
                y = ((Item) obj2).Y;
                z = ((Item) obj2).Z;
            }
            else if (obj2 is Mobile)
            {
                x = ((Mobile) obj2).X;
                y = ((Mobile) obj2).Y;
                z = ((Mobile) obj2).Z;
            }
            else
            {
                x = -1;
                y = -1;
                z = 0;
            }
        }

        public bool GetSpellContained(int index)
        {
            long num = ((long) 1L) << index;
            return ((this.m_SpellContained & num) != 0L);
        }

        public bool InAbsSquareRange(IPoint2D p, int xyRange)
        {
            int num;
            int num2;
            int num3;
            if (p == null)
            {
                return false;
            }
            this.GetAbsoluteLocation(out num, out num2, out num3);
            return ((((num >= (p.X - xyRange)) && (num <= (p.X + xyRange))) && (num2 >= (p.Y - xyRange))) && (num2 <= (p.Y + xyRange)));
        }

        public bool InAbsSquareRange(int xTile, int yTile, int xyRange)
        {
            int num;
            int num2;
            int num3;
            this.GetAbsoluteLocation(out num, out num2, out num3);
            return ((((num >= (xTile - xyRange)) && (num <= (xTile + xyRange))) && (num2 >= (yTile - xyRange))) && (num2 <= (yTile + xyRange)));
        }

        public bool InSquareRange(IPoint2D p, int xyRange)
        {
            if (p == null)
            {
                return false;
            }
            return ((((this.m_X >= (p.X - xyRange)) && (this.m_X <= (p.X + xyRange))) && (this.m_Y >= (p.Y - xyRange))) && (this.m_Y <= (p.Y + xyRange)));
        }

        public bool InSquareRange(int xTile, int yTile, int xyRange)
        {
            return ((((this.m_X >= (xTile - xyRange)) && (this.m_Y >= (yTile - xyRange))) && (this.m_X <= (xTile + xyRange))) && (this.m_Y <= (yTile + xyRange)));
        }

        public bool IsChildOf(Mobile mob)
        {
            if (mob != null)
            {
                Item backpack = mob.Backpack;
                if (backpack == null)
                {
                    return false;
                }
                for (Item item2 = this.Parent; item2 != null; item2 = item2.Parent)
                {
                    if (item2 == backpack)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Look()
        {
            return Network.Send(new PLookRequest(this));
        }

        public Gump OnBeginDrag()
        {
            int num = this.m_ID & 0x3fff;
            int num2 = (ushort) this.m_Amount;
            if (Map.m_ItemFlags[num & 0x3fff][TileFlag.Generic] && (num2 > 1))
            {
                GDragAmount amount = new GDragAmount(this);
                Gumps.Desktop.Children.Add(amount);
                return amount;
            }
            Network.Send(new PPickupItem(this, this.m_Amount));
            GDraggedItem toAdd = new GDraggedItem(this);
            Gumps.Desktop.Children.Add(toAdd);
            return toAdd;
        }

        public void OnDoubleClick()
        {
            this.Use();
            PUseRequest.Last = this;
        }

        public void OnSingleClick()
        {
            this.Look();
        }

        public void OnTarget()
        {
            Engine.Target(this);
        }

        public void OverrideHue(int hue)
        {
            bool shouldOverrideHue = this.m_ShouldOverrideHue;
            this.m_ShouldOverrideHue = hue >= 0;
            if (this.m_ShouldOverrideHue)
            {
                if (!shouldOverrideHue)
                {
                    this.m_RealHue = this.m_Hue;
                    this.m_Hue = (ushort) hue;
                }
            }
            else if (shouldOverrideHue)
            {
                this.m_Hue = (ushort) this.m_RealHue;
            }
        }

        public void Query()
        {
        }

        public void QueryProperties()
        {
            if (DateTime.Now >= this.m_NextQueryProps)
            {
                this.m_NextQueryProps = DateTime.Now + TimeSpan.FromSeconds(1.0);
                Network.Send(new PQueryProperties(this.m_Serial));
            }
        }

        public void RemoveEquip()
        {
            this.m_IsEquip = false;
            if (this.m_EquipParent != null)
            {
                ArrayList equip;
                if (this.m_EquipParent is Mobile)
                {
                    equip = ((Mobile) this.m_EquipParent).Equip;
                }
                else
                {
                    if (!(this.m_EquipParent is Item))
                    {
                        return;
                    }
                    equip = ((Item) this.m_EquipParent).Equip;
                }
                int index = 0;
                int count = equip.Count;
                bool flag = false;
                while (index < count)
                {
                    EquipEntry entry = (EquipEntry) equip[index];
                    if (entry.m_Item == this)
                    {
                        equip.RemoveAt(index);
                        count--;
                        flag = true;
                    }
                    else
                    {
                        index++;
                    }
                }
                if (flag && (this.m_EquipParent is Mobile))
                {
                    ((Mobile) this.m_EquipParent).EquipRemoved();
                }
                this.m_EquipParent = null;
            }
        }

        public void RemoveItem(Item toRemove)
        {
            if (toRemove != null)
            {
                if (this.m_Container != null)
                {
                    this.m_Container.OnItemRemove(toRemove);
                }
                if (this.m_Items.Contains(toRemove))
                {
                    this.m_Items.Remove(toRemove);
                    toRemove.Parent = null;
                }
            }
        }

        public void SetLocation(short x, short y, short z)
        {
            this.m_Z = z;
            if ((this.m_X != x) || (this.m_Y != y))
            {
                this.m_X = x;
                this.m_Y = y;
                if (this.m_InWorld)
                {
                    if (this.m_IsMulti)
                    {
                        Engine.Multis.Sort();
                        Map.Invalidate();
                        GRadar.Invalidate();
                    }
                    else if (!World.InUpdateRange(this))
                    {
                        World.Remove(this);
                    }
                }
            }
        }

        public void SetSpellContained(int index, bool value)
        {
            long num = ((long) 1L) << index;
            if (value)
            {
                this.m_SpellContained |= num;
            }
            else
            {
                this.m_SpellContained &= ~num;
            }
        }

        public void SubTrace(StreamWriter sw, int depth)
        {
            string str = new string(' ', 1 + (depth * 3));
            sw.WriteLine("{0}#", str);
            sw.WriteLine("{0}// {1}", str, Localization.GetString(0xf9060 + this.m_ID));
            sw.WriteLine((this.m_Hue == 0) ? "{2}Image: 0x{0:X}" : "{2}Image: 0x{0:X} Hue: 0x{1:X}", this.m_ID, this.m_Hue, str);
            sw.WriteLine("{0}Amount: {1}", str, this.m_Amount);
            sw.WriteLine("{2}Location: {0}, {1}", this.m_ContainerX, this.m_ContainerY, str);
            foreach (Item item in this.m_Items)
            {
                item.SubTrace(sw, depth + 1);
            }
            sw.WriteLine("{0}#", str);
        }

        public void Trace()
        {
            using (StreamWriter writer = new StreamWriter("subtrace.log", true))
            {
                this.SubTrace(writer, 0);
            }
            Debug.Trace("Serial: 0x{0:X8}", this.m_Serial);
            Debug.Trace("Location: ( {0}, {1}, {2} )", this.m_X, this.m_Y, this.m_Z);
            Debug.Trace("Container Location: ( {0}, {1} )", this.m_ContainerX, this.m_ContainerY);
            Debug.Trace("Visible: {0}", this.m_Visible);
            Debug.Trace("Hue: {0} ( 0x{0:X} )", this.m_Hue);
            Debug.Trace("HasParent: {0}", this.m_Parent != null);
            Debug.Trace("ID: {0} ( 0x{0:X} )", this.m_ID);
            Debug.Trace("Direction: {0}", this.m_Direction);
            Debug.Trace("Amount: {0}", this.m_Amount);
            Debug.Trace("Flags: {0}", this.m_Flags);
            Debug.Trace("IsMulti: {0}", this.m_IsMulti);
            Debug.Trace("IsEquip: {0}", this.m_IsEquip);
            Debug.Trace("InWorld: {0}", this.m_InWorld);
        }

        public void Update()
        {
            Map.UpdateItem(this);
        }

        public bool Use()
        {
            return Network.Send(new PUseRequest(this));
        }

        public short Amount
        {
            get
            {
                return this.m_Amount;
            }
            set
            {
                this.m_Amount = value;
            }
        }

        public int BookIconX
        {
            get
            {
                return this.m_BookIconX;
            }
            set
            {
                this.m_BookIconX = value;
            }
        }

        public int BookIconY
        {
            get
            {
                return this.m_BookIconY;
            }
            set
            {
                this.m_BookIconY = value;
            }
        }

        public int BottomY
        {
            get
            {
                return this.m_BottomY;
            }
            set
            {
                this.m_BottomY = value;
            }
        }

        public BBMessageBody BulletinBody
        {
            get
            {
                return this.m_BBBody;
            }
            set
            {
                this.m_BBBody = value;
            }
        }

        public BBMessageHeader BulletinHeader
        {
            get
            {
                return this.m_BBHeader;
            }
            set
            {
                this.m_BBHeader = value;
            }
        }

        public int Circle
        {
            get
            {
                return this.m_Circle;
            }
            set
            {
                this.m_Circle = value;
            }
        }

        int IPoint2D.X
        {
            get
            {
                return this.m_X;
            }
        }

        int IPoint2D.Y
        {
            get
            {
                return this.m_Y;
            }
        }

        int IPoint3D.Z
        {
            get
            {
                return this.m_Z;
            }
        }

        public IContainer Container
        {
            get
            {
                return this.m_Container;
            }
            set
            {
                this.m_Container = value;
            }
        }

        public short ContainerX
        {
            get
            {
                return this.m_ContainerX;
            }
            set
            {
                this.m_ContainerX = value;
            }
        }

        public short ContainerY
        {
            get
            {
                return this.m_ContainerY;
            }
            set
            {
                this.m_ContainerY = value;
            }
        }

        public ArrayList CorpseEquip
        {
            get
            {
                return this.m_CorpseEquip;
            }
        }

        public int CorpseSerial
        {
            get
            {
                return this.m_CorpseSerial;
            }
            set
            {
                this.m_CorpseSerial = value;
            }
        }

        public byte Direction
        {
            get
            {
                return this.m_Direction;
            }
            set
            {
                this.m_Direction = value;
            }
        }

        public ArrayList Equip
        {
            get
            {
                return this.m_Equip;
            }
        }

        public object EquipParent
        {
            get
            {
                return this.m_EquipParent;
            }
            set
            {
                this.m_EquipParent = value;
            }
        }

        public ItemFlags Flags
        {
            get
            {
                return this.m_Flags;
            }
            set
            {
                this.m_Flags = value;
                if ((this.m_Flags.Value & -161) != 0)
                {
                    string message = string.Format("Unknown item flags: 0x{0:X2}", this.m_Flags.Value);
                    Debug.Trace(message);
                    Engine.AddTextMessage(message);
                }
            }
        }

        public ushort Hue
        {
            get
            {
                return this.m_Hue;
            }
            set
            {
                if (this.m_ShouldOverrideHue)
                {
                    this.m_RealHue = value;
                }
                else
                {
                    this.m_Hue = value;
                }
            }
        }

        public short ID
        {
            get
            {
                return this.m_ID;
            }
            set
            {
                this.m_ID = value;
            }
        }

        public bool InWorld
        {
            get
            {
                return this.m_InWorld;
            }
            set
            {
                this.m_InWorld = value;
            }
        }

        public bool IsBones
        {
            get
            {
                int num = this.m_ID & 0x3fff;
                return ((num >= 0xeca) && (num <= 0xed2));
            }
        }

        public bool IsCorpse
        {
            get
            {
                return ((this.m_ID & 0x3fff) == 0x2006);
            }
        }

        public bool IsDoor
        {
            get
            {
                int num = this.m_ID & 0x3fff;
                return (((Map.m_ItemFlags[num & 0x3fff][TileFlag.Door] || (num == 0x692)) || ((num == 0x846) || (num == 0x873))) || ((num >= 0x6f5) && (num <= 0x6f6)));
            }
        }

        public bool IsEquip
        {
            get
            {
                return this.m_IsEquip;
            }
            set
            {
                this.m_IsEquip = value;
            }
        }

        public bool IsJewelry
        {
            get
            {
                return this.CheckItemID(new int[] { 0x1086, 0x1f06, 0x108a, 0x1f09 });
            }
        }

        public bool IsMulti
        {
            get
            {
                return this.m_IsMulti;
            }
            set
            {
                this.m_IsMulti = value;
            }
        }

        public ArrayList Items
        {
            get
            {
                if (this.m_Sort)
                {
                    this.m_Sort = false;
                    this.m_Items.Sort();
                }
                return this.m_Items;
            }
        }

        public Mobile LastLooked
        {
            get
            {
                return this.m_LastLooked;
            }
            set
            {
                this.m_LastLooked = value;
            }
        }

        public int LastSpell
        {
            get
            {
                return this.m_LastSpell;
            }
            set
            {
                this.m_LastSpell = value;
            }
        }

        public IHue LastTextHue
        {
            get
            {
                return this.m_LastTextHue;
            }
            set
            {
                this.m_LastTextHue = value;
            }
        }

        public int MessageFrame
        {
            get
            {
                return this.m_MessageFrame;
            }
            set
            {
                this.m_MessageFrame = value;
            }
        }

        public int MessageX
        {
            get
            {
                return this.m_MessageX;
            }
            set
            {
                this.m_MessageX = value;
            }
        }

        public int MessageY
        {
            get
            {
                return this.m_MessageY;
            }
            set
            {
                this.m_MessageY = value;
            }
        }

        public Client.Multi Multi
        {
            get
            {
                return this.m_Multi;
            }
            set
            {
                this.m_Multi = value;
            }
        }

        public int OldMapX
        {
            get
            {
                return this.m_OldMapX;
            }
            set
            {
                this.m_OldMapX = value;
            }
        }

        public int OldMapY
        {
            get
            {
                return this.m_OldMapY;
            }
            set
            {
                this.m_OldMapY = value;
            }
        }

        public bool OpenSB
        {
            get
            {
                return this.m_OpenSB;
            }
            set
            {
                this.m_OpenSB = value;
            }
        }

        public Item Parent
        {
            get
            {
                return this.m_Parent;
            }
            set
            {
                this.m_Parent = value;
            }
        }

        public int PropertyID
        {
            get
            {
                return this.m_Props;
            }
            set
            {
                if (this.m_Props != value)
                {
                    this.m_NextQueryProps = DateTime.Now;
                }
                this.m_Props = value;
            }
        }

        public ObjectPropertyList PropertyList
        {
            get
            {
                return ObjectPropertyList.Find(this.m_Serial, this.m_Props);
            }
        }

        public bool QueueOpenSB
        {
            get
            {
                return this.m_QueueOpenSB;
            }
            set
            {
                this.m_QueueOpenSB = value;
            }
        }

        public Client.RestoreInfo RestoreInfo
        {
            get
            {
                return this.m_RestoreInfo;
            }
            set
            {
                this.m_RestoreInfo = value;
            }
        }

        public int Revision
        {
            get
            {
                return this.m_Revision;
            }
            set
            {
                this.m_Revision = value;
            }
        }

        public int Serial
        {
            get
            {
                return this.m_Serial;
            }
        }

        public int SpellbookGraphic
        {
            get
            {
                return this.m_SpellbookGraphic;
            }
            set
            {
                this.m_SpellbookGraphic = value;
            }
        }

        public int SpellbookOffset
        {
            get
            {
                return this.m_SpellbookOffset;
            }
            set
            {
                this.m_SpellbookOffset = value;
            }
        }

        public long SpellContained
        {
            get
            {
                return this.m_SpellContained;
            }
            set
            {
                this.m_SpellContained = value;
            }
        }

        public bool TradeCheck1
        {
            get
            {
                return this.m_TradeCheck1;
            }
            set
            {
                this.m_TradeCheck1 = value;
            }
        }

        public bool TradeCheck2
        {
            get
            {
                return this.m_TradeCheck2;
            }
            set
            {
                this.m_TradeCheck2 = value;
            }
        }

        public bool Visible
        {
            get
            {
                return this.m_Visible;
            }
            set
            {
                if (this.m_Visible != value)
                {
                    this.m_Visible = value;
                    if (this.m_InWorld && this.m_IsMulti)
                    {
                        Map.Invalidate();
                        GRadar.Invalidate();
                    }
                }
            }
        }

        public short X
        {
            get
            {
                return this.m_X;
            }
            set
            {
                if (this.m_X != value)
                {
                    this.m_X = value;
                    if (this.m_InWorld)
                    {
                        if (this.m_IsMulti)
                        {
                            Engine.Multis.Sort();
                            Map.Invalidate();
                            GRadar.Invalidate();
                        }
                        else if (!World.InUpdateRange(this))
                        {
                            World.Remove(this);
                        }
                    }
                }
            }
        }

        public short Y
        {
            get
            {
                return this.m_Y;
            }
            set
            {
                if (this.m_Y != value)
                {
                    this.m_Y = value;
                    if (this.m_InWorld)
                    {
                        if (this.m_IsMulti)
                        {
                            Engine.Multis.Sort();
                            Map.Invalidate();
                            GRadar.Invalidate();
                        }
                        else if (!World.InUpdateRange(this))
                        {
                            World.Remove(this);
                        }
                    }
                }
            }
        }

        public short Z
        {
            get
            {
                return this.m_Z;
            }
            set
            {
                if (this.m_Z != value)
                {
                    this.m_Z = value;
                    if (this.m_InWorld && this.m_IsMulti)
                    {
                        Map.Invalidate();
                        GRadar.Invalidate();
                    }
                }
            }
        }
    }
}

