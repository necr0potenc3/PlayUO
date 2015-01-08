namespace Client
{
    using System;

    public class GBuyAccept : GRegion
    {
        private GBuyGump m_Owner;

        public GBuyAccept(GBuyGump owner) : base(30, 0xc1, 0x3f, 0x2a)
        {
            this.m_Owner = owner;
        }

        protected internal override void OnSingleClick(int x, int y)
        {
            this.m_Owner.Accept();
        }
    }
}

