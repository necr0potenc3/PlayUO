namespace Client
{
    using System;
    using System.Collections;

    public class VertexCachePool
    {
        private Queue m_Queue = new Queue();

        public VertexCache GetInstance()
        {
            if (this.m_Queue.Count > 0)
            {
                VertexCache cache = (VertexCache) this.m_Queue.Dequeue();
                cache.Invalidate();
                return cache;
            }
            return new VertexCache();
        }

        public void ReleaseInstance(VertexCache vc)
        {
            if (vc != null)
            {
                this.m_Queue.Enqueue(vc);
            }
        }
    }
}

