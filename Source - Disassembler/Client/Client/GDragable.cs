namespace Client
{
    using System;
    using System.Collections;
    using System.Windows.Forms;

    public class GDragable : Gump, ITranslucent
    {
        protected bool m_bAlpha;
        protected bool m_CanClose;
        protected ArrayList m_Dockers;
        protected bool m_Drag;
        protected bool m_Draw;
        protected float m_fAlpha;
        protected Texture m_Gump;
        protected int m_GumpID;
        protected int m_Height;
        protected IHue m_Hue;
        protected ArrayList m_Linked;
        protected bool m_LinksMoved;
        protected VertexCache m_vCache;
        protected int m_Width;

        public GDragable(int GumpID, int X, int Y) : this(GumpID, Hues.Default, X, Y)
        {
        }

        public GDragable(int GumpID, IHue Hue, int X, int Y) : base(X, Y)
        {
            this.m_CanClose = true;
            this.m_fAlpha = 1f;
            this.m_vCache = new VertexCache();
            this.m_GumpID = GumpID;
            this.m_Hue = Hue;
            base.m_CanDrag = true;
            base.m_QuickDrag = true;
            this.m_Dockers = new ArrayList();
            this.m_Linked = new ArrayList();
            this.m_Gump = Hue.GetGump(GumpID);
            if ((this.m_Gump != null) && !this.m_Gump.IsEmpty())
            {
                this.m_Width = this.m_Gump.Width;
                this.m_Height = this.m_Gump.Height;
                this.m_Draw = true;
            }
        }

        protected internal override void Draw(int X, int Y)
        {
            if (this.m_Draw)
            {
                if (this.m_bAlpha)
                {
                    Renderer.SetAlphaEnable(true);
                    Renderer.SetAlpha(this.m_fAlpha);
                }
                this.m_vCache.Draw(this.m_Gump, X, Y);
                if (this.m_bAlpha)
                {
                    Renderer.SetAlphaEnable(false);
                }
            }
        }

        protected internal override bool HitTest(int X, int Y)
        {
            return (this.m_Draw && this.m_Gump.HitTest(X, Y));
        }

        public bool Link(GDragable g, int Dock, int TheirDock)
        {
            int count = this.m_Linked.Count;
            for (int i = 0; i < count; i++)
            {
                Client.Linked linked = (Client.Linked) this.m_Linked[i];
                if (linked.Gump == g)
                {
                    return false;
                }
            }
            this.m_Linked.Add(new Client.Linked(g, Dock, TheirDock));
            return true;
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            if (this.m_CanClose && (mb == MouseButtons.Right))
            {
                Gumps.Destroy(this);
                Engine.CancelClick();
            }
        }

        public void UpdateLink(Gump gOld, GDragable gNew)
        {
            int count = this.m_Linked.Count;
            for (int i = 0; i < count; i++)
            {
                Client.Linked linked2 = (Client.Linked) this.m_Linked[i];
                if (linked2.Gump == gOld)
                {
                    linked2 = (Client.Linked) this.m_Linked[i];
                    linked2 = (Client.Linked) this.m_Linked[i];
                    Client.Linked linked = new Client.Linked(gNew, linked2.Dock, linked2.TheirDock);
                    this.m_Linked.RemoveAt(i);
                    this.m_Linked.Add(linked);
                    break;
                }
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
                this.m_bAlpha = !(value == 1f);
            }
        }

        public bool CanClose
        {
            get
            {
                return this.m_CanClose;
            }
            set
            {
                this.m_CanClose = value;
            }
        }

        public ArrayList Dockers
        {
            get
            {
                return this.m_Dockers;
            }
        }

        public bool Drag
        {
            get
            {
                return this.m_Drag;
            }
            set
            {
                this.m_Drag = value;
            }
        }

        public int GumpID
        {
            get
            {
                return this.m_GumpID;
            }
            set
            {
                if (this.m_GumpID != value)
                {
                    this.m_GumpID = value;
                    this.m_vCache.Invalidate();
                    this.m_Gump = this.m_Hue.GetGump(this.m_GumpID);
                    if ((this.m_Gump != null) && !this.m_Gump.IsEmpty())
                    {
                        this.m_Width = this.m_Gump.Width;
                        this.m_Height = this.m_Gump.Height;
                        this.m_Draw = true;
                    }
                    else
                    {
                        this.m_Draw = false;
                    }
                }
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
            set
            {
                if (this.m_Hue != value)
                {
                    this.m_Hue = value;
                    this.m_Gump = this.m_Hue.GetGump(this.m_GumpID);
                    this.m_vCache.Invalidate();
                    if ((this.m_Gump != null) && !this.m_Gump.IsEmpty())
                    {
                        this.m_Width = this.m_Gump.Width;
                        this.m_Height = this.m_Gump.Height;
                        this.m_Draw = true;
                    }
                    else
                    {
                        this.m_Draw = false;
                    }
                }
            }
        }

        public ArrayList Linked
        {
            get
            {
                return this.m_Linked;
            }
            set
            {
                this.m_Linked = value;
            }
        }

        public bool LinksMoved
        {
            get
            {
                return this.m_LinksMoved;
            }
            set
            {
                this.m_LinksMoved = value;
            }
        }

        public int OffsetX
        {
            get
            {
                return base.m_OffsetX;
            }
            set
            {
                base.m_OffsetX = value;
            }
        }

        public int OffsetY
        {
            get
            {
                return base.m_OffsetY;
            }
            set
            {
                base.m_OffsetY = value;
            }
        }

        public override int Width
        {
            get
            {
                return this.m_Width;
            }
        }
    }
}

