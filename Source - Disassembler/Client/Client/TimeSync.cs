namespace Client
{
    using System;

    public class TimeSync
    {
        private double m_Duration;
        private double m_rSeconds;
        private double m_Start;

        public TimeSync(double duration)
        {
            this.Initialize(duration);
        }

        public void Initialize(double duration)
        {
            this.m_Duration = duration;
            this.m_Start = Engine.m_dTicks;
            if (!Engine.m_SetTicks)
            {
                Engine.QueryPerformanceCounter(ref Engine.m_QPC);
                this.m_Start = Engine.m_QPC;
                this.m_Start /= Engine.m_QPF;
                Engine.m_Ticks = (int) (this.m_Start + 0.5);
                Engine.m_dTicks = this.m_Start;
                Engine.m_SetTicks = true;
            }
            this.m_rSeconds = 1.0 / (duration * 1000.0);
        }

        public double Elapsed
        {
            get
            {
                double ticks = Engine.Ticks;
                double num2 = ticks - this.m_Start;
                double num3 = num2 / 1000.0;
                if (num3 < 0.0)
                {
                    num3 = 0.0;
                }
                return num3;
            }
        }

        public double Normalized
        {
            get
            {
                double dTicks = Engine.m_dTicks;
                if (!Engine.m_SetTicks)
                {
                    Engine.QueryPerformanceCounter(ref Engine.m_QPC);
                    dTicks = Engine.m_QPC;
                    dTicks /= Engine.m_QPF;
                    Engine.m_Ticks = (int) (dTicks + 0.5);
                    Engine.m_dTicks = dTicks;
                    Engine.m_SetTicks = true;
                }
                double num2 = dTicks - this.m_Start;
                double num3 = num2 * this.m_rSeconds;
                if (num3 < 0.0)
                {
                    return 0.0;
                }
                if (num3 > 1.0)
                {
                    num3 = 1.0;
                }
                return num3;
            }
        }
    }
}

