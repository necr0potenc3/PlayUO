namespace Client
{
    using System;

    public class PUnknownLogin : Packet
    {
        public PUnknownLogin() : base(0xbf, "Unknown Login")
        {
            base.m_Stream.Write((short) 15);
            base.m_Stream.Write((byte) 10);
            base.m_Stream.Write(0x11f);
        }
    }
}

