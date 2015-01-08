namespace Client
{
    using System;

    internal class PacketHandler
    {
        private PacketCallback m_Callback;
        private int m_Count;
        private int m_Length;
        private string m_Name;
        private int m_PacketID;

        public PacketHandler(int packetID, string name, int length, PacketCallback callback)
        {
            this.m_Name = name;
            this.m_Callback = callback;
            this.m_PacketID = packetID;
            this.m_Length = length;
        }

        public void Handle(PacketReader pvSrc)
        {
            this.m_Callback(pvSrc);
            this.m_Count++;
        }

        public PacketCallback Callback
        {
            get
            {
                return this.m_Callback;
            }
        }

        public int Count
        {
            get
            {
                return this.m_Count;
            }
        }

        public int Length
        {
            get
            {
                return this.m_Length;
            }
            set
            {
                this.m_Length = value;
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }

        public int PacketID
        {
            get
            {
                return this.m_PacketID;
            }
        }
    }
}

