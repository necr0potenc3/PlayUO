namespace Client
{
    public class PStringQueryCancel : Packet
    {
        public PStringQueryCancel(int Serial, short Type) : base(0xac, "String Query Cancel")
        {
            base.m_Stream.Write(Serial);
            base.m_Stream.Write(Type);
            base.m_Stream.Write(false);
            base.m_Stream.Write((short)1);
            base.m_Stream.Write((byte)0);
        }
    }
}