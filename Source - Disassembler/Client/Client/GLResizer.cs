namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GLResizer : Gump
    {
        protected IResizable m_Target;
        protected int m_xOffset;

        public GLResizer(IResizable Target) : base(0, 0)
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
            this.m_xOffset = X;
        }

        protected internal override void OnMouseMove(int X, int Y, MouseButtons mb)
        {
            if (Gumps.Capture == this)
            {
                Point point = ((Gump) this.m_Target).PointToScreen(new Point(this.m_Target.Width, 0));
                Point point2 = base.PointToScreen(new Point(X, Y));
                int num = point.X - (point2.X - this.m_xOffset);
                if ((num >= this.m_Target.MinWidth) && (num <= this.m_Target.MaxWidth))
                {
                    this.m_Target.Width = num;
                    ((Gump) this.m_Target).X = point2.X - this.m_xOffset;
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

