namespace Client
{
    public class GImageClip : Gump, ITranslucent
    {
        protected bool m_bAlpha;
        private bool m_Draw;
        protected float m_fAlpha;
        private Texture m_Gump;
        private int m_GumpID;
        private int m_Height;
        private IHue m_Hue;
        private int m_Max;
        private int m_Val;
        private int m_Width;

        public GImageClip(int GumpID, int X, int Y, int Val, int Max) : this(GumpID, Hues.Default, X, Y, Val, Max)
        {
        }

        public GImageClip(int GumpID, IHue Hue, int X, int Y, int Val, int Max) : base(X, Y)
        {
            this.m_fAlpha = 1f;
            this.m_GumpID = GumpID;
            this.m_Hue = Hue;
            this.m_Val = Val;
            this.m_Max = Max;
            this.m_Gump = this.m_Hue.GetGump(GumpID);
            if ((this.m_Gump != null) && !this.m_Gump.IsEmpty())
            {
                this.m_Width = (int)(this.m_Gump.Width * this.Normal);
                this.m_Height = this.m_Gump.Height;
                this.m_Draw = true;
            }
            else
            {
                this.m_Width = this.m_Height = 0;
                this.m_Draw = false;
            }
            base.m_ITranslucent = true;
        }

        protected internal override void Draw(int X, int Y)
        {
            if (this.m_Draw)
            {
                if (this.m_bAlpha)
                {
                    Renderer.SetAlphaEnable(true);
                    Renderer.SetAlpha(this.m_fAlpha);
                    this.m_Gump.Draw(X, Y, this.m_Width, this.m_Height, 0xffffff);
                    Renderer.SetAlphaEnable(false);
                }
                else
                {
                    this.m_Gump.Draw(X, Y, this.m_Width, this.m_Height, 0xffffff);
                }
            }
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return false;
        }

        public void Resize(int Val, int Max)
        {
            this.m_Val = Val;
            this.m_Max = Max;
            this.m_Width = (int)(this.m_Gump.Width * this.Normal);
        }

        public float Alpha
        {
            get
            {
                return this.m_fAlpha;
            }
            set
            {
                this.m_fAlpha = value;
                this.m_bAlpha = !(value == 1f);
            }
        }

        public int GumpID
        {
            get
            {
                return this.m_GumpID;
            }
            set
            {
                if (this.m_GumpID != value)
                {
                    this.m_GumpID = value;
                    this.m_Gump = this.m_Hue.GetGump(this.m_GumpID);
                    if ((this.m_Gump != null) && !this.m_Gump.IsEmpty())
                    {
                        this.m_Width = (int)(this.m_Gump.Width * this.Normal);
                        this.m_Height = this.m_Gump.Height;
                        this.m_Draw = true;
                    }
                    else
                    {
                        this.m_Width = this.m_Height = 0;
                        this.m_Draw = false;
                    }
                }
            }
        }

        public override int Height
        {
            get
            {
                return this.m_Height;
            }
        }

        public IHue Hue
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
                    this.m_Gump = this.m_Hue.GetGump(this.m_GumpID);
                    if ((this.m_Gump != null) && !this.m_Gump.IsEmpty())
                    {
                        this.m_Width = (int)(this.m_Gump.Width * this.Normal);
                        this.m_Height = this.m_Gump.Height;
                        this.m_Draw = true;
                    }
                    else
                    {
                        this.m_Width = this.m_Height = 0;
                        this.m_Draw = false;
                    }
                }
            }
        }

        public double Normal
        {
            get
            {
                double num = ((double)this.m_Val) / ((double)this.m_Max);
                if (num < 0.0)
                {
                    return 0.0;
                }
                if (num > 1.0)
                {
                    return 1.0;
                }
                return num;
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