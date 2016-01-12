namespace Client
{
    public class PGameLogin : Packet
    {
        public PGameLogin(int AuthID, string un, string pw) : base(0x91, "Game Server Login", 0x41)
        {
            base.m_Stream.Write(AuthID);
            base.m_Stream.Write(un, 30);
            base.m_Stream.Write(pw, 30);
        }
    }
}