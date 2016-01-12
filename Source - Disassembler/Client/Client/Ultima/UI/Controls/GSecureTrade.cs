namespace Client
{
    using System.Windows.Forms;

    public class GSecureTrade : GAlphaBackground
    {
        public Gump m_Container;
        private int m_Serial;
        private bool m_ShouldClose;

        public GSecureTrade(int serial, Gump container, string myName, string theirName) : base(50, 50, 0x119, 0x74)
        {
            this.m_Serial = serial;
            this.m_Container = container;
            base.m_CanDrop = true;
            base.FillAlpha = 0.5f;
            base.FillColor = 0x6080ff;
            GBorder3D toAdd = new GBorder3D(false, 0, 0, this.Width, this.Height);
            toAdd.FillAlpha = 0f;
            toAdd.ShouldHitTest = false;
            base.m_Children.Add(toAdd);
            GBorder3D borderd2 = new GBorder3D(true, 6, 6, 0x84, 0x68);
            borderd2.FillAlpha = 0f;
            borderd2.ShouldHitTest = false;
            base.m_Children.Add(borderd2);
            GBorder3D borderd3 = new GBorder3D(false, 7, 7, 130, 20);
            borderd3.ShouldHitTest = false;
            GLabel label = new GLabel(this.Truncate(myName, Engine.GetUniFont(1), borderd3.Width - 0x1c), Engine.GetUniFont(1), Hues.Load(1), 0, 0);
            borderd3.Children.Add(label);
            label.Center();
            label.X = 0x1c - label.Image.xMin;
            base.m_Children.Add(borderd3);
            GBorder3D borderd4 = new GBorder3D(true, 0x8f, 6, 0x84, 0x68);
            borderd4.FillAlpha = 0f;
            borderd4.ShouldHitTest = false;
            base.m_Children.Add(borderd4);
            GBorder3D borderd5 = new GBorder3D(false, 0x90, 7, 130, 20);
            borderd5.ShouldHitTest = false;
            GLabel label2 = new GLabel(this.Truncate(theirName, Engine.GetUniFont(1), borderd5.Width - 0x1c), Engine.GetUniFont(1), Hues.Load(1), 0, 0);
            borderd5.Children.Add(label2);
            label2.Center();
            label2.X = (borderd5.Width - 0x1c) - label2.Image.xMax;
            base.m_Children.Add(borderd5);
            GAlphaBackground background = new GAlphaBackground(1, 1, 5, 0x72);
            background.ShouldHitTest = false;
            background.BorderColor = 0xc0c0c0;
            background.FillColor = 0xc0c0c0;
            background.FillAlpha = 1f;
            base.m_Children.Add(background);
            background = new GAlphaBackground(0x113, 1, 5, 0x72);
            background.ShouldHitTest = false;
            background.BorderColor = 0xc0c0c0;
            background.FillColor = 0xc0c0c0;
            background.FillAlpha = 1f;
            base.m_Children.Add(background);
            background = new GAlphaBackground(6, 1, 0x10d, 5);
            background.ShouldHitTest = false;
            background.BorderColor = 0xc0c0c0;
            background.FillColor = 0xc0c0c0;
            background.FillAlpha = 1f;
            base.m_Children.Add(background);
            background = new GAlphaBackground(6, 110, 0x10d, 5);
            background.ShouldHitTest = false;
            background.BorderColor = 0xc0c0c0;
            background.FillColor = 0xc0c0c0;
            background.FillAlpha = 1f;
            base.m_Children.Add(background);
            background = new GAlphaBackground(0x8a, 6, 5, 0x68);
            background.ShouldHitTest = false;
            background.BorderColor = 0xc0c0c0;
            background.FillColor = 0xc0c0c0;
            background.FillAlpha = 1f;
            base.m_Children.Add(background);
        }

        public void Close()
        {
            Network.Send(new PCancelTrade(this.m_Serial));
        }

        protected internal override void OnDragDrop(Gump g)
        {
            if (this.m_Container != null)
            {
                this.m_Container.OnDragDrop(g);
            }
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            base.BringToTop();
            if ((mb & MouseButtons.Right) != MouseButtons.None)
            {
                this.m_ShouldClose = true;
            }
        }

        protected internal override void OnMouseEnter(int x, int y, MouseButtons mb)
        {
            if (this.m_ShouldClose && ((mb & MouseButtons.Right) == MouseButtons.None))
            {
                this.m_ShouldClose = false;
            }
        }

        protected internal override void OnMouseUp(int x, int y, MouseButtons mb)
        {
            if (this.m_ShouldClose && ((mb & MouseButtons.Right) != MouseButtons.None))
            {
                this.Close();
            }
            this.m_ShouldClose = false;
        }

        public string Truncate(string text, Client.IFont font, int width)
        {
            if (font.GetStringWidth(text) > width)
            {
                while ((text.Length > 0) && (font.GetStringWidth(text + "...") > width))
                {
                    text = text.Substring(0, text.Length - 1);
                }
                text = text + "...";
            }
            return text;
        }
    }
}