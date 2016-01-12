namespace Client
{
    public class GCharacterProfile : GBackground
    {
        public GCharacterProfile(Mobile owner, string header, string body, string footer) : base(0x13c2, 100, 100, 0x19, 0x19, true)
        {
            string gUID = string.Format("Profile-{0}", owner.Serial);
            Gump g = Gumps.FindGumpByGUID(gUID);
            if (g != null)
            {
                base.m_IsDragging = g.m_IsDragging;
                base.m_OffsetX = g.m_OffsetX;
                base.m_OffsetY = g.m_OffsetY;
                if (Gumps.Drag == g)
                {
                    Gumps.Drag = this;
                }
                if (Gumps.LastOver == g)
                {
                    Gumps.LastOver = this;
                }
                if (Gumps.Focus == g)
                {
                    Gumps.Focus = this;
                }
                base.m_X = g.X;
                base.m_Y = g.Y;
                Gumps.Destroy(g);
            }
            base.m_GUID = gUID;
            base.m_CanDrag = true;
            base.m_QuickDrag = true;
            base.CanClose = true;
            Gump toAdd = this.CreateLabel(header, false);
            Gump gump3 = this.CreateLabel(body, true);
            Gump gump4 = this.CreateLabel(footer, false);
            toAdd.X = base.OffsetX;
            toAdd.Y = base.OffsetY;
            gump3.X = toAdd.X;
            gump3.Y = toAdd.Y + toAdd.Height;
            gump4.X = gump3.X;
            gump4.Y = gump3.Y + gump3.Height;
            this.Height = (((this.Height - base.UseHeight) + toAdd.Height) + gump3.Height) + gump4.Height;
            this.Width = toAdd.Width;
            if (gump3.Width > this.Width)
            {
                this.Width = gump3.Width;
            }
            if (gump4.Width > this.Width)
            {
                this.Width = gump4.Width;
            }
            this.Width += this.Width - base.UseWidth;
            base.m_Children.Add(toAdd);
            base.m_Children.Add(gump3);
            base.m_Children.Add(gump4);
        }

        private Gump CreateLabel(string text, bool scroll)
        {
            text = text.Replace('\r', '\n');
            GBackground background = new GBackground(0xbbc, 200, 100, true);
            GWrappedLabel toAdd = new GWrappedLabel(text, Engine.GetFont(1), Hues.Load(0x455), background.OffsetX, background.OffsetY, background.UseWidth);
            background.Height = toAdd.Height + (background.Height - background.UseHeight);
            background.Children.Add(toAdd);
            toAdd.Center();
            background.SetMouseOverride(this);
            return background;
        }
    }
}