namespace Client
{
    using System;

    public class PChatOpen : Packet
    {
        public PChatOpen(string un) : base(0xb5, "Chat Open", 0x40)
        {
            base.m_Stream.Write((byte) 1);
            if (un.Length > 0x1f)
            {
                un = un.Substring(0, 0x1f);
            }
            else if (un.Length < 0x1f)
            {
                un = un + new string('\0', 0x1f - un.Length);
            }
            base.m_Stream.WriteUnicode(un);
        }
    }
}

