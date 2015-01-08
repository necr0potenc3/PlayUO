namespace Client
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class GThreeToggle : Gump
    {
        private Clipper m_Clipper;
        private bool[] m_Draw;
        private Texture[] m_Images;
        private Client.OnStateChange m_OnStateChange;
        private Size[] m_Sizes;
        private int m_State;
        private VertexCache m_vCache;
        private static VertexCachePool m_vPool = new VertexCachePool();

        public GThreeToggle(Texture state0, Texture state1, Texture state2, int initialState, int x, int y) : base(x, y)
        {
            this.m_Images = new Texture[] { state0, state1, state2 };
            this.m_Draw = new bool[] { (this.m_Images[0] != null) && !this.m_Images[0].IsEmpty(), (this.m_Images[1] != null) && !this.m_Images[1].IsEmpty(), (this.m_Images[2] != null) && !this.m_Images[2].IsEmpty() };
            this.m_Sizes = new Size[] { this.m_Draw[0] ? new Size(this.m_Images[0].Width, this.m_Images[0].Height) : Size.Empty, this.m_Draw[1] ? new Size(this.m_Images[1].Width, this.m_Images[1].Height) : Size.Empty, this.m_Draw[2] ? new Size(this.m_Images[2].Width, this.m_Images[2].Height) : Size.Empty };
            this.m_State = initialState;
        }

        protected internal override void Draw(int x, int y)
        {
            if (this.m_Draw[this.m_State])
            {
                Renderer.SetAlphaEnable(true);
                Renderer.SetAlpha(1f);
                if (this.m_Clipper != null)
                {
                    this.m_Images[this.m_State].DrawClipped(x, y, this.m_Clipper);
                }
                else
                {
                    if (this.m_vCache == null)
                    {
                        this.m_vCache = this.VCPool.GetInstance();
                    }
                    this.m_vCache.Draw(this.m_Images[this.m_State], x, y);
                }
                Renderer.SetAlphaEnable(false);
            }
        }

        protected internal override bool HitTest(int x, int y)
        {
            return ((this.m_Draw[this.m_State] && ((this.m_Clipper == null) || this.m_Clipper.Evaluate(base.PointToScreen(new Point(x, y))))) && this.m_Images[this.m_State].HitTest(x, y));
        }

        protected internal override void OnDispose()
        {
            this.VCPool.ReleaseInstance(this.m_vCache);
        }

        protected internal override void OnMouseUp(int x, int y, MouseButtons mb)
        {
            if ((mb & MouseButtons.Left) != MouseButtons.None)
            {
                this.State = (this.m_State + 1) % 3;
            }
        }

        public void Scissor(Clipper c)
        {
            this.m_Clipper = c;
        }

        public override int Height
        {
            get
            {
                return this.m_Sizes[this.m_State].Height;
            }
        }

        public Client.OnStateChange OnStateChange
        {
            get
            {
                return this.m_OnStateChange;
            }
            set
            {
                this.m_OnStateChange = value;
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
                if (this.m_State != value)
                {
                    if (this.m_vCache != null)
                    {
                        this.m_vCache.Invalidate();
                    }
                    this.m_State = value;
                    if (this.m_OnStateChange != null)
                    {
                        this.m_OnStateChange(this.m_State, this);
                    }
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
                return this.m_Sizes[this.m_State].Width;
            }
        }
    }
}

