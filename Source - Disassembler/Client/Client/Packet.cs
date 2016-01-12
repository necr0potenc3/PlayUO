namespace Client
{
    using System.IO;

    public class Packet
    {
        protected bool m_Encode;
        protected int m_Length;
        protected string m_Name;
        public PacketWriter m_Stream;

        public Packet(byte packetID, string name)
        {
            this.m_Encode = true;
            this.m_Name = name;
            this.m_Stream = new PacketWriter();
            this.m_Stream.Write(packetID);
            this.m_Stream.Write((short)0);
        }

        public Packet(byte packetID, string name, int length)
        {
            this.m_Encode = true;
            this.m_Name = name;
            this.m_Length = length;
            this.m_Stream = new PacketWriter(length);
            this.m_Stream.Write(packetID);
        }

        public byte[] Compile()
        {
            this.m_Stream.Flush();
            if (this.m_Length == 0)
            {
                long length = this.m_Stream.Length;
                this.m_Stream.Seek(1L, SeekOrigin.Begin);
                this.m_Stream.Write((ushort)length);
                this.m_Stream.Flush();
            }
            return this.m_Stream.Compile();
        }

        public void Dispose()
        {
            this.m_Stream.Close();
            this.m_Stream = null;
            this.m_Name = null;
        }

        public bool Encode
        {
            get
            {
                return this.m_Encode;
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }
    }
}