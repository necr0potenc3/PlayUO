namespace Client
{
    using System;

    public class AnswerEntry
    {
        private int m_Hue;
        private int m_Index;
        private int m_ItemID;
        private string m_Text;

        public AnswerEntry(int index, int itemID, int hue, string text)
        {
            this.m_Index = index;
            this.m_ItemID = itemID;
            this.m_Hue = hue;
            this.m_Text = text;
        }

        public int Hue
        {
            get
            {
                return this.m_Hue;
            }
        }

        public int Index
        {
            get
            {
                return this.m_Index;
            }
        }

        public int ItemID
        {
            get
            {
                return this.m_ItemID;
            }
        }

        public string Text
        {
            get
            {
                return this.m_Text;
            }
        }
    }
}

