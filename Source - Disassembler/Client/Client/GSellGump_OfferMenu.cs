namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GSellGump_OfferMenu : GImage
    {
        private GSellAccept m_Accept;
        private GSellClear m_Clear;
        private Clipper m_ContentClipper;
        private int m_LastHeight;
        private int m_LastOffset;
        private GSellGump m_Owner;
        private GLabel m_Signature;
        private TimeSync m_SignatureAnimation;
        private GVSlider m_Slider;
        private GLabel m_Total;
        private int m_xLast;
        private int m_yLast;
        private int m_yOffset;

        public GSellGump_OfferMenu(GSellGump owner) : base(0x873, 170, 0xd6)
        {
            string str;
            this.m_Owner = owner;
            Mobile player = World.Player;
            if (((player != null) && ((str = player.Name) != null)) && ((str = str.Trim()).Length > 0))
            {
                this.m_Signature = new GLabel(str, Engine.GetFont(5), Hues.Load(0x455), 0x48, 0xc2);
                this.m_Signature.Visible = false;
                base.m_Children.Add(this.m_Signature);
            }
            this.m_Total = new GLabel("0", Engine.GetFont(6), Hues.Default, 0xbc, 0xa7);
            this.m_Accept = new GSellAccept(owner);
            this.m_Clear = new GSellClear(owner);
            base.m_Children.Add(this.m_Total);
            base.m_Children.Add(this.m_Accept);
            base.m_Children.Add(this.m_Clear);
            base.m_CanDrag = true;
            base.m_QuickDrag = true;
            GVSlider toAdd = new GVSlider(0x828, 0xed, 0x51, 0x22, 0x3a, 0.0, 0.0, 50.0, 1.0);
            this.m_Slider = toAdd;
            base.m_Children.Add(toAdd);
            base.m_Children.Add(new GHotspot(0xed, 0x42, 0x22, 0x54, toAdd));
        }

        protected internal override void Draw(int x, int y)
        {
            if ((this.m_xLast != x) || (this.m_yLast != y))
            {
                this.m_xLast = x;
                this.m_yLast = y;
                Clipper clipper = new Clipper(x + 0x20, y + 0x42, 0xc4, 0x5c);
                this.m_ContentClipper = clipper;
                foreach (Gump gump in base.m_Children.ToArray())
                {
                    if (gump is GSellGump_OfferedItem)
                    {
                        ((GSellGump_OfferedItem) gump).Clipper = clipper;
                    }
                }
            }
            if ((this.m_Signature != null) && (this.m_SignatureAnimation != null))
            {
                double normalized = this.m_SignatureAnimation.Normalized;
                if (normalized >= 1.0)
                {
                    this.m_Signature.Scissor(null);
                    this.m_SignatureAnimation = null;
                }
                else
                {
                    this.m_Signature.Scissor(0, 0, (int) (normalized * this.m_Signature.Width), this.m_Signature.Height);
                }
                Engine.Redraw();
            }
            if (this.m_LastHeight >= 0)
            {
                this.m_yOffset = 0x43 - ((int) ((this.m_Slider.GetValue() / 50.0) * this.m_LastHeight));
            }
            else
            {
                this.m_yOffset = 0x43;
            }
            this.m_LastOffset = 0x43 - this.m_yOffset;
            base.Draw(x, y);
        }

        protected internal override bool HitTest(int x, int y)
        {
            if (base.m_Invalidated)
            {
                base.Refresh();
            }
            return base.m_Image.HitTest(x, y);
        }

        protected internal override void OnDragStart()
        {
            base.m_IsDragging = false;
            Gumps.Drag = null;
            Point point = base.PointToScreen(new Point(0, 0)) - this.m_Owner.PointToScreen(new Point(0, 0));
            this.m_Owner.m_OffsetX = point.X + base.m_OffsetX;
            this.m_Owner.m_OffsetY = point.Y + base.m_OffsetY;
            this.m_Owner.m_IsDragging = true;
            Gumps.Drag = this.m_Owner;
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            if ((mb & MouseButtons.Right) != MouseButtons.None)
            {
                Gumps.Destroy(this.m_Owner);
            }
        }

        protected internal override void Render(int x, int y)
        {
            base.Render(x, y);
            int num = this.m_yOffset + this.m_LastOffset;
            num -= 0x43;
            if (num > 90)
            {
                this.m_LastHeight = num - 90;
            }
            else
            {
                this.m_LastHeight = -1;
            }
        }

        public void UpdateTotal()
        {
            this.m_Total.Text = this.m_Owner.ComputeTotalCost().ToString();
        }

        public void WriteSignature()
        {
            if (this.m_Signature != null)
            {
                this.m_Signature.Visible = true;
                this.m_SignatureAnimation = new TimeSync(0.5);
            }
        }

        public Clipper ContentClipper
        {
            get
            {
                return this.m_ContentClipper;
            }
        }

        public int yOffset
        {
            get
            {
                return this.m_yOffset;
            }
            set
            {
                this.m_yOffset = value;
            }
        }
    }
}

