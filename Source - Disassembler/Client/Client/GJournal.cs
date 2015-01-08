namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GJournal : GAlphaBackground, IResizable
    {
        protected int m_CropWidth;
        protected GHotspot m_Hotspot;
        protected GAlphaVSlider m_Scroller;
        protected bool m_ToClose;
        private static VertexCache m_vCache = new VertexCache();

        public GJournal() : base(50, 50, 300, 0xbc)
        {
            int num = Engine.m_Journal.Count - 1;
            if (num < 0)
            {
                num = 0;
            }
            base.m_Children.Add(new GVResizer(this));
            base.m_Children.Add(new GHResizer(this));
            base.m_Children.Add(new GLResizer(this));
            base.m_Children.Add(new GTResizer(this));
            base.m_Children.Add(new GHVResizer(this));
            base.m_Children.Add(new GLTResizer(this));
            base.m_Children.Add(new GHTResizer(this));
            base.m_Children.Add(new GLVResizer(this));
            this.m_Scroller = new GAlphaVSlider(0, 10, 0x10, 0xa9, (double) num, 0.0, (double) num, 1.0);
            this.m_Hotspot = new GHotspot(0, 4, 0x10, 180, this.m_Scroller);
            this.m_Hotspot.NormalHit = false;
            base.m_Children.Add(this.m_Scroller);
            base.m_Children.Add(this.m_Hotspot);
            this.Width = 300;
            this.Height = 0xbc;
        }

        protected internal override void Draw(int X, int Y)
        {
            base.Draw(X, Y);
            int x = X + 2;
            int num2 = base.m_Height - 2;
            int count = Engine.m_Journal.Count;
            int num4 = (int) this.m_Scroller.GetValue();
            if (num4 >= count)
            {
                num4 = count - 1;
            }
            UnicodeFont uniFont = Engine.GetUniFont(3);
            for (int i = num4; (i >= 0) && (i < count); i--)
            {
                Texture image;
                JournalEntry entry = (JournalEntry) Engine.m_Journal[i];
                if (entry.Width != this.m_CropWidth)
                {
                    string str = Engine.WrapText(entry.Text, this.m_CropWidth, uniFont);
                    image = uniFont.GetString(str, entry.Hue);
                    entry.Width = this.m_CropWidth;
                    entry.Image = image;
                }
                else
                {
                    image = entry.Image;
                }
                if ((image != null) && !image.IsEmpty())
                {
                    num2 -= image.Height;
                    if (num2 < 3)
                    {
                        image.DrawClipped(x, Y + num2, Clipper.TemporaryInstance(X, Y + 1, this.Width, this.Height));
                        break;
                    }
                    m_vCache.Draw(image, x, Y + num2);
                    num2 -= 4;
                }
            }
        }

        protected internal override void OnDispose()
        {
            Engine.m_JournalOpen = false;
            Engine.m_JournalGump = null;
            base.OnDispose();
        }

        public void OnEntryAdded()
        {
            double num = this.m_Scroller.GetValue();
            this.m_Scroller.End = Engine.m_Journal.Count;
            if (num != (Engine.m_Journal.Count - 1))
            {
                this.m_Scroller.SetValue(num, false);
            }
        }

        protected internal override void OnMouseDown(int X, int Y, MouseButtons mb)
        {
            base.BringToTop();
            if (mb == MouseButtons.Right)
            {
                this.m_ToClose = true;
            }
        }

        protected internal override void OnMouseEnter(int X, int Y, MouseButtons mb)
        {
            if (this.m_ToClose && (mb != MouseButtons.Right))
            {
                this.m_ToClose = false;
            }
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            if (this.m_ToClose)
            {
                Gumps.Destroy(this);
                Engine.m_JournalOpen = false;
                Engine.m_JournalGump = null;
            }
        }

        protected internal override void OnMouseWheel(int Delta)
        {
            base.BringToTop();
            this.m_Scroller.SetValue(this.m_Scroller.GetValue() + ((-Math.Sign(Delta) * 5.0) * this.m_Scroller.Increase), true);
        }

        public override int Height
        {
            get
            {
                return base.m_Height;
            }
            set
            {
                base.m_Height = value;
                double num = this.m_Scroller.GetValue();
                this.m_Hotspot.Height = base.m_Height - 8;
                this.m_Scroller.Height = base.m_Height - 0x13;
                this.m_Scroller.SetValue(num, false);
            }
        }

        public int MaxHeight
        {
            get
            {
                return Engine.ScreenHeight;
            }
        }

        public int MaxWidth
        {
            get
            {
                return Engine.ScreenWidth;
            }
        }

        public int MinHeight
        {
            get
            {
                return 100;
            }
        }

        public int MinWidth
        {
            get
            {
                return 100;
            }
        }

        public override int Width
        {
            get
            {
                return base.m_Width;
            }
            set
            {
                base.m_Width = value;
                this.m_CropWidth = base.m_Width - 0x18;
                this.m_Scroller.X = this.m_Hotspot.X = base.m_Width - 0x13;
            }
        }
    }
}

