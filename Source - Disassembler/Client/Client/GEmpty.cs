namespace Client
{
    using System;

    public class GEmpty : Gump
    {
        private int m_Height;
        private int m_Width;

        public GEmpty() : base(0, 0)
        {
        }

        public GEmpty(int X, int Y) : base(X, Y)
        {
        }

        public GEmpty(int X, int Y, int Width, int Height) : base(X, Y)
        {
            this.m_Width = Width;
            this.m_Height = Height;
        }

        public override int Height
        {
            get
            {
                return this.m_Height;
            }
            set
            {
                this.m_Height = value;
            }
        }

        public override int Width
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

