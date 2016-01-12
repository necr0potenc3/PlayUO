namespace Client
{
    using System.Windows.Forms;

    public class GContainerItem : Gump, IItemGump
    {
        private Client.Item m_Container;
        private bool m_Double;
        private bool m_Draw;
        private int m_Height;
        private IHue m_Hue;
        private Texture m_Image;
        private Client.Item m_Item;
        private int m_State;
        private int m_TileID;
        private VertexCache m_vCache;
        private VertexCache m_vCacheDouble;
        private static VertexCachePool m_vPool = new VertexCachePool();
        private int m_Width;
        private int m_xOffset;
        private int m_yOffset;

        public GContainerItem(Client.Item Item, Client.Item Container) : base(Item.ContainerX, Item.ContainerY)
        {
            this.m_Item = Item;
            this.m_Container = Container;
            this.m_TileID = this.m_Item.ID;
            this.m_Hue = Hues.GetItemHue(this.m_TileID, this.m_Item.Hue);
            this.State = 0;
            base.m_CanDrag = true;
            base.m_CanDrop = true;
            base.m_QuickDrag = false;
            base.m_DragCursor = false;
            if (Engine.Features.AOS)
            {
                base.Tooltip = new ItemTooltip(this.m_Item);
            }
        }

        protected internal override void Draw(int X, int Y)
        {
            if (this.m_Draw)
            {
                if (this.m_vCache == null)
                {
                    this.m_vCache = this.VCPool.GetInstance();
                }
                this.m_vCache.Draw(this.m_Image, X, Y);
                if (this.m_Double)
                {
                    if (this.m_vCacheDouble == null)
                    {
                        this.m_vCacheDouble = this.VCPool.GetInstance();
                    }
                    this.m_vCacheDouble.Draw(this.m_Image, X + 5, Y + 5);
                }
                this.m_Item.MessageX = X + this.m_xOffset;
                this.m_Item.MessageY = Y + this.m_yOffset;
                this.m_Item.BottomY = Y + this.m_Image.yMax;
                this.m_Item.MessageFrame = Renderer.m_ActFrames;
            }
        }

        protected internal override bool HitTest(int X, int Y)
        {
            if (this.m_Double)
            {
                return (this.m_Draw && ((((X < this.m_Image.Width) && (Y < this.m_Image.Height)) && this.m_Image.HitTest(X, Y)) || (((X >= 5) && (Y >= 5)) && this.m_Image.HitTest(X - 5, Y - 5))));
            }
            return (this.m_Draw && this.m_Image.HitTest(X, Y));
        }

        protected internal override void OnDispose()
        {
            this.VCPool.ReleaseInstance(this.m_vCache);
            this.m_vCache = null;
            this.VCPool.ReleaseInstance(this.m_vCacheDouble);
            this.m_vCacheDouble = null;
        }

        protected internal override void OnDoubleClick(int X, int Y)
        {
            this.m_Item.OnDoubleClick();
        }

        protected internal override void OnDragDrop(Gump g)
        {
            if ((g != null) && (g.GetType() == typeof(GDraggedItem)))
            {
                GDraggedItem item = (GDraggedItem)g;
                Client.Item item2 = item.Item;
                if (((GContainer)base.m_Parent).m_HitTest)
                {
                    TileFlags flags = Map.m_ItemFlags[this.m_Item.ID & 0x3fff];
                    if (flags[TileFlag.Container])
                    {
                        Network.Send(new PDropItem(item2.Serial, -1, -1, 0, this.m_Item.Serial));
                        Gumps.Destroy(item);
                    }
                    else if ((flags[TileFlag.Generic] && (item2.ID == this.m_Item.ID)) && (item2.Hue == this.m_Item.Hue))
                    {
                        Point point = ((GContainer)base.m_Parent).Clip(item.Image, item.Double, base.m_Parent.PointToClient(new Point(Engine.m_xMouse - item.m_OffsetX, Engine.m_yMouse - item.m_OffsetY)), item.m_OffsetX, item.m_OffsetY);
                        Network.Send(new PDropItem(item2.Serial, (short)point.X, (short)point.Y, 0, this.m_Item.Serial));
                        Gumps.Destroy(item);
                    }
                    else
                    {
                        base.m_Parent.OnDragDrop(item);
                    }
                }
                else
                {
                    base.m_Parent.OnDragDrop(item);
                }
            }
        }

        protected internal override void OnDragStart()
        {
            if (this.m_Item != null)
            {
                base.m_IsDragging = false;
                Gumps.LastOver = null;
                Gumps.Drag = null;
                this.State = 0;
                Gump gump = this.m_Item.OnBeginDrag();
                if (gump.GetType() == typeof(GDragAmount))
                {
                    ((GDragAmount)gump).ToDestroy = this;
                }
                else
                {
                    this.m_Item.RestoreInfo = new RestoreInfo(this.m_Item);
                    World.Remove(this.m_Item);
                    gump.m_OffsetX = base.m_OffsetX;
                    gump.m_OffsetY = base.m_OffsetY;
                    gump.X = Engine.m_xMouse - base.m_OffsetX;
                    gump.Y = Engine.m_yMouse - base.m_OffsetY;
                    if (base.m_Parent is GContainer)
                    {
                        ((GContainer)base.m_Parent).m_Hash[this.m_Item] = null;
                    }
                    Gumps.Destroy(this);
                }
            }
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            base.BringToTop();
        }

        protected internal override void OnMouseEnter(int X, int Y, MouseButtons mb)
        {
            this.State = 1;
        }

        protected internal override void OnMouseLeave()
        {
            this.State = 0;
            if (base.Tooltip != null)
            {
                ((ItemTooltip)base.Tooltip).Gump = null;
            }
        }

        protected internal override void OnMouseUp(int x, int y, MouseButtons mb)
        {
            if ((mb & MouseButtons.Right) != MouseButtons.None)
            {
                Point p = base.PointToScreen(new Point(x, y));
                p = base.m_Parent.PointToClient(p);
                base.m_Parent.OnMouseUp(p.X, p.Y, mb);
            }
            else if ((Engine.TargetHandler != null) && ((mb & MouseButtons.Left) != MouseButtons.None))
            {
                this.m_Item.OnTarget();
                Engine.CancelClick();
            }
            else if (((mb & MouseButtons.Left) != MouseButtons.None) && ((Control.ModifierKeys & Keys.Shift) != Keys.None))
            {
                Network.Send(new PPopupRequest(this.m_Item));
            }
        }

        protected internal override void OnSingleClick(int x, int y)
        {
            if (Engine.TargetHandler == null)
            {
                this.m_Item.OnSingleClick();
            }
        }

        public void Refresh()
        {
            this.m_TileID = this.m_Item.ID;
            this.m_Hue = Hues.GetItemHue(this.m_TileID, this.m_Item.Hue);
            this.State = 0;
            base.m_CanDrag = true;
            base.m_CanDrop = true;
            base.m_QuickDrag = false;
            base.m_DragCursor = false;
        }

        public Client.Item Container
        {
            get
            {
                return this.m_Container;
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
                return (this.m_Height + (this.m_Double ? 5 : 0));
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

        public int State
        {
            get
            {
                return this.m_State;
            }
            set
            {
                this.m_State = value;
                int tileID = this.m_TileID;
                int amount = (ushort)this.m_Item.Amount;
                if (this.m_Item != null)
                {
                    this.m_Double = Map.m_ItemFlags[tileID & 0x3fff][TileFlag.Generic] && (amount > 1);
                    if ((this.m_TileID >= 0xeea) && (this.m_TileID <= 0xef2))
                    {
                        int num3 = (this.m_TileID - 0xeea) / 3;
                        num3 *= 3;
                        num3 += 0xeea;
                        this.m_Double = false;
                        if (amount <= 1)
                        {
                            tileID = num3;
                        }
                        else if ((amount >= 2) && (amount <= 5))
                        {
                            tileID = num3 + 1;
                        }
                        else
                        {
                            tileID = num3 + 2;
                        }
                    }
                }
                this.m_Image = ((this.m_State == 0) ? this.m_Hue : Hues.Load(0x8035)).GetItem(tileID);
                if ((this.m_Image != null) && !this.m_Image.IsEmpty())
                {
                    this.m_xOffset = this.m_Image.xMin + (((this.m_Image.xMax - this.m_Image.xMin) + (this.m_Double ? 6 : 1)) / 2);
                    this.m_yOffset = this.m_Image.yMin;
                    this.m_Width = this.m_Image.Width;
                    this.m_Height = this.m_Image.Height;
                    this.m_Draw = this.m_Item != null;
                }
                else
                {
                    this.m_xOffset = this.m_yOffset = this.m_Width = this.m_Height = 0;
                    this.m_Draw = false;
                }
                if (this.m_vCache != null)
                {
                    this.m_vCache.Invalidate();
                }
                if (this.m_vCacheDouble != null)
                {
                    this.m_vCacheDouble.Invalidate();
                }
            }
        }

        public int TileID
        {
            get
            {
                return this.m_TileID;
            }
        }

        protected VertexCachePool VCPool
        {
            get
            {
                return m_vPool;
            }
        }

        public override int Width
        {
            get
            {
                return (this.m_Width + (this.m_Double ? 5 : 0));
            }
        }

        public int xOffset
        {
            get
            {
                return this.m_xOffset;
            }
        }

        public override int Y
        {
            get
            {
                int num = this.m_Item.ID & 0x3fff;
                int y = base.Y;
                if ((num >= 0x3585) && (num <= 0x358a))
                {
                    return (y - 20);
                }
                if ((num >= 0x358c) && (num <= 0x3591))
                {
                    y -= 20;
                }
                return y;
            }
            set
            {
                base.Y = value;
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