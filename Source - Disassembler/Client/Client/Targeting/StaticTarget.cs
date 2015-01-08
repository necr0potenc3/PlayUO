namespace Client.Targeting
{
    using Client;
    using System;

    public class StaticTarget : IPoint3D, IPoint2D
    {
        private IHue m_Hue;
        private int m_ID;
        private int m_RealID;
        private int m_X;
        private int m_Y;
        private int m_Z;

        public StaticTarget(int x, int y, int z, int id, int realID, IHue hue)
        {
            this.m_X = x;
            this.m_Y = y;
            this.m_Z = z;
            this.m_ID = id;
            this.m_RealID = realID;
            this.m_Hue = hue;
        }

        public IHue Hue
        {
            get
            {
                return this.m_Hue;
            }
        }

        public int ID
        {
            get
            {
                return this.m_ID;
            }
        }

        public int RealID
        {
            get
            {
                return this.m_RealID;
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

        public int Z
        {
            get
            {
                return this.m_Z;
            }
        }
    }
}

