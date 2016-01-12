namespace Client
{
    public class GBorder3D : GAlphaBackground
    {
        protected bool m_Inset;

        public GBorder3D(bool inset, int x, int y, int width, int height) : base(x, y, width, height)
        {
            this.m_Inset = inset;
            base.FillAlpha = 1f;
            base.FillColor = 0xc0c0c0;
        }

        protected internal override void Draw(int X, int Y)
        {
            Renderer.SetTexture(null);
            Renderer.AlphaTestEnable = false;
            if (base.m_FillAlpha == 1f)
            {
                Renderer.SetAlphaEnable(false);
                Renderer.SolidRect(base.m_FillColor, X + 1, Y + 1, base.m_Width - 2, base.m_Height - 2);
            }
            else
            {
                Renderer.SetAlphaEnable(true);
                Renderer.SetAlpha(base.m_FillAlpha);
                Renderer.SolidRect(base.m_FillColor, X + 1, Y + 1, base.m_Width - 2, base.m_Height - 2);
            }
            Renderer.SetAlphaEnable(false);
            Renderer.SolidRect(this.m_Inset ? 0x404040 : 0xffffff, X, Y, base.m_Width - 1, 1);
            Renderer.SolidRect(this.m_Inset ? 0x404040 : 0xffffff, X, Y + 1, 1, base.m_Height - 2);
            Renderer.SolidRect(0xc0c0c0, X, (Y + base.m_Height) - 1, 1, 1);
            Renderer.SolidRect(0xc0c0c0, (X + base.m_Width) - 1, Y, 1, 1);
            Renderer.SolidRect(this.m_Inset ? 0xffffff : 0x404040, X + 1, (Y + base.m_Height) - 1, base.m_Width - 1, 1);
            Renderer.SolidRect(this.m_Inset ? 0xffffff : 0x404040, (X + base.m_Width) - 1, Y + 1, 1, base.m_Height - 2);
            Renderer.AlphaTestEnable = true;
        }

        public bool Inset
        {
            get
            {
                return this.m_Inset;
            }
            set
            {
                this.m_Inset = value;
            }
        }
    }
}