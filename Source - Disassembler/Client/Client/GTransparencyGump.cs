namespace Client
{
    public class GTransparencyGump : GLabel
    {
        public GTransparencyGump() : base("Transparency On", Engine.DefaultFont, Engine.DefaultHue, 4, 4)
        {
        }

        protected internal override void Render(int X, int Y)
        {
            if (Engine.m_Ingame && Renderer.Transparency)
            {
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