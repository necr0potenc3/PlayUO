namespace Client
{
    using System;

    public class PSpeech : Packet
    {
        public PSpeech(string ToSay) : base(3, "Speech")
        {
            byte toWrite = 0;
            int textHue = World.CharData.TextHue;
            if (ToSay.StartsWith(": "))
            {
                toWrite = 2;
                textHue = World.CharData.EmoteHue;
                ToSay = string.Format("*{0}*", ToSay.Substring(2));
            }
            else if (ToSay.StartsWith("; "))
            {
                toWrite = 8;
                textHue = World.CharData.WhisperHue;
                ToSay = ToSay.Substring(2);
            }
            else if (ToSay.StartsWith("! "))
            {
                toWrite = 9;
                textHue = World.CharData.YellHue;
                ToSay = ToSay.Substring(2);
            }
            else if (ToSay.StartsWith(@"\ "))
            {
                ToSay = string.Format("<OOC> {0}", ToSay.Substring(2));
            }
            base.m_Stream.Write(toWrite);
            base.m_Stream.Write((short) textHue);
            base.m_Stream.Write((short) 3);
            base.m_Stream.Write(ToSay);
            base.m_Stream.Write((byte) 0);
        }
    }
}

