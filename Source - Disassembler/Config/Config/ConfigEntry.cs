namespace Config
{
    using System;
    using System.Windows.Forms;

    public class ConfigEntry : IComparable
    {
        private string m_Comment;
        private System.Windows.Forms.Control m_Control;
        private static int m_Count;
        private string m_FriendlyName;
        private int m_Index;
        private string m_Name;
        private System.Type m_Type;
        private object m_Value;

        public ConfigEntry(string name, string friendlyName, string comment, System.Type type, object value)
        {
            this.m_Name = name;
            this.m_FriendlyName = friendlyName;
            this.m_Comment = comment;
            this.m_Type = type;
            this.m_Value = value;
            this.m_Index = m_Count++;
        }

        int IComparable.CompareTo(object o)
        {
            if (o == null)
            {
                return 1;
            }
            ConfigEntry entry = o as ConfigEntry;
            if (entry == null)
            {
                throw new ArgumentException();
            }
            return this.m_Index.CompareTo(entry.m_Index);
        }

        public string Comment
        {
            get
            {
                return this.m_Comment;
            }
        }

        public System.Windows.Forms.Control Control
        {
            get
            {
                return this.m_Control;
            }
            set
            {
                this.m_Control = value;
            }
        }

        public string FriendlyName
        {
            get
            {
                return this.m_FriendlyName;
            }
        }

        public int Index
        {
            get
            {
                return this.m_Index;
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }

        public System.Type Type
        {
            get
            {
                return this.m_Type;
            }
        }

        public object Value
        {
            get
            {
                return this.m_Value;
            }
            set
            {
                if ((value == null) && this.m_Type.IsValueType)
                {
                    throw new ArgumentNullException("value", "Value types can not be null.");
                }
                this.m_Value = value;
            }
        }
    }
}

