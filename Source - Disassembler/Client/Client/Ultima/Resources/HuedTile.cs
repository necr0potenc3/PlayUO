namespace Client
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct HuedTile
    {
        private short m_ID;
        private short m_Hue;
        private sbyte m_Z;

        public int ID
        {
            get
            {
                return this.m_ID;
            }
        }

        public int Hue
        {
            get
            {
                return this.m_Hue;
            }
        }

        public int Z
        {
            get
            {
                return this.m_Z;
            }
            set
            {
                this.m_Z = (sbyte)value;
            }
        }

        public HuedTile(short id, short hue, sbyte z)
        {
            this.m_ID = id;
            this.m_Hue = hue;
            this.m_Z = z;
        }

        public void Set(short id, short hue, sbyte z)
        {
            this.m_ID = id;
            this.m_Hue = hue;
            this.m_Z = z;
        }
    }
}