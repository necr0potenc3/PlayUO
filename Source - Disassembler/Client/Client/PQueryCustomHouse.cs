namespace Client
{
    using System;

    public class PQueryCustomHouse : Packet
    {
        public PQueryCustomHouse(int serial) : base(0xbf, "Query Custom House")
        {
            base.m_Stream.Write((short) 30);
            base.m_Stream.Write(serial);
        }
    }
}

