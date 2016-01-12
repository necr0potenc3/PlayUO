namespace Client
{
    public class PPing : Packet
    {
        public PPing(int PingID) : base(0x73, "Ping", 2)
        {
            base.m_Stream.Write((byte)PingID);
        }
    }
}