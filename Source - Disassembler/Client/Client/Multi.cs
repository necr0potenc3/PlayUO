namespace Client
{
    using System.Collections;

    public class Multi
    {
        private sbyte[][] m_Inside;
        private ArrayList m_List;
        private int m_MultiID;
        private short[][] m_Radar;
        private sbyte[][] m_RunUO_Inside;
        private int m_xMax;
        private int m_xMin;
        private int m_yMax;
        private int m_yMin;

        public Multi(ArrayList list)
        {
            this.m_MultiID = 0;
            int count = list.Count;
            int num2 = 0;
            this.m_xMin = 0x3e8;
            this.m_yMin = 0x3e8;
            this.m_xMax = -1000;
            this.m_yMax = -1000;
            while (num2 < count)
            {
                MultiItem item = (MultiItem)list[num2];
                if (item.X < this.m_xMin)
                {
                    this.m_xMin = item.X;
                }
                if (item.Y < this.m_yMin)
                {
                    this.m_yMin = item.Y;
                }
                if (item.X > this.m_xMax)
                {
                    this.m_xMax = item.X;
                }
                if (item.Y > this.m_yMax)
                {
                    this.m_yMax = item.Y;
                }
                num2++;
            }
            this.m_List = list;
            this.UpdateRadar();
        }

        public Multi(int MultiID)
        {
            this.m_MultiID = MultiID;
            ArrayList list = Engine.Multis.Load(MultiID);
            int count = list.Count;
            int num2 = 0;
            this.m_xMin = 0x3e8;
            this.m_yMin = 0x3e8;
            this.m_xMax = -1000;
            this.m_yMax = -1000;
            while (num2 < count)
            {
                MultiItem item = (MultiItem)list[num2];
                if (item.X < this.m_xMin)
                {
                    this.m_xMin = item.X;
                }
                if (item.Y < this.m_yMin)
                {
                    this.m_yMin = item.Y;
                }
                if (item.X > this.m_xMax)
                {
                    this.m_xMax = item.X;
                }
                if (item.Y > this.m_yMax)
                {
                    this.m_yMax = item.Y;
                }
                num2++;
            }
            this.m_List = list;
            this.UpdateRadar();
        }

        public void Add(int itemID, int x, int y, int z)
        {
            if ((((x >= this.m_xMin) && (y >= this.m_yMin)) && (x <= this.m_xMax)) && (y <= this.m_yMax))
            {
                itemID &= 0x3fff;
                itemID |= 0x4000;
                for (int i = 0; i < this.m_List.Count; i++)
                {
                    MultiItem item = (MultiItem)this.m_List[i];
                    if (((item.X == x) && (item.Y == y)) && ((item.Z == z) && ((Map.GetHeight(item.ItemID) > 0) == (Map.GetHeight(itemID) > 0))))
                    {
                        this.m_List.RemoveAt(i--);
                    }
                }
                MultiItem item2 = new MultiItem();
                item2.Flags = 1;
                item2.ItemID = (short)itemID;
                item2.X = (short)x;
                item2.Y = (short)y;
                item2.Z = (short)z;
                this.m_List.Add(item2);
            }
        }

        public void GetBounds(out int xMin, out int yMin, out int xMax, out int yMax)
        {
            xMin = this.m_xMin;
            yMin = this.m_yMin;
            xMax = this.m_xMax;
            yMax = this.m_yMax;
        }

        public bool Remove(int x, int y, int z, int itemID)
        {
            if (((x < this.m_xMin) || (y < this.m_yMin)) || ((x > this.m_xMax) || (y > this.m_yMax)))
            {
                return false;
            }
            itemID &= 0x3fff;
            itemID |= 0x4000;
            bool flag = false;
            for (int i = 0; i < this.m_List.Count; i++)
            {
                MultiItem item = (MultiItem)this.m_List[i];
                if (((item.X == x) && (item.Y == y)) && ((item.Z == z) && (item.ItemID == itemID)))
                {
                    this.m_List.RemoveAt(i--);
                    flag = true;
                }
            }
            return flag;
        }

        public void UpdateRadar()
        {
            int num = (this.m_xMax - this.m_xMin) + 1;
            int num2 = (this.m_yMax - this.m_yMin) + 1;
            if ((num > 0) && (num2 > 0))
            {
                int[][] numArray = new int[num2][];
                int[][] numArray2 = new int[num2][];
                this.m_Inside = new sbyte[num2][];
                this.m_RunUO_Inside = new sbyte[num2][];
                this.m_Radar = new short[num2][];
                for (int i = 0; i < num2; i++)
                {
                    this.m_Radar[i] = new short[num];
                    this.m_Inside[i] = new sbyte[num];
                    this.m_RunUO_Inside[i] = new sbyte[num];
                    numArray[i] = new int[num];
                    numArray2[i] = new int[num];
                    for (int k = 0; k < num; k++)
                    {
                        numArray[i][k] = -2147483648;
                    }
                    for (int m = 0; m < num; m++)
                    {
                        numArray2[i][m] = -2147483648;
                    }
                    for (int n = 0; n < num; n++)
                    {
                        this.m_Inside[i][n] = 0x7f;
                    }
                    for (int num7 = 0; num7 < num; num7++)
                    {
                        this.m_RunUO_Inside[i][num7] = 0x7f;
                    }
                }
                for (int j = 0; j < this.m_List.Count; j++)
                {
                    MultiItem item = (MultiItem)this.m_List[j];
                    int index = item.X - this.m_xMin;
                    int num10 = item.Y - this.m_yMin;
                    if (((index >= 0) && (index < num)) && ((num10 >= 0) && (num10 < num2)))
                    {
                        int z = item.Z;
                        int num12 = z + Map.GetHeight(item.ItemID);
                        int num13 = numArray2[num10][index];
                        int num14 = numArray[num10][index];
                        int num15 = item.ItemID;
                        if (((num12 > num14) || ((num12 == num14) && (z > num13))) && ((((num15 != 0x4001) && (num15 != 0x5796)) && ((num15 != 0x61a4) && (num15 != 0x6198))) && ((num15 != 0x61bc) && (num15 != 0x6199))))
                        {
                            this.m_Radar[num10][index] = item.ItemID;
                            numArray2[num10][index] = z;
                            numArray[num10][index] = num12;
                        }
                        if (!Map.GetTileFlags(item.ItemID)[TileFlag.Roof])
                        {
                            num15 = item.ItemID & 0x3fff;
                            sbyte num16 = (sbyte)item.Z;
                            if (num16 < this.m_Inside[num10][index])
                            {
                                this.m_Inside[num10][index] = num16;
                            }
                            if ((((num15 < 0xb95) || (num15 > 0xc0e)) && ((num15 < 0xc43) || (num15 > 0xc44))) && (num16 < this.m_RunUO_Inside[num10][index]))
                            {
                                this.m_RunUO_Inside[num10][index] = num16;
                            }
                        }
                    }
                }
            }
        }

        public sbyte[][] Inside
        {
            get
            {
                return this.m_Inside;
            }
        }

        public ArrayList List
        {
            get
            {
                return this.m_List;
            }
        }

        public int MultiID
        {
            get
            {
                return this.m_MultiID;
            }
        }

        public short[][] Radar
        {
            get
            {
                return this.m_Radar;
            }
        }

        public sbyte[][] RunUO_Inside
        {
            get
            {
                return this.m_RunUO_Inside;
            }
        }
    }
}