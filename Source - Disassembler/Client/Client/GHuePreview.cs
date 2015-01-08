namespace Client
{
    using System;

    public class GHuePreview : Gump
    {
        private int[] m_Colors;
        private int m_Height;
        private int m_Hue;
        private bool m_Solid;
        private int m_Width;
        private float m_xRun;

        public GHuePreview(int X, int Y, int Width, int Height, int Hue, bool Solid) : base(X, Y)
        {
            this.m_Width = Width;
            this.m_Height = Height;
            this.m_Hue = Hue;
            this.m_Solid = Solid;
            if (!Solid)
            {
                this.m_xRun = 31f / ((float) Width);
                this.m_Colors = new int[0x20];
                for (int i = 0; i < 0x20; i++)
                {
                    ushort num2 = Hues.GetData((Hue - 1) & 0x7fff).colors[0x20 + i];
                    int num3 = (num2 >> 10) & 0x1f;
                    int num4 = (num2 >> 5) & 0x1f;
                    int num5 = num2 & 0x1f;
                    num3 = (int) (num3 * 8.225806f);
                    num4 = (int) (num4 * 8.225806f);
                    num5 = (int) (num5 * 8.225806f);
                    this.m_Colors[i] = ((num3 << 0x10) | (num4 << 8)) | num5;
                }
            }
            else
            {
                ushort num7 = Hues.GetData((Hue - 1) & 0x7fff).colors[0x30];
                int num8 = (num7 >> 10) & 0x1f;
                int num9 = (num7 >> 5) & 0x1f;
                int num10 = num7 & 0x1f;
                num8 = (int) (num8 * 8.225806f);
                num9 = (int) (num9 * 8.225806f);
                num10 = (int) (num10 * 8.225806f);
                int num11 = ((num8 << 0x10) | (num9 << 8)) | num10;
                this.m_Colors = new int[] { num11 };
            }
        }

        protected internal override void Draw(int X, int Y)
        {
            Renderer.SetTexture(null);
            Renderer.AlphaTestEnable = false;
            if (this.m_Solid)
            {
                Renderer.SolidRect(this.m_Colors[0], X, Y, this.m_Width, this.m_Height);
            }
            else
            {
                Renderer.GradientRectLR(this.m_Colors[0], this.m_Colors[0x1f], X, Y, this.m_Width, this.m_Height);
            }
            Renderer.AlphaTestEnable = true;
        }

        public override int Height
        {
            get
            {
                return this.m_Height;
            }
        }

        public int Hue
        {
            get
            {
                return this.m_Hue;
            }
            set
            {
                if (this.m_Hue != value)
                {
                    this.m_Hue = value;
                    if (!this.m_Solid)
                    {
                        this.m_xRun = 31f / ((float) this.Width);
                        this.m_Colors = new int[0x20];
                        for (int i = 0; i < 0x20; i++)
                        {
                            ushort num2 = Hues.GetData((this.m_Hue - 1) & 0x7fff).colors[0x20 + i];
                            int num3 = (num2 >> 10) & 0x1f;
                            int num4 = (num2 >> 5) & 0x1f;
                            int num5 = num2 & 0x1f;
                            num3 = (int) (num3 * 8.225806f);
                            num4 = (int) (num4 * 8.225806f);
                            num5 = (int) (num5 * 8.225806f);
                            this.m_Colors[i] = ((num3 << 0x10) | (num4 << 8)) | num5;
                        }
                    }
                    else
                    {
                        ushort num7 = Hues.GetData((this.m_Hue - 1) & 0x7fff).colors[0x30];
                        int num8 = (num7 >> 10) & 0x1f;
                        int num9 = (num7 >> 5) & 0x1f;
                        int num10 = num7 & 0x1f;
                        num8 = (int) (num8 * 8.225806f);
                        num9 = (int) (num9 * 8.225806f);
                        num10 = (int) (num10 * 8.225806f);
                        int num11 = ((num8 << 0x10) | (num9 << 8)) | num10;
                        this.m_Colors = new int[] { num11 };
                    }
                }
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

