namespace Client
{
    using System;
    using System.Collections;
    using System.Drawing;

    public class GServerGump : Gump
    {
        private ArrayList m_AlphaRegions;
        private bool m_CanClose;
        private bool m_CanMove;
        private int m_DialogID;
        private static Hashtable m_LocationCache = new Hashtable();
        private int m_Page;
        private Hashtable m_Pages;
        private int m_Serial;

        public GServerGump(int serial, int dialogID, int x, int y, string layout, string[] text) : base(x, y)
        {
            this.m_AlphaRegions = new ArrayList();
            this.m_Serial = serial;
            this.m_DialogID = dialogID;
            this.m_CanClose = true;
            this.m_CanMove = true;
            base.m_NonRestrictivePicking = true;
            this.m_Pages = new Hashtable();
            this.m_Page = -1;
            LayoutEntry[] list = this.ParseLayout(layout);
            this.ProcessLayout(list, text);
        }

        public static void ClearCachedLocation(int dialogID)
        {
            m_LocationCache.Remove(dialogID);
        }

        public static void GetCachedLocation(int dialogID, ref int x, ref int y)
        {
            LocationCacheEntry entry = m_LocationCache[dialogID] as LocationCacheEntry;
            if (entry != null)
            {
                x = entry.m_xOffset;
                y = entry.m_yOffset;
                m_LocationCache.Remove(dialogID);
            }
        }

        private void OnScroll(double vNew, double vOld, Gump g)
        {
            int y = (int) vNew;
            Gump tag = (Gump) g.GetTag("toScroll");
            int num2 = (int) g.GetTag("yBase");
            int h = (int) g.GetTag("viewHeight");
            tag.Y = num2 - y;
            ((GHtmlLabel) tag).Scissor(0, y, tag.Width, h);
        }

        public GumpList Pages(int page)
        {
            object obj2 = this.m_Pages[page];
            if (obj2 == null)
            {
                obj2 = this.m_Pages[page] = new GumpList(this);
            }
            return (GumpList) obj2;
        }

        private LayoutEntry[] ParseLayout(string layout)
        {
            int num;
            ArrayList dataStore = Engine.GetDataStore();
            int startIndex = 0;
            while ((num = layout.IndexOf('{', startIndex)) >= 0)
            {
                num++;
                startIndex = layout.IndexOf('}', num);
                dataStore.Add(new LayoutEntry(layout.Substring(num, startIndex - num).Trim()));
            }
            LayoutEntry[] entryArray = (LayoutEntry[]) dataStore.ToArray(typeof(LayoutEntry));
            Engine.ReleaseDataStore(dataStore);
            return entryArray;
        }

