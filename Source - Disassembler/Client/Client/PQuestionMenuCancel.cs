namespace Client
{
    using System;

    public class PQuestionMenuCancel : Packet
    {
        public PQuestionMenuCancel(int serial, int menuID) : base(0x7d, "Question Menu Cancel", 13)
        {
            base.m_Stream.Write(serial);
            base.m_Stream.Write((short) menuID);
            base.m_Stream.Write((short) 0);
            base.m_Stream.Write((short) 0);
            base.m_Stream.Write((short) 0);
        }
    }
}

