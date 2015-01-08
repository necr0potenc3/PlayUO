namespace Client
{
    using System;

    public class PPrompt_Cancel_Unicode : Packet
    {
        public PPrompt_Cancel_Unicode(int serial, int prompt) : base(0xc2, "Unicode Prompt Cancel")
        {
            base.m_Stream.Write(serial);
            base.m_Stream.Write(prompt);
            base.m_Stream.Write(0);
            base.m_Stream.Write(Localization.Language, 4);
        }
    }
}

