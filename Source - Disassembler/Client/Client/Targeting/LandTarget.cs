namespace Client.Targeting
{
    using Client;

    public class LandTarget : IPoint3D, IPoint2D
    {
        private int m_X;
        private int m_Y;
        private int m_Z;

        public LandTarget(int x, int y, int z)
        {
            this.m_X = x;
            this.m_Y = y;
            this.m_Z = z;
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

        public int Z
        {
            get
            {
                return this.m_Z;
            }
        }
    }
}