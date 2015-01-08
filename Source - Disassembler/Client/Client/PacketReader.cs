namespace Client
{
    using System;
    using System.IO;
    using System.Text;

    public class PacketReader
    {
        private int m_Bounds;
        private byte m_Command;
        private int m_Count;
        private byte[] m_Data;
        private bool m_FixedSize;
        private int m_Index;
        private static PacketReader m_Instance;
        private string m_Name;
        private string m_ReturnName;
        private int m_Start;

        public PacketReader(byte[] data, int index, int count, bool fixedSize, byte command, string name)
        {
            this.m_Data = data;
            this.m_Start = this.m_Index = index;
            this.m_Count = count;
            this.m_Bounds = this.m_Start + this.m_Count;
            this.m_FixedSize = fixedSize;
            this.m_Command = command;
            this.m_Name = name;
            this.m_ReturnName = name;
            if (!fixedSize)
            {
                this.m_Index += 3;
            }
            else
            {
                this.m_Index++;
            }
        }

        public static PacketReader Initialize(byte[] data, int index, int count, bool fixedSize, byte command, string name)
        {
            if (m_Instance == null)
            {
                m_Instance = new PacketReader(data, index, count, fixedSize, command, name);
            }
            else
            {
                m_Instance.m_Data = data;
                m_Instance.m_Start = m_Instance.m_Index = index;
                m_Instance.m_Count = count;
                m_Instance.m_Bounds = m_Instance.m_Start + m_Instance.m_Count;
                m_Instance.m_FixedSize = fixedSize;
                m_Instance.m_Command = command;
                m_Instance.m_Name = name;
                m_Instance.m_ReturnName = name;
                if (!fixedSize)
                {
                    m_Instance.m_Index += 3;
                }
                else
                {
                    m_Instance.m_Index++;
                }
            }
            return m_Instance;
        }

        public bool ReadBoolean()
        {
            return (this.m_Data[this.m_Index++] != 0);
        }

        public byte ReadByte()
        {
            return this.m_Data[this.m_Index++];
        }

        public byte[] ReadBytes(int length)
        {
            byte[] dst = new byte[length];
            Buffer.BlockCopy(this.m_Data, this.m_Index, dst, 0, length);
            this.m_Index += length;
            return dst;
        }

        public short ReadInt16()
        {
            return (short) ((this.m_Data[this.m_Index++] << 8) | this.m_Data[this.m_Index++]);
        }

        public int ReadInt32()
        {
            return ((((this.m_Data[this.m_Index++] << 0x18) | (this.m_Data[this.m_Index++] << 0x10)) | (this.m_Data[this.m_Index++] << 8)) | this.m_Data[this.m_Index++]);
        }

        public sbyte ReadSByte()
        {
            return (sbyte) this.m_Data[this.m_Index++];
        }

        public unsafe string ReadString()
        {
            fixed (byte* numRef = this.m_Data)
            {
                byte* numPtr = numRef + this.m_Index;
                byte* numPtr2 = numRef + this.m_Bounds;
                byte* numPtr3 = numPtr;
                byte* numPtr4 = numPtr;
                while (((numPtr4 = numPtr3) < numPtr2) && (*(numPtr3++) != 0))
                {
                }
                this.m_Index = (int) (((long) ((numPtr4 - numRef) / 1)) + 1L);
                return new string((sbyte*) numPtr, 0, (int) ((long) ((numPtr4 - numPtr) / 1)));
            }
        }

        public unsafe string ReadString(int fixedLength)
        {
            fixed (byte* numRef = this.m_Data)
            {
                byte* numPtr = numRef + this.m_Index;
                byte* numPtr2 = numPtr + fixedLength;
                byte* numPtr3 = numPtr;
                byte* numPtr4 = numPtr;
                if ((numRef + this.m_Bounds) < numPtr2)
                {
                    numPtr2 = numRef + this.m_Bounds;
                }
                while (((numPtr4 = numPtr3) < numPtr2) && (*(numPtr3++) != 0))
                {
                }
                this.m_Index += fixedLength;
                return new string((sbyte*) numPtr, 0, (int) ((long) ((numPtr4 - numPtr) / 1)));
            }
        }

        public ushort ReadUInt16()
        {
            return (ushort) ((this.m_Data[this.m_Index++] << 8) | this.m_Data[this.m_Index++]);
        }

        public uint ReadUInt32()
        {
            return (uint) ((((this.m_Data[this.m_Index++] << 0x18) | (this.m_Data[this.m_Index++] << 0x10)) | (this.m_Data[this.m_Index++] << 8)) | this.m_Data[this.m_Index++]);
        }

        public unsafe string ReadUnicodeLEString()
        {
            fixed (byte* numRef = this.m_Data)
            {
                byte* numPtr = numRef + this.m_Index;
                byte* numPtr2 = numRef + this.m_Bounds;
                byte* numPtr3 = numPtr;
                byte* numPtr4 = numPtr;
                while (((numPtr4 = numPtr3) < numPtr2) && ((*(numPtr3++) | *(numPtr3++)) != 0))
                {
                }
                this.m_Index = (int) (((long) ((numPtr4 - numRef) / 1)) + 2L);
                return new string((sbyte*) numPtr, 0, (int) ((long) ((numPtr4 - numPtr) / 1)), Encoding.Unicode);
            }
        }

        public unsafe string ReadUnicodeString()
        {
            fixed (byte* numRef = this.m_Data)
            {
                byte* numPtr = numRef + this.m_Index;
                byte* numPtr2 = numRef + this.m_Bounds;
                byte* numPtr3 = numPtr;
                byte* numPtr4 = numPtr;
                while (((numPtr4 = numPtr3) < numPtr2) && ((*(numPtr3++) | *(numPtr3++)) != 0))
                {
                }
                this.m_Index = (int) (((long) ((numPtr4 - numRef) / 1)) + 2L);
                return new string((sbyte*) numPtr, 0, (int) ((long) ((numPtr4 - numPtr) / 1)), Encoding.BigEndianUnicode);
            }
        }

        public unsafe string ReadUnicodeString(int fixedLength)
        {
            fixed (byte* numRef = this.m_Data)
            {
                byte* numPtr = numRef + this.m_Index;
                byte* numPtr2 = numPtr + (fixedLength << 1);
                byte* numPtr3 = numPtr;
                byte* numPtr4 = numPtr;
                if ((numRef + this.m_Bounds) < numPtr2)
                {
                    numPtr2 = numRef + this.m_Bounds;
                }
                while (((numPtr4 = numPtr3) < numPtr2) && ((*(numPtr3++) | *(numPtr3++)) != 0))
                {
                }
                this.m_Index += fixedLength << 1;
                return new string((sbyte*) numPtr, 0, (int) ((long) ((numPtr4 - numPtr) / 1)), Encoding.BigEndianUnicode);
            }
        }

        public unsafe string ReadUTF8()
        {
            fixed (byte* numRef = this.m_Data)
            {
                byte* numPtr = numRef + this.m_Index;
                byte* numPtr2 = numRef + this.m_Bounds;
                byte* numPtr3 = numPtr;
                byte* numPtr4 = numPtr;
                while (((numPtr4 = numPtr3) < numPtr2) && (*(numPtr3++) != 0))
                {
                }
                int index = this.m_Index;
                this.m_Index = (int) (((long) ((numPtr4 - numRef) / 1)) + 1L);
                return Encoding.UTF8.GetString(this.m_Data, index, (int) ((long) ((numPtr4 - numPtr) / 1)));
            }
        }

        public void Seek(int offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    this.m_Index = this.m_Start + offset;
                    break;

                case SeekOrigin.Current:
                    this.m_Index += offset;
                    break;

                case SeekOrigin.End:
                    this.m_Index = this.m_Bounds + offset;
                    break;
            }
        }

        public void Trace()
        {
            Engine.AddTextMessage(string.Format("Tracing packet 0x{0:X2} '{1}' of length {2} ( 0x{2:X} )", this.m_Command, this.m_Name, this.m_Count));
            StreamWriter tw = new StreamWriter("PacketTrace.log", true);
            if (this.m_Count < 0x10)
            {
                tw.WriteLine("Packet Server->Client '{0}' ( {1} bytes )", this.m_ReturnName, this.m_Count);
            }
            else
            {
                tw.WriteLine("Packet Server->Client '{0}' ( {1} [0x{1:X}] bytes )", this.m_ReturnName, this.m_Count);
            }
            tw.WriteLine();
            Network.Log(tw, this.m_Data, this.m_Start, this.m_Count);
            tw.WriteLine();
            tw.Flush();
            tw.Close();
        }

        public byte Command
        {
            get
            {
                return this.m_Command;
            }
        }

        public bool Finished
        {
            get
            {
                return (this.m_Index >= this.m_Bounds);
            }
        }

        public bool FixedSize
        {
            get
            {
                return this.m_FixedSize;
            }
        }

        public int Length
        {
            get
            {
                return this.m_Count;
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }

        public int Position
        {
            get
            {
                return (this.m_Index - this.m_Start);
            }
            set
            {
                this.m_Index = value + this.m_Start;
            }
        }

        public string ReturnName
        {
            get
            {
                return this.m_ReturnName;
            }
            set
            {
                this.m_ReturnName = value;
            }
        }

        public int Start
        {
            get
            {
                return this.m_Start;
            }
        }
    }
}

