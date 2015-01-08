namespace Client
{
    using System;

    public class PVirtueTrigger : Packet
    {
        public PVirtueTrigger(Mobile m) : base(0xb1, "Virtue Trigger")
        {
            base.m_Stream.Write(World.Serial);
            base.m_Stream.Write(0x1cd);
            base.m_Stream.Write(1);
            base.m_Stream.Write(1);
            base.m_Stream.Write(m.Serial);
        }
    }
}

