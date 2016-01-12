namespace Client
{
    public class PResyncRequest : Packet
    {
        public PResyncRequest() : base(0x22, "Resync Request", 3)
        {
            base.m_Stream.Write((short)0);
        }
    }
}