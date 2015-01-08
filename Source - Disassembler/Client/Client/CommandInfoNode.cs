namespace Client
{
    using System;
    using System.Text;

    public class CommandInfoNode : InfoNode
    {
        private string m_Description;

        public CommandInfoNode(string name, string desc) : this(name, desc, null)
        {
        }

        public CommandInfoNode(string name, string desc, string[,] parms) : base(name, new InfoNode[0])
        {
            if (parms != null)
            {
                StringBuilder builder = new StringBuilder(desc);
                builder.Append("<br><br>Parameters:");
                for (int i = 0; i < parms.GetLength(0); i++)
                {
                    string str = parms[i, 0];
                    string str2 = parms[i, 1];
                    if (str == null)
                    {
                        builder.AppendFormat("<br><div alignright=50>None</div>:<div alignleft=60>{0}</div>", str2);
                    }
                    else
                    {
                        builder.AppendFormat("<br><div alignright=50><u>{0}</u></div>:<div alignleft=60>{1}</div>", str, str2);
                    }
                }
                this.m_Description = builder.ToString();
            }
            else
            {
                this.m_Description = desc;
            }
        }

        public string Description
        {
            get
            {
                return this.m_Description;
            }
        }
    }
}

