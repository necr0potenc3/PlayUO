namespace Client
{
    using System.Windows.Forms;

    public class GHVResizer : Gump
    {
        protected IResizable m_Target;
        protected int m_xOffset;
        protected int m_yOffset;

        public GHVResizer(IResizable Target) : base(0, 0)
        {
            this.m_Target = Target;
        }

        protected internal override void Draw(int X, int Y)
        {
            base.m_X = this.m_Target.Width - 5;
            base.m_Y = this.m_Target.Height - 5;
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return (!Engine.amMoving && (Engine.TargetHandler == null));
        }

        protected internal override void OnMouseDown(int X, int Y, MouseButtons mb)
        {
            Gumps.Capture = this;
            this.m_xOffset = X;
            this.m_yOffset = Y;
        }

        protected internal override void OnMouseMove(int X, int Y, MouseButtons mb)
        {
            if (Gumps.Capture == this)
            {
                Point point = ((Gump)this.m_Target).PointToScreen(new Point(0, 0));
                Point point2 = base.PointToScreen(new Point(X, Y));
                int minWidth = ((point2.X - point.X) + 6) - this.m_xOffset;
                if (minWidth < this.m_Target.MinWidth)
                {
                    minWidth = this.m_Target.MinWidth;
                }
                else if (minWidth > this.m_Target.MaxWidth)
                {
                    minWidth = this.m_Target.MaxWidth;
                }
                int minHeight = ((point2.Y - point.Y) + 6) - this.m_yOffset;
                if (minHeight < this.m_Target.MinHeight)
                {
                    minHeight = this.m_Target.MinHeight;
                }
                else if (minHeight > this.m_Target.MaxHeight)
                {
                    minHeight = this.m_Target.MaxHeight;
                }
                this.m_Target.Width = minWidth;
                this.m_Target.Height = minHeight;
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
                return 6;
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