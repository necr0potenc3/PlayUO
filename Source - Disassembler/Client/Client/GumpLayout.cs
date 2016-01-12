namespace Client
{
    public class GumpLayout
    {
        private int m_Extra;
        private int m_Height;
        private int m_Type;
        private int m_Width;
        private int m_X;
        private int m_Y;

        public GumpLayout(int type, int extra, int x, int y, int width, int height)
        {
            this.m_Type = type;
            this.m_Extra = extra;
            this.m_X = x;
            this.m_Y = y;
            this.m_Width = width;
            this.m_Height = height;
        }

        public int Extra
        {
            get
            {
                return this.m_Extra;
            }
        }

        public int Height
        {
            get
            {
                return this.m_Height;
            }
        }

        public int Type
        {
            get
            {
                return this.m_Type;
            }
        }

        public int Width
        {
            get
            {
                return this.m_Width;
            }
        }

        public int X
        {
            get
            {
                return this.m_X;
            }
        }

        public int Y
        {
            get
            {
                return this.m_Y;
            }
        }
    }
}