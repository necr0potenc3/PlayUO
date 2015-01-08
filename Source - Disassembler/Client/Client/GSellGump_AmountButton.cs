namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GSellGump_AmountButton : GImage
    {
        private SellInfo m_Info;
        private int m_InitialOffset;
        private double m_Last;
        private double m_Offset;
        private GSellGump m_Owner;
        private bool m_Scrolling;

        public GSellGump_AmountButton(GSellGump owner, SellInfo info, int offset, int gumpID, int x) : base(gumpID, x, 0)
        {
            this.m_Owner = owner;
            this.m_Info = info;
            this.m_InitialOffset = offset;
            this.m_Offset = offset;
            this.m_Last = -1234.56;
        }

        private void AmountChanged()
        {
            if (this.m_Info.ToSell <= 0)
            {
                this.m_Info.ToSell = 0;
                this.m_Info.OfferedGump = null;
                Gumps.Destroy(base.m_Parent);
            }
            else if (this.m_Info.ToSell > this.m_Info.Amount)
            {
                this.m_Info.ToSell = this.m_Info.Amount;
            }
            this.m_Info.InventoryGump.Available = this.m_Info.Amount - this.m_Info.ToSell;
            if (this.m_Info.OfferedGump != null)
            {
                this.m_Info.OfferedGump.Amount = this.m_Info.ToSell;
            }
            this.m_Owner.OfferMenu.UpdateTotal();
        }

        protected internal override void Draw(int x, int y)
        {
            if ((this.m_Scrolling && (Gumps.LastOver == this)) && ((this.m_Last != -1234.56) && (Engine.dTicks > this.m_Last)))
            {
                this.m_Info.dToSell += ((Engine.dTicks - this.m_Last) / 1000.0) * this.m_Offset;
                this.m_Offset += (Math.Sign(this.m_Offset) * ((Engine.dTicks - this.m_Last) / 1000.0)) * 5.0;
                this.AmountChanged();
                this.m_Last = Engine.dTicks;
            }
            base.Draw(x, y);
        }

        protected internal override bool HitTest(int x, int y)
        {
            if (base.m_Invalidated)
            {
                base.Refresh();
            }
            return ((base.m_Draw && (base.m_Clipper != null)) && base.m_Clipper.Evaluate(base.PointToScreen(new Point(x, y))));
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            this.OnMouseEnter(x, y, mb);
        }

        protected internal override void OnMouseEnter(int x, int y, MouseButtons mb)
        {
            if ((mb & MouseButtons.Left) != MouseButtons.None)
            {
                this.m_Scrolling = true;
                Engine.ResetTicks();
                this.m_Last = Engine.dTicks + 150.0;
                this.m_Info.ToSell += Math.Sign(this.m_Offset);
                this.AmountChanged();
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
                this.m_Offset = this.m_InitialOffset;
            }
        }
    }
}

