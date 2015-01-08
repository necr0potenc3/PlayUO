namespace Client
{
    using System;
    using System.Text.RegularExpressions;

    public class ObjectProperty
    {
        private static Regex m_ArgReplace = new Regex(@"~(?<1>\d+).*?~", RegexOptions.Singleline);
        private static string[] m_Args;
        private string m_Arguments;
        private int m_Number;
        private string m_Text;

        public ObjectProperty(int number, string arguments)
        {
            this.m_Number = number;
            this.m_Arguments = arguments;
            m_Args = arguments.Split(new char[] { '\t' });
            for (int i = 0; i < m_Args.Length; i++)
            {
                if ((m_Args[i].Length > 1) && m_Args[i].StartsWith("#"))
                {
                    try
                    {
                        m_Args[i] = Localization.GetString(Convert.ToInt32(m_Args[i].Substring(1)));
                    }
                    catch
                    {
                    }
                }
            }
            this.m_Text = Localization.GetString(number);
            this.m_Text = m_ArgReplace.Replace(this.m_Text, new MatchEvaluator(ObjectProperty.ArgReplace_Eval));
        }

        private static string ArgReplace_Eval(Match m)
        {
            try
            {
                int index = Convert.ToInt32(m.Groups[1].Value) - 1;
                return m_Args[index];
            }
            catch
            {
                return m.Value;
            }
        }

        public string Arguments
        {
            get
            {
                return this.m_Arguments;
            }
        }

        public int Number
        {
            get
            {
                return this.m_Number;
            }
        }

        public string Text
        {
            get
            {
                return this.m_Text;
            }
        }
    }
}

