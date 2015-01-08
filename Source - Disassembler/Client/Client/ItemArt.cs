namespace Client
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    public class ItemArt
    {
        private ItemFactory m_Factory;
        private Entry3D[] m_Index = new Entry3D[0x4000];
        private FileStream m_Stream;
        private int[] m_Translate = new int[0x4000];
        private FileStream m_Verdata;

        public unsafe ItemArt()
        {
            FileStream fs = new FileStream(Engine.FileManager.ResolveMUL(Files.ArtIdx), FileMode.Open, FileAccess.Read, FileShare.Read);
            fs.Seek(0x30000L, SeekOrigin.Begin);
            fixed (Entry3D* entrydRef = this.m_Index)
            {
                Engine.NativeRead(fs, (void*) entrydRef, 0x30000);
            }
            fs.Close();
            this.m_Stream = new FileStream(Engine.FileManager.ResolveMUL(Files.ArtMul), FileMode.Open, FileAccess.Read, FileShare.Read);
            this.m_Verdata = new FileStream(Engine.FileManager.ResolveMUL(Files.Verdata), FileMode.Open, FileAccess.Read, FileShare.Read);
            int num = 0;
            Engine.NativeRead(this.m_Verdata, (void*) &num, 4);
            if (num > 0)
            {
                Entry5D[] entrydArray = new Entry5D[num];
                fixed (Entry5D* entrydRef2 = entrydArray)
                {
                    Engine.NativeRead(this.m_Verdata, (void*) entrydRef2, num * 20);
                    for (int k = 0; k < num; k++)
                    {
                        Entry5D entryd = entrydRef2[k];
                        if (((entryd.m_FileID == 4) && (entryd.m_BlockID >= 0x4000)) && (entryd.m_BlockID < 0x8000))
                        {
                            entryd.m_BlockID &= 0x3fff;
                            this.m_Index[entryd.m_BlockID].m_Lookup = entryd.m_Lookup;
                            this.m_Index[entryd.m_BlockID].m_Length = entryd.m_Length;
                            this.m_Index[entryd.m_BlockID].m_Extra = entryd.m_Extra;
                        }
                    }
                }
            }
            ArtTableEntry[] entries = ArtTable.m_Entries;
            for (int i = 0; i < this.m_Translate.Length; i++)
            {
                this.m_Translate[i] = i;
            }
            int length = entries.Length;
            for (int j = 0; j < length; j++)
            {
                ArtTableEntry entry = entries[j];
                if ((entry.m_OldID >= 0x4000) && (entry.m_NewID >= 0x4000))
                {
                    this.m_Translate[entry.m_NewID & 0x3fff] = entry.m_OldID & 0x3fff;
                    if (this.m_Index[entry.m_NewID & 0x3fff].m_Lookup == -1)
                    {
                        this.m_Index[entry.m_NewID & 0x3fff].m_Lookup = j | -2147483648;
                        this.m_Index[entry.m_NewID & 0x3fff].m_Length = this.m_Index[entry.m_OldID & 0x3fff].m_Length;
                    }
                }
            }
        }

        public void Dispose()
        {
            this.m_Stream.Close();
            this.m_Stream = null;
            this.m_Verdata.Close();
            this.m_Verdata = null;
            this.m_Index = null;
        }

        public void ForceTranslate(ref int itemID)
        {
            itemID = this.m_Translate[itemID & 0x3fff];
        }

        public Texture ReadFromDisk(int ItemID, IHue Hue)
        {
            ItemID &= 0x3fff;
            if ((ItemID >= 0x3584) && (ItemID <= 0x35a1))
            {
                return Hue.GetGump(0x91b + (ItemID - 0x3584));
            }
            if (this.m_Factory == null)
            {
                this.m_Factory = new ItemFactory(this);
            }
            return this.m_Factory.Load(ItemID, Hue);
        }

        public void Translate(ref int itemID)
        {
            itemID &= 0x3fff;
            int length = this.m_Index[itemID].m_Length;
            if (((length != -1) && ((length & -2147483648) != 0)) && (this.m_Index[itemID].m_Lookup == -1))
            {
                ArtTableEntry entry = ArtTable.m_Entries[length & 0x7fffffff];
                itemID = entry.m_OldID & 0x3fff;
            }
        }

        public void Translate(ref int itemID, ref int hue)
        {
            itemID &= 0x3fff;
            int lookup = this.m_Index[itemID].m_Lookup;
            if (((lookup != -1) && ((lookup & -2147483648) != 0)) && (this.m_Index[itemID].m_Lookup == -1))
            {
                ArtTableEntry entry = ArtTable.m_Entries[lookup & 0x7fffffff];
                itemID = entry.m_OldID & 0x3fff;
                hue = entry.m_NewHue;
            }
        }

        private class ItemFactory : TextureFactory
        {
            private byte[] m_Buffer;
            private IHue m_Hue;
            private int m_ItemID;
            private ItemArt m_Items;
            private int m_xMax;
            private int m_xMin;
            private int m_yMax;
            private int m_yMin;

            public ItemFactory(ItemArt items)
            {
                this.m_Items = items;
            }

            protected override void CoreAssignArgs(Texture tex)
            {
                tex.m_Factory = this;
                tex.m_FactoryArgs = new object[] { this.m_ItemID, this.m_Hue };
                tex.xMin = this.m_xMin;
                tex.yMin = this.m_yMin;
                tex.xMax = this.m_xMax;
                tex.yMax = this.m_yMax;
            }

            protected override void CoreGetDimensions(out int width, out int height)
            {
                width = this.m_Buffer[4] | (this.m_Buffer[5] << 8);
                height = this.m_Buffer[6] | (this.m_Buffer[7] << 8);
            }

            protected override bool CoreLookup()
            {
                FileStream verdata;
                Entry3D entryd = this.m_Items.m_Index[this.m_ItemID];
                int lookup = entryd.m_Lookup;
                int length = entryd.m_Length;
                if (lookup == -1)
                {
                    return false;
                }
                bool flag = (length & -2147483648) != 0;
                if ((lookup & -2147483648) != 0)
                {
                    lookup &= 0x7fffffff;
                    ArtTableEntry entry = ArtTable.m_Entries[lookup];
                    lookup = this.m_Items.m_Index[entry.m_OldID & 0x3fff].m_Lookup;
                    if (lookup == -1)
                    {
                        return false;
                    }
                }
                if (flag)
                {
                    length &= 0x7fffffff;
                    this.m_Items.m_Verdata.Seek((long) lookup, SeekOrigin.Begin);
                    verdata = this.m_Items.m_Verdata;
                }
                else
                {
                    this.m_Items.m_Stream.Seek((long) lookup, SeekOrigin.Begin);
                    verdata = this.m_Items.m_Stream;
                }
                if ((this.m_Buffer == null) || (length > this.m_Buffer.Length))
                {
                    this.m_Buffer = new byte[length];
                }
                if (Engine.NativeRead(verdata, this.m_Buffer, 0, length) != length)
                {
                    return false;
                }
                int num4 = this.m_Buffer[4] | (this.m_Buffer[5] << 8);
                int num5 = this.m_Buffer[6] | (this.m_Buffer[7] << 8);
                return ((num4 > 0) && (num5 > 0));
            }

            protected override unsafe void CoreProcessImage(int width, int height, int stride, ushort* pLine, ushort* pLineEnd, ushort* pImageEnd, int lineDelta, int lineEndDelta)
            {
                fixed (byte* numRef = this.m_Buffer)
                {
                    short* numPtr3 = (short*) ((numRef + 8) + (((IntPtr) height) * 2));
                    short* numPtr4 = (short*) (numRef + 6);
                    int num = width;
                    int num2 = height;
                    int num3 = 0;
                    int num4 = 0;
                    int num5 = 0;
                    int num6 = 0;
                    int num7 = 0;
                    int pixels = 0;
                    while (num6 < height)
                    {
                        short* numPtr = numPtr3 + *(++numPtr4);
                        ushort* numPtr2 = pLine;
                        num5 = 0;
                        if (((numPtr[0] + numPtr[1]) != 0) && (numPtr[0] < num))
                        {
                            num = numPtr[0];
                        }
                        goto Label_008D;
                    Label_0066:
                        numPtr2 += num7;
                        this.m_Hue.CopyPixels((void*) numPtr, (void*) numPtr2, pixels);
                        numPtr2 += pixels;
                        numPtr += pixels;
                    Label_008D:
                        numPtr++;
                        numPtr++;
                        if (((num7 = numPtr[0]) + (pixels = numPtr[0])) != 0)
                        {
                            goto Label_0066;
                        }
                        num5 = (int) ((long) ((numPtr2 - pLine) / 2));
                        if (num5 > 0)
                        {
                            if (num2 == height)
                            {
                                num2 = num6;
                            }
                            num4 = num6;
                            num5--;
                            if (num5 > num3)
                            {
                                num3 = num5;
                            }
                        }
                        num6++;
                        pLine += lineEndDelta;
                    }
                    this.m_xMin = num;
                    this.m_yMin = num2;
                    this.m_xMax = num3;
                    this.m_yMax = num4;
                }
            }

            public Texture Load(int itemID, IHue hue)
            {
                this.m_ItemID = itemID;
                this.m_Hue = hue;
                return base.Construct(false);
            }

            public override Texture Reconstruct(object[] args)
            {
                this.m_ItemID = (int) args[0];
                this.m_Hue = (IHue) args[1];
                return base.Construct(true);
            }
        }
    }
}

