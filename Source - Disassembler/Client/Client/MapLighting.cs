namespace Client
{
    using System;
    using System.Collections;

    public class MapLighting
    {
        private const float lightAmp = 2f;
        public static bool[] m_AlwaysStretch;
        private static bool[,] m_CalcPool;
        private static LandTile[,] m_CalcVert_Tiles;
        private static Vector[,] m_CalcVert_Verts;
        private static int[,] m_ColorMap;
        private static int[,] m_ColorMap2;
        private static Vector[,,] m_NormalPool;
        private static Vector[,] m_VertsPool;
        public static Vector vLight = new Vector(100f, -30f, 23f);

        static MapLighting()
        {
            vLight = vLight.Normalize();
        }

        public static void CalcVert(int x, int y)
        {
            m_CalcVert_Verts[x, y].m_X = (x - y) * 0x16;
            m_CalcVert_Verts[x, y].m_Y = (x + y) * 0x16;
            m_CalcVert_Verts[x, y].m_Z = m_CalcVert_Tiles[x, y].m_Z << 2;
        }

        public static int CF28(float f)
        {
            int num = (int) (255f * f);
            if (num > 0xff)
            {
                return 0xff;
            }
            if (num < 0x80)
            {
                num = 0x80;
            }
            return num;
        }

        public static void CheckStretchTable()
        {
            if (m_AlwaysStretch == null)
            {
                m_AlwaysStretch = new bool[0x4000];
                SetAlwaysStretch(3, 0xa7);
                SetAlwaysStretch(0xac, 0x1ab);
                SetAlwaysStretch(0x1b9, 0x1f3);
                SetAlwaysStretch(0x21f, 0x249);
                SetAlwaysStretch(0x25a, 0x405);
                SetAlwaysStretch(0x446, 0x479);
                SetAlwaysStretch(0x501, 0x510);
                SetAlwaysStretch(0x547, 0x9eb);
            }
        }

        public static void GetColorMap(ref MapPackage map)
        {
            int cellWidth = Renderer.cellWidth;
            int cellHeight = Renderer.cellHeight;
            Vector[,,] normalPool = m_NormalPool;
            ArrayList[,] cells = map.cells;
            byte[,] flags = map.flags;
            byte num3 = 1;
            Vector[,] vertsPool = m_VertsPool;
            bool[,] calcPool = m_CalcPool;
            CheckStretchTable();
            if (normalPool == null)
            {
                normalPool = m_NormalPool = new Vector[cellWidth, cellHeight, 2];
            }
            if (vertsPool == null)
            {
                vertsPool = m_VertsPool = new Vector[cellWidth, cellHeight];
            }
            if (calcPool == null)
            {
                calcPool = m_CalcPool = new bool[cellWidth, cellHeight];
            }
            else
            {
                Array.Clear(calcPool, 0, cellWidth * cellHeight);
            }
            m_CalcVert_Verts = vertsPool;
            m_CalcVert_Tiles = map.landTiles;
            CalcVert(0, 0);
            calcPool[0, 0] = true;
            for (int i = 0; i < (cellWidth - 1); i++)
            {
                for (int k = 0; k < (cellHeight - 1); k++)
                {
                    if (!calcPool[i + 1, k])
                    {
                        CalcVert(i + 1, k);
                        calcPool[i + 1, k] = true;
                    }
                    CalcVert(i + 1, k + 1);
                    calcPool[i + 1, k + 1] = true;
                    if (!calcPool[i, k + 1])
                    {
                        CalcVert(i, k + 1);
                        calcPool[i, k + 1] = true;
                    }
                    SurfaceNormal(normalPool, i, k, 0, *(vertsPool[i, k]), *(vertsPool[i + 1, k]), *(vertsPool[i, k + 1]));
                    SurfaceNormal(normalPool, i, k, 1, *(vertsPool[i, k + 1]), *(vertsPool[i + 1, k]), *(vertsPool[i + 1, k + 1]));
                }
            }
            int[,] colorMap = m_ColorMap;
            if (colorMap == null)
            {
                colorMap = m_ColorMap = new int[cellWidth, cellHeight];
            }
            int[,] numArray2 = m_ColorMap2;
            if (numArray2 == null)
            {
                numArray2 = m_ColorMap2 = new int[cellWidth, cellHeight];
            }
            for (int j = 1; j < (cellWidth - 1); j++)
            {
                byte[,] buffer2;
                IntPtr ptr;
                IntPtr ptr2;
                int[,] numArray3;
                for (int m = 1; m < (cellHeight - 1); m++)
                {
                    Vector vector = *(normalPool[j, m, 0]);
                    Vector vector2 = *(normalPool[j - 1, m - 1, 1]);
                    Vector vector3 = *(normalPool[j - 1, m, 0]);
                    Vector vector4 = *(normalPool[j - 1, m, 1]);
                    Vector vector5 = *(normalPool[j, m - 1, 0]);
                    Vector vector6 = *(normalPool[j, m - 1, 1]);
                    float num8 = ((((vector.m_X + vector2.m_X) + vector3.m_X) + vector4.m_X) + vector5.m_X) + vector6.m_X;
                    float num9 = ((((vector.m_Y + vector2.m_Y) + vector3.m_Y) + vector4.m_Y) + vector5.m_Y) + vector6.m_Y;
                    float num10 = ((((vector.m_Z + vector2.m_Z) + vector3.m_Z) + vector4.m_Z) + vector5.m_Z) + vector6.m_Z;
                    num8 *= 0.1666667f;
                    num9 *= 0.1666667f;
                    num10 *= 0.1666667f;
                    float num11 = (float) (1.0 / Math.Sqrt((double) (((num8 * num8) + (num9 * num9)) + (num10 * num10))));
                    num8 *= num11;
                    num9 *= num11;
                    num10 *= num11;
                    float num12 = ((num8 * vLight.m_X) + (num9 * vLight.m_Y)) + (num10 * vLight.m_Z);
                    num12 += 0.2151413f;
                    num12 += 0.8235294f;
                    int num13 = (int) ((255f * num12) + 0.5f);
                    if (num13 < 0x80)
                    {
                        num13 = 0x80;
                    }
                    else if (num13 > 0xff)
                    {
                        num13 = 0xff;
                    }
                    numArray2[j, m] = colorMap[j, m] = 0x10101 * num13;
                    if (((!m_AlwaysStretch[m_CalcVert_Tiles[j - 1, m - 1].m_ID & 0x3fff] && (colorMap[j, m] == 0xd2d2d2)) && ((colorMap[j - 1, m] == 0xd2d2d2) && (colorMap[j - 1, m - 1] == 0xd2d2d2))) && (colorMap[j, m - 1] == 0xd2d2d2))
                    {
                        int z = m_CalcVert_Tiles[j - 1, m - 1].m_Z;
                        int num15 = m_CalcVert_Tiles[j, m - 1].m_Z;
                        int num16 = m_CalcVert_Tiles[j, m].m_Z;
                        int num17 = m_CalcVert_Tiles[j - 1, m].m_Z;
                        if (((z == num15) && (z == num16)) && (z == num17))
                        {
                            (buffer2 = flags)[(int) (ptr = (IntPtr) (j - 1)), (int) (ptr2 = (IntPtr) (m - 1))] = (byte) (buffer2[(int) ptr, (int) ptr2] & ~num3);
                        }
                        else
                        {
                            (buffer2 = flags)[(int) (ptr = (IntPtr) (j - 1)), (int) (ptr2 = (IntPtr) (m - 1))] = (byte) (buffer2[(int) ptr, (int) ptr2] | num3);
                        }
                    }
                    else
                    {
                        (buffer2 = flags)[(int) (ptr = (IntPtr) (j - 1)), (int) (ptr2 = (IntPtr) (m - 1))] = (byte) (buffer2[(int) ptr, (int) ptr2] | num3);
                    }
                }
                (buffer2 = flags)[(int) (ptr = (IntPtr) j), 0] = (byte) (buffer2[(int) ptr, 0] | num3);
                (buffer2 = flags)[(int) (ptr = (IntPtr) j), (int) (ptr2 = (IntPtr) (cellHeight - 2))] = (byte) (buffer2[(int) ptr, (int) ptr2] | num3);
                (buffer2 = flags)[0, (int) (ptr = (IntPtr) j)] = (byte) (buffer2[0, (int) ptr] | num3);
                (buffer2 = flags)[(int) (ptr = (IntPtr) (cellWidth - 2)), (int) (ptr2 = (IntPtr) j)] = (byte) (buffer2[(int) ptr, (int) ptr2] | num3);
                numArray2[j, 0] = colorMap[j, 0] = 0;
                numArray2[j, cellHeight - 1] = colorMap[j, cellHeight - 1] = 0;
                numArray2[0, j] = (numArray3 = colorMap)[0, (int) (ptr = (IntPtr) j)] = numArray3[0, (int) ptr] | num3;
                numArray2[cellWidth - 1, j] = colorMap[cellWidth - 1, j] = 0;
            }
            map.colorMap = colorMap;
            map.realColors = colorMap;
            map.frameColors = numArray2;
            map.flags = flags;
        }

        private static void SetAlwaysStretch(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                if (Map.GetTexture(i) > 1)
                {
                    m_AlwaysStretch[i] = true;
                }
            }
        }

        public static void SurfaceNormal(Vector[,,] vNormal, int x, int y, int z, Vector v1, Vector v2, Vector v3)
        {
            float num = v1.m_X - v2.m_X;
            float num2 = v1.m_Y - v2.m_Y;
            float num3 = v1.m_Z - v2.m_Z;
            float num4 = v3.m_X - v1.m_X;
            float num5 = v3.m_Y - v1.m_Y;
            float num6 = v3.m_Z - v1.m_Z;
            float num7 = (num2 * num6) - (num3 * num5);
            float num8 = (num3 * num4) - (num * num6);
            float num9 = (num * num5) - (num2 * num4);
            float num10 = (float) (1.0 / Math.Sqrt((double) (((num7 * num7) + (num8 * num8)) + (num9 * num9))));
            vNormal[x, y, z].m_X = num7 * num10;
            vNormal[x, y, z].m_Y = num8 * num10;
            vNormal[x, y, z].m_Z = num9 * num10;
        }
    }
}

