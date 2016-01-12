namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GWindowsButton : Gump, IClickable
    {
        protected bool m_CanEnter;
        private bool m_CaptionDown;
        protected GLabel m_CaptionLabel;
        protected bool m_Enabled;
        protected int m_Height;
        protected Texture m_Image;
        protected int m_ImageColor;
        protected bool m_Invalidated;
        protected Client.OnClick m_OnClick;
        protected int m_State;
        protected WindowsButtonStyle m_Style;
        protected VertexCache m_vCache;
        protected int m_Width;

        public event EventHandler Clicked;

        public GWindowsButton(string text, int x, int y) : this(text, x, y, 100, 20)
        {
        }

        public GWindowsButton(string text, int x, int y, int width, int height) : base(x, y)
        {
            this.m_ImageColor = -1;
            this.m_Width = width;
            this.m_Height = height;
            this.m_Enabled = true;
            this.m_CaptionLabel = new GLabel(text, Engine.GetUniFont(2), GumpHues.ControlText, 5, 5);
            base.m_Children.Add(this.m_CaptionLabel);
            this.m_CaptionLabel.Center();
        }

        public void Click()
        {
            for (int i = 1; i <= 3; i++)
            {
                this.State = i % 3;
                Engine.DrawNow();
            }
            this.InternalOnClicked();
        }

        protected internal override void Draw(int x, int y)
        {
            Renderer.SetTexture(null);
            int num = 0;
            switch (this.m_Style)
            {
                case WindowsButtonStyle.Normal:
                    switch (this.m_State)
                    {
                        case 0:
                            GumpPaint.DrawRaised3D(x, y, this.m_Width, this.m_Height);
                            this.CaptionDown = false;
                            goto Label_0143;

                        case 1:
                            GumpPaint.DrawRaised3D(x, y, this.m_Width, this.m_Height);
                            this.CaptionDown = false;
                            goto Label_0143;

                        case 2:
                            GumpPaint.DrawFlat(x, y, this.m_Width, this.m_Height, GumpColors.ControlDark, GumpColors.Control);
                            this.CaptionDown = true;
                            num = 1;
                            goto Label_0143;
                    }
                    break;

                case WindowsButtonStyle.Flat:
                    switch (this.m_State)
                    {
                        case 0:
                            GumpPaint.DrawFlat(x, y, this.m_Width, this.m_Height, GumpColors.ControlDarkDark, GumpColors.Control);
                            this.CaptionDown = false;
                            goto Label_0143;

                        case 1:
                            GumpPaint.DrawFlat(x, y, this.m_Width, this.m_Height, GumpColors.ControlDarkDark, GumpColors.ControlAlternate);
                            this.CaptionDown = false;
                            goto Label_0143;

                        case 2:
                            GumpPaint.DrawFlat(x, y, this.m_Width, this.m_Height, GumpColors.ControlDarkDark, GumpPaint.Blend(GumpColors.ControlAlternate, GumpColors.ControlLightLight, 0x80));
                            this.CaptionDown = false;
                            goto Label_0143;
                    }
                    break;
            }
        Label_0143:
            if (this.m_Image != null)
            {
                if (this.m_vCache == null)
                {
                    this.m_vCache = new VertexCache();
                }
                if (this.m_ImageColor == -1)
                {
                    this.m_vCache.Draw(this.m_Image, ((num + x) + ((this.m_Width - ((this.m_Image.xMax - this.m_Image.xMin) + 1)) / 2)) - this.m_Image.xMin, ((num + y) + ((this.m_Height - ((this.m_Image.yMax - this.m_Image.yMin) + 1)) / 2)) - this.m_Image.yMin);
                }
                else
                {
                    Renderer.SetAlphaEnable(true);
                    Renderer.SetAlpha(1f);
                    this.m_vCache.Draw(this.m_Image, ((num + x) + ((this.m_Width - ((this.m_Image.xMax - this.m_Image.xMin) + 1)) / 2)) - this.m_Image.xMin, ((num + y) + ((this.m_Height - ((this.m_Image.yMax - this.m_Image.yMin) + 1)) / 2)) - this.m_Image.yMin, this.m_ImageColor);
                    Renderer.SetAlphaEnable(false);
                }
            }
        }

        protected internal override bool HitTest(int x, int y)
        {
            return true;
        }

        private void InternalOnClicked()
        {
            this.OnClicked();
            if (this.Clicked != null)
            {
                this.Clicked(this, EventArgs.Empty);
            }
            if (this.m_OnClick != null)
            {
                this.m_OnClick(this);
            }
        }

        protected virtual void OnClicked()
        {
        }

        protected internal override bool OnKeyDown(char c)
        {
            if (this.m_CanEnter && (c == '\r'))
            {
                this.Click();
                return true;
            }
            return false;
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            this.OnMouseEnter(x, y, mb);
        }

        protected internal override void OnMouseEnter(int x, int y, MouseButtons mb)
        {
            if (this.m_Enabled)
            {
                this.State = ((mb & MouseButtons.Left) != MouseButtons.None) ? 2 : 1;
            }
        }

        protected internal override void OnMouseLeave()
        {
            if (this.m_Enabled)
            {
                this.State = 0;
            }
        }

        protected internal override void OnMouseUp(int x, int y, MouseButtons mb)
        {
            if (this.m_Enabled && ((mb & MouseButtons.Left) != MouseButtons.None))
            {
                this.State = 1;
                this.InternalOnClicked();
            }
        }

        public bool CanEnter
        {
            get
            {
                return this.m_CanEnter;
            }
            set
            {
                this.m_CanEnter = value;
            }
        }

        public bool CaptionDown
        {
            get
            {
                return this.m_CaptionDown;
            }
            set
            {
                if (this.m_CaptionDown != value)
                {
                    this.m_CaptionDown = value;
                    int num = this.m_CaptionDown ? 1 : -1;
                    this.m_CaptionLabel.X += num;
                    this.m_CaptionLabel.Y += num;
                }
            }
        }

        public bool Enabled
        {
            get
            {
                return this.m_Enabled;
            }
            set
            {
                if (this.m_Enabled != value)
                {
                    this.m_Enabled = value;
                    if (!this.m_Enabled)
                    {
                        this.State = 0;
                    }
                }
            }
        }

        public override int Height
        {
            get
            {
                return this.m_Height;
            }
            set
            {
                this.m_Height = value;
            }
        }

        public Texture Image
        {
            get
            {
                return this.m_Image;
            }
            set
            {
                this.m_Image = value;
            }
        }

        public int ImageColor
        {
            get
            {
                return this.m_ImageColor;
            }
            set
            {
                this.m_ImageColor = value;
                if (this.m_vCache != null)
                {
                    this.m_vCache.Invalidate();
                }
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

        public int State
        {
            get
            {
                return this.m_State;
            }
            set
            {
                this.m_State = value;
            }
        }

        public WindowsButtonStyle Style
        {
            get
            {
                return this.m_Style;
            }
            set
            {
                this.m_Style = value;
            }
        }

        public string Text
        {
            get
            {
                return this.m_CaptionLabel.Text;
            }
            set
            {
                this.CaptionDown = false;
                this.m_CaptionLabel.Text = value;
                this.m_CaptionLabel.Center();
            }
        }

        public override int Width
        {
            get
            {
                return this.m_Width;
            }
            set
            {
                this.m_Width = value;
            }
        }
    }
}