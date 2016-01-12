namespace Client
{
    public class GComboBox : GBackground
    {
        protected int m_BackID;
        private GButton m_Button;
        protected int m_Count;
        private GBackground m_Dropdown;
        protected IFont m_Font;
        protected IHue m_HOver;
        protected IHue m_HRegular;
        protected int m_Index;
        protected string[] m_List;
        protected GLabel m_Text;

        public GComboBox(string[] List, int Index, int BackID, int NormalID, int OverID, int PressedID, int X, int Y, int Width, int Height, IFont Font, IHue HRegular, IHue HOver) : base(BackID, Width, Height, X, Y, true)
        {
            this.m_Index = -1;
            this.m_BackID = BackID;
            this.m_Font = Font;
            this.m_HRegular = HRegular;
            this.m_HOver = HOver;
            this.m_Button = new GButton(NormalID, OverID, PressedID, 0, 0, new OnClick(this.OpenList_OnClick));
            base.m_Children.Add(this.m_Button);
            this.m_Button.Center();
            this.m_Button.X = ((base.OffsetX + base.UseWidth) - this.m_Button.Width) - 1;
            this.m_Button.Y++;
            this.List = List;
            this.Index = Index;
        }

        private void OpenList_OnClick(Gump g)
        {
            if (this.m_Dropdown != null)
            {
                Gumps.Destroy(this.m_Dropdown);
            }
            Point point = base.PointToScreen(new Point(0, 0));
            this.m_Dropdown = new GBackground(this.m_BackID, this.Width, (this.m_Count * 20) + (this.Height - base.UseHeight), point.X, point.Y, true);
            this.m_Dropdown.DestroyOnUnfocus = true;
            int offsetY = this.m_Dropdown.OffsetY;
            int num2 = 0;
            for (int i = 0; i < this.m_Count; i++)
            {
                GTextButton toAdd = new GTextButton(this.m_List[i], this.m_Font, this.m_HRegular, this.m_HOver, this.m_Dropdown.OffsetX, offsetY, new OnClick(this.SetIndex_OnClick));
                toAdd.SetTag("Index", i);
                this.m_Dropdown.Children.Add(toAdd);
                offsetY += toAdd.Height;
                if ((toAdd.Width + 3) > num2)
                {
                    num2 = toAdd.Width + 3;
                }
            }
            this.m_Dropdown.Height = offsetY + (this.m_Dropdown.Height - (this.m_Dropdown.OffsetY + this.m_Dropdown.UseHeight));
            num2 += this.m_Dropdown.Width - this.m_Dropdown.UseWidth;
            if (num2 > this.m_Dropdown.Width)
            {
                this.m_Dropdown.Width = num2;
            }
            Gumps.Desktop.Children.Add(this.m_Dropdown);
        }

        private void SetIndex_OnClick(Gump g)
        {
            this.Index = (int)g.GetTag("Index");
            Gumps.Destroy(this.m_Dropdown);
        }

        public int Index
        {
            get
            {
                return this.m_Index;
            }
            set
            {
                if ((value < 0) || (value >= this.m_Count))
                {
                    this.m_Index = -1;
                    if (this.m_Text != null)
                    {
                        this.m_Text.Text = "";
                    }
                }
                else
                {
                    this.m_Index = value;
                    if (this.m_Text != null)
                    {
                        this.m_Text.Text = this.m_List[this.m_Index];
                        this.m_Text.Y = ((base.OffsetY + base.UseHeight) - this.m_Text.Image.yMax) - 2;
                    }
                    else
                    {
                        this.m_Text = new GLabel(this.m_List[this.m_Index], this.m_Font, this.m_HRegular, base.OffsetX, base.OffsetY);
                        this.m_Text.Scissor(0, 0, (this.m_Button.X - base.OffsetX) - 2, this.m_Text.Height);
                        this.m_Text.Y = ((base.OffsetY + base.UseHeight) - this.m_Text.Image.yMax) - 2;
                        base.m_Children.Add(this.m_Text);
                    }
                }
            }
        }

        public string[] List
        {
            get
            {
                return this.m_List;
            }
            set
            {
                this.m_List = value;
                this.m_Count = this.m_List.Length;
            }
        }
    }
}