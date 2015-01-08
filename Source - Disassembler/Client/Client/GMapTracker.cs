namespace Client
{
    using System;

    public class GMapTracker : GTracker
    {
        private static int m_MapX;
        private static int m_MapY;

        protected override string GetPluralString(string direction, int distance)
        {
            return ("Treasure: " + distance.ToString() + " tiles " + direction);
        }

        protected override string GetSingularString(string direction)
        {
            return ("Treasure: 1 tile " + direction);
        }

        protected internal override void Render(int X, int Y)
        {
            if ((m_MapX != 0) || (m_MapY != 0))
            {
                base.Render(X, Y, m_MapX, m_MapY);
            }
        }

        public static int MapX
        {
            get
            {
                return m_MapX;
            }
            set
            {
                m_MapX = value;
            }
        }

        public static int MapY
        {
            get
            {
                return m_MapY;
            }
            set
            {
                m_MapY = value;
            }
        }
    }
}

