namespace Client
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct BBPAItem
    {
        private int m_ItemID;
        private int m_Hue;

        public int ItemID
        {
            get
            {
                return this.m_ItemID;
            }
            set
            {
                this.m_ItemID = value;
            }
        }

        public int Hue
        {
            get
            {
                return this.m_Hue;
            }
            set
            {
                this.m_Hue = value;
            }
        }

        public BBPAItem(int itemID, int hue)
        {
            this.m_ItemID = itemID;
            this.m_Hue = hue;
        }
    }
}