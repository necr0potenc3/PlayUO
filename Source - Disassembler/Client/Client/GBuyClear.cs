namespace Client
{
    using System;

    public class GBuyClear : GRegion
    {
        private GBuyGump m_Owner;

        public GBuyClear(GBuyGump owner) : base(0xa9, 0xc7, 0x37, 0x23)
        {
            this.m_Owner = owner;
        }

        protected internal override void OnSingleClick(int x, int y)
        {
            this.m_Owner.Clear();
        }
    }
}