        private void ProcessHtmlGump(int thisPage, int x, int y, int width, int height, string text, bool hasBack, bool hasScroll, int color)
        {
            UnicodeFont uniFont = Engine.GetUniFont(1);
            if (!hasScroll)
            {
                if (hasBack)
                {
                    GServerBackground toAdd = new GServerBackground(this, x, y, width, height, 0xbbc, true);
                    GHtmlLabel label = new GHtmlLabel(text, uniFont, color, toAdd.OffsetX - 2, toAdd.OffsetY - 1, toAdd.UseWidth);
                    label.Scissor(0, 0, label.Width, toAdd.UseHeight);
                    toAdd.Children.Add(label);
                    this.Pages(thisPage).Add(toAdd);
                }
                else
                {
                    GHtmlLabel label2 = new GHtmlLabel(text, uniFont, color, x - 2, y - 1, width);
                    label2.Scissor(0, 0, label2.Width, height);
                    this.Pages(thisPage).Add(label2);
                }
            }
            else
            {
                GHtmlLabel label3;
                int num;
                width -= 15;
                if (hasBack)
                {
                    GServerBackground background2 = new GServerBackground(this, x, y, width, height, 0xbbc, true);
                    label3 = new GHtmlLabel(text, uniFont, color, background2.OffsetX - 2, background2.OffsetY - 1, background2.UseWidth);
                    label3.Scissor(0, 0, label3.Width, num = background2.UseHeight);
                    background2.Children.Add(label3);
                    this.Pages(thisPage).Add(background2);
                }
                else
                {
                    label3 = new GHtmlLabel(text, uniFont, color, x - 2, y - 1, width);
                    label3.Scissor(0, 0, label3.Width, num = height);
                    this.Pages(thisPage).Add(label3);
                }
                if ((height >= 0x1b) && (label3.Height > num))
                {
                    this.Pages(thisPage).Add(new GImage(0x101, x + width, y));
                    this.Pages(thisPage).Add(new GImage(0xff, x + width, (y + height) - 0x20));
                    for (int i = y + 30; (i + 0x20) < (y + height); i += 30)
                    {
                        this.Pages(thisPage).Add(new GImage(0x100, x + width, i));
                    }
                    GVSlider slider = new GVSlider(0xfe, (x + width) + 1, (y + 1) + 12, 13, (height - 2) - 0x18, 0.0, 0.0, (double) (label3.Height - num), 1.0);
                    slider.SetTag("yBase", label3.Y);
                    slider.SetTag("toScroll", label3);
                    slider.SetTag("viewHeight", num);
                    slider.OnValueChange = new OnValueChange(this.OnScroll);
                    this.Pages(thisPage).Add(slider);
                    this.Pages(thisPage).Add(new GHotspot(x + width, y, 15, height, slider));
                }
                else
                {
                    this.Pages(thisPage).Add(new GImage(0x101, x + width, y));
                    this.Pages(thisPage).Add(new GImage(0xff, x + width, (y + height) - 0x20));
                    for (int j = y + 30; (j + 0x20) < (y + height); j += 30)
                    {
                        this.Pages(thisPage).Add(new GImage(0x100, x + width, j));
                    }
                    this.Pages(thisPage).Add(new GImage(0xfe, Hues.Grayscale, (x + width) + 1, y + 1));
                }
            }
        }

