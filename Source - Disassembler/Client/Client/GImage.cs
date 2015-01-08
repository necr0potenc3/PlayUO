namespace Client
{
    using System;

    public class GImage : Gump, ITranslucent, IClipable
    {
        protected bool m_bAlpha;
        protected Client.Clipper m_Clipper;
        protected bool m_Draw;
        protected float m_fAlpha;
        protected int m_GumpID;
        protected int m_Height;
        protected IHue m_Hue;
        protected Texture m_Image;
        protected bool m_Invalidated;
        private VertexCache m_vCache;
        private static VertexCachePool m_vPool = new VertexCachePool();
        protected int m_Width;

        public GImage(int gumpID, int x, int y) : this(gumpID, Hues.Default, x, y)
        {
        }

        public GImage(int gumpID, IHue hue, int x, int y) : base(x, y)
        {
            this.m_GumpID = gumpID;
            this.m_Hue = hue;
            this.Invalidate();
            this.Refresh();
            base.m_ITranslucent = true;
        }

        protected internal override void Draw(int x, int y)
        {
            if (this.m_Invalidated)
            {
                this.Refresh();
            }
            if (this.m_Draw)
            {
                if (this.m_bAlpha)
                {
                    Renderer.SetAlphaEnable(true);
                    Renderer.SetAlpha(this.m_fAlpha);
                }
                if (this.m_Clipper == null)
                {
                    if (this.m_vCache == null)
                    {
                        this.m_vCache = this.VCPool.GetInstance();
                    }
                    this.m_vCache.Draw(this.m_Image, x, y);
                }
                else
                {
                    this.m_Image.DrawClipped(x, y, this.m_Clipper);
                }
                if (this.m_bAlpha)
                {
                    Renderer.SetAlphaEnable(false);
                }
            }
        }

        protected void Invalidate()
        {
            this.m_Invalidated = true;
        }

        protected internal override void OnDispose()
        {
            this.VCPool.ReleaseInstance(this.m_vCache);
            this.m_vCache = null;
        }

        protected void Refresh()
        {
            if (this.m_Invalidated)
            {
                if (this.m_vCache != null)
                {
                    this.m_vCache.Invalidate();
                }
                this.m_Image = this.m_Hue.GetGump(this.m_GumpID);
                if ((this.m_Image != null) && !this.m_Image.IsEmpty())
                {
                    this.m_Width = this.m_Image.Width;
                    this.m_Height = this.m_Image.Height;
                    this.m_Draw = true;
                }
                else
                {
                    this.m_Width = this.m_Height = 0;
                    this.m_Draw = false;
                }
                this.m_Invalidated = false;
            }
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
                    this.Invalidate();
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
                    this.Invalidate();
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

