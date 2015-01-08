namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GTextBox : Gump
    {
        private GBackground m_Back;
        private IClickable m_EnterButton;
        private bool m_Focus;
        private IFont m_Font;
        private int m_Height;
        private IHue m_HFocus;
        private IHue m_HNormal;
        private IHue m_HOver;
        private int m_MaxChars;
        private Client.OnBeforeTextChange m_OnBeforeTextChange;
        private Client.OnTextChange m_OnTextChange;
        private char m_PassChar;
        private string m_String;
        private GLabel m_Text;
        private bool m_Transparent;
        private int m_Width;

        public GTextBox(int GumpID, bool HasBorder, int X, int Y, int Width, int Height, string StartText, IFont f, IHue HNormal, IHue HOver, IHue HFocus) : this(GumpID, HasBorder, X, Y, Width, Height, StartText, f, HNormal, HOver, HFocus, '\0')
        {
        }

        public GTextBox(int GumpID, bool HasBorder, int X, int Y, int Width, int Height, string StartText, IFont f, IHue HNormal, IHue HOver, IHue HFocus, char PassChar) : base(X, Y)
        {
            this.m_String = "";
            this.m_MaxChars = -1;
            this.m_Transparent = GumpID == 0;
            this.m_Back = new GBackground(GumpID, Width, Height, HasBorder);
            this.m_PassChar = PassChar;
            this.m_String = StartText;
            this.m_Font = f;
            this.m_HNormal = HNormal;
            this.m_HFocus = HFocus;
            this.m_HOver = HOver;
            this.m_Width = Width;
            this.m_Height = Height;
            base.m_OverCursor = 14;
            this.UpdateLabel(false, this.m_HNormal);
        }

        protected internal override void Draw(int X, int Y)
        {
            this.m_Back.Draw(X, Y);
            if (this.m_MaxChars == -1)
            {
                this.m_Text.Draw((X + this.m_Back.OffsetX) + 3, (((Y + this.m_Back.Height) - this.m_Text.Height) - this.m_Back.OffsetY) - 1);
            }
            else
            {
                this.m_Text.Draw((X + this.m_Back.OffsetX) - 1, Y + this.m_Back.OffsetY);
            }
        }

        internal virtual void Focus()
        {
            this.m_Focus = true;
            this.UpdateLabel(false, this.m_HFocus);
            Gumps.TextFocus = this;
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return (this.m_Transparent || this.m_Back.HitTest(X, Y));
        }

        protected internal override bool OnKeyDown(char c)
        {
            if (c == '\b')
            {
                if (this.m_String.Length > 0)
                {
                    if (this.m_OnBeforeTextChange != null)
                    {
                        this.m_OnBeforeTextChange(this);
                    }
                    if (this.m_String.Length > 0)
                    {
                        this.m_String = this.m_String.Substring(0, this.m_String.Length - 1);
                        if (this.m_OnTextChange != null)
                        {
                            this.m_OnTextChange(this.m_String, this);
                        }
                    }
                }
            }
            else
            {
                if (c == '\r')
                {
                    if (this.m_EnterButton == null)
                    {
                        return false;
                    }
                    this.m_EnterButton.Click();
                    return true;
                }
                if (c == '\t')
                {
                    Gumps.TextBoxTab(this);
                    return true;
                }
                if (c < ' ')
                {
                    return false;
                }
                if (this.m_OnBeforeTextChange != null)
                {
                    this.m_OnBeforeTextChange(this);
                }
                if (this.m_MaxChars >= 0)
                {
                    if (this.m_String.Length >= this.m_MaxChars)
                    {
                        return false;
                    }
                }
                else if (((this.m_Font.GetStringWidth((this.m_PassChar == '\0') ? string.Format(this.ShowCaret ? "{0}{1}_" : "{0}{1}", this.m_String, c) : string.Format(this.ShowCaret ? "{0}_" : "{0}", new string(this.m_PassChar, this.m_String.Length + 1))) + 3) + (this.m_Back.OffsetX * 2)) >= this.m_Width)
                {
                    return false;
                }
                this.m_String = this.m_String + c;
                if (this.m_OnTextChange != null)
                {
                    this.m_OnTextChange(this.m_String, this);
                }
            }
            this.UpdateLabel(false, this.m_HFocus);
            return true;
        }

        protected internal override void OnMouseDown(int X, int Y, MouseButtons mb)
        {
            this.m_Focus = true;
            this.UpdateLabel(false, this.m_HFocus);
            Gumps.TextFocus = this;
        }

        protected internal override void OnMouseEnter(int X, int Y, MouseButtons mb)
        {
            if (!this.m_Focus)
            {
                this.UpdateLabel(false, this.m_HOver);
            }
        }

        protected internal override void OnMouseLeave()
        {
            if (!this.m_Focus)
            {
                this.UpdateLabel(false, this.m_HNormal);
            }
        }

        internal virtual void Unfocus()
        {
            this.m_Focus = false;
            this.UpdateLabel(false, this.m_HNormal);
        }

        private void UpdateLabel(bool forceUpdate, IHue hue)
        {
            string text = this.m_String;
            if (this.m_PassChar != '\0')
            {
                text = new string(this.m_PassChar, text.Length);
            }
            if (this.m_Focus && this.ShowCaret)
            {
                text = text + "_";
            }
            if ((this.m_Text != null) && !forceUpdate)
            {
                this.m_Text.Text = text;
                this.m_Text.Hue = hue;
            }
            else
            {
                if (this.m_Text != null)
                {
                    Gumps.Destroy(this.m_Text);
                }
                if (this.m_MaxChars >= 0)
                {
                    this.m_Text = new GWrappedLabel(text, this.m_Font, this.m_HNormal, 0, 0, this.m_Width);
                    this.m_Text.Scissor(0, 0, this.m_Width, this.m_Height);
                }
                else
                {
                    this.m_Text = new GLabel(text, this.m_Font, this.m_HNormal, 0, 0);
                }
            }
        }

        public IClickable EnterButton
        {
            get
            {
                return this.m_EnterButton;
            }
            set
            {
                this.m_EnterButton = value;
            }
        }

        public override int Height
        {
            get
            {
                return this.m_Height;
            }
        }

        public int MaxChars
        {
            get
            {
                return this.m_MaxChars;
            }
            set
            {
                this.m_MaxChars = value;
                this.UpdateLabel(true, this.m_HNormal);
            }
        }

        public Client.OnBeforeTextChange OnBeforeTextChange
        {
            get
            {
                return this.m_OnBeforeTextChange;
            }
            set
            {
                this.m_OnBeforeTextChange = value;
            }
        }

        public Client.OnTextChange OnTextChange
        {
            get
            {
                return this.m_OnTextChange;
            }
            set
            {
                this.m_OnTextChange = value;
            }
        }

        public virtual bool ShowCaret
        {
            get
            {
                return true;
            }
        }

        public string String
        {
            get
            {
                return this.m_String;
            }
            set
            {
                if (this.m_OnBeforeTextChange != null)
                {
                    this.m_OnBeforeTextChange(this);
                }
                this.m_String = value;
                if (this.m_OnTextChange != null)
                {
                    this.m_OnTextChange(this.m_String, this);
                }
                if (this.m_Focus)
                {
                    this.Focus();
                }
                else
                {
                    this.Unfocus();
                }
            }
        }

        public override int Width
        {
            get
            {
                return this.m_Width;
            }
        }
    }
}

