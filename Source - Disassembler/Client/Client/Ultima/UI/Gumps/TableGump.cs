namespace Client
{
    using System.Drawing;

    public class TableGump : GAlphaBackground
    {
        private int m_ColWidth;
        private int m_RowHeight;

        public TableGump(TableDescriptor2D desc) : base(0, 0, 380, 0x16 + (desc.Rows.Length * 0x15))
        {
            this.m_ColWidth = desc.Columns.Length;
            this.m_RowHeight = desc.Rows.Length;
            base.ShouldHitTest = false;
            base.FillAlpha = 1f;
            base.FillColor = GumpColors.Window;
            IHue windowText = GumpHues.WindowText;
            for (int i = 0; i <= desc.Rows.Length; i++)
            {
                for (int j = 0; j <= desc.Columns.Length; j++)
                {
                    if ((i != 0) || (j != 0))
                    {
                        string str;
                        if (i == 0)
                        {
                            str = desc.Columns[j - 1];
                        }
                        else if (j == 0)
                        {
                            str = desc.Rows[i - 1];
                        }
                        else
                        {
                            object obj2 = desc.Function(i - 1, j - 1);
                            if (obj2 is double)
                            {
                                str = ((double)obj2).ToString("F2");
                            }
                            else if (obj2 != null)
                            {
                                str = obj2.ToString();
                            }
                            else
                            {
                                str = "(null)";
                            }
                        }
                        this.Place(j, i, str, windowText);
                    }
                }
            }
        }

        protected internal override void Draw(int X, int Y)
        {
            base.Draw(X, Y);
            Renderer.SetAlphaEnable(false);
            Renderer.AlphaTestEnable = false;
            for (int i = 1; i <= this.m_ColWidth; i++)
            {
                int num2;
                int num3;
                int num4;
                int num5;
                this.GetCellDetails(i, 0, out num2, out num3, out num4, out num5);
                Renderer.DrawLine(X + num2, Y, X + num2, (Y + this.Height) - 1, 0);
            }
            for (int j = 1; j <= this.m_RowHeight; j++)
            {
                int num7;
                int num8;
                int num9;
                int num10;
                this.GetCellDetails(0, j, out num7, out num8, out num9, out num10);
                Renderer.DrawLine(X, Y + num8, (X + this.Width) - 1, Y + num8, 0);
            }
            int xMouse = Engine.m_xMouse;
            int yMouse = Engine.m_yMouse;
            xMouse -= X;
            yMouse -= Y;
            if (((xMouse < 0) || (yMouse < 0)) || (((this.m_ColWidth < 8) || (base.m_Parent == null)) || (Gumps.LastOver != base.m_Parent.Parent.Parent)))
            {
                Renderer.AlphaTestEnable = true;
            }
            else
            {
                int num13;
                if (xMouse <= 0x4a)
                {
                    num13 = 0;
                }
                else
                {
                    num13 = (xMouse - 0x25) / 0x26;
                }
                int y = yMouse / 0x15;
                if (((num13 >= 0) && (num13 <= this.m_ColWidth)) && ((y >= 0) && (y <= this.m_RowHeight)))
                {
                    Renderer.SetAlphaEnable(true);
                    Renderer.SetAlpha(0.35f);
                    this.Highlight(num13, y, X, Y);
                    this.Highlight(0, y, X, Y);
                    this.Highlight(num13, 0, X, Y);
                    Renderer.SetAlphaEnable(false);
                }
                Renderer.AlphaTestEnable = true;
            }
        }

        private void GetCellDetails(int x, int y, out int screenX, out int screenY, out int width, out int height)
        {
            if (x == 0)
            {
                screenX = 0;
                screenY = y * 0x15;
                width = 0x4c;
                height = 0x16;
            }
            else if (this.m_ColWidth == 1)
            {
                screenX = 0x25 + (x * 0x26);
                screenY = y * 0x15;
                width = 380 - screenX;
                height = 0x16;
            }
            else
            {
                screenX = 0x25 + (x * 0x26);
                screenY = y * 0x15;
                width = 0x27;
                height = 0x16;
            }
        }

        private void Highlight(int x, int y, int dx, int dy)
        {
            int num;
            int num2;
            int num3;
            int num4;
            this.GetCellDetails(x, y, out num, out num2, out num3, out num4);
            Renderer.SolidRect(Color.SteelBlue.ToArgb() & 0xffffff, (dx + num) + 1, (dy + num2) + 1, num3 - 2, num4 - 2);
        }

        private void Place(int x, int y, string text, IHue hue)
        {
            int num;
            int num2;
            int num3;
            int num4;
            this.GetCellDetails(x, y, out num, out num2, out num3, out num4);
            GLabel toAdd = new GLabel(text, Engine.GetUniFont(((x > 0) && (y > 0)) ? 2 : 1), hue, num + (num3 / 2), num2 + (num4 / 2));
            if ((x == 0) || (this.m_ColWidth == 8))
            {
                toAdd.X -= ((toAdd.Image.xMax - toAdd.Image.xMin) + 1) / 2;
            }
            else
            {
                toAdd.X -= (num3 / 2) - 5;
            }
            toAdd.X -= toAdd.Image.xMin;
            toAdd.Y -= (toAdd.Image.yMax - toAdd.Image.yMin) / 2;
            toAdd.Y -= toAdd.Image.yMin;
            base.m_Children.Add(toAdd);
        }
    }
}