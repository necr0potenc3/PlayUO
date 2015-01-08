namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GLTResizer : Gump
    {
        protected IResizable m_Target;
        protected int m_xOffset;
        protected int m_yOffset;

        public GLTResizer(IResizable Target) : base(0, 0)
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
            this.m_yOffset = Y;
        }

        protected internal override void OnMouseMove(int X, int Y, MouseButtons mb)
        {
            if (Gumps.Capture == this)
            {
                Point point = ((Gump) this.m_Target).PointToScreen(new Point(this.m_Target.Width, this.m_Target.Height));
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
                flag = false;
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
                    flag2 = true;
                }
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

