namespace Client
{
    using System;

    public class Frame
    {
        public int CenterX;
        public int CenterY;
        public Texture Image;
        private static Frame m_Empty;

        public static Frame Empty
        {
            get
            {
                if (m_Empty == null)
                {
                    m_Empty = new Frame();
                    m_Empty.Image = Texture.Empty;
                }
                return m_Empty;
            }
        }
    }
}

