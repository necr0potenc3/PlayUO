namespace Client
{
    public class GVirtueTrigger : GClickable
    {
        private Mobile m_Mobile;

        public GVirtueTrigger(Mobile m) : base(80, 4, 0x71)
        {
            this.m_Mobile = m;
            base.Tooltip = new Tooltip("Virtue System");
        }

        protected override void OnDoubleClicked()
        {
            Network.Send(new PVirtueTrigger(this.m_Mobile));
        }
    }
}