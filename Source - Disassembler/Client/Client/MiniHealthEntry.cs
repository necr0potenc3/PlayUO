namespace Client
{
    using System;
    using System.Collections;

    public class MiniHealthEntry
    {
        public Mobile m_Mobile;
        private static Queue m_Pool;
        public int m_X;
        public int m_Y;

        private MiniHealthEntry(int x, int y, Mobile m)
        {
            this.m_X = x;
            this.m_Y = y;
            this.m_Mobile = m;
        }

        public void Dispose()
        {
            m_Pool.Enqueue(this);
        }

        public static MiniHealthEntry PoolInstance(int x, int y, Mobile m)
        {
            if (m_Pool == null)
            {
                m_Pool = new Queue();
            }
            if (m_Pool.Count > 0)
            {
                MiniHealthEntry entry = (MiniHealthEntry) m_Pool.Dequeue();
                entry.m_X = x;
                entry.m_Y = y;
                entry.m_Mobile = m;
                return entry;
            }
            return new MiniHealthEntry(x, y, m);
        }
    }
}

