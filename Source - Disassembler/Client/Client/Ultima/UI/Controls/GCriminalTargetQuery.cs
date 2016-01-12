namespace Client
{
    using Client.Targeting;

    public class GCriminalTargetQuery : GMessageBoxYesNo
    {
        private ServerTargetHandler m_Handler;
        private Mobile m_Mobile;

        public GCriminalTargetQuery(Mobile m, ServerTargetHandler handler) : base("This may flag\nyou criminal!", true, null)
        {
            this.m_Mobile = m;
            this.m_Handler = handler;
        }

        protected override void OnSignal(bool response)
        {
            if (response)
            {
                Network.Send(new PTarget_Response(0, this.m_Handler, this.m_Mobile.Serial, this.m_Mobile.X, this.m_Mobile.Y, this.m_Mobile.Z, this.m_Mobile.Body));
            }
            else
            {
                Network.Send(new PTarget_Cancel(this.m_Handler));
            }
        }
    }
}