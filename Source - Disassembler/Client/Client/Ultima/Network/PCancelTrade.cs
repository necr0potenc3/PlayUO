namespace Client
{
    public class PCancelTrade : Packet
    {
        public PCancelTrade(int Serial) : base(0x6f, "Cancel Trade")
        {
            base.m_Stream.Write((byte)1);
            base.m_Stream.Write(Serial);
            base.m_Stream.Write(0);
            base.m_Stream.Write(0);
            base.m_Stream.Write((byte)0);
        }
    }
}