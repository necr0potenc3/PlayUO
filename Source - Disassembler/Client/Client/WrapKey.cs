namespace Client
{
    using System;

    public class WrapKey
    {
        public int m_HashCode;
        public int m_MaxWidth;
        public string m_ToWrap;

        public WrapKey(string toWrap, int maxWidth)
        {
            this.m_ToWrap = toWrap;
            this.m_MaxWidth = maxWidth;
            this.m_HashCode = ((toWrap == null) ? 0 : toWrap.GetHashCode()) ^ maxWidth;
        }

        public override bool Equals(object x)
        {
            WrapKey key = (WrapKey) x;
            return ((this == key) || (((this.m_HashCode == key.m_HashCode) && (this.m_MaxWidth == key.m_MaxWidth)) && (this.m_ToWrap == key.m_ToWrap)));
        }

        public override int GetHashCode()
        {
            return this.m_HashCode;
        }
    }
}

