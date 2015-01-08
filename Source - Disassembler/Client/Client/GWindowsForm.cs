namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GWindowsForm : Gump
    {
        private GLabel m_CaptionLabel;
        private Gump m_Client;
        private GWindowsButton m_CloseButton;
        private int m_Height;
        private int m_Width;

        public GWindowsForm() : this(0, 0, 200, 200)
        {
        }

        public GWindowsForm(int width, int height) : this(0, 0, width, height)
        {
        }

        public GWindowsForm(int x, int y, int width, int height) : base(x, y)
        {
            base.m_CanDrag = true;
            base.m_QuickDrag = true;
            this.m_Width = width;
            this.m_Height = height;
            this.m_CaptionLabel = new GLabel("Form", Engine.GetUniFont(1), Hues.Default, 7, 3);
            base.m_Children.Add(this.m_CaptionLabel);
            this.m_Client = new GEmpty(0, 0, 0, 0);
            base.m_Children.Add(this.m_Client);
            this.m_CloseButton = new GWindowsButton("", 0, 0, 0x10, 14);
            this.m_CloseButton.ImageColor = 0;
            this.m_CloseButton.Image = Engine.m_FormX;
            this.m_CloseButton.Clicked += new EventHandler(this.CloseButton_Clicked);
            base.m_Children.Add(this.m_CloseButton);
            this.ResizeClient();
        }

        public virtual void Close()
        {
            Gumps.Destroy(this);
        }

        private void CloseButton_Clicked(object sender, EventArgs e)
        {
            this.Close();
        }

        protected internal override void Draw(int X, int Y)
        {
            Renderer.SetTexture(null);
            GumpPaint.DrawRaised3D(X, Y, this.m_Width, this.m_Height);
            Gump focus = Gumps.Focus;
            if ((focus == this) || ((focus != null) && focus.IsChildOf(this)))
            {
                Renderer.GradientRectLR(GumpColors.ActiveCaption, GumpColors.ActiveCaptionGradient, X + 4, Y + 4, this.Width - 8, 0x12);
                this.m_CaptionLabel.Hue = GumpHues.ActiveCaptionText;
            }
            else
            {
                Renderer.GradientRectLR(GumpColors.InactiveCaption, GumpColors.InactiveCaptionGradient, X + 4, Y + 4, this.Width - 8, 0x12);
                this.m_CaptionLabel.Hue = GumpHues.InactiveCaptionText;
            }
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return true;
        }

        protected internal override void OnDragStart()
        {
            if (base.m_OffsetY > 0x15)
            {
                base.m_IsDragging = false;
                Gumps.Drag = null;
            }
            base.BringToTop();
        }

        protected internal override void OnMouseDown(int X, int Y, MouseButtons mb)
        {
            base.BringToTop();
        }

        public virtual void Resize()
        {
            this.ResizeClient();
        }

        public virtual void ResizeClient()
        {
            this.m_Client.X = 4;
            this.m_Client.Y = 0x17;
            this.m_Client.Width = this.Width - 8;
            this.m_Client.Height = this.Height - 0x1b;
            this.m_CloseButton.X = (this.Width - 6) - this.m_CloseButton.Width;
            this.m_CloseButton.Y = 6;
        }

        public Gump Client
        {
            get
            {
                return this.m_Client;
            }
        }

        public GWindowsButton CloseButton
        {
            get
            {
                return this.m_CloseButton;
            }
            set
            {
                this.m_CloseButton = value;
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
                this.Resize();
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
                this.m_CaptionLabel.Text = value;
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
                this.Resize();
            }
        }
    }
}

