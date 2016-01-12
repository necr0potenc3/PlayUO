namespace Client
{
    public class GBuyGump_OfferedItem : GRegion
    {
        private GLabel m_Amount;
        private GLabel m_Description;
        private GBuyGump_AmountButton m_Less;
        private GBuyGump_AmountButton m_More;
        private GBuyGump_OfferMenu m_OfferMenu;
        private int m_xAmount;

        public GBuyGump_OfferedItem(GBuyGump owner, BuyInfo bi) : base(0x20, 0x43, 0xc4, 0)
        {
            this.m_OfferMenu = owner.OfferMenu;
            IFont uniFont = Engine.GetUniFont(3);
            IHue hue = Hues.Load(0x288);
            this.m_xAmount = bi.ToBuy;
            this.m_Amount = new GLabel(bi.ToBuy.ToString(), uniFont, hue, 0, 0);
            this.m_Description = new GWrappedLabel(string.Format("{0} at {1} gp", bi.Name, bi.Price), uniFont, hue, 0x29, 0, 0x69);
            this.m_More = new GBuyGump_AmountButton(owner, bi, 5, 0x37, 0x9b);
            this.m_Less = new GBuyGump_AmountButton(owner, bi, -5, 0x38, 0xad);
            int height = (this.m_Amount.Image.yMax - this.m_Amount.Image.yMin) + 1;
            base.m_Height = height;
            height = (this.m_Description.Image.yMax - this.m_Description.Image.yMin) + 1;
            if (height > base.m_Height)
            {
                base.m_Height = height;
            }
            height = this.m_More.Height;
            if (height > base.m_Height)
            {
                base.m_Height = height;
            }
            height = this.m_Less.Height;
            if (height > base.m_Height)
            {
                base.m_Height = height;
            }
            this.m_Amount.X -= this.m_Amount.Image.xMin;
            this.m_Amount.Y = (base.m_Height - ((this.m_Amount.Image.yMax - this.m_Amount.Image.yMin) + 1)) / 2;
            this.m_Description.X -= this.m_Description.Image.xMin;
            this.m_Description.Y = (base.m_Height - ((this.m_Description.Image.yMax - this.m_Description.Image.yMin) + 1)) / 2;
            if (this.m_Amount.Y > this.m_Description.Y)
            {
                this.m_Amount.Y = this.m_Description.Y;
            }
            this.m_Amount.Y -= this.m_Amount.Image.yMin;
            this.m_Description.Y -= this.m_Description.Image.yMin;
            this.m_More.Y = (base.m_Height - this.m_More.Height) / 2;
            this.m_Less.Y = (base.m_Height - this.m_Less.Height) / 2;
            base.m_Children.Add(this.m_Amount);
            base.m_Children.Add(this.m_Description);
            base.m_Children.Add(this.m_More);
            base.m_Children.Add(this.m_Less);
            this.Clipper = this.m_OfferMenu.ContentClipper;
            if (Engine.Features.AOS)
            {
                base.Tooltip = new ItemTooltip(bi.Item);
            }
        }

        protected internal override void Render(int x, int y)
        {
            base.m_Y = this.m_OfferMenu.yOffset;
            this.m_OfferMenu.yOffset += base.m_Height + 2;
            base.Render(x, y);
        }

        public int Amount
        {
            get
            {
                return this.m_xAmount;
            }
            set
            {
                if (this.m_xAmount != value)
                {
                    this.m_xAmount = value;
                    this.m_Amount.Text = this.m_xAmount.ToString();
                    this.m_Amount.X = -this.m_Amount.Image.xMin;
                    this.m_Amount.Y = (base.m_Height - ((this.m_Amount.Image.yMax - this.m_Amount.Image.yMin) + 1)) / 2;
                    int num = this.m_Description.Y + this.m_Description.Image.yMin;
                    if (this.m_Amount.Y > num)
                    {
                        this.m_Amount.Y = num;
                    }
                    this.m_Amount.Y -= this.m_Amount.Image.yMin;
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
                this.m_Amount.Clipper = value;
                this.m_Description.Clipper = value;
                this.m_More.Clipper = value;
                this.m_Less.Clipper = value;
            }
        }
    }
}