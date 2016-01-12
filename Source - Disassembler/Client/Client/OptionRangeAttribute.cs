namespace Client
{
    using System;

    public class OptionRangeAttribute : Attribute
    {
        private int m_Max;
        private int m_Min;

        public OptionRangeAttribute(int min, int max)
        {
            this.m_Min = min;
            this.m_Max = max;
        }

        public int Max
        {
            get
            {
                return this.m_Max;
            }
        }

        public int Min
        {
            get
            {
                return this.m_Min;
            }
        }
    }
}