namespace Client
{
    public class EndDeathEffect : Fade
    {
        private GLabel m_Label;

        public EndDeathEffect(GLabel lbl) : base(0, 1f, 0f, 1f)
        {
            this.m_Label = lbl;
        }

        protected internal override void OnFadeComplete()
        {
            Gumps.Destroy(this.m_Label);
        }
    }
}