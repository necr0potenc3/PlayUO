namespace Client
{
    using System;

    public class GSecureTradeCheck : GButtonNew
    {
        private bool m_Checked;
        private Item m_Item;
        private GSecureTradeCheck m_Partner;

        public GSecureTradeCheck(int x, int y, Item item, GSecureTradeCheck partner) : base(0x867, 0x868, 0x868, x, y)
        {
            this.m_Item = item;
            this.m_Partner = partner;
            base.Enabled = this.m_Item != null;
        }

        protected override void OnClicked()
        {
            if (this.m_Item != null)
            {
                Network.Send(new PCheckTrade(this.m_Item, !this.m_Checked, this.m_Partner.m_Checked));
            }
        }

        public bool Checked
        {
            get
            {
                return this.m_Checked;
            }
            set
            {
                if (this.m_Checked != value)
                {
                    this.m_Checked = value;
                    if (this.m_Checked)
                    {
                        base.m_GumpID[0] = 0x869;
                        base.m_GumpID[1] = 0x86a;
                        base.m_GumpID[2] = 0x86a;
                    }
                    else
                    {
                        base.m_GumpID[0] = 0x867;
                        base.m_GumpID[1] = 0x868;
                        base.m_GumpID[2] = 0x868;
                    }
                    base.Invalidate();
                }
            }
        }
    }
}

