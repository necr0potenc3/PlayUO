namespace Client
{
    using System.Windows.Forms;

    public class GHResizer : Gump
    {
        protected IResizable m_Target;
        protected int m_xOffset;

        public GHResizer(IResizable Target) : base(0, 0)
        {
            this.m_Target = Target;
        }

        protected internal override void Draw(int X, int Y)
        {
            base.m_X = this.m_Target.Width - 5;
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return (!Engine.amMoving && (Engine.TargetHandler == null));
        }

        protected internal override void OnMouseDown(int X, int Y, MouseButtons mb)
        {
            Gumps.Capture = this;
            this.m_xOffset = X;
        }

        protected internal override void OnMouseMove(int X, int Y, MouseButtons mb)
        {
            if (Gumps.Capture == this)
            {
                Point point = ((Gump)this.m_Target).PointToScreen(new Point(0, 0));
                int minWidth = ((base.PointToScreen(new Point(X, Y)).X - point.X) + 6) - this.m_xOffset;
                if (minWidth < this.m_Target.MinWidth)
                {
                    minWidth = this.m_Target.MinWidth;
                }
                else if (minWidth > this.m_Target.MaxWidth)
                {
                    minWidth = this.m_Target.MaxWidth;
                }
                this.m_Target.Width = minWidth;
                Engine.Redraw();
            }
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            Gumps.Capture = null;
        }

        public override int Height
        {
            get
            {
                return this.m_Target.Height;
            }
        }

        public override int Width
        {
            get
            {
                return 6;
            }
        }
    }
}