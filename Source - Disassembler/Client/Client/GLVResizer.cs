namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GLVResizer : Gump
    {
        protected IResizable m_Target;
        protected int m_xOffset;
        protected int m_yOffset;

        public GLVResizer(IResizable Target) : base(0, 0)
        {
            this.m_Target = Target;
        }

        protected internal override void Draw(int X, int Y)
        {
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
                Point point = ((Gump) this.m_Target).PointToScreen(new Point(this.m_Target.Width, 0));
                Point point2 = base.PointToScreen(new Point(X, Y));
                int num = point.X - (point2.X - this.m_xOffset);
                bool flag = false;
                bool flag2 = false;
                if (num < this.m_Target.MinWidth)
                {
                    flag = true;
                }
                else if (num > this.m_Target.MaxWidth)
                {
                    flag = true;
                }
                if (!flag)
                {
                    this.m_Target.Width = num;
                    ((Gump) this.m_Target).X = point2.X - this.m_xOffset;
                    flag2 = true;
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
                this.m_Target.Height = minHeight;
                if (flag2)
                {
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
                return 6;
            }
        }
    }
}

