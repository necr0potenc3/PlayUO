namespace Client
{
    public class PCheckTrade : Packet
    {
        public PCheckTrade(Item item, bool check1, bool check2) : base(0x6f, "Check Trade")
        {
            base.m_Stream.Write((byte)2);
            base.m_Stream.Write(item.Serial);
            base.m_Stream.Write(check1 ? 1 : 0);
            base.m_Stream.Write(check2 ? 1 : 0);
            base.m_Stream.Write((byte)0);
        }
    }
}