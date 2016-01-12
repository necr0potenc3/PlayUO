namespace Client
{
    public class Stats
    {
        private static int m_yOffset;

        public static void Add(Gump g)
        {
            m_yOffset += g.Height + 2;
        }

        public static void Reset()
        {
            m_yOffset = 4;
        }

        public static int yOffset
        {
            get
            {
                return m_yOffset;
            }
        }
    }
}