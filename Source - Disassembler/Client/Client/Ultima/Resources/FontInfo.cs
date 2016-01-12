namespace Client
{
    public class FontInfo
    {
        private int m_Color;
        private UnicodeFont m_Font;
        private IHue m_Hue;

        public FontInfo(UnicodeFont font, int color)
        {
            this.m_Font = font;
            this.m_Color = color;
            this.m_Hue = new Hues.HFill(color);
        }

        public int Color
        {
            get
            {
                return this.m_Color;
            }
        }

        public UnicodeFont Font
        {
            get
            {
                return this.m_Font;
            }
        }

        public IHue Hue
        {
            get
            {
                return this.m_Hue;
            }
        }
    }
}