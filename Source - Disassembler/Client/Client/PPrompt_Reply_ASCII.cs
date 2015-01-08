namespace Client
{
    using System;

    public class PPrompt_Reply_ASCII : Packet
    {
        public PPrompt_Reply_ASCII(int serial, int prompt, string message) : base(0x9a, "ASCII Prompt Reply")
        {
            base.m_Stream.Write(serial);
            base.m_Stream.Write(prompt);
            base.m_Stream.Write(1);
            base.m_Stream.Write(message);
            base.m_Stream.Write((byte) 0);
        }
    }
}

