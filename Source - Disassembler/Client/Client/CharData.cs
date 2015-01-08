namespace Client
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    public class CharData
    {
        private bool m_AlwaysRun;
        private bool m_Archery;
        private bool m_AutoPickup;
        private ArrayList m_AutoUse;
        private int m_DefaultRegs;
        private int m_EmoteHue;
        private EquipManager m_Equip;
        private ArrayList m_Friends;
        private bool m_Halos;
        private bool m_IncomingNames;
        private ArrayList m_Layout;
        private bool m_LootGold;
        private string m_Name;
        private NotoQueryType m_NotoQuery;
        private int[] m_NotorietyHues;
        private bool m_QueueTargets;
        private int m_RegBag;
        private bool m_RestrictCures;
        private bool m_RestrictHeals;
        private int m_Serial;
        private string m_Shard;
        private int m_Stock;
        private int m_TextHue;
        private int m_WhisperHue;
        private int m_YellHue;

        public CharData()
        {
            this.m_TextHue = 0x60;
            this.m_EmoteHue = 0x60;
            this.m_WhisperHue = 0x60;
            this.m_YellHue = 0x60;
            this.m_NotoQuery = NotoQueryType.On;
            this.m_AutoPickup = true;
            this.m_RegBag = -1;
            this.m_Stock = -1;
            this.m_DefaultRegs = 100;
            this.m_IncomingNames = true;
            this.m_RestrictCures = true;
            this.m_RestrictHeals = true;
            this.m_NotorietyHues = new int[] { 0x59, 0x3f, 0x517, 0x3b2, 0x90, 0x22, 0x35 };
            this.m_Layout = new ArrayList();
            this.m_Equip = new EquipManager();
            this.m_Friends = new ArrayList();
            this.m_AutoUse = new ArrayList();
        }

        public CharData(int Serial)
        {
            this.m_TextHue = 0x60;
            this.m_EmoteHue = 0x60;
            this.m_WhisperHue = 0x60;
            this.m_YellHue = 0x60;
            this.m_NotoQuery = NotoQueryType.On;
            this.m_AutoPickup = true;
            this.m_RegBag = -1;
            this.m_Stock = -1;
            this.m_DefaultRegs = 100;
            this.m_IncomingNames = true;
            this.m_RestrictCures = true;
            this.m_RestrictHeals = true;
            this.m_NotorietyHues = new int[] { 0x59, 0x3f, 0x517, 0x3b2, 0x90, 0x22, 0x35 };
            this.m_Layout = new ArrayList();
            this.m_Equip = new EquipManager();
            this.m_Friends = new ArrayList();
            this.m_AutoUse = new ArrayList();
            this.m_Serial = Serial;
            string path = Engine.FileManager.BasePath("Data/Binary/Chardata.mul");
            if (File.Exists(path))
            {
                try
                {
                    using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
                    {
                        int num = reader.ReadInt32();
                        for (int i = 0; i < num; i++)
                        {
                            int num3 = reader.ReadInt32();
                            int num4 = reader.ReadInt32();
                            if (num3 == Serial)
                            {
                                this.LoadEntry(reader);
                                break;
                            }
                            reader.BaseStream.Seek((long) num4, SeekOrigin.Current);
                        }
                        reader.Close();
                    }
                }
                catch (Exception exception)
                {
                    this.m_TextHue = 0x60;
                    this.m_EmoteHue = 0x60;
                    this.m_WhisperHue = 0x60;
                    this.m_YellHue = 0x60;
                    this.m_Equip.AutoEquip.Clear();
                    this.m_Layout.Clear();
                    this.m_Friends.Clear();
                    this.m_AutoUse.Clear();
                    Debug.Trace("Chardata.mul read failed.");
                    Debug.Error(exception);
                    if (MessageBox.Show("Failed to read 'Data/Binary/Chardata.mul', it may be corrupt. If the problem persists, delete the file.\nDelete the file now?", "Client", MessageBoxButtons.YesNo, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        try
                        {
                            File.Delete(path);
                        }
                        catch
                        {
                            MessageBox.Show("Failed to delete 'Data/Binary/Chardata.mul'.", "Client", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        }
                    }
                }
            }
        }

        private void Construct(string FilePath, ArrayList Entries)
        {
            int count = Entries.Count;
            BinaryWriter bin = new BinaryWriter(File.Open(FilePath, FileMode.Create, FileAccess.Write));
            bin.Write((int) (count + 1));
            ArrayList list = new ArrayList();
            if (this.m_TextHue != 0x60)
            {
                list.Add(new Property(0, this.m_TextHue));
            }
            if (this.m_EmoteHue != 0x60)
            {
                list.Add(new Property(1, this.m_EmoteHue));
            }
            if (this.m_WhisperHue != 0x60)
            {
                list.Add(new Property(2, this.m_WhisperHue));
            }
            if (this.m_YellHue != 0x60)
            {
                list.Add(new Property(3, this.m_YellHue));
            }
            if (this.m_NotoQuery != NotoQueryType.On)
            {
                list.Add(new Property(7, (byte) this.m_NotoQuery));
            }
            if (this.m_QueueTargets)
            {
                list.Add(new Property(8, this.m_QueueTargets));
            }
            if (this.m_Halos)
            {
                list.Add(new Property(9, this.m_Halos));
            }
            list.Add(new Property(11, this.m_AutoPickup));
            if (this.m_LootGold)
            {
                list.Add(new Property(12, this.m_LootGold));
            }
            if (this.m_Archery)
            {
                list.Add(new Property(13, this.m_Archery));
            }
            if (this.m_RegBag != -1)
            {
                list.Add(new Property(14, this.m_RegBag));
            }
            if (this.m_Stock != -1)
            {
                list.Add(new Property(15, this.m_Stock));
            }
            if (this.m_DefaultRegs != 100)
            {
                list.Add(new Property(0x10, this.m_DefaultRegs));
            }
            if (this.m_Name != null)
            {
                list.Add(new Property(0x11, this.m_Name));
            }
            if (this.m_Shard != null)
            {
                list.Add(new Property(0x12, this.m_Shard));
            }
            if (this.m_NotorietyHues[0] != 0x59)
            {
                list.Add(new Property(0x13, this.m_NotorietyHues[0]));
            }
            if (this.m_NotorietyHues[1] != 0x3f)
            {
                list.Add(new Property(20, this.m_NotorietyHues[1]));
            }
            if (this.m_NotorietyHues[2] != 0x517)
            {
                list.Add(new Property(0x15, this.m_NotorietyHues[2]));
            }
            if (this.m_NotorietyHues[3] != 0x3b2)
            {
                list.Add(new Property(0x16, this.m_NotorietyHues[3]));
            }
            if (this.m_NotorietyHues[4] != 0x90)
            {
                list.Add(new Property(0x17, this.m_NotorietyHues[4]));
            }
            if (this.m_NotorietyHues[5] != 0x22)
            {
                list.Add(new Property(0x18, this.m_NotorietyHues[5]));
            }
            if (this.m_NotorietyHues[6] != 0x35)
            {
                list.Add(new Property(0x19, this.m_NotorietyHues[6]));
            }
            list.Add(new Property(0x1a, this.m_IncomingNames));
            list.Add(new Property(0x1b, this.m_RestrictCures));
            list.Add(new Property(0x1c, this.m_RestrictHeals));
            for (int i = 0; i < this.m_AutoUse.Count; i++)
            {
                Item item = (Item) this.m_AutoUse[i];
                list.Add(new Property(10, item.Serial));
            }
            for (int j = 0; j < this.m_Friends.Count; j++)
            {
                Mobile mobile = (Mobile) this.m_Friends[j];
                list.Add(new Property(6, mobile.Serial));
            }
            if (this.m_Equip != null)
            {
                IEnumerator enumerator = this.m_Equip.AutoEquip.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    list.Add(new Property(5, (int) current.Key, (int) current.Value));
                }
            }
            Gump desktop = Gumps.Desktop;
            if (desktop != null)
            {
                for (int m = 0; m < desktop.Children.Count; m++)
                {
                    IRestorableGump gump2 = desktop.Children[m] as IRestorableGump;
                    if ((gump2 != null) && ((Gump) gump2).Visible)
                    {
                        list.Add(new Property(4, new GumpLayout(gump2.Type, gump2.Extra, gump2.X, gump2.Y, gump2.Width, gump2.Height)));
                    }
                }
            }
            this.SaveEntry(this.m_Serial, (Property[]) list.ToArray(typeof(Property)), bin);
            for (int k = 0; k < count; k++)
            {
                bin.Write((byte[]) Entries[k]);
            }
            bin.Flush();
            bin.Close();
        }

        private void LoadEntry(BinaryReader bin)
        {
            int num = bin.ReadInt32();
            for (int i = 0; i < num; i++)
            {
                int num3 = bin.ReadInt32();
                int count = bin.ReadInt32();
                switch (num3)
                {
                    case 0:
                        this.m_TextHue = bin.ReadInt32();
                        break;

                    case 1:
                        this.m_EmoteHue = bin.ReadInt32();
                        break;

                    case 2:
                        this.m_WhisperHue = bin.ReadInt32();
                        break;

                    case 3:
                        this.m_YellHue = bin.ReadInt32();
                        break;

                    case 4:
                        this.m_Layout.Add(new GumpLayout(bin.ReadInt32(), bin.ReadInt32(), bin.ReadInt32(), bin.ReadInt32(), bin.ReadInt32(), bin.ReadInt32()));
                        break;

                    case 5:
                        this.m_Equip.AutoEquip.Add(bin.ReadInt32(), bin.ReadInt32());
                        break;

                    case 6:
                    {
                        Mobile mobile = World.WantMobile(bin.ReadInt32());
                        mobile.m_IsFriend = true;
                        this.m_Friends.Add(mobile);
                        break;
                    }
                    case 7:
                        this.m_NotoQuery = (NotoQueryType) bin.ReadByte();
                        break;

                    case 8:
                        this.m_QueueTargets = bin.ReadBoolean();
                        break;

                    case 9:
                        this.m_Halos = bin.ReadBoolean();
                        break;

                    case 10:
                    {
                        Item item = World.WantItem(bin.ReadInt32());
                        item.OverrideHue(0x22);
                        this.m_AutoUse.Add(item);
                        break;
                    }
                    case 11:
                        this.m_AutoPickup = bin.ReadBoolean();
                        break;

                    case 12:
                        this.m_LootGold = bin.ReadBoolean();
                        break;

                    case 13:
                        this.m_Archery = bin.ReadBoolean();
                        break;

                    case 14:
                        this.m_RegBag = bin.ReadInt32();
                        break;

                    case 15:
                        this.m_Stock = bin.ReadInt32();
                        break;

                    case 0x10:
                        this.m_DefaultRegs = bin.ReadInt32();
                        break;

                    case 0x11:
                        this.m_Name = Encoding.ASCII.GetString(bin.ReadBytes(count));
                        break;

                    case 0x12:
                        this.m_Shard = Encoding.ASCII.GetString(bin.ReadBytes(count));
                        break;

                    case 0x13:
                        this.m_NotorietyHues[0] = bin.ReadInt32();
                        break;

                    case 20:
                        this.m_NotorietyHues[1] = bin.ReadInt32();
                        break;

                    case 0x15:
                        this.m_NotorietyHues[2] = bin.ReadInt32();
                        break;

                    case 0x16:
                        this.m_NotorietyHues[3] = bin.ReadInt32();
                        break;

                    case 0x17:
                        this.m_NotorietyHues[4] = bin.ReadInt32();
                        break;

                    case 0x18:
                        this.m_NotorietyHues[5] = bin.ReadInt32();
                        break;

                    case 0x19:
                        this.m_NotorietyHues[6] = bin.ReadInt32();
                        break;

                    case 0x1a:
                        this.m_IncomingNames = bin.ReadBoolean();
                        break;

                    case 0x1b:
                        this.m_RestrictCures = bin.ReadBoolean();
                        break;

                    case 0x1c:
                        this.m_RestrictHeals = bin.ReadBoolean();
                        break;

                    default:
                        bin.BaseStream.Seek((long) count, SeekOrigin.Current);
                        break;
                }
            }
        }

        public void Save()
        {
            ArrayList entries = new ArrayList();
            string path = Engine.FileManager.BasePath("Data/Binary/Chardata.mul");
            if (File.Exists(path))
            {
                BinaryReader reader = new BinaryReader(File.OpenRead(path));
                int num = reader.ReadInt32();
                for (int i = 0; i < num; i++)
                {
                    int num3 = reader.ReadInt32();
                    int num4 = reader.ReadInt32();
                    if (num3 == this.m_Serial)
                    {
                        reader.BaseStream.Seek((long) num4, SeekOrigin.Current);
                    }
                    else
                    {
                        reader.BaseStream.Seek(-8L, SeekOrigin.Current);
                        entries.Add(reader.ReadBytes(num4 + 8));
                    }
                }
                reader.Close();
            }
            this.Construct(path, entries);
        }

        private void SaveEntry(int Serial, Property[] Properties, BinaryWriter bin)
        {
            int length = Properties.Length;
            int num2 = 4;
            for (int i = 0; i < length; i++)
            {
                num2 += Properties[i].Length;
            }
            bin.Write(Serial);
            bin.Write(num2);
            bin.Write(length);
            for (int j = 0; j < length; j++)
            {
                Properties[j].Write(bin);
            }
        }

        [OptionHue, Optionable("Ally", "Notoriety Hues", Default=0x3f)]
        public int AllyHue
        {
            get
            {
                return this.m_NotorietyHues[1];
            }
            set
            {
                this.m_NotorietyHues[1] = value;
                Hues.ClearNotos();
            }
        }

        [Optionable("Always Run", "Options", Default=false)]
        public bool AlwaysRun
        {
            get
            {
                return this.m_AlwaysRun;
            }
            set
            {
                this.m_AlwaysRun = value;
            }
        }

        [Optionable("Pickup Arrows", "Options", Default=false)]
        public bool Archery
        {
            get
            {
                return this.m_Archery;
            }
            set
            {
                this.m_Archery = value;
                this.Save();
            }
        }

        [Optionable("Attackable", "Notoriety Hues", Default=0x517), OptionHue]
        public int AttackableHue
        {
            get
            {
                return this.m_NotorietyHues[2];
            }
            set
            {
                this.m_NotorietyHues[2] = value;
                Hues.ClearNotos();
            }
        }

        [Optionable("Pickup Regs & Bolas", "Options", Default=true)]
        public bool AutoPickup
        {
            get
            {
                return this.m_AutoPickup;
            }
            set
            {
                this.m_AutoPickup = value;
                this.Save();
            }
        }

        public ArrayList AutoUse
        {
            get
            {
                return this.m_AutoUse;
            }
        }

        [OptionHue, Optionable("Criminal", "Notoriety Hues", Default=0x3b2)]
        public int CriminalHue
        {
            get
            {
                return this.m_NotorietyHues[3];
            }
            set
            {
                this.m_NotorietyHues[3] = value;
                Hues.ClearNotos();
            }
        }

        public int DefaultRegs
        {
            get
            {
                return this.m_DefaultRegs;
            }
            set
            {
                this.m_DefaultRegs = value;
                this.Save();
            }
        }

        [OptionHue, Optionable("Emote", "Text Hues", Default=0x60)]
        public int EmoteHue
        {
            get
            {
                return this.m_EmoteHue;
            }
            set
            {
                this.m_EmoteHue = value;
                Renderer.SetText(Engine.m_Text);
            }
        }

        [Optionable("Enemy", "Notoriety Hues", Default=0x90), OptionHue]
        public int EnemyHue
        {
            get
            {
                return this.m_NotorietyHues[4];
            }
            set
            {
                this.m_NotorietyHues[4] = value;
                Hues.ClearNotos();
            }
        }

        public EquipManager Equip
        {
            get
            {
                return this.m_Equip;
            }
        }

        public ArrayList Friends
        {
            get
            {
                return this.m_Friends;
            }
        }

        [Optionable("Notoriety Halos", "Options", Default=false)]
        public bool Halos
        {
            get
            {
                return this.m_Halos;
            }
            set
            {
                this.m_Halos = value;
                this.Save();
            }
        }

        [Optionable("Incoming Names", "Options", Default=true)]
        public bool IncomingNames
        {
            get
            {
                return this.m_IncomingNames;
            }
            set
            {
                this.m_IncomingNames = value;
            }
        }

        [OptionHue, Optionable("Innocent", "Notoriety Hues", Default=0x59)]
        public int InnocentHue
        {
            get
            {
                return this.m_NotorietyHues[0];
            }
            set
            {
                this.m_NotorietyHues[0] = value;
                Hues.ClearNotos();
            }
        }

        public ArrayList Layout
        {
            get
            {
                return this.m_Layout;
            }
        }

        [Optionable("Loot Gold", "Options", Default=false)]
        public bool LootGold
        {
            get
            {
                return this.m_LootGold;
            }
            set
            {
                this.m_LootGold = value;
                this.Save();
            }
        }

        [Optionable("Macros", "Options"), MacroEditor]
        public string Macros
        {
            get
            {
                return "...";
            }
            set
            {
            }
        }

        [OptionHue, Optionable("Murderer", "Notoriety Hues", Default=0x22)]
        public int MurdererHue
        {
            get
            {
                return this.m_NotorietyHues[5];
            }
            set
            {
                this.m_NotorietyHues[5] = value;
                Hues.ClearNotos();
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
                this.m_Name = value;
            }
        }

        [Optionable("Notoriety Query", "Options", Default=true)]
        public NotoQueryType NotoQuery
        {
            get
            {
                return this.m_NotoQuery;
            }
            set
            {
                this.m_NotoQuery = value;
                this.Save();
            }
        }

        public int[] NotorietyHues
        {
            get
            {
                return this.m_NotorietyHues;
            }
            set
            {
                this.m_NotorietyHues = value;
            }
        }

        [Optionable("Queue Targets", "Options", Default=false)]
        public bool QueueTargets
        {
            get
            {
                return this.m_QueueTargets;
            }
            set
            {
                this.m_QueueTargets = value;
                this.Save();
            }
        }

        public Item RegBag
        {
            get
            {
                if (this.m_RegBag != -1)
                {
                    Item item = World.FindItem(this.m_RegBag);
                    if ((item != null) && item.IsChildOf(World.Player))
                    {
                        return item;
                    }
                }
                return null;
            }
            set
            {
                this.m_RegBag = (value == null) ? -1 : value.Serial;
                this.Save();
            }
        }

        [Optionable("Only Cure When Poisoned", "Options", Default=true)]
        public bool RestrictCures
        {
            get
            {
                return this.m_RestrictCures;
            }
            set
            {
                this.m_RestrictCures = value;
            }
        }

        [Optionable("No Heal When Poisoned", "Options", Default=true)]
        public bool RestrictHeals
        {
            get
            {
                return this.m_RestrictHeals;
            }
            set
            {
                this.m_RestrictHeals = value;
            }
        }

        public string Shard
        {
            get
            {
                return this.m_Shard;
            }
            set
            {
                this.m_Shard = value;
            }
        }

        public Item Stock
        {
            get
            {
                if (this.m_Stock != -1)
                {
                    Item item = World.FindItem(this.m_Stock);
                    if ((item != null) && item.InAbsSquareRange(World.Player, 2))
                    {
                        return item;
                    }
                }
                return null;
            }
            set
            {
                this.m_Stock = (value == null) ? -1 : value.Serial;
                this.Save();
            }
        }

        [Optionable("Speech", "Text Hues", Default=0x60), OptionHue]
        public int TextHue
        {
            get
            {
                return this.m_TextHue;
            }
            set
            {
                this.m_TextHue = value;
                Renderer.SetText(Engine.m_Text);
            }
        }

        [Optionable("Vendor", "Notoriety Hues", Default=0x35), OptionHue]
        public int VendorHue
        {
            get
            {
                return this.m_NotorietyHues[6];
            }
            set
            {
                this.m_NotorietyHues[6] = value;
                Hues.ClearNotos();
            }
        }

        [Optionable("Whisper", "Text Hues", Default=0x60), OptionHue]
        public int WhisperHue
        {
            get
            {
                return this.m_WhisperHue;
            }
            set
            {
                this.m_WhisperHue = value;
                Renderer.SetText(Engine.m_Text);
            }
        }

        [OptionHue, Optionable("Yell", "Text Hues", Default=0x60)]
        public int YellHue
        {
            get
            {
                return this.m_YellHue;
            }
            set
            {
                this.m_YellHue = value;
                Renderer.SetText(Engine.m_Text);
            }
        }

        private class Property
        {
            private object m_Data;
            private int m_ID;
            private int m_Length;
            private PropertyType m_Type;

            public Property(int ID, GumpLayout g)
            {
                this.m_ID = ID;
                this.m_Data = g;
                this.m_Length = 0x18;
                this.m_Type = PropertyType.GumpLayout;
            }

            public Property(int ID, bool Data)
            {
                this.m_ID = ID;
                this.m_Data = Data;
                this.m_Length = 1;
                this.m_Type = PropertyType.Boolean;
            }

            public Property(int ID, byte Data)
            {
                this.m_ID = ID;
                this.m_Data = Data;
                this.m_Length = 1;
                this.m_Type = PropertyType.Byte;
            }

            public Property(int ID, int Data)
            {
                this.m_ID = ID;
                this.m_Data = Data;
                this.m_Length = 4;
                this.m_Type = PropertyType.Numeric;
            }

            public Property(int ID, string Data)
            {
                this.m_ID = ID;
                this.m_Data = Encoding.ASCII.GetBytes(Data);
                this.m_Length = ((byte[]) this.m_Data).Length;
                this.m_Type = PropertyType.String;
            }

            public Property(int id, int data1, int data2)
            {
                this.m_ID = id;
                this.m_Data = new int[] { data1, data2 };
                this.m_Length = 8;
                this.m_Type = PropertyType.Numeric2;
            }

            public void Write(BinaryWriter Output)
            {
                Output.Write(this.m_ID);
                Output.Write(this.m_Length);
                switch (this.m_Type)
                {
                    case PropertyType.Numeric:
                        Output.Write((int) this.m_Data);
                        break;

                    case PropertyType.Numeric2:
                        Output.Write(((int[]) this.m_Data)[0]);
                        Output.Write(((int[]) this.m_Data)[1]);
                        break;

                    case PropertyType.Byte:
                        Output.Write((byte) this.m_Data);
                        break;

                    case PropertyType.String:
                        Output.Write((byte[]) this.m_Data);
                        break;

                    case PropertyType.GumpLayout:
                    {
                        GumpLayout data = (GumpLayout) this.m_Data;
                        Output.Write(data.Type);
                        Output.Write(data.Extra);
                        Output.Write(data.X);
                        Output.Write(data.Y);
                        Output.Write(data.Width);
                        Output.Write(data.Height);
                        break;
                    }
                    case PropertyType.Boolean:
                        Output.Write((bool) this.m_Data);
                        break;
                }
            }

            public int Length
            {
                get
                {
                    return (8 + this.m_Length);
                }
            }

            private enum PropertyType
            {
                Numeric,
                Numeric2,
                Byte,
                String,
                GumpLayout,
                Boolean
            }
        }
    }
}

