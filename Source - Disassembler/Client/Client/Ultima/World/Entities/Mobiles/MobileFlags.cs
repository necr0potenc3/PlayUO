﻿namespace Client
{
    public class MobileFlags
    {
        private Mobile m_Target;
        private int m_Value;

        public MobileFlags(Mobile who)
        {
            this.m_Target = who;
        }

        public MobileFlags Clone()
        {
            MobileFlags flags = new MobileFlags(this.m_Target);
            flags.m_Value = this.m_Value;
            return flags;
        }

        public override string ToString()
        {
            if ((this.m_Value & -223) != 0)
            {
                return string.Format("0x{0:X2}", this.m_Value);
            }
            return ((MobileFlag)this.m_Value).ToString();
        }

        public bool this[MobileFlag flag]
        {
            get
            {
                return ((this.m_Value & (int)flag) != 0);
            }
            set
            {
                if (value)
                {
                    this.m_Value |= (int)flag;
                }
                else
                {
                    this.m_Value &= (int)~flag;
                }
                this.m_Target.OnFlagsChanged();
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
                this.m_Target.OnFlagsChanged();
            }
        }
    }
}