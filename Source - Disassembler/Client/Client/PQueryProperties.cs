namespace Client
{
    using System;

    public class PQueryProperties : Packet
    {
        public PQueryProperties(int serial) : base(0xd6, "Query Properties")
        {
            base.m_Stream.Write(serial);
        }
    }
}

