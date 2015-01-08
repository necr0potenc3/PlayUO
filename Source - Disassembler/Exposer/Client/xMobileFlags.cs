namespace Client
{
    using System;
    using System.Reflection;

    public class xMobileFlags
    {
        private int m_Serial;
        private int m_Value;

        public xMobileFlags(int Serial, int Value)
        {
            this.m_Serial = Serial;
            this.m_Value = Value;
        }

        public xMobileFlags Clone()
        {
            return new xMobileFlags(this.m_Serial, this.m_Value);
        }

        public bool this[xMobileFlag flag]
        {
            get
            {
                return ((this.m_Value & flag) != 0);
            }
            set
            {
                if (value)
                {
                    this.m_Value |= flag;
                }
                else
                {
                    this.m_Value &= ~flag;
                }
                Interop.SetMobile(this.m_Serial, "Flags", this.m_Value);
            }
        }

        public int Value
        {
            get
            {
                return this.m_Value;
            }
            set
            {
                this.m_Value = value;
                Interop.SetMobile(this.m_Serial, "Flags", this.m_Value);
            }
        }
    }
}

