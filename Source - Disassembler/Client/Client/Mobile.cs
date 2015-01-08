namespace Client
{
    using System;
    using System.Collections;
    using System.IO;

    public class Mobile : IPoint3D, IEntity, IMessageOwner, IPoint2D, IAnimationOwner
    {
        public string lastSpell;
        private Client.Animation m_Animation;
        private short m_Armor;
        private bool m_BigStatus;
        private short m_Body;
        private bool m_Bonded;
        private int m_ColdResist;
        private int m_CorpseSerial;
        private int m_DamageMax;
        private int m_DamageMin;
        private int m_Dex;
        private byte m_Direction;
        private int m_EnergyResist;
        private ArrayList m_Equip = new ArrayList(0);
        private Frames m_fAnimationPool;
        private int m_FireResist;
        private MobileFlags m_Flags;
        private int m_FollowersCur;
        private int m_FollowersMax = 5;
        private byte m_Gender;
        private bool m_Ghost;
        private int m_Gold;
        private IHue m_hAnimationPool;
        private int m_HorseFootsteps;
        private int m_HPCur;
        private int m_HPMax;
        private short m_Hue;
        private bool m_Human;
        private bool m_HumanOrGhost;
        private int m_iAnimationPool;
        private bool m_Ignored;
        private int m_Int;
        public bool m_IsFactionGuard;
        public bool m_IsFriend;
        private bool m_IsKUOC;
        private bool m_IsMoving;
        private bool m_IsPet;
        public int m_KUOC_F;
        public int m_KUOC_X;
        public int m_KUOC_Y;
        public int m_KUOC_Z;
        private int m_LastFrame = -12345;
        private int m_LastWalk;
        private int m_LightLevel;
        private int m_Luck;
        private int m_ManaCur;
        private int m_ManaMax;
        private int m_MessageFrame;
        private int m_MessageX;
        private int m_MessageY;
        private int m_MovedTiles;
        private string m_Name = "";
        private DateTime m_NextQueryProps;
        private Client.Notoriety m_Notoriety;
        private static string[] m_NotorietyStrings = new string[] { "You are now innocent.", "You are now an ally.", "You may now be attacked freely, but are not a criminal.", "You are now a criminal.", "You are now an enemy.", "You are now a murderer.", "You are now invulnerable." };
        private int m_OldMapX;
        private int m_OldMapY;
        private bool m_OpenedStatus;
        private Gump m_Paperdoll;
        private bool m_PaperdollCanDrag;
        private string m_PaperdollName = "";
        private int m_PaperdollX = 0x7fffffff;
        private int m_PaperdollY = 0x7fffffff;
        private bool m_Player;
        private int m_PoisonResist;
        private int m_Props;
        private bool m_Refresh;
        private int m_ScreenX;
        private int m_ScreenY;
        private bool m_SentKUOC;
        private int m_Serial;
        private int m_Sounds;
        private int m_StamCur;
        private int m_StamMax;
        private int m_StatCap = 0xe1;
        private IMobileStatus m_StatusBar;
        private int m_Str;
        private int m_TithingPoints;
        private bool m_Traced = false;
        private AnimationVertexCache m_vCache;
        private bool m_Visible;
        private Queue m_Walking;
        private int m_Weight;
        private short m_X;
        private short m_XReal;
        private short m_Y;
        private short m_YReal;
        private short m_Z;
        private short m_ZReal;

        public Mobile(int serial)
        {
            this.m_Flags = new MobileFlags(this);
            this.m_Walking = new Queue(0);
            this.m_Serial = serial;
        }

        public void AddEquip(EquipEntry e)
        {
            if (LayerComparer.FromDirection(this.m_Direction).IsValid(e.m_Layer))
            {
                int count = this.m_Equip.Count;
                int index = 0;
                while (index < count)
                {
                    EquipEntry entry = (EquipEntry) this.m_Equip[index];
                    if (entry.m_Layer == e.m_Layer)
                    {
                        this.m_Equip.RemoveAt(index);
                        count--;
                    }
                    else
                    {
                        index++;
                    }
                }
                this.m_Equip.Add(e);
                this.EquipChanged();
            }
        }

        public void AddTextMessage(string Name, string Message, IFont Font, IHue Hue, bool unremovable)
        {
            if (!this.m_Ignored && (!Message.StartsWith("[Party]->[") || (Message.IndexOf("KUOC_") < 0)))
            {
                if ((Font is Font) && Message.StartsWith("(Guard, "))
                {
                    this.m_IsFactionGuard = true;
                }
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
        }

        public bool Attack()
        {
            return Network.Send(new PAttackRequest(this));
        }

        public void AutoTrace()
        {
            if (!this.m_Traced)
            {
                this.m_Traced = true;
            }
        }

        public bool CheckGuarded()
        {
            MapPackage cache = Map.GetCache();
            int num = this.m_X - cache.CellX;
            int num2 = this.m_Y - cache.CellY;
            return ((((num >= 0) && (num < Renderer.cellWidth)) && ((num2 >= 0) && (num2 < Renderer.cellHeight))) && cache.landTiles[num, num2].m_Guarded);
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

        public double DistanceSqrt(IPoint2D p)
        {
            int num = this.m_X - p.X;
            int num2 = this.m_Y - p.Y;
            return Math.Sqrt((double) ((num * num) + (num2 * num2)));
        }

        public int DistanceTo(int xTile, int yTile)
        {
            int num = this.m_X - xTile;
            int num2 = this.m_Y - yTile;
            return (int) Math.Sqrt((double) ((num * num) + (num2 * num2)));
        }

        public void Draw(Texture t, int x, int y)
        {
            if (this.m_vCache == null)
            {
                this.m_vCache = new AnimationVertexCache();
            }
            this.m_vCache.Draw(t, x, y);
        }

        public void DrawGame(Texture t, int x, int y)
        {
            if (this.m_vCache == null)
            {
                this.m_vCache = new AnimationVertexCache();
            }
            this.m_vCache.DrawGame(t, x, y);
        }

        public void EquipChanged()
        {
            GCombatGump.Update();
            this.m_Equip.Sort(LayerComparer.FromDirection(this.m_Direction));
            if (this.m_Paperdoll != null)
            {
                Gumps.OpenPaperdoll(this, this.m_PaperdollName, this.m_PaperdollCanDrag);
            }
        }

        public void EquipRemoved()
        {
            GCombatGump.Update();
            if (this.m_Paperdoll != null)
            {
                Gumps.OpenPaperdoll(this, this.m_PaperdollName, this.m_PaperdollCanDrag);
            }
        }

        public Item FindEquip(IItemValidator check)
        {
            ArrayList equip = this.m_Equip;
            int count = equip.Count;
            for (int i = 0; i < count; i++)
            {
                EquipEntry entry = (EquipEntry) equip[i];
                if (check.IsValid(entry.m_Item))
                {
                    return entry.m_Item;
                }
            }
            return null;
        }

        public Item FindEquip(Layer layer)
        {
            ArrayList equip = this.m_Equip;
            int count = equip.Count;
            for (int i = 0; i < count; i++)
            {
                EquipEntry entry = (EquipEntry) equip[i];
                if (entry.m_Layer == layer)
                {
                    return entry.m_Item;
                }
            }
            return null;
        }

        public bool HasEquip(Item check)
        {
            ArrayList equip = this.m_Equip;
            int count = equip.Count;
            for (int i = 0; i < count; i++)
            {
                if (((EquipEntry) equip[i]).m_Item == check)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasEquipOnLayer(Layer check)
        {
            ArrayList equip = this.m_Equip;
            int count = equip.Count;
            for (int i = 0; i < count; i++)
            {
                if (((EquipEntry) equip[i]).m_Layer == check)
                {
                    return true;
                }
            }
            return false;
        }

        public bool InSquareRange(IPoint2D p, int xyRange)
        {
            return ((((this.m_X >= (p.X - xyRange)) && (this.m_X <= (p.X + xyRange))) && (this.m_Y >= (p.Y - xyRange))) && (this.m_Y <= (p.Y + xyRange)));
        }

        public bool InSquareRange(int xTile, int yTile, int xyRange)
        {
            return ((((this.m_X >= (xTile - xyRange)) && (this.m_X <= (xTile + xyRange))) && (this.m_Y >= (yTile - xyRange))) && (this.m_Y <= (yTile + xyRange)));
        }

        private void InternalOnFlagsChanged(MobileFlags flags)
        {
            if (!this.m_Refresh && (this.m_StatusBar != null))
            {
                this.m_StatusBar.OnFlagsChange(flags);
            }
            this.m_Flags = flags;
            if ((this.m_Flags.Value & -223) != 0)
            {
                this.Trace();
                string message = string.Format("Unknown mobile flags: 0x{0:X2}", this.m_Flags.Value);
                Debug.Error(message);
                Engine.AddTextMessage(message);
            }
            if (this.m_Player && ((this.m_Flags.Value & 0x80) == 0))
            {
                Engine.m_Stealth = false;
                Engine.m_StealthSteps = 0;
            }
        }

        public bool Look()
        {
            return Network.Send(new PLookRequest(this));
        }

        public void OnDoubleClick()
        {
            Mobile player = World.Player;
            if (player != null)
            {
                if ((player.Flags[MobileFlag.Warmode] && !player.Ghost) && (!this.Ghost && !this.Player))
                {
                    if ((this.m_Notoriety == Client.Notoriety.Innocent) && ((World.CharData.NotoQuery == NotoQueryType.On) || ((World.CharData.NotoQuery == NotoQueryType.Smart) && (this.CheckGuarded() || player.CheckGuarded()))))
                    {
                        Gumps.Desktop.Children.Add(new GCriminalAttackQuery(this));
                    }
                    else
                    {
                        this.Attack();
                    }
                }
                else
                {
                    this.Use();
                    PUseRequest.Last = this;
                }
            }
        }

        internal void OnFlagsChanged()
        {
            this.InternalOnFlagsChanged(this.m_Flags);
        }

        public void OnSingleClick()
        {
            this.Look();
        }

        public void OnTarget()
        {
            Engine.Target(this);
        }

        public void OpenStatus(bool Drag)
        {
            int x = 0;
            int y = 0;
            bool flag = this.m_StatusBar != null;
            bool flag2 = flag && (Gumps.Drag == this.m_StatusBar.Gump);
            bool flag3 = flag && (Gumps.StartDrag == this.m_StatusBar.Gump);
            int num3 = flag ? this.m_StatusBar.Gump.m_OffsetX : 0;
            int num4 = flag ? this.m_StatusBar.Gump.m_OffsetY : 0;
            if (flag)
            {
                x = this.m_StatusBar.Gump.X;
                y = this.m_StatusBar.Gump.Y;
                this.m_StatusBar.Close();
            }
            if (this.m_BigStatus)
            {
                this.m_StatusBar = new GStatusBar(this, x, y);
            }
            else if ((Party.State == PartyState.Joined) && (Array.IndexOf(Party.Members, this) >= 0))
            {
                this.m_StatusBar = new GPartyHealthBar(this, x, y);
            }
            else
            {
                this.m_StatusBar = new GHealthBar(this, x, y);
            }
            if (!flag || Drag)
            {
                this.m_StatusBar.Gump.X = Engine.m_xMouse - (this.m_StatusBar.Gump.Width / 2);
                this.m_StatusBar.Gump.Y = Engine.m_yMouse - (this.m_StatusBar.Gump.Height / 2);
            }
            if (flag2 || Drag)
            {
                if (Drag)
                {
                    this.m_StatusBar.Gump.m_OffsetX = this.m_StatusBar.Gump.Width / 2;
                    this.m_StatusBar.Gump.m_OffsetY = this.m_StatusBar.Gump.Height / 2;
                }
                else
                {
                    this.m_StatusBar.Gump.m_OffsetX = num3;
                    this.m_StatusBar.Gump.m_OffsetY = num4;
                }
                this.m_StatusBar.Gump.m_IsDragging = true;
                Gumps.Drag = this.m_StatusBar.Gump;
            }
            else if (flag3)
            {
                this.m_StatusBar.Gump.m_OffsetX = num3;
                this.m_StatusBar.Gump.m_OffsetY = num4;
                Gumps.StartDrag = this.m_StatusBar.Gump;
            }
            Gumps.Desktop.Children.Add(this.m_StatusBar.Gump);
            this.m_OpenedStatus = true;
        }

        public void QueryProperties()
        {
            if (DateTime.Now >= this.m_NextQueryProps)
            {
                this.m_NextQueryProps = DateTime.Now + TimeSpan.FromSeconds(1.0);
                Network.Send(new PQueryProperties(this.m_Serial));
            }
        }

        public bool QueryStats()
        {
            this.m_OpenedStatus = true;
            return Network.Send(new PQueryStats(this.m_Serial));
        }

        private void RecurseTrace(StreamWriter sw, string indent, Item item)
        {
            sw.WriteLine("{0}Item 0x{1:X}", indent, item.Serial);
            sw.WriteLine("{0}{{", indent);
            indent = indent + "   ";
            sw.WriteLine("{0}Index: {1} (0x{1:X})", indent, item.ID & 0x3fff);
            sw.WriteLine("{0}  Hue: {1} (0x{1:X})", indent, item.Hue);
            sw.WriteLine("{0} Amnt: {1}", indent, item.Amount);
            if (item.Items.Count > 0)
            {
                sw.WriteLine();
                sw.WriteLine("{0}{1} Items:", indent, item.Items.Count);
                sw.WriteLine("{0}{{", indent);
                indent = indent + "   ";
                for (int i = 0; i < item.Items.Count; i++)
                {
                    this.RecurseTrace(sw, indent, (Item) item.Items[i]);
                }
                indent = indent.Substring(0, indent.Length - 3);
                sw.WriteLine("{0}}}", indent);
            }
            indent = indent.Substring(0, indent.Length - 3);
            sw.WriteLine("{0}}}", indent);
        }

        public void SetLocation(short x, short y, short z)
        {
            this.m_Z = z;
            if ((this.m_X != x) || (this.m_Y != y))
            {
                if (this.m_Player && (Engine.Multis.IsInMulti(this.m_X, this.m_Y, this.m_Z) != Engine.Multis.IsInMulti(x, y, z)))
                {
                    Map.Invalidate();
                }
                this.m_X = x;
                this.m_Y = y;
                this.m_KUOC_X = x;
                this.m_KUOC_Y = y;
                this.m_KUOC_Z = z;
                if (!this.m_Player)
                {
                    if (this.m_Visible && !World.InUpdateRange(this))
                    {
                        World.Remove(this);
                    }
                    else if (((this.m_Name != null) && (this.m_Name.Length > 0)) && (Array.IndexOf(Party.Members, this) >= 0))
                    {
                        GRadar.AddTag(this.m_X, this.m_Y, this.m_Name, this.m_Serial);
                    }
                }
            }
        }

        public void SetReal(int x, int y, int z)
        {
            this.m_XReal = (short) x;
            this.m_YReal = (short) y;
            this.m_ZReal = (short) z;
        }

        private void StatChange(string name, int oldValue, int newValue)
        {
            int num = newValue - oldValue;
            if (num != 0)
            {
                Engine.AddTextMessage(string.Format("Your {0} has {1} by {2}. It is now {3}.", new object[] { name, (num > 0) ? "increased" : "decreased", Math.Abs(num), newValue }), Engine.GetFont(3), Hues.Load(0x170));
            }
        }

        public void Trace()
        {
            Debug.Trace("Serial: 0x{0:X8}", this.m_Serial);
            Debug.Trace("Body: 0x{0:X}", this.m_Body);
            Debug.Trace("Location: ({0}, {1}, {2}):0x{3:X}", new object[] { this.m_X, this.m_Y, this.m_Z, this.m_Direction });
            Debug.Trace("Hue: 0x{0:X}", this.m_Hue);
            Debug.Trace("Flags: {0}", this.m_Flags);
            Debug.Trace("Visible: {0}", this.m_Visible);
            Debug.Trace("Armor: {0}", this.m_Armor);
            Debug.Trace("Gender: {0}", this.m_Gender);
            Debug.Trace("Gold: {0}", this.m_Gold);
            Debug.Trace("Name: {0}", this.m_Name);
            Debug.Trace("Notoriety: {0}", this.m_Notoriety);
            Debug.Trace("Player: {0}", this.m_Player);
            Debug.Trace("Weight: {0}", this.m_Weight);
            Debug.Trace("IsMoving: {0}", this.m_IsMoving);
            Debug.Trace("WalkCount: {0}", this.m_Walking.Count);
            Debug.Trace("Animated: {0}", (this.m_Animation == null) ? ((object) 0) : ((object) this.m_Animation.Running));
            foreach (EquipEntry entry in this.m_Equip)
            {
                Debug.Trace("\tAnim: {0}", entry.m_Animation);
                Debug.Trace("\tLayer: {0}", entry.m_Layer);
                Debug.Trace("\tItem:");
                entry.m_Item.Trace();
            }
        }

        public void Update()
        {
            Map.UpdateMobile(this);
        }

        public void UpdateReal()
        {
            this.SetReal(this.m_X, this.m_Y, this.m_Z);
        }

        public bool Use()
        {
            return Network.Send(new PUseRequest(this));
        }

        public bool UsingTwoHandedWeapon()
        {
            ArrayList equip = this.m_Equip;
            int count = equip.Count;
            for (int i = 0; i < count; i++)
            {
                EquipEntry entry = (EquipEntry) equip[i];
                if ((entry.m_Layer == Layer.TwoHanded) && Map.m_ItemFlags[entry.m_Item.ID & 0x3fff][TileFlag.Unknown3])
                {
                    int num3 = entry.m_Item.ID & 0x3fff;
                    if (((num3 < 0x1b72) || (num3 > 0x1b7b)) || ((num3 < 0x1bc3) || (num3 > 0x1bc7)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public Client.Animation Animation
        {
            get
            {
                return this.m_Animation;
            }
            set
            {
                this.m_Animation = value;
            }
        }

        public int Armor
        {
            get
            {
                return this.m_Armor;
            }
            set
            {
                if ((!this.m_Refresh && (this.m_StatusBar != null)) && (this.m_Armor != ((short) value)))
                {
                    this.m_StatusBar.OnArmorChange((short) value);
                }
                this.m_Armor = (short) value;
            }
        }

        public Item Backpack
        {
            get
            {
                return this.FindEquip(Layer.Backpack);
            }
        }

        public bool BigStatus
        {
            get
            {
                return this.m_BigStatus;
            }
            set
            {
                this.m_BigStatus = value;
            }
        }

        public short Body
        {
            get
            {
                return this.m_Body;
            }
            set
            {
                if (this.m_Body != value)
                {
                    this.m_Body = value;
                    int body = this.m_Body;
                    Engine.m_Animations.Translate(ref body);
                    this.m_Human = (((body == 400) || (body == 0x191)) || ((body == 0x3df) || (body == 0x3db))) || (body == 990);
                    this.m_Ghost = (body == 0x192) || (body == 0x193);
                    this.m_HumanOrGhost = this.m_Human || this.m_Ghost;
                }
            }
        }

        public bool Bonded
        {
            get
            {
                return this.m_Bonded;
            }
            set
            {
                this.m_Bonded = value;
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

        public int ColdResist
        {
            get
            {
                return this.m_ColdResist;
            }
            set
            {
                this.m_ColdResist = value;
                if (!this.m_Refresh && (this.m_StatusBar != null))
                {
                    this.m_StatusBar.OnColdChange();
                }
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

        public int DamageMax
        {
            get
            {
                return this.m_DamageMax;
            }
            set
            {
                this.m_DamageMax = value;
                if (!this.m_Refresh && (this.m_StatusBar != null))
                {
                    this.m_StatusBar.OnDamageChange();
                }
            }
        }

        public int DamageMin
        {
            get
            {
                return this.m_DamageMin;
            }
            set
            {
                this.m_DamageMin = value;
                if (!this.m_Refresh && (this.m_StatusBar != null))
                {
                    this.m_StatusBar.OnDamageChange();
                }
            }
        }

        public int Dex
        {
            get
            {
                return this.m_Dex;
            }
            set
            {
                if (this.m_Dex != value)
                {
                    if (!this.m_Refresh && (this.m_StatusBar != null))
                    {
                        this.m_StatusBar.OnDexChange(value);
                    }
                    if ((this.m_Player && (this.m_Dex != 0)) && Engine.m_Ingame)
                    {
                        this.StatChange("dexterity", this.m_Dex, value);
                    }
                    this.m_Dex = value;
                }
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

        public int EnergyResist
        {
            get
            {
                return this.m_EnergyResist;
            }
            set
            {
                this.m_EnergyResist = value;
                if (!this.m_Refresh && (this.m_StatusBar != null))
                {
                    this.m_StatusBar.OnEnergyChange();
                }
            }
        }

        public ArrayList Equip
        {
            get
            {
                return this.m_Equip;
            }
            set
            {
                this.m_Equip = value;
            }
        }

        public int FireResist
        {
            get
            {
                return this.m_FireResist;
            }
            set
            {
                this.m_FireResist = value;
                if (!this.m_Refresh && (this.m_StatusBar != null))
                {
                    this.m_StatusBar.OnFireChange();
                }
            }
        }

        public MobileFlags Flags
        {
            get
            {
                return this.m_Flags;
            }
            set
            {
                this.InternalOnFlagsChanged(value);
            }
        }

        public int FollowersCur
        {
            get
            {
                return this.m_FollowersCur;
            }
            set
            {
                if (this.m_FollowersCur != value)
                {
                    if (!this.m_Refresh && (this.m_StatusBar != null))
                    {
                        this.m_StatusBar.OnFollCurChange(value);
                    }
                    this.m_FollowersCur = value;
                }
            }
        }

        public int FollowersMax
        {
            get
            {
                return this.m_FollowersMax;
            }
            set
            {
                if (this.m_FollowersMax != value)
                {
                    if (!this.m_Refresh && (this.m_StatusBar != null))
                    {
                        this.m_StatusBar.OnFollMaxChange(value);
                    }
                    this.m_FollowersMax = value;
                }
            }
        }

        public byte Gender
        {
            get
            {
                return this.m_Gender;
            }
            set
            {
                if ((!this.m_Refresh && (this.m_StatusBar != null)) && (this.m_Gender != value))
                {
                    this.m_StatusBar.OnGenderChange(value);
                }
                this.m_Gender = value;
            }
        }

        public bool Ghost
        {
            get
            {
                return (this.m_Ghost || (this.m_Player && Renderer.m_DeathOverride));
            }
        }

        public int Gold
        {
            get
            {
                return this.m_Gold;
            }
            set
            {
                if ((!this.m_Refresh && (this.m_StatusBar != null)) && (this.m_Gold != value))
                {
                    this.m_StatusBar.OnGoldChange(value);
                }
                this.m_Gold = value;
            }
        }

        public int HorseFootsteps
        {
            get
            {
                return this.m_HorseFootsteps;
            }
            set
            {
                this.m_HorseFootsteps = value;
            }
        }

        public int HPCur
        {
            get
            {
                return this.m_HPCur;
            }
            set
            {
                if (this.m_HPCur != value)
                {
                    if (!this.m_Refresh && (this.m_StatusBar != null))
                    {
                        this.m_StatusBar.OnHPCurChange(value);
                    }
                    this.m_HPCur = value;
                }
            }
        }

        public int HPMax
        {
            get
            {
                return this.m_HPMax;
            }
            set
            {
                if (this.m_HPMax != value)
                {
                    if (!this.m_Refresh && (this.m_StatusBar != null))
                    {
                        this.m_StatusBar.OnHPMaxChange(value);
                    }
                    this.m_HPMax = value;
                }
            }
        }

        public short Hue
        {
            get
            {
                return this.m_Hue;
            }
            set
            {
                this.m_Hue = value;
            }
        }

        public bool Human
        {
            get
            {
                return this.m_Human;
            }
        }

        public bool HumanOrGhost
        {
            get
            {
                return (this.m_HumanOrGhost || (this.m_Player && Renderer.m_DeathOverride));
            }
        }

        public bool Ignored
        {
            get
            {
                return this.m_Ignored;
            }
            set
            {
                this.m_Ignored = value;
            }
        }

        public int Int
        {
            get
            {
                return this.m_Int;
            }
            set
            {
                if (this.m_Int != value)
                {
                    if (!this.m_Refresh && (this.m_StatusBar != null))
                    {
                        this.m_StatusBar.OnIntChange(value);
                    }
                    if (this.m_Player && (this.m_Int != 0))
                    {
                        this.StatChange("intelligence", this.m_Int, value);
                    }
                    this.m_Int = value;
                }
            }
        }

        public bool IsKUOC
        {
            get
            {
                return this.m_IsKUOC;
            }
            set
            {
                this.m_IsKUOC = value;
            }
        }

        public bool IsMoving
        {
            get
            {
                return this.m_IsMoving;
            }
            set
            {
                this.m_IsMoving = value;
            }
        }

        public bool IsPet
        {
            get
            {
                return this.m_IsPet;
            }
            set
            {
                this.m_IsPet = value;
            }
        }

        public int LastFrame
        {
            get
            {
                return this.m_LastFrame;
            }
            set
            {
                this.m_LastFrame = value;
            }
        }

        public int LastWalk
        {
            get
            {
                return this.m_LastWalk;
            }
            set
            {
                this.m_LastWalk = value;
            }
        }

        public int LightLevel
        {
            get
            {
                return this.m_LightLevel;
            }
            set
            {
                this.m_LightLevel = value;
            }
        }

        public int Luck
        {
            get
            {
                return this.m_Luck;
            }
            set
            {
                this.m_Luck = value;
                if (!this.m_Refresh && (this.m_StatusBar != null))
                {
                    this.m_StatusBar.OnLuckChange();
                }
            }
        }

        public int ManaCur
        {
            get
            {
                return this.m_ManaCur;
            }
            set
            {
                if (this.m_ManaCur != value)
                {
                    if (!this.m_Refresh && (this.m_StatusBar != null))
                    {
                        this.m_StatusBar.OnManaCurChange(value);
                    }
                    this.m_ManaCur = value;
                }
            }
        }

        public int ManaMax
        {
            get
            {
                return this.m_ManaMax;
            }
            set
            {
                if (this.m_ManaMax != value)
                {
                    if (!this.m_Refresh && (this.m_StatusBar != null))
                    {
                        this.m_StatusBar.OnManaMaxChange(value);
                    }
                    this.m_ManaMax = value;
                }
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

        public int MovedTiles
        {
            get
            {
                return this.m_MovedTiles;
            }
            set
            {
                this.m_MovedTiles = value;
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
            set
            {
                if (this.m_Name != value)
                {
                    if (!this.m_Refresh && (this.m_StatusBar != null))
                    {
                        this.m_StatusBar.OnNameChange(value);
                    }
                    this.m_Name = value;
                    if (this.m_Player)
                    {
                        string str2;
                        string name = this.m_Name;
                        if ((name == null) || ((name = name.Trim()).Length <= 0))
                        {
                            str2 = "Ultima Online";
                        }
                        else
                        {
                            str2 = "Ultima Online - " + name;
                            World.CharData.Name = value;
                        }
                        Engine.m_Display.Text = str2;
                    }
                }
            }
        }

        public Client.Notoriety Notoriety
        {
            get
            {
                return this.m_Notoriety;
            }
            set
            {
                if (this.m_Notoriety != value)
                {
                    this.m_Notoriety = value;
                    if (!this.m_Refresh && (this.m_StatusBar != null))
                    {
                        this.m_StatusBar.OnNotorietyChange(value);
                    }
                    if ((this.m_Player && Engine.m_Ingame) && ((this.m_Notoriety >= Client.Notoriety.Innocent) && (this.m_Notoriety <= Client.Notoriety.Vendor)))
                    {
                        Engine.AddTextMessage(m_NotorietyStrings[((int) this.m_Notoriety) - 1], Engine.DefaultFont, Hues.GetNotoriety(this.m_Notoriety));
                    }
                }
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

        public bool OpenedStatus
        {
            get
            {
                return this.m_OpenedStatus;
            }
            set
            {
                this.m_OpenedStatus = value;
            }
        }

        public Gump Paperdoll
        {
            get
            {
                return this.m_Paperdoll;
            }
            set
            {
                this.m_Paperdoll = value;
            }
        }

        public bool PaperdollCanDrag
        {
            get
            {
                return this.m_PaperdollCanDrag;
            }
            set
            {
                this.m_PaperdollCanDrag = value;
            }
        }

        public string PaperdollName
        {
            get
            {
                return this.m_PaperdollName;
            }
            set
            {
                this.m_PaperdollName = value;
            }
        }

        public int PaperdollX
        {
            get
            {
                return this.m_PaperdollX;
            }
            set
            {
                this.m_PaperdollX = value;
            }
        }

        public int PaperdollY
        {
            get
            {
                return this.m_PaperdollY;
            }
            set
            {
                this.m_PaperdollY = value;
            }
        }

        public bool Player
        {
            get
            {
                return this.m_Player;
            }
            set
            {
                this.m_Player = value;
            }
        }

        public int PoisonResist
        {
            get
            {
                return this.m_PoisonResist;
            }
            set
            {
                this.m_PoisonResist = value;
                if (!this.m_Refresh && (this.m_StatusBar != null))
                {
                    this.m_StatusBar.OnPoisonChange();
                }
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

        public bool Refresh
        {
            get
            {
                return this.m_Refresh;
            }
            set
            {
                if ((this.m_Refresh && !value) && (this.m_StatusBar != null))
                {
                    this.m_StatusBar.OnRefresh();
                }
                this.m_Refresh = value;
            }
        }

        public int ScreenX
        {
            get
            {
                return this.m_ScreenX;
            }
            set
            {
                this.m_ScreenX = value;
            }
        }

        public int ScreenY
        {
            get
            {
                return this.m_ScreenY;
            }
            set
            {
                this.m_ScreenY = value;
            }
        }

        public bool SentKUOC
        {
            get
            {
                return this.m_SentKUOC;
            }
            set
            {
                this.m_SentKUOC = value;
            }
        }

        public int Serial
        {
            get
            {
                return this.m_Serial;
            }
        }

        public int Sounds
        {
            get
            {
                return this.m_Sounds;
            }
            set
            {
                this.m_Sounds = value;
            }
        }

        public int StamCur
        {
            get
            {
                return this.m_StamCur;
            }
            set
            {
                if (this.m_StamCur != value)
                {
                    if (!this.m_Refresh && (this.m_StatusBar != null))
                    {
                        this.m_StatusBar.OnStamCurChange(value);
                    }
                    this.m_StamCur = value;
                }
            }
        }

        public int StamMax
        {
            get
            {
                return this.m_StamMax;
            }
            set
            {
                if (this.m_StamMax != value)
                {
                    if (!this.m_Refresh && (this.m_StatusBar != null))
                    {
                        this.m_StatusBar.OnStamMaxChange(value);
                    }
                    this.m_StamMax = value;
                }
            }
        }

        public int StatCap
        {
            get
            {
                return this.m_StatCap;
            }
            set
            {
                if (this.m_StatCap != value)
                {
                    if (!this.m_Refresh && (this.m_StatusBar != null))
                    {
                        this.m_StatusBar.OnStatCapChange(value);
                    }
                    this.m_StatCap = value;
                }
            }
        }

        public IMobileStatus StatusBar
        {
            get
            {
                return this.m_StatusBar;
            }
            set
            {
                this.m_StatusBar = value;
            }
        }

        public int Str
        {
            get
            {
                return this.m_Str;
            }
            set
            {
                if (this.m_Str != value)
                {
                    if (!this.m_Refresh && (this.m_StatusBar != null))
                    {
                        this.m_StatusBar.OnStrChange(value);
                    }
                    if ((this.m_Player && (this.m_Str != 0)) && Engine.m_Ingame)
                    {
                        this.StatChange("strength", this.m_Str, value);
                    }
                    this.m_Str = value;
                }
            }
        }

        public int TithingPoints
        {
            get
            {
                return this.m_TithingPoints;
            }
            set
            {
                this.m_TithingPoints = value;
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
                    if (this.m_StatusBar is GPartyHealthBar)
                    {
                        this.m_StatusBar.OnFlagsChange(this.m_Flags);
                        this.QueryStats();
                    }
                }
            }
        }

        public Queue Walking
        {
            get
            {
                return this.m_Walking;
            }
        }

        public bool Warmode
        {
            get
            {
                return (this.m_Flags[MobileFlag.Warmode] && !this.Ghost);
            }
        }

        public int Weight
        {
            get
            {
                return this.m_Weight;
            }
            set
            {
                if ((!this.m_Refresh && (this.m_StatusBar != null)) && (this.m_Weight != value))
                {
                    this.m_StatusBar.OnWeightChange(value);
                }
                this.m_Weight = value;
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
                    this.m_KUOC_X = value;
                    if (!this.m_Player)
                    {
                        if (this.m_Visible && !World.InUpdateRange(this))
                        {
                            World.Remove(this);
                        }
                        else if (((this.m_Name != null) && (this.m_Name.Length > 0)) && (Array.IndexOf(Party.Members, this) >= 0))
                        {
                            GRadar.AddTag(this.m_X, this.m_Y, this.m_Name, this.m_Serial);
                        }
                    }
                }
            }
        }

        public short XReal
        {
            get
            {
                return this.m_XReal;
            }
            set
            {
                this.m_XReal = value;
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
                    this.m_KUOC_Y = value;
                    if (!this.m_Player)
                    {
                        if (this.m_Visible && !World.InUpdateRange(this))
                        {
                            World.Remove(this);
                        }
                        else if (((this.m_Name != null) && (this.m_Name.Length > 0)) && (Array.IndexOf(Party.Members, this) >= 0))
                        {
                            GRadar.AddTag(this.m_X, this.m_Y, this.m_Name, this.m_Serial);
                        }
                    }
                }
            }
        }

        public short YReal
        {
            get
            {
                return this.m_YReal;
            }
            set
            {
                this.m_YReal = value;
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
                this.m_Z = value;
                this.m_KUOC_Z = value;
            }
        }

        public short ZReal
        {
            get
            {
                return this.m_ZReal;
            }
            set
            {
                this.m_ZReal = value;
            }
        }
    }
}

