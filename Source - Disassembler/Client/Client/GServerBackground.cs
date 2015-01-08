namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GServerBackground : GBackground
    {
        private GServerGump m_Owner;

        public GServerBackground(GServerGump owner, int x, int y, int width, int height, int gumpID, bool border) : base(gumpID, width, height, x, y, border)
        {
            this.m_Owner = owner;
            base.m_QuickDrag = true;
            base.m_CanDrag = owner.CanMove;
        }

        protected internal override void OnDragStart()
        {
            base.m_IsDragging = false;
            Gumps.Drag = null;
            Point point = base.PointToScreen(new Point(0, 0)) - this.m_Owner.PointToScreen(new Point(0, 0));
            this.m_Owner.m_OffsetX = point.X + base.m_OffsetX;
            this.m_Owner.m_OffsetY = point.Y + base.m_OffsetY;
            this.m_Owner.m_IsDragging = true;
            Gumps.Drag = this.m_Owner;
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            this.m_Owner.BringToTop();
        }

        protected internal override void OnMouseUp(int x, int y, MouseButtons mb)
        {
            if (((mb & MouseButtons.Right) != MouseButtons.None) && this.m_Owner.CanClose)
            {
                GServerGump.ClearCachedLocation(this.m_Owner.DialogID);
                Network.Send(new PGumpButton(this.m_Owner, 0));
                Gumps.Destroy(base.m_Parent);
            }
        }
    }
}

