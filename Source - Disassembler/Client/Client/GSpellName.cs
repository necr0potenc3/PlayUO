namespace Client
{
    using System;

    public class GSpellName : GTextButton
    {
        private int m_SpellID;

        public GSpellName(int SpellID, string Name, IFont Font, IHue HRegular, IHue HOver, int X, int Y) : base(Name, Font, HRegular, HOver, X, Y, null)
        {
            this.m_SpellID = SpellID;
            base.m_CanDrag = true;
            base.m_QuickDrag = false;
        }

        protected internal override void OnDoubleClick(int X, int Y)
        {
            Network.Send(new PCastSpell(this.m_SpellID));
            Item tag = (Item) base.m_Parent.GetTag("Container");
            tag.LastSpell = this.m_SpellID;
            base.m_Parent.Visible = false;
            Gumps.Desktop.Children.Add(new GSpellbookIcon(base.m_Parent, tag));
        }

        protected internal override void OnDragStart()
        {
            GSpellIcon icon;
            base.m_IsDragging = false;
            Gumps.Drag = null;
            icon = new GSpellIcon(this.m_SpellID) {
                m_OffsetX = icon.Width / 2,
                m_OffsetY = icon.Height / 2,
                X = Engine.m_xMouse - icon.m_OffsetX,
                Y = Engine.m_yMouse - icon.m_OffsetY,
                m_IsDragging = true
            };
            Gumps.Desktop.Children.Add(icon);
            Gumps.Drag = icon;
        }
    }
}

