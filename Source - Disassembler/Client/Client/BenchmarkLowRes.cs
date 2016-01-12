namespace Client
{
    using System;

    public class BenchmarkLowRes
    {
        private long m_Elapsed;
        private long m_Overhead;
        private long m_Start;

        public BenchmarkLowRes()
        {
            this.Start();
            this.Stop();
            this.m_Overhead = this.Elapsed;
        }

        public void Start()
        {
            this.m_Start = DateTime.Now.Ticks;
        }

        public void Stop()
        {
            this.m_Elapsed = (DateTime.Now.Ticks - this.m_Start) - this.m_Overhead;
        }

        public long Elapsed
        {
            get
            {
                return this.m_Elapsed;
            }
        }
    }
}