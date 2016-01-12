namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GPaperdollItem : Gump, ITranslucent, IItemGump
    {
        private bool m_Draw;
        private float m_fAlpha;
        private int m_GumpID;
        private int m_Height;
        private IHue m_Hue;
        private Texture m_Image;
        private Client.Item m_Item;
        private int m_Layer;
        private int m_Serial;
        private bool m_ValidDrag;
        private VertexCache m_vCache;
        private static VertexCachePool m_vPool = new VertexCachePool();
        private int m_Width;
        private int m_xOffset;
        private int m_yOffset;

        public GPaperdollItem(int X, int Y, int GumpID, int Serial, IHue Hue, int Layer, Mobile owner, bool canDrag) : base(X, Y)
        {
            this.m_fAlpha = 1f;
            this.m_GumpID = GumpID;
            this.m_Serial = Serial;
            this.m_Hue = Hue;
            this.m_Layer = Layer;
            this.m_Item = World.FindItem(this.m_Serial);
            if ((this.m_Item != null) && Engine.Features.AOS)
            {
                base.Tooltip = new ItemTooltip(this.m_Item);
            }
            this.m_Image = this.m_Hue.GetGump(this.m_GumpID);
            if ((this.m_Image != null) && !this.m_Image.IsEmpty())
            {
                this.m_Width = this.m_Image.Width;
                this.m_Height = this.m_Image.Height;
                this.m_Draw = this.m_Item != null;
            }
            else
            {
                this.m_Width = 0;
                this.m_Height = 0;
                this.m_Draw = false;
            }
            base.m_ITranslucent = true;
            this.m_ValidDrag = (((canDrag && (this.m_Layer >= 1)) && ((this.m_Layer <= 0x18) && (this.m_Layer != 11))) && (this.m_Layer != 0x10)) && (this.m_Layer != 0x15);
            base.m_CanDrag = true;
            base.m_QuickDrag = (((this.m_Layer < 1) || (this.m_Layer > 0x18)) || (this.m_Layer == 11)) || (this.m_Layer == 0x10);
        }

        protected internal override void Draw(int x, int y)
        {
            if (this.m_Draw)
            {
                bool flag = !(this.m_fAlpha == 1f);
                if (flag)
                {
                    Renderer.SetAlpha(this.m_fAlpha);
                    Renderer.SetAlphaEnable(true);
                }
                if (this.m_vCache == null)
                {
                    this.m_vCache = this.VCPool.GetInstance();
                }
                this.m_vCache.Draw(this.m_Image, x, y);
                if (flag)
                {
                    Renderer.SetAlphaEnable(false);
                }
                this.m_Item.MessageX = x + this.m_xOffset;
                this.m_Item.MessageY = y + this.m_yOffset;
                this.m_Item.BottomY = y + this.m_yOffset;
                this.m_Item.MessageFrame = Renderer.m_ActFrames;
            }
        }

        protected internal override bool HitTest(int X, int Y)
        {
            if (((this.m_Layer == 14) || (this.m_Layer == 8)) || ((this.m_Layer == 10) || (this.m_Layer == 0x12)))
            {
                if (this.m_Draw && base.m_CanDrag)
                {
                    int num = -3;
                    int num2 = 0;
                    while (num <= 3)
                    {
                        int num3 = -3;
                        while (num3 <= 3)
                        {
                            if ((((((int)(Math.Sqrt((double)((num * num) + (num3 * num3))) + 0.5)) <= 3) && ((X + num) >= 0)) && (((X + num) < this.m_Width) && ((Y + num3) >= 0))) && (((Y + num3) < this.m_Height) && this.m_Image.HitTest(X + num, Y + num3)))
                            {
                                return true;
                            }
                            num3++;
                            num2++;
                        }
                        num++;
                    }
                }
                return false;
            }
            return ((this.m_Draw && base.m_CanDrag) && this.m_Image.HitTest(X, Y));
        }

        protected internal override void OnDispose()
        {
            this.VCPool.ReleaseInstance(this.m_vCache);
            this.m_vCache = null;
        }

        protected internal override void OnDoubleClick(int X, int Y)
        {
            this.m_Item.OnDoubleClick();
            this.m_xOffset = X;
            this.m_yOffset = Y;
        }

        protected internal override void OnDragStart()
        {
            if (this.m_ValidDrag && (this.m_Item != null))
            {
                base.m_IsDragging = false;
                Gumps.LastOver = null;
                Gumps.Drag = null;
                Gump gump = this.m_Item.OnBeginDrag();
                if (gump.GetType() == typeof(GDragAmount))
                {
                    ((GDragAmount)gump).ToDestroy = this;
                }
                else
                {
                    this.m_Item.RestoreInfo = new RestoreInfo(this.m_Item);
                    World.Remove(this.m_Item);
                    Gumps.Destroy(this);
                }
            }
            else
            {
                base.m_IsDragging = false;
                Gumps.Drag = null;
                Point point = base.PointToScreen(new Point(0, 0)) - base.m_Parent.PointToScreen(new Point(0, 0));
                base.m_Parent.m_OffsetX = point.X + base.m_OffsetX;
                base.m_Parent.m_OffsetY = point.Y + base.m_OffsetY;
                base.m_Parent.m_IsDragging = true;
                Gumps.Drag = base.m_Parent;
            }
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            base.m_Parent.BringToTop();
        }

        protected internal override void OnMouseEnter(int X, int Y, MouseButtons mb)
        {
        }

        protected internal override void OnMouseLeave()
        {
            if (base.Tooltip != null)
            {
                ((ItemTooltip)base.Tooltip).Gump = null;
            }
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            if ((mb & MouseButtons.Right) != MouseButtons.None)
            {
                Point p = base.PointToScreen(new Point(X, Y));
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
                this.m_Item.Look();
                this.m_xOffset = x;
                this.m_yOffset = y;
            }
        }

        public float Alpha
        {
            get
            {
                return this.m_fAlpha;
            }
            set
            {
                this.m_fAlpha = value;
            }
        }

        public override int Height
        {
            get
            {
                return this.m_Height;
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

        public int Layer
        {
            get
            {
                return this.m_Layer;
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
                return this.m_yOffset;
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