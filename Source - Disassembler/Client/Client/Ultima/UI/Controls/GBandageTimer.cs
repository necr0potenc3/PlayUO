namespace Client
{
    using System;

    public class GBandageTimer : GLabel
    {
        private static bool m_Active;
        private static DateTime m_Start;

        public GBandageTimer() : base("", Engine.DefaultFont, Engine.DefaultHue, 4, 4)
        {
        }

        protected internal override void Render(int X, int Y)
        {
            if (m_Active && Engine.m_Ingame)
            {
                TimeSpan span = (TimeSpan)(DateTime.Now - m_Start);
                if (span >= TimeSpan.FromSeconds(20.0))
                {
                    m_Active = false;
                }
                else
                {
                    this.Text = string.Format("Bandage: {0} seconds elapsed", (int)span.TotalSeconds);
                    base.Render(X, Y);
                    Stats.Add(this);
                }
            }
        }

        public static void Start()
        {
            m_Active = true;
            m_Start = DateTime.Now;
        }

        public static void Stop()
        {
            m_Active = false;
        }

        public override int Y
        {
            get
            {
                return Stats.yOffset;
            }
            set
            {
            }
        }
    }
}