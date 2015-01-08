namespace Client
{
    using System;

    public class BBPosterAppearance
    {
        private int m_Body;
        private int m_Hue;
        private BBPAItem[] m_Items;

        public BBPosterAppearance(int body, int hue, BBPAItem[] items)
        {
            this.m_Body = body;
            this.m_Hue = hue;
            this.m_Items = items;
        }

        public int Body
        {
            get
            {
                return this.m_Body;
            }
        }

        public int Hue
        {
            get
            {
                return this.m_Hue;
            }
        }

        public BBPAItem[] Items
        {
            get
            {
                return this.m_Items;
            }
        }
    }
}

