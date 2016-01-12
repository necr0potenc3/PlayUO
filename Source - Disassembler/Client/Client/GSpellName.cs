namespace Client
{
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
            Item tag = (Item)base.m_Parent.GetTag("Container");
            tag.LastSpell = this.m_SpellID;
            base.m_Parent.Visible = false;
            Gumps.Desktop.Children.Add(new GSpellbookIcon(base.m_Parent, tag));
        }

        protected internal override void OnDragStart()
        {
            base.m_IsDragging = false;
            Gumps.Drag = null;
            GSpellIcon toAdd = new GSpellIcon(this.m_SpellID);
            toAdd.m_OffsetX = toAdd.Width / 2;
            toAdd.m_OffsetY = toAdd.Height / 2;
            toAdd.X = Engine.m_xMouse - toAdd.m_OffsetX;
            toAdd.Y = Engine.m_yMouse - toAdd.m_OffsetY;
            toAdd.m_IsDragging = true;
            Gumps.Desktop.Children.Add(toAdd);
            Gumps.Drag = toAdd;
        }
    }
}