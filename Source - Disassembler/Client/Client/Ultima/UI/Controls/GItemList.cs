namespace Client
{
    using System.Windows.Forms;

    public class GItemList : GDragable
    {
        private GLabel m_EntryLabel;
        private GHitspot m_Left;
        private int m_MenuID;
        private GHitspot m_Right;
        private int m_Serial;
        private int m_xLast;
        private double m_xOffset;
        private double m_xOffsetCap;
        private int m_yLast;

        public GItemList(int serial, int menuID, string question, AnswerEntry[] answers) : base(0x910, 0x19, 0x19)
        {
            this.m_xLast = -1234;
            this.m_yLast = -1234;
            this.m_Serial = serial;
            this.m_MenuID = menuID;
            this.m_EntryLabel = new GLabel("", Engine.GetFont(1), Hues.Load(0x75f), 0x27, 0x6a);
            base.m_Children.Add(this.m_EntryLabel);
            GLabel toAdd = new GLabel(question, Engine.GetFont(1), Hues.Load(0x75f), 0x27, 0x13);
            toAdd.Scissor(0, 0, 0xda, 11);
            base.m_Children.Add(toAdd);
            int x = 0x25;
            for (int i = 0; i < answers.Length; i++)
            {
                GItemListEntry entry = new GItemListEntry(x, answers[i], this);
                base.m_Children.Add(entry);
                x += entry.Width;
            }
            x -= 0x25;
            if (x >= 0xde)
            {
                this.m_xOffsetCap = -(x - 0xde);
                this.m_Left = new GItemListScroller(0x17, this, 150);
                this.m_Right = new GItemListScroller(0x105, this, -150);
                base.m_Children.Add(this.m_Left);
                base.m_Children.Add(this.m_Right);
            }
        }

        protected internal override void Draw(int x, int y)
        {
            base.Draw(x, y);
            if ((this.m_xLast != x) || (this.m_yLast != y))
            {
                this.m_xLast = x;
                this.m_yLast = y;
                Clipper clipper = new Clipper(x + 0x25, y + 0x2d, 0xde, 0x2f);
                for (int i = 0; i < base.m_Children.Count; i++)
                {
                    if (base.m_Children[i] is GItemListEntry)
                    {
                        ((GItemListEntry)base.m_Children[i]).Clipper = clipper;
                    }
                }
                this.m_EntryLabel.Scissor(new Clipper(x + 0x27, y + 0x6a, 0xda, 11));
            }
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            base.BringToTop();
        }

        protected internal override void OnMouseUp(int x, int y, MouseButtons mb)
        {
            if ((mb & MouseButtons.Right) != MouseButtons.None)
            {
                Network.Send(new PQuestionMenuCancel(this.m_Serial, this.m_MenuID));
                Gumps.Destroy(this);
            }
        }

        public GLabel EntryLabel
        {
            get
            {
                return this.m_EntryLabel;
            }
        }

        public int MenuID
        {
            get
            {
                return this.m_MenuID;
            }
        }

        public int Serial
        {
            get
            {
                return this.m_Serial;
            }
        }

        public double xOffset
        {
            get
            {
                return this.m_xOffset;
            }
            set
            {
                if (value > 0.0)
                {
                    value = 0.0;
                }
                else if (value < this.m_xOffsetCap)
                {
                    value = this.m_xOffsetCap;
                }
                this.m_xOffset = value;
                int num = (int)value;
                for (int i = 0; i < base.m_Children.Count; i++)
                {
                    if (base.m_Children[i] is GItemListEntry)
                    {
                        ((GItemListEntry)base.m_Children[i]).xOffset = num;
                    }
                }
            }
        }
    }
}