        private void ProcessLayout(LayoutEntry[] list, string[] text)
        {
            int page = 0;
            int num2 = 0;
            bool flag = false;
            for (int i = 0; i < list.Length; i++)
            {
                string str;
                IHue hue;
                int num5;
                string str3;
                int num6;
                IFont uniFont;
                GLabel label;
                object obj2;
                LayoutEntry le = list[i];
                if (((obj2 = le.Name) != null) && ((obj2 = <PrivateImplementationDetails>.$$method0x60000f4-1[obj2]) != null))
                {
                    switch (((int) obj2))
                    {
                        case 0:
                        case 1:
                        {
                            continue;
                        }
                        case 2:
                            goto Label_020B;

                        case 3:
                            goto Label_0243;

                        case 4:
                        {
                            this.m_CanClose = false;
                            continue;
                        }
                        case 5:
                        {
                            this.m_CanMove = false;
                            continue;
                        }
                        case 6:
                        {
                            this.Pages(page).Add(new GServerBackground(this, le[0], le[1], le[3], le[4], le[2] + 4, true));
                            continue;
                        }
                        case 7:
                        {
                            this.Pages(page).Add(new GServerBackground(this, le[0], le[1], le[2], le[3], le[4], false));
                            continue;
                        }
                        case 8:
                            goto Label_0301;

                        case 9:
                        {
                            this.Pages(page).Add(new GServerTextBox(text[le[6]], le));
                            continue;
                        }
                        case 10:
                        {
                            this.Pages(page).Add(new GItemArt(le[0], le[1], le[2]));
                            continue;
                        }
                        case 11:
                        {
                            this.Pages(page).Add(new GServerButton(this, le));
                            continue;
                        }
                        case 12:
                        {
                            this.Pages(page).Add(new GServerRadio(this, le));
                            continue;
                        }
                        case 13:
                        {
                            this.Pages(page).Add(new GServerCheckBox(this, le));
                            continue;
                        }
                        case 14:
                        {
                            int num4 = le[2];
                            this.Pages(page).Add(new GLabel(text[le[3]], Engine.GetUniFont(1), Hues.Load(num4 + 1), le[0] - 1, le[1]));
                            continue;
                        }
                        case 15:
                            num5 = le[4];
                            str3 = text[le[5]];
                            num6 = le[2];
                            uniFont = Engine.GetUniFont(1);
                            if (uniFont.GetStringWidth(str3) <= num6)
                            {
                                goto Label_051A;
                            }
                            goto Label_04EB;

                        case 0x10:
                        {
                            this.ProcessHtmlGump(page, le[0], le[1], le[2], le[3], Localization.GetString(le[4]), le[5] != 0, le[6] != 0, ((le[5] == 0) && (le[6] != 0)) ? 0xffffff : 0);
                            continue;
                        }
                        case 0x11:
                        {
                            this.ProcessHtmlGump(page, le[0], le[1], le[2], le[3], Localization.GetString(le[4]), le[5] != 0, le[6] != 0, Engine.C16232(le[7]));
                            continue;
                        }
                        case 0x12:
                        {
                            this.ProcessHtmlGump(page, le[0], le[1], le[2], le[3], text[le[4]], le[5] != 0, le[6] != 0, ((le[5] == 0) && (le[6] != 0)) ? 0xffffff : 0);
                            continue;
                        }
                    }
                }
                goto Label_06A2;
            Label_020B:
                this.Pages(page).Add(new GTransparentRegion(this, le[0], le[1], le[2], le[3]));
                continue;
            Label_0243:
                page = le[0];
                if (page == 0)
                {
                    flag = false;
                    num2 = 0;
                }
                else if (!flag || (page < num2))
                {
                    num2 = page;
                    flag = true;
                }
                continue;
            Label_0301:
                str = le.GetAttribute("hue");
                if (str != null)
                {
                    try
                    {
                        hue = Hues.Load(Convert.ToInt32(str) + 1);
                    }
                    catch
                    {
                        hue = Hues.Default;
                    }
                }
                else
                {
                    hue = Hues.Default;
                }
                if (le.GetAttribute("class") == "VirtueGumpItem")
                {
                    this.Pages(page).Add(new GVirtueItem(this, le[0], le[1], le[2], hue));
                }
                else
                {
                    this.Pages(page).Add(new GServerImage(this, le[0], le[1], le[2], hue));
                }
                continue;
            Label_04D8:
                str3 = str3.Substring(0, str3.Length - 1);
            Label_04EB:
                if ((str3.Length > 0) && (uniFont.GetStringWidth(str3 + "...") > num6))
                {
                    goto Label_04D8;
                }
                str3 = str3 + "...";
            Label_051A:
                label = new GLabel(str3, uniFont, Hues.Load(num5 + 1), le[0] - 1, le[1]);
                label.Scissor(0, 0, num6, le[3]);
                this.Pages(page).Add(label);
                continue;
            Label_06A2:
                Engine.AddTextMessage(le.Name);
            }
            this.Page = (num2 == 0) ? 1 : num2;
        }

