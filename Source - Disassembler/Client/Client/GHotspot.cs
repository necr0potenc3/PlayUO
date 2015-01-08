namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GHotspot : Gump
    {
        private int m_Height;
        private bool m_NormalHit;
        private Gump m_Target;
        private int m_Width;

        public GHotspot(int X, int Y, int Width, int Height, Gump Target) : base(X, Y)
        {
            this.m_NormalHit = true;
            this.m_Width = Width;
            this.m_Height = Height;
            this.m_Target = Target;
        }

        protected internal override void Draw(int X, int Y)
        {
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return (this.m_NormalHit || (!Engine.amMoving && (Engine.TargetHandler == null)));
        }

        protected internal override void OnDoubleClick(int X, int Y)
        {
            if (this.m_Target != null)
            {
                this.m_Target.OnDoubleClick((base.m_X - this.m_Target.X) + X, (base.m_Y - this.m_Target.Y) + Y);
            }
        }

        protected internal override void OnDragStart()
        {
            if (this.m_Target != null)
            {
                base.m_IsDragging = false;
                Gumps.Drag = null;
                Point point = base.PointToScreen(new Point(0, 0)) - this.m_Target.PointToScreen(new Point(0, 0));
                this.m_Target.m_OffsetX = point.X + base.m_OffsetX;
                this.m_Target.m_OffsetY = point.Y + base.m_OffsetY;
                this.m_Target.m_IsDragging = true;
                Gumps.Drag = this.m_Target;
            }
        }

        protected internal override void OnMouseDown(int X, int Y, MouseButtons mb)
        {
            if (this.m_Target != null)
            {
                this.m_Target.OnMouseDown((base.m_X - this.m_Target.X) + X, (base.m_Y - this.m_Target.Y) + Y, mb);
            }
        }

        protected internal override void OnMouseEnter(int X, int Y, MouseButtons mb)
        {
            if (this.m_Target != null)
            {
                this.m_Target.OnMouseEnter((base.m_X - this.m_Target.X) + X, (base.m_Y - this.m_Target.Y) + Y, mb);
            }
        }

        protected internal override void OnMouseLeave()
        {
            if (this.m_Target != null)
            {
                this.m_Target.OnMouseLeave();
            }
        }

        protected internal override void OnMouseMove(int X, int Y, MouseButtons mb)
        {
            if (this.m_Target != null)
            {
                this.m_Target.OnMouseMove((base.m_X - this.m_Target.X) + X, (base.m_Y - this.m_Target.Y) + Y, mb);
            }
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            if (this.m_Target != null)
            {
                this.m_Target.OnMouseUp((base.m_X - this.m_Target.X) + X, (base.m_Y - this.m_Target.Y) + Y, mb);
            }
        }

        protected internal override void OnMouseWheel(int Delta)
        {
            if (this.m_Target != null)
            {
                this.m_Target.OnMouseWheel(Delta);
            }
        }

        protected internal override void OnSingleClick(int X, int Y)
        {
            if (this.m_Target != null)
            {
                this.m_Target.OnSingleClick((base.m_X - this.m_Target.X) + X, (base.m_Y - this.m_Target.Y) + Y);
            }
        }

        public override int Height
        {
            get
            {
                return this.m_Height;
            }
            set
            {
                this.m_Height = value;
            }
        }

        public bool NormalHit
        {
            get
            {
                return this.m_NormalHit;
            }
            set
            {
                this.m_NormalHit = value;
            }
        }

        public override int Width
        {
            get
            {
                return this.m_Width;
            }
            set
            {
                this.m_Width = value;
            }
        }
    }
}

