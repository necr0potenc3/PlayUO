namespace Client
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct Tile
    {
        private short m_ID;
        private sbyte m_Z;
        public int ID
        {
            get
            {
                return this.m_ID;
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
                this.m_Z = (sbyte) value;
            }
        }
        public bool Ignored
        {
            get
            {
                return (((this.m_ID == 2) || (this.m_ID == 0x1db)) || ((this.m_ID >= 430) && (this.m_ID <= 0x1b5)));
            }
        }
        public Tile(short id, sbyte z)
        {
            this.m_ID = id;
            this.m_Z = z;
        }

        public void Set(short id, sbyte z)
        {
            this.m_ID = id;
            this.m_Z = z;
        }
    }
}

