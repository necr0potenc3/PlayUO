namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GTextButton : GLabel
    {
        protected bool m_CanHitTest;
        private IHue[] m_Hues;
        private Client.OnClick m_OnClick;
        private Client.OnHighlight m_OnHighlight;
        private int m_State;

        public GTextButton(string text, IFont font, IHue defaultHue, IHue focusHue, int x, int y, Client.OnClick onClick) : base(x, y)
        {
            this.m_CanHitTest = true;
            this.m_Hues = new IHue[] { defaultHue, focusHue };
            this.m_OnClick = onClick;
            base.m_Text = text;
            base.m_Font = font;
            base.m_Hue = defaultHue;
            base.m_ITranslucent = true;
            this.Refresh();
        }

        protected internal override bool HitTest(int x, int y)
        {
            if (!this.m_CanHitTest)
            {
                return false;
            }
            if (!base.m_Clip)
            {
                return true;
            }
            if (base.m_Relative)
            {
                return Clipper.TemporaryInstance(base.m_xClipOffset, base.m_yClipOffset, base.m_xClipWidth, base.m_yClipHeight).Evaluate(x, y);
            }
            return ((base.m_Clipper == null) || base.m_Clipper.Evaluate(base.PointToScreen(new Point(x, y))));
        }

        protected internal override void OnMouseEnter(int x, int y, MouseButtons mb)
        {
            this.State = 1;
        }

        protected internal override void OnMouseLeave()
        {
            this.State = 0;
        }

        protected internal override void OnMouseUp(int x, int y, MouseButtons mb)
        {
            if (this.m_OnClick != null)
            {
                base.SetTag("Buttons", mb);
                this.m_OnClick(this);
            }
        }

        public bool CanHitTest
        {
            get
            {
                return this.m_CanHitTest;
            }
            set
            {
                this.m_CanHitTest = value;
            }
        }

        public IHue DefaultHue
        {
            get
            {
                return this.m_Hues[0];
            }
            set
            {
                if (this.m_Hues[0] != value)
                {
                    this.m_Hues[0] = value;
                    if (this.m_State == 0)
                    {
                        base.m_Hue = value;
                        this.Invalidate();
                    }
                }
            }
        }

        public IHue FocusHue
        {
            get
            {
                return this.m_Hues[1];
            }
            set
            {
                if (this.m_Hues[1] != value)
                {
                    this.m_Hues[1] = value;
                    if (this.m_State == 1)
                    {
                        base.m_Hue = value;
                        this.Invalidate();
                    }
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

        public Client.OnHighlight OnHighlight
        {
            get
            {
                return this.m_OnHighlight;
            }
            set
            {
                this.m_OnHighlight = value;
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
                if (this.m_State != value)
                {
                    this.m_State = value;
                    if ((this.m_State == 1) && (this.m_OnHighlight != null))
                    {
                        this.m_OnHighlight(this);
                    }
                    base.m_Hue = this.m_Hues[this.m_State];
                    this.Invalidate();
                }
            }
        }
    }
}

