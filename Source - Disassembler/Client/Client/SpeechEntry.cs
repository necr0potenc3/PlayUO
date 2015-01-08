namespace Client
{
    using System;

    public class SpeechEntry : IComparable
    {
        public short m_KeywordID;
        public string[] m_Keywords;

        public SpeechEntry(int idKeyword, string keyword)
        {
            this.m_KeywordID = (short) idKeyword;
            this.m_Keywords = keyword.Split(new char[] { '*' });
        }

        public int CompareTo(object x)
        {
            if ((x == null) || (x.GetType() != typeof(SpeechEntry)))
            {
                return -1;
            }
            if (x != this)
            {
                SpeechEntry entry = (SpeechEntry) x;
                if (this.m_KeywordID < entry.m_KeywordID)
                {
                    return -1;
                }
                if (this.m_KeywordID > entry.m_KeywordID)
                {
                    return 1;
                }
            }
            return 0;
        }
    }
}

