namespace Client
{
    using System;

    public class PParty_Quit : Packet
    {
        public PParty_Quit() : base(0xbf, "Quit Party")
        {
            base.m_Stream.Write((short) 6);
            base.m_Stream.Write((byte) 2);
            base.m_Stream.Write(World.Serial);
        }
    }
}

