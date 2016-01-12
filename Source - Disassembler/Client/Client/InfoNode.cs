namespace Client
{
    public class InfoNode
    {
        private InfoNode[] m_Children;
        private GMenuItem m_Menu;
        private string m_Name;
        private InfoInputTree m_Tree;

        public InfoNode(string name, params InfoNode[] children)
        {
            this.m_Name = name;
            this.m_Children = children;
        }

        public virtual GMenuItem CreateMenu()
        {
            if (this.m_Children.Length > 0)
            {
                GMenuItem item = new GMenuItem(this.Name);
                for (int i = 0; i < this.m_Children.Length; i++)
                {
                    item.Add(this.m_Children[i].Menu);
                }
                return item;
            }
            return new InternalMenuItem(this);
        }

        public InfoNode[] Children
        {
            get
            {
                return this.m_Children;
            }
        }

        public GMenuItem Menu
        {
            get
            {
                if ((this.m_Menu == null) || this.m_Menu.Disposed)
                {
                    this.m_Menu = this.CreateMenu();
                }
                return this.m_Menu;
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }

        public InfoInputTree Tree
        {
            get
            {
                return this.m_Tree;
            }
            set
            {
                this.m_Tree = value;
                for (int i = 0; i < this.m_Children.Length; i++)
                {
                    this.m_Children[i].Tree = value;
                }
            }
        }

        private class InternalMenuItem : GMenuItem
        {
            private InfoNode m_Node;

            public InternalMenuItem(InfoNode node) : base(node.Name)
            {
                this.m_Node = node;
            }

            public override void OnClick()
            {
                if ((this.m_Node.Children.Length == 0) && (this.m_Node.Tree != null))
                {
                    this.m_Node.Tree.Active = this.m_Node;
                }
            }
        }
    }
}