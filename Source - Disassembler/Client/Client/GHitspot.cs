namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GHitspot : Gump, IClipable
    {
        private Client.Clipper m_Clipper;
        private int m_Height;
        private OnClick m_Target;
        private bool m_WasDown;
        private int m_Width;

        public GHitspot(int X, int Y, int Width, int Height, OnClick Target) : base(X, Y)
        {
            this.m_Width = Width;
            this.m_Height = Height;
            this.m_Target = Target;
        }

        protected internal override void Draw(int X, int Y)
        {
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return ((this.m_Clipper == null) || this.m_Clipper.Evaluate(base.PointToScreen(new Point(X, Y))));
        }

        protected internal override void OnMouseDown(int X, int Y, MouseButtons mb)
        {
            this.m_WasDown = true;
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            if (this.m_WasDown && (this.m_Target != null))
            {
                this.m_Target(this);
            }
            this.m_WasDown = false;
        }

        public Client.Clipper Clipper
        {
            get
            {
                return this.m_Clipper;
            }
            set
            {
                this.m_Clipper = value;
            }
        }

        public override int Height
        {
            get
            {
                return this.m_Height;
            }
        }

        public override int Width
        {
            get
            {
                return this.m_Width;
            }
        }
    }
}

