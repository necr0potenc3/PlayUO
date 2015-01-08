namespace Client
{
    using System;

    public class PRenameMobile : Packet
    {
        public PRenameMobile(int Serial, string Name) : base(0x75, "Rename Mobile", 0x23)
        {
            base.m_Stream.Write(Serial);
            base.m_Stream.Write(Name, 30);
        }
    }
}

