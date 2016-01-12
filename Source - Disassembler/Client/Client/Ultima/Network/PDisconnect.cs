namespace Client
{
    public class PDisconnect : Packet
    {
        public PDisconnect() : base(1, "Disconnect", 5)
        {
            base.m_Stream.Write(-1);
        }
    }
}