namespace Client
{
    using System;

    public class BBMessageBody
    {
        private BBPosterAppearance m_Appearance;
        private string[] m_Lines;

        public BBMessageBody(BBPosterAppearance appearance, string[] lines)
        {
            this.m_Appearance = appearance;
            this.m_Lines = lines;
        }

        public BBPosterAppearance Appearance
        {
            get
            {
                return this.m_Appearance;
            }
        }

        public string[] Lines
        {
            get
            {
                return this.m_Lines;
            }
        }
    }
}

