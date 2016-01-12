namespace Client
{
    using System.Windows.Forms;

    public class GMouseRouter : Gump
    {
        private bool m_Draw;
        private Texture m_Gump;
        private int m_GumpID;
        private int m_Height;
        private Gump m_Target;
        private VertexCache m_vCache;
        private int m_Width;

        public GMouseRouter(int GumpID, int X, int Y, Gump Target) : this(GumpID, Hues.Default, X, Y, Target)
        {
        }

        public GMouseRouter(int GumpID, IHue Hue, int X, int Y, Gump Target) : base(X, Y)
        {
            this.m_vCache = new VertexCache();
            this.m_Target = Target;
            this.m_GumpID = GumpID;
            this.m_Gump = Hue.GetGump(this.m_GumpID);
            if ((this.m_Gump != null) && !this.m_Gump.IsEmpty())
            {
                this.m_Width = this.m_Gump.Width;
                this.m_Height = this.m_Gump.Height;
                this.m_Draw = true;
            }
        }

        protected internal override void Draw(int X, int Y)
        {
            if (this.m_Draw)
            {
                this.m_vCache.Draw(this.m_Gump, X, Y);
            }
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return (this.m_Draw && this.m_Gump.HitTest(X, Y));
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