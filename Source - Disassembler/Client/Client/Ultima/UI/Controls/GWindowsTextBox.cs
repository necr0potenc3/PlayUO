namespace Client
{
    using System.Windows.Forms;

    public class GWindowsTextBox : Gump
    {
        protected int m_Height;
        protected WindowsTextBoxStyle m_Style;
        protected GTextBox m_TextBox;
        protected int m_Width;

        public GWindowsTextBox(int x, int y, int width, int height, string initialText, Client.IFont font, IHue normalHue, IHue overHue, IHue focusHue, char passChar) : base(x, y)
        {
            this.m_Width = width;
            this.m_Height = height;
            this.m_TextBox = new GTextBox(0, false, 3, 3, width - 6, height - 6, initialText, font, normalHue, overHue, focusHue, passChar);
            this.Layout();
            base.m_Children.Add(this.m_TextBox);
        }

        protected internal override void Draw(int x, int y)
        {
            Renderer.SetTexture(null);
            switch (this.m_Style)
            {
                case WindowsTextBoxStyle.Normal:
                    GumpPaint.DrawSunken3D(x, y, this.m_Width, this.m_Height, GumpColors.Window);
                    break;

                case WindowsTextBoxStyle.Flat:
                    GumpPaint.DrawFlat(x, y, this.m_Width, this.m_Height, GumpColors.ControlDarkDark, GumpColors.Window);
                    break;
            }
        }

        protected internal override bool HitTest(int x, int y)
        {
            return true;
        }

        public virtual void Layout()
        {
            int num = 0;
            switch (this.m_Style)
            {
                case WindowsTextBoxStyle.Normal:
                    num = 3;
                    break;

                case WindowsTextBoxStyle.Flat:
                    num = 2;
                    break;
            }
            this.m_TextBox.X = 0;
            this.m_TextBox.Y = num * 2;
            this.m_TextBox.Width = this.m_Width;
            this.m_TextBox.Height = this.m_Height;
        }

        protected internal override void OnDoubleClick(int X, int Y)
        {
            this.m_TextBox.OnDoubleClick(X - this.m_TextBox.X, Y - this.m_TextBox.Y);
        }

        protected internal override void OnMouseDown(int X, int Y, MouseButtons mb)
        {
            this.m_TextBox.OnMouseDown(X - this.m_TextBox.X, Y - this.m_TextBox.Y, mb);
        }

        protected internal override void OnMouseEnter(int X, int Y, MouseButtons mb)
        {
            this.m_TextBox.OnMouseEnter(X - this.m_TextBox.X, Y - this.m_TextBox.Y, mb);
        }

        protected internal override void OnMouseLeave()
        {
            this.m_TextBox.OnMouseLeave();
        }

        protected internal override void OnMouseMove(int X, int Y, MouseButtons mb)
        {
            this.m_TextBox.OnMouseMove(X - this.m_TextBox.X, Y - this.m_TextBox.Y, mb);
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            this.m_TextBox.OnMouseUp(X - this.m_TextBox.X, Y - this.m_TextBox.Y, mb);
        }

        protected internal override void OnMouseWheel(int Delta)
        {
            this.m_TextBox.OnMouseWheel(Delta);
        }

        protected internal override void OnSingleClick(int X, int Y)
        {
            this.m_TextBox.OnSingleClick(X - this.m_TextBox.X, Y - this.m_TextBox.Y);
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
                this.Layout();
            }
        }

        public WindowsTextBoxStyle Style
        {
            get
            {
                return this.m_Style;
            }
            set
            {
                this.m_Style = value;
                this.Layout();
            }
        }

        public GTextBox TextBox
        {
            get
            {
                return this.m_TextBox;
            }
            set
            {
                this.m_TextBox = value;
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
                this.Layout();
            }
        }
    }
}