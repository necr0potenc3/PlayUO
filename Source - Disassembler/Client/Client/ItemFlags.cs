namespace Client
{
    public class ItemFlags
    {
        private int m_Value;

        public override string ToString()
        {
            if ((this.m_Value & -161) != 0)
            {
                return string.Format("Unknown flags: 0x{0:X2}", this.m_Value);
            }
            return ((ItemFlag)this.m_Value).ToString();
        }

        public bool this[ItemFlag flag]
        {
            get
            {
                int num = (int)flag;
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
                if ((this.m_Value & -161) != 0)
                {
                    string message = string.Format("Unknown item flags: 0x{0:X2}", this.m_Value);
                    Debug.Trace(message);
                    Engine.AddTextMessage(message);
                }
            }
        }
    }
}