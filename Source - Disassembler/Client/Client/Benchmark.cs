namespace Client
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    public class Benchmark
    {
        public const int AnimationInitCache = 1;
        public const int AnimationInitLinks = 0;
        public const int Cache = 7;
        public const int CursorInit = 2;
        public const int GetArt = 3;
        public const int GetTexture128x128 = 5;
        public const int GetTexture64x64 = 4;
        private int m_BenchID;
        private long m_Elapsed;
        private long m_Start;
        public const int Render = 6;

        public Benchmark(int BenchID)
        {
            this.m_BenchID = BenchID;
        }

        public static string Format(long Elapsed)
        {
            long num = Elapsed;
            long num2 = num / 0x2710L;
            long num3 = num2 / 0x3e8L;
            long num4 = num3 / 60L;
            long num5 = num4 / 60L;
            long num6 = num5 / 60L;
            long num7 = num6 / 7L;
            num = num % 0x2710L;
            num2 = num2 % 0x3e8L;
            num3 = num3 % 60L;
            num4 = num4 % 60L;
            num5 = num5 % 60L;
            num6 = num6 % 7L;
            StringBuilder builder = new StringBuilder();
            if (num6 != 0L)
            {
                builder.Append(num6);
                builder.Append("d ");
            }
            if (num5 != 0L)
            {
                builder.Append(num5);
                builder.Append("h ");
            }
            if (num4 != 0L)
            {
                builder.Append(num4);
                builder.Append("m ");
            }
            if (num3 != 0L)
            {
                builder.Append(num3);
                builder.Append("s ");
            }
            if (num2 != 0L)
            {
                builder.Append(num2);
                builder.Append("ms ");
            }
            builder.Append(num);
            builder.Append("ns");
            return builder.ToString();
        }

        [DllImport("Kernel32")]
        private static extern bool QueryPerformanceCounter(ref long Counter);
        [DllImport("Kernel32")]
        private static extern bool QueryPerformanceFrequency(ref long Frequency);
        public void Start()
        {
            this.m_Start = Engine.Ticks;
        }

        public void StopNoLog()
        {
            this.m_Elapsed = Engine.Ticks - this.m_Start;
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

