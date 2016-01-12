namespace Client
{
    using System;

    public class MovingEffect : Effect
    {
        protected bool m_Animated;
        protected sbyte[] m_Animation;
        protected int m_Delay;
        protected int m_FrameCount;
        protected IHue m_Hue;
        protected int m_ItemID;
        public int m_RenderMode;
        protected int m_Start;
        protected TimeSync m_Sync;

        public MovingEffect(int ItemID, IHue Hue)
        {
            base.m_Children = new EffectList();
            this.m_Hue = Hue;
            ItemID &= 0x3fff;
            this.m_ItemID = ItemID | 0x4000;
            this.m_Animated = Map.m_ItemFlags[ItemID][TileFlag.Animation];
            if (this.m_Animated)
            {
                AnimData anim = Map.GetAnim(ItemID);
                this.m_FrameCount = anim.frameCount;
                this.m_Delay = anim.frameInterval;
                this.m_Animation = new sbyte[0x40];
                for (int i = 0; i < 0x40; i++)
                {
                    this.m_Animation[i] = anim[i];
                }
                if (this.m_Delay == 0)
                {
                    this.m_Delay = 1;
                }
                this.m_Animated = this.m_FrameCount > 0;
            }
        }

        public MovingEffect(Item Source, Item Target, int ItemID, IHue Hue) : this(ItemID, Hue)
        {
            base.SetSource(Source);
            base.SetTarget(Target);
        }

        public MovingEffect(Item Source, Mobile Target, int ItemID, IHue Hue) : this(ItemID, Hue)
        {
            base.SetSource(Source);
            base.SetTarget(Target);
        }

        public MovingEffect(Mobile Source, Item Target, int ItemID, IHue Hue) : this(ItemID, Hue)
        {
            base.SetSource(Source);
            base.SetTarget(Target);
        }

        public MovingEffect(Mobile Source, Mobile Target, int ItemID, IHue Hue) : this(ItemID, Hue)
        {
            base.SetSource(Source);
            base.SetTarget(Target);
        }

        public MovingEffect(Item Source, int xTarget, int yTarget, int zTarget, int ItemID, IHue Hue) : this(ItemID, Hue)
        {
            base.SetSource(Source);
            base.SetTarget(xTarget, yTarget, zTarget);
        }

        public MovingEffect(Mobile Source, int xTarget, int yTarget, int zTarget, int ItemID, IHue Hue) : this(ItemID, Hue)
        {
            base.SetSource(Source);
            base.SetTarget(xTarget, yTarget, zTarget);
        }

        public MovingEffect(int xSource, int ySource, int zSource, Item Target, int ItemID, IHue Hue) : this(ItemID, Hue)
        {
            base.SetSource(xSource, ySource, zSource);
            base.SetTarget(Target);
        }

        public MovingEffect(int xSource, int ySource, int zSource, Mobile Target, int ItemID, IHue Hue) : this(ItemID, Hue)
        {
            base.SetSource(xSource, ySource, zSource);
            base.SetTarget(Target);
        }

        public MovingEffect(int xSource, int ySource, int zSource, int xTarget, int yTarget, int zTarget, int ItemID, IHue Hue) : this(ItemID, Hue)
        {
            base.SetSource(xSource, ySource, zSource);
            base.SetTarget(xTarget, yTarget, zTarget);
        }

        public MovingEffect(int Source, int Target, int xSource, int ySource, int zSource, int xTarget, int yTarget, int zTarget, int ItemID, IHue Hue) : this(ItemID, Hue)
        {
            Mobile source = World.FindMobile(Source);
            if (source != null)
            {
                base.SetSource(source);
                if ((!source.Player && !source.IsMoving) && (((xSource != 0) || (ySource != 0)) || (zSource != 0)))
                {
                    source.SetLocation((short)xSource, (short)ySource, (short)zSource);
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
                        item.SetLocation((short)xSource, (short)ySource, (short)zSource);
                        item.Update();
                    }
                }
                else
                {
                    base.SetSource(xSource, ySource, zSource);
                }
            }
            source = World.FindMobile(Target);
            if (source != null)
            {
                base.SetTarget(source);
                if ((!source.Player && !source.IsMoving) && (((xTarget != 0) || (yTarget != 0)) || (zTarget != 0)))
                {
                    source.SetLocation((short)xTarget, (short)yTarget, (short)zTarget);
                    source.Update();
                    source.UpdateReal();
                }
            }
            else
            {
                Item target = World.FindItem(Target);
                if (target != null)
                {
                    base.SetTarget(target);
                    if (((xTarget != 0) || (yTarget != 0)) || (zTarget != 0))
                    {
                        target.SetLocation((short)xTarget, (short)yTarget, (short)zTarget);
                        target.Update();
                    }
                }
                else
                {
                    base.SetTarget(xTarget, yTarget, zTarget);
                }
            }
        }

        public override void OnStart()
        {
            this.m_Start = Renderer.m_Frames;
            this.m_Sync = new TimeSync(0.25);
        }

        public override bool Slice()
        {
            double normalized = this.m_Sync.Normalized;
            if (normalized >= 1.0)
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
            int num11 = (Engine.GameWidth >> 1) + ((x - y) * 0x16);
            int num12 = ((Engine.GameHeight >> 1) + ((x + y) * 0x16)) - (z * 4);
            if (base.m_Source is Mobile)
            {
                num12 -= 30;
            }
            num11 += Engine.GameX;
            num12 += Engine.GameY;
            num11 -= Renderer.m_xScroll;
            num12 -= Renderer.m_yScroll;
            if ((base.m_Source != null) && (base.m_Source.GetType() == typeof(Mobile)))
            {
                Mobile source = (Mobile)base.m_Source;
                if (source.Walking.Count > 0)
                {
                    WalkAnimation animation = (WalkAnimation)source.Walking.Peek();
                    if (animation.Snapshot(ref xOffset, ref yOffset, ref fOffset))
                    {
                        num11 += xOffset;
                        num12 += yOffset;
                    }
                }
            }
            base.GetTarget(out x, out y, out z);
            x -= xWorld;
            y -= yWorld;
            z -= zWorld;
            int num13 = (Engine.GameWidth >> 1) + ((x - y) * 0x16);
            int num14 = ((Engine.GameHeight >> 1) + ((x + y) * 0x16)) - (z * 4);
            if (base.m_Target is Mobile)
            {
                num14 -= 50;
            }
            num13 += Engine.GameX;
            num14 += Engine.GameY;
            num13 -= Renderer.m_xScroll;
            num14 -= Renderer.m_yScroll;
            if ((base.m_Target != null) && (base.m_Target.GetType() == typeof(Mobile)))
            {
                Mobile target = (Mobile)base.m_Target;
                if (target.Walking.Count > 0)
                {
                    WalkAnimation animation2 = (WalkAnimation)target.Walking.Peek();
                    if (animation2.Snapshot(ref xOffset, ref yOffset, ref fOffset))
                    {
                        num13 += xOffset;
                        num14 += yOffset;
                    }
                }
            }
            Texture texture = null;
            IHue hue = Renderer.m_Dead ? Hues.Grayscale : this.m_Hue;
            if (this.m_Animated)
            {
                texture = hue.GetItem(this.m_ItemID + this.m_Animation[((Renderer.m_Frames - this.m_Start) / this.m_Delay) % this.m_FrameCount]);
            }
            else
            {
                texture = hue.GetItem(this.m_ItemID);
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
            int num15 = num11 + ((int)((num13 - num11) * normalized));
            int num16 = num12 + ((int)((num14 - num12) * normalized));
            texture.DrawRotated(num15, num16, Math.Atan2((double)(num12 - num14), (double)(num11 - num13)));
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