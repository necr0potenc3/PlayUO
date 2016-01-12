namespace Client
{
    public class GQuestArrow : GTracker
    {
        private static bool m_Active;
        private static int m_ArrowX;
        private static int m_ArrowY;

        public static void Activate(int x, int y)
        {
            m_Active = true;
            m_ArrowX = x;
            m_ArrowY = y;
        }

        protected override string GetPluralString(string direction, int distance)
        {
            return ("Target: " + distance.ToString() + " tiles " + direction);
        }

        protected override string GetSingularString(string direction)
        {
            return ("Target: 1 tile " + direction);
        }

        protected internal override void Render(int X, int Y)
        {
            if (m_Active)
            {
                base.Render(X, Y, m_ArrowX, m_ArrowY);
            }
        }

        public static void Stop()
        {
            m_Active = false;
        }
    }
}