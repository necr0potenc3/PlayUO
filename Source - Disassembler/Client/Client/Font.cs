namespace Client
{
    using System;
    using System.Collections;
    using System.IO;

    public class Font : IFont, IFontFactory
    {
        private static byte[] m_Buffer;
        private FontCache m_Cache;
        private int m_FontID;
        public FontImage[] m_Images;
        private short[] m_Palette;
        private Hashtable m_WrapCache = new Hashtable();

        public unsafe Font(int fid)
        {
            this.m_FontID = fid;
            this.m_Cache = new FontCache(this);
            this.m_Images = new FontImage[0xe0];
            string path = Engine.FileManager.BasePath("Data/QuickLoad/Fonts.mul");
            if (!File.Exists(path))
            {
                Reformat();
            }
            FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader reader = new BinaryReader(input);
            if (DateTime.FromFileTime(reader.ReadInt64()) != new FileInfo(Engine.FileManager.ResolveMUL(Files.Fonts)).LastWriteTime)
            {
                reader.Close();
                Reformat();
                input = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                reader = new BinaryReader(input);
            }
            input.Seek((long) (12 + (fid * 8)), SeekOrigin.Begin);
            int num = reader.ReadInt32();
            int bytes = reader.ReadInt32();
            input.Seek((long) num, SeekOrigin.Begin);
            if ((m_Buffer == null) || (bytes > m_Buffer.Length))
            {
                m_Buffer = new byte[bytes];
            }
            fixed (byte* numRef = m_Buffer)
            {
                Engine.NativeRead(input, (void*) numRef, bytes);
                byte* numPtr = numRef;
                for (int i = 0; i < 0xe0; i++)
                {
                    int xWidth = numPtr[0];
                    int yHeight = numPtr[1];
                    numPtr += 3;
                    FontImage image = new FontImage(xWidth, yHeight);
                    int xDelta = image.xDelta;
                    fixed (byte* numRef2 = image.xyPixels)
                    {
                        byte* numPtr2 = numRef2;
                        int num7 = 0;
                        while (num7 < yHeight)
                        {
                            int num6 = 0;
                            byte* numPtr3 = numPtr2;
                            while (num6 < xWidth)
                            {
                                *(numPtr3++) = *(numPtr++);
                                num6++;
                            }
                            num7++;
                            numPtr2 += xDelta;
                        }
                    }
                    this.m_Images[i] = image;
                }
                int num9 = *((int*) numPtr);
                numPtr += 4;
                short* numPtr4 = (short*) numPtr;
                this.m_Palette = new short[num9];
                for (int j = 0; j < num9; j++)
                {
                    numPtr4++;
                    this.m_Palette[j] = numPtr4[0];
                }
                numPtr = (byte*) numPtr4;
            }
            reader.Close();
        }

        unsafe Texture IFontFactory.CreateInstance(string text, IHue hue)
        {
            int num;
            char ch;
            FontImage image;
            if ((text == null) || (text.Length <= 0))
            {
                return Texture.Empty;
            }
            int yHeight = 0;
            int num3 = 0;
            int width = 0;
            int height = 1;
            char[] chArray = text.ToCharArray();
            for (num = 0; num < chArray.Length; num++)
            {
                ch = chArray[num];
                if ((ch >= ' ') && (ch < 'Ā'))
                {
                    image = this.m_Images[ch - ' '];
                    num3 += image.xWidth;
                    if (num3 > width)
                    {
                        width = num3;
                    }
                    if (image.yHeight > yHeight)
                    {
                        yHeight = image.yHeight;
                    }
                }
                else if (ch == '\n')
                {
                    num3 = 0;
                    height++;
                }
            }
            height *= yHeight;
            if ((width <= 0) || (height <= 0))
            {
                return Texture.Empty;
            }
            Texture texture = new Texture(width, height, false);
            if (texture.IsEmpty())
            {
                return Texture.Empty;
            }
            short[] numArray = new short[this.m_Palette.Length];
            fixed (short* numRef = numArray)
            {
                fixed (short* numRef2 = this.m_Palette)
                {
                    hue.CopyPixels((void*) (numRef2 + 1), (void*) (numRef + 1), this.m_Palette.Length - 1);
                }
                LockData data = texture.Lock(LockFlags.WriteOnly);
                short* pvSrc = (short*) data.pvSrc;
                short* numPtr2 = pvSrc;
                int num6 = data.Pitch >> 1;
                int num7 = num6 * yHeight;
                for (num = 0; num < chArray.Length; num++)
                {
                    ch = chArray[num];
                    if ((ch >= ' ') && (ch < 'Ā'))
                    {
                        image = this.m_Images[ch - ' '];
                        int xWidth = image.xWidth;
                        int num11 = image.yHeight;
                        short* numPtr3 = numPtr2;
                        numPtr3 += (yHeight - num11) * num6;
                        int num8 = num6 - xWidth;
                        int num9 = image.xDelta - xWidth;
                        fixed (byte* numRef3 = image.xyPixels)
                        {
                            byte* numPtr4 = numRef3;
                            int num12 = 0;
                            while (num12 < num11)
                            {
                                int num13 = xWidth >> 2;
                                int num14 = xWidth & 3;
                                while (--num13 >= 0)
                                {
                                    numPtr3[0] = numRef[numPtr4[0]];
                                    numPtr3[1] = numRef[numPtr4[1]];
                                    numPtr3[2] = numRef[numPtr4[2]];
                                    numPtr3[3] = numRef[numPtr4[3]];
                                    numPtr3 += 4;
                                    numPtr4 += 4;
                                }
                                while (--num14 >= 0)
                                {
                                    numPtr3++;
                                    numPtr3[0] = numRef[*(numPtr4++)];
                                }
                                num12++;
                                numPtr3 += num8;
                                numPtr4 += num9;
                            }
                        }
                        numPtr2 += image.xWidth;
                    }
                    else if (ch == '\n')
                    {
                        pvSrc += num7;
                        numPtr2 = pvSrc;
                    }
                }
                texture.Unlock();
                return texture;
            }
        }

        public void Dispose()
        {
            this.m_Cache.Dispose();
            this.m_Cache = null;
            this.m_Palette = null;
            this.m_Images = null;
            m_Buffer = null;
            this.m_WrapCache.Clear();
            this.m_WrapCache = null;
        }

        public Texture GetString(string String, IHue Hue)
        {
            return this.m_Cache[String, Hue];
        }

        public int GetStringWidth(string text)
        {
            if ((text == null) || (text.Length <= 0))
            {
                return 0;
            }
            char[] chArray = text.ToCharArray();
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < chArray.Length; i++)
            {
                char ch = chArray[i];
                if ((ch >= ' ') && (ch < 'Ā'))
                {
                    FontImage image = this.m_Images[ch - ' '];
                    num += image.xWidth;
                    if (num > num2)
                    {
                        num2 = num;
                    }
                }
                else if (ch == '\n')
                {
                    num = 0;
                }
            }
            return num2;
        }

        public static void Reformat()
        {
            string path = Engine.FileManager.ResolveMUL(Files.Fonts);
            if (!File.Exists(path))
            {
                throw new InvalidOperationException(string.Format("Unable to reformat the font file, it doesn't exist. (inputPath={0})", path));
            }
            FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryReader reader = new BinaryReader(input);
            FileStream output = new FileStream(Engine.FileManager.BasePath("Data/QuickLoad/Fonts.mul"), FileMode.Create, FileAccess.Write, FileShare.None);
            BinaryWriter writer = new BinaryWriter(output);
            FileInfo info = new FileInfo(path);
            writer.Write(info.LastWriteTime.ToFileTime());
            writer.Write(10);
            writer.Write(new byte[80]);
            for (int i = 0; i < 10; i++)
            {
                writer.Flush();
                long length = output.Length;
                output.Seek((long) (12 + (i * 8)), SeekOrigin.Begin);
                writer.Write((int) length);
                output.Seek(length, SeekOrigin.Begin);
                reader.ReadByte();
                int num3 = 0;
                ArrayList list = new ArrayList();
                list.Add((short) 0);
                for (int j = 0; j < 0xe0; j++)
                {
                    int num5 = reader.ReadByte();
                    int num6 = reader.ReadByte();
                    int num7 = reader.ReadByte();
                    byte[,] buffer = new byte[num5, num6];
                    for (int m = 0; m < num6; m++)
                    {
                        for (int num9 = 0; num9 < num5; num9++)
                        {
                            int num10 = reader.ReadInt16() & 0x7fff;
                            int count = -1;
                            if (num10 != 0)
                            {
                                num10 |= 0x8000;
                            }
                            for (int num12 = 0; num12 < list.Count; num12++)
                            {
                                if (((short) list[num12]) == ((short) num10))
                                {
                                    count = num12;
                                    break;
                                }
                            }
                            if (count == -1)
                            {
                                count = list.Count;
                                list.Add((short) num10);
                            }
                            buffer[num9, m] = (byte) count;
                        }
                    }
                    writer.Write((byte) num5);
                    writer.Write((byte) num6);
                    writer.Write((byte) num7);
                    num3 += 3;
                    for (int n = 0; n < num6; n++)
                    {
                        for (int num14 = 0; num14 < num5; num14++)
                        {
                            writer.Write(buffer[num14, n]);
                        }
                    }
                    num3 += num5 * num6;
                }
                writer.Write(list.Count);
                num3 += 4;
                for (int k = 0; k < list.Count; k++)
                {
                    writer.Write((short) list[k]);
                }
                num3 += list.Count * 2;
                length = output.Length;
                output.Seek((long) ((12 + (i * 8)) + 4), SeekOrigin.Begin);
                writer.Write(num3);
                output.Seek(length, SeekOrigin.Begin);
            }
            reader.Close();
            writer.Close();
        }

        public override string ToString()
        {
            return string.Format("<ASCII Font #{0}>", this.m_FontID);
        }

        public string Name
        {
            get
            {
                return string.Format("Font[{0}]", this.m_FontID);
            }
        }

        public Hashtable WrapCache
        {
            get
            {
                return this.m_WrapCache;
            }
        }
    }
}

