namespace Client
{
    public class GServerList : GBackground
    {
        private GServerEntry[] m_Entries;
        private GVSlider m_Slider;
        private int m_xLast;
        private int m_yLast;

        public GServerList(Server[] servers, int x, int y, int width, int height, int gumpID, IFont font, IHue hue, int selectionBorderColor, int selectionFillColor, int selectionFillAlpha) : base(gumpID, width, height, x, y, true)
        {
            this.m_xLast = -2147483648;
            this.m_yLast = -2147483648;
            int offsetX = base.OffsetX;
            int offsetY = base.OffsetY;
            int useWidth = base.UseWidth;
            this.m_Entries = new GServerEntry[servers.Length];
            for (int i = 0; i < servers.Length; i++)
            {
                this.m_Entries[i] = new GServerEntry(servers[i], font, hue, offsetX, offsetY, useWidth, selectionBorderColor, selectionFillColor, ((float)selectionFillAlpha) / 255f);
                offsetY += this.m_Entries[i].Height - 1;
                base.m_Children.Add(this.m_Entries[i]);
            }
            offsetY++;
            offsetY -= base.OffsetY;
            if (offsetY > (base.UseHeight - 2))
            {
                base.m_Children.Add(new GImage(0x101, this.Width - 6, 4));
                base.m_Children.Add(new GImage(0xff, this.Width - 6, this.Height - 0x25));
                for (int j = 0x22; (j + 0x20) < (this.Height - 5); j += 30)
                {
                    base.m_Children.Add(new GImage(0x100, this.Width - 6, j));
                }
                base.m_NonRestrictivePicking = true;
                this.m_Slider = new GVSlider(0xfe, this.Width - 5, 0x11, 13, 0xec, 0.0, 0.0, (double)(offsetY - (base.UseHeight - 2)), 1.0);
                this.m_Slider.OnValueChange = new OnValueChange(this.OnScroll);
                this.m_Slider.ScrollOffset = 20.0;
                base.m_Children.Add(this.m_Slider);
                base.m_Children.Add(new GHotspot(this.Width - 6, 4, 15, this.Height - 9, this.m_Slider));
            }
        }

        protected internal override void Draw(int x, int y)
        {
            base.Draw(x, y);
            if ((this.m_xLast != x) || (this.m_yLast != y))
            {
                Clipper clipper = new Clipper(x + base.OffsetX, y + base.OffsetY, base.UseWidth, base.UseHeight);
                Gump[] gumpArray = base.m_Children.ToArray();
                for (int i = 0; i < gumpArray.Length; i++)
                {
                    GServerEntry entry = gumpArray[i] as GServerEntry;
                    if (entry != null)
                    {
                        entry.Clipper = clipper;
                    }
                }
            }
        }

        protected internal override void OnMouseWheel(int delta)
        {
            if (this.m_Slider != null)
            {
                this.m_Slider.OnMouseWheel(delta);
            }
        }

        private void OnScroll(double vNew, double vOld, Gump sender)
        {
            int num = (int)vNew;
            Gump[] gumpArray = base.m_Children.ToArray();
            for (int i = 0; i < gumpArray.Length; i++)
            {
                GServerEntry entry = gumpArray[i] as GServerEntry;
                if (entry != null)
                {
                    entry.Y = entry.yBase - num;
                }
            }
        }
    }
}