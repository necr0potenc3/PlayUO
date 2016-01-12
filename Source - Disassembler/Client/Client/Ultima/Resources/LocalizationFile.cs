namespace Client
{
    using System;
    using System.IO;
    using System.Text;

    public class LocalizationFile
    {
        private static byte[] m_Buffer = new byte[4];
        private string m_Name;
        private string[] m_Text;
        private bool m_Valid;

        public LocalizationFile(string path)
        {
            this.m_Name = Path.GetFileName(path);
            if (File.Exists(path))
            {
                this.ReadFromDisk(path);
            }
        }

        private unsafe void ReadFromDisk(string path)
        {
            try
            {
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                stream.Seek(0x1cL, SeekOrigin.Begin);
                int num = this.ReadInt32_BE(stream);
                long position = stream.Position;
                while (stream.ReadByte() > 0)
                {
                }
                int num3 = this.ReadInt32_LE(stream);
                stream.Seek(4L, SeekOrigin.Current);
                while (stream.ReadByte() > 0)
                {
                }
                stream.Seek(4L, SeekOrigin.Current);
                int num4 = this.ReadInt32_LE(stream);
                stream.Seek(((position + num) + (num & 1)) + 4L, SeekOrigin.Begin);
                int count = this.ReadInt32_BE(stream);
                if (count > m_Buffer.Length)
                {
                    m_Buffer = new byte[count];
                }
                stream.Read(m_Buffer, 0, count);
                stream.Close();
                this.m_Valid = true;
                switch (num3)
                {
                    case 1:
                        this.m_Text = new string[num4];
                        fixed (byte* numRef = m_Buffer)
                        {
                            byte* numPtr = numRef;
                            byte* numPtr2 = numPtr + count;
                            for (int i = 0; i < num4; i++)
                            {
                                byte* numPtr3 = numPtr;
                                while ((numPtr < numPtr2) && (*(numPtr++) != 0))
                                {
                                }
                                this.m_Text[i] = new string((sbyte*)numPtr3, 0, (int)(((long)((numPtr - numPtr3) / 1)) - 1L));
                            }
                        }
                        return;

                    case 2:
                        {
                            this.m_Text = new string[num4];
                            Encoding unicode = Encoding.Unicode;
                            fixed (byte* numRef2 = m_Buffer)
                            {
                                byte* numPtr4 = numRef2;
                                byte* numPtr5 = numPtr4 + count;
                                for (int j = 0; j < num4; j++)
                                {
                                    byte* numPtr6 = numPtr4;
                                    while ((numPtr4 < numPtr5) && ((*(numPtr4++) | *(numPtr4++)) != 0))
                                    {
                                    }
                                    this.m_Text[j] = new string((sbyte*)numPtr6, 0, (int)(((long)((numPtr4 - numPtr6) / 1)) - 2L), unicode);
                                }
                            }
                            return;
                        }
                }
                throw new InvalidOperationException(string.Format("Character size invalid. (charSize={0})", num3));
            }
            catch
            {
                this.m_Valid = false;
            }
        }

        private int ReadInt32_BE(Stream stream)
        {
            stream.Read(m_Buffer, 0, 4);
            return ((((m_Buffer[0] << 0x18) | (m_Buffer[1] << 0x10)) | (m_Buffer[2] << 8)) | m_Buffer[3]);
        }

        private int ReadInt32_LE(Stream stream)
        {
            stream.Read(m_Buffer, 0, 4);
            return (((m_Buffer[0] | (m_Buffer[1] << 8)) | (m_Buffer[2] << 0x10)) | (m_Buffer[3] << 0x18));
        }

        public int Count
        {
            get
            {
                return this.m_Text.Length;
            }
        }

        public string this[int index]
        {
            get
            {
                if (this.m_Valid)
                {
                    if ((index < 0) || (index >= this.m_Text.Length))
                    {
                        return string.Format("<Index out of bounds: {0}:{1} ({2})>", this.m_Name, index, this.m_Text.Length);
                    }
                    return this.m_Text[index];
                }
                return string.Format("<Invalid localization file: {0}>", this.m_Name);
            }
        }
    }
}