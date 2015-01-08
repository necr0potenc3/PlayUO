namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GButton : Gump, ITranslucent, IClickable
    {
        protected bool m_bAlpha;
        protected bool m_CanEnter;
        protected bool[] m_Draw;
        protected bool m_Enabled;
        protected float m_fAlpha;
        protected Texture[] m_Gump;
        protected int[] m_GumpID;
        protected int m_Height;
        protected OnClick m_OnClick;
        protected int m_State;
        protected VertexCache m_vCache;
        private static VertexCachePool m_vPool = new VertexCachePool();
        protected int m_Width;

        public GButton(int GumpID, int X, int Y, OnClick ClickHandler) : this(GumpID, GumpID + 1, GumpID + 2, X, Y, ClickHandler)
        {
        }

        public GButton(int NormalID, int OverID, int PressedID, int X, int Y, OnClick ClickHandler) : base(X, Y)
        {
            this.m_fAlpha = 1f;
            this.m_GumpID = new int[] { NormalID, OverID, PressedID };
            this.m_Gump = new Texture[3];
            this.m_Draw = new bool[3];
            IHue hue = Hues.Default;
            for (int i = 0; i < 3; i++)
            {
                this.m_Gump[i] = hue.GetGump(this.m_GumpID[i]);
                this.m_Draw[i] = (this.m_Gump[i] != null) && !this.m_Gump[i].IsEmpty();
            }
            this.m_OnClick = ClickHandler;
            this.Enabled = this.m_OnClick != null;
            base.m_ITranslucent = true;
        }

        public void Click()
        {
            if (this.m_Enabled)
            {
                this.State = 1;
                Engine.DrawNow();
                this.State = 2;
                Engine.DrawNow();
                this.State = 0;
                Engine.DrawNow();
                if (this.m_OnClick != null)
                {
                    this.m_OnClick(this);
                }
            }
        }

        protected internal override void Draw(int x, int y)
        {
            if (this.m_Draw[this.m_State])
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
                this.m_vCache.Draw(this.m_Gump[this.m_State], x, y);
                if (this.m_bAlpha)
                {
                    Renderer.SetAlphaEnable(false);
                }
            }
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return ((this.m_Enabled && this.m_Draw[this.m_State]) && this.m_Gump[this.m_State].HitTest(X, Y));
        }

        protected internal override void OnDispose()
        {
            this.VCPool.ReleaseInstance(this.m_vCache);
            this.m_vCache = null;
        }

        protected internal override bool OnKeyDown(char c)
        {
            if ((c == '\r') && this.m_CanEnter)
            {
                this.Click();
                return true;
            }
            return false;
        }

        protected internal override void OnMouseDown(int X, int Y, MouseButtons mb)
        {
            if (this.m_Enabled)
            {
                this.State = 2;
            }
        }

        protected internal override void OnMouseEnter(int X, int Y, MouseButtons mb)
        {
            if (this.m_Enabled)
            {
                this.State = (mb == MouseButtons.None) ? 1 : 2;
            }
        }

        protected internal override void OnMouseLeave()
        {
            if (this.m_Enabled)
            {
                this.State = 0;
            }
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            if (this.m_Enabled)
            {
                this.State = 1;
                if (this.m_OnClick != null)
                {
                    this.m_OnClick(this);
                }
            }
        }

        public void SetGumpID(int GumpID)
        {
            this.SetGumpID(GumpID, GumpID + 1, GumpID + 2);
        }

        public void SetGumpID(int NormalID, int OverID, int PressedID)
        {
            this.m_GumpID = new int[] { NormalID, OverID, PressedID };
            this.m_Gump = new Texture[3];
            this.m_Draw = new bool[3];
            IHue hue = Hues.Default;
            for (int i = 0; i < 3; i++)
            {
                this.m_Gump[i] = hue.GetGump(this.m_GumpID[i]);
                this.m_Draw[i] = (this.m_Gump[i] != null) && !this.m_Gump[i].IsEmpty();
            }
            if (this.m_vCache != null)
            {
                this.m_vCache.Invalidate();
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

        public bool CanEnter
        {
            get
            {
                return this.m_CanEnter;
            }
            set
            {
                this.m_CanEnter = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return this.m_Enabled;
            }
            set
            {
                this.m_Enabled = value && (this.m_OnClick != null);
                if (this.m_Enabled)
                {
                    this.m_Gump = new Texture[3];
                    this.m_Draw = new bool[3];
                    IHue hue = Hues.Default;
                    for (int i = 0; i < 3; i++)
                    {
                        this.m_Gump[i] = hue.GetGump(this.m_GumpID[i]);
                        this.m_Draw[i] = (this.m_Gump[i] != null) && !this.m_Gump[i].IsEmpty();
                    }
                    this.State = this.m_State;
                }
                else
                {
                    this.m_Gump = new Texture[3];
                    this.m_Draw = new bool[3];
                    IHue grayscale = Hues.Grayscale;
                    for (int j = 0; j < 3; j++)
                    {
                        this.m_Gump[j] = grayscale.GetGump(this.m_GumpID[j]);
                        this.m_Draw[j] = (this.m_Gump[j] != null) && !this.m_Gump[j].IsEmpty();
                    }
                    this.State = 0;
                }
                if (this.m_vCache != null)
                {
                    this.m_vCache.Invalidate();
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

        public int State
        {
            get
            {
                return this.m_State;
            }
            set
            {
                this.m_State = value;
                if (this.m_Draw[this.m_State])
                {
                    this.m_Width = this.m_Gump[this.m_State].Width;
                    this.m_Height = this.m_Gump[this.m_State].Height;
                }
                else
                {
                    this.m_Width = 0;
                    this.m_Height = 0;
                }
                if (this.m_vCache != null)
                {
                    this.m_vCache.Invalidate();
                }
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
                return this.m_Width;
            }
        }
    }
}

