namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GHTResizer : Gump
    {
        protected IResizable m_Target;
        protected int m_xOffset;
        protected int m_yOffset;

        public GHTResizer(IResizable Target) : base(0, 0)
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
            this.m_yOffset = Y;
        }

        protected internal override void OnMouseMove(int X, int Y, MouseButtons mb)
        {
            if (Gumps.Capture == this)
            {
                Point point = ((Gump) this.m_Target).PointToScreen(new Point(0, this.m_Target.Height));
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
                bool flag = false;
                int num2 = point.Y - (point2.Y - this.m_yOffset);
                if (num2 < this.m_Target.MinHeight)
                {
                    flag = true;
                }
                else if (num2 > this.m_Target.MaxHeight)
                {
                    flag = true;
                }
                if (!flag)
                {
                    this.m_Target.Height = num2;
                    ((Gump) this.m_Target).Y = point2.Y - this.m_yOffset;
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

