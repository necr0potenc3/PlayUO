namespace Client
{
    using System.Drawing;
    using System.Windows.Forms;

    public class GMenuItem : GAlphaBackground
    {
        private Color m_DefaultColor;
        private bool m_DropDown;
        private Color m_ExpandedColor;
        private bool m_MakeTopmost;
        private Color m_OverColor;
        private string m_Text;

        public GMenuItem(string text) : base(0, 50, 120, 0x18)
        {
            this.m_Text = text;
            this.m_DefaultColor = Color.FromArgb(0xc0, 0xc0, 0xc0);
            this.m_OverColor = Color.FromArgb(0x20, 0x40, 0x80);
            this.m_ExpandedColor = Color.FromArgb(0x20, 0x40, 0x80);
            base.FillAlpha = 0.25f;
            base.m_CanDrag = false;
            GLabel toAdd = new GLabel(text, Engine.DefaultFont, Hues.Load(0x481), 0, 0);
            base.m_Children.Add(toAdd);
            toAdd.Center();
            toAdd.X = 4 - toAdd.Image.xMin;
            base.m_NonRestrictivePicking = true;
        }

        public void Add(GMenuItem child)
        {
            if ((child != this) && !this.Contains(child))
            {
                base.m_Children.Add(child);
                child.Visible = false;
                this.Layout();
            }
        }

        public bool Contains(GMenuItem child)
        {
            return (base.m_Children.IndexOf(child) >= 0);
        }

        public void Layout()
        {
            int num3;
            int num4;
            int num = 0;
            Gump[] gumpArray = base.m_Children.ToArray();
            for (int i = 0; i < gumpArray.Length; i++)
            {
                if (gumpArray[i] is GMenuItem)
                {
                    num++;
                }
            }
            if (this.m_DropDown)
            {
                num3 = 0;
                num4 = this.Height - 1;
            }
            else
            {
                Gump desktop = Gumps.Desktop;
                num3 = 0x7c;
                num4 = 0;
                if (desktop != null)
                {
                    int num5 = 1 + (num * 0x17);
                    Client.Point p = base.PointToScreen(new Client.Point(0, 0));
                    int y = desktop.PointToClient(p).Y;
                    int num7 = (desktop.Height - y) - 1;
                    num7 /= 0x17;
                    if (num7 < 1)
                    {
                        num7 = 1;
                    }
                    if (num7 < num)
                    {
                        num4 = (this.Height - (((num - num7) + 1) * 0x17)) - 1;
                    }
                    if ((y + num4) < 0)
                    {
                        num4 = -y;
                    }
                }
            }
            for (int j = 0; j < gumpArray.Length; j++)
            {
                GMenuItem item2 = gumpArray[j] as GMenuItem;
                if (item2 != null)
                {
                    if (item2.X != num3)
                    {
                        item2.X = num3;
                    }
                    if (item2.Y != num4)
                    {
                        item2.Y = num4;
                    }
                    num4 += 0x17;
                }
            }
        }

        public virtual void OnClick()
        {
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            if (mb == MouseButtons.Left)
            {
                this.OnClick();
                if (base.GetType() != typeof(GMenuItem))
                {
                    this.Unexpand();
                }
            }
        }

        public void Remove(GMenuItem child)
        {
            if ((child != this) && this.Contains(child))
            {
                base.m_Children.Remove(child);
                child.Visible = false;
                this.Layout();
            }
        }

        protected internal override void Render(int rx, int ry)
        {
            int num;
            bool flag;
            GMenuItem lastOver = Gumps.LastOver as GMenuItem;
            if (lastOver == null)
            {
                num = this.m_DefaultColor.ToArgb() & 0xffffff;
                flag = true;
            }
            else
            {
                GMenuItem parent = lastOver;
                while ((parent != null) && (parent != this))
                {
                    parent = parent.Parent as GMenuItem;
                }
                flag = parent != this;
                if (flag)
                {
                    num = this.m_DefaultColor.ToArgb() & 0xffffff;
                }
                else if (lastOver == this)
                {
                    num = this.m_OverColor.ToArgb() & 0xffffff;
                }
                else
                {
                    num = this.m_ExpandedColor.ToArgb() & 0xffffff;
                }
            }
            base.FillColor = num;
            if (flag)
            {
                if (this.Width != 120)
                {
                    this.Width = 120;
                }
                Gump[] gumpArray = base.m_Children.ToArray();
                for (int i = 0; i < gumpArray.Length; i++)
                {
                    if (gumpArray[i] is GMenuItem)
                    {
                        ((GMenuItem)gumpArray[i]).Visible = false;
                    }
                }
            }
            else
            {
                bool flag2 = false;
                Gump[] gumpArray2 = base.m_Children.ToArray();
                for (int j = 0; j < gumpArray2.Length; j++)
                {
                    if (gumpArray2[j] is GMenuItem)
                    {
                        ((GMenuItem)gumpArray2[j]).Visible = true;
                        flag2 = true;
                    }
                }
                int num4 = (flag2 && !this.m_DropDown) ? 0x7d : 120;
                if (this.Width != num4)
                {
                    this.Width = num4;
                }
                if (flag2 && this.m_MakeTopmost)
                {
                    base.BringToTop();
                }
                this.Layout();
            }
            base.Render(rx, ry);
        }

        public void SetHue(IHue hue)
        {
            GLabel label = null;
            if (base.m_Children.Count > 0)
            {
                label = base.m_Children[0] as GLabel;
            }
            if (label != null)
            {
                label.Hue = hue;
            }
        }

        public void Unexpand()
        {
            int num = this.m_DefaultColor.ToArgb() & 0xffffff;
            base.FillColor = num;
            if (this.Width != 120)
            {
                this.Width = 120;
            }
            Gump[] gumpArray = base.m_Children.ToArray();
            for (int i = 0; i < gumpArray.Length; i++)
            {
                if (gumpArray[i] is GMenuItem)
                {
                    ((GMenuItem)gumpArray[i]).Visible = false;
                }
            }
            if (base.m_Parent is GMenuItem)
            {
                ((GMenuItem)base.m_Parent).Unexpand();
            }
        }

        public Color DefaultColor
        {
            get
            {
                return this.m_DefaultColor;
            }
            set
            {
                this.m_DefaultColor = value;
            }
        }

        public bool DropDown
        {
            get
            {
                return this.m_DropDown;
            }
            set
            {
                this.m_DropDown = value;
                this.Layout();
            }
        }

        public Color ExpandedColor
        {
            get
            {
                return this.m_ExpandedColor;
            }
            set
            {
                this.m_ExpandedColor = value;
            }
        }

        public bool MakeTopmost
        {
            get
            {
                return this.m_MakeTopmost;
            }
            set
            {
                this.m_MakeTopmost = value;
            }
        }

        public Color OverColor
        {
            get
            {
                return this.m_OverColor;
            }
            set
            {
                this.m_OverColor = value;
            }
        }

        public string Text
        {
            get
            {
                return this.m_Text;
            }
            set
            {
                if (this.m_Text != value)
                {
                    this.m_Text = value;
                    GLabel label = null;
                    if (base.m_Children.Count > 0)
                    {
                        label = base.m_Children[0] as GLabel;
                    }
                    if (label != null)
                    {
                        label.Text = this.m_Text;
                        label.Center();
                        label.X = 4 - label.Image.xMin;
                    }
                }
            }
        }
    }
}