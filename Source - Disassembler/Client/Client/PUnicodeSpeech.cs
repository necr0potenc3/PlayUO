namespace Client
{
    using System.Text;

    public class PUnicodeSpeech : Packet
    {
        public PUnicodeSpeech(string toSay) : this(toSay, true)
        {
        }

        public PUnicodeSpeech(string ToSay, bool encode) : base(0xad, "Unicode Speech")
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
            SpeechEntry[] keywords = Strings.GetKeywords(ToSay);
            if (encode && (keywords.Length > 0))
            {
                toWrite = (byte)(toWrite | 0xc0);
            }
            ToSay = Engine.ConvertFont(ToSay);
            base.m_Stream.Write(toWrite);
            base.m_Stream.Write((short)textHue);
            base.m_Stream.Write((short)3);
            base.m_Stream.Write(Localization.Language, 4);
            if (!encode || (keywords.Length <= 0))
            {
                base.m_Stream.WriteUnicode(ToSay);
                base.m_Stream.Write((short)0);
            }
            else
            {
                base.m_Stream.Write((byte)(keywords.Length >> 4));
                int num3 = keywords.Length & 15;
                bool flag = false;
                int index = 0;
                while (index < keywords.Length)
                {
                    SpeechEntry entry = keywords[index];
                    int keywordID = entry.m_KeywordID;
                    if (flag)
                    {
                        base.m_Stream.Write((byte)(keywordID >> 4));
                        num3 = keywordID & 15;
                    }
                    else
                    {
                        base.m_Stream.Write((byte)((num3 << 4) | ((keywordID >> 8) & 15)));
                        base.m_Stream.Write((byte)keywordID);
                    }
                    index++;
                    flag = !flag;
                }
                if (!flag)
                {
                    base.m_Stream.Write((byte)(num3 << 4));
                }
                base.m_Stream.Write(Encoding.UTF8.GetBytes(ToSay));
                base.m_Stream.Write((byte)0);
            }
        }
    }
}