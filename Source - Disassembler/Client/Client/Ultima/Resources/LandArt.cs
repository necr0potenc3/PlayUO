namespace Client
{
    using System.IO;

    public class LandArt
    {
        private LandFactory m_Factory;
        private int[] m_Length;
        private int[] m_Lookup = new int[0x4000];
        private int[] m_Offset;
        private Stream m_Stream;

        public unsafe LandArt()
        {
            Stream stream = Engine.FileManager.OpenMUL(Files.ArtIdx);
            byte[] buffer = new byte[0x30000];
            Engine.NativeRead((FileStream)stream, buffer, 0, buffer.Length);
            stream.Close();
            fixed (byte* numRef = buffer)
            {
                int* numPtr = (int*)numRef;
                int num = 0;
                while (num < 0x4000)
                {
                    this.m_Lookup[num++] = numPtr[0];
                    numPtr += 3;
                }
            }
            this.m_Length = new int[] {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0x10,
                0x11, 0x12, 0x13, 20, 0x15, 0x16, 0x16, 0x15, 20, 0x13, 0x12, 0x11, 0x10, 15, 14, 13,
                12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1
            };
            this.m_Offset = new int[] {
                0x15, 20, 0x13, 0x12, 0x11, 0x10, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6,
                5, 4, 3, 2, 1, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
                10, 11, 12, 13, 14, 15, 0x10, 0x11, 0x12, 0x13, 20, 0x15
            };
            this.m_Stream = Engine.FileManager.OpenMUL(Files.ArtMul);
            ArtTableEntry[] entries = ArtTable.m_Entries;
            int length = entries.Length;
            for (int i = 0; i < length; i++)
            {
                ArtTableEntry entry = entries[i];
                if (((entry.m_OldID < 0x4000) && (entry.m_NewID < 0x4000)) && (this.m_Lookup[entry.m_OldID & 0x3fff] >= 0))
                {
                    this.m_Lookup[entry.m_NewID & 0x3fff] = this.m_Lookup[entry.m_OldID & 0x3fff];
                }
            }
        }

        public void Dispose()
        {
            this.m_Lookup = null;
            this.m_Offset = null;
            this.m_Length = null;
            this.m_Stream.Close();
            this.m_Stream = null;
        }

        public Texture ReadFromDisk(int LandID, IHue Hue)
        {
            if (this.m_Factory == null)
            {
                this.m_Factory = new LandFactory(this);
            }
            return this.m_Factory.Load(LandID, Hue);
        }

        private class LandFactory : TextureFactory
        {
            private byte[] m_Buffer = new byte[0x800];
            private IHue m_Hue;
            private LandArt m_Land;
            private int m_LandID;
            private int[] m_Length;
            private int[] m_Offset;

            public LandFactory(LandArt land)
            {
                this.m_Land = land;
                this.m_Length = new int[] {
                    1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 0x10,
                    0x11, 0x12, 0x13, 20, 0x15, 0x16, 0x16, 0x15, 20, 0x13, 0x12, 0x11, 0x10, 15, 14, 13,
                    12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1
                };
                this.m_Offset = new int[] {
                    0x15, 20, 0x13, 0x12, 0x11, 0x10, 15, 14, 13, 12, 11, 10, 9, 8, 7, 6,
                    5, 4, 3, 2, 1, 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
                    10, 11, 12, 13, 14, 15, 0x10, 0x11, 0x12, 0x13, 20, 0x15
                };
            }

            protected override void CoreAssignArgs(Texture tex)
            {
                tex.m_Factory = this;
                tex.m_FactoryArgs = new object[] { this.m_LandID, this.m_Hue };
            }

            protected override void CoreGetDimensions(out int width, out int height)
            {
                width = height = 0x2c;
            }

            protected override bool CoreLookup()
            {
                int num = this.m_Land.m_Lookup[this.m_LandID];
                return (num != -1);
            }

            protected override unsafe void CoreProcessImage(int width, int height, int stride, ushort* pLine, ushort* pLineEnd, ushort* pImageEnd, int lineDelta, int lineEndDelta)
            {
                int num = this.m_Land.m_Lookup[this.m_LandID];
                if (num != -1)
                {
                    this.m_Land.m_Stream.Seek((long)num, SeekOrigin.Begin);
                    fixed (byte* numRef = this.m_Buffer)
                    {
                        Engine.NativeRead((FileStream)this.m_Land.m_Stream, (void*)numRef, 0x800);
                        int* numPtr = (int*)numRef;
                        short* numPtr2 = (short*)pLine;
                        int num2 = 11;
                        fixed (int* numRef2 = this.m_Offset)
                        {
                            fixed (int* numRef3 = this.m_Length)
                            {
                                int* numPtr3 = numRef2;
                                for (int* numPtr4 = numRef3; --num2 >= 0; numPtr4 += 4)
                                {
                                    int* numPtr5 = (int*)(pLine + numPtr3[0]);
                                    int num3 = numPtr4[0];
                                    this.m_Hue.CopyPixels((void*)numPtr, (void*)numPtr5, num3 << 1);
                                    numPtr += num3;
                                    pLine += lineEndDelta;
                                    numPtr5 = (int*)(pLine + numPtr3[1]);
                                    num3 = numPtr4[1];
                                    this.m_Hue.CopyPixels((void*)numPtr, (void*)numPtr5, num3 << 1);
                                    numPtr += num3;
                                    pLine += lineEndDelta;
                                    numPtr5 = (int*)(pLine + numPtr3[2]);
                                    num3 = numPtr4[2];
                                    this.m_Hue.CopyPixels((void*)numPtr, (void*)numPtr5, num3 << 1);
                                    numPtr += num3;
                                    pLine += lineEndDelta;
                                    numPtr5 = (int*)(pLine + numPtr3[3]);
                                    num3 = numPtr4[3];
                                    this.m_Hue.CopyPixels((void*)numPtr, (void*)numPtr5, num3 << 1);
                                    numPtr += num3;
                                    pLine += lineEndDelta;
                                    numPtr3 += 4;
                                }
                            }
                        }
                    }
                }
            }

            public Texture Load(int landID, IHue hue)
            {
                this.m_LandID = landID & 0x3fff;
                this.m_Hue = hue;
                return base.Construct(false);
            }

            public override Texture Reconstruct(object[] args)
            {
                this.m_LandID = (int)args[0];
                this.m_Hue = (IHue)args[1];
                return base.Construct(true);
            }
        }
    }
}