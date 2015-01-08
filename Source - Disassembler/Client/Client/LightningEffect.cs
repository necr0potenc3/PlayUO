namespace Client
{
    using System;

    public class LightningEffect : Effect
    {
        protected IHue m_Hue;
        protected int m_Start;
        protected Texture m_tCache;
        protected VertexCache m_vCache;

        public LightningEffect(IHue Hue)
        {
            this.m_vCache = new VertexCache();
            base.m_Children = new EffectList();
            this.m_Hue = Hue;
        }

        public LightningEffect(Item Source, IHue Hue) : this(Hue)
        {
            base.SetSource(Source);
        }

        public LightningEffect(Mobile Source, IHue Hue) : this(Hue)
        {
            base.SetSource(Source);
        }

        public LightningEffect(int xSource, int ySource, int zSource, IHue Hue) : this(Hue)
        {
            base.SetSource(xSource, ySource, zSource);
        }

        public LightningEffect(int Source, int xSource, int ySource, int zSource, IHue Hue) : this(Hue)
        {
            Mobile source = World.FindMobile(Source);
            if (source != null)
            {
                base.SetSource(source);
            }
            else
            {
                Item item = World.FindItem(Source);
                if (item != null)
                {
                    base.SetSource(item);
                }
                else
                {
                    base.SetSource(xSource, ySource, zSource);
                }
            }
        }

        public override void OnStart()
        {
            this.m_Start = Renderer.m_Frames;
        }

        public override bool Slice()
        {
            if ((Renderer.m_Frames - this.m_Start) >= 10)
            {
                return false;
            }
            int xWorld = Renderer.m_xWorld;
            int yWorld = Renderer.m_yWorld;
            int zWorld = Renderer.m_zWorld;
            int x = 0;
            int y = 0;
            int z = 0;
            int xOffset = 0;
            int yOffset = 0;
            int fOffset = 0;
            base.GetSource(out x, out y, out z);
            x -= xWorld;
            y -= yWorld;
            z -= zWorld;
            int num10 = ((Engine.GameWidth >> 1) - 0x16) + ((x - y) * 0x16);
            int num11 = (((Engine.GameHeight >> 1) + 11) + ((x + y) * 0x16)) - (z * 4);
            num10 += Engine.GameX;
            num11 += Engine.GameY;
            num10 -= Renderer.m_xScroll;
            num11 -= Renderer.m_yScroll;
            if ((base.m_Source != null) && (base.m_Source.GetType() == typeof(Mobile)))
            {
                Mobile source = (Mobile) base.m_Source;
                if (source.Walking.Count > 0)
                {
                    WalkAnimation animation = (WalkAnimation) source.Walking.Peek();
                    if (animation.Snapshot(ref xOffset, ref yOffset, ref fOffset))
                    {
                        num10 += xOffset;
                        num11 += yOffset;
                    }
                }
            }
            Texture gump = (Renderer.m_Dead ? Hues.Grayscale : this.m_Hue).GetGump(0x4e20 + (Renderer.m_Frames - this.m_Start));
            if (this.m_tCache != gump)
            {
                this.m_tCache = gump;
                this.m_vCache.Invalidate();
            }
            this.m_vCache.DrawGame(gump, num10 - (gump.Width / 2), num11 - gump.Height);
            return true;
        }

        public IHue Hue
        {
            get
            {
                return this.m_Hue;
            }
            set
            {
                this.m_Hue = value;
            }
        }
    }
}

