namespace Client
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Windows.Forms;

    public class GContainer : GFader, IContainer
    {
        protected internal Hashtable m_Hash;
        public bool m_HitTest;
        private Client.Item m_Item;
        public bool m_NoDrop;
        public bool m_TradeContainer;
        private int m_xBoundLeft;
        private int m_xBoundRight;
        private int m_yBoundBottom;
        private int m_yBoundTop;

        public GContainer(Client.Item container, int gumpID) : this(container, gumpID, Hues.Default)
        {
        }

        public GContainer(Client.Item container, int gumpID, IHue hue) : base(0.25f, 0.25f, 0.6f, gumpID, 50, 50, hue)
        {
            this.m_HitTest = true;
            this.m_Hash = new Hashtable();
            this.m_Item = container;
            base.m_CanDrop = true;
            this.GetBounds(gumpID);
            base.m_NonRestrictivePicking = true;
            ArrayList items = container.Items;
            int count = items.Count;
            for (int i = 0; i < count; i++)
            {
                Client.Item item = (Client.Item) items[i];
                if (((base.m_GumpID != 9) || ((item.ID & 0x3fff) < 0x2006)) || ((item.ID & 0x3fff) > 0x2050))
                {
                    Client.Gump toAdd = new GContainerItem(item, this.m_Item);
                    this.m_Hash[item] = toAdd;
                    toAdd.m_CanDrag = !this.m_TradeContainer;
                    base.m_Children.Add(toAdd);
                }
            }
        }

        public Point Clip(Texture img, bool xDouble, Point p, int xOffset, int yOffset)
        {
            if ((base.m_GumpID == 0x91a) || (base.m_GumpID == 0x92e))
            {
                return p;
            }
            if (Engine.m_ContainerGrid)
            {
                Point point2;
                Rectangle grid = this.GetGrid();
                Point point = new Point(p, xOffset, yOffset) - new Point(grid.Left, grid.Top);
                point /= 0x15;
                if (point.X < 0)
                {
                    point.X = 0;
                }
                if (point.Y < 0)
                {
                    point.Y = 0;
                }
                if (point.X >= grid.Width)
                {
                    point.X = grid.Width - 1;
                }
                if (point.Y >= grid.Height)
                {
                    point.Y = grid.Height - 1;
                }
                return new Point((grid.X + (point.X * 0x15)) + ((20 - ((xDouble ? 6 : 1) + (img.xMax - img.xMin))) / 2), (grid.Y + (point.Y * 0x15)) + ((20 - ((xDouble ? 6 : 1) + (img.yMax - img.yMin))) / 2)) { X = point2.X - img.xMin, Y = point2.Y - img.yMin };
            }
            Point point3 = new Point(p.X, p.Y);
            int num = p.X + img.xMin;
            int num2 = p.Y + img.yMin;
            int num3 = p.X + img.xMax;
            int num4 = p.Y + img.yMax;
            if (num < this.m_xBoundLeft)
            {
                point3.X = this.m_xBoundLeft - img.xMin;
            }
            if (num2 < this.m_yBoundTop)
            {
                point3.Y = this.m_yBoundTop - img.yMin;
            }
            if (xDouble)
            {
                num3 += 5;
                num4 += 5;
                if (num3 > this.m_xBoundRight)
                {
                    point3.X = (this.m_xBoundRight - img.xMax) - 5;
                }
                if (num4 > this.m_yBoundBottom)
                {
                    point3.Y = (this.m_yBoundBottom - img.yMax) - 5;
                }
                return point3;
            }
            if (num3 > this.m_xBoundRight)
            {
                point3.X = this.m_xBoundRight - img.xMax;
            }
            if (num4 > this.m_yBoundBottom)
            {
                point3.Y = this.m_yBoundBottom - img.yMax;
            }
            return point3;
        }

        public void Close()
        {
            Engine.Sounds.PlayContainerClose(base.m_GumpID);
            Gumps.Destroy(this);
        }

        protected internal override void Draw(int x, int y)
        {
            if (!this.m_TradeContainer)
            {
                base.Draw(x, y);
            }
            if (((base.m_GumpID != 0x91a) && (base.m_GumpID != 0x92e)) && ((base.m_GumpID != 0x52) && Engine.m_ContainerGrid))
            {
                Rectangle grid = this.GetGrid();
                Renderer.SetTexture(null);
                Renderer.AlphaTestEnable = false;
                float num = (float) (base.m_fAlpha * Math.Sqrt((double) base.m_fAlpha));
                if (num != 1f)
                {
                    Renderer.SetAlpha((float) (base.m_fAlpha * Math.Sqrt((double) base.m_fAlpha)));
                    Renderer.SetAlphaEnable(true);
                }
                int num2 = 0;
                for (int i = x + grid.Left; num2 <= grid.Width; i += 0x15)
                {
                    Renderer.DrawLine(i - 1, (y + grid.Top) - 1, i - 1, (y + grid.Top) + (grid.Height * 0x15));
                    num2++;
                }
                int num4 = 0;
                for (int j = y + grid.Top; num4 <= grid.Height; j += 0x15)
                {
                    Renderer.DrawLine((x + grid.Left) - 1, j - 1, (x + grid.Left) + (grid.Width * 0x15), j - 1);
                    num4++;
                }
                Renderer.SetAlphaEnable(false);
                Renderer.AlphaTestEnable = true;
            }
        }

        private void GetBounds(int gumpID)
        {
            Rectangle rectangle = Engine.ContainerBoundsTable.Translate(gumpID);
            this.m_xBoundLeft = rectangle.X;
            this.m_yBoundTop = rectangle.Y;
            this.m_xBoundRight = rectangle.Right - 1;
            this.m_yBoundBottom = rectangle.Bottom - 1;
        }

        public Rectangle GetGrid()
        {
            int num = (this.m_xBoundRight - this.m_xBoundLeft) + 1;
            int num2 = (this.m_yBoundBottom - this.m_yBoundTop) + 1;
            int width = 0;
            int num4 = 0;
            while ((num4 + 20) < num)
            {
                num4 += 0x15;
                width++;
            }
            int height = 0;
            int num6 = 0;
            while ((num6 + 20) < num2)
            {
                num6 += 0x15;
                height++;
            }
            return new Rectangle(this.m_xBoundLeft + ((num - (width * 0x15)) / 2), this.m_yBoundTop + ((num2 - (height * 0x15)) / 2), width, height);
        }

        protected internal override bool HitTest(int x, int y)
        {
            return (this.m_HitTest && base.HitTest(x, y));
        }

        protected internal override void OnDispose()
        {
            this.m_Item.Container = null;
        }

        protected internal override void OnDragDrop(Client.Gump g)
        {
            if (!this.m_HitTest)
            {
                base.m_Parent.OnDragDrop(g);
            }
            else if ((g != null) && (g.GetType() == typeof(GDraggedItem)))
            {
                GDraggedItem item = (GDraggedItem) g;
                Point point = this.Clip(item.Image, item.Double, base.PointToClient(new Point(Engine.m_xMouse - g.m_OffsetX, Engine.m_yMouse - g.m_OffsetY)), g.m_OffsetX, g.m_OffsetY);
                int num = item.Item.ID & 0x3fff;
                if ((num >= 0x3585) && (num <= 0x358a))
                {
                    point.Y += 20;
                }
                else if ((num >= 0x358c) && (num <= 0x3591))
                {
                    point.Y += 20;
                }
                Gumps.Destroy(item);
                Network.Send(new PDropItem(item.Item.Serial, (short) point.X, (short) point.Y, 0, this.m_Item.Serial));
            }
        }

        protected internal override void OnDragStart()
        {
            if (this.m_TradeContainer)
            {
                base.m_IsDragging = false;
                Gumps.Drag = null;
                Point point = base.m_Parent.PointToClient(new Point(Engine.m_xMouse, Engine.m_yMouse));
                base.m_Parent.m_OffsetX = point.X;
                base.m_Parent.m_OffsetY = point.Y;
                base.m_Parent.m_IsDragging = true;
                Gumps.Drag = base.m_Parent;
            }
        }

        public void OnItemAdd(Client.Item item)
        {
            if (((base.m_GumpID != 9) || ((item.ID & 0x3fff) < 0x2006)) || ((item.ID & 0x3fff) > 0x2050))
            {
                GContainerItem g = (GContainerItem) this.m_Hash[item];
                if (g != null)
                {
                    Gumps.Destroy(g);
                }
                this.m_Hash[item] = g = new GContainerItem(item, this.m_Item);
                g.m_CanDrag = !this.m_TradeContainer;
                base.m_Children.Add(g);
            }
        }

        public void OnItemRefresh(Client.Item item)
        {
            if (((base.m_GumpID == 9) && ((item.ID & 0x3fff) >= 0x2006)) && ((item.ID & 0x3fff) <= 0x2050))
            {
                this.OnItemRemove(item);
            }
            else
            {
                GContainerItem toAdd = (GContainerItem) this.m_Hash[item];
                if (toAdd == null)
                {
                    toAdd = new GContainerItem(item, this.m_Item);
                    this.m_Hash[item] = toAdd;
                    base.m_Children.Add(toAdd);
                }
                else
                {
                    toAdd.Refresh();
                }
                toAdd.m_CanDrag = !this.m_TradeContainer;
            }
        }

        public void OnItemRemove(Client.Item item)
        {
            GContainerItem g = (GContainerItem) this.m_Hash[item];
            if (g != null)
            {
                Gumps.Destroy(g);
                this.m_Hash[item] = null;
            }
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            base.BringToTop();
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            if (base.m_CanClose && (mb == MouseButtons.Right))
            {
                if (this.m_TradeContainer)
                {
                    ((GSecureTrade) base.m_Parent).Close();
                }
                else
                {
                    this.Close();
                }
            }
        }

        public Client.Gump Gump
        {
            get
            {
                return this;
            }
        }

        public Client.Item Item
        {
            get
            {
                return this.m_Item;
            }
        }
    }
}

