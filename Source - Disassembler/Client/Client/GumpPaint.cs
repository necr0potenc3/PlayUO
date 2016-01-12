namespace Client
{
    using System.Drawing;

    public class GumpPaint
    {
        private static Client.Clipper m_Clipper;

        public static Color Blend(Color c1, Color c2, int p)
        {
            return Color.FromArgb(((c1.R * p) + (c2.R * (0xff - p))) / 0xff, ((c1.G * p) + (c2.G * (0xff - p))) / 0xff, ((c1.B * p) + (c2.B * (0xff - p))) / 0xff);
        }

        public static Color Blend(Color c1, Color c2, float f)
        {
            return Blend(c1, c2, (int)(f * 255f));
        }

        public static int Blend(int c1, int c2, int p)
        {
            return (Blend(Color.FromArgb(c1), Color.FromArgb(c2), p).ToArgb() & 0xffffff);
        }

        public static int Blend(int c1, int c2, float f)
        {
            return (Blend(Color.FromArgb(c1), Color.FromArgb(c2), f).ToArgb() & 0xffffff);
        }

        public static void DrawFlat(int x, int y, int w, int h)
        {
            DrawRect(SystemColors.ControlDark, x, y, w, h);
            DrawRect(SystemColors.Control, x + 1, y + 1, w - 2, h - 2);
        }

        public static void DrawFlat(int x, int y, int w, int h, int border, int fill)
        {
            DrawRect(border, x, y, w, h);
            DrawRect(fill, x + 1, y + 1, w - 2, h - 2);
        }

        public static void DrawRaised3D(int x, int y, int w, int h)
        {
            DrawRect(SystemColors.ControlDarkDark, x, y, w, h);
            DrawRect(SystemColors.ControlLight, x, y, w - 1, h - 1);
            DrawRect(SystemColors.ControlDark, x + 1, y + 1, w - 2, h - 2);
            DrawRect(SystemColors.ControlLightLight, x + 1, y + 1, w - 3, h - 3);
            DrawRect(SystemColors.Control, x + 2, y + 2, w - 4, h - 4);
        }

        public static void DrawRaised3D(int x, int y, int w, int h, int fill)
        {
            DrawRect(SystemColors.ControlDarkDark, x, y, w, h);
            DrawRect(SystemColors.ControlLight, x, y, w - 1, h - 1);
            DrawRect(SystemColors.ControlDark, x + 1, y + 1, w - 2, h - 2);
            DrawRect(SystemColors.ControlLightLight, x + 1, y + 1, w - 3, h - 3);
            DrawRect(fill, x + 2, y + 2, w - 4, h - 4);
        }

        public static void DrawRect(Color c, int x, int y, int w, int h)
        {
            if (m_Clipper == null)
            {
                Renderer.SolidRect(c.ToArgb() & 0xffffff, x, y, w, h);
            }
            else
            {
                Renderer.SolidRect(c.ToArgb() & 0xffffff, x, y, w, h, m_Clipper);
            }
        }

        public static void DrawRect(int c, int x, int y, int w, int h)
        {
            if (m_Clipper == null)
            {
                Renderer.SolidRect(c, x, y, w, h);
            }
            else
            {
                Renderer.SolidRect(c, x, y, w, h, m_Clipper);
            }
        }

        public static void DrawSunken3D(int x, int y, int w, int h)
        {
            DrawRect(SystemColors.ControlLightLight, x, y, w, h);
            DrawRect(SystemColors.ControlDark, x, y, w - 1, h - 1);
            DrawRect(SystemColors.ControlLight, x + 1, y + 1, w - 2, h - 2);
            DrawRect(SystemColors.ControlDarkDark, x + 1, y + 1, w - 3, h - 3);
            DrawRect(SystemColors.Control, x + 2, y + 2, w - 4, h - 4);
        }

        public static void DrawSunken3D(int x, int y, int w, int h, int fill)
        {
            DrawRect(SystemColors.ControlLightLight, x, y, w, h);
            DrawRect(SystemColors.ControlDark, x, y, w - 1, h - 1);
            DrawRect(SystemColors.ControlLight, x + 1, y + 1, w - 2, h - 2);
            DrawRect(SystemColors.ControlDarkDark, x + 1, y + 1, w - 3, h - 3);
            DrawRect(fill, x + 2, y + 2, w - 4, h - 4);
        }

        public static void Invalidate()
        {
        }

        public static Client.Clipper Clipper
        {
            get
            {
                return m_Clipper;
            }
            set
            {
                m_Clipper = value;
            }
        }
    }
}