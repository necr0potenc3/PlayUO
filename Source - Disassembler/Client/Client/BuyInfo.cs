namespace Client
{
    using System;

    public class BuyInfo : IComparable
    {
        private GBuyGump_InventoryItem m_InventoryGump;
        private Client.Item m_Item;
        private string m_Name;
        private GBuyGump_OfferedItem m_OfferedGump;
        private int m_Price;
        private double m_ToBuy;

        public BuyInfo(Client.Item item, int price, string name)
        {
            this.m_Item = item;
            this.m_Price = price;
            try
            {
                this.m_Name = Localization.GetString(Convert.ToInt32(name));
            }
            catch
            {
                this.m_Name = name;
            }
        }

        int IComparable.CompareTo(object x)
        {
            if (x == null)
            {
                return 1;
            }
            BuyInfo info = x as BuyInfo;
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
                return this.m_Item.Amount;
            }
        }

        public double dToBuy
        {
            get
            {
                return this.m_ToBuy;
            }
            set
            {
                this.m_ToBuy = value;
            }
        }

        public IHue Hue
        {
            get
            {
                return Hues.GetItemHue(this.m_Item.ID, this.m_Item.Hue);
            }
        }

        public GBuyGump_InventoryItem InventoryGump
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
                return (this.m_Item.ID & 0x3fff);
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }

        public GBuyGump_OfferedItem OfferedGump
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

        public int ToBuy
        {
            get
            {
                return (int)(this.m_ToBuy + 0.5);
            }
            set
            {
                this.m_ToBuy = value;
            }
        }
    }
}