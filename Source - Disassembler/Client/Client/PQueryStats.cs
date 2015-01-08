namespace Client
{
    using System;

    public class PQueryStats : Packet
    {
        public PQueryStats(int Serial) : base(0x34, "Query Stats", 10)
        {
            base.m_Stream.Write(-303174163);
            base.m_Stream.Write((byte) 4);
            base.m_Stream.Write(Serial);
        }
    }
}

