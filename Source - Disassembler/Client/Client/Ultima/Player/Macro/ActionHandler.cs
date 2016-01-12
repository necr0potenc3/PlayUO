namespace Client
{
    using System;
    using System.Collections;

    public class ActionHandler : IComparable
    {
        private string m_Action;
        private static ArrayList m_List;
        private string m_Name;
        private ParamNode[] m_Params;
        private Client.Plugin m_Plugin;
        private static ActionNode m_RootNode;
        private static Hashtable m_Table;

        private ActionHandler(string action, string name, ParamNode[] parms, Client.Plugin plugin)
        {
            this.m_Action = action;
            this.m_Name = name;
            this.m_Params = parms;
            this.m_Plugin = plugin;
        }

        public int CompareTo(object obj)
        {
            return this.m_Name.CompareTo(((ActionHandler)obj).m_Name);
        }

        public static ActionHandler Find(string action)
        {
            if (m_Table == null)
            {
                return null;
            }
            return (m_Table[action] as ActionHandler);
        }

        public static void Register(string action, ParamNode[] parms, Client.Plugin plugin)
        {
            string str;
            if (m_Table == null)
            {
                m_Table = new Hashtable();
            }
            if (m_List == null)
            {
                m_List = new ArrayList();
            }
            if (m_RootNode == null)
            {
                m_RootNode = new ActionNode("-root-");
            }
            string[] strArray = action.Split(new char[] { '|' });
            ActionNode rootNode = m_RootNode;
            for (int i = 0; i < (strArray.Length - 1); i++)
            {
                ActionNode node = rootNode.GetNode(strArray[i]);
                if (node == null)
                {
                    rootNode.Nodes.Add(node = new ActionNode(strArray[i]));
                    rootNode.Nodes.Sort();
                }
                rootNode = node;
            }
            action = strArray[strArray.Length - 1];
            int index = action.IndexOf('@');
            if (index >= 0)
            {
                str = action.Substring(index + 1);
                action = action.Substring(0, index);
            }
            else
            {
                str = action;
            }
            ActionHandler handler = new ActionHandler(action, str, parms, plugin);
            rootNode.Handlers.Add(handler);
            rootNode.Handlers.Sort();
            m_Table[action] = handler;
            m_List.Add(handler);
        }

        public string Action
        {
            get
            {
                return this.m_Action;
            }
        }

        public static ArrayList List
        {
            get
            {
                if (m_List == null)
                {
                    m_List = new ArrayList();
                }
                return m_List;
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }

        public ParamNode[] Params
        {
            get
            {
                return this.m_Params;
            }
        }

        public Client.Plugin Plugin
        {
            get
            {
                return this.m_Plugin;
            }
        }

        public static ActionNode Root
        {
            get
            {
                if (m_RootNode == null)
                {
                    m_RootNode = new ActionNode("-root-");
                }
                return m_RootNode;
            }
        }

        public static Hashtable Table
        {
            get
            {
                if (m_Table == null)
                {
                    m_Table = new Hashtable();
                }
                return m_Table;
            }
        }
    }
}