namespace Client
{
    using System;

    public class PParty_Accept : Packet
    {
        public PParty_Accept(Mobile req) : base(0xbf, "Party Join Accept")
        {
            base.m_Stream.Write((short) 6);
            base.m_Stream.Write((byte) 8);
            base.m_Stream.Write(req.Serial);
        }
    }
}

