namespace Client
{
    using System;

    public class SellInfo : IComparable
    {
        private int m_Amount;
        private IHue m_Hue;
        private GSellGump_InventoryItem m_InventoryGump;
        private Client.Item m_Item;
        private int m_ItemID;
        private string m_Name;
        private GSellGump_OfferedItem m_OfferedGump;
        private int m_Price;
        private double m_ToSell;

        public SellInfo(Client.Item item, int itemID, int hue, int amount, int price, string name)
        {
            this.m_Item = item;
            this.m_ItemID = itemID;
            this.m_Amount = amount;
            this.m_Price = price;
            try
            {
                this.m_Name = Localization.GetString(Convert.ToInt32(name));
            }
            catch
            {
                this.m_Name = name;
            }
            if (!Map.m_ItemFlags[itemID & 0x3fff][TileFlag.PartialHue])
            {
                hue ^= 0x8000;
            }
            this.m_Hue = Hues.Load(hue);
        }

        int IComparable.CompareTo(object x)
        {
            if (x == null)
            {
                return 1;
            }
            SellInfo info = x as SellInfo;
            if (info == null)
            {
                throw new ArgumentException();
            }
            int num = Map.GetQuality(this.m_Item.ID).CompareTo(Map.GetQuality(info.m_Item.ID));
            if (num == 0)
            {
                num = this.m_Item.ID.CompareTo(info.m_Item.ID);
                if (num == 0)
                {
                    num = this.m_Item.Serial.CompareTo(info.m_Item.Serial);
                }
            }
            return num;
        }

        public int Amount
        {
            get
            {
                return this.m_Amount;
            }
        }

        public double dToSell
        {
            get
            {
                return this.m_ToSell;
            }
            set
            {
                this.m_ToSell = value;
            }
        }

        public IHue Hue
        {
            get
            {
                return this.m_Hue;
            }
        }

        public GSellGump_InventoryItem InventoryGump
        {
            get
            {
                return this.m_InventoryGump;
            }
            set
            {
                this.m_InventoryGump = value;
            }
        }

        public Client.Item Item
        {
            get
            {
                return this.m_Item;
            }
        }

        public int ItemID
        {
            get
            {
                return this.m_ItemID;
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }

        public GSellGump_OfferedItem OfferedGump
        {
            get
            {
                return this.m_OfferedGump;
            }
            set
            {
                this.m_OfferedGump = value;
            }
        }

        public int Price
        {
            get
            {
                return this.m_Price;
            }
        }

        public int ToSell
        {
            get
            {
                return (int) (this.m_ToSell + 0.5);
            }
            set
            {
                this.m_ToSell = value;
            }
        }
    }
}

