namespace Client
{
    using System;

    public class PMultiTarget_Cancel : Packet
    {
        public PMultiTarget_Cancel(int targetID) : base(0x6c, "Multi Target Cancel", 0x13)
        {
            base.m_Stream.Write((byte) 0);
            base.m_Stream.Write(targetID);
            base.m_Stream.Write((byte) 0);
            base.m_Stream.Write(0);
            base.m_Stream.Write(-1);
            base.m_Stream.Write(0);
        }
    }
}

