namespace Client
{
    public class GAttributeCurMax : GEmpty
    {
        private int m_Current;
        private GLabel m_GCurrent;
        private GLabel m_GMaximum;
        private int m_Maximum;

        public GAttributeCurMax(int x, int y, int w, int h, int c, int m, IFont font, IHue hue) : base(x, y, w, h)
        {
            this.m_Current = c;
            this.m_Maximum = m;
            this.m_GCurrent = new GWrappedLabel(this.m_Current.ToString(), font, hue, 0, 0, w * 2);
            this.m_GMaximum = new GWrappedLabel(this.m_Maximum.ToString(), font, hue, 0, 11, w * 2);
            base.m_Children.Add(this.m_GCurrent);
            base.m_Children.Add(this.m_GMaximum);
            this.Update();
        }

        protected internal override void Draw(int X, int Y)
        {
            Renderer.SetTexture(null);
            Renderer.AlphaTestEnable = false;
            Renderer.SolidRect(0x212131, X, Y + 10, this.Width, 1);
            Renderer.AlphaTestEnable = true;
        }

        public void SetValues(int cur, int max)
        {
            if ((this.m_Current != cur) || (this.m_Maximum != max))
            {
                if (this.m_Current != cur)
                {
                    this.m_Current = cur;
                    this.m_GCurrent.Text = cur.ToString();
                }
                if (this.m_Maximum != max)
                {
                    this.m_Maximum = max;
                    this.m_GMaximum.Text = max.ToString();
                }
                this.Update();
            }
        }

        public void Update()
        {
            this.m_GCurrent.X = (this.Width - this.m_GCurrent.Width) / 2;
            this.m_GCurrent.Y = 13 - this.m_GCurrent.Height;
            this.m_GMaximum.X = (this.Width - this.m_GMaximum.Width) / 2;
        }

        public int Current
        {
            get
            {
                return this.m_Current;
            }
            set
            {
                this.SetValues(value, this.m_Maximum);
            }
        }

        public int Maximum
        {
            get
            {
                return this.m_Maximum;
            }
            set
            {
                this.SetValues(this.m_Current, value);
            }
        }
    }
}