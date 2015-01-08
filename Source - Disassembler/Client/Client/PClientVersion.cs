namespace Client
{
    using System;

    public class PClientVersion : Packet
    {
        public PClientVersion(string version) : base(0xbd, "Client Version")
        {
            base.m_Stream.Write(version);
            base.m_Stream.Write((byte) 0);
        }
    }
}

