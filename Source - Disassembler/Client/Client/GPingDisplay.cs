namespace Client
{
    using System;

    public class GPingDisplay : GLabel
    {
        private IHue[] m_Hues;

        public GPingDisplay() : base("", Engine.DefaultFont, Engine.DefaultHue, 4, 4)
        {
            this.m_Hues = new IHue[] { Hues.Load(0x44), Hues.Load(0x3f), Hues.Load(0x3a), Hues.Load(0x35), Hues.Load(0x30), Hues.Load(0x2b), Hues.Load(0x26) };
        }

        protected internal override void Render(int X, int Y)
        {
            if (Engine.m_Ingame && Renderer.DrawPing)
            {
                int ping = Engine.Ping;
                string str = ((ping / 5) * 5).ToString();
                int index = 0;
                index = (ping - 0x19) / 0x4b;
                if (index < 0)
                {
                    index = 0;
                }
                else if (index > 6)
                {
                    index = 6;
                }
                if (ping < 5)
                {
                    str = "below 5";
                }
                else if (ping > 0x1388)
                {
                    str = "over 5000";
                }
                this.Hue = this.m_Hues[index];
                this.Text = string.Format("Ping: {0}", str);
                base.Render(X, Y);
                Stats.Add(this);
            }
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

