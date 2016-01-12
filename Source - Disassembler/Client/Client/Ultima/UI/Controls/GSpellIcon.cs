namespace Client
{
    using System.Windows.Forms;

    public class GSpellIcon : GClickable, IRestorableGump
    {
        public int m_SpellID;

        public GSpellIcon(int spellID) : base(0, 0, GetGumpIDFor(spellID))
        {
            this.m_SpellID = spellID;
            base.m_CanDrag = true;
            base.m_QuickDrag = false;
        }

        protected internal override void Draw(int X, int Y)
        {
            base.Draw(X, Y);
            if (Gumps.LastOver == this)
            {
                Renderer.SetTexture(null);
                Renderer.AlphaTestEnable = false;
                Renderer.SetAlphaEnable(true);
                Renderer.SetAlpha(0.1f);
                Renderer.SolidRect(0xffffff, X - 1, Y - 1, this.Width + 2, this.Height + 2);
                Renderer.SetAlpha(0.6f);
                Renderer.TransparentRect(0xffffff, X - 1, Y - 1, this.Width + 2, this.Height + 2);
                Renderer.SetAlphaEnable(false);
                Renderer.AlphaTestEnable = true;
            }
        }

        public static int GetGumpIDFor(int spellID)
        {
            if ((spellID >= 1) && (spellID <= 0x40))
            {
                spellID--;
                return (0x8c0 + spellID);
            }
            if ((spellID >= 0x65) && (spellID <= 0x74))
            {
                spellID -= 0x65;
                return (0x5000 + spellID);
            }
            if ((spellID >= 0xc9) && (spellID <= 210))
            {
                spellID -= 0xc9;
                return (0x5100 + spellID);
            }
            return 0;
        }

        protected override void OnDoubleClicked()
        {
            Network.Send(new PCastSpell(this.m_SpellID));
        }

        protected internal override void OnDragStart()
        {
            if ((Control.ModifierKeys & Keys.Shift) == Keys.None)
            {
                base.m_IsDragging = false;
                Gumps.Drag = null;
            }
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            if (((mb & MouseButtons.Right) != MouseButtons.None) && ((Control.ModifierKeys & Keys.Shift) == Keys.None))
            {
                Point point = base.PointToScreen(new Point(x, y));
                int distance = 0;
                Engine.movingDir = Engine.GetDirection(point.X, point.Y, ref distance);
                Engine.amMoving = true;
            }
            else
            {
                base.BringToTop();
            }
        }

        protected internal override void OnMouseMove(int X, int Y, MouseButtons mb)
        {
            if (((mb & MouseButtons.Right) != MouseButtons.None) && Engine.amMoving)
            {
                Point point = base.PointToScreen(new Point(X, Y));
                int distance = 0;
                Engine.movingDir = Engine.GetDirection(point.X, point.Y, ref distance);
            }
        }

        protected internal override void OnMouseUp(int x, int y, MouseButtons mb)
        {
            if ((mb & MouseButtons.Right) != MouseButtons.None)
            {
                if ((Control.ModifierKeys & Keys.Shift) != Keys.None)
                {
                    Gumps.Destroy(this);
                    Engine.CancelClick();
                }
                else
                {
                    Engine.amMoving = false;
                }
            }
        }

        int IRestorableGump.Extra
        {
            get
            {
                return (this.m_SpellID + 1);
            }
        }

        int IRestorableGump.Type
        {
            get
            {
                return 0;
            }
        }
    }
}