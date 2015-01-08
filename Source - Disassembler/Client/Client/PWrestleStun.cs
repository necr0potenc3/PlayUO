namespace Client
{
    using System;

    public class PWrestleStun : Packet
    {
        public PWrestleStun() : base(0xbf, "Wrestle Stun")
        {
            base.m_Stream.Write((short) 10);
        }
    }
}

