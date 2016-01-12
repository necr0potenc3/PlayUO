namespace Client
{
    using System;

    public class GListBox : GBackground
    {
        protected int m_Count;
        protected IFont m_Font;
        protected IHue m_HOver;
        protected IHue m_HRegular;
        protected int m_Index;
        protected int m_ItemCount;
        protected Client.OnClick m_OnClick;
        private static Type tGListItem = typeof(GListItem);

        public GListBox(IFont Font, IHue HRegular, IHue HOver, int BackID, int X, int Y, int Width, int Height, bool HasBorder) : base(BackID, Width, Height, X, Y, HasBorder)
        {
            this.m_Font = Font;
            this.m_HRegular = HRegular;
            this.m_HOver = HOver;
            this.m_ItemCount = base.UseHeight / 0x12;
        }

        public void AddItem(string Text)
        {
            base.m_Children.Add(new GListItem(Text, this.m_Count++, this));
        }

        public void AddItem(string Text, Tooltip t)
        {
            GListItem toAdd = new GListItem(Text, this.m_Count++, this);
            toAdd.Tooltip = t;
            base.m_Children.Add(toAdd);
        }

        protected internal void OnListItemClick(GListItem who)
        {
            if (this.m_OnClick != null)
            {
                base.SetTag("Clicked", who);
                this.m_OnClick(this);
            }
        }

        public int Count
        {
            get
            {
                return this.m_Count;
            }
        }

        public IFont Font
        {
            get
            {
                return this.m_Font;
            }
        }

        public IHue HOver
        {
            get
            {
                return this.m_HOver;
            }
        }

        public IHue HRegular
        {
            get
            {
                return this.m_HRegular;
            }
        }

        public int ItemCount
        {
            get
            {
                return this.m_ItemCount;
            }
        }

        public Client.OnClick OnClick
        {
            get
            {
                return this.m_OnClick;
            }
            set
            {
                this.m_OnClick = value;
            }
        }

        public int StartIndex
        {
            get
            {
                return this.m_Index;
            }
            set
            {
                if (this.m_Index != value)
                {
                    this.m_Index = value;
                    Gump[] gumpArray = base.m_Children.ToArray();
                    for (int i = 0; i < gumpArray.Length; i++)
                    {
                        Gump gump = gumpArray[i];
                        if (gump.GetType() == tGListItem)
                        {
                            ((GListItem)base.m_Children[i]).Layout();
                        }
                    }
                }
            }
        }
    }
}