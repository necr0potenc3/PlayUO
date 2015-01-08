namespace Client
{
    using System;

    public class SortLock : ILocked
    {
        private int m_X;
        private int m_Y;

        public SortLock(int x, int y)
        {
            this.m_X = x;
            this.m_Y = y;
        }

        public void Invoke()
        {
            Map.Sort(this.m_X, this.m_Y);
        }
    }
}

