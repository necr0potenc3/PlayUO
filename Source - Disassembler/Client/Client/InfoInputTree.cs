namespace Client
{
    using System;
    using System.Drawing;

    public class InfoInputTree : InfoInput
    {
        private InfoNode[] m_Nodes;

        public InfoInputTree(string name, params InfoNode[] nodes) : base(name)
        {
            this.m_Nodes = nodes;
            for (int i = 0; i < this.m_Nodes.Length; i++)
            {
                this.m_Nodes[i].Tree = this;
            }
        }

        public override Gump CreateGump()
        {
            GMainMenu menu = new GMainMenu(0, 0);
            GMenuItem child = new GMenuItem(base.Name) {
                DropDown = true
            };
            for (int i = 0; i < this.m_Nodes.Length; i++)
            {
                child.Add(this.m_Nodes[i].Menu);
            }
            menu.Add(child);
            this.RecurseFormatMenu(child);
            return menu;
        }

        private GMenuItem FormatMenu(GMenuItem mi)
        {
            mi.FillAlpha = 1f;
            mi.DefaultColor = GumpPaint.Blend(Color.WhiteSmoke, SystemColors.Control, (float) 0.5f);
            mi.OverColor = GumpPaint.Blend(Color.SteelBlue, SystemColors.Control, (float) 0.5f);
            mi.ExpandedColor = GumpPaint.Blend(Color.SteelBlue, SystemColors.Control, (float) 0.5f);
            mi.SetHue(Hues.Load(1));
            return mi;
        }

        private void RecurseFormatMenu(GMenuItem mi)
        {
            this.FormatMenu(mi);
            for (int i = 0; i < mi.Children.Count; i++)
            {
                if (mi.Children[i] is GMenuItem)
                {
                    this.RecurseFormatMenu((GMenuItem) mi.Children[i]);
                }
            }
        }

        public override void UpdateGump(Gump g)
        {
            GMainMenu menu = (GMainMenu) g;
            GMenuItem item = (GMenuItem) menu.Children[0];
            InfoNode active = base.Active as InfoNode;
            if (active == null)
            {
                item.Text = base.Name;
            }
            else
            {
                item.Text = active.Name;
            }
        }

        public InfoNode[] Nodes
        {
            get
            {
                return this.m_Nodes;
            }
        }
    }
}

