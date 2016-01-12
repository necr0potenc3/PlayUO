namespace Client
{
    using System.Collections;
    using System.Text.RegularExpressions;

    public class HtmlElement
    {
        private Hashtable m_Attributes;
        private static Regex[] m_AttributesRegex = new Regex[] { new Regex(@"(?<name>\w+)\s*=\s*'(?<value>.*?)'"), new Regex("(?<name>\\w+)\\s*=\\s*\"(?<value>.*?)\""), new Regex(@"(?<name>\w+)\s*=\s*(?<value>[^\s]*)") };
        private string m_Name;
        private ElementType m_Type;

        public HtmlElement(string name, ElementType type, Hashtable attributes)
        {
            this.m_Name = name;
            this.m_Type = type;
            this.m_Attributes = attributes;
        }

        public string GetAttribute(string name)
        {
            if (this.m_Attributes == null)
            {
                return null;
            }
            return (string)this.m_Attributes[name];
        }

        public static HtmlElement[] GetElements(string text)
        {
            ArrayList list = new ArrayList();
            int startIndex = 0;
            int index = 0;
            int num3 = 0;
            while (true)
            {
                num3 = index;
                startIndex = text.IndexOf('<', index);
                if (startIndex == -1)
                {
                    list.Add(new HtmlElement(text.Substring(num3), ElementType.Text, null));
                    break;
                }
                index = text.IndexOf('>', startIndex + 1);
                if (index == -1)
                {
                    list.Add(new HtmlElement(text.Substring(num3), ElementType.Text, null));
                    break;
                }
                if (startIndex != num3)
                {
                    list.Add(new HtmlElement(text.Substring(num3, startIndex - num3), ElementType.Text, null));
                }
                index++;
                list.Add(Parse(text.Substring(startIndex, index - startIndex)));
            }
            return (HtmlElement[])list.ToArray(typeof(HtmlElement));
        }

        public static HtmlElement Parse(string ele)
        {
            if (ele.StartsWith("<"))
            {
                ele = ele.Substring(1);
            }
            if (ele.EndsWith(">"))
            {
                ele = ele.Substring(0, ele.Length - 1);
            }
            ElementType type = ele.StartsWith("/") ? ElementType.End : ElementType.Start;
            if (type == ElementType.End)
            {
                ele = ele.Substring(1);
            }
            int index = ele.IndexOf(' ');
            if (index == -1)
            {
                return new HtmlElement(ele, type, null);
            }
            string name = ele.Substring(0, index);
            string input = ele.Substring(++index);
            Hashtable attributes = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
            for (int i = 0; i < m_AttributesRegex.Length; i++)
            {
                Match match;
            Label_009C:
                match = m_AttributesRegex[i].Match(input);
                if (match.Success)
                {
                    string str3 = match.Groups["name"].Value;
                    string str4 = match.Groups["value"].Value;
                    attributes[str3] = str4;
                    input = input.Remove(match.Index, match.Length);
                    goto Label_009C;
                }
            }
            return new HtmlElement(name, type, attributes);
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }

        public ElementType Type
        {
            get
            {
                return this.m_Type;
            }
        }
    }
}