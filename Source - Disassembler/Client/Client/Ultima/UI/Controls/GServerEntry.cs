namespace Client
{
    public class GServerEntry : Gump
    {
        private Client.Clipper m_Clipper;
        private int m_Height;
        private GLabel m_Name;
        private GLabel m_PercentFull;
        private int m_SelectedBorderColor;
        private float m_SelectedFillAlpha;
        private int m_SelectedFillColor;
        private Server m_Server;
        private int m_Width;
        private int m_yBase;

        public GServerEntry(Server server, IFont font, IHue hue, int x, int y, int width, int selectedBorderColor, int selectedFillColor, float selectedFillAlpha) : base(x, y)
        {
            this.m_yBase = y;
            this.m_SelectedBorderColor = selectedBorderColor;
            this.m_SelectedFillColor = selectedFillColor;
            this.m_SelectedFillAlpha = selectedFillAlpha;
            this.m_Server = server;
            this.m_Name = new GLabel(server.Name, font, hue, 4, 4);
            this.m_Name.X -= this.m_Name.Image.xMin;
            base.m_Children.Add(this.m_Name);
            this.m_PercentFull = new GLabel(string.Format("{0}% full", server.PercentFull), font, hue, width - 5, 4);
            this.m_PercentFull.X -= this.m_PercentFull.Image.xMax;
            base.m_Children.Add(this.m_PercentFull);
            int num = (this.m_Name.Image.yMax - this.m_Name.Image.yMin) + 1;
            this.m_Height = num;
            num = (this.m_PercentFull.Image.yMax - this.m_PercentFull.Image.yMin) + 1;
            if (num > this.m_Height)
            {
                this.m_Height = num;
            }
            this.m_Height += 8;
            this.m_Name.Y = ((this.m_Height - ((this.m_Name.Image.yMax - this.m_Name.Image.yMin) + 1)) / 2) - this.m_Name.Image.yMin;
            this.m_PercentFull.Y = ((this.m_Height - ((this.m_PercentFull.Image.yMax - this.m_PercentFull.Image.yMin) + 1)) / 2) - this.m_PercentFull.Image.yMin;
            this.m_Width = width;
        }

        protected internal override void Draw(int x, int y)
        {
            if ((Gumps.LastOver == this) && (this.m_Clipper != null))
            {
                int yStart = y;
                int yEnd = y + this.m_Height;
                if ((yStart < this.m_Clipper.yEnd) && (yEnd > this.m_Clipper.yStart))
                {
                    if (yStart < this.m_Clipper.yStart)
                    {
                        yStart = this.m_Clipper.yStart;
                    }
                    if (yEnd > this.m_Clipper.yEnd)
                    {
                        yEnd = this.m_Clipper.yEnd;
                    }
                    int height = yEnd - yStart;
                    Renderer.SetTexture(null);
                    Renderer.AlphaTestEnable = false;
                    Renderer.TransparentRect(this.m_SelectedBorderColor, x, yStart, this.m_Width, height);
                    Renderer.SetAlphaEnable(true);
                    Renderer.SetAlpha(this.m_SelectedFillAlpha);
                    Renderer.SolidRect(this.m_SelectedFillColor, x + 1, yStart + 1, this.m_Width - 2, height - 2);
                    Renderer.SetAlphaEnable(false);
                    Renderer.AlphaTestEnable = true;
                }
            }
            base.Draw(x, y);
        }

        protected internal override bool HitTest(int x, int y)
        {
            return ((this.m_Clipper != null) && this.m_Clipper.Evaluate(base.PointToScreen(new Point(x, y))));
        }

        protected internal override void OnDoubleClick(int x, int y)
        {
            Cursor.Hourglass = true;
            NewConfig.LastServerID = this.m_Server.ServerID;
            NewConfig.Save();
            this.m_Server.Select();
            Gumps.Desktop.Children.Clear();
            xGumps.Display("Connecting");
            Engine.DrawNow();
        }

        protected internal override void OnMouseWheel(int delta)
        {
            if (base.m_Parent != null)
            {
                base.m_Parent.OnMouseWheel(delta);
                Gumps.Invalidated = true;
            }
        }

        public Client.Clipper Clipper
        {
            get
            {
                return this.m_Clipper;
            }
            set
            {
                this.m_Clipper = value;
                this.m_Name.Clipper = value;
                this.m_PercentFull.Clipper = value;
            }
        }

        public override int Height
        {
            get
            {
                return this.m_Height;
            }
        }

        public override int Width
        {
            get
            {
                return this.m_Width;
            }
        }

        public int yBase
        {
            get
            {
                return this.m_yBase;
            }
        }
    }
}