namespace Client
{
    public class PLookRequest : Packet
    {
        public PLookRequest(IEntity e) : base(9, "Look Request", 5)
        {
            base.m_Stream.Write(e.Serial);
        }
    }
}