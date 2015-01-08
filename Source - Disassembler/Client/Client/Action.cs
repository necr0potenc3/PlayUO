namespace Client
{
    using System;

    public class Action
    {
        private ActionHandler m_Action;
        private string m_Line;
        private string m_Param;

        public Action(string line)
        {
            string str;
            string str2;
            this.m_Line = line;
            int index = line.IndexOf(' ');
            if (index >= 0)
            {
                str = line.Substring(0, index);
                str2 = line.Substring(index + 1);
            }
            else
            {
                str = line;
                str2 = "";
            }
            this.m_Action = ActionHandler.Find(str);
            this.m_Param = str2;
        }

        public ActionHandler Handler
        {
            get
            {
                return this.m_Action;
            }
            set
            {
                this.m_Action = value;
            }
        }

        public string Line
        {
            get
            {
                return this.m_Line;
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
    }
}

