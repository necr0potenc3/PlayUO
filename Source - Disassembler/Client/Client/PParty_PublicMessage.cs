namespace Client
{
    using System;

    public class PParty_PublicMessage : Packet
    {
        public PParty_PublicMessage(string text) : base(0xbf, "Public Party Message")
        {
            base.m_Stream.Write((short) 6);
            base.m_Stream.Write((byte) 4);
            base.m_Stream.WriteUnicode(text);
            base.m_Stream.Write((short) 0);
        }
    }
}