        protected internal override void Render(int X, int Y)
        {
            if (this.m_AlphaRegions.Count == 0)
            {
                base.Render(X, Y);
            }
            else if (base.m_Visible)
            {
                int x = X + this.X;
                int y = Y + this.Y;
                this.Draw(x, y);
                Gump[] gumpArray = base.m_Children.ToArray();
                RectangleList list = new RectangleList();
                RectangleList list2 = new RectangleList();
                int num3 = 0;
                for (int i = 0; i < gumpArray.Length; i++)
                {
                    Gump gump = gumpArray[i];
                    if (gump is GTransparentRegion)
                    {
                        num3++;
                        continue;
                    }
                    if (num3 >= this.m_AlphaRegions.Count)
                    {
                        gump.Render(x, y);
                        continue;
                    }
                    Rectangle rect = new Rectangle(gump.X, gump.Y, gump.Width, gump.Height);
                    list.Add(rect);
                    for (int j = num3; j < this.m_AlphaRegions.Count; j++)
                    {
                        Gump gump2 = (Gump) this.m_AlphaRegions[j];
                        if (gump2 is GTransparentRegion)
                        {
                            Rectangle a = new Rectangle(gump2.X, gump2.Y, gump2.Width, gump2.Height);
                            Rectangle rectangle3 = Rectangle.Intersect(a, rect);
                            list.Remove(rectangle3);
                            list2.Add(rectangle3);
                        }
                    }
                    if (list2.Count > 0)
                    {
                        for (int k = i + 1; k < gumpArray.Length; k++)
                        {
                            Gump gump3 = gumpArray[k];
                            if (gump3 is GServerBackground)
                            {
                                GServerBackground background = (GServerBackground) gump3;
                                Rectangle rectangle4 = new Rectangle(background.X + background.OffsetX, background.Y + background.OffsetY, background.UseWidth, background.UseHeight);
                                list.Remove(rectangle4);
                                list2.Remove(rectangle4);
                            }
                            else if (gump3 == this.m_AlphaRegions[this.m_AlphaRegions.Count - 1])
                            {
                                break;
                            }
                        }
                        if ((list2.Count == 1) && (list.Count == 0))
                        {
                            Renderer.SetAlphaEnable(true);
                            Renderer.SetAlpha(0.5f);
                            gump.Render(x, y);
                            Renderer.SetAlphaEnable(false);
                        }
                        else
                        {
                            for (int m = 0; m < list.Count; m++)
                            {
                                Rectangle rectangle5 = list[m];
                                if (Renderer.SetViewport(x + rectangle5.X, y + rectangle5.Y, rectangle5.Width, rectangle5.Height))
                                {
                                    gump.Render(x, y);
                                }
                            }
                            for (int n = 0; n < list2.Count; n++)
                            {
                                Rectangle rectangle6 = list2[n];
                                if (Renderer.SetViewport(x + rectangle6.X, y + rectangle6.Y, rectangle6.Width, rectangle6.Height))
                                {
                                    Renderer.SetAlphaEnable(true);
                                    Renderer.SetAlpha(0.5f);
                                    gump.Render(x, y);
                                    Renderer.SetAlphaEnable(false);
                                }
                            }
                            if ((list.Count > 0) || (list2.Count > 0))
                            {
                                Renderer.SetViewport(0, 0, Engine.ScreenWidth, Engine.ScreenHeight);
                            }
                        }
                        list.Clear();
                        list2.Clear();
                        continue;
                    }
                    gump.Render(x, y);
                }
            }
        }

        public static void SetCachedLocation(int dialogID, int x, int y)
        {
            LocationCacheEntry entry = m_LocationCache[dialogID] as LocationCacheEntry;
            if (entry == null)
            {
                m_LocationCache[dialogID] = entry = new LocationCacheEntry(dialogID);
            }
            entry.m_xOffset = x;
            entry.m_yOffset = y;
        }

        public bool CanClose
        {
            get
            {
                return this.m_CanClose;
            }
        }

        public bool CanMove
        {
            get
            {
                return this.m_CanMove;
            }
        }

        public int DialogID
        {
            get
            {
                return this.m_DialogID;
            }
        }

        public int Page
        {
            get
            {
                return this.m_Page;
            }
            set
            {
                if (this.m_Page != value)
                {
                    this.m_Page = value;
                    base.m_Children.Set(this.Pages(0));
                    if (this.m_Page != 0)
                    {
                        base.m_Children.Add(this.Pages(this.m_Page));
                    }
                    this.m_AlphaRegions.Clear();
                    for (int i = 0; i < base.m_Children.Count; i++)
                    {
                        if (base.m_Children[i] is GTransparentRegion)
                        {
                            this.m_AlphaRegions.Add(base.m_Children[i]);
                        }
                    }
                }
            }
        }

        public int Serial
        {
            get
            {
                return this.m_Serial;
            }
        }

        private class LocationCacheEntry
        {
            public int m_DialogID;
            public int m_xOffset;
            public int m_yOffset;

            public LocationCacheEntry(int dialogID)
            {
                this.m_DialogID = dialogID;
            }
        }
    }
}

