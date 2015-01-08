namespace Client
{
    using System;

    public class PParty_PrivateMessage : Packet
    {
        public PParty_PrivateMessage(Mobile who, string text) : base(0xbf, "Private Party Message")
        {
            base.m_Stream.Write((short) 6);
            base.m_Stream.Write((byte) 3);
            base.m_Stream.Write(who.Serial);
            base.m_Stream.WriteUnicode(text);
            base.m_Stream.Write((short) 0);
        }
    }
}

