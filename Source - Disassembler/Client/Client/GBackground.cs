namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GBackground : Gump
    {
        private bool m_CanClose;
        private bool m_Destroy;
        private GumpImage[] m_Gumps;
        private bool m_HasBorder;
        private int m_Height;
        private Gump m_Override;
        private int m_Width;

        public GBackground(int BackID, int Width, int Height, bool HasBorder) : this(BackID, Width, Height, 0, 0, HasBorder)
        {
        }

        public GBackground(int BackID, int Width, int Height, int X, int Y, bool HasBorder) : base(X, Y)
        {
            this.m_HasBorder = HasBorder;
            this.m_Width = Width;
            this.m_Height = Height;
            if (HasBorder)
            {
                this.m_Gumps = new GumpImage[] { new GumpImage(BackID), new GumpImage(BackID - 4), new GumpImage(BackID - 3), new GumpImage(BackID - 2), new GumpImage(BackID - 1), new GumpImage(BackID + 1), new GumpImage(BackID + 2), new GumpImage(BackID + 3), new GumpImage(BackID + 4) };
                this.m_Gumps[2].X = this.m_Gumps[1].Width;
                this.m_Gumps[2].Width = (this.m_Width - this.m_Gumps[1].Width) - this.m_Gumps[3].Width;
                this.m_Gumps[3].X = this.m_Width - this.m_Gumps[3].Width;
                this.m_Gumps[4].Y = this.m_Gumps[1].Height;
                this.m_Gumps[4].Height = (this.m_Height - this.m_Gumps[1].Height) - this.m_Gumps[6].Height;
                this.m_Gumps[5].X = this.m_Width - this.m_Gumps[5].Width;
                this.m_Gumps[5].Y = this.m_Gumps[3].Height;
                this.m_Gumps[5].Height = (this.m_Height - this.m_Gumps[3].Height) - this.m_Gumps[8].Height;
                this.m_Gumps[6].Y = this.m_Height - this.m_Gumps[6].Height;
                this.m_Gumps[7].X = this.m_Gumps[6].Width;
                this.m_Gumps[7].Y = this.m_Height - this.m_Gumps[7].Height;
                this.m_Gumps[7].Width = (this.m_Width - this.m_Gumps[6].Width) - this.m_Gumps[8].Width;
                this.m_Gumps[8].X = this.m_Width - this.m_Gumps[8].Width;
                this.m_Gumps[8].Y = this.m_Height - this.m_Gumps[8].Height;
                this.m_Gumps[0].X = this.m_Gumps[1].Width;
                this.m_Gumps[0].Y = this.m_Gumps[1].Height;
                this.m_Gumps[0].Width = (this.m_Width - this.m_Gumps[1].Width) - this.m_Gumps[8].Width;
                this.m_Gumps[0].Height = (this.m_Height - this.m_Gumps[1].Height) - this.m_Gumps[8].Height;
            }
            else
            {
                this.m_Gumps = new GumpImage[] { new GumpImage(BackID, 0, 0, Width, Height) };
            }
        }

        public GBackground(int X, int Y, int Width, int Height, int G1, int G2, int G3, int G4, int G5, int G6, int G7, int G8, int G9) : base(X, Y)
        {
            this.m_HasBorder = true;
            this.m_Width = Width;
            this.m_Height = Height;
            this.m_Gumps = new GumpImage[] { new GumpImage(G5), new GumpImage(G1), new GumpImage(G2), new GumpImage(G3), new GumpImage(G4), new GumpImage(G6), new GumpImage(G7), new GumpImage(G8), new GumpImage(G9) };
            this.m_Gumps[2].X = this.m_Gumps[1].Width;
            this.m_Gumps[2].Width = (this.m_Width - this.m_Gumps[1].Width) - this.m_Gumps[3].Width;
            this.m_Gumps[3].X = this.m_Width - this.m_Gumps[3].Width;
            this.m_Gumps[4].Y = this.m_Gumps[1].Height;
            this.m_Gumps[4].Height = (this.m_Height - this.m_Gumps[1].Height) - this.m_Gumps[6].Height;
            this.m_Gumps[5].X = this.m_Width - this.m_Gumps[5].Width;
            this.m_Gumps[5].Y = this.m_Gumps[3].Height;
            this.m_Gumps[5].Height = (this.m_Height - this.m_Gumps[3].Height) - this.m_Gumps[8].Height;
            this.m_Gumps[6].Y = this.m_Height - this.m_Gumps[6].Height;
            this.m_Gumps[7].X = this.m_Gumps[6].Width;
            this.m_Gumps[7].Y = this.m_Height - this.m_Gumps[7].Height;
            this.m_Gumps[7].Width = (this.m_Width - this.m_Gumps[6].Width) - this.m_Gumps[8].Width;
            this.m_Gumps[8].X = this.m_Width - this.m_Gumps[8].Width;
            this.m_Gumps[8].Y = this.m_Height - this.m_Gumps[8].Height;
            this.m_Gumps[0].X = this.m_Gumps[1].Width;
            this.m_Gumps[0].Y = this.m_Gumps[1].Height;
            this.m_Gumps[0].Width = (this.m_Width - this.m_Gumps[1].Width) - this.m_Gumps[8].Width;
            this.m_Gumps[0].Height = (this.m_Height - this.m_Gumps[1].Height) - this.m_Gumps[8].Height;
        }

        protected internal override void Draw(int x, int y)
        {
            for (int i = 0; i < this.m_Gumps.Length; i++)
            {
                this.m_Gumps[i].Draw(x, y);
            }
        }

        protected internal override bool HitTest(int X, int Y)
        {
            int length = this.m_Gumps.Length;
            for (int i = 0; i < length; i++)
            {
                GumpImage image = this.m_Gumps[i];
                if (((image.CanDraw && (X >= image.X)) && ((X < (image.X + image.Width)) && (Y >= image.Y))) && (Y < (image.Y + image.Height)))
                {
                    return image.Image.HitTest(X - image.X, Y - image.Y);
                }
            }
            return false;
        }

        private void Layout()
        {
            if (this.m_HasBorder)
            {
                this.m_Gumps[2].X = this.m_Gumps[1].Width;
                this.m_Gumps[2].Width = (this.m_Width - this.m_Gumps[1].Width) - this.m_Gumps[3].Width;
                this.m_Gumps[3].X = this.m_Width - this.m_Gumps[3].Width;
                this.m_Gumps[4].Y = this.m_Gumps[1].Height;
                this.m_Gumps[4].Height = (this.m_Height - this.m_Gumps[1].Height) - this.m_Gumps[6].Height;
                this.m_Gumps[5].X = this.m_Width - this.m_Gumps[5].Width;
                this.m_Gumps[5].Y = this.m_Gumps[3].Height;
                this.m_Gumps[5].Height = (this.m_Height - this.m_Gumps[3].Height) - this.m_Gumps[8].Height;
                this.m_Gumps[6].Y = this.m_Height - this.m_Gumps[6].Height;
                this.m_Gumps[7].X = this.m_Gumps[6].Width;
                this.m_Gumps[7].Y = this.m_Height - this.m_Gumps[7].Height;
                this.m_Gumps[7].Width = (this.m_Width - this.m_Gumps[6].Width) - this.m_Gumps[8].Width;
                this.m_Gumps[8].X = this.m_Width - this.m_Gumps[8].Width;
                this.m_Gumps[8].Y = this.m_Height - this.m_Gumps[8].Height;
                this.m_Gumps[0].X = this.m_Gumps[1].Width;
                this.m_Gumps[0].Y = this.m_Gumps[1].Height;
                this.m_Gumps[0].Width = (this.m_Width - this.m_Gumps[1].Width) - this.m_Gumps[8].Width;
                this.m_Gumps[0].Height = (this.m_Height - this.m_Gumps[1].Height) - this.m_Gumps[8].Height;
            }
            else
            {
                this.m_Gumps[0].Width = this.m_Width;
                this.m_Gumps[0].Height = this.m_Height;
            }
        }

        protected internal override void OnDragStart()
        {
            if (this.m_Override != null)
            {
                base.m_IsDragging = false;
                Gumps.Drag = null;
                Point point = base.PointToScreen(new Point(0, 0)) - this.m_Override.PointToScreen(new Point(0, 0));
                this.m_Override.m_OffsetX = point.X + base.m_OffsetX;
                this.m_Override.m_OffsetY = point.Y + base.m_OffsetY;
                this.m_Override.m_IsDragging = true;
                Gumps.Drag = this.m_Override;
            }
        }

        protected internal override void OnFocusChanged(Gump g)
        {
            if (this.m_Destroy && ((g == null) || !g.IsChildOf(this)))
            {
                Gumps.Destroy(this);
            }
        }

        protected internal override void OnMouseDown(int X, int Y, MouseButtons mb)
        {
            if (this.m_Override != null)
            {
                this.m_Override.OnMouseDown(X, Y, mb);
            }
        }

        protected internal override void OnMouseEnter(int X, int Y, MouseButtons mb)
        {
            if (this.m_Override != null)
            {
                this.m_Override.OnMouseEnter(X, Y, mb);
            }
        }

        protected internal override void OnMouseLeave()
        {
            if (this.m_Override != null)
            {
                this.m_Override.OnMouseLeave();
            }
        }

        protected internal override void OnMouseMove(int X, int Y, MouseButtons mb)
        {
            if (this.m_Override != null)
            {
                this.m_Override.OnMouseMove(X, Y, mb);
            }
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            if (this.m_Override != null)
            {
                this.m_Override.OnMouseUp(X, Y, mb);
            }
            else if ((mb == MouseButtons.Right) && this.m_CanClose)
            {
                Gumps.Destroy(this);
            }
        }

        protected internal override void OnMouseWheel(int Delta)
        {
            if (this.m_Override != null)
            {
                this.m_Override.OnMouseWheel(Delta);
            }
        }

        public void SetMouseOverride(Gump g)
        {
            this.m_Override = g;
            base.m_QuickDrag = base.m_CanDrag = ((g != null) && g.m_CanDrag) && g.m_QuickDrag;
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

        public bool DestroyOnUnfocus
        {
            get
            {
                return this.m_Destroy;
            }
            set
            {
                this.m_Destroy = value;
            }
        }

        public override int Height
        {
            get
            {
                return this.m_Height;
            }
            set
            {
                this.m_Height = value;
                this.Layout();
            }
        }

        public int OffsetX
        {
            get
            {
                return this.m_Gumps[0].X;
            }
        }

        public int OffsetY
        {
            get
            {
                return this.m_Gumps[0].Y;
            }
        }

        public int UseHeight
        {
            get
            {
                return this.m_Gumps[0].Height;
            }
        }

        public int UseWidth
        {
            get
            {
                return this.m_Gumps[0].Width;
            }
        }

        public override int Width
        {
            get
            {
                return this.m_Width;
            }
            set
            {
                this.m_Width = value;
                this.Layout();
            }
        }
    }
}

