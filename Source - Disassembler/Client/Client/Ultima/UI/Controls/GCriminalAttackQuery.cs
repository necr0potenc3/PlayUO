namespace Client
{
    public class GCriminalAttackQuery : GMessageBoxYesNo
    {
        private Mobile m_Mobile;

        public GCriminalAttackQuery(Mobile m) : base("This may flag\nyou criminal!", true, null)
        {
            this.m_Mobile = m;
        }

        protected override void OnSignal(bool response)
        {
            if (response)
            {
                this.m_Mobile.Attack();
            }
        }
    }
}