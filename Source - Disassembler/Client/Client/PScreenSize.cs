namespace Client
{
    public class PScreenSize : Packet
    {
        public PScreenSize() : base(0xbf, "Screen Size")
        {
            base.m_Stream.Write((short)5);
            base.m_Stream.Write(Engine.GameWidth);
            base.m_Stream.Write(0x1ffffa7);
        }
    }
}