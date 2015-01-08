namespace Client
{
    using System;

    public class JournalEntry
    {
        private IHue m_Hue;
        private Texture m_Image;
        private int m_Serial;
        private string m_Text;
        private DateTime m_Time;
        private int m_Width;

        public JournalEntry(string text, IHue hue, int serial)
        {
            this.m_Text = text;
            this.m_Hue = hue;
            this.m_Serial = serial;
            this.m_Time = DateTime.Now;
        }

        public IHue Hue
        {
            get
            {
                return this.m_Hue;
            }
        }

        public Texture Image
        {
            get
            {
                return this.m_Image;
            }
            set
            {
                this.m_Image = value;
            }
        }

        public int Serial
        {
            get
            {
                return this.m_Serial;
            }
        }

        public string Text
        {
            get
            {
                return this.m_Text;
            }
            set
            {
                this.m_Text = value;
            }
        }

        public DateTime Time
        {
            get
            {
                return this.m_Time;
            }
        }

        public int Width
        {
            get
            {
                return this.m_Width;
            }
            set
            {
                this.m_Width = value;
            }
        }
    }
}

