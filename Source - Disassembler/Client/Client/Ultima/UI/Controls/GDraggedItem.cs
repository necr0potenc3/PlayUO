namespace Client
{
    using System.Windows.Forms;

    public class GDraggedItem : Gump, IItemGump
    {
        private bool m_Double;
        private bool m_Draw;
        private int m_Height;
        private IHue m_Hue;
        private Texture m_Image;
        private Client.Item m_Item;
        private VertexCache m_vCache;
        private VertexCache m_vCacheDouble;
        private int m_Width;
        private int m_xOffset;
        private int m_yOffset;

        public GDraggedItem(Client.Item item) : base(0, 0)
        {
            this.m_vCache = new VertexCache();
            this.m_Item = item;
            int index = this.m_Item.ID & 0x3fff;
            int amount = (ushort)this.m_Item.Amount;
            this.m_Double = Map.m_ItemFlags[index][TileFlag.Generic] && (amount > 1);
            if ((index >= 0xeea) && (index <= 0xef2))
            {
                int num3 = (index - 0xeea) / 3;
                num3 *= 3;
                num3 += 0xeea;
                this.m_Double = false;
                if (amount <= 1)
                {
                    index = num3;
                }
                else if ((amount >= 2) && (amount <= 5))
                {
                    index = num3 + 1;
                }
                else
                {
                    index = num3 + 2;
                }
            }
            this.m_Hue = Hues.GetItemHue(index, this.m_Item.Hue);
            this.m_Image = this.m_Hue.GetItem(index);
            if ((this.m_Image != null) && !this.m_Image.IsEmpty())
            {
                this.m_Draw = true;
                this.m_Width = this.m_Image.Width;
                this.m_Height = this.m_Image.Height;
                int num4 = this.m_Double ? 6 : 1;
                this.m_xOffset = base.m_OffsetX = this.m_Image.xMin + (((this.m_Image.xMax - this.m_Image.xMin) + num4) / 2);
                this.m_yOffset = this.m_Image.yMin;
                base.m_OffsetY = this.m_yOffset + (((this.m_Image.yMax - this.m_Image.yMin) + num4) / 2);
                if (this.m_Double)
                {
                    this.m_Width += 5;
                    this.m_Height += 5;
                }
            }
            base.m_DragCursor = false;
            base.m_CanDrag = true;
            base.m_QuickDrag = true;
            base.m_IsDragging = true;
            Gumps.Drag = this;
            Gumps.LastOver = this;
            base.m_X = Engine.m_xMouse - base.m_OffsetX;
            base.m_Y = Engine.m_yMouse - base.m_OffsetY;
        }

        protected internal override void Draw(int x, int y)
        {
            if (this.m_Draw)
            {
                this.m_vCache.Draw(this.m_Image, x, y);
                if (this.m_Double)
                {
                    if (this.m_vCacheDouble == null)
                    {
                        this.m_vCacheDouble = new VertexCache();
                    }
                    this.m_vCacheDouble.Draw(this.m_Image, x + 5, y + 5);
                }
                this.m_Item.MessageX = this.X + this.m_xOffset;
                this.m_Item.MessageY = this.Y + this.m_yOffset;
                this.m_Item.BottomY = this.Y + this.m_Image.yMax;
                this.m_Item.MessageFrame = Renderer.m_ActFrames;
            }
        }

        protected internal override bool HitTest(int x, int y)
        {
            if (this.m_Double)
            {
                return (this.m_Draw && ((((this.X < this.m_Image.Width) && (this.Y < this.m_Image.Height)) && this.m_Image.HitTest(x, y)) || (((this.X >= 5) && (this.Y >= 5)) && this.m_Image.HitTest(x - 5, y - 5))));
            }
            return (this.m_Draw && this.m_Image.HitTest(x, y));
        }

        protected internal override void OnDoubleClick(int X, int Y)
        {
            this.m_Item.OnDoubleClick();
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            base.BringToTop();
        }

        protected internal override void OnMouseUp(int x, int y, MouseButtons mb)
        {
            if ((Engine.TargetHandler != null) && ((mb & MouseButtons.Left) != MouseButtons.None))
            {
                this.m_Item.OnTarget();
                Engine.CancelClick();
            }
        }

        protected internal override void OnSingleClick(int x, int y)
        {
            if (Engine.TargetHandler == null)
            {
                this.m_Item.OnSingleClick();
            }
        }

        public bool Double
        {
            get
            {
                return this.m_Double;
            }
        }

        public override int Height
        {
            get
            {
                return this.m_Height;
            }
        }

        public IHue Hue
        {
            get
            {
                return this.m_Hue;
            }
        }

        public Texture Image
        {
            get
            {
                return this.m_Image;
            }
        }

        public Client.Item Item
        {
            get
            {
                return this.m_Item;
            }
        }

        public override int Width
        {
            get
            {
                return this.m_Width;
            }
        }

        public int xOffset
        {
            get
            {
                return this.m_xOffset;
            }
        }

        public int yBottom
        {
            get
            {
                return this.m_Image.yMax;
            }
        }

        public int yOffset
        {
            get
            {
                return this.m_yOffset;
            }
        }
    }
}