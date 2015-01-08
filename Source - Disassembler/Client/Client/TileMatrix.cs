namespace Client
{
    using System;
    using System.IO;

    public class TileMatrix
    {
        private int m_BlockHeight;
        private WeakReference[][] m_Blocks;
        private int m_BlockWidth;
        private HuedTile[][][] m_EmptyStaticBlock;
        private int m_Height;
        private FileStream m_Index;
        private BinaryReader m_IndexReader;
        private Tile[] m_InvalidLandBlock;
        private static TileList[][] m_Lists;
        private FileStream m_Map;
        private TileMatrixPatch m_Patch;
        private FileStream m_Statics;
        private int m_Width;

        public TileMatrix(int fileIndex, int mapID, int width, int height)
        {
            this.m_Width = width;
            this.m_Height = height;
            this.m_BlockWidth = width >> 3;
            this.m_BlockHeight = height >> 3;
            if (fileIndex != 0x7f)
            {
                string path = Path.Combine(Engine.FileManager.FilePath, string.Format("map{0}.mul", fileIndex));
                if (File.Exists(path))
                {
                    this.m_Map = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                }
                else
                {
                    Console.WriteLine("Warning: Tile matrix #{0} does not exist", fileIndex);
                }
                string str2 = Path.Combine(Engine.FileManager.FilePath, string.Format("staidx{0}.mul", fileIndex));
                if (File.Exists(str2))
                {
                    this.m_Index = new FileStream(str2, FileMode.Open, FileAccess.Read, FileShare.Read);
                    this.m_IndexReader = new BinaryReader(this.m_Index);
                }
                string str3 = Path.Combine(Engine.FileManager.FilePath, string.Format("statics{0}.mul", fileIndex));
                if (File.Exists(str3))
                {
                    this.m_Statics = new FileStream(str3, FileMode.Open, FileAccess.Read, FileShare.Read);
                }
            }
            this.m_EmptyStaticBlock = new HuedTile[8][][];
            for (int i = 0; i < 8; i++)
            {
                this.m_EmptyStaticBlock[i] = new HuedTile[8][];
                for (int j = 0; j < 8; j++)
                {
                    this.m_EmptyStaticBlock[i][j] = new HuedTile[0];
                }
            }
            this.m_InvalidLandBlock = new Tile[0xc4];
            this.m_Blocks = new WeakReference[this.m_BlockWidth][];
            this.m_Patch = new TileMatrixPatch(this, mapID);
        }

        public bool CheckLoaded(int x, int y)
        {
            return (((((x >= 0) && (x < this.m_BlockWidth)) && ((y >= 0) && (y < this.m_BlockHeight))) && (this.m_Blocks[x] != null)) && (this.m_Blocks[x][y] != null));
        }

        public void Dispose()
        {
            if (this.m_Map != null)
            {
                this.m_Map.Close();
            }
            if (this.m_Statics != null)
            {
                this.m_Statics.Close();
            }
            if (this.m_IndexReader != null)
            {
                this.m_IndexReader.Close();
            }
        }

        public MapBlock GetBlock(int x, int y)
        {
            MapBlock target;
            if (((x < 0) || (y < 0)) || ((x >= this.m_BlockWidth) || (y >= this.m_BlockHeight)))
            {
                return null;
            }
            if (this.m_Blocks[x] == null)
            {
                this.m_Blocks[x] = new WeakReference[this.m_BlockHeight];
            }
            WeakReference reference = this.m_Blocks[x][y];
            if (reference == null)
            {
                this.m_Blocks[x][y] = new WeakReference(target = this.LoadBlock(x, y));
            }
            else if (!reference.IsAlive)
            {
                reference.Target = target = this.LoadBlock(x, y);
            }
            else
            {
                target = (MapBlock) reference.Target;
            }
            return target;
        }

        public Tile[] GetLandBlock(int x, int y)
        {
            if (((x < 0) || (y < 0)) || (((x >= this.m_BlockWidth) || (y >= this.m_BlockHeight)) || (this.m_Map == null)))
            {
                return this.m_InvalidLandBlock;
            }
            return this.GetBlock(x, y).m_LandTiles;
        }

        public Tile GetLandTile(int x, int y)
        {
            return this.GetLandBlock(x >> 3, y >> 3)[((y & 7) << 3) + (x & 7)];
        }

        public HuedTile[][][] GetStaticBlock(int x, int y)
        {
            if ((((x < 0) || (y < 0)) || ((x >= this.m_BlockWidth) || (y >= this.m_BlockHeight))) || ((this.m_Statics == null) || (this.m_Index == null)))
            {
                return this.m_EmptyStaticBlock;
            }
            return this.GetBlock(x, y).m_StaticTiles;
        }

        public HuedTile[] GetStaticTiles(int x, int y)
        {
            return this.GetStaticBlock(x >> 3, y >> 3)[x & 7][y & 7];
        }

        private MapBlock LoadBlock(int x, int y)
        {
            return new MapBlock(this.ReadLandBlock(x, y), this.ReadStaticBlock(x, y));
        }

        private unsafe Tile[] ReadLandBlock(int x, int y)
        {
            FileStream landData;
            int num = 0;
            int[][] landBlockRefs = this.m_Patch.LandBlockRefs;
            if ((landBlockRefs != null) && (landBlockRefs[x] != null))
            {
                num = landBlockRefs[x][y];
            }
            if (num < 0)
            {
                landData = this.m_Patch.LandData;
                if (landData == null)
                {
                    return new Tile[0x40];
                }
                landData.Seek((long) (4 + (0xc4 * ~num)), SeekOrigin.Begin);
            }
            else
            {
                landData = this.m_Map;
                if (landData == null)
                {
                    return new Tile[0x40];
                }
                landData.Seek((long) (4 + (0xc4 * ((x * this.m_BlockHeight) + y))), SeekOrigin.Begin);
            }
            Tile[] tileArray = new Tile[0x40];
            fixed (Tile* tileRef = tileArray)
            {
                Engine.NativeRead(landData, (void*) tileRef, 0xc0);
            }
            return tileArray;
        }

        private unsafe HuedTile[][][] ReadStaticBlock(int x, int y)
        {
            BinaryReader staticLookupReader;
            FileStream staticData;
            int num = 0;
            int[][] staticBlockRefs = this.m_Patch.StaticBlockRefs;
            if ((staticBlockRefs != null) && (staticBlockRefs[x] != null))
            {
                num = staticBlockRefs[x][y];
            }
            if (num < 0)
            {
                staticLookupReader = this.m_Patch.StaticLookupReader;
                staticData = this.m_Patch.StaticData;
                if ((staticLookupReader == null) || (staticData == null))
                {
                    return this.m_EmptyStaticBlock;
                }
                staticLookupReader.BaseStream.Seek((long) (12 * ~num), SeekOrigin.Begin);
            }
            else
            {
                staticLookupReader = this.m_IndexReader;
                staticData = this.m_Statics;
                if ((staticLookupReader == null) || (staticData == null))
                {
                    return this.m_EmptyStaticBlock;
                }
                staticLookupReader.BaseStream.Seek((long) (((x * this.m_BlockHeight) + y) * 12), SeekOrigin.Begin);
            }
            int num2 = staticLookupReader.ReadInt32();
            int bytes = staticLookupReader.ReadInt32();
            if ((num2 < 0) || (bytes <= 0))
            {
                return this.m_EmptyStaticBlock;
            }
            int num4 = bytes / 7;
            staticData.Seek((long) num2, SeekOrigin.Begin);
            StaticTile[] tileArray = new StaticTile[num4];
            fixed (StaticTile* tileRef = tileArray)
            {
                Engine.NativeRead(staticData, (void*) tileRef, bytes);
                if (m_Lists == null)
                {
                    m_Lists = new TileList[8][];
                    for (int j = 0; j < 8; j++)
                    {
                        m_Lists[j] = new TileList[8];
                        for (int k = 0; k < 8; k++)
                        {
                            m_Lists[j][k] = new TileList();
                        }
                    }
                }
                TileList[][] lists = m_Lists;
                StaticTile* tilePtr = tileRef;
                StaticTile* tilePtr2 = tileRef + num4;
                while (tilePtr < tilePtr2)
                {
                    lists[tilePtr->m_X & 7][tilePtr->m_Y & 7].Add((short) ((tilePtr->m_ID & 0x3fff) + 0x4000), tilePtr->m_Hue, tilePtr->m_Z);
                    tilePtr++;
                }
                HuedTile[][][] tileArray2 = new HuedTile[8][][];
                for (int i = 0; i < 8; i++)
                {
                    tileArray2[i] = new HuedTile[8][];
                    for (int m = 0; m < 8; m++)
                    {
                        tileArray2[i][m] = lists[i][m].ToArray();
                    }
                }
                return tileArray2;
            }
        }

        public int BlockHeight
        {
            get
            {
                return this.m_BlockHeight;
            }
        }

        public int BlockWidth
        {
            get
            {
                return this.m_BlockWidth;
            }
        }

        public HuedTile[][][] EmptyStaticBlock
        {
            get
            {
                return this.m_EmptyStaticBlock;
            }
        }

        public int Height
        {
            get
            {
                return this.m_Height;
            }
        }

        public Tile[] InvalidLandBlock
        {
            get
            {
                return this.m_InvalidLandBlock;
            }
        }

        public TileMatrixPatch Patch
        {
            get
            {
                return this.m_Patch;
            }
        }

        public int Width
        {
            get
            {
                return this.m_Width;
            }
        }
    }
}

