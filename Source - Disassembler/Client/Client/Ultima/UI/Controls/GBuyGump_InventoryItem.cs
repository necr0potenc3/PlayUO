namespace Client
{
    using System.Windows.Forms;

    public class GBuyGump_InventoryItem : GRegion
    {
        private GLabel m_Available;
        private GLabel m_Description;
        private GItemArt m_Image;
        private BuyInfo m_Info;
        private GBuyGump m_Owner;
        private bool m_Selected;
        private GImage[] m_Separator;
        private int m_xAvailable;
        private int m_yBase;

        public GBuyGump_InventoryItem(GBuyGump owner, BuyInfo si, int y, bool seperate) : base(0x20, y, 0xc3, 0)
        {
            this.m_Owner = owner;
            this.m_yBase = y;
            this.m_Info = si;
            Client.IFont uniFont = Engine.GetUniFont(3);
            IHue hue = Hues.Load(0x288);
            this.m_Image = new GItemArt(0, 0, si.ItemID, si.Hue);
            this.m_Description = new GWrappedLabel(string.Format("{0} at {1} gp", si.Name, si.Price), uniFont, hue, 0x3a, 0, 0x69);
            this.m_Available = new GLabel(si.Amount.ToString(), uniFont, hue, 0xc3, 0);
            int num = (this.m_Image.Image.yMax - this.m_Image.Image.yMin) + 1;
            base.m_Height = num;
            num = (this.m_Description.Image.yMax - this.m_Description.Image.yMin) + 1;
            if (num > base.m_Height)
            {
                base.m_Height = num;
            }
            num = (this.m_Available.Image.yMax - this.m_Available.Image.yMin) + 1;
            if (num > base.m_Height)
            {
                base.m_Height = num;
            }
            this.m_Image.X += (0x38 - ((this.m_Image.Image.xMax - this.m_Image.Image.xMin) + 1)) / 2;
            this.m_Image.Y += (base.m_Height - ((this.m_Image.Image.yMax - this.m_Image.Image.yMin) + 1)) / 2;
            this.m_Image.X -= this.m_Image.Image.xMin;
            this.m_Image.Y -= this.m_Image.Image.yMin;
            this.m_Description.X -= this.m_Description.Image.xMin;
            this.m_Description.Y += (base.m_Height - ((this.m_Description.Image.yMax - this.m_Description.Image.yMin) + 1)) / 2;
            this.m_Description.Y -= this.m_Description.Image.yMin;
            this.m_Available.X -= this.m_Available.Image.xMax + 1;
            this.m_Available.Y += (base.m_Height - ((this.m_Available.Image.yMax - this.m_Available.Image.yMin) + 1)) / 2;
            this.m_Available.Y -= this.m_Available.Image.yMin;
            base.m_Children.Add(this.m_Image);
            base.m_Children.Add(this.m_Description);
            base.m_Children.Add(this.m_Available);
            this.m_xAvailable = si.Amount;
            if (seperate)
            {
                GImage image;
                this.m_Separator = new GImage[11];
                this.m_Separator[0] = image = new GImage(0x39, 0, base.m_Height);
                base.m_Children.Add(image);
                for (int i = 0; i < 9; i++)
                {
                    this.m_Separator[i + 1] = image = new GImage(0x3a, 30 + (i * 0x10), base.m_Height);
                    base.m_Children.Add(image);
                }
                this.m_Separator[10] = image = new GImage(0x3b, 0xa5, base.m_Height);
                base.m_Children.Add(image);
            }
            else
            {
                this.m_Separator = new GImage[0];
            }
            if (Engine.Features.AOS)
            {
                base.Tooltip = new ItemTooltip(si.Item);
            }
        }

        protected internal override void OnDoubleClick(int x, int y)
        {
            if (this.m_Info.ToBuy < this.m_Info.Amount)
            {
                if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
                {
                    this.m_Info.ToBuy = this.m_Info.Amount;
                }
                else
                {
                    this.m_Info.ToBuy++;
                }
                if (this.m_Info.OfferedGump == null)
                {
                    this.m_Info.OfferedGump = new GBuyGump_OfferedItem(this.m_Owner, this.m_Info);
                    this.m_Owner.OfferMenu.Children.Add(this.m_Info.OfferedGump);
                }
                this.m_Info.OfferedGump.Amount = this.m_Info.ToBuy;
                this.Available = this.m_Info.Amount - this.m_Info.ToBuy;
                this.m_Owner.OfferMenu.UpdateTotal();
            }
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            if ((mb & MouseButtons.Left) != MouseButtons.None)
            {
                this.m_Owner.Selected = this.m_Info;
            }
            if ((mb & MouseButtons.Right) != MouseButtons.None)
            {
                Gumps.Destroy(this.m_Owner);
            }
        }

        public int Available
        {
            get
            {
                return this.m_xAvailable;
            }
            set
            {
                if (this.m_xAvailable != value)
                {
                    this.m_xAvailable = value;
                    this.m_Available.Text = value.ToString();
                    this.m_Available.X = (0xc3 - this.m_Available.Image.xMax) - 1;
                    this.m_Available.Y = ((base.m_Height - ((this.m_Available.Image.yMax - this.m_Available.Image.yMin) + 1)) / 2) - this.m_Available.Image.yMin;
                }
            }
        }

        public override Client.Clipper Clipper
        {
            get
            {
                return base.m_Clipper;
            }
            set
            {
                base.m_Clipper = value;
                this.m_Image.Clipper = value;
                this.m_Description.Clipper = value;
                this.m_Available.Clipper = value;
                for (int i = 0; i < this.m_Separator.Length; i++)
                {
                    this.m_Separator[i].Clipper = value;
                }
            }
        }

        public int Offset
        {
            set
            {
                base.m_Y = this.m_yBase + value;
            }
        }

        public bool Selected
        {
            get
            {
                return this.m_Selected;
            }
            set
            {
                if (this.m_Selected != value)
                {
                    this.m_Selected = value;
                    this.m_Description.Hue = this.m_Available.Hue = Hues.Load(value ? 0x66c : 0x288);
                }
            }
        }
    }
}