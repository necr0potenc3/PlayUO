namespace Client
{
    using System;
    using System.IO;
    using System.Text;

    public class PacketWriter
    {
        private byte[] m_Buffer;
        private int m_BufferLength;
        private int m_Index;
        private MemoryStream m_Stream;

        public PacketWriter() : this(0x20)
        {
        }

        public PacketWriter(int capacity)
        {
            this.m_Stream = new MemoryStream(capacity);
            this.m_BufferLength = (capacity > 0x10) ? 0x10 : capacity;
            this.m_Buffer = new byte[this.m_BufferLength];
        }

        public void Close()
        {
            this.m_Stream.Close();
            this.m_Buffer = null;
        }

        public byte[] Compile()
        {
            this.Flush();
            return this.m_Stream.ToArray();
        }

        public void Flush()
        {
            if (this.m_Index > 0)
            {
                this.m_Stream.Write(this.m_Buffer, 0, this.m_Index);
                this.m_Index = 0;
            }
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            if (this.m_Index > 0)
            {
                this.Flush();
            }
            this.m_Stream.Seek(offset, origin);
        }

        public void Write(bool toWrite)
        {
            if ((this.m_Index + 1) > this.m_BufferLength)
            {
                this.Flush();
            }
            this.m_Buffer[this.m_Index++] = toWrite ? ((byte) 1) : ((byte) 0);
        }

        public void Write(byte[] toWrite)
        {
            if (this.m_Index > 0)
            {
                this.Flush();
            }
            this.m_Stream.Write(toWrite, 0, toWrite.Length);
        }

        public void Write(byte toWrite)
        {
            if ((this.m_Index + 1) > this.m_BufferLength)
            {
                this.Flush();
            }
            this.m_Buffer[this.m_Index++] = toWrite;
        }

        public void Write(short toWrite)
        {
            if ((this.m_Index + 2) > this.m_BufferLength)
            {
                this.Flush();
            }
            this.m_Buffer[this.m_Index++] = (byte) (toWrite >> 8);
            this.m_Buffer[this.m_Index++] = (byte) toWrite;
        }

        public void Write(int toWrite)
        {
            if ((this.m_Index + 4) > this.m_BufferLength)
            {
                this.Flush();
            }
            this.m_Buffer[this.m_Index++] = (byte) (toWrite >> 0x18);
            this.m_Buffer[this.m_Index++] = (byte) (toWrite >> 0x10);
            this.m_Buffer[this.m_Index++] = (byte) (toWrite >> 8);
            this.m_Buffer[this.m_Index++] = (byte) toWrite;
        }

        public void Write(sbyte toWrite)
        {
            if ((this.m_Index + 1) > this.m_BufferLength)
            {
                this.Flush();
            }
            this.m_Buffer[this.m_Index++] = (byte) toWrite;
        }

        public void Write(string toWrite)
        {
            if (this.m_Index > 0)
            {
                this.Flush();
            }
            this.Write(Encoding.ASCII.GetBytes(toWrite));
        }

        public void Write(ushort toWrite)
        {
            if ((this.m_Index + 2) > this.m_BufferLength)
            {
                this.Flush();
            }
            this.m_Buffer[this.m_Index++] = (byte) (toWrite >> 8);
            this.m_Buffer[this.m_Index++] = (byte) toWrite;
        }

        public void Write(uint toWrite)
        {
            if ((this.m_Index + 4) > this.m_BufferLength)
            {
                this.Flush();
            }
            this.m_Buffer[this.m_Index++] = (byte) (toWrite >> 0x18);
            this.m_Buffer[this.m_Index++] = (byte) (toWrite >> 0x10);
            this.m_Buffer[this.m_Index++] = (byte) (toWrite >> 8);
            this.m_Buffer[this.m_Index++] = (byte) toWrite;
        }

        public void Write(string toWrite, int cb)
        {
            if (this.m_Index > 0)
            {
                this.Flush();
            }
            int length = toWrite.Length;
            if (length > cb)
            {
                length = cb;
            }
            if (length > 0)
            {
                this.Write(Encoding.ASCII.GetBytes(toWrite.ToCharArray(), 0, length));
            }
            int count = cb - length;
            if (count > 0)
            {
                this.m_Stream.Write(new byte[count], 0, count);
            }
        }

        public void WriteEncoded(int toWrite)
        {
            this.Write((byte) 0);
            this.Write(toWrite);
        }

        public void WriteUnicode(string toWrite)
        {
            if (this.m_Index > 0)
            {
                this.Flush();
            }
            this.Write(Encoding.BigEndianUnicode.GetBytes(toWrite));
        }

        public void WriteUnicodeLE(string toWrite)
        {
            if (this.m_Index > 0)
            {
                this.Flush();
            }
            this.Write(Encoding.Unicode.GetBytes(toWrite));
        }

        public long Length
        {
            get
            {
                if (this.m_Index > 0)
                {
                    this.Flush();
                }
                return this.m_Stream.Length;
            }
        }
    }
}

