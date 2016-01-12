namespace Client
{
    using System.Windows.Forms;

    public class GItemListScroller : GHitspot
    {
        private double m_Last;
        private int m_Offset;
        private GItemList m_Owner;
        private bool m_Scrolling;

        public GItemListScroller(int x, GItemList owner, int offset) : base(x, 0x3b, 12, 0x13, null)
        {
            this.m_Owner = owner;
            this.m_Offset = offset;
            this.m_Last = -1234.56;
        }

        protected internal override void Draw(int x, int y)
        {
            if (this.m_Scrolling && (Gumps.LastOver == this))
            {
                if (this.m_Last != -1234.56)
                {
                    this.m_Owner.xOffset += ((Engine.dTicks - this.m_Last) / 1000.0) * this.m_Offset;
                }
                this.m_Last = Engine.dTicks;
            }
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            this.m_Owner.BringToTop();
            this.OnMouseEnter(x, y, mb);
        }

        protected internal override void OnMouseEnter(int x, int y, MouseButtons mb)
        {
            if ((mb & MouseButtons.Left) != MouseButtons.None)
            {
                this.m_Scrolling = true;
                this.m_Last = Engine.dTicks;
            }
        }

        protected internal override void OnMouseLeave()
        {
            this.m_Scrolling = false;
        }

        protected internal override void OnMouseUp(int x, int y, MouseButtons mb)
        {
            if ((mb & MouseButtons.Left) != MouseButtons.None)
            {
                this.m_Scrolling = false;
                this.m_Last = -1234.56;
            }
        }
    }
}