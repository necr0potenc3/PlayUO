namespace Client
{
    public class PCharSelect : Packet
    {
        public PCharSelect(string charName, int charIdx) : base(0x5d, "Character Select", 0x49)
        {
            base.m_Stream.Write((uint)0xedededed);
            base.m_Stream.Write(charName, 30);
            base.m_Stream.Write((short)0);
            base.m_Stream.Write(0x11f);
            base.m_Stream.Write(0);
            base.m_Stream.Write(0x65);
            base.m_Stream.Write("", 0x10);
            base.m_Stream.Write(charIdx);
            base.m_Stream.Write(Network.ClientIP);
        }
    }
}