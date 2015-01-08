namespace Client
{
    using System;
    using System.IO;

    public class Hues
    {
        private static IHue m_Bright;
        private static HDefault m_Default;
        private static HGrayscale m_Grayscale;
        private static HueData[] m_HueData;
        private static IHue[,] m_NotorietyHues = new IHue[7, 2];
        private static HPartial[] m_Partial;
        private static HRegular[] m_Regular;

        static unsafe Hues()
        {
            Debug.TimeBlock("Initializing Hues");
            m_Default = new HDefault();
            m_HueData = new HueData[0xbb8];
            m_Partial = new HPartial[0xbb8];
            m_Regular = new HRegular[0xbb8];
            string path = "Data/QuickLoad/Hues.mul";
            string str2 = Engine.FileManager.BasePath(path);
            FileInfo info = new FileInfo(Engine.FileManager.ResolveMUL(Files.Hues));
            FileInfo info2 = new FileInfo(Engine.FileManager.ResolveMUL(Files.Verdata));
            if (File.Exists(str2))
            {
                BinaryReader reader = new BinaryReader(Engine.FileManager.OpenBaseMUL(path));
                DateTime time = DateTime.FromFileTime(reader.ReadInt64());
                DateTime time2 = DateTime.FromFileTime(reader.ReadInt64());
                if ((info.LastWriteTime == time) && (info2.LastWriteTime == time2))
                {
                    int num = 0xbb8;
                    int num2 = 0;
                    byte[] src = reader.ReadBytes(num * 0x44);
                    int srcOffset = 0;
                    while (num2 < num)
                    {
                        HueData data = new HueData {
                            colors = new ushort[0x40]
                        };
                        Buffer.BlockCopy(src, srcOffset, data.colors, 0x40, 0x40);
                        srcOffset += 0x44;
                        m_HueData[num2++] = data;
                    }
                    reader.Close();
                    Patch();
                    Debug.EndBlock();
                    return;
                }
                reader.Close();
            }
            int count = 0x40d1c;
            byte[] buffer2 = new byte[count];
            Stream stream = Engine.FileManager.OpenMUL(Files.Hues);
            stream.Read(buffer2, 0, count);
            stream.Close();
            fixed (byte* numRef = buffer2)
            {
                int num5 = 0;
                int num6 = 0;
                short* numPtr = (short*) numRef;
                do
                {
                    numPtr += 2;
                    int num7 = 0;
                    do
                    {
                        HueData data2 = new HueData {
                            colors = new ushort[0x40]
                        };
                        for (int j = 0; j < 0x20; j++)
                        {
                            numPtr++;
                            data2.colors[0x20 + j] = (ushort) numPtr[0];
                        }
                        numPtr++;
                        data2.tableStart = numPtr[0];
                        numPtr++;
                        data2.tableEnd = numPtr[0];
                        m_HueData[num5++] = data2;
                        numPtr += 10;
                    }
                    while (++num7 < 8);
                }
                while (++num6 < 0x177);
            }
            Stream stream2 = Engine.FileManager.OpenMUL(Files.Verdata);
            buffer2 = new byte[stream2.Length];
            stream2.Read(buffer2, 0, buffer2.Length);
            stream2.Close();
            fixed (byte* numRef2 = buffer2)
            {
                int* numPtr2 = (int*) numRef2;
                numPtr2++;
                int num9 = numPtr2[0];
                int num10 = 0;
                while (num10++ < num9)
                {
                    numPtr2++;
                    int num11 = numPtr2[0];
                    if (num11 == 0x20)
                    {
                        numPtr2++;
                        int num12 = numPtr2[0];
                        numPtr2++;
                        int num13 = numPtr2[0];
                        numPtr2++;
                        int num14 = numPtr2[0];
                        numPtr2++;
                        int num15 = numPtr2[0];
                        short* numPtr3 = (short*) ((numRef2 + num13) + 4);
                        for (int k = 0; k < 8; k++)
                        {
                            HueData data3 = new HueData {
                                colors = new ushort[0x40]
                            };
                            for (int m = 0; m < 0x20; m++)
                            {
                                numPtr3++;
                                data3.colors[0x20 + m] = (ushort) numPtr3[0];
                            }
                            numPtr3++;
                            data3.tableStart = numPtr3[0];
                            numPtr3++;
                            data3.tableEnd = numPtr3[0];
                            m_HueData[(num12 << 3) + k] = data3;
                            numPtr3 += 10;
                        }
                    }
                    else
                    {
                        numPtr2 += 4;
                    }
                }
            }
            BinaryWriter writer = new BinaryWriter(Engine.FileManager.CreateBaseMUL(str2));
            writer.Write(info.LastWriteTime.ToFileTime());
            writer.Write(info2.LastWriteTime.ToFileTime());
            int num18 = 0xbb8;
            for (int i = 0; i < num18; i++)
            {
                HueData data4 = m_HueData[i];
                for (int n = 0; n < 0x20; n++)
                {
                    ushort[] numArray;
                    IntPtr ptr;
                    (numArray = data4.colors)[(int) (ptr = (IntPtr) (0x20 + n))] = (ushort) (numArray[(int) ptr] | 0x8000);
                    writer.Write(data4.colors[0x20 + n]);
                }
                writer.Write(data4.tableStart);
                writer.Write(data4.tableEnd);
            }
            writer.Flush();
            writer.Close();
            Patch();
            Debug.EndBlock();
        }

        public static void ClearNotos()
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    m_NotorietyHues[i, j] = null;
                }
            }
        }

        public static void Dispose()
        {
            if (m_Default != null)
            {
                m_Default.Dispose();
                m_Default = null;
            }
            if (m_Grayscale != null)
            {
                m_Grayscale.Dispose();
                m_Grayscale = null;
            }
            if (m_Bright != null)
            {
                m_Bright.Dispose();
                m_Bright = null;
            }
            for (int i = 0; i < 0xbb8; i++)
            {
                if (m_Partial[i] != null)
                {
                    m_Partial[i].Dispose();
                    m_Partial[i] = null;
                }
                if (m_Regular[i] != null)
                {
                    m_Regular[i].Dispose();
                    m_Regular[i] = null;
                }
            }
            m_Partial = null;
            m_Regular = null;
            m_HueData = null;
        }

        public static HueData GetData(int HueIndex)
        {
            return m_HueData[HueIndex];
        }

        public static IHue GetItemHue(int itemID, int hue)
        {
            hue ^= ((Map.m_ItemFlags[itemID & 0x3fff].Value >> 3) & 0x8000) ^ 0x8000;
            return Load(hue);
        }

        public static IHue GetNotoriety(Notoriety n)
        {
            return GetNotoriety(n, true);
        }

        public static IHue GetNotoriety(Notoriety n, bool full)
        {
            if ((n >= Notoriety.Innocent) && (n <= Notoriety.Vendor))
            {
                int index = ((int) n) - 1;
                int num2 = full ? 1 : 0;
                IHue hue = m_NotorietyHues[index, num2];
                if (hue == null)
                {
                    hue = m_NotorietyHues[index, num2] = Load(World.CharData.NotorietyHues[index] | (num2 << 15));
                }
                return hue;
            }
            return Default;
        }

        public static HueData GetNotorietyData(Notoriety n)
        {
            if ((n >= Notoriety.Innocent) && (n <= Notoriety.Vendor))
            {
                return m_HueData[World.CharData.NotorietyHues[((int) n) - 1]];
            }
            return new HueData();
        }

        public static IHue Load(int hueID)
        {
            int num = hueID & 0x7fff;
            hueID &= 0xffff;
            if ((num == 0) || (num > 0xbb8))
            {
                return m_Default;
            }
            if ((hueID & 0x8000) == 0)
            {
                IHue hue = m_Partial[hueID - 1];
                if (hue == null)
                {
                    hue = m_Partial[hueID - 1] = new HPartial(m_HueData[hueID - 1], hueID);
                }
                return hue;
            }
            IHue hue2 = m_Regular[num - 1];
            if (hue2 == null)
            {
                hue2 = m_Regular[num - 1] = new HRegular(m_HueData[num - 1], hueID);
            }
            return hue2;
        }

        public static IHue LoadByRgb(int rgbColor)
        {
            int num = (rgbColor >> 0x10) & 0xff;
            int num2 = (rgbColor >> 8) & 0xff;
            int num3 = rgbColor & 0xff;
            num = num >> 3;
            num2 = num2 >> 3;
            num3 = num3 >> 3;
            int num4 = 0x3e8;
            int hueID = 0;
            for (int i = 0; i < 0xbb8; i++)
            {
                int num7 = m_HueData[i].colors[0x38];
                int num8 = (num7 >> 10) & 0x1f;
                int num9 = (num7 >> 5) & 0x1f;
                int num10 = num7 & 0x1f;
                num8 = Math.Abs((int) (num8 - num));
                num9 = Math.Abs((int) (num9 - num2));
                num10 = Math.Abs((int) (num10 - num3));
                int num11 = (num8 + num9) + num10;
                if (num11 < num4)
                {
                    num4 = num11;
                    hueID = i + 1;
                }
            }
            return Load(hueID);
        }

        public static void Patch()
        {
            HueData data = m_HueData[0xbb7];
            for (int i = 0; i < 0x20; i++)
            {
                int num2 = 8 + ((i * 7) / 8);
                if (num2 > 0x1f)
                {
                    num2 = 0x1f;
                }
                data.colors[0x20 + i] = (ushort) (((0x8000 | (num2 << 10)) | (num2 << 5)) | num2);
            }
            m_HueData[0xbb7] = data;
        }

        public static IHue Bright
        {
            get
            {
                if (m_Bright == null)
                {
                    m_Bright = Load(0x3b2);
                }
                return m_Bright;
            }
        }

        public static IHue Default
        {
            get
            {
                return m_Default;
            }
        }

        public static IHue Grayscale
        {
            get
            {
                if (m_Grayscale == null)
                {
                    m_Grayscale = new HGrayscale();
                }
                return m_Grayscale;
            }
        }

        private class HBright : IHue
        {
            private AnimationCache m_Anim;
            private GumpCache m_Gumps;
            private ItemCache m_Items;
            private LandCache m_Land;
            private TextureCache m_Textures;

            public unsafe void Apply(LockData ld)
            {
                ushort* pvSrc = (ushort*) ld.pvSrc;
                int num = ld.Height * (ld.Pitch >> 1);
                while (num-- != 0)
                {
                    ushort color = pvSrc[0];
                    if ((color & 0x8000) != 0x8000)
                    {
                        pvSrc++;
                    }
                    else
                    {
                        int num3 = (int) (Engine.GrayScale(color) * 1.15);
                        if (num3 > 0x1f)
                        {
                            num3 = 0x1f;
                        }
                        pvSrc++;
                        pvSrc[0] = (ushort) (((num3 | (num3 << 5)) | (num3 << 10)) | (color & 0x8000));
                    }
                }
            }

            public void Apply(Texture Target)
            {
                LockData ld = Target.Lock(LockFlags.ReadWrite);
                this.Apply(ld);
                Target.Unlock();
            }

            public unsafe void CopyPixels(void* pvSrc, void* pvDest, int Pixels)
            {
                ushort* numPtr = (ushort*) pvSrc;
                ushort* numPtr2 = (ushort*) pvDest;
                while (--Pixels >= 0)
                {
                    numPtr++;
                    ushort color = numPtr[0];
                    int num2 = (int) (Engine.GrayScale(color) * 1.15);
                    if (num2 > 0x1f)
                    {
                        num2 = 0x1f;
                    }
                    numPtr2++;
                    numPtr2[0] = (ushort) ((((num2 << 10) | (num2 << 5)) | num2) | 0x8000);
                }
            }

            public void Dispose()
            {
                if (this.m_Items != null)
                {
                    this.m_Items.Dispose();
                    this.m_Items = null;
                }
                if (this.m_Gumps != null)
                {
                    this.m_Gumps.Dispose();
                    this.m_Gumps = null;
                }
                if (this.m_Land != null)
                {
                    this.m_Land.Dispose();
                    this.m_Land = null;
                }
                if (this.m_Textures != null)
                {
                    this.m_Textures.Dispose();
                    this.m_Textures = null;
                }
                if (this.m_Anim != null)
                {
                    this.m_Anim.Dispose();
                    this.m_Anim = null;
                }
            }

            public unsafe void FillLine(ushort* pSrc, ushort* pDest, ushort* pEnd)
            {
                ushort* numPtr = pDest;
                while (pDest < pEnd)
                {
                    ushort num = pSrc[0];
                    ushort num2 = pSrc[1];
                    pSrc += 2;
                    numPtr += num2;
                    if (num != 0)
                    {
                        num = this.Pixel((ushort) (num | 0x8000));
                        while (pDest < numPtr)
                        {
                            pDest++;
                            pDest[0] = num;
                        }
                    }
                    else
                    {
                        pDest += num2;
                    }
                }
            }

            public unsafe void FillLine(void* pSrc, void* pDest, int Count)
            {
                short* numPtr = (short*) pSrc;
                byte* numPtr2 = (byte*) pDest;
                while (--Count >= 0)
                {
                    numPtr++;
                    int num = numPtr[0];
                    numPtr++;
                    int num2 = numPtr[0];
                    if (num != 0)
                    {
                        num = this.Pixel((ushort) (num | 0x8000));
                        int num3 = num2 >> 1;
                        int num4 = (num << 0x10) | num;
                        int* numPtr3 = (int*) numPtr2;
                        while (--num3 >= 0)
                        {
                            numPtr3++;
                            numPtr3[0] = num4;
                        }
                        numPtr2 = (byte*) numPtr3;
                        switch ((num2 & 1))
                        {
                            case 0:
                            {
                                continue;
                            }
                            case 1:
                            {
                                *((short*) numPtr2) = (short) num;
                                numPtr2 += 2;
                                continue;
                            }
                        }
                    }
                    else
                    {
                        numPtr2 += num2 << 1;
                    }
                }
            }

            public unsafe void FillPixels(void* pvDest, int Color, int Pixels)
            {
                Color = this.Pixel((ushort) (Color | 0x8000));
                int num = Pixels >> 1;
                int* numPtr = (int*) pvDest;
                int num2 = (Color << 0x10) | Color;
                while (--num >= 0)
                {
                    numPtr++;
                    numPtr[0] = num2;
                }
                if ((Pixels & 1) != 0)
                {
                    *((short*) numPtr) = (short) Color;
                }
            }

            public Frames GetAnimation(int RealID)
            {
                if (this.m_Anim == null)
                {
                    this.m_Anim = new AnimationCache(this);
                }
                return this.m_Anim[RealID];
            }

            public Texture GetGump(int GumpID)
            {
                if (this.m_Gumps == null)
                {
                    this.m_Gumps = new GumpCache(this);
                }
                return this.m_Gumps[GumpID];
            }

            public Texture GetItem(int ItemID)
            {
                if (this.m_Items == null)
                {
                    this.m_Items = new ItemCache(this);
                }
                return this.m_Items[ItemID];
            }

            public Texture GetLand(int LandID)
            {
                if (this.m_Land == null)
                {
                    this.m_Land = new LandCache(this);
                }
                return this.m_Land[LandID];
            }

            public Texture GetTexture(int TextureID)
            {
                if (this.m_Textures == null)
                {
                    this.m_Textures = new TextureCache(this);
                }
                return this.m_Textures[TextureID];
            }

            public int HueID()
            {
                return 0xffff;
            }

            public ushort Pixel(ushort input)
            {
                int num = (int) (Engine.GrayScale(input) * 1.15);
                if (num > 0x1f)
                {
                    num = 0x1f;
                }
                return (ushort) ((((num << 10) | (num << 5)) | num) | (input & 0x8000));
            }

            public override string ToString()
            {
                return "<Amplified Grayscale>";
            }
        }

        public class HDefault : IHue, IHintable
        {
            private const int DoubleOpaque = -2147450880;
            private AnimationCache m_Anim;
            private Texture[] m_Gumps = new Texture[0x10000];
            private bool[] m_ItemHint = new bool[0x4000];
            private Texture[] m_Items = new Texture[0x4000];
            private Texture[] m_Land = new Texture[0x4000];
            private bool[] m_LandHint = new bool[0x4000];
            private bool[] m_TextureHint = new bool[0x1000];
            private Texture[] m_Textures = new Texture[0x1000];

            public void Apply(LockData ld)
            {
            }

            public void Apply(Texture Target)
            {
            }

            public unsafe void CopyPixels(void* pvSrc, void* pvDest, int Pixels)
            {
                int* numPtr = (int*) pvSrc;
                int* numPtr2 = (int*) pvDest;
                int* numPtr3 = numPtr + ((Pixels >> 1) & -4);
                while (numPtr < numPtr3)
                {
                    numPtr2[0] = numPtr[0] | -2147450880;
                    numPtr2[1] = numPtr[1] | -2147450880;
                    numPtr2[2] = numPtr[2] | -2147450880;
                    numPtr2[3] = numPtr[3] | -2147450880;
                    numPtr2 += 4;
                    numPtr += 4;
                }
                int num = (Pixels >> 1) & 3;
                switch (num)
                {
                    case 1:
                        goto Label_00A2;

                    case 2:
                        break;

                    case 3:
                        numPtr2[2] = numPtr[2] | -2147450880;
                        break;

                    default:
                        goto Label_00AE;
                }
                numPtr2[1] = numPtr[1] | -2147450880;
            Label_00A2:
                numPtr2[0] = numPtr[0] | -2147450880;
            Label_00AE:
                numPtr2 += num;
                numPtr += num;
                if ((Pixels & 1) != 0)
                {
                    *((short*) numPtr2) = (short) (0x8000 | *(((ushort*) numPtr)));
                }
            }

            public void Dispose()
            {
                int index = 0;
                while (index < 0x1000)
                {
                    if (this.m_Gumps[index] != null)
                    {
                        this.m_Gumps[index].Dispose();
                        this.m_Gumps[index] = null;
                    }
                    if (this.m_Items[index] != null)
                    {
                        this.m_Items[index].Dispose();
                        this.m_Items[index] = null;
                    }
                    if (this.m_Land[index] != null)
                    {
                        this.m_Land[index].Dispose();
                        this.m_Land[index] = null;
                    }
                    if (this.m_Textures[index] != null)
                    {
                        this.m_Textures[index].Dispose();
                        this.m_Textures[index] = null;
                    }
                    index++;
                }
                while (index < 0x4000)
                {
                    if (this.m_Gumps[index] != null)
                    {
                        this.m_Gumps[index].Dispose();
                        this.m_Gumps[index] = null;
                    }
                    if (this.m_Items[index] != null)
                    {
                        this.m_Items[index].Dispose();
                        this.m_Items[index] = null;
                    }
                    if (this.m_Land[index] != null)
                    {
                        this.m_Land[index].Dispose();
                        this.m_Land[index] = null;
                    }
                    index++;
                }
                while (index < 0x10000)
                {
                    if (this.m_Gumps[index] != null)
                    {
                        this.m_Gumps[index].Dispose();
                        this.m_Gumps[index] = null;
                    }
                    index++;
                }
                if (this.m_Anim != null)
                {
                    this.m_Anim.Dispose();
                    this.m_Anim = null;
                }
                this.m_Gumps = null;
                this.m_Items = null;
                this.m_Land = null;
                this.m_Textures = null;
                this.m_ItemHint = null;
                this.m_LandHint = null;
                this.m_TextureHint = null;
            }

            public unsafe void FillLine(ushort* pSrc, ushort* pDest, ushort* pEnd)
            {
                ushort* numPtr = pDest;
                while (pDest < pEnd)
                {
                    ushort num = pSrc[0];
                    ushort num2 = pSrc[1];
                    pSrc += 2;
                    numPtr += num2;
                    if (num != 0)
                    {
                        num = (ushort) (num | 0x8000);
                        while (pDest < numPtr)
                        {
                            pDest++;
                            pDest[0] = num;
                        }
                    }
                    else
                    {
                        pDest += num2;
                    }
                }
            }

            public unsafe void FillPixels(void* pvDest, int Color, int Pixels)
            {
                Texture.FillPixels(pvDest, Color, Pixels);
            }

            public Frames GetAnimation(int RealID)
            {
                if (this.m_Anim == null)
                {
                    this.m_Anim = new AnimationCache(this);
                }
                return this.m_Anim[RealID];
            }

            public Texture GetGump(int GumpID)
            {
                GumpID &= 0xffff;
                if (this.m_Gumps[GumpID] == null)
                {
                    this.m_Gumps[GumpID] = Engine.m_Gumps.ReadFromDisk(GumpID, this);
                }
                return this.m_Gumps[GumpID];
            }

            public Texture GetItem(int ItemID)
            {
                ItemID &= 0x3fff;
                if (this.m_Items[ItemID] == null)
                {
                    this.m_Items[ItemID] = Engine.ItemArt.ReadFromDisk(ItemID, this);
                    this.m_ItemHint[ItemID] = true;
                }
                return this.m_Items[ItemID];
            }

            public Texture GetLand(int LandID)
            {
                LandID &= 0x3fff;
                if (this.m_Land[LandID] == null)
                {
                    this.m_Land[LandID] = Engine.LandArt.ReadFromDisk(LandID, this);
                    this.m_LandHint[LandID] = true;
                }
                return this.m_Land[LandID];
            }

            public Texture GetTexture(int TextureID)
            {
                TextureID &= 0xfff;
                if (this.m_Textures[TextureID] == null)
                {
                    this.m_Textures[TextureID] = Engine.TextureArt.ReadFromDisk(TextureID, this);
                    this.m_TextureHint[TextureID] = true;
                }
                return this.m_Textures[TextureID];
            }

            public bool HintItem(int ItemID)
            {
                ItemID &= 0x3fff;
                if (!this.m_ItemHint[ItemID])
                {
                    this.m_ItemHint[ItemID] = true;
                    return false;
                }
                return true;
            }

            public bool HintLand(int LandID)
            {
                LandID &= 0x3fff;
                int texture = Map.GetTexture(LandID);
                MapLighting.CheckStretchTable();
                if (!MapLighting.m_AlwaysStretch[LandID])
                {
                    if (!this.m_LandHint[LandID])
                    {
                        this.m_LandHint[LandID] = true;
                        if ((texture > 0) && (texture < 0x1000))
                        {
                            this.m_TextureHint[texture] = true;
                        }
                        return false;
                    }
                }
                else if (((texture > 0) && (texture < 0x1000)) && !this.m_TextureHint[texture])
                {
                    this.m_TextureHint[texture] = true;
                    return false;
                }
                return true;
            }

            public int HueID()
            {
                return 0;
            }

            public ushort Pixel(ushort input)
            {
                return input;
            }

            public override string ToString()
            {
                return "<Default>";
            }
        }

        public class HFill : IHue
        {
            private ushort m_Color;

            public HFill(int rgb32)
            {
                int num = (rgb32 >> 0x10) & 0xff;
                int num2 = (rgb32 >> 8) & 0xff;
                int num3 = rgb32 & 0xff;
                num = num >> 3;
                num2 = num2 >> 3;
                num3 = num3 >> 3;
                num = num << 10;
                num2 = num2 << 5;
                this.m_Color = (ushort) ((num | num2) | num3);
                this.m_Color = (ushort) (this.m_Color | 0x8000);
            }

            public void Apply(LockData ld)
            {
                throw new InvalidOperationException();
            }

            public void Apply(Texture Target)
            {
                throw new InvalidOperationException();
            }

            public unsafe void CopyPixels(void* pvSrc, void* pvDest, int count)
            {
                ushort* numPtr = (ushort*) pvDest;
                ushort* numPtr2 = numPtr + count;
                ushort color = this.m_Color;
                while (numPtr < numPtr2)
                {
                    numPtr++;
                    numPtr[0] = color;
                }
            }

            public void Dispose()
            {
            }

            public override bool Equals(object obj)
            {
                return ((obj is Hues.HFill) && (((Hues.HFill) obj).m_Color == this.m_Color));
            }

            public unsafe void FillLine(ushort* pSrc, ushort* pDest, ushort* pEnd)
            {
                throw new InvalidOperationException();
            }

            public unsafe void FillLine(void* pSrc, void* pDest, int Count)
            {
                throw new InvalidOperationException();
            }

            public unsafe void FillPixels(void* pvDest, int Color, int Pixels)
            {
                throw new InvalidOperationException();
            }

            public Frames GetAnimation(int RealID)
            {
                throw new InvalidOperationException();
            }

            public Texture GetGump(int GumpID)
            {
                throw new InvalidOperationException();
            }

            public override int GetHashCode()
            {
                return this.m_Color;
            }

            public Texture GetItem(int ItemID)
            {
                throw new InvalidOperationException();
            }

            public Texture GetLand(int LandID)
            {
                throw new InvalidOperationException();
            }

            public Texture GetTexture(int TextureID)
            {
                throw new InvalidOperationException();
            }

            public int HueID()
            {
                return 0xffff;
            }

            public ushort Pixel(ushort input)
            {
                throw new InvalidOperationException();
            }
        }

        private class HGrayscale : IHue, IHintable
        {
            private AnimationCache m_Anim;
            public static ushort[] m_Colors = new ushort[0x10000];
            private GumpCache m_Gumps;
            private bool[] m_ItemHint = new bool[0x4000];
            private Texture[] m_Items = new Texture[0x4000];
            private Texture[] m_Land = new Texture[0x4000];
            private bool[] m_LandHint = new bool[0x4000];
            private bool[] m_TextureHint = new bool[0x1000];
            private Texture[] m_Textures = new Texture[0x1000];

            static unsafe HGrayscale()
            {
                fixed (ushort* numRef = m_Colors)
                {
                    ushort* numPtr = numRef + 0x8000;
                    int num = 0x20;
                    float num4 = 0f;
                    float num5 = 0f;
                    float num6 = 0f;
                    while (--num >= 0)
                    {
                        int num2 = 0x20;
                        while (--num2 >= 0)
                        {
                            int num3 = (int) (num6 * 31f);
                            numPtr[0] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[1] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[2] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[3] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[4] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[5] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[6] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[7] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[8] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[9] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[10] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[11] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[12] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[13] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[14] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[15] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[0x10] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[0x11] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[0x12] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[0x13] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[20] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[0x15] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[0x16] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[0x17] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[0x18] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[0x19] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[0x1a] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[0x1b] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[0x1c] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[0x1d] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[30] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            num3 = (int) (num6 * 31f);
                            numPtr[0x1f] = (ushort) (0x8000 | (num3 * 0x421));
                            num6 += 0.003677419f;
                            numPtr += 0x20;
                            num5 += 0.01893548f;
                            num6 = num5;
                        }
                        num4 += 0.009645161f;
                        num5 = num6 = num4;
                    }
                }
            }

            public unsafe void Apply(LockData ld)
            {
                ushort* pvSrc = (ushort*) ld.pvSrc;
                int height = ld.Height;
                int width = ld.Width;
                int num3 = 0;
                int num4 = (ld.Pitch >> 1) - width;
                fixed (ushort* numRef = m_Colors)
                {
                    ushort* numPtr2 = numRef;
                    while (--height >= 0)
                    {
                        num3 = width;
                        while (--num3 >= 0)
                        {
                            pvSrc++;
                            pvSrc[0] = numPtr2[pvSrc[0]];
                        }
                        pvSrc += num4;
                    }
                }
            }

            public void Apply(Texture Target)
            {
                LockData ld = Target.Lock(LockFlags.ReadWrite);
                this.Apply(ld);
                Target.Unlock();
            }

            public unsafe void CopyPixels(void* pvSrc, void* pvDest, int Pixels)
            {
                ushort* numPtr = (ushort*) pvSrc;
                ushort* numPtr2 = (ushort*) pvDest;
                ushort* numPtr3 = numPtr + Pixels;
                fixed (ushort* numRef = m_Colors)
                {
                    while (numPtr < numPtr3)
                    {
                        numPtr2++;
                        numPtr++;
                        numPtr2[0] = numRef[numPtr[0] | 0x8000];
                    }
                }
            }

            public void Dispose()
            {
                int index = 0;
                while (index < 0x1000)
                {
                    if (this.m_Items[index] != null)
                    {
                        this.m_Items[index].Dispose();
                        this.m_Items[index] = null;
                    }
                    if (this.m_Land[index] != null)
                    {
                        this.m_Land[index].Dispose();
                        this.m_Land[index] = null;
                    }
                    if (this.m_Textures[index] != null)
                    {
                        this.m_Textures[index].Dispose();
                        this.m_Textures[index] = null;
                    }
                    index++;
                }
                while (index < 0x4000)
                {
                    if (this.m_Items[index] != null)
                    {
                        this.m_Items[index].Dispose();
                        this.m_Items[index] = null;
                    }
                    if (this.m_Land[index] != null)
                    {
                        this.m_Land[index].Dispose();
                        this.m_Land[index] = null;
                    }
                    index++;
                }
                this.m_Items = null;
                this.m_Land = null;
                this.m_Textures = null;
                this.m_ItemHint = null;
                this.m_LandHint = null;
                this.m_TextureHint = null;
                if (this.m_Gumps != null)
                {
                    this.m_Gumps.Dispose();
                    this.m_Gumps = null;
                }
                if (this.m_Anim != null)
                {
                    this.m_Anim.Dispose();
                    this.m_Anim = null;
                }
                m_Colors = null;
            }

            public unsafe void FillLine(ushort* pSrc, ushort* pDest, ushort* pEnd)
            {
                ushort* numPtr = pDest;
                while (pDest < pEnd)
                {
                    ushort num = pSrc[0];
                    ushort num2 = pSrc[1];
                    pSrc += 2;
                    numPtr += num2;
                    if (num != 0)
                    {
                        num = m_Colors[num | 0x8000];
                        while (pDest < numPtr)
                        {
                            pDest++;
                            pDest[0] = num;
                        }
                    }
                    else
                    {
                        pDest += num2;
                    }
                }
            }

            public unsafe void FillPixels(void* pvDest, int Color, int Pixels)
            {
                Color = m_Colors[Color | 0x8000];
                int num = Pixels >> 1;
                int* numPtr = (int*) pvDest;
                int num2 = (Color << 0x10) | Color;
                while (--num >= 0)
                {
                    numPtr++;
                    numPtr[0] = num2;
                }
                if ((Pixels & 1) != 0)
                {
                    *((short*) numPtr) = (short) Color;
                }
            }

            public Frames GetAnimation(int RealID)
            {
                if (this.m_Anim == null)
                {
                    this.m_Anim = new AnimationCache(this);
                }
                return this.m_Anim[RealID];
            }

            public Texture GetGump(int GumpID)
            {
                if (this.m_Gumps == null)
                {
                    this.m_Gumps = new GumpCache(this);
                }
                return this.m_Gumps[GumpID];
            }

            public Texture GetItem(int ItemID)
            {
                ItemID &= 0x3fff;
                if (this.m_Items[ItemID] == null)
                {
                    this.m_Items[ItemID] = Engine.ItemArt.ReadFromDisk(ItemID, this);
                    this.m_ItemHint[ItemID] = true;
                }
                return this.m_Items[ItemID];
            }

            public Texture GetLand(int LandID)
            {
                LandID &= 0x3fff;
                if (this.m_Land[LandID] == null)
                {
                    this.m_Land[LandID] = Engine.LandArt.ReadFromDisk(LandID, this);
                    this.m_LandHint[LandID] = true;
                }
                return this.m_Land[LandID];
            }

            public Texture GetTexture(int TextureID)
            {
                TextureID &= 0xfff;
                if (this.m_Textures[TextureID] == null)
                {
                    this.m_Textures[TextureID] = Engine.TextureArt.ReadFromDisk(TextureID, this);
                    this.m_TextureHint[TextureID] = true;
                }
                return this.m_Textures[TextureID];
            }

            public bool HintItem(int ItemID)
            {
                ItemID &= 0x3fff;
                if (!this.m_ItemHint[ItemID])
                {
                    this.m_ItemHint[ItemID] = true;
                    return false;
                }
                return true;
            }

            public bool HintLand(int LandID)
            {
                LandID &= 0x3fff;
                int texture = Map.GetTexture(LandID);
                MapLighting.CheckStretchTable();
                if (!MapLighting.m_AlwaysStretch[LandID])
                {
                    if (!this.m_LandHint[LandID])
                    {
                        this.m_LandHint[LandID] = true;
                        if ((texture > 0) && (texture < 0x1000))
                        {
                            this.m_TextureHint[texture] = true;
                        }
                        return false;
                    }
                }
                else if (((texture > 0) && (texture < 0x1000)) && !this.m_TextureHint[texture])
                {
                    this.m_TextureHint[texture] = true;
                    return false;
                }
                return true;
            }

            public int HueID()
            {
                return 0xffff;
            }

            public ushort Pixel(ushort input)
            {
                return m_Colors[input];
            }

            public override string ToString()
            {
                return "<Grayscale>";
            }
        }

        private class HPartial : IHue
        {
            private int hue;
            private AnimationCache m_Anim;
            private HueData m_Data;
            private GumpCache m_Gumps;
            private ItemCache m_Items;
            private LandCache m_Land;
            private TextureCache m_Textures;

            public HPartial(HueData hd, int HueID)
            {
                this.hue = HueID;
                this.m_Data = hd;
            }

            public unsafe void Apply(LockData ld)
            {
                ushort* pvSrc = (ushort*) ld.pvSrc;
                int height = ld.Height;
                int width = ld.Width;
                int num3 = 0;
                int num4 = (ld.Pitch >> 1) - width;
                fixed (ushort* numRef = this.m_Data.colors)
                {
                    ushort* numPtr2 = numRef;
                    goto Label_0092;
                Label_0043:
                    num3 = width;
                    while (--num3 >= 0)
                    {
                        ushort num5 = pvSrc[0];
                        if (((num5 & 0x1f) == ((num5 >> 5) & 0x1f)) && ((num5 & 0x1f) == ((num5 >> 10) & 0x1f)))
                        {
                            pvSrc[0] = numPtr2[num5 >> 10];
                        }
                        pvSrc++;
                    }
                    pvSrc += num4;
                Label_008A:
                    if (--height >= 0)
                    {
                        goto Label_0043;
                    }
                Label_0092:
                    if (--height >= 0)
                    {
                        goto Label_008A;
                    }
                }
            }

            public void Apply(Texture Target)
            {
                LockData ld = Target.Lock(LockFlags.ReadWrite);
                this.Apply(ld);
                Target.Unlock();
            }

            public unsafe void CopyPixels(void* pvSrc, void* pvDest, int Pixels)
            {
                fixed (ushort* numRef = this.m_Data.colors)
                {
                    ushort* numPtr = (ushort*) pvSrc;
                    ushort* numPtr2 = (ushort*) pvDest;
                    ushort* numPtr3 = numPtr + Pixels;
                    while (numPtr < numPtr3)
                    {
                        numPtr++;
                        int num = numPtr[0];
                        if (((num & 0x1f) == ((num >> 5) & 0x1f)) && ((num & 0x1f) == ((num >> 10) & 0x1f)))
                        {
                            numPtr2++;
                            numPtr2[0] = numRef[(num >> 10) | 0x20];
                        }
                        else
                        {
                            numPtr2++;
                            numPtr2[0] = (ushort) (num | 0x8000);
                        }
                    }
                }
            }

            public void Dispose()
            {
                if (this.m_Items != null)
                {
                    this.m_Items.Dispose();
                    this.m_Items = null;
                }
                if (this.m_Gumps != null)
                {
                    this.m_Gumps.Dispose();
                    this.m_Gumps = null;
                }
                if (this.m_Land != null)
                {
                    this.m_Land.Dispose();
                    this.m_Land = null;
                }
                if (this.m_Textures != null)
                {
                    this.m_Textures.Dispose();
                    this.m_Textures = null;
                }
                if (this.m_Anim != null)
                {
                    this.m_Anim.Dispose();
                    this.m_Anim = null;
                }
            }

            public unsafe void FillLine(ushort* pSrc, ushort* pDest, ushort* pEnd)
            {
                ushort* numPtr = pDest;
                while (pDest < pEnd)
                {
                    ushort num = pSrc[0];
                    ushort num2 = pSrc[1];
                    pSrc += 2;
                    numPtr += num2;
                    if (num != 0)
                    {
                        if (((num & 0x1f) == ((num >> 5) & 0x1f)) && ((num & 0x1f) == ((num >> 10) & 0x1f)))
                        {
                            num = this.m_Data.colors[(num >> 10) | 0x20];
                        }
                        else
                        {
                            num = (ushort) (num | 0x8000);
                        }
                        while (pDest < numPtr)
                        {
                            pDest++;
                            pDest[0] = num;
                        }
                    }
                    else
                    {
                        pDest += num2;
                    }
                }
            }

            public unsafe void FillPixels(void* pvDest, int Color, int Pixels)
            {
                if (((Color & 0x1f) == ((Color >> 5) & 0x1f)) && ((Color & 0x1f) == ((Color >> 10) & 0x1f)))
                {
                    Color = this.m_Data.colors[(Color >> 10) | 0x20];
                }
                else
                {
                    Color |= 0x8000;
                }
                int num = Pixels >> 1;
                int* numPtr = (int*) pvDest;
                int num2 = (Color << 0x10) | Color;
                while (--num >= 0)
                {
                    numPtr++;
                    numPtr[0] = num2;
                }
                if ((Pixels & 1) != 0)
                {
                    *((short*) numPtr) = (short) Color;
                }
            }

            public Frames GetAnimation(int RealID)
            {
                if (this.m_Anim == null)
                {
                    this.m_Anim = new AnimationCache(this);
                }
                return this.m_Anim[RealID];
            }

            public Texture GetGump(int GumpID)
            {
                if (this.m_Gumps == null)
                {
                    this.m_Gumps = new GumpCache(this);
                }
                return this.m_Gumps[GumpID];
            }

            public Texture GetItem(int ItemID)
            {
                if (this.m_Items == null)
                {
                    this.m_Items = new ItemCache(this);
                }
                return this.m_Items[ItemID];
            }

            public Texture GetLand(int LandID)
            {
                if (this.m_Land == null)
                {
                    this.m_Land = new LandCache(this);
                }
                return this.m_Land[LandID];
            }

            public Texture GetTexture(int TextureID)
            {
                if (this.m_Textures == null)
                {
                    this.m_Textures = new TextureCache(this);
                }
                return this.m_Textures[TextureID];
            }

            public int HueID()
            {
                return this.hue;
            }

            public ushort Pixel(ushort c)
            {
                if (((c & 0x1f) == ((c >> 10) & 0x1f)) && ((c & 0x1f) == ((c >> 5) & 0x1f)))
                {
                    return this.m_Data.colors[c >> 10];
                }
                return c;
            }

            public override string ToString()
            {
                return string.Format("<Partial 0x{0:X4}>", this.hue);
            }
        }

        private class HRegular : IHue
        {
            private int hue;
            private AnimationCache m_Anim;
            private HueData m_Data;
            private GumpCache m_Gumps;
            private ItemCache m_Items;
            private LandCache m_Land;
            private TextureCache m_Textures;

            public HRegular(HueData hd, int HueID)
            {
                this.hue = HueID;
                this.m_Data = hd;
            }

            public unsafe void Apply(LockData ld)
            {
                ushort* pvSrc = (ushort*) ld.pvSrc;
                int height = ld.Height;
                int width = ld.Width;
                int num3 = 0;
                int num4 = (ld.Pitch >> 1) - width;
                fixed (ushort* numRef = this.m_Data.colors)
                {
                    ushort* numPtr2 = numRef;
                    while (--height >= 0)
                    {
                        num3 = width;
                        while (--num3 >= 0)
                        {
                            pvSrc++;
                            pvSrc[0] = numPtr2[pvSrc[0] >> 10];
                        }
                        pvSrc += num4;
                    }
                }
            }

            public void Apply(Texture Target)
            {
                LockData ld = Target.Lock(LockFlags.ReadWrite);
                this.Apply(ld);
                Target.Unlock();
            }

            public unsafe void CopyPixels(void* pvSrc, void* pvDest, int Pixels)
            {
                fixed (ushort* numRef = this.m_Data.colors)
                {
                    ushort* numPtr = (ushort*) pvSrc;
                    ushort* numPtr2 = (ushort*) pvDest;
                    ushort* numPtr3 = numPtr + (Pixels & -4);
                    while (numPtr < numPtr3)
                    {
                        numPtr2[0] = numRef[(numPtr[0] >> 10) | 0x20];
                        numPtr2[1] = numRef[(numPtr[1] >> 10) | 0x20];
                        numPtr2[2] = numRef[(numPtr[2] >> 10) | 0x20];
                        numPtr2[3] = numRef[(numPtr[3] >> 10) | 0x20];
                        numPtr2 += 4;
                        numPtr += 4;
                    }
                    switch ((Pixels & 3))
                    {
                        case 1:
                            goto Label_00D4;

                        case 2:
                            break;

                        case 3:
                            numPtr2[2] = numRef[(numPtr[2] >> 10) | 0x20];
                            break;

                        default:
                            goto Label_00E7;
                    }
                    numPtr2[1] = numRef[(numPtr[1] >> 10) | 0x20];
                Label_00D4:
                    numPtr2[0] = numRef[(numPtr[0] >> 10) | 0x20];
                }
            Label_00E7:;
            }

            public void Dispose()
            {
                if (this.m_Items != null)
                {
                    this.m_Items.Dispose();
                    this.m_Items = null;
                }
                if (this.m_Gumps != null)
                {
                    this.m_Gumps.Dispose();
                    this.m_Gumps = null;
                }
                if (this.m_Land != null)
                {
                    this.m_Land.Dispose();
                    this.m_Land = null;
                }
                if (this.m_Textures != null)
                {
                    this.m_Textures.Dispose();
                    this.m_Textures = null;
                }
                if (this.m_Anim != null)
                {
                    this.m_Anim.Dispose();
                    this.m_Anim = null;
                }
            }

            public unsafe void FillLine(ushort* pSrc, ushort* pDest, ushort* pEnd)
            {
                ushort* numPtr = pDest;
                while (pDest < pEnd)
                {
                    ushort num = pSrc[0];
                    ushort num2 = pSrc[1];
                    pSrc += 2;
                    numPtr += num2;
                    if (num != 0)
                    {
                        num = this.m_Data.colors[(num >> 10) | 0x20];
                        while (pDest < numPtr)
                        {
                            pDest++;
                            pDest[0] = num;
                        }
                    }
                    else
                    {
                        pDest += num2;
                    }
                }
            }

            public unsafe void FillPixels(void* pvDest, int Color, int Pixels)
            {
                Color = this.m_Data.colors[(Color >> 10) | 0x20];
                int num = Pixels >> 1;
                int* numPtr = (int*) pvDest;
                int num2 = (Color << 0x10) | Color;
                while (--num >= 0)
                {
                    numPtr++;
                    numPtr[0] = num2;
                }
                if ((Pixels & 1) != 0)
                {
                    *((short*) numPtr) = (short) Color;
                }
            }

            public Frames GetAnimation(int RealID)
            {
                if (this.m_Anim == null)
                {
                    this.m_Anim = new AnimationCache(this);
                }
                return this.m_Anim[RealID];
            }

            public Texture GetGump(int GumpID)
            {
                if (this.m_Gumps == null)
                {
                    this.m_Gumps = new GumpCache(this);
                }
                return this.m_Gumps[GumpID];
            }

            public Texture GetItem(int ItemID)
            {
                if (this.m_Items == null)
                {
                    this.m_Items = new ItemCache(this);
                }
                return this.m_Items[ItemID];
            }

            public Texture GetLand(int LandID)
            {
                if (this.m_Land == null)
                {
                    this.m_Land = new LandCache(this);
                }
                return this.m_Land[LandID];
            }

            public Texture GetTexture(int TextureID)
            {
                if (this.m_Textures == null)
                {
                    this.m_Textures = new TextureCache(this);
                }
                return this.m_Textures[TextureID];
            }

            public int HueID()
            {
                return this.hue;
            }

            public ushort Pixel(ushort c)
            {
                return this.m_Data.colors[c >> 10];
            }

            public override string ToString()
            {
                return string.Format("<Regular 0x{0:X4}>", this.hue);
            }
        }

        public class HTemperedHue : IHue, IHintable
        {
            private AnimationCache m_Anim;
            private ushort[] m_Colors = new ushort[0x10000];
            private GumpCache m_Gumps;
            private bool[] m_ItemHint = new bool[0x4000];
            private Texture[] m_Items = new Texture[0x4000];
            private Texture[] m_Land = new Texture[0x4000];
            private bool[] m_LandHint = new bool[0x4000];
            private bool[] m_TextureHint = new bool[0x1000];
            private Texture[] m_Textures = new Texture[0x1000];

            public unsafe HTemperedHue(int color, int percent)
            {
                int num = (0x1f * percent) / 100;
                int num2 = 0x1f - num;
                int num3 = color & 0x1f;
                int num4 = (color >> 5) & 0x1f;
                int num5 = (color >> 10) & 0x1f;
                fixed (ushort* numRef = this.m_Colors)
                {
                    ushort* numPtr = numRef + 0x8000;
                    ushort* numPtr2 = numPtr + 0x8000;
                    for (int i = 0; numPtr < numPtr2; i++)
                    {
                        int num7 = i & 0x1f;
                        int num8 = (i >> 5) & 0x1f;
                        int num9 = (i >> 10) & 0x1f;
                        int num10 = ((num3 * num2) + (num7 * num)) / 0x1f;
                        int num11 = ((num4 * num2) + (num8 * num)) / 0x1f;
                        int num12 = ((num5 * num2) + (num9 * num)) / 0x1f;
                        numPtr++;
                        numPtr[0] = (ushort) (((0x8000 | num10) | (num11 << 5)) | (num12 << 10));
                    }
                }
            }

            public unsafe void Apply(LockData ld)
            {
                ushort* pvSrc = (ushort*) ld.pvSrc;
                int height = ld.Height;
                int width = ld.Width;
                int num3 = 0;
                int num4 = (ld.Pitch >> 1) - width;
                fixed (ushort* numRef = this.m_Colors)
                {
                    ushort* numPtr2 = numRef;
                    while (--height >= 0)
                    {
                        num3 = width;
                        while (--num3 >= 0)
                        {
                            pvSrc++;
                            pvSrc[0] = numPtr2[pvSrc[0]];
                        }
                        pvSrc += num4;
                    }
                }
            }

            public void Apply(Texture Target)
            {
                LockData ld = Target.Lock(LockFlags.ReadWrite);
                this.Apply(ld);
                Target.Unlock();
            }

            public unsafe void CopyPixels(void* pvSrc, void* pvDest, int Pixels)
            {
                ushort* numPtr = (ushort*) pvSrc;
                ushort* numPtr2 = (ushort*) pvDest;
                ushort* numPtr3 = numPtr + Pixels;
                fixed (ushort* numRef = this.m_Colors)
                {
                    while (numPtr < numPtr3)
                    {
                        numPtr2++;
                        numPtr++;
                        numPtr2[0] = numRef[numPtr[0] | 0x8000];
                    }
                }
            }

            public void Dispose()
            {
                int index = 0;
                while (index < 0x1000)
                {
                    if (this.m_Items[index] != null)
                    {
                        this.m_Items[index].Dispose();
                        this.m_Items[index] = null;
                    }
                    if (this.m_Land[index] != null)
                    {
                        this.m_Land[index].Dispose();
                        this.m_Land[index] = null;
                    }
                    if (this.m_Textures[index] != null)
                    {
                        this.m_Textures[index].Dispose();
                        this.m_Textures[index] = null;
                    }
                    index++;
                }
                while (index < 0x4000)
                {
                    if (this.m_Items[index] != null)
                    {
                        this.m_Items[index].Dispose();
                        this.m_Items[index] = null;
                    }
                    if (this.m_Land[index] != null)
                    {
                        this.m_Land[index].Dispose();
                        this.m_Land[index] = null;
                    }
                    index++;
                }
                this.m_Items = null;
                this.m_Land = null;
                this.m_Textures = null;
                this.m_ItemHint = null;
                this.m_LandHint = null;
                this.m_TextureHint = null;
                if (this.m_Gumps != null)
                {
                    this.m_Gumps.Dispose();
                    this.m_Gumps = null;
                }
                if (this.m_Anim != null)
                {
                    this.m_Anim.Dispose();
                    this.m_Anim = null;
                }
                this.m_Colors = null;
            }

            public unsafe void FillLine(ushort* pSrc, ushort* pDest, ushort* pEnd)
            {
                ushort* numPtr = pDest;
                while (pDest < pEnd)
                {
                    ushort num = pSrc[0];
                    ushort num2 = pSrc[1];
                    pSrc += 2;
                    numPtr += num2;
                    if (num != 0)
                    {
                        num = this.m_Colors[num | 0x8000];
                        while (pDest < numPtr)
                        {
                            pDest++;
                            pDest[0] = num;
                        }
                    }
                    else
                    {
                        pDest += num2;
                    }
                }
            }

            public unsafe void FillPixels(void* pvDest, int Color, int Pixels)
            {
                Color = this.m_Colors[Color | 0x8000];
                int num = Pixels >> 1;
                int* numPtr = (int*) pvDest;
                int num2 = (Color << 0x10) | Color;
                while (--num >= 0)
                {
                    numPtr++;
                    numPtr[0] = num2;
                }
                if ((Pixels & 1) != 0)
                {
                    *((short*) numPtr) = (short) Color;
                }
            }

            public Frames GetAnimation(int RealID)
            {
                if (this.m_Anim == null)
                {
                    this.m_Anim = new AnimationCache(this);
                }
                return this.m_Anim[RealID];
            }

            public Texture GetGump(int GumpID)
            {
                if (this.m_Gumps == null)
                {
                    this.m_Gumps = new GumpCache(this);
                }
                return this.m_Gumps[GumpID];
            }

            public Texture GetItem(int ItemID)
            {
                ItemID &= 0x3fff;
                if (this.m_Items[ItemID] == null)
                {
                    this.m_Items[ItemID] = Engine.ItemArt.ReadFromDisk(ItemID, this);
                    this.m_ItemHint[ItemID] = true;
                }
                return this.m_Items[ItemID];
            }

            public Texture GetLand(int LandID)
            {
                LandID &= 0x3fff;
                if (this.m_Land[LandID] == null)
                {
                    this.m_Land[LandID] = Engine.LandArt.ReadFromDisk(LandID, this);
                    this.m_LandHint[LandID] = true;
                }
                return this.m_Land[LandID];
            }

            public Texture GetTexture(int TextureID)
            {
                TextureID &= 0xfff;
                if (this.m_Textures[TextureID] == null)
                {
                    this.m_Textures[TextureID] = Engine.TextureArt.ReadFromDisk(TextureID, this);
                    this.m_TextureHint[TextureID] = true;
                }
                return this.m_Textures[TextureID];
            }

            public bool HintItem(int ItemID)
            {
                ItemID &= 0x3fff;
                if (!this.m_ItemHint[ItemID])
                {
                    this.m_ItemHint[ItemID] = true;
                    return false;
                }
                return true;
            }

            public bool HintLand(int LandID)
            {
                LandID &= 0x3fff;
                int texture = Map.GetTexture(LandID);
                MapLighting.CheckStretchTable();
                if (!MapLighting.m_AlwaysStretch[LandID])
                {
                    if (!this.m_LandHint[LandID])
                    {
                        this.m_LandHint[LandID] = true;
                        if ((texture > 0) && (texture < 0x1000))
                        {
                            this.m_TextureHint[texture] = true;
                        }
                        return false;
                    }
                }
                else if (((texture > 0) && (texture < 0x1000)) && !this.m_TextureHint[texture])
                {
                    this.m_TextureHint[texture] = true;
                    return false;
                }
                return true;
            }

            public int HueID()
            {
                return 0xffff;
            }

            public ushort Pixel(ushort input)
            {
                return this.m_Colors[input];
            }

            public override string ToString()
            {
                return "<Tempered>";
            }
        }
    }
}

