namespace Client
{
    using System;
    using System.Collections;
    using System.IO;

    public class Multis
    {
        private ArrayList[] m_Cache = new ArrayList[0x2000];
        private ArrayList m_Items = new ArrayList();
        public int m_Level = 1;
        private static int[] m_RouteMap;
        private static int[,] m_RouteTable = new int[,] { 
            { 0x151, 0x14b }, { 0x127, 0x117 }, { 0x12b, 0x117 }, { 0x132, 0x117 }, { 0x135, 0x117 }, { 0x14c, 0x117 }, { 0x14f, 0x117 }, { 0x85c, 20 }, { 0x862, 20 }, { 0x85e, 0x15 }, { 0x861, 0x15 }, { 0x864, 0x15 }, { 0x878, 0x15 }, { 0x88b, 0x15 }, { 0x85d, 0x16 }, { 0x860, 0x16 }, 
            { 0x863, 0x16 }, { 0x877, 0x16 }, { 0x88a, 0x16 }, { 0x1ff, 0x117 }, { 0x202, 0x11a }, { 0x200, 0x20b }, { 0x203, 0x20b }, { 0x205, 0x20b }, { 0x207, 0x20b }, { 0x209, 0x20b }, { 0x201, 0x20c }, { 0x204, 0x20c }, { 0x206, 0x20c }, { 520, 0x20c }, { 0x20a, 0x20c }, { 10, 0x16 }, 
            { 7, 0x16 }, { 12, 0x16 }, { 6, 20 }, { 13, 0x15 }, { 8, 0x15 }, { 11, 0x15 }, { 9, 0x17 }, { 14, 0x16 }, { 15, 0x15 }, { 0xab, 0xbf }, { 0xa8, 0xbf }, { 0xad, 0xbf }, { 0xa6, 0xbd }, { 0xac, 190 }, { 0xa7, 190 }, { 170, 190 }, 
            { 0xa9, 0xc0 }, { 0xba, 0xbf }, { 0xb9, 190 }, { 0xae, 0xbd }, { 0xaf, 0xbf }, { 0xb0, 190 }, { 0xb1, 0xc0 }, { 0xb2, 0xbf }, { 0xb3, 190 }, { 180, 190 }, { 0xb5, 0xbf }, { 0xbb, 190 }, { 0xbc, 0xbf }, { 30, 0x25 }, { 0x1c, 0x25 }, { 0x21, 0x25 }, 
            { 0x1a, 0x24 }, { 0x20, 0x26 }, { 0x1b, 0x26 }, { 0x1f, 0x26 }, { 0x1d, 0x27 }, { 0x22, 0x25 }, { 0x23, 0x26 }, { 0x1d0, 0x1e9 }, { 0x1cf, 0x1e8 }, { 0x1d1, 490 }, { 0x1d2, 0x1eb }, { 0x1d3, 0x1e9 }, { 0x1d4, 490 }, { 200, 0xde }, { 0xc7, 220 }, { 0xc9, 0xdd }, 
            { 0xcc, 0xdf }, { 0xca, 0xde }, { 0xcb, 0xdd }, { 0x37, 0x42 }, { 0x34, 0x42 }, { 0x39, 0x42 }, { 0x33, 0x41 }, { 0x3a, 0x43 }, { 0x35, 0x43 }, { 0x38, 0x43 }, { 0x36, 0x44 }, { 0x3b, 0x42 }, { 60, 0x43 }, { 0x58, 0x63 }, { 0x59, 0x65 }, { 0x57, 100 }, 
            { 90, 0x66 }, { 0x5c, 0x63 }, { 0x5e, 0x63 }, { 0x5d, 100 }, { 0x5b, 100 }, { 0x1bc, 0x1c8 }, { 440, 0x1c8 }, { 0x1b7, 0x1c9 }, { 0x1bd, 0x1c9 }, { 0x1be, 0x1c8 }, { 450, 0x1c8 }, { 0x1c0, 0x1c8 }, { 0x1c1, 0x1c9 }, { 0x1c3, 0x1c9 }, { 0x1bf, 0x1c9 }, { 0x1ab, 0x215b }, 
            { 0x1a6, 0x215b }, { 0x1a7, 0x215b }, { 0x1a5, 0x215c }, { 0x1aa, 0x215a }, { 0x1a9, 0x215a }, { 0x1ac, 0x215a }, { 0x95, 0xa2 }, { 0x92, 0xa2 }, { 0x97, 0xa2 }, { 0x90, 0x9a }, { 150, 0xa3 }, { 0x91, 0xa3 }, { 0x94, 0xa3 }, { 0x93, 0x9d }, { 0x98, 0xa2 }, { 0x99, 0xa3 }, 
            { 0x228, 0x16 }, { 550, 20 }, { 0x227, 0x15 }, { 0x229, 0x17 }, { 0xf9, 0x10f }, { 0xf8, 270 }, { 250, 0x110 }, { 0xfb, 0x111 }, { 0xfc, 0x10f }, { 0xfd, 0x110 }, { 0xff, 0x10f }, { 0xfe, 270 }, { 0x100, 0x110 }, { 0x101, 0x111 }, { 0x102, 0x10f }, { 0x103, 0x110 }, 
            { 0x105, 0x10f }, { 260, 270 }, { 0x106, 0x110 }, { 0x107, 0x111 }, { 0x108, 0x10f }, { 0x109, 0x110 }, { 670, 0x2ba }, { 0x29d, 0x2b9 }, { 0x29f, 0x2bb }, { 0x2a0, 700 }, { 0x2ae, 0x2ba }, { 0x2ad, 0x2bb }, { 0x298, 0x2ba }, { 0x297, 0x2b9 }, { 0x299, 0x2bb }, { 0x29a, 700 }, 
            { 0x29b, 0x2ba }, { 0x29c, 0x2bb }, { 0x292, 0x2ba }, { 0x291, 0x2b9 }, { 0x293, 0x2bb }, { 660, 700 }, { 0x295, 0x2ba }, { 0x296, 0x2bb }, { 0x121, 0x110 }, { 290, 0x110 }, { 0x123, 0x10f }, { 0x124, 0x10f }, { 0x159, 0x165 }, { 0x158, 0x164 }, { 0x15a, 0x166 }, { 0x15b, 0x167 }, 
            { 0x15c, 0x165 }, { 0x15d, 0x166 }, { 0x160, 0x165 }, { 350, 0x164 }, { 0x15f, 0x166 }, { 0x161, 0x167 }, { 0x163, 0x165 }, { 0x162, 0x166 }, { 0x256, 0x1b1 }, { 0x255, 0x164 }, { 0x257, 0x1b2 }, { 0x250, 0x1b1 }, { 0x24f, 0x164 }, { 0x251, 0x1b2 }, { 0x24d, 0x1b1 }, { 0x24c, 0x164 }, 
            { 590, 0x1b2 }, { 0x3c8, 0x3ba }, { 0x3c7, 0x3b8 }, { 0x3c9, 0x3b7 }, { 970, 0x3bc }, { 990, 0x3ba }, { 0x3df, 0x3b7 }, { 0x3cc, 0x3ba }, { 0x3cb, 0x3b8 }, { 0x3cd, 0x3b7 }, { 0x3ce, 0x3bc }, { 0x3d7, 0x3ba }, { 980, 0x3ba }, { 0x3e1, 0x3ba }, { 0x3d3, 0x3b8 }, { 0x3e2, 100 }, 
            { 0x3d5, 0x3b7 }, { 0x3d8, 0x3b7 }, { 0x3d6, 0x3bc }, { 0x3e0, 0x3b8 }, { 0x138, 330 }, { 310, 330 }, { 0x133, 330 }, { 0x134, 0x14b }, { 0x137, 0x14b }, { 0x139, 0x14b }, { 0x12a, 0x17 }, { 0x13a, 330 }, { 0x13b, 0x14b }, { 310, 330 }, { 0x12e, 330 }, { 0x128, 330 }, 
            { 0x129, 0x14b }, { 0x137, 0x14b }, { 0x12f, 0x14b }, { 0x130, 330 }, { 310, 330 }, { 300, 330 }, { 0x12d, 0x14b }, { 0x137, 0x14b }, { 0x131, 0x14b }, { 0x150, 330 }, { 0x14e, 0x14b }, { 0x156, 330 }, { 0x157, 0x14b }, { 0x152, 330 }, { 0x153, 0x14b }, { 340, 330 }, 
            { 0x155, 0x14b }, { 910, 330 }, { 0x38f, 0x14b }, { 0x382, 0x17 }, { 0x390, 330 }, { 0x393, 330 }, { 0x38b, 330 }, { 0x38c, 0x14b }, { 0x394, 0x14b }, { 0x391, 0x14b }, { 0x386, 330 }, { 0x393, 330 }, { 0x380, 330 }, { 0x381, 0x14b }, { 0x394, 0x14b }, { 0x387, 0x14b }, 
            { 0x388, 330 }, { 0x393, 330 }, { 900, 330 }, { 0x385, 0x14b }, { 0x394, 0x14b }, { 0x389, 0x14b }, { 0x2c, 0x25 }, { 0x29, 0x25 }, { 40, 0x24 }, { 0x2a, 0x26 }, { 0x2b, 0x26 }, { 470, 0x1e9 }, { 0x1d7, 0x1e9 }, { 0x1d5, 0x1e8 }, { 0x1d9, 490 }, { 0x1d8, 490 }, 
            { 0x1dc, 0x1e9 }, { 0x1dd, 0x1e9 }, { 0x1db, 0x1e8 }, { 0x1df, 490 }, { 0x1de, 490 }, { 0x1e6, 490 }, { 0x1e7, 0x1e9 }, { 0xd1, 0xde }, { 0xcf, 0xde }, { 0xcd, 220 }, { 0xce, 0xdd }, { 0xd0, 0xdd }, { 0xda, 0xde }, { 0xd8, 0xde }, { 0xd4, 220 }, { 0xd7, 0xdd }, 
            { 0xd9, 0xdd }, { 0xd3, 0xdd }, { 0xe4, 0xde }, { 0xdb, 0xdf }, { 0xe5, 0xdd }, { 210, 0xde }, { 0x47, 0x42 }, { 0x48, 0x42 }, { 0x45, 0x41 }, { 70, 0x43 }, { 0x49, 0x43 }, { 0x6f, 0x63 }, { 0x70, 0x63 }, { 0x6d, 0x65 }, { 110, 100 }, { 0x71, 100 }, 
            { 0x76, 0x65 }, { 0x74, 0x65 }, { 0x73, 0x65 }, { 0x72, 0x65 }, { 0x75, 0x65 }, { 0x77, 0x65 }, { 0x281, 0x277 }, { 0x282, 0x27c }, { 0x43a, 0x443 }, { 0x439, 0x443 }, { 0x438, 0x442 }, { 0x43b, 0x444 }, { 0x43c, 0x444 }, { 0x43f, 0x443 }, { 0x43e, 0x443 }, { 0x43d, 0x442 }, 
            { 0x440, 0x444 }, { 0x441, 0x444 }, { 0x114, 0x10f }, { 0x113, 0x10f }, { 0x112, 270 }, { 0x115, 0x110 }, { 0x116, 0x110 }, { 0x11c, 270 }, { 0x11b, 270 }, { 0x11d, 270 }, { 0x11f, 270 }, { 0x11e, 270 }, { 0x120, 270 }, { 0x448, 0x451 }, { 0x447, 0x451 }, { 0x446, 0x450 }, 
            { 0x449, 0x452 }, { 0x44a, 0x452 }, { 0x44d, 0x451 }, { 0x44c, 0x451 }, { 0x44b, 0x450 }, { 0x44e, 0x452 }, { 0x44f, 0x452 }, { 690, 0x2ba }, { 0x2b1, 0x2ba }, { 0x2b0, 0x2b9 }, { 0x2b3, 0x2bb }, { 0x2b4, 0x2bb }, { 680, 0x2b9 }, { 0x2a7, 0x2b9 }, { 0x2a9, 0x2b9 }, { 0x2ab, 0x2b9 }, 
            { 0x2aa, 0x2b9 }, { 0x2ac, 0x2b9 }, { 0x2bd, 0x2b9 }, { 0x2be, 0x2b9 }, { 0x2c0, 0x2b9 }, { 0x2bf, 0x2b9 }, { 0x170, 0x165 }, { 370, 0x165 }, { 0x16d, 0x165 }, { 0x16c, 0x164 }, { 0x16e, 0x166 }, { 0x171, 0x166 }, { 0x16f, 0x166 }, { 0x18b, 0x166 }, { 0x18a, 0x166 }, { 0x18c, 0x165 }, 
            { 0x18d, 0x165 }, { 0x18e, 0x165 }, { 0x18f, 0x166 }, { 0x193, 0x165 }, { 0x192, 0x165 }, { 400, 0x166 }, { 0x191, 0x166 }, { 0x175, 0x164 }, { 0x176, 0x164 }, { 0x177, 0x164 }, { 0x178, 0x164 }, { 380, 0x164 }, { 0x17a, 0x164 }, { 0x17b, 0x164 }, { 0x179, 0x164 }, { 0x17d, 0x164 }, 
            { 0x173, 0x164 }, { 0x17e, 0x164 }, { 0x17f, 0x164 }, { 0x180, 0x164 }, { 0x181, 0x164 }, { 390, 0x164 }, { 0x187, 0x164 }, { 0x188, 0x164 }, { 0x189, 0x164 }, { 0x195, 0x164 }, { 0x194, 0x164 }, { 0x196, 0x164 }, { 0x324, 0x63 }, { 0x326, 0x63 }, { 0x328, 0x63 }, { 810, 0x63 }, 
            { 0x32c, 100 }, { 0x32e, 100 }, { 0x330, 100 }, { 0x332, 100 }, { 820, 0x16 }, { 0x336, 0x16 }, { 0x338, 0x16 }, { 0x33a, 0x16 }, { 0x33c, 0x15 }, { 830, 0x15 }, { 0x340, 0x15 }, { 0x342, 0x15 }, { 0x344, 0x16 }, { 0x346, 0x16 }, { 840, 0x16 }, { 0x34a, 0x16 }, 
            { 0x34c, 0x15 }, { 0x34e, 0x15 }, { 0x350, 0x15 }, { 850, 0x15 }, { 0x354, 0x63 }, { 0x356, 0x63 }, { 0x358, 0x63 }, { 0x35a, 0x63 }, { 860, 100 }, { 0x35e, 100 }, { 0x360, 100 }, { 0x362, 100 }
         };

