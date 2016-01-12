namespace Client
{
    using System.IO;
    using System.Net;
    using System.Text;

    public class UOAMPacket
    {
        private static byte[] m_Buffer = new byte[4];
        private MemoryStream m_Stream = new MemoryStream();

        public void Align(int to)
        {
            int num = (int)(this.m_Stream.Position % ((long)to));
            if (num != 0)
            {
                this.Write(new byte[to - num]);
            }
        }

        public byte[] Compile()
        {
            this.m_Stream.Seek(8L, SeekOrigin.Begin);
            this.Write((int)this.m_Stream.Length);
            return this.m_Stream.ToArray();
        }

        public long Seek(long offset, SeekOrigin origin)
        {
            return this.m_Stream.Seek(offset, origin);
        }

        public void Write(byte[] buffer)
        {
            this.m_Stream.Write(buffer, 0, buffer.Length);
        }

        public void Write(byte value)
        {
            this.m_Stream.WriteByte(value);
        }

        public void Write(short value)
        {
            m_Buffer[0] = (byte)value;
            m_Buffer[1] = (byte)(value >> 8);
            this.m_Stream.Write(m_Buffer, 0, 2);
        }

        public void Write(int value)
        {
            m_Buffer[0] = (byte)value;
            m_Buffer[1] = (byte)(value >> 8);
            m_Buffer[2] = (byte)(value >> 0x10);
            m_Buffer[3] = (byte)(value >> 0x18);
            this.m_Stream.Write(m_Buffer, 0, 4);
        }

        public void Write(IPAddress ipaddr)
        {
            this.Write((uint)ipaddr.Address);
        }

        public void Write(sbyte value)
        {
            this.m_Stream.WriteByte((byte)value);
        }

        public void Write(ushort value)
        {
            m_Buffer[0] = (byte)value;
            m_Buffer[1] = (byte)(value >> 8);
            this.m_Stream.Write(m_Buffer, 0, 2);
        }

        public void Write(uint value)
        {
            m_Buffer[0] = (byte)value;
            m_Buffer[1] = (byte)(value >> 8);
            m_Buffer[2] = (byte)(value >> 0x10);
            m_Buffer[3] = (byte)(value >> 0x18);
            this.m_Stream.Write(m_Buffer, 0, 4);
        }

        public void Write(string value, int length)
        {
            if (value.Length >= length)
            {
                value = value.Substring(0, length - 1);
            }
            this.Write(Encoding.ASCII.GetBytes(value));
            this.Write(new byte[length - value.Length]);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            this.m_Stream.Write(buffer, offset, count);
        }

        public long Length
        {
            get
            {
                return this.m_Stream.Length;
            }
        }
    }
}