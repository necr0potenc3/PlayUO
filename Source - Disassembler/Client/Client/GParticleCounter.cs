namespace Client
{
    using System;

    public class GParticleCounter : GLabel
    {
        public GParticleCounter() : base("", Engine.DefaultFont, Engine.DefaultHue, 4, 4)
        {
        }

        protected internal override void Render(int X, int Y)
        {
            if (Engine.m_Ingame && Renderer.DrawPCount)
            {
                this.Text = string.Format("Particles: {0}", Engine.Effects.ParticleCount);
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

