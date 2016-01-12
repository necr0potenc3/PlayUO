namespace Client
{
    using System.Collections;

    public class World
    {
        private static Client.CharData m_CharData;
        private static Hashtable m_Items = new Hashtable();
        private static ArrayList m_Messages = new ArrayList();
        private static Hashtable m_Mobiles = new Hashtable();
        private static Mobile m_Player;
        private static int m_Range = 0x12;
        private static int m_Serial;
        private static int m_X;
        private static int m_Y;
        private static int m_Z;

        public static void AddStaticMessage(int Serial, string Message)
        {
            int count = m_Messages.Count;
            for (int i = 0; i < count; i++)
            {
                StaticMessage message = (StaticMessage)m_Messages[i];
                if (message.Serial == Serial)
                {
                    return;
                }
            }
            m_Messages.Add(new StaticMessage(Engine.m_xClick - Engine.GameX, Engine.m_yClick - Engine.GameY, Serial, Message));
        }

        public static void Clear()
        {
            m_Serial = 0;
            m_Player = null;
            if (m_Mobiles != null)
            {
                m_Mobiles.Clear();
            }
            if (m_Items != null)
            {
                m_Items.Clear();
            }
            if (m_Messages != null)
            {
                m_Messages.Clear();
            }
            Engine.Multis.Clear();
            Engine.m_Display.Text = "Ultima Online";
        }

        public static void DrawAllMessages()
        {
            int count = m_Messages.Count;
            if (count != 0)
            {
                int index = 0;
                while (index < count)
                {
                    StaticMessage message = (StaticMessage)m_Messages[index];
                    if (message.Alpha <= 0f)
                    {
                        m_Messages.RemoveAt(index);
                        count--;
                    }
                    else
                    {
                        if (message.Elapsed && !message.Disposing)
                        {
                            message.Dispose();
                        }
                        Renderer.m_TextToDraw.Add(message);
                        index++;
                    }
                }
            }
        }

        public static Item FindItem(params IItemValidator[] check)
        {
            IEnumerator enumerator = m_Items.Values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Item current = (Item)enumerator.Current;
                if ((current.Visible && current.InWorld) && (!current.IsMulti && InRange(current)))
                {
                    for (int i = 0; i < check.Length; i++)
                    {
                        if (check[i].IsValid(current))
                        {
                            return current;
                        }
                    }
                }
            }
            return null;
        }

        public static Item FindItem(IItemValidator check)
        {
            IEnumerator enumerator = m_Items.Values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Item current = (Item)enumerator.Current;
                if (((current.Visible && current.InWorld) && (!current.IsMulti && InRange(current))) && check.IsValid(current))
                {
                    return current;
                }
            }
            return null;
        }

        public static Item FindItem(int serial)
        {
            return (Item)m_Items[serial];
        }

        public static Item[] FindItems(IItemValidator check)
        {
            ArrayList dataStore = Engine.GetDataStore();
            IEnumerator enumerator = m_Items.Values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Item current = (Item)enumerator.Current;
                if (((current.Visible && current.InWorld) && (!current.IsMulti && InRange(current))) && check.IsValid(current))
                {
                    dataStore.Add(current);
                }
            }
            Item[] itemArray = (Item[])dataStore.ToArray(typeof(Item));
            Engine.ReleaseDataStore(dataStore);
            return itemArray;
        }

        public static Mobile FindMobile(int serial)
        {
            return (Mobile)m_Mobiles[serial];
        }

        public static int GetAmount(params Item[] items)
        {
            int num = 0;
            for (int i = 0; i < items.Length; i++)
            {
                num += (ushort)items[i].Amount;
            }
            return num;
        }

        public static bool InRange(IPoint2D p)
        {
            return ((m_Player == null) || ((((p.X >= (m_Player.X - m_Range)) && (p.X <= (m_Player.X + m_Range))) && (p.Y >= (m_Player.Y - m_Range))) && (p.Y <= (m_Player.Y + m_Range))));
        }

        public static bool InRange(Point3D p1, Point3D p2, int range)
        {
            return ((((p1.X >= (p2.X - range)) && (p1.X <= (p2.X + range))) && (p1.Y >= (p2.Y - range))) && (p1.Y <= (p2.Y + range)));
        }

        public static bool InUpdateRange(IPoint2D p)
        {
            return ((m_Player == null) || ((((p.X >= (m_X - m_Range)) && (p.X <= (m_X + m_Range))) && (p.Y >= (m_Y - m_Range))) && (p.Y <= (m_Y + m_Range))));
        }

        public static void Offset(int X, int Y)
        {
            int count = m_Messages.Count;
            for (int i = 0; i < count; i++)
            {
                ((StaticMessage)m_Messages[i]).Offset(X, Y);
            }
        }

        public static void Remove(Item item)
        {
            if (item != null)
            {
                if (item.IsMulti)
                {
                    Engine.Multis.Unregister(item);
                }
                if (item.Serial == 0x40000430)
                {
                    int num = 0;
                    num++;
                }
                item.Visible = false;
                if (item.InWorld)
                {
                    Map.RemoveItem(item);
                    item.InWorld = false;
                }
                if (item.Container != null)
                {
                    item.Container.Close();
                    item.Container = null;
                }
                if (item.Parent != null)
                {
                    item.Parent.RemoveItem(item);
                }
                if (item.IsEquip)
                {
                    item.RemoveEquip();
                }
                if (item.Items.Count > 0)
                {
                    item.Items.Clear();
                }
            }
        }

        public static void Remove(Mobile m)
        {
            if ((m != null) && !m.Player)
            {
                if (Engine.m_Highlight == m)
                {
                    Engine.m_Highlight = null;
                }
                if (m.Visible && (m.CorpseSerial == 0))
                {
                    Map.RemoveMobile(m);
                    m.Visible = false;
                }
                bool flag = false;
                if ((m.StatusBar != null) && !(m.StatusBar is GPartyHealthBar))
                {
                    m.StatusBar.Close();
                    m.StatusBar = null;
                    m.OpenedStatus = false;
                    flag = true;
                }
                else if (m.OpenedStatus)
                {
                    m.OpenedStatus = false;
                    flag = true;
                }
                if (flag)
                {
                    Network.Send(new PCloseStatus(m));
                }
                if (m.Paperdoll != null)
                {
                    Gumps.Destroy(m.Paperdoll);
                    m.Paperdoll = null;
                }
            }
        }

        public static void SetLocation(int x, int y, int z)
        {
            m_Z = z;
            if ((m_X != x) || (m_Y != y))
            {
                m_X = x;
                m_Y = y;
                IEnumerator enumerator = m_Mobiles.Values.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    Mobile current = (Mobile)enumerator.Current;
                    if ((current.Visible && !current.Player) && !InUpdateRange(current))
                    {
                        Remove(current);
                    }
                }
                enumerator = m_Items.Values.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    Item p = (Item)enumerator.Current;
                    if ((p.Visible && p.InWorld) && (!p.IsMulti && !InUpdateRange(p)))
                    {
                        Remove(p);
                    }
                }
            }
        }

        public static Item WantItem(int serial)
        {
            Item item = (Item)m_Items[serial];
            if (item == null)
            {
                item = new Item(serial);
                m_Items.Add(serial, item);
            }
            return item;
        }

        public static Item WantItem(int serial, ref bool wasFound)
        {
            Item item = (Item)m_Items[serial];
            wasFound = item != null;
            if (item == null)
            {
                item = new Item(serial);
                m_Items.Add(serial, item);
            }
            return item;
        }

        public static Mobile WantMobile(int serial)
        {
            Mobile mobile = (Mobile)m_Mobiles[serial];
            if (mobile == null)
            {
                mobile = new Mobile(serial);
                m_Mobiles.Add(serial, mobile);
            }
            return mobile;
        }

        public static Mobile WantMobile(int serial, ref bool wasFound)
        {
            Mobile mobile = (Mobile)m_Mobiles[serial];
            wasFound = mobile != null;
            if (mobile == null)
            {
                mobile = new Mobile(serial);
                m_Mobiles.Add(serial, mobile);
            }
            return mobile;
        }

        public static Client.CharData CharData
        {
            get
            {
                if (m_CharData == null)
                {
                    return new Client.CharData();
                }
                return m_CharData;
            }
            set
            {
                m_CharData = value;
            }
        }

        public static Hashtable Items
        {
            get
            {
                return m_Items;
            }
        }

        public static Hashtable Mobiles
        {
            get
            {
                return m_Mobiles;
            }
        }

        public static Mobile Player
        {
            get
            {
                return m_Player;
            }
        }

        public static int Range
        {
            get
            {
                return m_Range;
            }
            set
            {
                m_Range = value;
            }
        }

        public static int Serial
        {
            get
            {
                return m_Serial;
            }
            set
            {
                if (m_Player != null)
                {
                    m_Player.Player = false;
                }
                if (m_CharData != null)
                {
                    m_CharData.Save();
                }
                m_Serial = value;
                m_Player = FindMobile(m_Serial);
                if (m_Player != null)
                {
                    m_Player.Player = true;
                }
                m_CharData = new Client.CharData(m_Serial);
                Renderer.SetText(Engine.m_Text);
            }
        }

        public static int X
        {
            get
            {
                return m_X;
            }
            set
            {
                SetLocation(value, m_Y, m_Z);
            }
        }

        public static int Y
        {
            get
            {
                return m_Y;
            }
            set
            {
                SetLocation(m_X, value, m_Z);
            }
        }

        public static int Z
        {
            get
            {
                return m_Z;
            }
            set
            {
                m_Z = value;
            }
        }
    }
}