namespace Client
{
    using System.Text;

    public class UOAMPacketReader
    {
        private byte[] m_Buffer;
        private int m_Index;

        public UOAMPacketReader(byte[] buffer)
        {
            this.m_Buffer = buffer;
            this.m_Index = 0x18;
        }

        public void Align(int to)
        {
            int num = this.m_Index % to;
            if (num != 0)
            {
                this.m_Index += to - num;
            }
        }

        public byte ReadByte()
        {
            if ((this.m_Index + 1) > this.m_Buffer.Length)
            {
                return 0;
            }
            return this.m_Buffer[this.m_Index++];
        }

        public short ReadInt16()
        {
            if ((this.m_Index + 2) > this.m_Buffer.Length)
            {
                return 0;
            }
            return (short)(this.m_Buffer[this.m_Index++] | (this.m_Buffer[this.m_Index++] << 8));
        }

        public int ReadInt32()
        {
            if ((this.m_Index + 4) > this.m_Buffer.Length)
            {
                return 0;
            }
            return (((this.m_Buffer[this.m_Index++] | (this.m_Buffer[this.m_Index++] << 8)) | (this.m_Buffer[this.m_Index++] << 0x10)) | (this.m_Buffer[this.m_Index++] << 0x18));
        }

        public sbyte ReadSByte()
        {
            if ((this.m_Index + 1) > this.m_Buffer.Length)
            {
                return 0;
            }
            return (sbyte)this.m_Buffer[this.m_Index++];
        }

        public string ReadString(int length)
        {
            int num;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; (i < length) && ((num = this.m_Buffer[this.m_Index + i]) != 0); i++)
            {
                builder.Append((char)num);
            }
            this.m_Index += length;
            return builder.ToString();
        }

        public ushort ReadUInt16()
        {
            if ((this.m_Index + 2) > this.m_Buffer.Length)
            {
                return 0;
            }
            return (ushort)(this.m_Buffer[this.m_Index++] | (this.m_Buffer[this.m_Index++] << 8));
        }

        public uint ReadUInt32()
        {
            if ((this.m_Index + 4) > this.m_Buffer.Length)
            {
                return 0;
            }
            return (uint)(((this.m_Buffer[this.m_Index++] | (this.m_Buffer[this.m_Index++] << 8)) | (this.m_Buffer[this.m_Index++] << 0x10)) | (this.m_Buffer[this.m_Index++] << 0x18));
        }
    }
}