namespace Client
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;

    public class Map
    {
        private static AnimData[] m_Anim;
        private static MapPackage m_Cached;
        private static ArrayList[,] m_CellPool;
        private static TileMatrix m_Felucca;
        private static byte[,] m_FlagPool;
        public static int m_Height;
        private static TileMatrix m_Ilshenar;
        private static byte[,] m_IndexPool;
        private static int[] m_InvalidLandTiles = new int[] { 580 };
        private static bool m_IsCached;
        private static ItemData[] m_Item;
        public static TileFlags[] m_ItemFlags;
        private static LandData[] m_Land;
        private static LandTile[,] m_LandTiles;
        private static bool m_Locked;
        private static Queue m_LockQueue = new Queue();
        private static TileMatrix m_Malas;
        private static byte[] m_MapDataBuffer;
        private static int m_MaxLOSDistance = 0x12;
        private unsafe static sbyte* m_pAnims;
        private static Point3DList m_PathList = new Point3DList();
        private static bool m_QueueInvalidate;
        private static byte[] m_StaDataBuffer;
        private static byte[] m_StaIndexBuffer;
        private static MapBlock[] m_StrongReferences;
        private static FileStream m_TileDataStream;
        private static TileMatrix m_Tokuno;
        private static TileMatrix m_Trammel;
        private static Vector m_vLight;
        public static int m_Width;
        private static int m_World;
        private static int m_X;
        private static int m_Y;
        private static Type tCorpseCell = typeof(CorpseCell);
        private static Type tDynamicItem = typeof(DynamicItem);
        private static Type tLandTile = typeof(LandTile);
        private static Type tMobileCell = typeof(MobileCell);
        private static Type tStaticItem = typeof(StaticItem);

        static unsafe Map()
        {
            Debug.TimeBlock("Initializing Map");
            m_MapDataBuffer = new byte[0xc4];
            m_StaIndexBuffer = new byte[12];
            m_StaDataBuffer = new byte[0x100];
            m_pAnims = (sbyte*) Memory.Alloc(0x100000);
            string path = "Data/QuickLoad/TileData.mul";
            string str2 = Engine.FileManager.BasePath(path);
            FileInfo info = new FileInfo(Engine.FileManager.ResolveMUL(Files.Tiledata));
            FileInfo info2 = new FileInfo(Engine.FileManager.ResolveMUL(Files.Animdata));
            FileInfo info3 = new FileInfo(Engine.FileManager.ResolveMUL(Files.Verdata));
            if (File.Exists(str2))
            {
                BinaryReader reader = new BinaryReader(Engine.FileManager.OpenBaseMUL(path));
                DateTime time = DateTime.FromFileTime(reader.ReadInt64());
                DateTime time2 = DateTime.FromFileTime(reader.ReadInt64());
                DateTime time3 = DateTime.FromFileTime(reader.ReadInt64());
                if (((reader.BaseStream.Length == ((((((0x4000 * sizeof(LandData)) + (0x4000 * sizeof(ItemData))) + 0x110000) + 8) + 8) + 8)) && (info.LastWriteTime == time)) && ((info2.LastWriteTime == time2) && (info3.LastWriteTime == time3)))
                {
                    byte[] buffer = new byte[((0x4000 * sizeof(LandData)) + (0x4000 * sizeof(ItemData))) + 0x110000];
                    reader.BaseStream.Read(buffer, 0, buffer.Length);
                    m_Land = new LandData[0x4000];
                    m_Item = new ItemData[0x4000];
                    m_Anim = new AnimData[0x4000];
                    fixed (LandData* dataRef = m_Land)
                    {
                        LandData* dataPtr = dataRef;
                        fixed (ItemData* dataRef2 = m_Item)
                        {
                            ItemData* dataPtr2 = dataRef2;
                            fixed (AnimData* dataRef3 = m_Anim)
                            {
                                AnimData* dataPtr3 = dataRef3;
                                fixed (byte* numRef = buffer)
                                {
                                    byte* numPtr = numRef;
                                    LandData* dataPtr4 = (LandData*) numPtr;
                                    int num = 0x4000;
                                    while (--num >= 0)
                                    {
                                        dataPtr++;
                                        dataPtr4++;
                                        dataPtr[0] = dataPtr4[0];
                                    }
                                    ItemData* dataPtr5 = (ItemData*) dataPtr4;
                                    num = 0x4000;
                                    int num2 = -1;
                                    m_ItemFlags = new TileFlags[0x4000];
                                    while (--num >= 0)
                                    {
                                        m_ItemFlags[++num2].Value = dataPtr5->m_Flags;
                                        dataPtr2++;
                                        dataPtr5++;
                                        dataPtr2[0] = dataPtr5[0];
                                    }
                                    num = 0x4000;
                                    numPtr = (byte*) dataPtr5;
                                    sbyte* pAnims = m_pAnims;
                                    while (--num >= 0)
                                    {
                                        dataPtr3->pvFrames = pAnims;
                                        pAnims += 0x40;
                                        int* pvFrames = (int*) dataPtr3->pvFrames;
                                        int* numPtr4 = (int*) numPtr;
                                        pvFrames[0] = numPtr4[0];
                                        pvFrames[1] = numPtr4[1];
                                        pvFrames[2] = numPtr4[2];
                                        pvFrames[3] = numPtr4[3];
                                        pvFrames[4] = numPtr4[4];
                                        pvFrames[5] = numPtr4[5];
                                        pvFrames[6] = numPtr4[6];
                                        pvFrames[7] = numPtr4[7];
                                        pvFrames[8] = numPtr4[8];
                                        pvFrames[9] = numPtr4[9];
                                        pvFrames[10] = numPtr4[10];
                                        pvFrames[11] = numPtr4[11];
                                        pvFrames[12] = numPtr4[12];
                                        pvFrames[13] = numPtr4[13];
                                        pvFrames[14] = numPtr4[14];
                                        pvFrames[15] = numPtr4[15];
                                        numPtr += 0x40;
                                        dataPtr3->unknown = *(numPtr++);
                                        dataPtr3->frameCount = *(numPtr++);
                                        dataPtr3->frameInterval = *(numPtr++);
                                        dataPtr3->frameStartInterval = *(numPtr++);
                                        dataPtr3++;
                                    }
                                }
                            }
                        }
                    }
                    reader.Close();
                    Debug.EndBlock();
                    return;
                }
                reader.Close();
            }
            m_Land = new LandData[0x4000];
            m_Item = new ItemData[0x4000];
            int count = 0x68800;
            byte[] buffer2 = new byte[count];
            Stream stream = Engine.FileManager.OpenMUL(Files.Tiledata);
            stream.Read(buffer2, 0, count);
            fixed (LandData* dataRef4 = m_Land)
            {
                LandData* dataPtr6 = dataRef4;
                fixed (byte* numRef2 = buffer2)
                {
                    int num4 = 0;
                    byte* numPtr5 = numRef2;
                    while (num4++ < 0x200)
                    {
                        numPtr5 += 4;
                        int num5 = 0;
                        while (num5++ < 0x20)
                        {
                            dataPtr6->m_Flags = *((int*) numPtr5);
                            dataPtr6->m_Texture = *((short*) (numPtr5 + 4));
                            dataPtr6++;
                            numPtr5 += 0x1a;
                        }
                    }
                }
            }
            count = 0x94800;
            buffer2 = new byte[count];
            stream.Read(buffer2, 0, count);
            fixed (ItemData* dataRef5 = m_Item)
            {
                ItemData* dataPtr7 = dataRef5;
                fixed (byte* numRef3 = buffer2)
                {
                    int num6 = 0;
                    byte* numPtr6 = numRef3;
                    int num7 = 0;
                    while (num6++ < 0x200)
                    {
                        numPtr6 += 4;
                        int num8 = 0;
                        while (num8++ < 0x20)
                        {
                            num7++;
                            dataPtr7->m_Flags = *((int*) numPtr6);
                            dataPtr7->m_Weight = numPtr6[4];
                            dataPtr7->m_Quality = numPtr6[5];
                            dataPtr7->m_Extra = numPtr6[6];
                            dataPtr7->m_Quantity = numPtr6[9];
                            dataPtr7->m_Animation = *((short*) (numPtr6 + 10));
                            dataPtr7->m_Value = numPtr6[15];
                            dataPtr7->m_Height = numPtr6[0x10];
                            dataPtr7++;
                            numPtr6 += 0x25;
                        }
                    }
                }
            }
            stream.Close();
            m_Anim = new AnimData[0x4000];
            count = 0x112000;
            buffer2 = new byte[count];
            Stream stream2 = Engine.FileManager.OpenMUL(Files.Animdata);
            stream2.Read(buffer2, 0, count);
            stream2.Close();
            fixed (AnimData* dataRef6 = m_Anim)
            {
                AnimData* dataPtr8 = dataRef6;
                fixed (byte* numRef4 = buffer2)
                {
                    byte* numPtr7 = numRef4;
                    int num9 = 0;
                    sbyte* numPtr8 = m_pAnims;
                    while (num9++ < 0x800)
                    {
                        numPtr7 += 4;
                        int num10 = 0;
                        while (num10++ < 8)
                        {
                            dataPtr8->pvFrames = numPtr8;
                            numPtr8 += 0x40;
                            sbyte* numPtr9 = (sbyte*) numPtr7;
                            for (int i = 0; i < 0x40; i++)
                            {
                                dataPtr8->pvFrames[i] = *(numPtr9++);
                            }
                            numPtr7 += 0x40;
                            dataPtr8->unknown = *(numPtr7++);
                            dataPtr8->frameCount = *(numPtr7++);
                            dataPtr8->frameInterval = *(numPtr7++);
                            dataPtr8->frameStartInterval = *(numPtr7++);
                            dataPtr8++;
                        }
                    }
                }
            }
            Stream stream3 = Engine.FileManager.OpenMUL(Files.Verdata);
            buffer2 = new byte[stream3.Length];
            stream3.Read(buffer2, 0, buffer2.Length);
            stream3.Close();
            fixed (byte* numRef5 = buffer2)
            {
                int* numPtr10 = (int*) numRef5;
                numPtr10++;
                int num12 = numPtr10[0];
                int num13 = 0;
                while (num13++ < num12)
                {
                    numPtr10++;
                    int num14 = numPtr10[0];
                    if (num14 == 30)
                    {
                        numPtr10++;
                        int num15 = numPtr10[0];
                        numPtr10++;
                        int num16 = numPtr10[0];
                        numPtr10++;
                        int num17 = numPtr10[0];
                        numPtr10++;
                        int num18 = numPtr10[0];
                        if (num15 < 0x200)
                        {
                            fixed (LandData* dataRef7 = m_Land)
                            {
                                LandData* dataPtr9 = dataRef7;
                                dataPtr9 += num15 * 0x20;
                                byte* numPtr11 = (numRef5 + num16) + 4;
                                int num19 = 0;
                                while (num19++ < 0x20)
                                {
                                    dataPtr9->m_Flags = *((int*) numPtr11);
                                    dataPtr9->m_Texture = *((short*) (numPtr11 + 4));
                                    dataPtr9++;
                                    numPtr11 += 0x1a;
                                }
                            }
                        }
                        else if (num15 < 0x400)
                        {
                            fixed (ItemData* dataRef8 = m_Item)
                            {
                                ItemData* dataPtr10 = dataRef8;
                                dataPtr10 += (num15 - 0x200) * 0x20;
                                byte* numPtr12 = (numRef5 + num16) + 4;
                                int num20 = 0;
                                while (num20++ < 0x20)
                                {
                                    dataPtr10->m_Flags = *((int*) numPtr12);
                                    dataPtr10->m_Weight = numPtr12[4];
                                    dataPtr10->m_Quality = numPtr12[5];
                                    dataPtr10->m_Quantity = numPtr12[9];
                                    dataPtr10->m_Animation = *((short*) (numPtr12 + 10));
                                    dataPtr10->m_Value = numPtr12[15];
                                    dataPtr10->m_Height = numPtr12[0x10];
                                    dataPtr10++;
                                    numPtr12 += 0x25;
                                }
                            }
                        }
                    }
                    else
                    {
                        numPtr10 += 4;
                    }
                }
            }
            BinaryWriter writer = new BinaryWriter(Engine.FileManager.CreateBaseMUL(path));
            writer.Write(info.LastWriteTime.ToFileTime());
            writer.Write(info2.LastWriteTime.ToFileTime());
            writer.Write(info3.LastWriteTime.ToFileTime());
            fixed (LandData* dataRef9 = m_Land)
            {
                LandData* dataPtr11 = dataRef9;
                fixed (ItemData* dataRef10 = m_Item)
                {
                    ItemData* dataPtr12 = dataRef10;
                    fixed (AnimData* dataRef11 = m_Anim)
                    {
                        AnimData* dataPtr13 = dataRef11;
                        byte[] buffer3 = new byte[(0x4000 * sizeof(LandData)) + (0x4000 * sizeof(ItemData))];
                        fixed (byte* numRef6 = buffer3)
                        {
                            LandData* dataPtr14 = (LandData*) numRef6;
                            for (int k = 0; k < 0x4000; k++)
                            {
                                dataPtr14++;
                                dataPtr11++;
                                dataPtr14[0] = dataPtr11[0];
                            }
                            ItemData* dataPtr15 = (ItemData*) dataPtr14;
                            m_ItemFlags = new TileFlags[0x4000];
                            for (int m = 0; m < 0x4000; m++)
                            {
                                m_ItemFlags[m].Value = dataPtr12->m_Flags;
                                dataPtr15++;
                                dataPtr12++;
                                dataPtr15[0] = dataPtr12[0];
                            }
                        }
                        writer.Write(buffer3);
                        for (int j = 0; j < 0x4000; j++)
                        {
                            for (int n = 0; n < 0x40; n++)
                            {
                                writer.Write(dataPtr13->pvFrames[n]);
                            }
                            writer.Write(dataPtr13->unknown);
                            writer.Write(dataPtr13->frameCount);
                            writer.Write(dataPtr13->frameInterval);
                            writer.Write(dataPtr13->frameStartInterval);
                            dataPtr13++;
                        }
                    }
                }
            }
            writer.Flush();
            writer.Close();
            Debug.EndBlock();
        }

        private static void Add(int x, int y, Item item)
        {
            if ((item != null) && (m_Cached.cells != null))
            {
                item.OldMapX = (m_X << 3) + x;
                item.OldMapY = (m_Y << 3) + y;
                m_Cached.cells[x, y].Add(DynamicItem.Instantiate(item));
            }
        }

        private static void Add(int x, int y, Mobile m)
        {
            if ((m != null) && (m_Cached.cells != null))
            {
                m.OldMapX = (m_X << 3) + x;
                m.OldMapY = (m_Y << 3) + y;
                m_Cached.cells[x, y].Add(MobileCell.Instantiate(m));
            }
        }

        private static float CalcZ(int xOffset, int yOffset, sbyte Z)
        {
            return CalcZ(xOffset, yOffset, Z, 0);
        }

        private static float CalcZ(int xOffset, int yOffset, sbyte Z, byte height)
        {
            return (float) (xOffset + yOffset);
        }

        public static unsafe void Dispose()
        {
            if (m_pAnims != null)
            {
                Memory.Free((void*) m_pAnims);
            }
            for (int i = 0; i < 0x4000; i++)
            {
                m_Anim[i].pvFrames = null;
            }
            m_ItemFlags = null;
            m_Land = null;
            m_Item = null;
            m_Anim = null;
            m_MapDataBuffer = null;
            m_StaDataBuffer = null;
            m_StaIndexBuffer = null;
            m_CellPool = null;
            if (m_TileDataStream != null)
            {
                m_TileDataStream.Close();
                m_TileDataStream = null;
            }
        }

        public static void FixPoints(ref Point3D top, ref Point3D bottom)
        {
            if (bottom.X < top.X)
            {
                int x = top.X;
                top.X = bottom.X;
                bottom.X = x;
            }
            if (bottom.Y < top.Y)
            {
                int y = top.Y;
                top.Y = bottom.Y;
                bottom.Y = y;
            }
            if (bottom.Z < top.Z)
            {
                int z = top.Z;
                top.Z = bottom.Z;
                bottom.Z = z;
            }
        }

        public static AnimData GetAnim(int ItemID)
        {
            return m_Anim[ItemID & 0x3fff];
        }

        public static short GetAnimation(int ItemID)
        {
            return m_Item[ItemID & 0x3fff].m_Animation;
        }

        public static int GetAverageZ(int x, int y)
        {
            int z = 0;
            int avg = 0;
            int top = 0;
            GetAverageZ(x, y, ref z, ref avg, ref top);
            return avg;
        }

        public static void GetAverageZ(int x, int y, ref int z, ref int avg, ref int top)
        {
            int num = GetZ(x, y, Engine.m_World);
            int num2 = GetZ(x, y + 1, Engine.m_World);
            int num3 = GetZ(x + 1, y, Engine.m_World);
            int num4 = GetZ(x + 1, y + 1, Engine.m_World);
            z = num;
            if (num2 < z)
            {
                z = num2;
            }
            if (num3 < z)
            {
                z = num3;
            }
            if (num4 < z)
            {
                z = num4;
            }
            top = num;
            if (num2 > top)
            {
                top = num2;
            }
            if (num3 > top)
            {
                top = num3;
            }
            if (num4 > top)
            {
                top = num4;
            }
            if (Math.Abs((int) (num - num4)) <= Math.Abs((int) (num2 - num3)))
            {
                avg = (int) Math.Floor((double) (((double) (num + num4)) / 2.0));
            }
            else
            {
                avg = (int) Math.Floor((double) (((double) (num2 + num3)) / 2.0));
            }
        }

        public static MapPackage GetCache()
        {
            return m_Cached;
        }

        public static LandTile GetCell(int xCell, int yCell, int world)
        {
            Tile landTile = GetMatrix(world).GetLandTile(xCell, yCell);
            LandTile t = new LandTile();
            LandTile.Initialize(t, (short) landTile.ID, (sbyte) landTile.Z);
            return t;
        }

        public static int GetDispID(int id, int amount, ref bool xDouble)
        {
            id &= 0x3fff;
            xDouble = (amount > 1) && m_ItemFlags[id][TileFlag.Generic];
            if ((id >= 0xeea) && (id <= 0xef2))
            {
                int num = (id - 0xeea) / 3;
                num *= 3;
                num += 0xeea;
                xDouble = false;
                if (amount <= 1)
                {
                    id = num;
                    return id;
                }
                if (amount <= 5)
                {
                    id = num + 1;
                    return id;
                }
                id = num + 2;
            }
            return id;
        }

        public static int GetExtra(int ItemID)
        {
            return m_Item[ItemID & 0x3fff].m_Extra;
        }

        public static byte GetHeight(int TileID)
        {
            if (TileID >= 0x4000)
            {
                return m_Item[TileID & 0x3fff].m_Height;
            }
            return 0;
        }

        public static TileFlags GetLandFlags(int LandID)
        {
            return new TileFlags(m_Land[LandID & 0x3fff].m_Flags);
        }

        public static MapPackage GetMap(int X, int Y, int W, int H)
        {
            LandTile tile;
            int num28;
            int num29;
            if ((((m_X == X) && (m_Y == Y)) && ((m_Width == W) && (m_Height == H))) && (((m_World == Engine.m_World) && (m_vLight == MapLighting.vLight)) && (m_IsCached && !m_QueueInvalidate)))
            {
                return m_Cached;
            }
            m_QueueInvalidate = false;
            if (m_Cached.cells != null)
            {
                int length = m_Cached.cells.GetLength(0);
                int num2 = m_Cached.cells.GetLength(1);
                for (int num3 = 0; num3 < length; num3++)
                {
                    for (int num4 = 0; num4 < num2; num4++)
                    {
                        ArrayList list = m_Cached.cells[num3, num4];
                        if (list != null)
                        {
                            int count = list.Count;
                            for (int num6 = 0; num6 < count; num6++)
                            {
                                ((ICell) list[num6]).Dispose();
                            }
                        }
                    }
                }
            }
            m_X = X;
            m_Y = Y;
            m_Width = W;
            m_Height = H;
            m_World = Engine.m_World;
            m_vLight = MapLighting.vLight;
            if (m_StrongReferences == null)
            {
                m_StrongReferences = new MapBlock[W * H];
            }
            int num7 = W << 3;
            int num8 = H << 3;
            if (m_CellPool == null)
            {
                m_CellPool = new ArrayList[num7, num8];
                for (int num9 = 0; num9 < num7; num9++)
                {
                    for (int num10 = 0; num10 < num8; num10++)
                    {
                        m_CellPool[num9, num10] = new ArrayList(4);
                    }
                }
            }
            else
            {
                for (int num11 = 0; num11 < num7; num11++)
                {
                    for (int num12 = 0; num12 < num8; num12++)
                    {
                        m_CellPool[num11, num12].Clear();
                    }
                }
            }
            if (m_LandTiles == null)
            {
                m_LandTiles = new LandTile[num7, num8];
                for (int num13 = 0; num13 < num7; num13++)
                {
                    for (int num14 = 0; num14 < num8; num14++)
                    {
                        m_LandTiles[num13, num14] = new LandTile();
                    }
                }
            }
            if (m_IndexPool == null)
            {
                m_IndexPool = new byte[num7, num8];
            }
            if (m_FlagPool == null)
            {
                m_FlagPool = new byte[num7, num8];
            }
            ArrayList[,] cellPool = m_CellPool;
            IComparer comparer = TileSorter.Comparer;
            MapPackage map = new MapPackage {
                cells = cellPool,
                CellX = X << 3,
                CellY = Y << 3,
                landTiles = m_LandTiles
            };
            Engine.Multis.Update(map);
            TileMatrix matrix = GetMatrix(Engine.m_World);
            int num15 = 0;
            for (int i = X; num15 < W; i++)
            {
                int num17 = 0;
                for (int num18 = Y; num17 < H; num18++)
                {
                    MapBlock block = matrix.GetBlock(i, num18);
                    m_StrongReferences[(num17 * W) + num15] = block;
                    HuedTile[][][] tileArray = (block == null) ? matrix.EmptyStaticBlock : block.m_StaticTiles;
                    Tile[] tileArray2 = (block == null) ? matrix.InvalidLandBlock : block.m_LandTiles;
                    int index = 0;
                    int num20 = i << 3;
                    for (int num21 = num15 << 3; index < 8; num21++)
                    {
                        int num22 = 0;
                        int num23 = num18 << 3;
                        for (int num24 = num17 << 3; num22 < 8; num24++)
                        {
                            HuedTile[] tileArray3 = tileArray[index][num22];
                            for (int num25 = 0; num25 < tileArray3.Length; num25++)
                            {
                                cellPool[num21, num24].Add(StaticItem.Instantiate(tileArray3[num25], num25, ((num20 * matrix.Height) + num23) | (num25 << 0x19)));
                            }
                            tile = m_LandTiles[num21, num24];
                            Tile tile2 = tileArray2[(num22 * 8) + index];
                            LandTile.Initialize(tile, (short) tile2.ID, (sbyte) tile2.Z);
                            cellPool[num21, num24].Add(tile);
                            num22++;
                            num23++;
                        }
                        index++;
                        num20++;
                    }
                    num17++;
                }
                num15++;
            }
            int num26 = X << 3;
            int num27 = Y << 3;
            IEnumerator enumerator = World.Items.Values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Item current = (Item) enumerator.Current;
                if (current.Serial == 0x40000430)
                {
                    int num30 = 0;
                    num30++;
                }
                if ((current.Visible && current.InWorld) && !current.IsMulti)
                {
                    num28 = current.X - num26;
                    num29 = current.Y - num27;
                    if (((num28 >= 0) && (num28 < num7)) && ((num29 >= 0) && (num29 < num8)))
                    {
                        current.OldMapX = current.X;
                        current.OldMapY = current.Y;
                        cellPool[num28, num29].Add(DynamicItem.Instantiate(current));
                    }
                }
            }
            enumerator = World.Mobiles.Values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Mobile mobile = (Mobile) enumerator.Current;
                if (mobile.Visible)
                {
                    num28 = mobile.X - num26;
                    num29 = mobile.Y - num27;
                    if (((num28 >= 0) && (num28 < num7)) && ((num29 >= 0) && (num29 < num8)))
                    {
                        mobile.OldMapX = mobile.X;
                        mobile.OldMapY = mobile.Y;
                        cellPool[num28, num29].Add(MobileCell.Instantiate(mobile));
                    }
                }
            }
            for (int j = 0; j < num7; j++)
            {
                for (int num32 = 0; num32 < num8; num32++)
                {
                    int num34;
                    int z;
                    int num36;
                    tile = m_LandTiles[j, num32];
                    int num33 = num34 = z = num36 = tile.m_Z;
                    if (j < (num7 - 1))
                    {
                        z = m_LandTiles[j + 1, num32].m_Z;
                    }
                    if (num32 < (num8 - 1))
                    {
                        num34 = m_LandTiles[j, num32 + 1].m_Z;
                    }
                    if ((j < (num7 - 1)) && (num32 < (num8 - 1)))
                    {
                        num36 = m_LandTiles[j + 1, num32 + 1].m_Z;
                    }
                    if (tile.m_FoldLeftRight = Math.Abs((int) (num33 - num36)) <= Math.Abs((int) (num34 - z)))
                    {
                        tile.SortZ = (sbyte) Math.Floor((double) (((double) (num33 + num36)) / 2.0));
                    }
                    else
                    {
                        tile.SortZ = (sbyte) Math.Floor((double) (((double) (num34 + z)) / 2.0));
                    }
                    tile.m_Guarded = !GetLandFlags(tile.m_ID)[TileFlag.Impassable] && (Region.Find(Region.GuardedRegions, j + num26, num32 + num27, tile.SortZ, Engine.m_World) != null);
                    ArrayList list2 = cellPool[j, num32];
                    if (list2.Count > 1)
                    {
                        list2.Sort(comparer);
                    }
                }
            }
            map = new MapPackage {
                flags = m_FlagPool,
                cells = cellPool,
                CellX = X << 3,
                CellY = Y << 3,
                landTiles = m_LandTiles
            };
            MapLighting.GetColorMap(ref map);
            m_Cached = map;
            m_IsCached = true;
            for (int k = -1; k <= H; k++)
            {
                Engine.QueueMapLoad(X - 1, Y + k, matrix);
            }
            for (int m = 0; m < W; m++)
            {
                Engine.QueueMapLoad(X + m, Y - 1, matrix);
                Engine.QueueMapLoad(X + m, Y + H, matrix);
            }
            for (int n = -1; n <= H; n++)
            {
                Engine.QueueMapLoad(X + W, Y + n, matrix);
            }
            return map;
        }

        public static TileMatrix GetMatrix(int world)
        {
            switch (world)
            {
                case 0:
                    return Felucca;

                case 1:
                    return Trammel;

                case 2:
                    return Ilshenar;

                case 3:
                    return Malas;

                case 4:
                    return Tokuno;
            }
            return Felucca;
        }

        public static Point3D GetPoint(object o, bool eye)
        {
            Point3D pointd;
            if (o is Mobile)
            {
                Mobile mobile = (Mobile) o;
                return new Point3D(mobile.X, mobile.Y, mobile.Z) { Z = pointd.Z + 14 };
            }
            if (o is Item)
            {
                Item item = (Item) o;
                return new Point3D(item.X, item.Y, item.Z) { Z = pointd.Z + ((GetHeight(item.ID) / 2) + 1) };
            }
            if (o is Point3D)
            {
                return (Point3D) o;
            }
            if (o is IPoint3D)
            {
                return new Point3D((IPoint3D) o);
            }
            Console.WriteLine("Warning: Invalid object ({0}) in line of sight", o);
            return new Point3D(0, 0, 0);
        }

        public static byte GetQuality(int TileID)
        {
            return m_Item[TileID & 0x3fff].m_Quality;
        }

        public static byte GetQuantity(int TileID)
        {
            return m_Item[TileID & 0x3fff].m_Quantity;
        }

        public static short GetTexture(int LandID)
        {
            return m_Land[LandID & 0x3fff].m_Texture;
        }

        public static TileFlags GetTileFlags(int TileID)
        {
            if (TileID >= 0x4000)
            {
                return m_ItemFlags[TileID & 0x3fff];
            }
            return new TileFlags(m_Land[TileID & 0x3fff].m_Flags);
        }

        public static string GetTileName(int TileID)
        {
            int num;
            if (m_TileDataStream == null)
            {
                m_TileDataStream = new FileStream(Engine.FileManager.ResolveMUL(Files.Tiledata), FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            if (TileID < 0x4000)
            {
                TileID &= 0x3fff;
                m_TileDataStream.Seek((long) (((((TileID >> 5) + 1) * 4) + (TileID * 0x1a)) + 6), SeekOrigin.Begin);
            }
            else
            {
                TileID &= 0x3fff;
                m_TileDataStream.Seek((long) (((0x68800 + (((TileID >> 5) + 1) * 4)) + (TileID * 0x25)) + 0x11), SeekOrigin.Begin);
            }
            int num2 = 20;
            StringBuilder builder = new StringBuilder(20);
            while ((--num2 >= 0) && ((num = m_TileDataStream.ReadByte()) != 0))
            {
                builder.Append((char) num);
            }
            return builder.ToString();
        }

        public static string GetTileProperName(int TileID)
        {
            string str = ReplaceAmount(GetTileName(TileID), 1);
            TileFlags tileFlags = GetTileFlags(TileID);
            if (tileFlags[TileFlag.ArticleA])
            {
                return string.Format("a {0}", str);
            }
            if (tileFlags[TileFlag.ArticleAn])
            {
                return string.Format("an {0}", str);
            }
            return str;
        }

        public static int GetWeight(int ItemID)
        {
            return m_Item[ItemID & 0x3fff].m_Weight;
        }

        public static int GetZ(int x, int y, int w)
        {
            return GetMatrix(w).GetLandTile(x, y).Z;
        }

        public static bool InRange(IPoint2D p)
        {
            return ((((p.X >= m_Cached.CellX) && (p.X <= (m_Cached.CellX + Renderer.cellWidth))) && (p.Y >= m_Cached.CellY)) && (p.Y <= (m_Cached.CellY + Renderer.cellHeight)));
        }

        public static void Invalidate()
        {
            m_IsCached = false;
            Engine.Redraw();
        }

        public static bool IsValid(int X, int Y)
        {
            return ((((X >= 0) && (X < (m_Width << 3))) && (Y >= 0)) && (Y < (m_Height << 3)));
        }

        public static bool LineOfSight(Mobile from, Mobile to)
        {
            if (from == to)
            {
                return true;
            }
            Point3D org = new Point3D(from.X, from.Y, from.Z);
            Point3D dest = new Point3D(to.X, to.Y, to.Z);
            org.Z += 14;
            dest.Z += 14;
            return LineOfSight(org, dest);
        }

        public static bool LineOfSight(Mobile from, Point3D target)
        {
            Point3D pointd;
            pointd = new Point3D(from.X, from.Y, from.Z) {
                Z = pointd.Z + 14
            };
            return LineOfSight(pointd, target);
        }

        public static bool LineOfSight(Point3D org, Point3D dest)
        {
            if (!World.InRange(org, dest, m_MaxLOSDistance))
            {
                return false;
            }
            Point3D pointd = org;
            Point3D pointd2 = dest;
            if (((org.X > dest.X) || ((org.X == dest.X) && (org.Y > dest.Y))) || (((org.X == dest.X) && (org.Y == dest.Y)) && (org.Z > dest.Z)))
            {
                Point3D pointd3 = org;
                org = dest;
                dest = pointd3;
            }
            Point3DList pathList = m_PathList;
            if (org != dest)
            {
                double num4;
                if (pathList.Count > 0)
                {
                    pathList.Clear();
                }
                int num8 = dest.X - org.X;
                int num9 = dest.Y - org.Y;
                int num10 = dest.Z - org.Z;
                double num3 = Math.Sqrt((double) ((num8 * num8) + (num9 * num9)));
                if (num10 != 0)
                {
                    num4 = Math.Sqrt((num3 * num3) + (num10 * num10));
                }
                else
                {
                    num4 = num3;
                }
                double num = ((double) num9) / num4;
                double num2 = ((double) num8) / num4;
                num3 = ((double) num10) / num4;
                double y = org.Y;
                double z = org.Z;
                double x = org.X;
                while ((NumberBetween(x, dest.X, org.X, 0.5) && NumberBetween(y, dest.Y, org.Y, 0.5)) && NumberBetween(z, dest.Z, org.Z, 0.5))
                {
                    int num11 = (int) Math.Round(x);
                    int num12 = (int) Math.Round(y);
                    int num13 = (int) Math.Round(z);
                    if (pathList.Count > 0)
                    {
                        Point3D last = pathList.Last;
                        if (((last.X != num11) || (last.Y != num12)) || (last.Z != num13))
                        {
                            pathList.Add(num11, num12, num13);
                        }
                    }
                    else
                    {
                        pathList.Add(num11, num12, num13);
                    }
                    x += num2;
                    y += num;
                    z += num3;
                }
                if (pathList.Count == 0)
                {
                    return true;
                }
                if (pathList.Last != dest)
                {
                    pathList.Add(dest);
                }
                Point3D top = org;
                Point3D bottom = dest;
                FixPoints(ref top, ref bottom);
                int count = pathList.Count;
                MapPackage cache = GetCache();
                for (int i = 0; i < count; i++)
                {
                    Point3D pointd7 = pathList[i];
                    int num17 = pointd7.X - cache.CellX;
                    int num18 = pointd7.Y - cache.CellY;
                    if (((num17 < 0) || (num18 < 0)) || ((num17 >= Renderer.cellWidth) || (num18 >= Renderer.cellHeight)))
                    {
                        return false;
                    }
                    ArrayList list2 = cache.cells[num17, num18];
                    bool flag = false;
                    bool flag2 = false;
                    for (int j = 0; j < list2.Count; j++)
                    {
                        int height;
                        TileFlags flags;
                        ICell cell = (ICell) list2[j];
                        if (cell is LandTile)
                        {
                            LandTile tile = (LandTile) cell;
                            for (int k = 0; k < m_InvalidLandTiles.Length; k++)
                            {
                                if (tile.ID == m_InvalidLandTiles[k])
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            int num21 = 0;
                            int avg = 0;
                            int num23 = 0;
                            GetAverageZ(pointd7.X, pointd7.Y, ref num21, ref avg, ref num23);
                            if ((((num21 > pointd7.Z) || (num23 < pointd7.Z)) || (((pointd7.X == pointd2.X) && (pointd7.Y == pointd2.Y)) && ((num21 <= pointd2.Z) && (num23 >= pointd2.Z)))) || tile.Ignored)
                            {
                                continue;
                            }
                            return false;
                        }
                        if (cell is StaticItem)
                        {
                            flag2 = true;
                            StaticItem item = (StaticItem) cell;
                            flags = m_ItemFlags[item.m_RealID & 0x3fff];
                            height = m_Item[item.m_RealID & 0x3fff].m_Height;
                            if (flags[TileFlag.Bridge])
                            {
                                height /= 2;
                            }
                            if ((((item.m_Z <= pointd7.Z) && ((item.m_Z + height) >= pointd7.Z)) && (flags[TileFlag.Window] || flags[TileFlag.NoShoot])) && (((pointd7.X != pointd2.X) || (pointd7.Y != pointd2.Y)) || ((item.m_Z > pointd2.Z) || ((item.m_Z + height) < pointd2.Z))))
                            {
                                return false;
                            }
                        }
                        else if (cell is DynamicItem)
                        {
                            flag2 = true;
                            DynamicItem item2 = (DynamicItem) cell;
                            flags = m_ItemFlags[item2.m_ID & 0x3fff];
                            height = m_Item[item2.m_ID & 0x3fff].m_Height;
                            if (flags[TileFlag.Bridge])
                            {
                                height /= 2;
                            }
                            if ((((item2.m_Z <= pointd7.Z) && ((item2.m_Z + height) >= pointd7.Z)) && (flags[TileFlag.Window] || flags[TileFlag.NoShoot])) && (((pointd7.X != pointd2.X) || (pointd7.Y != pointd2.Y)) || ((item2.m_Z > pointd2.Z) || ((item2.m_Z + height) < pointd2.Z))))
                            {
                                return false;
                            }
                        }
                    }
                    if (flag && !flag2)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool LineOfSight(object from, object dest)
        {
            return ((from == dest) || LineOfSight(GetPoint(from, true), GetPoint(dest, false)));
        }

        public static void Lock()
        {
            m_Locked = true;
        }

        public static bool NumberBetween(double num, int bound1, int bound2, double allowance)
        {
            if (bound1 > bound2)
            {
                int num2 = bound1;
                bound1 = bound2;
                bound2 = num2;
            }
            return ((num < (bound2 + allowance)) && (num > (bound1 - allowance)));
        }

        public static void QueueInvalidate()
        {
            m_QueueInvalidate = true;
        }

        private static void Remove(int x, int y, Item item)
        {
            if ((item != null) && (m_Cached.cells != null))
            {
                item.OldMapX = 0;
                item.OldMapY = 0;
                ArrayList list = m_Cached.cells[x, y];
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    ICell cell = (ICell) list[i];
                    if (cell.CellType == tDynamicItem)
                    {
                        if (((DynamicItem) cell).m_Item != item)
                        {
                            continue;
                        }
                        cell.Dispose();
                        list.RemoveAt(i);
                        break;
                    }
                    if ((cell.CellType == tCorpseCell) && (((CorpseCell) cell).Serial == item.Serial))
                    {
                        cell.Dispose();
                        list.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        private static void Remove(int x, int y, Mobile m)
        {
            if ((m != null) && (m_Cached.cells != null))
            {
                m.OldMapX = 0;
                m.OldMapY = 0;
                ArrayList list = m_Cached.cells[x, y];
                int count = list.Count;
                for (int i = 0; i < count; i++)
                {
                    ICell cell = (ICell) list[i];
                    if ((cell.CellType == tMobileCell) && (((MobileCell) cell).m_Mobile == m))
                    {
                        cell.Dispose();
                        list.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public static void RemoveItem(Item item)
        {
            if (m_Locked)
            {
                m_LockQueue.Enqueue(new RemoveItemLock(item));
            }
            else if (item != null)
            {
                int num = m_X << 3;
                int num2 = m_Y << 3;
                int x = item.OldMapX - num;
                int y = item.OldMapY - num2;
                if (IsValid(x, y))
                {
                    Remove(x, y, item);
                }
            }
        }

        public static void RemoveMobile(Mobile m)
        {
            if (m_Locked)
            {
                m_LockQueue.Enqueue(new RemoveMobileLock(m));
            }
            else if (m != null)
            {
                int num = m_X << 3;
                int num2 = m_Y << 3;
                int x = m.OldMapX - num;
                int y = m.OldMapY - num2;
                if (IsValid(x, y))
                {
                    Remove(x, y, m);
                }
            }
        }

        public static string ReplaceAmount(string Name, int Amount)
        {
            if (Name.IndexOf('%') == -1)
            {
                return Name;
            }
            Match match = Regex.Match(Name, "(?<1>[^%]*)%(?<2>[^%/]*)(?<3>/[^%]*)?%");
            if (Amount == 1)
            {
                return (match.Groups[1].Value + ((match.Groups[3].Value.Length > 0) ? match.Groups[3].Value.Substring(1) : match.Groups[3].Value));
            }
            if (match.Groups[2].Success)
            {
                return (match.Groups[1].Value + match.Groups[2].Value);
            }
            return match.Groups[1].Value;
        }

        public static void Sort(int X, int Y)
        {
            if (m_Locked)
            {
                m_LockQueue.Enqueue(new SortLock(X, Y));
            }
            else
            {
                ArrayList[,] cells = m_Cached.cells;
                if (cells != null)
                {
                    ArrayList list = cells[X, Y];
                    if (list.Count > 1)
                    {
                        list.Sort(TileSorter.Comparer);
                    }
                }
            }
        }

        public static void Unlock()
        {
            m_Locked = false;
            while (m_LockQueue.Count > 0)
            {
                ((ILocked) m_LockQueue.Dequeue()).Invoke();
            }
        }

        public static void UpdateItem(Item item)
        {
            if (m_Locked)
            {
                m_LockQueue.Enqueue(new UpdateItemLock(item));
            }
            else if (item != null)
            {
                int num = m_X << 3;
                int num2 = m_Y << 3;
                int x = item.OldMapX - num;
                int y = item.OldMapY - num2;
                int num5 = item.X - num;
                int num6 = item.Y - num2;
                if (IsValid(x, y))
                {
                    Remove(x, y, item);
                }
                if (item.Visible && IsValid(num5, num6))
                {
                    Add(num5, num6, item);
                    Sort(num5, num6);
                }
            }
        }

        public static void UpdateMobile(Mobile m)
        {
            if (m_Locked)
            {
                m_LockQueue.Enqueue(new UpdateMobileLock(m));
            }
            else if (m != null)
            {
                int num = m_X << 3;
                int num2 = m_Y << 3;
                int x = m.OldMapX - num;
                int y = m.OldMapY - num2;
                int num5 = m.X - num;
                int num6 = m.Y - num2;
                if (IsValid(x, y))
                {
                    Remove(x, y, m);
                }
                if (m.Visible && IsValid(num5, num6))
                {
                    Add(num5, num6, m);
                    Sort(num5, num6);
                }
            }
        }

        public static TileMatrix Felucca
        {
            get
            {
                if (m_Felucca == null)
                {
                    m_Felucca = new TileMatrix(0, 0, 0x1800, 0x1000);
                }
                return m_Felucca;
            }
        }

        public static TileMatrix Ilshenar
        {
            get
            {
                if (m_Ilshenar == null)
                {
                    m_Ilshenar = new TileMatrix(2, 2, 0x900, 0x640);
                }
                return m_Ilshenar;
            }
        }

        public static int[] InvalidLandTiles
        {
            get
            {
                return m_InvalidLandTiles;
            }
            set
            {
                m_InvalidLandTiles = value;
            }
        }

        public static TileMatrix Malas
        {
            get
            {
                if (m_Malas == null)
                {
                    m_Malas = new TileMatrix(3, 3, 0xa00, 0x800);
                }
                return m_Malas;
            }
        }

        public static int MaxLOSDistance
        {
            get
            {
                return m_MaxLOSDistance;
            }
            set
            {
                m_MaxLOSDistance = value;
            }
        }

        public static TileMatrix Tokuno
        {
            get
            {
                if (m_Tokuno == null)
                {
                    m_Tokuno = new TileMatrix(4, 4, 0x5a8, 0x5a8);
                }
                return m_Tokuno;
            }
        }

        public static TileMatrix Trammel
        {
            get
            {
                if (m_Trammel == null)
                {
                    m_Trammel = new TileMatrix(0, 1, 0x1800, 0x1000);
                }
                return m_Trammel;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct ItemData
        {
            public int m_Flags;
            public byte m_Weight;
            public byte m_Quality;
            public byte m_Extra;
            public byte m_Quantity;
            public short m_Animation;
            public byte m_Value;
            public byte m_Height;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct LandData
        {
            public int m_Flags;
            public short m_Texture;
        }
    }
}

