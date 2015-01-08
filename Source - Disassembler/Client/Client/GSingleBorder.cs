namespace Client
{
    using System;

    public class GSingleBorder : Gump
    {
        protected int m_Height;
        protected int m_Width;

        public GSingleBorder(int X, int Y, int Width, int Height) : base(X, Y)
        {
            this.m_Width = Width;
            this.m_Height = Height;
        }

        protected internal override void Draw(int X, int Y)
        {
            Renderer.SetTexture(null);
            Renderer.AlphaTestEnable = false;
            Renderer.TransparentRect(0, X, Y, this.m_Width, this.m_Height);
            Renderer.AlphaTestEnable = true;
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

