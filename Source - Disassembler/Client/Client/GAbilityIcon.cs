namespace Client
{
    using System;
    using System.Collections;

    public class GAbilityIcon : GDragable
    {
        private bool m_InBook;
        private static ArrayList m_Instances = new ArrayList();
        private bool m_Primary;

        public GAbilityIcon(bool inBook, bool primary, int gumpID, int x, int y) : base(gumpID, x, y)
        {
            this.m_InBook = inBook;
            this.m_Primary = primary;
            m_Instances.Add(this);
            base.m_QuickDrag = false;
            base.m_CanClose = !inBook;
        }

        protected internal override void OnDispose()
        {
            m_Instances.Remove(this);
            base.OnDispose();
        }

        protected internal override void OnDoubleClick(int X, int Y)
        {
            AbilityInfo info = this.m_Primary ? AbilityInfo.Primary : AbilityInfo.Secondary;
            if (AbilityInfo.Active == info)
            {
                AbilityInfo.Active = null;
            }
            else
            {
                AbilityInfo.Active = info;
            }
        }

        protected internal override void OnDragStart()
        {
            if (this.m_InBook)
            {
                GAbilityIcon icon;
                base.m_IsDragging = false;
                Gumps.Drag = null;
                icon = new GAbilityIcon(false, this.m_Primary, base.GumpID, Engine.m_xMouse, Engine.m_yMouse) {
                    Hue = base.Hue,
                    m_OffsetX = icon.Width / 2,
                    m_OffsetY = icon.Height / 2,
                    X = Engine.m_xMouse - icon.m_OffsetX,
                    Y = Engine.m_yMouse - icon.m_OffsetY,
                    m_IsDragging = true
                };
                Gumps.Desktop.Children.Add(icon);
                Gumps.Drag = icon;
            }
            else
            {
                base.OnDragStart();
            }
        }

        public static void Update()
        {
            for (int i = 0; i < m_Instances.Count; i++)
            {
                GAbilityIcon icon = (GAbilityIcon) m_Instances[i];
                AbilityInfo info = icon.m_Primary ? AbilityInfo.Primary : AbilityInfo.Secondary;
                icon.GumpID = info.Icon;
                icon.Hue = (info == AbilityInfo.Active) ? Hues.Load(0x8026) : Hues.Default;
                icon.Tooltip = new Tooltip(Localization.GetString(info.Name), true);
                icon.Tooltip.Delay = 0.25f;
            }
        }
    }
}

