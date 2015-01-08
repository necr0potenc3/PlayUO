namespace Client
{
    using System;

    public class PSetLanguage : Packet
    {
        public PSetLanguage() : base(0xbf, "Set Language")
        {
            base.m_Stream.Write((short) 11);
            base.m_Stream.Write(Localization.Language, 4);
        }
    }
}

