namespace Client
{
    using System;
    using System.Collections;
    using System.Windows.Forms;

    public class Walking
    {
        private static DateTime m_LastLiftBlocker;
        private const int PersonHeight = 0x10;
        private const int StepHeight = 2;

        public static bool Calculate(int x, int y, int z, Direction dir, out int newZ, out int newDir)
        {
            int walkDirection = Engine.GetWalkDirection(dir);
            newZ = z;
            newDir = walkDirection;
            if (!IsDiagonal(walkDirection))
            {
                int num4;
                int num5;
                int num6;
                int num2 = Turn(walkDirection, 1);
                int num3 = Turn(walkDirection, -1);
                bool flag = CheckMovement(x, y, z, num2, out num4);
                bool flag2 = CheckMovement(x, y, z, num3, out num5);
                bool flag3 = CheckMovement(x, y, z, walkDirection, out num6);
                Mobile player = World.Player;
                if (flag3 && ((player.Body == 0x3db) ? (flag || flag2) : (flag && flag2)))
                {
                    newZ = num6;
                }
                else if (flag)
                {
                    newZ = num4;
                    newDir = num2;
                }
                else if (flag2)
                {
                    newZ = num5;
                    newDir = num3;
                }
                else
                {
                    return false;
                }
                return true;
            }
            return CheckMovement(x, y, z, walkDirection, out newZ);
        }

        public static bool CheckMovement(int xStart, int yStart, int zStart, int dir, out int zNew)
        {
            int num8;
            int num9;
            Mobile player = World.Player;
            if (player == null)
            {
                zNew = 0;
                return false;
            }
            int x = xStart;
            int y = yStart;
            Offset(dir, ref x, ref y);
            MapPackage cache = Map.GetCache();
            int num3 = x - cache.CellX;
            int num4 = y - cache.CellY;
            if (!Map.IsValid(num3, num4))
            {
                zNew = 0;
                return false;
            }
            LandTile tile = cache.landTiles[num3, num4];
            ArrayList tiles = cache.cells[num3, num4];
            try
            {
                if (((player.Notoriety == Notoriety.Murderer) && tile.m_Guarded) && (!cache.landTiles[xStart - cache.CellX, yStart - cache.CellY].m_Guarded && ((Control.ModifierKeys & (Keys.Control | Keys.Shift)) != (Keys.Control | Keys.Shift))))
                {
                    zNew = 0;
                    return false;
                }
            }
            catch
            {
            }
            bool flag = Map.GetLandFlags(tile.m_ID)[TileFlag.Impassable];
            bool flag2 = ((tile.m_ID != 2) && (tile.m_ID != 0x1db)) && ((tile.m_ID < 430) || (tile.m_ID > 0x1b5));
            int z = 0;
            int avg = 0;
            int top = 0;
            Map.GetAverageZ(x, y, ref z, ref avg, ref top);
            GetStartZ(xStart, yStart, zStart, out num8, out num9);
            zNew = num8;
            bool flag3 = false;
            int num10 = num9 + 2;
            int num11 = num8 + 0x10;
            bool ignoreDoors = player.Ghost || (player.Body == 0x3db);
            bool ignoreMobs = (ignoreDoors || (player.StamCur == player.StamMax)) || (Engine.m_World != 0);
            if (Engine.m_Stealth)
            {
                ignoreMobs = false;
            }
            for (int i = 0; i < tiles.Count; i++)
            {
                ICell cell = (ICell)tiles[i];
                if (cell is StaticItem)
                {
                    StaticItem item = (StaticItem)cell;
                    TileFlags flags = Map.m_ItemFlags[item.m_RealID & 0x3fff];
                    if (flags[TileFlag.Surface] && !flags[TileFlag.Impassable])
                    {
                        int num13 = item.m_Z;
                        int num14 = num13;
                        int ourZ = num13 + item.CalcHeight;
                        int num16 = ourZ + 0x10;
                        int ourTop = num11;
                        if (flag3)
                        {
                            int num18 = Math.Abs((int)(ourZ - player.Z)) - Math.Abs((int)(zNew - player.Z));
                            if ((num18 > 0) || ((num18 == 0) && (ourZ > zNew)))
                            {
                                continue;
                            }
                        }
                        if ((ourZ + 0x10) > ourTop)
                        {
                            ourTop = ourZ + 0x10;
                        }
                        if (!flags[TileFlag.Bridge])
                        {
                            num14 += item.Height;
                        }
                        if (num10 >= num14)
                        {
                            int num19 = num13;
                            if (item.Height >= 2)
                            {
                                num19 += 2;
                            }
                            else
                            {
                                num19 += item.Height;
                            }
                            if (((!flag2 || (num19 >= avg)) || ((avg <= ourZ) || (ourTop <= z))) && IsOk(ignoreMobs, ignoreDoors, ourZ, ourTop, tiles))
                            {
                                zNew = ourZ;
                                flag3 = true;
                            }
                        }
                    }
                }
                else if (cell is DynamicItem)
                {
                    Item item2 = ((DynamicItem)cell).m_Item;
                    TileFlags flags2 = Map.m_ItemFlags[item2.ID & 0x3fff];
                    if (flags2[TileFlag.Surface] && !flags2[TileFlag.Impassable])
                    {
                        int num20 = item2.Z;
                        int num21 = num20;
                        int num22 = num20;
                        int height = Map.GetHeight((item2.ID & 0x3fff) + 0x4000);
                        if (flags2[TileFlag.Bridge])
                        {
                            num22 += height / 2;
                        }
                        else
                        {
                            num22 += height;
                        }
                        if (flag3)
                        {
                            int num24 = Math.Abs((int)(num22 - player.Z)) - Math.Abs((int)(zNew - player.Z));
                            if ((num24 > 0) || ((num24 == 0) && (num22 > zNew)))
                            {
                                continue;
                            }
                        }
                        int num25 = num22 + 0x10;
                        int num26 = num11;
                        if ((num22 + 0x10) > num26)
                        {
                            num26 = num22 + 0x10;
                        }
                        if (!flags2[TileFlag.Bridge])
                        {
                            num21 += height;
                        }
                        if (num10 >= num21)
                        {
                            int num27 = num20;
                            if (height >= 2)
                            {
                                num27 += 2;
                            }
                            else
                            {
                                num27 += height;
                            }
                            if (((!flag2 || (num27 >= avg)) || ((avg <= num22) || (num26 <= z))) && IsOk(ignoreMobs, ignoreDoors, num22, num26, tiles))
                            {
                                zNew = num22;
                                flag3 = true;
                            }
                        }
                    }
                }
            }
            if ((flag2 && !flag) && (num10 >= z))
            {
                int num28 = avg;
                int num29 = num28 + 0x10;
                int num30 = num11;
                if ((num28 + 0x10) > num30)
                {
                    num30 = num28 + 0x10;
                }
                bool flag6 = true;
                if (flag3)
                {
                    int num31 = Math.Abs((int)(num28 - player.Z)) - Math.Abs((int)(zNew - player.Z));
                    if ((num31 > 0) || ((num31 == 0) && (num28 > zNew)))
                    {
                        flag6 = false;
                    }
                }
                if (flag6 && IsOk(ignoreMobs, ignoreDoors, num28, num30, tiles))
                {
                    zNew = num28;
                    flag3 = true;
                }
            }
            return flag3;
        }

