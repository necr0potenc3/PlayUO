namespace Client
{
    using System;

    public class TableDescriptor2D
    {
        private string[] m_Columns;
        private TableComputeFunction m_Function;
        private string[] m_Rows;

        public TableDescriptor2D(string[] columns, string[] rows, TableComputeFunction func)
        {
            this.m_Columns = columns;
            this.m_Rows = rows;
            this.m_Function = func;
        }

        public string[] Columns
        {
            get
            {
                return this.m_Columns;
            }
        }

        public TableComputeFunction Function
        {
            get
            {
                return this.m_Function;
            }
            set
            {
                this.m_Function = value;
            }
        }

        public string[] Rows
        {
            get
            {
                return this.m_Rows;
            }
        }
    }
}

