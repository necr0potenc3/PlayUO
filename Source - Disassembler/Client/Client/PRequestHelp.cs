namespace Client
{
    using System;

    public class PRequestHelp : Packet
    {
        public PRequestHelp() : base(0x9b, "Request Help", 0x102)
        {
            base.m_Stream.Write(new byte[0x101]);
        }
    }
}

