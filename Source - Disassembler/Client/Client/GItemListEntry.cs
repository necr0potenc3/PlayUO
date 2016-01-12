namespace Client
{
    using System.Windows.Forms;

    public class GItemListEntry : Gump
    {
        private Client.Clipper m_Clipper;
        private bool m_Draw;
        private AnswerEntry m_Entry;
        private int m_Height;
        private IHue m_Hue;
        private Texture m_Image;
        private int m_ImageOffsetX;
        private int m_ImageOffsetY;
        private GItemList m_Owner;
        private int m_Width;
        private int m_xOffset;

        public GItemListEntry(int x, AnswerEntry entry, GItemList owner) : base(x, 0x2d)
        {
            this.m_Entry = entry;
            this.m_Owner = owner;
            int hue = entry.Hue;
            if (hue > 0)
            {
                hue++;
            }
            this.m_Hue = Hues.GetItemHue(entry.ItemID, hue);
            this.m_Image = this.m_Hue.GetItem(entry.ItemID);
            if ((this.m_Image != null) && !this.m_Image.IsEmpty())
            {
                this.m_Draw = true;
                this.m_Height = 0x2f;
                int num2 = (this.m_Image.xMax - this.m_Image.xMin) + 1;
                this.m_Width = 0x2f;
                if (num2 > this.m_Width)
                {
                    this.m_Width = num2;
                }
                this.m_ImageOffsetX = ((this.m_Width - ((this.m_Image.xMax - this.m_Image.xMin) + 1)) / 2) - this.m_Image.xMin;
                this.m_ImageOffsetY = ((this.m_Height - ((this.m_Image.yMax - this.m_Image.yMin) + 1)) / 2) - this.m_Image.yMin;
            }
        }

        protected internal override void Draw(int x, int y)
        {
            if (this.m_Draw && (this.m_Clipper != null))
            {
                this.m_Image.DrawClipped(x + this.m_ImageOffsetX, y + this.m_ImageOffsetY, this.m_Clipper);
            }
        }

        protected internal override bool HitTest(int x, int y)
        {
            return ((this.m_Draw && (this.m_Clipper != null)) && this.m_Clipper.Evaluate(base.PointToScreen(new Point(x, y))));
        }

        protected internal override void OnDoubleClick(int x, int y)
        {
            Network.Send(new PQuestionMenuResponse(this.m_Owner.Serial, this.m_Owner.MenuID, this.m_Entry.Index, this.m_Entry.ItemID, this.m_Entry.Hue));
            Gumps.Destroy(this.m_Owner);
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            this.m_Owner.BringToTop();
        }

        protected internal override void OnMouseEnter(int x, int y, MouseButtons mb)
        {
            this.m_Image = Hues.Load(0x8035).GetItem(this.m_Entry.ItemID);
            this.m_Draw = (this.m_Image != null) && !this.m_Image.IsEmpty();
            this.m_Owner.EntryLabel.Text = this.m_Entry.Text;
        }

        protected internal override void OnMouseLeave()
        {
            this.m_Image = this.m_Hue.GetItem(this.m_Entry.ItemID);
            this.m_Draw = (this.m_Image != null) && !this.m_Image.IsEmpty();
            this.m_Owner.EntryLabel.Text = "";
        }

        public Client.Clipper Clipper
        {
            get
            {
                return this.m_Clipper;
            }
            set
            {
                this.m_Clipper = value;
            }
        }

        public override int Height
        {
            get
            {
                return this.m_Height;
            }
        }

        public override int Width
        {
            get
            {
                return this.m_Width;
            }
        }

        public override int X
        {
            get
            {
                return (base.m_X + this.m_xOffset);
            }
        }

        public int xOffset
        {
            get
            {
                return this.m_xOffset;
            }
            set
            {
                this.m_xOffset = value;
            }
        }
    }
}