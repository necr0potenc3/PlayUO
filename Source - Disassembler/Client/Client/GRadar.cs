namespace Client
{
    using System.Collections;
    using System.IO;
    using System.Windows.Forms;

    public class GRadar : Gump, IResizable
    {
        private const short BLACK = -32768;
        private static short[] m_Colors;
        public static Mobile m_FocusMob;
        private static BitArray m_Guarded;
        protected int m_Height;
        private static Texture m_Image;
        private static bool m_Open;
        private static MapBlock[] m_StrongReferences;
        private static Texture m_Swap;
        private static ArrayList m_Tags = new ArrayList();
        private static bool m_ToClose;
        private static VertexCache m_vCache = new VertexCache();
        protected int m_Width;
        private static int m_World;
        private static int m_xBlock;
        private static int m_xWidth;
        private static int m_yBlock;
        private static int m_yHeight;

        public GRadar() : base(0x19, 0x19)
        {
            m_Open = true;
            this.m_Width = 260;
            this.m_Height = 260;
            base.m_Children.Add(new GVResizer(this));
            base.m_Children.Add(new GHResizer(this));
            base.m_Children.Add(new GLResizer(this));
            base.m_Children.Add(new GTResizer(this));
            base.m_Children.Add(new GHVResizer(this));
            base.m_Children.Add(new GLTResizer(this));
            base.m_Children.Add(new GHTResizer(this));
            base.m_Children.Add(new GLVResizer(this));
            base.m_CanDrag = true;
            base.m_QuickDrag = false;
            m_FocusMob = null;
        }

        public static void AddTag(int x, int y, string name)
        {
            AddTag(x, y, name, name);
        }

        public static void AddTag(int x, int y, string name, object key)
        {
            for (int i = 0; i < m_Tags.Count; i++)
            {
                TagEntry entry = (TagEntry)m_Tags[i];
                if (entry.m_Key.Equals(key))
                {
                    entry.m_X = x;
                    entry.m_Y = y;
                    entry.m_Name = name;
                    return;
                }
            }
            m_Tags.Add(new TagEntry(x, y, name, key));
        }

        public static void Dispose()
        {
            if (m_Image != null)
            {
                m_Image.Dispose();
                m_Image = null;
            }
            if (m_Swap != null)
            {
                m_Swap.Dispose();
                m_Swap = null;
            }
            m_Colors = null;
        }

        protected internal override void Draw(int X, int Y)
        {
            Mobile mobile = (m_FocusMob == null) ? World.Player : m_FocusMob;
            if (mobile != null)
            {
                DrawImage(X + 2, Y + 2, this.m_Width - 4, this.m_Height - 4, (mobile.Visible || mobile.Player) ? mobile.X : mobile.m_KUOC_X, (mobile.Visible || mobile.Player) ? mobile.Y : mobile.m_KUOC_Y, (mobile.Visible || mobile.Player) ? Engine.m_World : mobile.m_KUOC_F);
            }
            Renderer.SetTexture(null);
            Renderer.SetAlphaEnable(true);
            Renderer.AlphaTestEnable = false;
            Renderer.SetAlpha(0.25f);
            Renderer.TransparentRect(0, X + 4, Y + 4, this.m_Width - 8, this.m_Height - 8);
            Renderer.DrawLine(X, Y + 2, X, (Y + this.m_Height) - 2);
            Renderer.DrawLine(X + 2, Y, (X + this.m_Width) - 2, Y);
            Renderer.DrawLine((X + this.m_Width) - 1, Y + 2, (X + this.m_Width) - 1, (Y + this.m_Height) - 2);
            Renderer.DrawLine(X + 2, (Y + this.m_Height) - 1, (X + this.m_Width) - 2, (Y + this.m_Height) - 1);
            Renderer.DrawPoints(new Point[] { new Point(X + 1, Y + 1), new Point(X + 1, (Y + this.m_Height) - 2), new Point((X + this.m_Width) - 2, Y + 1), new Point((X + this.m_Width) - 2, (Y + this.m_Height) - 2) });
            Renderer.SetAlpha(0.5f);
            Renderer.DrawLine(X + 1, Y + 2, X + 1, (Y + this.m_Height) - 2);
            Renderer.DrawLine(X + 2, Y + 1, (X + this.m_Width) - 2, Y + 1);
            Renderer.DrawLine((X + this.m_Width) - 2, Y + 2, (X + this.m_Width) - 2, (Y + this.m_Height) - 2);
            Renderer.DrawLine(X + 2, (Y + this.m_Height) - 2, (X + this.m_Width) - 2, (Y + this.m_Height) - 2);
            Renderer.TransparentRect(0, X + 3, Y + 3, this.m_Width - 6, this.m_Height - 6);
            Renderer.SetAlpha(1f);
            Renderer.TransparentRect(0, X + 2, Y + 2, this.m_Width - 4, this.m_Height - 4);
            Renderer.SetAlphaEnable(false);
            Renderer.AlphaTestEnable = true;
        }

        protected static void DrawImage(int X, int Y, int Width, int Height, int xCenter, int yCenter, int world)
        {
            if (m_Image == null)
            {
                m_Image = new Texture(Width, Height, true);
            }
            int num = xCenter >> 3;
            int num2 = yCenter >> 3;
            int num3 = xCenter & 7;
            int num4 = yCenter & 7;
            int num5 = num3;
            int num6 = num4;
            int x = 0;
            int y = 0;
            double num9 = 0.0;
            if (((m_xBlock == num) && (m_yBlock == num2)) && ((m_World == world) && (m_Image != null)))
            {
                Renderer.SetFilterEnable(true);
                m_Image.Draw(X, Y, Width, Height, 0f + ((float)(((double)num5) / ((double)m_Image.Width))), 0.5f + ((float)(((double)num6) / ((double)m_Image.Height))), 0.5f + ((float)(((double)num5) / ((double)m_Image.Width))), 0f + ((float)(((double)num6) / ((double)m_Image.Height))), 1f + ((float)(((double)num5) / ((double)m_Image.Width))), 0.5f + ((float)(((double)num6) / ((double)m_Image.Height))), 0.5f + ((float)(((double)num5) / ((double)m_Image.Width))), 1f + ((float)(((double)num6) / ((double)m_Image.Height))));
                Renderer.SetFilterEnable(false);
                x = (X + (Width >> 1)) - 1;
                y = (m_Image.Height >> 1) - 0x10;
                num9 = ((double)y) / ((double)m_Image.Height);
                y = (int)(num9 * Height);
                y += Y;
                DrawTags(X, Y, Width, Height, xCenter, yCenter);
                Renderer.SetTexture(null);
                Renderer.AlphaTestEnable = false;
                Renderer.SolidRect(0xffffff, x, y, 1, 1);
                Renderer.SetAlphaEnable(true);
                Renderer.SetAlpha(0.5f);
                Renderer.SolidRect(0xffffff, x - 1, y, 1, 1);
                Renderer.SolidRect(0xffffff, x + 1, y, 1, 1);
                Renderer.SolidRect(0xffffff, x, y - 1, 1, 1);
                Renderer.SolidRect(0xffffff, x, y + 1, 1, 1);
                Renderer.SetAlpha(0.25f);
                Renderer.SolidRect(0xffffff, x - 2, y, 1, 1);
                Renderer.SolidRect(0xffffff, x + 2, y, 1, 1);
                Renderer.SolidRect(0xffffff, x, y - 2, 1, 1);
                Renderer.SolidRect(0xffffff, x, y + 2, 1, 1);
                Renderer.SetAlpha(0.15f);
                Renderer.SolidRect(0xffffff, x - 1, y - 1, 1, 1);
                Renderer.SolidRect(0xffffff, x + 1, y - 1, 1, 1);
                Renderer.SolidRect(0xffffff, x - 1, y + 1, 1, 1);
                Renderer.SolidRect(0xffffff, x + 1, y + 1, 1, 1);
                Renderer.SetAlphaEnable(false);
                Renderer.AlphaTestEnable = true;
            }
            else
            {
                int num10 = num - 15;
                int num11 = num2 - 15;
                int w = 0x20;
                int h = 0x20;
                m_xWidth = w;
                m_yHeight = h;
                m_xBlock = num;
                m_yBlock = num2;
                m_World = world;
                Load(num10, num11, w, h, world, m_Image);
                if ((m_Image != null) && !m_Image.IsEmpty())
                {
                    Renderer.SetFilterEnable(true);
                    m_Image.Draw(X, Y, Width, Height, 0f + ((float)(((double)num5) / ((double)m_Image.Width))), 0.5f + ((float)(((double)num6) / ((double)m_Image.Height))), 0.5f + ((float)(((double)num5) / ((double)m_Image.Width))), 0f + ((float)(((double)num6) / ((double)m_Image.Height))), 1f + ((float)(((double)num5) / ((double)m_Image.Width))), 0.5f + ((float)(((double)num6) / ((double)m_Image.Height))), 0.5f + ((float)(((double)num5) / ((double)m_Image.Width))), 1f + ((float)(((double)num6) / ((double)m_Image.Height))));
                    Renderer.SetFilterEnable(false);
                }
                x = (X + (Width >> 1)) - 1;
                y = (m_Image.Height >> 1) - 0x10;
                num9 = ((double)y) / ((double)m_Image.Height);
                y = (int)(num9 * Height);
                y += Y;
                DrawTags(X, Y, Width, Height, xCenter, yCenter);
                Renderer.SetTexture(null);
                Renderer.AlphaTestEnable = false;
                Renderer.SolidRect(0xffffff, x, y, 1, 1);
                Renderer.SetAlphaEnable(true);
                Renderer.SetAlpha(0.5f);
                Renderer.SolidRect(0xffffff, x - 1, y, 1, 1);
                Renderer.SolidRect(0xffffff, x + 1, y, 1, 1);
                Renderer.SolidRect(0xffffff, x, y - 1, 1, 1);
                Renderer.SolidRect(0xffffff, x, y + 1, 1, 1);
                Renderer.SetAlpha(0.25f);
                Renderer.SolidRect(0xffffff, x - 2, y, 1, 1);
                Renderer.SolidRect(0xffffff, x + 2, y, 1, 1);
                Renderer.SolidRect(0xffffff, x, y - 2, 1, 1);
                Renderer.SolidRect(0xffffff, x, y + 2, 1, 1);
                Renderer.SetAlpha(0.15f);
                Renderer.SolidRect(0xffffff, x - 1, y - 1, 1, 1);
                Renderer.SolidRect(0xffffff, x + 1, y - 1, 1, 1);
                Renderer.SolidRect(0xffffff, x - 1, y + 1, 1, 1);
                Renderer.SolidRect(0xffffff, x + 1, y + 1, 1, 1);
                Renderer.SetAlphaEnable(false);
                Renderer.AlphaTestEnable = true;
            }
        }

        private static void DrawTags(int x, int y, int width, int height, int xCenter, int yCenter)
        {
            int num = (width >> 1) - 1;
            int num2 = (m_Image.Height >> 1) - 0x10;
            double num3 = ((double)num2) / ((double)m_Image.Height);
            num2 = (int)(num3 * height);
            double num4 = ((double)width) / 256.0;
            double num5 = ((double)height) / 256.0;
            if ((m_FocusMob != World.Player) && (m_FocusMob != null))
            {
                Mobile player = World.Player;
                int num6 = player.X - xCenter;
                int num7 = player.Y - yCenter;
                int num8 = num;
                int num9 = num2;
                num8 += (int)((num6 - num7) * num4);
                num9 += (int)((num6 + num7) * num5);
                if (num8 <= 1)
                {
                    num8 = 2;
                }
                else if (num8 >= (width - 2))
                {
                    num8 = width - 3;
                }
                if (num9 <= 1)
                {
                    num9 = 2;
                }
                else if (num9 >= (height - 2))
                {
                    num9 = height - 3;
                }
                Texture t = Engine.GetUniFont(2).GetString("You", Hues.Bright);
                if ((num8 < num) && (num9 < num2))
                {
                    m_vCache.Draw(t, ((num8 + x) - t.xMin) + 2, ((num9 + y) - t.yMin) + 2);
                }
                else if ((num8 >= num) && (num9 < num2))
                {
                    m_vCache.Draw(t, ((num8 + x) - t.xMax) - 2, ((num9 + y) - t.yMin) + 2);
                }
                else if ((num8 < num) && (num9 >= num2))
                {
                    m_vCache.Draw(t, ((num8 + x) - t.xMin) + 2, ((num9 + y) - t.yMax) - 2);
                }
                else if ((num8 >= num) && (num9 >= num2))
                {
                    m_vCache.Draw(t, ((num8 + x) - t.xMax) - 2, ((num9 + y) - t.yMax) - 2);
                }
                num8 += x;
                num9 += y;
                Renderer.SetTexture(null);
                Renderer.AlphaTestEnable = false;
                Renderer.SolidRect(0xffffff, num8, num9, 1, 1);
                Renderer.SetAlphaEnable(true);
                Renderer.SetAlpha(0.5f);
                Renderer.SolidRect(0xffffff, num8 - 1, num9, 1, 1);
                Renderer.SolidRect(0xffffff, num8 + 1, num9, 1, 1);
                Renderer.SolidRect(0xffffff, num8, num9 - 1, 1, 1);
                Renderer.SolidRect(0xffffff, num8, num9 + 1, 1, 1);
                Renderer.SetAlpha(0.25f);
                Renderer.SolidRect(0xffffff, num8 - 2, num9, 1, 1);
                Renderer.SolidRect(0xffffff, num8 + 2, num9, 1, 1);
                Renderer.SolidRect(0xffffff, num8, num9 - 2, 1, 1);
                Renderer.SolidRect(0xffffff, num8, num9 + 2, 1, 1);
                Renderer.SetAlpha(0.15f);
                Renderer.SolidRect(0xffffff, num8 - 1, num9 - 1, 1, 1);
                Renderer.SolidRect(0xffffff, num8 + 1, num9 - 1, 1, 1);
                Renderer.SolidRect(0xffffff, num8 - 1, num9 + 1, 1, 1);
                Renderer.SolidRect(0xffffff, num8 + 1, num9 + 1, 1, 1);
                Renderer.SetAlphaEnable(false);
                Renderer.AlphaTestEnable = true;
            }
            for (int i = 0; i < m_Tags.Count; i++)
            {
                TagEntry entry = (TagEntry)m_Tags[i];
                int num11 = entry.m_X - xCenter;
                int num12 = entry.m_Y - yCenter;
                int num13 = num;
                int num14 = num2;
                num13 += (int)((num11 - num12) * num4);
                num14 += (int)((num11 + num12) * num5);
                if (num13 <= 1)
                {
                    num13 = 2;
                }
                else if (num13 >= (width - 2))
                {
                    num13 = width - 3;
                }
                if (num14 <= 1)
                {
                    num14 = 2;
                }
                else if (num14 >= (height - 2))
                {
                    num14 = height - 3;
                }
                Texture texture2 = Engine.GetUniFont(2).GetString(entry.m_Name, Hues.Bright);
                if ((num13 < num) && (num14 < num2))
                {
                    m_vCache.Draw(texture2, ((num13 + x) - texture2.xMin) + 2, ((num14 + y) - texture2.yMin) + 2);
                }
                else if ((num13 >= num) && (num14 < num2))
                {
                    m_vCache.Draw(texture2, ((num13 + x) - texture2.xMax) - 2, ((num14 + y) - texture2.yMin) + 2);
                }
                else if ((num13 < num) && (num14 >= num2))
                {
                    m_vCache.Draw(texture2, ((num13 + x) - texture2.xMin) + 2, ((num14 + y) - texture2.yMax) - 2);
                }
                else if ((num13 >= num) && (num14 >= num2))
                {
                    m_vCache.Draw(texture2, ((num13 + x) - texture2.xMax) - 2, ((num14 + y) - texture2.yMax) - 2);
                }
                num13 += x;
                num14 += y;
                Renderer.SetTexture(null);
                Renderer.AlphaTestEnable = false;
                Renderer.SolidRect(0xffffff, num13, num14, 1, 1);
                Renderer.SetAlphaEnable(true);
                Renderer.SetAlpha(0.5f);
                Renderer.SolidRect(0xffffff, num13 - 1, num14, 1, 1);
                Renderer.SolidRect(0xffffff, num13 + 1, num14, 1, 1);
                Renderer.SolidRect(0xffffff, num13, num14 - 1, 1, 1);
                Renderer.SolidRect(0xffffff, num13, num14 + 1, 1, 1);
                Renderer.SetAlpha(0.25f);
                Renderer.SolidRect(0xffffff, num13 - 2, num14, 1, 1);
                Renderer.SolidRect(0xffffff, num13 + 2, num14, 1, 1);
                Renderer.SolidRect(0xffffff, num13, num14 - 2, 1, 1);
                Renderer.SolidRect(0xffffff, num13, num14 + 2, 1, 1);
                Renderer.SetAlpha(0.15f);
                Renderer.SolidRect(0xffffff, num13 - 1, num14 - 1, 1, 1);
                Renderer.SolidRect(0xffffff, num13 + 1, num14 - 1, 1, 1);
                Renderer.SolidRect(0xffffff, num13 - 1, num14 + 1, 1, 1);
                Renderer.SolidRect(0xffffff, num13 + 1, num14 + 1, 1, 1);
                Renderer.SetAlphaEnable(false);
                Renderer.AlphaTestEnable = true;
            }
        }

        private static Point GetPoint(int xTile, int yTile, int xCenter, int yCenter, int xDotCenter, int yDotCenter, double xScale, double yScale)
        {
            int num = xTile - xCenter;
            int num2 = yTile - yCenter;
            int x = xDotCenter;
            int num4 = yDotCenter;
            x += (int)((num - num2) * xScale);
            return new Point(x, num4 + ((int)((num + num2) * yScale)));
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return (!Engine.amMoving && (Engine.TargetHandler == null));
        }

        public static void Invalidate()
        {
            m_xBlock = -1;
        }

        private static unsafe void Load(int x, int y, int w, int h, int world, Texture tex)
        {
            if (m_Colors == null)
            {
                LoadColors();
            }
            if ((m_StrongReferences == null) || (m_StrongReferences.Length != (w * h)))
            {
                m_StrongReferences = new MapBlock[w * h];
            }
            if ((m_Guarded == null) || (m_Guarded.Length != ((w * h) * 0x40)))
            {
                m_Guarded = new BitArray((w * h) * 0x40);
            }
            else
            {
                m_Guarded.SetAll(false);
            }
            Region[] guardedRegions = Region.GuardedRegions;
            int num = x * 8;
            int num2 = y * 8;
            int num3 = w * 8;
            int num4 = h * 8;
            for (int i = 0; i < guardedRegions.Length; i++)
            {
                Region region = guardedRegions[i];
                RegionWorld world2 = region.World;
                bool flag = false;
                switch (world2)
                {
                    case RegionWorld.Britannia:
                        flag = (world == 0) || (world == 1);
                        break;

                    case RegionWorld.Felucca:
                        flag = world == 0;
                        break;

                    case RegionWorld.Trammel:
                        flag = world == 1;
                        break;

                    case RegionWorld.Ilshenar:
                        flag = world == 2;
                        break;

                    case RegionWorld.Malas:
                        flag = world == 3;
                        break;

                    case RegionWorld.Tokuno:
                        flag = world == 4;
                        break;
                }
                if (flag)
                {
                    int num6 = region.X - num;
                    int num7 = region.Y - num2;
                    if (((num6 < num3) && (num7 < num4)) && ((num6 > -region.Width) && (num7 > -region.Height)))
                    {
                        int num8 = num6 + region.Width;
                        int num9 = num7 + region.Height;
                        if (num6 < 0)
                        {
                            num6 = 0;
                        }
                        if (num7 < 0)
                        {
                            num7 = 0;
                        }
                        for (int j = num6; (j < num8) && (j < num3); j++)
                        {
                            for (int k = num7; (k < num9) && (k < num4); k++)
                            {
                                m_Guarded[(k * num3) + j] = true;
                            }
                        }
                    }
                }
            }
            TileMatrix matrix = Map.GetMatrix(world);
            LockData data = tex.Lock(LockFlags.WriteOnly);
            int num12 = data.Pitch >> 1;
            fixed (short* numRef = m_Colors)
            {
                for (int m = 0; m < w; m++)
                {
                    short* numPtr = (short*)((int)data.pvSrc + ((m << 3) * 2));
                    for (int num14 = 0; num14 < h; num14++)
                    {
                        MapBlock block = matrix.GetBlock(x + m, y + num14);
                        m_StrongReferences[(num14 * w) + m] = block;
                        HuedTile[][][] tileArray = (block == null) ? matrix.EmptyStaticBlock : block.m_StaticTiles;
                        Tile[] tileArray2 = (block == null) ? matrix.InvalidLandBlock : block.m_LandTiles;
                        int index = 0;
                        for (int num16 = 0; index < 8; num16 += 8)
                        {
                            for (int num17 = 0; num17 < 8; num17++)
                            {
                                int num18 = -255;
                                int num19 = -255;
                                int num20 = 0;
                                int hue = 0;
                                for (int num24 = 0; num24 < tileArray[num17][index].Length; num24++)
                                {
                                    HuedTile tile = tileArray[num17][index][num24];
                                    int tileID = tile.ID;
                                    switch (tileID)
                                    {
                                        case 0x4001:
                                        case 0x5796:
                                        case 0x61a4:
                                        case 0x6198:
                                        case 0x61bc:
                                        case 0x6199:
                                            break;

                                        default:
                                            {
                                                int z = tile.Z;
                                                int num23 = z + Map.GetHeight(tileID);
                                                if ((num23 > num18) || ((z > num19) && (num23 >= num18)))
                                                {
                                                    num18 = num23;
                                                    num19 = z;
                                                    num20 = tileID;
                                                    hue = tile.Hue;
                                                }
                                                break;
                                            }
                                    }
                                }
                                if ((tileArray2[num16 + num17].Z > num18) && (tileArray2[num16 + num17].ID != 2))
                                {
                                    num20 = tileArray2[num16 + num17].ID;
                                    hue = 0;
                                }
                                int num26 = ((((num14 << 3) + index) * num3) + (m << 3)) + num17;
                                if (m_Guarded[num26] && ((((num26 >= 1) && !m_Guarded[num26 - 1]) || ((num26 >= num3) && !m_Guarded[num26 - num3])) || (((num26 < (m_Guarded.Length - 1)) && !m_Guarded[num26 + 1]) || ((num26 < (m_Guarded.Length - num3)) && !m_Guarded[num26 + num3]))))
                                {
                                    numPtr[num17] = -31776;
                                }
                                else if (hue == 0)
                                {
                                    numPtr[num17] = numRef[num20];
                                }
                                else
                                {
                                    numPtr[num17] = (short)Hues.Load((hue & 0x3fff) | 0x8000).Pixel((ushort)numRef[num20]);
                                }
                            }
                            numPtr += num12;
                            index++;
                        }
                    }
                }
                ArrayList list = Engine.Multis.Items;
                for (int n = 0; n < list.Count; n++)
                {
                    Item item = (Item)list[n];
                    if (item.InWorld && item.Visible)
                    {
                        CustomMultiEntry customMulti = CustomMultiLoader.GetCustomMulti(item.Serial, item.Revision);
                        Multi multi = null;
                        if (customMulti != null)
                        {
                            multi = customMulti.Multi;
                        }
                        if (multi == null)
                        {
                            multi = item.Multi;
                        }
                        if (multi != null)
                        {
                            short[][] radar = multi.Radar;
                            if (radar != null)
                            {
                                int num28;
                                int num29;
                                int num30;
                                int num31;
                                multi.GetBounds(out num28, out num29, out num30, out num31);
                                int num32 = 0;
                                for (int num33 = (item.Y - (y << 3)) + num29; num32 < radar.Length; num33++)
                                {
                                    if ((num33 >= 0) && (num33 < (h << 3)))
                                    {
                                        short* numPtr2 = (short*)((int)data.pvSrc + ((num33 * num12) * 2));
                                        short[] numArray2 = radar[num32];
                                        int num34 = 0;
                                        for (int num35 = (item.X - (x << 3)) + num28; num34 < numArray2.Length; num35++)
                                        {
                                            if (((num35 >= 0) && (num35 < (w << 3))) && (numArray2[num34] != 0))
                                            {
                                                numPtr2[num35] = numRef[numArray2[num34]];
                                            }
                                            num34++;
                                        }
                                    }
                                    num32++;
                                }
                            }
                        }
                    }
                }
            }
            tex.Unlock();
        }

        private static unsafe void LoadColors()
        {
            Debug.TimeBlock("Initializing Radar");
            m_Colors = new short[0x8000];
            byte[] buffer = new byte[0x10000];
            Stream stream = Engine.FileManager.OpenMUL("RadarCol.mul");
            Engine.NativeRead((FileStream)stream, buffer, 0, buffer.Length);
            stream.Close();
            fixed (byte* numRef = buffer)
            {
                fixed (short* numRef2 = m_Colors)
                {
                    ushort* numPtr = (ushort*)numRef;
                    ushort* numPtr2 = (ushort*)numRef2;
                    int num = 0;
                    while (num++ < 0x8000)
                    {
                        numPtr2++;
                        numPtr++;
                        numPtr2[0] = (ushort)(numPtr[0] | 0x8000);
                    }
                    for (int i = 0; i < 0x4000; i++)
                    {
                        numRef2[i] = numRef2[TextureTable.m_Table[i]];
                    }
                }
            }
            Debug.EndBlock();
        }

        protected internal override void OnDispose()
        {
            m_Open = false;
        }

        protected internal override void OnMouseDown(int X, int Y, MouseButtons mb)
        {
            if (mb == MouseButtons.Right)
            {
                m_ToClose = true;
            }
        }

        protected internal override void OnMouseEnter(int X, int Y, MouseButtons mb)
        {
            if (m_ToClose && (mb != MouseButtons.Right))
            {
                m_ToClose = false;
            }
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            if (m_ToClose)
            {
                Gumps.Destroy(this);
            }
            else if (((mb & MouseButtons.Left) != MouseButtons.None) && ((Control.ModifierKeys & Keys.Control) != Keys.None))
            {
                m_FocusMob = null;
            }
        }

        public static void Open()
        {
            if (!m_Open)
            {
                Gumps.Desktop.Children.Add(new GRadar());
            }
        }

        public static void Swap()
        {
            Texture texture = m_Image;
            m_Image = m_Swap;
            m_Swap = texture;
        }

        public override int Height
        {
            get
            {
                return this.m_Height;
            }
            set
            {
                this.m_Height = value;
            }
        }

        public int MaxHeight
        {
            get
            {
                return 260;
            }
        }

        public int MaxWidth
        {
            get
            {
                return 260;
            }
        }

        public int MinHeight
        {
            get
            {
                return 0x44;
            }
        }

        public int MinWidth
        {
            get
            {
                return 0x44;
            }
        }

        public override int Width
        {
            get
            {
                return this.m_Width;
            }
            set
            {
                this.m_Width = value;
            }
        }

        private class TagEntry
        {
            public object m_Key;
            public string m_Name;
            public int m_X;
            public int m_Y;

            public TagEntry(int x, int y, string name, object key)
            {
                this.m_X = x;
                this.m_Y = y;
                this.m_Name = name;
                this.m_Key = key;
            }
        }
    }
}