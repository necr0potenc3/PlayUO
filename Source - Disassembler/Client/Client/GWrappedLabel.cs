namespace Client
{
    using System;

    public class GWrappedLabel : GLabel
    {
        protected int m_WrapWidth;

        public GWrappedLabel(string text, IFont font, IHue hue, int x, int y, int width) : base(x, y)
        {
            this.m_WrapWidth = width;
            base.m_Text = text;
            base.m_Font = font;
            base.m_Hue = hue;
            base.m_ITranslucent = true;
            this.Refresh();
        }

        public override void Refresh()
        {
            if (base.m_Invalidated)
            {
                base.m_Image = base.m_Font.GetString(Engine.WrapText(base.m_Text, this.m_WrapWidth, base.m_Font), base.m_Hue);
                if (base.m_Draw = (base.m_Image != null) && !base.m_Image.IsEmpty())
                {
                    base.m_Width = base.m_Image.Width;
                    base.m_Height = base.m_Image.Height;
                }
                base.m_Invalidated = false;
                if (base.m_vCache != null)
                {
                    base.m_vCache.Invalidate();
                }
            }
        }
    }
}

