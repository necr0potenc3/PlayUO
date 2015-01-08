namespace Client
{
    using System;
    using System.Collections;

    public class ActionNode : IComparable
    {
        private ArrayList m_Handlers;
        private string m_Name;
        private ArrayList m_Nodes;

        public ActionNode(string name)
        {
            this.m_Name = name;
            this.m_Nodes = new ArrayList();
            this.m_Handlers = new ArrayList();
        }

        public int CompareTo(object obj)
        {
            return this.m_Name.CompareTo(((ActionNode) obj).m_Name);
        }

        public ActionHandler GetHandler(string action)
        {
            for (int i = 0; i < this.m_Handlers.Count; i++)
            {
                ActionHandler handler = (ActionHandler) this.m_Handlers[i];
                if (handler.Action == action)
                {
                    return handler;
                }
            }
            return null;
        }

        public ActionNode GetNode(string name)
        {
            for (int i = 0; i < this.m_Nodes.Count; i++)
            {
                ActionNode node = (ActionNode) this.m_Nodes[i];
                if (node.m_Name == name)
                {
                    return node;
                }
            }
            return null;
        }

        public ArrayList Handlers
        {
            get
            {
                return this.m_Handlers;
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }

        public ArrayList Nodes
        {
            get
            {
                return this.m_Nodes;
            }
        }
    }
}

