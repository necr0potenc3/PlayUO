namespace Client
{
    using System;

    public class ParamNode
    {
        private string m_Name;
        private ParamNode[] m_Nodes;
        private string m_Param;

        public ParamNode(string name, string param) : this(name, param, null)
        {
        }

        public ParamNode(string name, ParamNode[] nodes) : this(name, null, nodes)
        {
        }

        public ParamNode(string name, string[] nodes) : this(name, null, new ParamNode[nodes.Length])
        {
            for (int i = 0; i < this.m_Nodes.Length; i++)
            {
                this.m_Nodes[i] = new ParamNode(nodes[i], nodes[i]);
            }
        }

        private ParamNode(string name, string param, ParamNode[] nodes)
        {
            this.m_Name = name;
            this.m_Param = param;
            this.m_Nodes = nodes;
        }

        public static ParamNode[] Count(int start, int count, string format)
        {
            ParamNode[] nodeArray = new ParamNode[count];
            for (int i = 0; i < count; i++)
            {
                string name = string.Format(format, 1 + i);
                nodeArray[i] = new ParamNode(name, i.ToString());
            }
            return nodeArray;
        }

        public static ParamNode[] Empty
        {
            get
            {
                return new ParamNode[0];
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
            set
            {
                this.m_Name = value;
            }
        }

        public ParamNode[] Nodes
        {
            get
            {
                return this.m_Nodes;
            }
            set
            {
                this.m_Nodes = value;
            }
        }

        public string Param
        {
            get
            {
                return this.m_Param;
            }
            set
            {
                this.m_Param = value;
            }
        }

        public static ParamNode[] Toggle
        {
            get
            {
                return new ParamNode[] { new ParamNode("Toggle", ""), new ParamNode("On", "On"), new ParamNode("Off", "Off") };
            }
        }
    }
}

