namespace Client
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Runtime.InteropServices;

    public class UnicodeFont : IFont, IFontFactory
    {
        private const short Border = -32768;
        private byte[] m_4Bytes;
        private bool m_Bold;
        private FontCache[] m_Cache;
        private static short[] m_Colors = new short[0x100];
        private UnicodeFontFactory m_Factory;
        private string m_FileName = "";
        private int m_FontID;
        private static short[] m_HuedColors = new short[0x100];
        private bool m_Italic;
        private int[] m_Lookup;
        private static byte[] m_NullChar = new byte[] { 0xff, 0x81, 0x81, 0x81, 0x81, 0x81, 0x81, 0x81, 0x81, 0xff };
        private int m_SpaceWidth = 8;
        private Stream m_Stream;
        private bool m_Underline;
        private Hashtable m_WrapCache = new Hashtable();

        static UnicodeFont()
        {
            m_HuedColors[0] = 0;
            m_HuedColors[0x80] = -32768;
            short num = -1;
            int num2 = 0;
            int index = 1;
            int num4 = 0x81;
            while (num2 < 0x20)
            {
                m_Colors[index] = m_Colors[num4] = num;
                num2++;
                index++;
                num4++;
                num = (short) (num - 0x421);
            }
        }

        public unsafe UnicodeFont(int FontID)
        {
            this.m_FileName = string.Format("UniFont{0}.mul", (FontID != 0) ? FontID.ToString() : "");
            this.m_FontID = FontID;
            this.m_Stream = Engine.FileManager.OpenMUL(this.m_FileName);
            this.m_Lookup = new int[0x10000];
            fixed (int* numRef = this.m_Lookup)
            {
                Engine.NativeRead((FileStream) this.m_Stream, (void*) numRef, 0x40000);
            }
            this.m_4Bytes = new byte[4];
            this.m_Cache = new FontCache[8];
            for (int i = 0; i < this.m_Cache.Length; i++)
            {
                this.m_Cache[i] = new FontCache(this);
            }
        }

        Texture IFontFactory.CreateInstance(string String, IHue Hue)
        {
            if ((String == null) || (String.Length <= 0))
            {
                return Texture.Empty;
            }
            if (Hue == Hues.Default)
            {
                Hue = Hues.Bright;
            }
            if (this.m_Factory == null)
            {
                this.m_Factory = new UnicodeFontFactory(this);
            }
            return this.m_Factory.Load(String, Hue);
        }

        public void Dispose()
        {
            for (int i = 0; i < this.m_Cache.Length; i++)
            {
                this.m_Cache[i].Dispose();
                this.m_Cache[i] = null;
            }
            this.m_Cache = null;
            this.m_Stream.Close();
            this.m_Stream = null;
            m_Colors = null;
            m_HuedColors = null;
            this.m_FileName = null;
            this.m_Lookup = null;
            this.m_WrapCache.Clear();
            this.m_WrapCache = null;
            this.m_4Bytes = null;
            m_NullChar = null;
        }

        public int GetBits()
        {
            int num = 0;
            if (this.m_Underline)
            {
                num |= 1;
            }
            if (this.m_Bold)
            {
                num |= 2;
            }
            if (this.m_Italic)
            {
                num |= 4;
            }
            return num;
        }

        public Texture GetString(string String, IHue Hue)
        {
            return this.m_Cache[this.GetBits()][String, Hue];
        }

        public int GetStringWidth(string text)
        {
            if ((text == null) || (text.Length <= 0))
            {
                return 2;
            }
            if (this.m_Factory == null)
            {
                this.m_Factory = new UnicodeFontFactory(this);
            }
            return this.m_Factory.MeasureWidth(text);
        }

        [DllImport("Kernel32")]
        private static extern unsafe void RtlZeroMemory(void* pvTarget, int byteCount);
        public override string ToString()
        {
            return string.Format("<Unicode Font #{0}>", this.m_FontID);
        }

        public bool Bold
        {
            get
            {
                return this.m_Bold;
            }
            set
            {
                this.m_Bold = value;
            }
        }

        public bool Italic
        {
            get
            {
                return this.m_Italic;
            }
            set
            {
                this.m_Italic = value;
            }
        }

        public string Name
        {
            get
            {
                return string.Format("UniFont[{0}]", this.m_FontID);
            }
        }

        public int SpaceWidth
        {
            get
            {
                return this.m_SpaceWidth;
            }
            set
            {
                this.m_SpaceWidth = value;
            }
        }

        public bool Underline
        {
            get
            {
                return this.m_Underline;
            }
            set
            {
                this.m_Underline = value;
            }
        }

        public Hashtable WrapCache
        {
            get
            {
                return this.m_WrapCache;
            }
        }

        private class UnicodeFontFactory : TextureFactory
        {
            private static byte[] m_Buffer;
            private UnicodeFont m_Font;
            private IHue m_Hue;
            private CharacterBits[] m_LowCharacters;
            private string m_String;
            private int m_xMax;
            private int m_xMin;
            private int m_yMax;
            private int m_yMin;

            public UnicodeFontFactory(UnicodeFont font)
            {
                this.m_Font = font;
            }

            protected override void CoreAssignArgs(Texture tex)
            {
                tex.m_Factory = this;
                tex.m_FactoryArgs = new object[] { this.m_String, this.m_Hue };
                tex.xMin = this.m_xMin;
                tex.yMin = this.m_yMin;
                tex.xMax = this.m_xMax;
                tex.yMax = this.m_yMax;
            }

            protected override void CoreGetDimensions(out int width, out int height)
            {
                string str = this.m_String;
                int num = 0;
                int num2 = 1;
                int num3 = 0;
                int num4 = 0x12;
                int spaceWidth = this.m_Font.SpaceWidth;
                bool flag = false;
                for (int i = 0; i < str.Length; i++)
                {
                    char c = str[i];
                    switch (c)
                    {
                        case '\t':
                        {
                            flag = false;
                            num += 0x18;
                            if (num > num3)
                            {
                                num3 = num;
                            }
                            continue;
                        }
                        case '\n':
                        case '\r':
                        {
                            if (!flag)
                            {
                                num = 0;
                                num2 += 0x12;
                                num4 += 0x12;
                                flag = true;
                            }
                            continue;
                        }
                        case ' ':
                        {
                            flag = false;
                            num += spaceWidth;
                            if (num > num3)
                            {
                                num3 = num;
                            }
                            continue;
                        }
                    }
                    flag = false;
                    CharacterBits character = this.GetCharacter(c, false);
                    num += (1 + character.m_xOffset) + character.m_xWidth;
                    if (num > num3)
                    {
                        num3 = num;
                    }
                    if (((num2 + character.m_yOffset) + character.m_yHeight) > num4)
                    {
                        num4 = (num2 + character.m_yOffset) + character.m_yHeight;
                    }
                }
                width = num3 + 1;
                height = num4 + 2;
            }

            protected override bool CoreLookup()
            {
                return ((this.m_String != null) && (this.m_String.Length > 0));
            }

            protected override unsafe void CoreProcessImage(int width, int height, int stride, ushort* pLine, ushort* pLineEnd, ushort* pImageEnd, int lineDelta, int lineEndDelta)
            {
                string str = this.m_String;
                int num = 0;
                int num2 = 1;
                int spaceWidth = this.m_Font.SpaceWidth;
                bool flag = false;
                int byteCount = width * height;
                byte[] buffer = m_Buffer;
                if ((buffer == null) || (byteCount > buffer.Length))
                {
                    m_Buffer = buffer = new byte[byteCount];
                }
                bool flag2 = ((this.m_Hue.HueID() & 0x3fff) != 1) && !(this.m_Hue is Hues.HFill);
                fixed (byte* numRef = buffer)
                {
                    UnicodeFont.RtlZeroMemory((void*) numRef, byteCount);
                    for (int i = 0; i < str.Length; i++)
                    {
                        char c = str[i];
                        switch (c)
                        {
                            case '\t':
                            {
                                flag = false;
                                num += 0x18;
                                continue;
                            }
                            case '\n':
                            case '\r':
                            {
                                if (!flag)
                                {
                                    num = 0;
                                    num2 += 0x12;
                                    flag = true;
                                }
                                continue;
                            }
                            case ' ':
                            {
                                flag = false;
                                num += spaceWidth;
                                continue;
                            }
                        }
                        flag = false;
                        CharacterBits character = this.GetCharacter(c, true);
                        num += 1 + character.m_xOffset;
                        fixed (byte* numRef2 = character.m_Bits)
                        {
                            byte* numPtr = numRef2;
                            byte* numPtr2 = (numRef + ((num2 + character.m_yOffset) * width)) + ((num + character.m_xWidth) - 1);
                            int num6 = 0x20 - character.m_xWidth;
                            int num7 = (character.m_xWidth + 7) >> 3;
                            int num10 = 0;
                            while (num10 < character.m_yHeight)
                            {
                                uint num11 = *((uint*) numPtr);
                                num11 = ((uint) ((((num11 & 0xff) << 0x18) | ((num11 & 0xff00) << 8)) | ((num11 & 0xff0000) >> 8))) | ((num11 >> 0x18) & 0xff);
                                num11 = num11 >> num6;
                                byte* numPtr3 = numPtr2;
                                if (flag2)
                                {
                                    int num8 = (num + character.m_xWidth) - 1;
                                    int num9 = (num2 + character.m_yOffset) + num10;
                                    bool flag3 = num9 <= 0;
                                    bool flag4 = num9 >= (height - 1);
                                    if (!flag3 && !flag4)
                                    {
                                        while (num11 != 0)
                                        {
                                            if ((num11 & 1) != 0)
                                            {
                                                bool flag5 = num8 <= 0;
                                                bool flag6 = num8 >= (width - 1);
                                                if (!flag5)
                                                {
                                                    byte* numPtr1 = numPtr3 - 1;
                                                    numPtr1[0] = (byte) (numPtr1[0] | 0x80);
                                                }
                                                if (!flag6)
                                                {
                                                    byte* numPtr6 = numPtr3 + 1;
                                                    numPtr6[0] = (byte) (numPtr6[0] | 0x80);
                                                }
                                                byte* numPtr7 = numPtr3 - width;
                                                numPtr7[0] = (byte) (numPtr7[0] | 0x80);
                                                numPtr3[0] = (byte) (numPtr3[0] | 2);
                                                byte* numPtr8 = numPtr3 + width;
                                                numPtr8[0] = (byte) (numPtr8[0] | 0x80);
                                            }
                                            numPtr3--;
                                            num8--;
                                            num11 = num11 >> 1;
                                        }
                                    }
                                    else if (flag3 && !flag4)
                                    {
                                        while (num11 != 0)
                                        {
                                            if ((num11 & 1) != 0)
                                            {
                                                bool flag7 = num8 <= 0;
                                                bool flag8 = num8 >= (width - 1);
                                                if (!flag7)
                                                {
                                                    byte* numPtr9 = numPtr3 - 1;
                                                    numPtr9[0] = (byte) (numPtr9[0] | 0x80);
                                                }
                                                if (!flag8)
                                                {
                                                    byte* numPtr10 = numPtr3 + 1;
                                                    numPtr10[0] = (byte) (numPtr10[0] | 0x80);
                                                }
                                                numPtr3[0] = (byte) (numPtr3[0] | 2);
                                                byte* numPtr11 = numPtr3 + width;
                                                numPtr11[0] = (byte) (numPtr11[0] | 0x80);
                                            }
                                            numPtr3--;
                                            num8--;
                                            num11 = num11 >> 1;
                                        }
                                    }
                                    else if (!flag3 && flag4)
                                    {
                                        while (num11 != 0)
                                        {
                                            if ((num11 & 1) != 0)
                                            {
                                                bool flag9 = num8 <= 0;
                                                bool flag10 = num8 >= (width - 1);
                                                if (!flag9)
                                                {
                                                    byte* numPtr12 = numPtr3 - 1;
                                                    numPtr12[0] = (byte) (numPtr12[0] | 0x80);
                                                }
                                                if (!flag10)
                                                {
                                                    byte* numPtr13 = numPtr3 + 1;
                                                    numPtr13[0] = (byte) (numPtr13[0] | 0x80);
                                                }
                                                byte* numPtr14 = numPtr3 - width;
                                                numPtr14[0] = (byte) (numPtr14[0] | 0x80);
                                                numPtr3[0] = (byte) (numPtr3[0] | 2);
                                            }
                                            numPtr3--;
                                            num8--;
                                            num11 = num11 >> 1;
                                        }
                                    }
                                    else
                                    {
                                        while (num11 != 0)
                                        {
                                            if ((num11 & 1) != 0)
                                            {
                                                bool flag11 = num8 <= 0;
                                                bool flag12 = num8 >= (width - 1);
                                                if (!flag11)
                                                {
                                                    byte* numPtr15 = numPtr3 - 1;
                                                    numPtr15[0] = (byte) (numPtr15[0] | 0x80);
                                                }
                                                if (!flag12)
                                                {
                                                    byte* numPtr16 = numPtr3 + 1;
                                                    numPtr16[0] = (byte) (numPtr16[0] | 0x80);
                                                }
                                                numPtr3[0] = (byte) (numPtr3[0] | 2);
                                            }
                                            numPtr3--;
                                            num8--;
                                            num11 = num11 >> 1;
                                        }
                                    }
                                }
                                else
                                {
                                    while (num11 != 0)
                                    {
                                        if ((num11 & 1) != 0)
                                        {
                                            numPtr3[0] = (byte) (numPtr3[0] | 2);
                                        }
                                        numPtr3--;
                                        num11 = num11 >> 1;
                                    }
                                }
                                num10++;
                                numPtr2 += width;
                                numPtr += num7;
                            }
                        }
                        num += character.m_xWidth;
                    }
                    int num12 = width;
                    int num13 = height;
                    int num14 = 0;
                    int num15 = 0;
                    bool underline = this.m_Font.Underline;
                    byte* numPtr4 = numRef;
                    fixed (short* numRef3 = UnicodeFont.m_Colors)
                    {
                        fixed (short* numRef4 = UnicodeFont.m_HuedColors)
                        {
                            ushort* numPtr5 = (ushort*) numRef4;
                            this.m_Hue.CopyPixels((void*) (numRef3 + 1), (void*) (numPtr5 + 1), 0x20);
                            this.m_Hue.CopyPixels((void*) (numRef3 + 0x81), (void*) (numPtr5 + 0x81), 0x20);
                            for (int j = 0; j < height; j++)
                            {
                                for (int k = 0; k < width; k++)
                                {
                                    if (numPtr4[0] != 0)
                                    {
                                        if (k < num12)
                                        {
                                            num12 = k;
                                        }
                                        if (k > num14)
                                        {
                                            num14 = k;
                                        }
                                        if (j < num13)
                                        {
                                            num13 = j;
                                        }
                                        if (j > num15)
                                        {
                                            num15 = j;
                                        }
                                    }
                                    if (underline && ((j % 0x12) == 15))
                                    {
                                        numPtr4[0] = 0x10;
                                    }
                                    pLine++;
                                    pLine[0] = numPtr5[*(numPtr4++)];
                                }
                                pLine += lineDelta;
                            }
                        }
                    }
                    this.m_xMin = num12;
                    this.m_yMin = num13;
                    this.m_xMax = num14;
                    this.m_yMax = num15;
                }
            }

            private CharacterBits GetCharacter(char c, bool needBits)
            {
                int index = c;
                if ((index >= 0) && (index < 0x100))
                {
                    if (this.m_LowCharacters == null)
                    {
                        this.m_LowCharacters = new CharacterBits[0x100];
                    }
                    CharacterBits bits = this.m_LowCharacters[index];
                    if (bits == null)
                    {
                        this.m_LowCharacters[index] = bits = this.LoadCharacter(index, true);
                    }
                    return bits;
                }
                return this.LoadCharacter(index, needBits);
            }

            public Texture Load(string text, IHue hue)
            {
                this.m_String = text;
                this.m_Hue = hue;
                return base.Construct(false);
            }

            private CharacterBits LoadCharacter(int index, bool needBits)
            {
                this.m_Font.m_Stream.Seek((long) this.m_Font.m_Lookup[index], SeekOrigin.Begin);
                return new CharacterBits((FileStream) this.m_Font.m_Stream, needBits);
            }

            public int MeasureWidth(string text)
            {
                int num;
                int num2;
                this.m_String = text;
                if (!this.CoreLookup())
                {
                    return 2;
                }
                this.CoreGetDimensions(out num, out num2);
                return num;
            }

            public override Texture Reconstruct(object[] args)
            {
                this.m_String = (string) args[0];
                this.m_Hue = (IHue) args[1];
                return base.Construct(true);
            }

            private class CharacterBits
            {
                public byte[] m_Bits;
                private static byte[] m_Header = new byte[4];
                public int m_xOffset;
                public int m_xWidth;
                public int m_yHeight;
                public int m_yOffset;

                public CharacterBits(FileStream stream, bool needBits)
                {
                    if (stream.Read(m_Header, 0, 4) == 4)
                    {
                        this.m_xOffset = (sbyte) m_Header[0];
                        this.m_yOffset = (sbyte) m_Header[1];
                        this.m_xWidth = (sbyte) m_Header[2];
                        this.m_yHeight = (sbyte) m_Header[3];
                        int count = ((this.m_xWidth + 7) >> 3) * this.m_yHeight;
                        if (count > 0)
                        {
                            if (!needBits)
                            {
                                return;
                            }
                            this.m_Bits = new byte[count];
                            if (stream.Read(this.m_Bits, 0, count) == count)
                            {
                                return;
                            }
                        }
                    }
                    this.m_xOffset = 0;
                    this.m_yOffset = 4;
                    this.m_xWidth = 8;
                    this.m_yHeight = 10;
                    this.m_Bits = UnicodeFont.m_NullChar;
                }
            }
        }
    }
}

