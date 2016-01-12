namespace Client
{
    using System.Windows.Forms;

    public class GTResizer : Gump
    {
        protected IResizable m_Target;
        protected int m_yOffset;

        public GTResizer(IResizable Target) : base(0, 0)
        {
            this.m_Target = Target;
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return (!Engine.amMoving && (Engine.TargetHandler == null));
        }

        protected internal override void OnMouseDown(int X, int Y, MouseButtons mb)
        {
            Gumps.Capture = this;
            this.m_yOffset = Y;
        }

        protected internal override void OnMouseMove(int X, int Y, MouseButtons mb)
        {
            if (Gumps.Capture == this)
            {
                Point point = ((Gump)this.m_Target).PointToScreen(new Point(0, this.m_Target.Height));
                Point point2 = base.PointToScreen(new Point(X, Y));
                int num = point.Y - (point2.Y - this.m_yOffset);
                if ((num >= this.m_Target.MinHeight) && (num <= this.m_Target.MaxHeight))
                {
                    this.m_Target.Height = num;
                    ((Gump)this.m_Target).Y = point2.Y - this.m_yOffset;
                    Engine.Redraw();
                }
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
                return this.m_Target.Width;
            }
        }
    }
}