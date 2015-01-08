namespace Client
{
    using System;

    public class ItemTooltip : ITooltip
    {
        private Client.Gump m_Gump;
        private Item m_Item;

        public ItemTooltip(Item item)
        {
            this.m_Item = item;
        }

        public Client.Gump GetGump()
        {
            if (this.m_Gump != null)
            {
                return this.m_Gump;
            }
            if (this.m_Item.PropertyList == null)
            {
                this.m_Item.QueryProperties();
                return null;
            }
            return (this.m_Gump = new GObjectProperties(0xf9060 + (this.m_Item.ID & 0x3fff), this.m_Item, this.m_Item.PropertyList));
        }

        public float Delay
        {
            get
            {
                return 0.25f;
            }
            set
            {
            }
        }

        public Client.Gump Gump
        {
            get
            {
                return this.m_Gump;
            }
            set
            {
                this.m_Gump = value;
            }
        }
    }
}

