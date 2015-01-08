namespace Client
{
    using System;

    public class AnimatedItemEffect : Effect
    {
        protected bool m_Animated;
        protected sbyte[] m_Animation;
        protected int m_Delay;
        protected int m_Duration;
        protected int m_FrameCount;
        protected IHue m_Hue;
        protected int m_ItemID;
        public int m_RenderMode;
        protected int m_Start;
        protected VertexCache m_vCache;

        public AnimatedItemEffect(int ItemID, IHue Hue, int duration)
        {
            base.m_Children = new EffectList();
            this.m_Duration = duration;
            this.m_Hue = Hue;
            ItemID &= 0x3fff;
            this.m_ItemID = ItemID | 0x4000;
            this.m_Animated = true;
            AnimData anim = Map.GetAnim(ItemID);
            this.m_FrameCount = anim.frameCount;
            this.m_Delay = anim.frameInterval;
            this.m_Animation = new sbyte[0x40];
            for (int i = 0; i < 0x40; i++)
            {
                this.m_Animation[i] = anim[i];
            }
            if (this.m_FrameCount == 0)
            {
                this.m_FrameCount = 1;
                this.m_Animation[0] = 0;
            }
            if (this.m_Delay == 0)
            {
                this.m_Delay = 1;
            }
        }

        public AnimatedItemEffect(Item Source, int ItemID, IHue Hue, int duration) : this(ItemID, Hue, duration)
        {
            base.SetSource(Source);
        }

        public AnimatedItemEffect(Mobile Source, int ItemID, IHue Hue, int duration) : this(ItemID, Hue, duration)
        {
            base.SetSource(Source);
        }

        public AnimatedItemEffect(int Source, int ItemID, IHue Hue, int duration) : this(Source, 0, 0, 0, ItemID, Hue, duration)
        {
        }

        public AnimatedItemEffect(int xSource, int ySource, int zSource, int ItemID, IHue Hue, int duration) : this(ItemID, Hue, duration)
        {
            base.SetSource(xSource, ySource, zSource);
        }

        public AnimatedItemEffect(int Source, int xSource, int ySource, int zSource, int ItemID, IHue Hue, int duration) : this(ItemID, Hue, duration)
        {
            Mobile source = World.FindMobile(Source);
            if (source != null)
            {
                base.SetSource(source);
                if ((!source.Player && !source.IsMoving) && (((xSource != 0) || (ySource != 0)) || (zSource != 0)))
                {
                    source.SetLocation((short) xSource, (short) ySource, (short) zSource);
                    source.Update();
                    source.UpdateReal();
                }
            }
            else
            {
                Item item = World.FindItem(Source);
                if (item != null)
                {
                    base.SetSource(item);
                    if (((xSource != 0) || (ySource != 0)) || (zSource != 0))
                    {
                        item.SetLocation((short) xSource, (short) ySource, (short) zSource);
                        item.Update();
                    }
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

        public override void OnStop()
        {
            base.VCPool.ReleaseInstance(this.m_vCache);
            this.m_vCache = null;
        }

        public override bool Slice()
        {
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
            int num10 = (Engine.GameWidth >> 1) + ((x - y) * 0x16);
            int num11 = (((Engine.GameHeight >> 1) + 0x16) + ((x + y) * 0x16)) - (z * 4);
            num10 += Engine.GameX;
            num11 += Engine.GameY;
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
            num10 -= Renderer.m_xScroll;
            num11 -= Renderer.m_yScroll;
            Texture t = null;
            IHue hue = Renderer.m_Dead ? Hues.Grayscale : this.m_Hue;
            if (this.m_Animated)
            {
                if ((Renderer.m_Frames - this.m_Start) >= this.m_Duration)
                {
                    return false;
                }
                t = hue.GetItem(this.m_ItemID + this.m_Animation[((Renderer.m_Frames - this.m_Start) / this.m_Delay) % this.m_FrameCount]);
            }
            else
            {
                if ((Renderer.m_Frames - this.m_Start) >= this.m_Duration)
                {
                    return false;
                }
                t = hue.GetItem(this.m_ItemID);
            }
            if (this.m_vCache == null)
            {
                this.m_vCache = base.VCPool.GetInstance();
            }
            switch (this.m_RenderMode)
            {
                case 2:
                    Renderer.SetAlpha(1f);
                    Renderer.SetAlphaEnable(true);
                    Renderer.SetBlendType(DrawBlendType.Additive);
                    break;

                case 3:
                    Renderer.SetAlpha(1.5f);
                    Renderer.SetAlphaEnable(true);
                    Renderer.SetBlendType(DrawBlendType.Additive);
                    break;

                case 4:
                    Renderer.SetAlpha(0.5f);
                    Renderer.SetAlphaEnable(true);
                    Renderer.SetBlendType(DrawBlendType.Normal);
                    break;
            }
            this.m_vCache.DrawGame(t, num10 - (t.Width / 2), num11 - t.Height);
            switch (this.m_RenderMode)
            {
                case 2:
                case 3:
                case 4:
                    Renderer.SetAlphaEnable(false);
                    Renderer.SetBlendType(DrawBlendType.Normal);
                    break;
            }
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