        private static void GetStartZ(int xStart, int yStart, int zStart, out int zLow, out int zTop)
        {
            MapPackage cache = Map.GetCache();
            int x = xStart - cache.CellX;
            int y = yStart - cache.CellY;
            if (!Map.IsValid(x, y))
            {
                zLow = zStart;
                zTop = zStart;
            }
            else
            {
                int num12;
                LandTile tile = cache.landTiles[x, y];
                ArrayList list = cache.cells[x, y];
                bool flag = Map.GetLandFlags(tile.m_ID)[TileFlag.Impassable];
                bool flag2 = ((tile.m_ID != 2) && (tile.m_ID != 0x1db)) && ((tile.m_ID < 430) || (tile.m_ID > 0x1b5));
                int z = 0;
                int avg = 0;
                int top = 0;
                Map.GetAverageZ(xStart, yStart, ref z, ref avg, ref top);
                zTop = num12 = 0;
                int num6 = zLow = num12;
                bool flag3 = false;
                if (((flag2 && !flag) && (zStart >= avg)) && (!flag3 || (avg >= num6)))
                {
                    zLow = z;
                    num6 = avg;
                    if (!flag3 || (top > zTop))
                    {
                        zTop = top;
                    }
                    flag3 = true;
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ICell cell = (ICell)list[i];
                    if (cell is StaticItem)
                    {
                        StaticItem item = (StaticItem)cell;
                        TileFlags flags = Map.m_ItemFlags[item.m_RealID & 0x3fff];
                        int num8 = item.m_Z + item.CalcHeight;
                        if ((flags[TileFlag.Surface] && (zStart >= num8)) && (!flag3 || (num8 >= num6)))
                        {
                            num6 = num8;
                            int num9 = item.m_Z + item.m_Height;
                            if (!flag3 || (num9 > zTop))
                            {
                                zTop = num9;
                            }
                            zLow = item.m_Z;
                            flag3 = true;
                        }
                    }
                    else if (cell is DynamicItem)
                    {
                        Item item2 = ((DynamicItem)cell).m_Item;
                        TileFlags flags2 = Map.m_ItemFlags[item2.ID & 0x3fff];
                        int num10 = item2.Z;
                        if (flags2[TileFlag.Bridge])
                        {
                            num10 += Map.GetHeight((item2.ID & 0x3fff) + 0x4000) / 2;
                        }
                        else
                        {
                            num10 += Map.GetHeight((item2.ID & 0x3fff) + 0x4000);
                        }
                        if ((flags2[TileFlag.Surface] && (zStart >= num10)) && (!flag3 || (num10 >= num6)))
                        {
                            num6 = num10;
                            int num11 = item2.Z + Map.GetHeight((item2.ID & 0x3fff) + 0x4000);
                            if (!flag3 || (num11 > zTop))
                            {
                                zTop = num11;
                            }
                            zLow = item2.Z;
                            flag3 = true;
                        }
                    }
                }
                if (!flag3)
                {
                    zLow = zTop = zStart;
                }
                else if (zStart > zTop)
                {
                    zTop = zStart;
                }
            }
        }

