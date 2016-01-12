namespace Client
{
    public class PUseRequest : Packet
    {
        private static IEntity m_Last;

        public PUseRequest(IEntity e) : base(6, "Use Request", 5)
        {
            base.m_Stream.Write(e.Serial);
        }

        public static void SendLast()
        {
            if (m_Last != null)
            {
                Network.Send(new PUseRequest(m_Last));
            }
        }

        public static IEntity Last
        {
            get
            {
                return m_Last;
            }
            set
            {
                m_Last = value;
            }
        }
    }
}