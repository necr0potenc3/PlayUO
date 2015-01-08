namespace Client
{
    using System;

    public class DragEffect : MovingEffect
    {
        protected bool m_Double;
        private VertexCache m_vCache;
        private VertexCache m_vCacheDouble;

        public DragEffect(int itemID, int sourceSerial, int xSource, int ySource, int zSource, int targetSerial, int xTarget, int yTarget, int zTarget, IHue hue, bool shouldDouble) : base(sourceSerial, targetSerial, xSource, ySource, zSource, xTarget, yTarget, zTarget, itemID, hue)
        {
            this.m_Double = shouldDouble;
        }

        public override void OnStart()
        {
            base.m_Start = Renderer.m_Frames;
            base.m_Sync = new TimeSync(0.25);
        }

        public override void OnStop()
        {
            base.VCPool.ReleaseInstance(this.m_vCache);
            this.m_vCache = null;
            base.VCPool.ReleaseInstance(this.m_vCache);
            this.m_vCacheDouble = null;
        }

        public override bool Slice()
        {
            double normalized = base.m_Sync.Normalized;
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
            int num12 = ((Engine.ScreenHeight >> 1) + ((x + y) * 0x16)) - (z * 4);
            num11 += Engine.GameX;
            num12 += Engine.GameY;
            num11 -= Renderer.m_xScroll;
            num12 -= Renderer.m_yScroll;
            if ((base.m_Source != null) && (base.m_Source.GetType() == typeof(Mobile)))
            {
                Mobile source = (Mobile) base.m_Source;
                if (source.Walking.Count > 0)
                {
                    WalkAnimation animation = (WalkAnimation) source.Walking.Peek();
                    if (animation.Snapshot(ref xOffset, ref yOffset, ref fOffset))
                    {
                        num11 += xOffset;
                        num12 += yOffset;
                    }
                }
                num12 -= 30;
            }
            base.GetTarget(out x, out y, out z);
            x -= xWorld;
            y -= yWorld;
            z -= zWorld;
            int num13 = (Engine.GameWidth >> 1) + ((x - y) * 0x16);
            int num14 = ((Engine.GameHeight >> 1) + ((x + y) * 0x16)) - (z * 4);
            num13 += Engine.GameX;
            num14 += Engine.GameY;
            num13 -= Renderer.m_xScroll;
            num14 -= Renderer.m_yScroll;
            if ((base.m_Target != null) && (base.m_Target.GetType() == typeof(Mobile)))
            {
                Mobile target = (Mobile) base.m_Target;
                if (target.Walking.Count > 0)
                {
                    WalkAnimation animation2 = (WalkAnimation) target.Walking.Peek();
                    if (animation2.Snapshot(ref xOffset, ref yOffset, ref fOffset))
                    {
                        num13 += xOffset;
                        num14 += yOffset;
                    }
                }
                num14 -= 30;
            }
            Texture t = null;
            if (base.m_Animated)
            {
                if (Renderer.m_Dead)
                {
                    t = Hues.Grayscale.GetItem(base.m_ItemID + base.m_Animation[((Renderer.m_Frames - base.m_Start) / base.m_Delay) % base.m_FrameCount]);
                }
                else
                {
                    t = base.m_Hue.GetItem(base.m_ItemID + base.m_Animation[((Renderer.m_Frames - base.m_Start) / base.m_Delay) % base.m_FrameCount]);
                }
            }
            else if (Renderer.m_Dead)
            {
                t = Hues.Grayscale.GetItem(base.m_ItemID);
            }
            else
            {
                t = base.m_Hue.GetItem(base.m_ItemID);
            }
            if (base.m_Source == null)
            {
                num11 -= t.Width / 2;
                num12 += 0x16 - t.Height;
            }
            else
            {
                num11 -= t.xMin + ((t.xMax - t.xMin) / 2);
                num12 -= t.yMin + ((t.yMax - t.yMin) / 2);
            }
            if (base.m_Target == null)
            {
                num13 -= t.Width / 2;
                num14 += 0x16 - t.Height;
            }
            else
            {
                num13 -= t.xMin + ((t.xMax - t.xMin) / 2);
                num14 -= t.yMin + ((t.yMax - t.yMin) / 2);
            }
            int num15 = num11 + ((int) ((num13 - num11) * normalized));
            int num16 = num12 + ((int) ((num14 - num12) * normalized));
            if (this.m_vCache == null)
            {
                this.m_vCache = base.VCPool.GetInstance();
            }
            this.m_vCache.DrawGame(t, num15, num16);
            if (this.m_Double)
            {
                if (this.m_vCacheDouble == null)
                {
                    this.m_vCacheDouble = base.VCPool.GetInstance();
                }
                this.m_vCacheDouble.DrawGame(t, num15 + 5, num16 + 5);
            }
            return true;
        }
    }
}

