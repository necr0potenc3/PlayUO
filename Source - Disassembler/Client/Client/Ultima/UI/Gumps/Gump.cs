namespace Client
{
    using System.Collections;
    using System.Windows.Forms;

    public class Gump
    {
        protected internal bool m_CanDrag;
        protected internal bool m_CanDrop;
        protected GumpList m_Children;
        protected internal bool m_Disposed;
        protected internal int m_DragClipX = 50;
        protected internal int m_DragClipY = 50;
        protected internal bool m_DragCursor = true;
        protected string m_GUID = "";
        protected int m_Handle;
        protected internal bool m_IsDragging;
        protected internal bool m_ITranslucent;
        protected bool m_Modal;
        protected internal bool m_NonRestrictivePicking;
        protected internal int m_OffsetX;
        protected internal int m_OffsetY;
        protected internal int m_OverCursor = 9;
        protected internal bool m_OverridesCursor = true;
        protected Gump m_Parent;
        protected internal bool m_QuickDrag;
        protected internal bool m_Restore;
        private Hashtable m_Tags;
        protected internal ITooltip m_Tooltip;
        protected internal bool m_Visible = true;
        protected int m_X;
        protected int m_Y;

        public Gump(int X, int Y)
        {
            this.m_Children = new GumpList(this);
            this.m_X = X;
            this.m_Y = Y;
        }

        public void BringToTop()
        {
            if (this.m_Parent != null)
            {
                int index = this.m_Parent.Children.IndexOf(this);
                this.m_Parent.Children.RemoveAt(index);
                this.m_Parent.Children.Add(this);
                this.m_Parent.BringToTop();
            }
        }

        public virtual void Center()
        {
            if (this.m_Parent == null)
            {
                this.X = (Engine.ScreenWidth - this.Width) / 2;
                this.Y = (Engine.ScreenHeight - this.Height) / 2;
            }
            else
            {
                this.X = (this.m_Parent.Width - this.Width) / 2;
                this.Y = (this.m_Parent.Height - this.Height) / 2;
            }
        }

        protected internal virtual void Draw(int X, int Y)
        {
        }

        public object GetTag(string Name)
        {
            if (this.m_Tags == null)
            {
                return null;
            }
            return this.m_Tags[Name];
        }

        public bool HasTag(string Name)
        {
            if (this.m_Tags == null)
            {
                return false;
            }
            return this.m_Tags.Contains(Name);
        }

        protected internal virtual bool HitTest(int X, int Y)
        {
            return false;
        }

        public bool IsChildOf(Gump g)
        {
            for (Gump gump = this; gump != null; gump = gump.Parent)
            {
                if (gump == g)
                {
                    return true;
                }
            }
            return false;
        }

        public void OffsetChildren(int xOffset, int yOffset)
        {
            foreach (Gump gump in this.m_Children.ToArray())
            {
                gump.X += xOffset;
                gump.Y += yOffset;
            }
        }

        protected internal virtual void OnDispose()
        {
        }

        protected internal virtual void OnDoubleClick(int X, int Y)
        {
        }

        protected internal virtual void OnDragDrop(Gump g)
        {
        }

        protected internal virtual void OnDragEnter(Gump g)
        {
        }

        protected internal virtual void OnDragLeave(Gump g)
        {
        }

        protected internal virtual void OnDragMove()
        {
        }

        protected internal virtual void OnDragStart()
        {
        }

        protected internal virtual void OnFocusChanged(Gump Focused)
        {
        }

        protected internal virtual bool OnKeyDown(char Key)
        {
            return false;
        }

        protected internal virtual void OnMouseDown(int X, int Y, MouseButtons mb)
        {
        }

        protected internal virtual void OnMouseEnter(int X, int Y, MouseButtons mb)
        {
        }

        protected internal virtual void OnMouseLeave()
        {
        }

        protected internal virtual void OnMouseMove(int X, int Y, MouseButtons mb)
        {
        }

        protected internal virtual void OnMouseUp(int X, int Y, MouseButtons mb)
        {
        }

        protected internal virtual void OnMouseWheel(int Delta)
        {
        }

        protected internal virtual void OnSingleClick(int X, int Y)
        {
        }

        public Point PointToClient(Point p)
        {
            int num = 0;
            int num2 = 0;
            for (Gump gump = this; gump != null; gump = gump.Parent)
            {
                num += gump.X;
                num2 += gump.Y;
            }
            return new Point(p, -num, -num2);
        }

        public Point PointToScreen(Point p)
        {
            int xOffset = 0;
            int yOffset = 0;
            for (Gump gump = this; gump != null; gump = gump.Parent)
            {
                xOffset += gump.X;
                yOffset += gump.Y;
            }
            return new Point(p, xOffset, yOffset);
        }

        public void RemoveTag(string Name)
        {
            this.m_Tags.Remove(Name);
        }

        protected internal virtual void Render(int X, int Y)
        {
            if (this.m_Visible)
            {
                int x = X + this.X;
                int y = Y + this.Y;
                this.Draw(x, y);
                Gump[] gumpArray = this.m_Children.ToArray();
                for (int i = 0; i < gumpArray.Length; i++)
                {
                    gumpArray[i].Render(x, y);
                }
            }
        }

        public void SetTag(string Name, object Value)
        {
            if (this.m_Tags == null)
            {
                this.m_Tags = new Hashtable();
            }
            this.m_Tags[Name] = Value;
        }

        public GumpList Children
        {
            get
            {
                return this.m_Children;
            }
        }

        public bool Disposed
        {
            get
            {
                return this.m_Disposed;
            }
        }

        public string GUID
        {
            get
            {
                return this.m_GUID;
            }
            set
            {
                this.m_GUID = value;
                Gumps.Restore(this);
            }
        }

        public virtual int Height
        {
            get
            {
                return Engine.ScreenHeight;
            }
            set
            {
            }
        }

        public bool Modal
        {
            get
            {
                return this.m_Modal;
            }
            set
            {
                if (value)
                {
                    if (!this.HasTag("Dispose"))
                    {
                        this.SetTag("Dispose", "Modal");
                    }
                    Gumps.Modal = this;
                }
                else
                {
                    if (this.HasTag("Dispose"))
                    {
                        this.RemoveTag("Dispose");
                    }
                    if (Gumps.Modal == this)
                    {
                        Gumps.Modal = null;
                    }
                }
            }
        }

        public Gump Parent
        {
            get
            {
                return this.m_Parent;
            }
            set
            {
                this.m_Parent = value;
            }
        }

        public ITooltip Tooltip
        {
            get
            {
                return this.m_Tooltip;
            }
            set
            {
                this.m_Tooltip = value;
            }
        }

        public bool Visible
        {
            get
            {
                return this.m_Visible;
            }
            set
            {
                this.m_Visible = value;
                Gumps.Invalidate();
            }
        }

        public virtual int Width
        {
            get
            {
                return Engine.ScreenWidth;
            }
            set
            {
            }
        }

        public virtual int X
        {
            get
            {
                return this.m_X;
            }
            set
            {
                this.m_X = value;
            }
        }

        public virtual int Y
        {
            get
            {
                return this.m_Y;
            }
            set
            {
                this.m_Y = value;
            }
        }
    }
}