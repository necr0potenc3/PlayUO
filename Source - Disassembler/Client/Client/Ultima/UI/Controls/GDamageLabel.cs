namespace Client
{
    public class GDamageLabel : GLabel
    {
        private Mobile m_Mobile;
        private TimeSync m_Sync;
        private Timer m_Timer;

        public GDamageLabel(int damage, Mobile m) : base(damage.ToString(), Engine.DefaultFont, Hues.Load(m.Flags[MobileFlag.Poisoned] ? 0x3f : 0x26), m.ScreenX, m.ScreenY - 30)
        {
            this.m_Mobile = m;
            this.m_Sync = new TimeSync(0.75);
            this.UpdatePosition();
        }

        private void Delete_OnTick(Timer t)
        {
            Gumps.Destroy(this);
        }

        protected internal override void Render(int X, int Y)
        {
            this.UpdatePosition();
            base.Render(X, Y);
        }

        public void UpdatePosition()
        {
            double normalized = this.m_Sync.Normalized;
            if (normalized >= 1.0)
            {
                if (this.m_Timer == null)
                {
                    this.m_Timer = new Timer(new OnTick(this.Delete_OnTick), 0, 1);
                    this.m_Timer.Start(false);
                }
            }
            else
            {
                if (normalized >= 0.5)
                {
                    this.Alpha = (float)(1.0 - ((normalized - 0.5) * 2.0));
                }
                int xWorld = Renderer.m_xWorld;
                int yWorld = Renderer.m_yWorld;
                int zWorld = Renderer.m_zWorld;
                int x = this.m_Mobile.X;
                int y = this.m_Mobile.Y;
                int z = this.m_Mobile.Z;
                x -= xWorld;
                y -= yWorld;
                z -= zWorld;
                int num8 = (Engine.GameWidth >> 1) + ((x - y) * 0x16);
                int num9 = (((Engine.GameHeight >> 1) + 0x16) + ((x + y) * 0x16)) - (z * 4);
                num8 += Engine.GameX;
                num9 += Engine.GameY;
                if (this.m_Mobile.Walking.Count > 0)
                {
                    WalkAnimation animation = (WalkAnimation)this.m_Mobile.Walking.Peek();
                    int xOffset = 0;
                    int yOffset = 0;
                    int fOffset = 0;
                    if (animation.Snapshot(ref xOffset, ref yOffset, ref fOffset))
                    {
                        num8 += xOffset;
                        num9 += yOffset;
                    }
                }
                num8 -= Renderer.m_xScroll;
                num9 -= Renderer.m_yScroll;
                this.X = (num8 - (((base.Image.xMax - base.Image.xMin) + 1) / 2)) - base.Image.xMin;
                this.Y = (num9 - 30) - ((int)(normalized * 40.0));
                base.Scissor(new Clipper(Engine.GameX, Engine.GameY, Engine.GameWidth, Engine.GameHeight));
            }
        }
    }
}