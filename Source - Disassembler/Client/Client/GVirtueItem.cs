namespace Client
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    public class GVirtueItem : GServerImage
    {
        private static int[] m_Table = new int[] { 
            0x6c, 0x481, 0x963, 0x965, 110, 0x60a, 0x60f, 0x2a, 0x69, 0x8a4, 0x8a7, 0x34, 0x6f, 0x965, 0x8fd, 0x480, 
            0x70, 0xea, 0x845, 0x20, 0x6b, 0x11, 0x269, 0x13d, 0x6d, 0x8a1, 0x8a3, 0x42, 0x6a, 0x543, 0x547, 0x61
         };
        private GAlphaBackground m_Title;

        public GVirtueItem(GServerGump owner, int x, int y, int gumpID, IHue hue) : base(owner, x, y, gumpID, hue)
        {
            base.m_QuickDrag = false;
            int num = hue.HueID() - 1;
            int num2 = -1;
            int num3 = 0;
            for (int i = 0; i < m_Table.Length; i += 4)
            {
                if (m_Table[i] == gumpID)
                {
                    num2 = i / 4;
                    for (int j = 1; j < 4; j++)
                    {
                        if (m_Table[i + j] == num)
                        {
                            num3 = j;
                            break;
                        }
                    }
                }
            }
            if (num2 >= 0)
            {
                this.m_Title = new GAlphaBackground(30 - x, 40 - y, 0, 0);
                GLabel toAdd = new GLabel(Localization.GetString((0x100978 + (num3 * 8)) + num2), Engine.GetUniFont(0), hue, 3, 3);
                this.m_Title.Children.Add(toAdd);
                toAdd.X -= toAdd.Image.xMin;
                toAdd.Y -= toAdd.Image.yMin;
                this.m_Title.Width = (toAdd.Image.xMax - toAdd.Image.xMin) + 7;
                this.m_Title.Height = (toAdd.Image.yMax - toAdd.Image.yMin) + 7;
                Size size = Engine.m_Gumps.Measure(0x68);
                this.m_Title.X += (size.Width - this.m_Title.Width) / 2;
                this.m_Title.Y += (size.Height - this.m_Title.Height) / 2;
                this.m_Title.Visible = false;
                base.m_Children.Add(this.m_Title);
            }
        }

        protected internal override bool HitTest(int x, int y)
        {
            if (base.m_Invalidated)
            {
                base.Refresh();
            }
            return base.m_Draw;
        }

        protected internal override void OnDoubleClick(int x, int y)
        {
            Network.Send(new PVirtueItemTrigger(base.m_Owner, base.m_GumpID));
        }

        protected internal override void OnMouseEnter(int x, int y, MouseButtons mb)
        {
            if (this.m_Title != null)
            {
                this.m_Title.Visible = true;
            }
        }

        protected internal override void OnMouseLeave()
        {
            if (this.m_Title != null)
            {
                this.m_Title.Visible = false;
            }
        }
    }
}

