namespace Client
{
    public class GLabel : Gump, ITranslucent, IClipable
    {
        protected bool m_bAlpha;
        protected bool m_Clip;
        protected Client.Clipper m_Clipper;
        protected bool m_Draw;
        protected float m_fAlpha;
        protected IFont m_Font;
        protected int m_Height;
        protected IHue m_Hue;
        protected Texture m_Image;
        protected bool m_Invalidated;
        protected bool m_Relative;
        protected int m_SpaceWidth;
        protected string m_Text;
        protected bool m_Underline;
        protected VertexCache m_vCache;
        private static VertexCachePool m_vPool = new VertexCachePool();
        protected int m_Width;
        protected int m_xClipOffset;
        protected int m_xClipWidth;
        protected int m_yClipHeight;
        protected int m_yClipOffset;

        protected GLabel(int X, int Y) : base(X, Y)
        {
            this.m_Invalidated = true;
            this.m_Text = "";
            this.m_fAlpha = 1f;
        }

        public GLabel(string Text, IFont Font, IHue Hue, int X, int Y) : base(X, Y)
        {
            this.m_Invalidated = true;
            this.m_Text = "";
            this.m_fAlpha = 1f;
            this.m_Text = Text;
            this.m_Font = Font;
            this.m_Hue = Hue;
            base.m_ITranslucent = true;
            this.Refresh();
        }

        public override void Center()
        {
            if (base.m_Parent == null)
            {
                base.Center();
            }
            else
            {
                if (this.m_Invalidated)
                {
                    this.Refresh();
                }
                base.m_X = ((base.m_Parent.Width - (this.m_Image.xMax - this.m_Image.xMin)) / 2) - this.m_Image.xMin;
                base.m_Y = ((base.m_Parent.Height - (this.m_Image.yMax - this.m_Image.yMin)) / 2) - this.m_Image.yMin;
            }
        }

        protected internal override void Draw(int X, int Y)
        {
            if (this.m_Invalidated)
            {
                this.Refresh();
            }
            if (this.m_Draw)
            {
                if (!this.m_Clip)
                {
                    if (this.m_bAlpha)
                    {
                        Renderer.SetAlpha(this.m_fAlpha);
                        Renderer.SetAlphaEnable(true);
                    }
                    if (this.m_vCache == null)
                    {
                        this.m_vCache = this.VCPool.GetInstance();
                    }
                    this.m_vCache.Draw(this.m_Image, X, Y);
                    if (this.m_bAlpha)
                    {
                        Renderer.SetAlphaEnable(false);
                    }
                }
                else if (!this.m_Relative)
                {
                    if (!this.m_bAlpha)
                    {
                        this.m_Image.DrawClipped(X, Y, this.m_Clipper);
                    }
                    else
                    {
                        Renderer.SetAlphaEnable(true);
                        Renderer.SetAlpha(this.m_fAlpha);
                        this.m_Image.DrawClipped(X, Y, this.m_Clipper);
                        Renderer.SetAlphaEnable(false);
                    }
                }
                else if (!this.m_bAlpha)
                {
                    this.m_Image.DrawClipped(X, Y, Client.Clipper.TemporaryInstance(X + this.m_xClipOffset, Y + this.m_yClipOffset, this.m_xClipWidth, this.m_yClipHeight));
                }
                else
                {
                    Renderer.SetAlphaEnable(true);
                    Renderer.SetAlpha(this.m_fAlpha);
                    this.m_Image.DrawClipped(X, Y, Client.Clipper.TemporaryInstance(X + this.m_xClipOffset, Y + this.m_yClipOffset, this.m_xClipWidth, this.m_yClipHeight));
                    Renderer.SetAlphaEnable(false);
                }
            }
        }

        public virtual void Invalidate()
        {
            this.m_Invalidated = true;
        }

        protected internal override void OnDispose()
        {
            this.VCPool.ReleaseInstance(this.m_vCache);
            this.m_vCache = null;
        }

        public virtual void Refresh()
        {
            if (this.m_Invalidated)
            {
                UnicodeFont font = this.m_Font as UnicodeFont;
                bool flag = (font != null) && font.Underline;
                int num = (font == null) ? 0 : font.SpaceWidth;
                if (font != null)
                {
                    font.Underline = this.m_Underline;
                }
                if ((font != null) && (this.m_SpaceWidth > 0))
                {
                    font.SpaceWidth = this.m_SpaceWidth;
                }
                this.m_Image = this.m_Font.GetString(this.m_Text, this.m_Hue);
                if ((this.m_Image != null) && !this.m_Image.IsEmpty())
                {
                    this.m_Width = this.m_Image.Width;
                    this.m_Height = this.m_Image.Height;
                    this.m_Draw = true;
                }
                else
                {
                    this.m_Draw = false;
                }
                this.m_Invalidated = false;
                if (this.m_vCache != null)
                {
                    this.m_vCache.Invalidate();
                }
                if (font != null)
                {
                    font.Underline = flag;
                }
                if (font != null)
                {
                    font.SpaceWidth = num;
                }
            }
        }

        public void Scissor(Client.Clipper Clipper)
        {
            this.m_Relative = false;
            this.m_Clip = Clipper != null;
            this.m_Clipper = Clipper;
        }

        public void Scissor(int xOffset, int yOffset, int xWidth, int yHeight)
        {
            this.m_Relative = true;
            this.m_Clip = true;
            this.m_Clipper = null;
            this.m_xClipOffset = xOffset;
            this.m_yClipOffset = yOffset;
            this.m_xClipWidth = xWidth;
            this.m_yClipHeight = yHeight;
        }

        public virtual float Alpha
        {
            get
            {
                return this.m_fAlpha;
            }
            set
            {
                this.m_fAlpha = value;
                this.m_bAlpha = !(this.m_fAlpha == 1f);
            }
        }

        public Client.Clipper Clipper
        {
            get
            {
                return this.m_Clipper;
            }
            set
            {
                this.m_Relative = false;
                this.m_Clip = value != null;
                this.m_Clipper = value;
            }
        }

        public virtual IFont Font
        {
            get
            {
                return this.m_Font;
            }
            set
            {
                if (this.m_Font != value)
                {
                    this.Invalidate();
                    this.m_Font = value;
                }
            }
        }

        public override int Height
        {
            get
            {
                if (this.m_Invalidated)
                {
                    this.Refresh();
                }
                return this.m_Height;
            }
        }

        public virtual IHue Hue
        {
            get
            {
                return this.m_Hue;
            }
            set
            {
                if (this.m_Hue != value)
                {
                    this.Invalidate();
                    this.m_Hue = value;
                }
            }
        }

        public Texture Image
        {
            get
            {
                if (this.m_Invalidated)
                {
                    this.Refresh();
                }
                return this.m_Image;
            }
        }

        public int SpaceWidth
        {
            get
            {
                return this.m_SpaceWidth;
            }
            set
            {
                this.m_SpaceWidth = value;
                this.Invalidate();
            }
        }

        public virtual string Text
        {
            get
            {
                return this.m_Text;
            }
            set
            {
                if (this.m_Text != value)
                {
                    this.Invalidate();
                    this.m_Text = value;
                }
            }
        }

        public bool Underline
        {
            get
            {
                return this.m_Underline;
            }
            set
            {
                this.m_Underline = value;
                this.Invalidate();
            }
        }

        protected VertexCachePool VCPool
        {
            get
            {
                return m_vPool;
            }
        }

        public override int Width
        {
            get
            {
                if (this.m_Invalidated)
                {
                    this.Refresh();
                }
                return this.m_Width;
            }
        }
    }
}