namespace Client
{
    public class PAccount : Packet
    {
        public PAccount(string un, string pw) : base(0x80, "Account Login", 0x3e)
        {
            base.m_Stream.Write(un, 30);
            base.m_Stream.Write(pw, 30);
            base.m_Stream.Write((byte)0);
        }
    }
}