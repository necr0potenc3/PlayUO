namespace Client
{
    using System;
    using System.Collections;

    public class TileSorter : IComparer
    {
        private const int Background = 1;
        private const int Bridge = 0x400;
        private int c1Flags;
        private int c1Height;
        private int c1ItemID;
        private int c1SortInfluence;
        private int c1SortZ;
        private int c2Flags;
        private int c2Height;
        private int c2ItemID;
        private int c2SortInfluence;
        private int c2SortZ;
        public static readonly IComparer Comparer = new TileSorter();
        private const int Foliage = 0x20000;
        private IItem item1;
        private IItem item2;
        private const int Roof = 0x10000000;
        private static Type tDynamicItem = typeof(DynamicItem);
        private static Type tLandTile = typeof(LandTile);
        private static Type tMobileCell = typeof(MobileCell);
        private static Type tStaticItem = typeof(StaticItem);

        private TileSorter()
        {
        }

        public int Compare(object x, object y)
        {
            int num;
            int num2;
            int num3;
            int num4;
            int num5;
            int num6;
            int num7;
            int num8;
            this.GetStats(x, out num, out num2, out num3, out num4);
            this.GetStats(y, out num5, out num6, out num7, out num8);
            num += num2;
            num5 += num6;
            int num9 = num - num5;
            if (num9 == 0)
            {
                num9 = num3 - num7;
            }
            if (num9 == 0)
            {
                num9 = num2 - num6;
            }
            if (num9 == 0)
            {
                num9 = num4 - num8;
            }
            return num9;
        }

        public void GetStats(object obj, out int z, out int treshold, out int type, out int tiebreaker)
        {
            if (obj is MobileCell)
            {
                MobileCell cell = (MobileCell)obj;
                z = cell.Z;
                treshold = 2;
                type = 3;
                if (cell.m_Mobile.Player)
                {
                    tiebreaker = 0x40000000;
                }
                else
                {
                    tiebreaker = cell.Serial;
                }
            }
            else if (obj is LandTile)
            {
                LandTile tile = (LandTile)obj;
                z = tile.SortZ;
                treshold = 0;
                type = 0;
                tiebreaker = 0;
            }
            else if (obj is DynamicItem)
            {
                int num;
                DynamicItem item = (DynamicItem)obj;
                z = item.Z;
                if (Map.m_ItemFlags[item.ID & 0x3fff][TileFlag.Background])
                {
                    num = 0;
                }
                else
                {
                    num = 1;
                }
                treshold = (item.Height == 0) ? num : (num + 1);
                type = ((item.ID & 0x3fff) == 0x2006) ? 4 : 2;
                tiebreaker = item.Serial;
            }
            else if (obj is StaticItem)
            {
                int num2;
                StaticItem item2 = (StaticItem)obj;
                z = item2.Z;
                if (Map.m_ItemFlags[item2.ID & 0x3fff][TileFlag.Background])
                {
                    num2 = 0;
                }
                else
                {
                    num2 = 1;
                }
                treshold = (item2.Height == 0) ? num2 : (num2 + 1);
                type = 1;
                tiebreaker = item2.m_SortInfluence;
            }
            else
            {
                z = 0;
                treshold = 0;
                type = 0;
                tiebreaker = 0;
            }
        }

        public int OldCompare(object x, object y)
        {
            bool flag5;
            bool flag6;
            ICell cell = (ICell)x;
            ICell cell2 = (ICell)y;
            Type cellType = cell.CellType;
            Type type2 = cell2.CellType;
            bool flag = cellType == tLandTile;
            bool flag2 = type2 == tLandTile;
            bool flag3 = !flag && ((cellType == tDynamicItem) || (cellType == tStaticItem));
            bool flag4 = !flag2 && ((type2 == tDynamicItem) || (type2 == tStaticItem));
            if (flag3)
            {
                this.item1 = (IItem)cell;
                if (cellType == tStaticItem)
                {
                    StaticItem item = (StaticItem)cell;
                    this.c1ItemID = item.m_ID;
                    this.c1SortZ = item.m_Z;
                    this.c1Height = item.m_Height;
                    this.c1SortInfluence = item.m_SortInfluence;
                }
                else
                {
                    DynamicItem item2 = (DynamicItem)cell;
                    this.c1ItemID = item2.m_ID;
                    this.c1SortZ = item2.m_Z;
                    this.c1Height = item2.m_Height;
                }
                this.c1Flags = Map.m_ItemFlags[this.c1ItemID & 0x3fff].Value;
                flag5 = (this.c1ItemID & 0x3fff) == 0x2006;
                if ((this.c1Flags & 0x400) != 0)
                {
                    this.c1Height /= 2;
                }
            }
            else
            {
                this.c1SortZ = cell.SortZ;
                this.c1Height = cell.Height;
                flag5 = false;
            }
            if (flag4)
            {
                this.item2 = (IItem)cell2;
                if (type2 == tStaticItem)
                {
                    StaticItem item3 = (StaticItem)cell2;
                    this.c2ItemID = item3.m_ID;
                    this.c2SortZ = item3.m_Z;
                    this.c2Height = item3.m_Height;
                    this.c2SortInfluence = item3.m_SortInfluence;
                }
                else
                {
                    DynamicItem item4 = (DynamicItem)cell2;
                    this.c2ItemID = item4.m_ID;
                    this.c2SortZ = item4.m_Z;
                    this.c2Height = item4.m_Height;
                }
                this.c2Flags = Map.m_ItemFlags[this.c2ItemID & 0x3fff].Value;
                flag6 = (this.c2ItemID & 0x3fff) == 0x2006;
                if ((this.c2Flags & 0x400) != 0)
                {
                    this.c2Height /= 2;
                }
            }
            else
            {
                this.c2SortZ = cell2.SortZ;
                this.c2Height = cell2.Height;
                flag6 = false;
            }
            if (flag5 && !flag6)
            {
                return 1;
            }
            if (!flag5 && flag6)
            {
                return -1;
            }
            if (!flag5 || !flag6)
            {
                if ((flag3 && (this.c1ItemID != 0x4001)) && (((this.c1Flags & 0x10000000) != 0) && (this.c1SortZ > this.c2SortZ)))
                {
                    return 1;
                }
                if ((flag4 && (this.c2ItemID != 0x4001)) && (((this.c2Flags & 0x10000000) != 0) && (this.c2SortZ > this.c1SortZ)))
                {
                    return -1;
                }
                if ((flag3 && (this.c1ItemID != 0x4001)) && ((this.c1Flags & 0x20000) != 0))
                {
                    return 1;
                }
                if ((flag4 && (this.c2ItemID != 0x4001)) && ((this.c2Flags & 0x20000) != 0))
                {
                    return -1;
                }
                if ((!flag && flag2) && ((this.c1SortZ + this.c1Height) >= (this.c2SortZ + this.c2Height)))
                {
                    return 1;
                }
                if ((flag && !flag2) && ((this.c2SortZ + this.c2Height) <= (this.c1SortZ + this.c1Height)))
                {
                    return -1;
                }
                if ((this.c1SortZ + this.c1Height) > (this.c2SortZ + this.c2Height))
                {
                    return 1;
                }
                if ((this.c1SortZ + this.c1Height) < (this.c2SortZ + this.c2Height))
                {
                    return -1;
                }
                if (this.c1SortZ > this.c2SortZ)
                {
                    return 1;
                }
                if (this.c1SortZ < this.c2SortZ)
                {
                    return -1;
                }
                if (flag3 && flag4)
                {
                    bool flag7 = (this.c1Flags & 1) != 0;
                    bool flag8 = (this.c2Flags & 1) != 0;
                }
                if (flag && !flag2)
                {
                    return -1;
                }
                if (!flag && flag2)
                {
                    return 1;
                }
                bool flag9 = (!flag3 && !flag) && (cellType == tMobileCell);
                bool flag10 = (!flag4 && !flag2) && (cellType == tMobileCell);
                if (!flag9 && flag10)
                {
                    return -1;
                }
                if (flag9 && !flag10)
                {
                    return 1;
                }
                if (flag9 && flag10)
                {
                    Mobile mobile = ((MobileCell)cell).m_Mobile;
                    Mobile mobile2 = ((MobileCell)cell2).m_Mobile;
                    if ((mobile != null) && (mobile2 != null))
                    {
                        bool player = mobile.Player;
                        bool flag12 = mobile2.Player;
                        if (player && !flag12)
                        {
                            return 1;
                        }
                        if (flag12 && !player)
                        {
                            return -1;
                        }
                    }
                    return 0;
                }
                if ((cellType == tDynamicItem) && (type2 == tStaticItem))
                {
                    return 1;
                }
                if ((cellType == tStaticItem) && (type2 == tDynamicItem))
                {
                    return -1;
                }
                if ((cellType == tStaticItem) && (type2 == tStaticItem))
                {
                    if (this.c2SortInfluence < this.c1SortInfluence)
                    {
                        return -1;
                    }
                    if (this.c2SortInfluence > this.c1SortInfluence)
                    {
                        return 1;
                    }
                    return 0;
                }
                if ((cellType == tDynamicItem) && (type2 == tDynamicItem))
                {
                    Item item5 = ((DynamicItem)cell).m_Item;
                    Item item6 = ((DynamicItem)cell2).m_Item;
                    if (item5.Serial < item6.Serial)
                    {
                        return -1;
                    }
                    if (item5.Serial > item6.Serial)
                    {
                        return 1;
                    }
                    return 0;
                }
            }
            return 0;
        }
    }
}