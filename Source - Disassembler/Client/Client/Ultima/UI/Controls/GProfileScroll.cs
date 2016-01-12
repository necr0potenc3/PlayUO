namespace Client
{
    using System.Windows.Forms;

    public class GProfileScroll : GImage
    {
        private Mobile m_Mobile;

        public GProfileScroll(Mobile m) : base(0x7d2, 0x18, 0xc3)
        {
            this.m_Mobile = m;
            base.m_Tooltip = new Tooltip(Strings.GetString("Tooltips.ProfileScroll"));
            base.m_CanDrag = true;
            base.m_QuickDrag = false;
        }

        protected internal override bool HitTest(int x, int y)
        {
            if (base.m_Invalidated)
            {
                base.Refresh();
            }
            return ((base.m_Draw && (Gumps.Drag == null)) && base.m_Image.HitTest(x, y));
        }

        protected internal override void OnDoubleClick(int x, int y)
        {
            Network.Send(new PProfileRequest(this.m_Mobile));
        }

        protected internal override void OnDragStart()
        {
            base.m_IsDragging = false;
            Gumps.Drag = null;
            Point point = base.PointToScreen(new Point(0, 0)) - base.m_Parent.PointToScreen(new Point(0, 0));
            base.m_Parent.m_OffsetX = point.X + base.m_OffsetX;
            base.m_Parent.m_OffsetY = point.Y + base.m_OffsetY;
            base.m_Parent.m_IsDragging = true;
            Gumps.Drag = base.m_Parent;
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            base.m_Parent.BringToTop();
        }

        protected internal override void OnMouseUp(int x, int y, MouseButtons mb)
        {
            if ((mb & MouseButtons.Right) != MouseButtons.None)
            {
                Point p = base.PointToScreen(new Point(x, y));
                p = base.m_Parent.PointToClient(p);
                base.m_Parent.OnMouseUp(p.X, p.Y, mb);
            }
        }
    }
}