        public static bool IsDiagonal(int dir)
        {
            return ((dir & 1) == 0);
        }

        private static bool IsOk(bool ignoreMobs, bool ignoreDoors, int ourZ, int ourTop, ArrayList tiles)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                ICell cell = (ICell)tiles[i];
                if (cell is StaticItem)
                {
                    StaticItem item = (StaticItem)cell;
                    TileFlags flags = Map.m_ItemFlags[item.m_RealID & 0x3fff];
                    if (flags[TileFlag.Surface | TileFlag.Impassable])
                    {
                        int z = item.m_Z;
                        int num3 = z + item.CalcHeight;
                        if ((num3 > ourZ) && (ourTop > z))
                        {
                            return false;
                        }
                    }
                }
                else if (cell is DynamicItem)
                {
                    Item item2 = ((DynamicItem)cell).m_Item;
                    TileFlags flags2 = Map.m_ItemFlags[item2.ID & 0x3fff];
                    if (flags2[TileFlag.Surface | TileFlag.Impassable] && !item2.IsDoor)
                    {
                        int num4 = item2.Z;
                        int num5 = num4;
                        if (flags2[TileFlag.Bridge])
                        {
                            num5 += Map.GetHeight((item2.ID & 0x3fff) + 0x4000) / 2;
                        }
                        else
                        {
                            num5 += Map.GetHeight((item2.ID & 0x3fff) + 0x4000);
                        }
                        if ((num5 > ourZ) && (ourTop > num4))
                        {
                            if ((!item2.Flags[ItemFlag.CanMove] && (Map.GetWeight(item2.ID) >= 0xff)) || (((Control.ModifierKeys & Keys.Shift) == Keys.None) || ((m_LastLiftBlocker + TimeSpan.FromSeconds(0.6)) >= DateTime.Now)))
                            {
                                return false;
                            }
                            m_LastLiftBlocker = DateTime.Now;
                            Network.Send(new PPickupItem(item2, item2.Amount));
                            Network.Send(new PDropItem(item2.Serial, -1, -1, 0, World.Serial));
                        }
                    }
                }
                else if (!ignoreMobs && (cell is MobileCell))
                {
                    Mobile mobile = ((MobileCell)cell).m_Mobile;
                    if (!mobile.Ghost && !mobile.Bonded)
                    {
                        int num6 = mobile.Z;
                        int num7 = num6 + 0x10;
                        if ((num7 > ourZ) && (ourTop > num6))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public static void Offset(int dir, ref int x, ref int y)
        {
            switch ((dir & 7))
            {
                case 0:
                    y--;
                    break;

                case 1:
                    x++;
                    y--;
                    break;

                case 2:
                    x++;
                    break;

                case 3:
                    x++;
                    y++;
                    break;

                case 4:
                    y++;
                    break;

                case 5:
                    x--;
                    y++;
                    break;

                case 6:
                    x--;
                    break;

                case 7:
                    x--;
                    y--;
                    break;
            }
        }

        public static int Turn(int dir, int offset)
        {
            return ((((dir & 7) + offset) & 7) | (dir & 0x80));
        }
    }
}