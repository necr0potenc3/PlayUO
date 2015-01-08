namespace Client
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class Cursor
    {
        private static CursorEntry[,] m_Cursors = new CursorEntry[0x10, 3];
        private static bool m_Gold;
        private static bool m_Hourglass;
        private static bool m_Visible = true;

        public static void Dispose()
        {
            for (int i = 0; i < 0x10; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (m_Cursors[i, j] != null)
                    {
                        m_Cursors[i, j].m_Image.Dispose();
                        m_Cursors[i, j].m_Image = null;
                    }
                }
            }
            m_Cursors = null;
        }

        public static void Draw()
        {
            CursorEntry cursor = GetCursor();
            if (cursor != null)
            {
                if (Renderer.m_AlphaEnable)
                {
                    Renderer.SetAlphaEnablePrecalc(false);
                }
                cursor.Draw(Engine.m_xMouse, Engine.m_yMouse);
            }
        }

        public static CursorEntry GetCursor()
        {
            if (!m_Visible)
            {
                return null;
            }
            int idCursor = 7;
            int idType = 0;
            if (Engine.TargetHandler != null)
            {
                idCursor = 12;
            }
            else if (m_Hourglass)
            {
                idCursor = 13;
            }
            else if ((Gumps.Drag != null) && Gumps.Drag.m_DragCursor)
            {
                idCursor = 8;
            }
            else if ((Gumps.LastOver != null) && Gumps.LastOver.m_OverridesCursor)
            {
                idCursor = Gumps.LastOver.m_OverCursor;
            }
            else if (GObjectProperties.Instance != null)
            {
                idCursor = 9;
            }
            else if (Engine.m_Ingame)
            {
                idCursor = (int) Engine.pointingDir;
            }
            if (Engine.m_Ingame)
            {
                Mobile player = World.Player;
                if (player != null)
                {
                    if (player.Flags[MobileFlag.Warmode])
                    {
                        idType = 1;
                    }
                    else if (m_Gold)
                    {
                        idType = 2;
                    }
                }
                else if (m_Gold)
                {
                    idType = 2;
                }
            }
            CursorEntry entry = m_Cursors[idCursor, idType];
            if (entry == null)
            {
                entry = m_Cursors[idCursor, idType] = LoadCursor(idCursor, idType);
            }
            return entry;
        }

        private static unsafe CursorEntry LoadCursor(int idCursor, int idType)
        {
            int num;
            IHue hue;
            switch (idType)
            {
                case 0:
                    hue = Hues.Default;
                    num = 0x606a + idCursor;
                    break;

                case 1:
                    hue = Hues.Default;
                    num = 0x6053 + idCursor;
                    break;

                case 2:
                    hue = Hues.Load(0x896d);
                    num = 0x606a + idCursor;
                    break;

                default:
                    return null;
            }
            Texture item = hue.GetItem(num);
            if ((item == null) || item.IsEmpty())
            {
                return new CursorEntry(0, 0, 0, 0, Texture.Empty);
            }
            if (item.m_Factory != null)
            {
                item.m_Factory.Remove(item);
                item.m_Factory = null;
                item.m_FactoryArgs = null;
            }
            int xOffset = 0;
            int yOffset = 0;
            if (idType < 2)
            {
                LockData data = item.Lock(LockFlags.ReadWrite);
                int width = data.Width;
                int height = data.Height;
                int num6 = data.Pitch >> 1;
                short* pvSrc = (short*) data.pvSrc;
                short* numPtr2 = (short*) (data.pvSrc + (((height - 1) * num6) * 2));
                for (int i = 0; i < width; i++)
                {
                    if ((pvSrc[0] & 0x7fff) == 0x3e0)
                    {
                        xOffset = i;
                    }
                    pvSrc++;
                    pvSrc[0] = 0;
                    numPtr2++;
                    numPtr2[0] = 0;
                }
                pvSrc = (short*) data.pvSrc;
                numPtr2 = (short*) (data.pvSrc + ((width - 1) * 2));
                for (int j = 0; j < height; j++)
                {
                    if ((pvSrc[0] & 0x7fff) == 0x3e0)
                    {
                        yOffset = j;
                    }
                    pvSrc[0] = 0;
                    numPtr2[0] = 0;
                    pvSrc += num6;
                    numPtr2 += num6;
                }
                item.Unlock();
            }
            else
            {
                CursorEntry entry = m_Cursors[idCursor, 1];
                if (entry == null)
                {
                    entry = m_Cursors[idCursor, 0];
                    if (entry == null)
                    {
                        entry = m_Cursors[idCursor, 1] = LoadCursor(idCursor, 1);
                    }
                }
                xOffset = entry.m_xOffset;
                yOffset = entry.m_yOffset;
                LockData data2 = item.Lock(LockFlags.ReadWrite);
                int num9 = data2.Width;
                int num10 = data2.Height;
                int num11 = data2.Pitch >> 1;
                short* numPtr3 = (short*) data2.pvSrc;
                short* numPtr4 = (short*) (data2.pvSrc + (((num10 - 1) * num11) * 2));
                for (int k = 0; k < num9; k++)
                {
                    numPtr3++;
                    numPtr3[0] = 0;
                    numPtr4++;
                    numPtr4[0] = 0;
                }
                numPtr3 = (short*) data2.pvSrc;
                numPtr4 = (short*) (data2.pvSrc + ((num9 - 1) * 2));
                for (int m = 0; m < num10; m++)
                {
                    numPtr3[0] = 0;
                    numPtr4[0] = 0;
                    numPtr3 += num11;
                    numPtr4 += num11;
                }
                item.Unlock();
            }
            return new CursorEntry(idCursor, idType, xOffset, yOffset, item);
        }

        public static void MoveTo(Gump who)
        {
            Point point = who.PointToScreen(new Point(who.Width / 2, who.Height / 2));
            Cursor.Position = Engine.m_Display.PointToScreen((Point) point);
            Gumps.Invalidate();
        }

        public static bool Gold
        {
            get
            {
                return m_Gold;
            }
            set
            {
                m_Gold = value;
            }
        }

        public static int Height
        {
            get
            {
                CursorEntry cursor = GetCursor();
                return ((cursor != null) ? cursor.m_Image.Height : 0);
            }
        }

        public static bool Hourglass
        {
            get
            {
                return m_Hourglass;
            }
            set
            {
                m_Hourglass = value;
            }
        }

        public static bool Visible
        {
            get
            {
                return m_Visible;
            }
            set
            {
                m_Visible = value;
            }
        }

        public static int Width
        {
            get
            {
                CursorEntry cursor = GetCursor();
                return ((cursor != null) ? cursor.m_Image.Width : 0);
            }
        }
    }
}