        public void Clear()
        {
            this.m_Items.Clear();
        }

        public void Dispose()
        {
            this.m_Cache = null;
            this.m_Items.Clear();
            this.m_Items = null;
        }

        public bool IsInMulti(int px, int py, int pz)
        {
            for (int i = 0; i < this.m_Items.Count; i++)
            {
                Item item = (Item) this.m_Items[i];
                if (item.InWorld && item.IsMulti)
                {
                    CustomMultiEntry customMulti = CustomMultiLoader.GetCustomMulti(item.Serial, item.Revision);
                    Multi m = null;
                    if (customMulti != null)
                    {
                        m = customMulti.Multi;
                    }
                    if (m == null)
                    {
                        m = item.Multi;
                    }
                    if ((m != null) && this.IsInMulti(item, m, px, py, pz))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsInMulti(Item item, Multi m, int px, int py, int pz)
        {
            int num;
            int num2;
            int num3;
            int num4;
            if ((((item.X >= 0x1400) && (item.X < 0x1800)) && ((item.Y >= 0x900) && (item.Y < 0x1000))) && ((Engine.m_World == 0) || (Engine.m_World == 1)))
            {
                return true;
            }
            m.GetBounds(out num, out num2, out num3, out num4);
            int num5 = px - item.X;
            int num6 = py - item.Y;
            if (((num5 >= num) && (num5 <= num3)) && ((num6 >= num2) && (num6 <= num4)))
            {
                if ((item.Multi != m) && (num6 < num4))
                {
                    return true;
                }
                int index = num5 - num;
                int num8 = num6 - num2;
                if (((m != item.Multi) && (index == 0)) && (num6 == num4))
                {
                    return true;
                }
                if (m.Inside == null)
                {
                    m.UpdateRadar();
                }
                int num9 = m.Inside[num8][index] + item.Z;
                if ((pz == num9) || ((pz + 0x10) > num9))
                {
                    return true;
                }
            }
            return false;
        }

        public ArrayList Load(int multiID)
        {
            multiID &= 0x1fff;
            ArrayList list = this.m_Cache[multiID];
            if (list == null)
            {
                list = this.m_Cache[multiID] = this.ReadFromDisk(multiID);
            }
            return list;
        }

        private unsafe ArrayList ReadFromDisk(int multiID)
        {
            BinaryReader reader = new BinaryReader(Engine.FileManager.OpenMUL(Files.MultiIdx));
            reader.BaseStream.Seek((long) (multiID * 12), SeekOrigin.Begin);
            int num = reader.ReadInt32();
            int num2 = reader.ReadInt32();
            reader.Close();
            if (num == -1)
            {
                return new ArrayList();
            }
            Stream stream = Engine.FileManager.OpenMUL(Files.MultiMul);
            stream.Seek((long) num, SeekOrigin.Begin);
            byte[] buffer = new byte[num2];
            Engine.NativeRead((FileStream) stream, buffer, 0, buffer.Length);
            stream.Close();
            int num3 = num2 / 12;
            MultiItem[] c = new MultiItem[num3];
            fixed (byte* numRef = buffer)
            {
                MultiItem* itemPtr = (MultiItem*) numRef;
                MultiItem* itemPtr2 = itemPtr + num3;
                fixed (MultiItem* itemRef = c)
                {
                    MultiItem* itemPtr3 = itemRef;
                    while (itemPtr < itemPtr2)
                    {
                        itemPtr->ItemID = (short) (itemPtr->ItemID & 0x3fff);
                        itemPtr->ItemID = (short) (itemPtr->ItemID + 0x4000);
                        itemPtr3++;
                        itemPtr++;
                        itemPtr3[0] = itemPtr[0];
                    }
                }
            }
            return new ArrayList(c);
        }

        public void Register(Item i, int id)
        {
            if ((i.Multi == null) || (i.Multi.MultiID != id))
            {
                Multi multi = new Multi(id);
                i.Multi = multi;
                Map.Invalidate();
                GRadar.Invalidate();
            }
            if (!this.m_Items.Contains(i))
            {
                this.m_Items.Add(i);
                this.m_Items.Sort(MultiComparer.Instance);
                Map.Invalidate();
                GRadar.Invalidate();
            }
        }

        public bool RunUO_IsInside(Item item, Multi m, int px, int py, int pz)
        {
            int num;
            int num2;
            int num3;
            int num4;
            m.GetBounds(out num, out num2, out num3, out num4);
            int num5 = px - item.X;
            int num6 = py - item.Y;
            if (((num5 >= num) && (num5 <= num3)) && ((num6 >= num2) && (num6 <= num4)))
            {
                if ((item.Multi != m) && (num6 < num4))
                {
                    return true;
                }
                int index = num5 - num;
                int num8 = num6 - num2;
                if (m.RunUO_Inside == null)
                {
                    m.UpdateRadar();
                }
                int num9 = m.RunUO_Inside[num8][index] + item.Z;
                if ((pz == num9) || ((pz + 0x10) > num9))
                {
                    return true;
                }
            }
            return false;
        }

        public void Sort()
        {
            this.m_Items.Sort(MultiComparer.Instance);
        }

        public void Unregister(Item i)
        {
            this.m_Items.Remove(i);
            Map.Invalidate();
            GRadar.Invalidate();
        }

        public void Update(MapPackage map)
        {
            int count = this.m_Items.Count;
            if (count != 0)
            {
                int length = map.cells.GetLength(0);
                int num3 = map.cells.GetLength(1);
                int cellX = map.CellX;
                int cellY = map.CellY;
                int num6 = cellX + length;
                int num7 = cellY + num3;
                for (int i = 0; i < count; i++)
                {
                    Item item = (Item) this.m_Items[i];
                    if (item.InWorld && item.Visible)
                    {
                        CustomMultiEntry customMulti = CustomMultiLoader.GetCustomMulti(item.Serial, item.Revision);
                        Multi m = null;
                        if (customMulti != null)
                        {
                            m = customMulti.Multi;
                        }
                        if (m == null)
                        {
                            m = item.Multi;
                        }
                        if (m != null)
                        {
                            int num9;
                            int num10;
                            int num11;
                            int num12;
                            m.GetBounds(out num9, out num10, out num11, out num12);
                            num9 += item.X;
                            num10 += item.Y;
                            num11 += item.X;
                            num12 += item.Y;
                            if ((((num9 < num6) && (num11 >= cellX)) && (num10 < num7)) && (num12 >= cellY))
                            {
                                ArrayList list = m.List;
                                int num13 = list.Count;
                                bool flag = false;
                                Mobile player = World.Player;
                                if (player != null)
                                {
                                    flag = this.IsInMulti(item, m, player.X, player.Y, player.Z);
                                }
                                int num14 = -2147483648 | i;
                                for (int j = 0; j < num13; j++)
                                {
                                    MultiItem item2 = (MultiItem) list[j];
                                    if ((item2.Flags == 0) && (j != 0))
                                    {
                                        continue;
                                    }
                                    int num16 = item.X + item2.X;
                                    int num17 = item.Y + item2.Y;
                                    num16 -= cellX;
                                    num17 -= cellY;
                                    if ((((num16 < 0) || (num16 >= length)) || (num17 < 0)) || (num17 >= num3))
                                    {
                                        continue;
                                    }
                                    bool flag2 = true;
                                    int itemID = item2.ItemID;
                                    if (flag || (this.m_Level == 0))
                                    {
                                        goto Label_03C6;
                                    }
                                    int num19 = 7;
                                    if (customMulti == null)
                                    {
                                        switch ((m.MultiID & 0x3fff))
                                        {
                                            case 0x7a:
                                            case 0x7c:
                                            case 0x7e:
                                            case 0x98:
                                            case 0x9c:
                                                goto Label_0270;

                                            case 150:
                                            case 0xa2:
                                                goto Label_0266;

                                            case 0x9a:
                                                goto Label_0275;

                                            case 0x9e:
                                                num19 = 5;
                                                break;
                                        }
                                    }
                                    goto Label_027A;
                                Label_0266:
                                    num19 = 4;
                                    goto Label_027A;
                                Label_0270:
                                    num19 = 6;
                                    goto Label_027A;
                                Label_0275:
                                    num19 = 8;
                                Label_027A:
                                    if (Map.m_ItemFlags[itemID & 0x3fff][TileFlag.Bridge] || Map.m_ItemFlags[itemID & 0x3fff][TileFlag.Surface])
                                    {
                                        flag2 = item2.Z < (num19 + (this.m_Level * 20));
                                    }
                                    else
                                    {
                                        flag2 = item2.Z < (num19 + ((this.m_Level - 1) * 20));
                                        if ((item2.Z >= (num19 + ((this.m_Level - 1) * 20))) && (item2.Z <= ((num19 + ((this.m_Level - 1) * 20)) + 2)))
                                        {
                                            if (m_RouteMap == null)
                                            {
                                                m_RouteMap = new int[0x4000];
                                                for (int k = 0; k < m_RouteTable.GetLength(0); k++)
                                                {
                                                    m_RouteMap[m_RouteTable[k, 0]] = m_RouteTable[k, 1];
                                                }
                                            }
                                            int num21 = m_RouteMap[itemID & 0x3fff];
                                            if (num21 != 0)
                                            {
                                                itemID = num21;
                                                flag2 = true;
                                            }
                                        }
                                    }
                                    if (!flag2 && ((((itemID == 0x4001) || (itemID == 0x5796)) || ((itemID == 0x61a4) || (itemID == 0x6198))) || ((itemID == 0x61bc) || (itemID == 0x6199))))
                                    {
                                        flag2 = true;
                                    }
                                Label_03C6:
                                    if (flag2)
                                    {
                                        map.cells[num16, num17].Add(StaticItem.Instantiate((short) itemID, item2.ItemID, (sbyte) (item.Z + item2.Z), num14 | (j << 0x10)));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public ArrayList Items
        {
            get
            {
                return this.m_Items;
            }
        }

        private class MultiComparer : IComparer
        {
            public static readonly Multis.MultiComparer Instance = new Multis.MultiComparer();

            public int Compare(object x, object y)
            {
                Item item = (Item) x;
                Item item2 = (Item) y;
                if (item.Y > item2.Y)
                {
                    return 1;
                }
                if (item.Y < item2.Y)
                {
                    return -1;
                }
                if (item.X > item2.X)
                {
                    return 1;
                }
                if (item.X < item2.X)
                {
                    return -1;
                }
                return 0;
            }
        }
    }
}

