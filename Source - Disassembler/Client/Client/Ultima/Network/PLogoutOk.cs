namespace Client
{
    public class PLogoutOk : Packet
    {
        public PLogoutOk() : base(0xd1, "Logout Ok", 2)
        {
            base.m_Stream.Write((byte)1);
        }
    }
}