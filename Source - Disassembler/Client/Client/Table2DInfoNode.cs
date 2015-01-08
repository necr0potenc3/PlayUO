namespace Client
{
    using System;

    public class Table2DInfoNode : InfoNode
    {
        private TableDescriptor2D[] m_Descriptors;

        public Table2DInfoNode(string name, TableDescriptor2D[] desc) : base(name, new InfoNode[0])
        {
            this.m_Descriptors = desc;
        }

        public TableDescriptor2D[] Descriptors
        {
            get
            {
                return this.m_Descriptors;
            }
        }
    }
}

