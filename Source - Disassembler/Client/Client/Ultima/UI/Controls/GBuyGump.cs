namespace Client
{
    using System;

    public class GBuyGump : GDragable
    {
        private BuyInfo[] m_Info;
        private GBuyGump_OfferMenu m_OfferMenu;
        private BuyInfo m_Selected;
        private int m_Serial;
        private int m_xLast;
        private int m_yLast;

        public GBuyGump(int serial, BuyInfo[] info) : base(0x870, 15, 15)
        {
            base.m_GUID = string.Format("GBuyGump-{0}", serial);
            this.m_Serial = serial;
            this.m_Info = info;
            UnicodeFont uniFont = Engine.GetUniFont(3);
            IHue hue = Hues.Load(0x288);
            Array.Sort(info);
            int y = 0x42;
            for (int i = 0; i < info.Length; i++)
            {
                bool seperate = i != (info.Length - 1);
                BuyInfo si = info[i];
                GBuyGump_InventoryItem toAdd = new GBuyGump_InventoryItem(this, si, y, seperate);
                base.m_Children.Add(toAdd);
                si.InventoryGump = toAdd;
                y += toAdd.Height;
                if (seperate)
                {
                    y += 0x10;
                }
            }
            if (y > 230)
            {
                GVSlider slider = new GVSlider(0x828, 0xed, 0x51, 0x22, 0x5c, 0.0, 0.0, (double)(y - 230), 1.0);
                slider.OnValueChange = new OnValueChange(this.Slider_OnValueChange);
                base.m_Children.Add(slider);
                base.m_Children.Add(new GHotspot(0xed, 0x42, 0x22, 0x7a, slider));
            }
            base.m_NonRestrictivePicking = true;
            this.m_OfferMenu = new GBuyGump_OfferMenu(this);
            base.m_Children.Add(this.m_OfferMenu);
            base.m_X = (Engine.ScreenWidth - (this.m_OfferMenu.X + this.m_OfferMenu.Width)) / 2;
            base.m_Y = (Engine.ScreenHeight - (this.m_OfferMenu.Y + this.m_OfferMenu.Height)) / 2;
        }

        public void Accept()
        {
            Network.Send(new PBuyItems(this.m_Serial, this.m_Info));
            this.m_OfferMenu.WriteSignature();
        }

        public void Clear()
        {
            for (int i = 0; i < this.m_Info.Length; i++)
            {
                BuyInfo info = this.m_Info[i];
                if (info.ToBuy > 0)
                {
                    info.ToBuy = 0;
                    info.InventoryGump.Available = info.Amount;
                    Gumps.Destroy(info.OfferedGump);
                    info.OfferedGump = null;
                }
            }
        }

        public int ComputeTotalCost()
        {
            int num = 0;
            for (int i = 0; i < this.m_Info.Length; i++)
            {
                num += this.m_Info[i].ToBuy * this.m_Info[i].Price;
            }
            return num;
        }

        protected internal override void Draw(int x, int y)
        {
            if ((this.m_xLast != x) || (this.m_yLast != y))
            {
                this.m_xLast = x;
                this.m_yLast = y;
                Clipper clipper = new Clipper(x + 0x1f, y + 0x3b, 0xc5, 0xb1);
                foreach (Gump gump in base.m_Children.ToArray())
                {
                    if (gump is GBuyGump_InventoryItem)
                    {
                        ((GBuyGump_InventoryItem)gump).Clipper = clipper;
                    }
                }
            }
            base.Draw(x, y);
        }

        private void Slider_OnValueChange(double v, double o, Gump slider)
        {
            Gump[] gumpArray = base.m_Children.ToArray();
            int num = -((int)v);
            for (int i = 0; i < gumpArray.Length; i++)
            {
                Gump gump = gumpArray[i];
                if (gump is GBuyGump_InventoryItem)
                {
                    ((GBuyGump_InventoryItem)gump).Offset = num;
                }
            }
        }

        public GBuyGump_OfferMenu OfferMenu
        {
            get
            {
                return this.m_OfferMenu;
            }
        }

        public BuyInfo Selected
        {
            get
            {
                return this.m_Selected;
            }
            set
            {
                if (this.m_Selected != value)
                {
                    if (this.m_Selected != null)
                    {
                        this.m_Selected.InventoryGump.Selected = false;
                    }
                    this.m_Selected = value;
                    this.m_Selected.InventoryGump.Selected = true;
                }
            }
        }
    }
}