namespace Client
{
    using System;

    public class Tooltip : ITooltip
    {
        protected bool m_Big;
        protected float m_Delay;
        protected Gump m_Gump;
        protected string m_Text;
        private int m_WrapWidth;

        public Tooltip(string Text) : this(Text, false)
        {
        }

        public Tooltip(string Text, bool Big) : this(Text, Big, Big ? Engine.ScreenWidth : 100)
        {
        }

        public Tooltip(string Text, bool Big, int Width)
        {
            this.m_Text = Text;
            this.m_Big = Big;
            this.m_Gump = null;
            this.m_Delay = 1f;
            this.m_WrapWidth = Width;
        }

        public virtual Gump GetGump()
        {
            if (this.m_Gump == null)
            {
                GWrappedLabel label;
                if ((this.m_Text == null) || (this.m_Text.Length <= 0))
                {
                    return (Gump) (this.m_Gump = null);
                }
                this.m_Gump = new GAlphaBackground(0, 0, 100, 100);
                label = new GWrappedLabel(this.m_Text, Engine.GetUniFont(1), Hues.Load(0x481), 4, 4, this.m_WrapWidth) {
                    X = label.X - label.Image.xMin,
                    Y = label.Y - label.Image.yMin
                };
                this.m_Gump.Width = (label.Image.xMax - label.Image.xMin) + 9;
                this.m_Gump.Height = (label.Image.yMax - label.Image.yMin) + 9;
                this.m_Gump.Children.Add(label);
            }
            return this.m_Gump;
        }

        public bool Big
        {
            get
            {
                return this.m_Big;
            }
            set
            {
                if (this.m_Big != value)
                {
                    this.m_Gump = null;
                    this.m_Big = value;
                }
            }
        }

        public float Delay
        {
            get
            {
                return this.m_Delay;
            }
            set
            {
                this.m_Delay = value;
            }
        }

        public string Text
        {
            get
            {
                return this.m_Text;
            }
            set
            {
                if (this.m_Text != value)
                {
                    this.m_Gump = null;
                    this.m_Text = value;
                }
            }
        }
    }
}

