namespace Client
{
    public class PSetWarMode : Packet
    {
        public PSetWarMode(bool warMode, short unk1, byte unk2) : base(0x72, "Set War Mode", 5)
        {
            base.m_Stream.Write(warMode);
            base.m_Stream.Write(unk1);
            base.m_Stream.Write(unk2);
        }
    }
}