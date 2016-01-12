namespace Client
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;
    using System.Xml.XPath;

    public class xGumps
    {
        private static XPathNavigator m_Evaluator;
        private static Regex m_GetData = new Regex(@"GetData\( \w+ \)", RegexOptions.None);
        private static Regex m_GetString = new Regex(@"GetString\( .* \)", RegexOptions.None);
        private static Regex m_Hex = new Regex(@"0x\w+", RegexOptions.None);
        private static string m_hScreen = "";
        private static Hashtable m_MainGumps;
        private static Regex m_NotOp = new Regex(@"!(\d+)", RegexOptions.None);
        private static Regex m_SizeOfHeight = new Regex(@"SizeOf\( \w+ \).Height", RegexOptions.None);
        private static Regex m_SizeOfWidth = new Regex(@"SizeOf\( \w+ \).Width", RegexOptions.None);
        private static Hashtable m_Variables;
        private static string m_wScreen = "";
        private static Hashtable m_xFiles;

        static xGumps()
        {
            Debug.TimeBlock("Initializing xGumps");
            m_xFiles = new Hashtable();
            m_Variables = new Hashtable();
            m_MainGumps = new Hashtable();
            m_wScreen = Engine.ScreenWidth.ToString();
            m_hScreen = Engine.ScreenHeight.ToString();
            XmlTextReader xml = new XmlTextReader(Engine.FileManager.BasePath("Data/Gumps/xGumps.xml"));
            xml.WhitespaceHandling = WhitespaceHandling.None;
            Parse_xGumps(xml);
            xml.Close();
            Debug.EndBlock();
        }

        public static void AddGumpTo(string Parent, Gump Child)
        {
            if (m_xFiles.Contains(Parent) && m_MainGumps.Contains(Parent))
            {
                ((Gump)m_MainGumps[Parent]).Children.Add(Child);
            }
        }

        public static bool Display(string Name)
        {
            if (!m_xFiles.Contains(Name))
            {
                return false;
            }
            return Display(Name, Gumps.Desktop, (string)m_xFiles[Name]);
        }

        public static bool Display(string Name, string Parent)
        {
            if (!m_xFiles.Contains(Name))
            {
                return false;
            }
            if (!m_xFiles.Contains(Parent))
            {
                return false;
            }
            if (!m_MainGumps.Contains(Parent))
            {
                return false;
            }
            return Display(Name, (Gump)m_MainGumps[Parent], (string)m_xFiles[Name]);
        }

        private static bool Display(string Name, Gump Parent, string xFile)
        {
            string path = Engine.FileManager.BasePath(string.Format("Data/Gumps/{0}", xFile));
            StreamReader input = new StreamReader(File.OpenRead(path), Encoding.Default, true, 0x800);
            XmlTextReader xml = new XmlTextReader(input);
            xml.WhitespaceHandling = WhitespaceHandling.None;
            if (m_Evaluator == null)
            {
                m_Evaluator = new XPathDocument(path).CreateNavigator();
            }
            bool flag = Parse(xml, Name, Parent);
            xml.Close();
            return flag;
        }

        public static Gump FindGump(string Parent)
        {
            if (!m_xFiles.Contains(Parent))
            {
                return null;
            }
            if (!m_MainGumps.Contains(Parent))
            {
                return null;
            }
            return (m_MainGumps[Parent] as Gump);
        }

        public static bool GetBool(string Value)
        {
            if ((Value == null) || (Value.Length <= 0))
            {
                return false;
            }
            try
            {
                Value = m_GetData.Replace(Value, new MatchEvaluator(xGumps.GetData_Replace));
                Value = m_NotOp.Replace(Value, "not($1)");
                StringBuilder builder = new StringBuilder(Value);
                builder.Replace("true", "1");
                builder.Replace("false", "0");
                builder.Replace("||", " or ");
                builder.Replace("&&", " and ");
                builder.Append(" and 1");
                return (bool)m_Evaluator.Evaluate(builder.ToString());
            }
            catch
            {
                return (Value == "1");
            }
        }

        private static string GetData_Replace(Match m)
        {
            try
            {
                object obj2;
                string key = m.Value.Substring(9);
                key = key.Substring(0, key.Length - 2);
                if (((obj2 = key) != null) /*&& ((obj2 = <PrivateImplementationDetails>.$$method0x6000dc4-1[obj2]) != null)*/)
                {
                    switch (((int)obj2))
                    {
                        case 0:
                            return "4.0.8b";

                        case 1:
                            return Engine.CharacterCount.ToString();

                        case 2:
                            return NewConfig.Username;

                        case 3:
                            return NewConfig.Password;

                        case 4:
                            return (NewConfig.FullScreen ? "1" : "0");

                        case 5:
                            return ((Engine.LastServer != null) ? "1" : "0");

                        case 6:
                            {
                                Server lastServer = Engine.LastServer;
                                return ((lastServer != null) ? lastServer.ServerID.ToString() : "-1");
                            }
                        case 7:
                            {
                                Server server3 = Engine.LastServer;
                                return ((server3 != null) ? server3.Address.Address.ToString() : "-1");
                            }
                        case 8:
                            {
                                Server server4 = Engine.LastServer;
                                return ((server4 != null) ? server4.Name : "");
                            }
                    }
                }
                if (m_Variables.Contains(key))
                {
                    return (string)m_Variables[key];
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        private static IFont GetFont(string Value)
        {
            if ((Value == null) || (Value.Length <= 0))
            {
                return Engine.GetUniFont(3);
            }
            try
            {
                if (Value.StartsWith("Unicode"))
                {
                    return Engine.GetUniFont(GetInt(Value.Split(new char[] { ' ' })[1]));
                }
                return Engine.GetFont(GetInt(Value));
            }
            catch
            {
                return Engine.GetUniFont(3);
            }
        }

        private static IFont GetFonth(string Value)
        {
            if ((Value == null) || (Value.Length <= 0))
            {
                return Engine.GetUniFont(3);
            }
            try
            {
                if (Value.StartsWith("Unicode"))
                {
                    return Engine.GetUniFont(Convert.ToInt32(Value.Split(new char[] { ' ' })[1].Substring(2), 0x10));
                }
                return Engine.GetFont(Convert.ToInt32(Value.Substring(2), 0x10));
            }
            catch
            {
                return Engine.GetUniFont(3);
            }
        }

        private static IFont GetFonti(string Value)
        {
            if ((Value == null) || (Value.Length <= 0))
            {
                return Engine.GetUniFont(0);
            }
            try
            {
                if (Value.StartsWith("Unicode"))
                {
                    return Engine.GetUniFont(Convert.ToInt32(Value.Split(new char[] { ' ' })[1]));
                }
                return Engine.GetFont(Convert.ToInt32(Value));
            }
            catch
            {
                return Engine.GetUniFont(3);
            }
        }

        private static Gump GetGump(string Value)
        {
            if ((Value == null) || (Value.Length <= 0))
            {
                return null;
            }
            try
            {
                return Gumps.FindGumpByGUID(m_GetData.Replace(Value, new MatchEvaluator(xGumps.GetData_Replace)));
            }
            catch
            {
                return null;
            }
        }

        private static Gump GetGumps(string Value)
        {
            if ((Value == null) || (Value.Length <= 0))
            {
                return null;
            }
            try
            {
                return Gumps.FindGumpByGUID(Value);
            }
            catch
            {
                return null;
            }
        }

        private static IHue GetHue(string Value)
        {
            if ((Value == null) || (Value.Length <= 0))
            {
                return Hues.Default;
            }
            try
            {
                return Hues.Load(GetInt(Value));
            }
            catch
            {
                return Hues.Default;
            }
        }

        private static IHue GetHueh(string Value)
        {
            if ((Value == null) || (Value.Length <= 0))
            {
                return Hues.Default;
            }
            try
            {
                return Hues.Load(Convert.ToInt32(Value.Substring(2), 0x10));
            }
            catch
            {
                return Hues.Default;
            }
        }

        private static IHue GetHuei(string Value)
        {
            if ((Value == null) || (Value.Length <= 0))
            {
                return Hues.Default;
            }
            try
            {
                return Hues.Load(Convert.ToInt32(Value));
            }
            catch
            {
                return Hues.Default;
            }
        }

        private static int GetInt(string Value)
        {
            if ((Value == null) || (Value.Length <= 0))
            {
                return 0;
            }
            try
            {
                Value = m_SizeOfWidth.Replace(Value, new MatchEvaluator(xGumps.SizeOfWidth_Replace));
                Value = m_SizeOfHeight.Replace(Value, new MatchEvaluator(xGumps.SizeOfHeight_Replace));
                Value = m_GetData.Replace(Value, new MatchEvaluator(xGumps.GetData_Replace));
                Value = m_Hex.Replace(Value, new MatchEvaluator(xGumps.Hex_Replace));
                StringBuilder builder = new StringBuilder(Value);
                builder.Replace("_wScreen", m_wScreen);
                builder.Replace("_hScreen", m_hScreen);
                builder.Replace("_wGame", "640");
                builder.Replace("_hGame", "480");
                builder.Replace("/", " div ");
                return (int)((double)m_Evaluator.Evaluate(builder.ToString()));
            }
            catch
            {
                return 0;
            }
        }

        private static string GetString(string Value)
        {
            if ((Value == null) || (Value.Length <= 0))
            {
                return "";
            }
            try
            {
                Value = m_GetString.Replace(Value, new MatchEvaluator(xGumps.GetString_Replace));
                return m_GetData.Replace(Value, new MatchEvaluator(xGumps.GetData_Replace));
            }
            catch
            {
                return "<error>";
            }
        }

        private static string GetString_Replace(Match m)
        {
            try
            {
                string str = m.Value.Substring(11);
                return Strings.GetString(str.Substring(0, str.Length - 2));
            }
            catch
            {
                return "";
            }
        }

        public static string Hex_Replace(Match m)
        {
            try
            {
                return Convert.ToInt32(m.Value.Substring(2), 0x10).ToString();
            }
            catch
            {
                return "0";
            }
        }

        private static bool Parse(XmlTextReader xml, string Name, Gump Parent)
        {
            while (xml.Read())
            {
                switch (xml.NodeType)
                {
                    case XmlNodeType.Element:
                        string str;
                        if (((str = xml.Name) != null) && (string.IsInterned(str) == "Gumps"))
                        {
                            if (!Parse_Gumps(xml, Name, Parent))
                            {
                                return false;
                            }
                            continue;
                        }
                        return false;

                    case XmlNodeType.Comment:
                    case XmlNodeType.XmlDeclaration:
                        {
                            continue;
                        }
                }
                return false;
            }
            return true;
        }

        private static bool Parse_Align(XmlTextReader xml, Gump toAlign)
        {
            string str = "";
            bool flag = false;
            string str2 = "";
            bool flag2 = false;
            bool flag3 = xml.IsEmptyElement;
            while (xml.MoveToNextAttribute())
            {
                switch (xml.Name)
                {
                    case "X":
                        {
                            str = GetString(xml.Value);
                            flag = true;
                            continue;
                        }
                    case "Xs":
                        {
                            str = xml.Value;
                            flag = true;
                            continue;
                        }
                    case "Y":
                        {
                            str2 = GetString(xml.Value);
                            flag2 = true;
                            continue;
                        }
                    case "Ys":
                        {
                            str2 = xml.Value;
                            flag2 = true;
                            continue;
                        }
                }
                return false;
            }
            Gump parent = toAlign.Parent;
            if (flag)
            {
                switch (str)
                {
                    case "Left":
                        toAlign.X = 0;
                        goto Label_013D;

                    case "Center":
                        toAlign.X = (parent.Width - toAlign.Width) / 2;
                        goto Label_013D;

                    case "Right":
                        toAlign.X = parent.Width - toAlign.Width;
                        goto Label_013D;
                }
                return false;
            }
        Label_013D:
            if (flag2)
            {
                switch (str2)
                {
                    case "Top":
                        toAlign.Y = 0;
                        goto Label_01B9;

                    case "Center":
                        toAlign.Y = (parent.Height - toAlign.Height) / 2;
                        goto Label_01B9;

                    case "Bottom":
                        toAlign.Y = parent.Height - toAlign.Height;
                        goto Label_01B9;
                }
                return false;
            }
        Label_01B9:
            if (!flag3)
            {
                Skip(xml);
            }
            return true;
        }

        private static bool Parse_AlignOld(XmlTextReader xml, Gump Parent)
        {
            string str = "";
            bool flag = xml.IsEmptyElement;
            while (xml.MoveToNextAttribute())
            {
                string str2;
                if (((str2 = xml.Name) != null) && (string.IsInterned(str2) == "Value"))
                {
                    str = GetString(xml.Value);
                }
                else
                {
                    return false;
                }
            }
            switch (str)
            {
                case "Left":
                    Parent.X = 0;
                    break;

                case "Top":
                    Parent.Y = 0;
                    break;

                case "Right":
                    Parent.X = Parent.Parent.Width - Parent.Width;
                    break;

                case "Bottom":
                    Parent.Y = Parent.Parent.Height - Parent.Height;
                    break;

                case "Left Center":
                    Parent.X = 0;
                    Parent.Y = (Parent.Parent.Height - Parent.Height) / 2;
                    break;

                case "Right Center":
                    Parent.X = Parent.Parent.Width - Parent.Width;
                    Parent.Y = (Parent.Parent.Height - Parent.Height) / 2;
                    break;

                case "Top Center":
                    Parent.X = (Parent.Parent.Width - Parent.Width) / 2;
                    Parent.Y = 0;
                    break;

                case "Bottom Center":
                    Parent.X = (Parent.Parent.Width - Parent.Width) / 2;
                    Parent.Y = Parent.Parent.Height - Parent.Height;
                    break;

                case "Center X":
                    Parent.X = (Parent.Parent.Width - Parent.Width) / 2;
                    break;

                case "Center Y":
                    Parent.Y = (Parent.Parent.Height - Parent.Height) / 2;
                    break;

                default:
                    return false;
            }
            if (!flag)
            {
                Skip(xml);
            }
            return true;
        }

        private static bool Parse_AlignOldNew(XmlTextReader xml, Gump toAlign, bool x)
        {
            string str = "";
            bool flag = xml.IsEmptyElement;
            while (xml.MoveToNextAttribute())
            {
                string str2;
                if (((str2 = xml.Name) != null) && (string.IsInterned(str2) == "Value"))
                {
                    str = GetString(xml.Value);
                }
                else
                {
                    return false;
                }
            }
            Gump parent = toAlign.Parent;
            if (x)
            {
                switch (str)
                {
                    case "Left":
                        toAlign.X = 0;
                        goto Label_014D;

                    case "Center":
                        toAlign.X = (parent.Width - toAlign.Width) / 2;
                        goto Label_014D;

                    case "Right":
                        toAlign.X = parent.Width - toAlign.Width;
                        goto Label_014D;
                }
                return false;
            }
            switch (str)
            {
                case "Top":
                    toAlign.Y = 0;
                    break;

                case "Center":
                    toAlign.Y = (parent.Height - toAlign.Height) / 2;
                    break;

                case "Bottom":
                    toAlign.Y = parent.Height - toAlign.Height;
                    break;

                default:
                    return false;
            }
        Label_014D:
            if (!flag)
            {
                Skip(xml);
            }
            return true;
        }

        private static bool Parse_Background(XmlTextReader xml, Gump Parent, string Name)
        {
            int backID = 0;
            int width = 0;
            int height = 0;
            int x = 0;
            int y = 0;
            bool hasBorder = false;
            bool flag2 = false;
            int num6 = 0;
            int num7 = 0;
            int num8 = 0;
            int num9 = 0;
            int num10 = 0;
            int num11 = 0;
            int num12 = 0;
            int num13 = 0;
            int num14 = 0;
            bool flag3 = xml.IsEmptyElement;
            while (xml.MoveToNextAttribute())
            {
                switch (xml.Name)
                {
                    case "GumpID":
                        {
                            backID = GetInt(xml.Value);
                            continue;
                        }
                    case "GumpIDi":
                        {
                            backID = Convert.ToInt32(xml.Value);
                            continue;
                        }
                    case "GumpIDh":
                        {
                            backID = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            continue;
                        }
                    case "Width":
                        {
                            width = GetInt(xml.Value);
                            continue;
                        }
                    case "Widthi":
                        {
                            width = Convert.ToInt32(xml.Value);
                            continue;
                        }
                    case "Widthh":
                        {
                            width = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            continue;
                        }
                    case "Height":
                        {
                            height = GetInt(xml.Value);
                            continue;
                        }
                    case "Heighti":
                        {
                            height = Convert.ToInt32(xml.Value);
                            continue;
                        }
                    case "Heighth":
                        {
                            height = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            continue;
                        }
                    case "X":
                        {
                            x = GetInt(xml.Value);
                            continue;
                        }
                    case "Xi":
                        {
                            x = Convert.ToInt32(xml.Value);
                            continue;
                        }
                    case "Xh":
                        {
                            x = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            continue;
                        }
                    case "Y":
                        {
                            y = GetInt(xml.Value);
                            continue;
                        }
                    case "Yi":
                        {
                            y = Convert.ToInt32(xml.Value);
                            continue;
                        }
                    case "Yh":
                        {
                            y = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            continue;
                        }
                    case "Border":
                        {
                            hasBorder = GetBool(xml.Value);
                            continue;
                        }
                    case "Borderb":
                        {
                            hasBorder = Convert.ToBoolean(xml.Value);
                            continue;
                        }
                    case "G1":
                        {
                            num6 = GetInt(xml.Value);
                            flag2 = true;
                            continue;
                        }
                    case "G1i":
                        {
                            num6 = Convert.ToInt32(xml.Value);
                            flag2 = true;
                            continue;
                        }
                    case "G1h":
                        {
                            num6 = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            flag2 = true;
                            continue;
                        }
                    case "G2":
                        {
                            num7 = GetInt(xml.Value);
                            flag2 = true;
                            continue;
                        }
                    case "G2i":
                        {
                            num7 = Convert.ToInt32(xml.Value);
                            flag2 = true;
                            continue;
                        }
                    case "G2h":
                        {
                            num7 = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            flag2 = true;
                            continue;
                        }
                    case "G3":
                        {
                            num8 = GetInt(xml.Value);
                            flag2 = true;
                            continue;
                        }
                    case "G3i":
                        {
                            num8 = Convert.ToInt32(xml.Value);
                            flag2 = true;
                            continue;
                        }
                    case "G3h":
                        {
                            num8 = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            flag2 = true;
                            continue;
                        }
                    case "G4":
                        {
                            num9 = GetInt(xml.Value);
                            flag2 = true;
                            continue;
                        }
                    case "G4i":
                        {
                            num9 = Convert.ToInt32(xml.Value);
                            flag2 = true;
                            continue;
                        }
                    case "G4h":
                        {
                            num9 = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            flag2 = true;
                            continue;
                        }
                    case "G5":
                        {
                            num10 = GetInt(xml.Value);
                            flag2 = true;
                            continue;
                        }
                    case "G5i":
                        {
                            num10 = Convert.ToInt32(xml.Value);
                            flag2 = true;
                            continue;
                        }
                    case "G5h":
                        {
                            num10 = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            flag2 = true;
                            continue;
                        }
                    case "G6":
                        {
                            num11 = GetInt(xml.Value);
                            flag2 = true;
                            continue;
                        }
                    case "G6i":
                        {
                            num11 = Convert.ToInt32(xml.Value);
                            flag2 = true;
                            continue;
                        }
                    case "G6h":
                        {
                            num11 = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            flag2 = true;
                            continue;
                        }
                    case "G7":
                        {
                            num12 = GetInt(xml.Value);
                            flag2 = true;
                            continue;
                        }
                    case "G7i":
                        {
                            num12 = Convert.ToInt32(xml.Value);
                            flag2 = true;
                            continue;
                        }
                    case "G7h":
                        {
                            num12 = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            flag2 = true;
                            continue;
                        }
                    case "G8":
                        {
                            num13 = GetInt(xml.Value);
                            flag2 = true;
                            continue;
                        }
                    case "G8i":
                        {
                            num13 = Convert.ToInt32(xml.Value);
                            flag2 = true;
                            continue;
                        }
                    case "G8h":
                        {
                            num13 = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            flag2 = true;
                            continue;
                        }
                    case "G9":
                        {
                            num14 = GetInt(xml.Value);
                            flag2 = true;
                            continue;
                        }
                    case "G9i":
                        {
                            num14 = Convert.ToInt32(xml.Value);
                            flag2 = true;
                            continue;
                        }
                    case "G9h":
                        {
                            num14 = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            flag2 = true;
                            continue;
                        }
                }
                return false;
            }
            Gump toAdd = null;
            if (!flag2)
            {
                toAdd = new GBackground(backID, width, height, x, y, hasBorder);
            }
            else
            {
                toAdd = new GBackground(x, y, width, height, num6, num7, num8, num9, num10, num11, num12, num13, num14);
            }
            Parent.Children.Add(toAdd);
            if (flag3)
            {
                return true;
            }
            while (xml.Read())
            {
                XmlNodeType nodeType = xml.NodeType;
                if (nodeType != XmlNodeType.Element)
                {
                    if (nodeType != XmlNodeType.Comment)
                    {
                        return (nodeType == XmlNodeType.EndElement);
                    }
                }
                else if (!Parse_Element(xml, toAdd, Name))
                {
                    return false;
                }
            }
            return false;
        }

        private static bool Parse_Button(XmlTextReader xml, Gump Parent, string Name)
        {
            int gumpID = 0;
            int x = 0;
            int y = 0;
            string method = "";
            ITooltip tooltip = null;
            bool @bool = false;
            bool flag2 = true;
            bool flag3 = xml.IsEmptyElement;
            while (xml.MoveToNextAttribute())
            {
                switch (xml.Name)
                {
                    case "GumpID":
                        {
                            gumpID = GetInt(xml.Value);
                            continue;
                        }
                    case "GumpIDi":
                        {
                            gumpID = Convert.ToInt32(xml.Value);
                            continue;
                        }
                    case "GumpIDh":
                        {
                            gumpID = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            continue;
                        }
                    case "X":
                        {
                            x = GetInt(xml.Value);
                            continue;
                        }
                    case "Xi":
                        {
                            x = Convert.ToInt32(xml.Value);
                            continue;
                        }
                    case "Xh":
                        {
                            x = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            continue;
                        }
                    case "Y":
                        {
                            y = GetInt(xml.Value);
                            continue;
                        }
                    case "Yi":
                        {
                            y = Convert.ToInt32(xml.Value);
                            continue;
                        }
                    case "Yh":
                        {
                            y = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            continue;
                        }
                    case "OnClick":
                        {
                            method = GetString(xml.Value);
                            continue;
                        }
                    case "OnClicks":
                        {
                            method = xml.Value;
                            continue;
                        }
                    case "Tooltip":
                        {
                            tooltip = new Tooltip(GetString(xml.Value));
                            continue;
                        }
                    case "Tooltips":
                        {
                            tooltip = new Tooltip(xml.Value);
                            continue;
                        }
                    case "CanEnter":
                        {
                            @bool = GetBool(xml.Value);
                            continue;
                        }
                    case "CanEnterb":
                        {
                            @bool = Convert.ToBoolean(xml.Value);
                            continue;
                        }
                    case "Enabled":
                        {
                            flag2 = GetBool(xml.Value);
                            continue;
                        }
                    case "Enabledb":
                        {
                            flag2 = Convert.ToBoolean(xml.Value);
                            continue;
                        }
                }
                return false;
            }
            OnClick clickHandler = null;
            try
            {
                if (method.Length > 0)
                {
                    clickHandler = (OnClick)Delegate.CreateDelegate(typeof(OnClick), typeof(Engine), method);
                }
            }
            catch
            {
                clickHandler = null;
            }
            GButton toAdd = new GButton(gumpID, x, y, clickHandler);
            toAdd.Tooltip = tooltip;
            toAdd.CanEnter = @bool;
            toAdd.Enabled = flag2;
            Parent.Children.Add(toAdd);
            if (flag3)
            {
                return true;
            }
            while (xml.Read())
            {
                XmlNodeType nodeType = xml.NodeType;
                if (nodeType != XmlNodeType.Element)
                {
                    if (nodeType != XmlNodeType.Comment)
                    {
                        return (nodeType == XmlNodeType.EndElement);
                    }
                }
                else if (!Parse_Element(xml, toAdd, Name))
                {
                    return false;
                }
            }
            return false;
        }

        private static bool Parse_DragClip(XmlTextReader xml, Gump Parent)
        {
            int num = 0;
            int num2 = 0;
            bool flag = xml.IsEmptyElement;
            while (xml.MoveToNextAttribute())
            {
                switch (xml.Name)
                {
                    case "X":
                        {
                            num = GetInt(xml.Value);
                            continue;
                        }
                    case "Y":
                        {
                            num2 = GetInt(xml.Value);
                            continue;
                        }
                }
                return false;
            }
            Parent.m_DragClipX = num;
            Parent.m_DragClipY = num2;
            if (!flag)
            {
                Skip(xml);
            }
            return true;
        }

        private static bool Parse_Element(XmlTextReader xml, Gump Parent, string Name)
        {
            switch (xml.Name)
            {
                case "ServerList":
                    if (Parse_ServerList(xml, Parent, Name))
                    {
                        break;
                    }
                    return false;

                case "Background":
                    if (Parse_Background(xml, Parent, Name))
                    {
                        break;
                    }
                    return false;

                case "Label":
                    if (Parse_Label(xml, Parent, Name))
                    {
                        break;
                    }
                    return false;

                case "Image":
                    if (Parse_Image(xml, Parent, Name))
                    {
                        break;
                    }
                    return false;

                case "Button":
                    if (Parse_Button(xml, Parent, Name))
                    {
                        break;
                    }
                    return false;

                case "Tooltip":
                    if (Parse_Tooltip(xml, Parent))
                    {
                        break;
                    }
                    return false;

                case "Include":
                    if (Parse_Include(xml, Parent, Name))
                    {
                        break;
                    }
                    return false;

                case "Tag":
                    if (Parse_Tag(xml, Parent))
                    {
                        break;
                    }
                    return false;

                case "If":
                    if (Parse_If(xml, Parent, Name))
                    {
                        break;
                    }
                    return false;

                case "TextButton":
                    if (Parse_TextButton(xml, Parent, Name))
                    {
                        break;
                    }
                    return false;

                case "GUID":
                    if (Parse_GUID(xml, Parent))
                    {
                        break;
                    }
                    return false;

                case "HotSpot":
                    if (Parse_HotSpot(xml, Parent, Name))
                    {
                        break;
                    }
                    return false;

                case "Align":
                    if (Parse_Align(xml, Parent))
                    {
                        break;
                    }
                    return false;

                case "TextBox":
                    if (Parse_TextBox(xml, Parent, Name))
                    {
                        break;
                    }
                    return false;

                case "Modal":
                    Parent.Modal = true;
                    break;

                case "Movable":
                    Parent.m_CanDrag = true;
                    break;

                case "QuickDrag":
                    Parent.m_QuickDrag = true;
                    break;

                case "Main":
                    m_MainGumps[Name] = Parent;
                    break;

                case "Restore":
                    Parent.m_Restore = true;
                    break;

                case "DragClip":
                    if (Parse_DragClip(xml, Parent))
                    {
                        break;
                    }
                    return false;

                case "HitSpot":
                    if (Parse_HitSpot(xml, Parent, Name))
                    {
                        break;
                    }
                    return false;

                case "Focus":
                    Gumps.Focus = Parent;
                    break;

                default:
                    return false;
            }
            return true;
        }

        private static bool Parse_GUID(XmlTextReader xml, Gump Parent)
        {
            string str = "";
            bool flag = xml.IsEmptyElement;
            while (xml.MoveToNextAttribute())
            {
                switch (xml.Name)
                {
                    case "Value":
                        {
                            str = GetString(xml.Value);
                            continue;
                        }
                    case "Values":
                        {
                            str = xml.Value;
                            continue;
                        }
                }
                return false;
            }
            Parent.GUID = str;
            if (!flag)
            {
                Skip(xml);
            }
            return true;
        }

        private static bool Parse_Gump(XmlTextReader xml, string Find, Gump Parent)
        {
            string name = "";
            while (xml.MoveToNextAttribute())
            {
                string str2;
                if (((str2 = xml.Name) != null) && (string.IsInterned(str2) == "Name"))
                {
                    name = xml.Value;
                }
                else
                {
                    return false;
                }
            }
            if (name != Find)
            {
                Skip(xml);
                return true;
            }
            while (xml.Read())
            {
                XmlNodeType nodeType = xml.NodeType;
                if (nodeType != XmlNodeType.Element)
                {
                    if (nodeType != XmlNodeType.Comment)
                    {
                        return (nodeType == XmlNodeType.EndElement);
                    }
                }
                else if (!Parse_Element(xml, Parent, name))
                {
                    return false;
                }
            }
            return false;
        }

        private static bool Parse_Gumps(XmlTextReader xml, string Name, Gump Parent)
        {
            if (!xml.IsEmptyElement)
            {
                while (xml.Read())
                {
                    XmlNodeType nodeType = xml.NodeType;
                    switch (nodeType)
                    {
                        case XmlNodeType.Element:
                            string str;
                            if (((str = xml.Name) != null) && (string.IsInterned(str) == "Gump"))
                            {
                                if (!Parse_Gump(xml, Name, Parent))
                                {
                                    return false;
                                }
                                continue;
                            }
                            return false;

                        case XmlNodeType.Comment:
                            {
                                continue;
                            }
                    }
                    return (nodeType == XmlNodeType.EndElement);
                }
            }
            return false;
        }

        private static bool Parse_HitSpot(XmlTextReader xml, Gump Parent, string Name)
        {
            int x = 0;
            int y = 0;
            int width = 0;
            int height = 0;
            string method = "";
            bool flag = xml.IsEmptyElement;
            while (xml.MoveToNextAttribute())
            {
                switch (xml.Name)
                {
                    case "X":
                        {
                            x = GetInt(xml.Value);
                            continue;
                        }
                    case "Y":
                        {
                            y = GetInt(xml.Value);
                            continue;
                        }
                    case "Width":
                        {
                            width = GetInt(xml.Value);
                            continue;
                        }
                    case "Height":
                        {
                            height = GetInt(xml.Value);
                            continue;
                        }
                    case "OnClick":
                        {
                            method = GetString(xml.Value);
                            continue;
                        }
                }
                return false;
            }
            GHitspot toAdd = new GHitspot(x, y, width, height, (OnClick)Delegate.CreateDelegate(typeof(OnClick), typeof(Engine), method));
            Parent.Children.Add(toAdd);
            if (flag)
            {
                return true;
            }
            while (xml.Read())
            {
                XmlNodeType nodeType = xml.NodeType;
                if (nodeType != XmlNodeType.Element)
                {
                    if (nodeType != XmlNodeType.Comment)
                    {
                        return (nodeType == XmlNodeType.EndElement);
                    }
                }
                else if (!Parse_Element(xml, toAdd, Name))
                {
                    return false;
                }
            }
            return false;
        }

        private static bool Parse_HotSpot(XmlTextReader xml, Gump Parent, string Name)
        {
            int x = 0;
            int y = 0;
            int width = 0;
            int height = 0;
            Gump target = null;
            bool flag = xml.IsEmptyElement;
            while (xml.MoveToNextAttribute())
            {
                switch (xml.Name)
                {
                    case "X":
                        {
                            x = GetInt(xml.Value);
                            continue;
                        }
                    case "Y":
                        {
                            y = GetInt(xml.Value);
                            continue;
                        }
                    case "Width":
                        {
                            width = GetInt(xml.Value);
                            continue;
                        }
                    case "Height":
                        {
                            height = GetInt(xml.Value);
                            continue;
                        }
                    case "Target":
                        {
                            target = GetGump(xml.Value);
                            continue;
                        }
                }
                return false;
            }
            GHotspot toAdd = new GHotspot(x, y, width, height, target);
            Parent.Children.Add(toAdd);
            if (flag)
            {
                return true;
            }
            while (xml.Read())
            {
                XmlNodeType nodeType = xml.NodeType;
                if (nodeType != XmlNodeType.Element)
                {
                    if (nodeType != XmlNodeType.Comment)
                    {
                        return (nodeType == XmlNodeType.EndElement);
                    }
                }
                else if (!Parse_Element(xml, toAdd, Name))
                {
                    return false;
                }
            }
            return false;
        }

        private static bool Parse_If(XmlTextReader xml, Gump Parent, string Name)
        {
            bool @bool = false;
            bool flag2 = xml.IsEmptyElement;
            while (xml.MoveToNextAttribute())
            {
                string str;
                if (((str = xml.Name) != null) && (string.IsInterned(str) == "Condition"))
                {
                    @bool = GetBool(xml.Value);
                }
                else
                {
                    return false;
                }
            }
            if (!@bool && !flag2)
            {
                Skip(xml);
            }
            else if (@bool && !flag2)
            {
                while (xml.Read())
                {
                    XmlNodeType nodeType = xml.NodeType;
                    if (nodeType != XmlNodeType.Element)
                    {
                        if (nodeType != XmlNodeType.Comment)
                        {
                            return (nodeType == XmlNodeType.EndElement);
                        }
                    }
                    else if (!Parse_Element(xml, Parent, Name))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool Parse_Image(XmlTextReader xml, Gump Parent, string Name)
        {
            Gump gump;
            int gumpID = 0;
            int x = 0;
            int y = 0;
            bool @bool = false;
            float num4 = 1f;
            bool flag2 = xml.IsEmptyElement;
            while (xml.MoveToNextAttribute())
            {
                switch (xml.Name)
                {
                    case "GumpID":
                        {
                            gumpID = GetInt(xml.Value);
                            continue;
                        }
                    case "GumpIDi":
                        {
                            gumpID = Convert.ToInt32(xml.Value);
                            continue;
                        }
                    case "GumpIDh":
                        {
                            gumpID = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            continue;
                        }
                    case "X":
                        {
                            x = GetInt(xml.Value);
                            continue;
                        }
                    case "Xi":
                        {
                            x = Convert.ToInt32(xml.Value);
                            continue;
                        }
                    case "Xh":
                        {
                            x = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            continue;
                        }
                    case "Y":
                        {
                            y = GetInt(xml.Value);
                            continue;
                        }
                    case "Yi":
                        {
                            y = Convert.ToInt32(xml.Value);
                            continue;
                        }
                    case "Yh":
                        {
                            y = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            continue;
                        }
                    case "HitTest":
                        {
                            @bool = GetBool(xml.Value);
                            continue;
                        }
                    case "HitTestb":
                        {
                            @bool = Convert.ToBoolean(xml.Value);
                            continue;
                        }
                    case "Alpha":
                        {
                            num4 = ((float)GetInt(xml.Value)) / 255f;
                            continue;
                        }
                    case "Alphai":
                        {
                            num4 = ((float)Convert.ToInt32(xml.Value)) / 255f;
                            continue;
                        }
                    case "Alphah":
                        {
                            num4 = ((float)Convert.ToInt32(xml.Value.Substring(2), 0x10)) / 255f;
                            continue;
                        }
                }
                return false;
            }
            if (!@bool)
            {
                gump = new GImage(gumpID, x, y);
                ((GImage)gump).Alpha = num4;
            }
            else
            {
                gump = new GDragable(gumpID, x, y);
                gump.m_CanDrag = false;
                ((GDragable)gump).Alpha = num4;
            }
            Parent.Children.Add(gump);
            if (flag2)
            {
                return true;
            }
            while (xml.Read())
            {
                XmlNodeType nodeType = xml.NodeType;
                if (nodeType != XmlNodeType.Element)
                {
                    if (nodeType != XmlNodeType.Comment)
                    {
                        return (nodeType == XmlNodeType.EndElement);
                    }
                }
                else if (!Parse_Element(xml, gump, Name))
                {
                    return false;
                }
            }
            return false;
        }

        private static bool Parse_Include(XmlTextReader xml, Gump Parent, string Name)
        {
            string key = "";
            bool flag = xml.IsEmptyElement;
            while (xml.MoveToNextAttribute())
            {
                switch (xml.Name)
                {
                    case "Gump":
                        {
                            key = GetString(xml.Value);
                            continue;
                        }
                    case "Gumps":
                        {
                            key = xml.Value;
                            continue;
                        }
                }
                return false;
            }
            if (m_xFiles.Contains(key))
            {
                Display(key, Parent, (string)m_xFiles[key]);
            }
            if (!flag)
            {
                if (m_MainGumps[key] == null)
                {
                    return false;
                }
                while (xml.Read())
                {
                    XmlNodeType nodeType = xml.NodeType;
                    if (nodeType != XmlNodeType.Element)
                    {
                        if (nodeType != XmlNodeType.Comment)
                        {
                            return (nodeType == XmlNodeType.EndElement);
                        }
                    }
                    else if (!Parse_Element(xml, (Gump)m_MainGumps[key], Name))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool Parse_Label(XmlTextReader xml, Gump Parent, string Name)
        {
            string text = "";
            IFont defaultFont = Engine.DefaultFont;
            IHue huei = Hues.Default;
            int x = 0;
            int y = 0;
            int width = 0;
            bool flag = false;
            bool flag2 = xml.IsEmptyElement;
            while (xml.MoveToNextAttribute())
            {
                switch (xml.Name)
                {
                    case "Text":
                        {
                            text = GetString(xml.Value);
                            continue;
                        }
                    case "Texts":
                        {
                            text = xml.Value;
                            continue;
                        }
                    case "Font":
                        {
                            defaultFont = GetFont(xml.Value);
                            continue;
                        }
                    case "Fonti":
                        {
                            defaultFont = GetFonti(xml.Value);
                            continue;
                        }
                    case "Fonth":
                        {
                            defaultFont = GetFonth(xml.Value);
                            continue;
                        }
                    case "Hue":
                        {
                            huei = GetHue(xml.Value);
                            continue;
                        }
                    case "Huei":
                        {
                            huei = GetHuei(xml.Value);
                            continue;
                        }
                    case "Hueh":
                        {
                            huei = GetHueh(xml.Value);
                            continue;
                        }
                    case "X":
                        {
                            x = GetInt(xml.Value);
                            continue;
                        }
                    case "Xi":
                        {
                            x = Convert.ToInt32(xml.Value);
                            continue;
                        }
                    case "Xh":
                        {
                            x = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            continue;
                        }
                    case "Y":
                        {
                            y = GetInt(xml.Value);
                            continue;
                        }
                    case "Yi":
                        {
                            y = Convert.ToInt32(xml.Value);
                            continue;
                        }
                    case "Yh":
                        {
                            y = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            continue;
                        }
                    case "Width":
                        {
                            width = GetInt(xml.Value);
                            flag = true;
                            continue;
                        }
                    case "Widthi":
                        {
                            width = Convert.ToInt32(xml.Value);
                            flag = true;
                            continue;
                        }
                    case "Widthh":
                        {
                            width = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            flag = true;
                            continue;
                        }
                }
                return false;
            }
            Gump toAdd = null;
            if (!flag)
            {
                toAdd = new GLabel(text, defaultFont, huei, x, y);
            }
            else
            {
                toAdd = new GWrappedLabel(text, defaultFont, huei, x, y, width);
            }
            Parent.Children.Add(toAdd);
            if (flag2)
            {
                return true;
            }
            while (xml.Read())
            {
                XmlNodeType nodeType = xml.NodeType;
                if (nodeType != XmlNodeType.Element)
                {
                    if (nodeType != XmlNodeType.Comment)
                    {
                        return (nodeType == XmlNodeType.EndElement);
                    }
                }
                else if (!Parse_Element(xml, toAdd, Name))
                {
                    return false;
                }
            }
            return false;
        }

        private static bool Parse_ServerList(XmlTextReader xml, Gump Parent, string Name)
        {
            int gumpID = 0;
            int x = 0;
            int y = 0;
            int width = 0;
            int height = 0;
            int selectionBorderColor = 0;
            int selectionFillColor = 0;
            int selectionFillAlpha = 0;
            IFont defaultFont = Engine.DefaultFont;
            IHue hue = Hues.Default;
            while (xml.MoveToNextAttribute())
            {
                switch (xml.Name)
                {
                    case "GumpID":
                        {
                            gumpID = GetInt(xml.Value);
                            continue;
                        }
                    case "X":
                        {
                            x = GetInt(xml.Value);
                            continue;
                        }
                    case "Y":
                        {
                            y = GetInt(xml.Value);
                            continue;
                        }
                    case "Width":
                        {
                            width = GetInt(xml.Value);
                            continue;
                        }
                    case "Height":
                        {
                            height = GetInt(xml.Value);
                            continue;
                        }
                    case "Font":
                        {
                            defaultFont = GetFont(xml.Value);
                            continue;
                        }
                    case "Hue":
                        {
                            hue = GetHue(xml.Value);
                            continue;
                        }
                    case "SelectionBorderColor":
                        {
                            selectionBorderColor = GetInt(xml.Value);
                            continue;
                        }
                    case "SelectionFillColor":
                        {
                            selectionFillColor = GetInt(xml.Value);
                            continue;
                        }
                    case "SelectionFillAlpha":
                        {
                            selectionFillAlpha = GetInt(xml.Value);
                            continue;
                        }
                }
                return false;
            }
            Parent.Children.Add(new GServerList(Engine.Servers, x, y, width, height, gumpID, defaultFont, hue, selectionBorderColor, selectionFillColor, selectionFillAlpha));
            return true;
        }

        private static bool Parse_Tag(XmlTextReader xml, Gump Parent)
        {
            string name = "";
            string str2 = "";
            object @bool = "";
            bool flag = xml.IsEmptyElement;
            while (xml.MoveToNextAttribute())
            {
                switch (xml.Name)
                {
                    case "Name":
                        {
                            name = GetString(xml.Value);
                            continue;
                        }
                    case "Names":
                        {
                            name = xml.Value;
                            continue;
                        }
                    case "Type":
                        {
                            str2 = GetString(xml.Value);
                            continue;
                        }
                    case "Types":
                        {
                            str2 = xml.Value;
                            continue;
                        }
                    case "Value":
                        switch (str2)
                        {
                            case "String":
                                {
                                    @bool = GetString(xml.Value);
                                    continue;
                                }
                            case "Integer":
                                {
                                    @bool = GetInt(xml.Value);
                                    continue;
                                }
                            case "Boolean":
                                {
                                    @bool = GetBool(xml.Value);
                                    continue;
                                }
                            case "Font":
                                {
                                    @bool = GetFont(xml.Value);
                                    continue;
                                }
                            case "Hue":
                                {
                                    @bool = GetHue(xml.Value);
                                    continue;
                                }
                            case "Gump":
                                {
                                    @bool = GetGump(xml.Value);
                                    continue;
                                }
                        }
                        return false;
                }
                return false;
            }
            if (!flag)
            {
                Skip(xml);
            }
            Parent.SetTag(name, @bool);
            return true;
        }

        private static bool Parse_TextBox(XmlTextReader xml, Gump Parent, string Name)
        {
            int gumpID = 0;
            bool hasBorder = false;
            int x = 0;
            int y = 0;
            int width = 0;
            int height = 0;
            string startText = "";
            char passChar = '\0';
            bool flag2 = false;
            Gump gumps = null;
            IFont defaultFont = Engine.DefaultFont;
            IHue defaultHue = Engine.DefaultHue;
            IHue hOver = Engine.DefaultHue;
            IHue hFocus = Engine.DefaultHue;
            bool flag3 = xml.IsEmptyElement;
            while (xml.MoveToNextAttribute())
            {
                switch (xml.Name)
                {
                    case "GumpID":
                        {
                            gumpID = GetInt(xml.Value);
                            continue;
                        }
                    case "GumpIDi":
                        {
                            gumpID = Convert.ToInt32(xml.Value);
                            continue;
                        }
                    case "GumpIDh":
                        {
                            gumpID = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            continue;
                        }
                    case "Width":
                        {
                            width = GetInt(xml.Value);
                            continue;
                        }
                    case "Widthi":
                        {
                            width = Convert.ToInt32(xml.Value);
                            continue;
                        }
                    case "Widthh":
                        {
                            width = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            continue;
                        }
                    case "Height":
                        {
                            height = GetInt(xml.Value);
                            continue;
                        }
                    case "Heighti":
                        {
                            height = Convert.ToInt32(xml.Value);
                            continue;
                        }
                    case "Heighth":
                        {
                            height = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            continue;
                        }
                    case "X":
                        {
                            x = GetInt(xml.Value);
                            continue;
                        }
                    case "Xi":
                        {
                            x = Convert.ToInt32(xml.Value);
                            continue;
                        }
                    case "Xh":
                        {
                            x = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            continue;
                        }
                    case "Y":
                        {
                            y = GetInt(xml.Value);
                            continue;
                        }
                    case "Yi":
                        {
                            y = Convert.ToInt32(xml.Value);
                            continue;
                        }
                    case "Yh":
                        {
                            y = Convert.ToInt32(xml.Value.Substring(2), 0x10);
                            continue;
                        }
                    case "Border":
                        {
                            hasBorder = GetBool(xml.Value);
                            continue;
                        }
                    case "Borderb":
                        {
                            hasBorder = Convert.ToBoolean(xml.Value);
                            continue;
                        }
                    case "PassChar":
                        {
                            passChar = GetString(xml.Value)[0];
                            flag2 = true;
                            continue;
                        }
                    case "PassChars":
                        {
                            passChar = xml.Value[0];
                            flag2 = true;
                            continue;
                        }
                    case "Text":
                        {
                            startText = GetString(xml.Value);
                            continue;
                        }
                    case "Texts":
                        {
                            startText = xml.Value;
                            continue;
                        }
                    case "Font":
                        {
                            defaultFont = GetFont(xml.Value);
                            continue;
                        }
                    case "Fonti":
                        {
                            defaultFont = GetFonti(xml.Value);
                            continue;
                        }
                    case "Fonth":
                        {
                            defaultFont = GetFonth(xml.Value);
                            continue;
                        }
                    case "RegularHue":
                        {
                            defaultHue = GetHue(xml.Value);
                            continue;
                        }
                    case "RegularHuei":
                        {
                            defaultHue = GetHuei(xml.Value);
                            continue;
                        }
                    case "RegularHueh":
                        {
                            defaultHue = GetHueh(xml.Value);
                            continue;
                        }
                    case "OverHue":
                        {
                            hOver = GetHue(xml.Value);
                            continue;
                        }
                    case "OverHuei":
                        {
                            hOver = GetHuei(xml.Value);
                            continue;
                        }
                    case "OverHueh":
                        {
                            hOver = GetHueh(xml.Value);
                            continue;
                        }
                    case "FocusHue":
                        {
                            hFocus = GetHue(xml.Value);
                            continue;
                        }
                    case "FocusHuei":
                        {
                            hFocus = GetHuei(xml.Value);
                            continue;
                        }
                    case "FocusHueh":
                        {
                            hFocus = GetHueh(xml.Value);
                            continue;
                        }
                    case "EnterButton":
                        {
                            gumps = GetGump(xml.Value);
                            continue;
                        }
                    case "EnterButtons":
                        {
                            gumps = GetGumps(xml.Value);
                            continue;
                        }
                }
                return false;
            }
            GTextBox toAdd = null;
            if (!flag2)
            {
                toAdd = new GTextBox(gumpID, hasBorder, x, y, width, height, startText, defaultFont, defaultHue, hOver, hFocus);
            }
            else
            {
                toAdd = new GTextBox(gumpID, hasBorder, x, y, width, height, startText, defaultFont, defaultHue, hOver, hFocus, passChar);
            }
            if (gumps.GetType() == typeof(GButton))
            {
                toAdd.EnterButton = (GButton)gumps;
            }
            Parent.Children.Add(toAdd);
            if (flag3)
            {
                return true;
            }
            while (xml.Read())
            {
                XmlNodeType nodeType = xml.NodeType;
                if (nodeType != XmlNodeType.Element)
                {
                    if (nodeType != XmlNodeType.Comment)
                    {
                        return (nodeType == XmlNodeType.EndElement);
                    }
                }
                else if (!Parse_Element(xml, toAdd, Name))
                {
                    return false;
                }
            }
            return false;
        }

        private static bool Parse_TextButton(XmlTextReader xml, Gump Parent, string Name)
        {
            string text = "";
            int x = 0;
            int y = 0;
            string method = "";
            ITooltip tooltip = null;
            IHue defaultHue = Engine.DefaultHue;
            IHue focusHue = Engine.DefaultHue;
            IFont defaultFont = Engine.DefaultFont;
            bool flag = xml.IsEmptyElement;
            while (xml.MoveToNextAttribute())
            {
                switch (xml.Name)
                {
                    case "Text":
                        {
                            text = GetString(xml.Value);
                            continue;
                        }
                    case "RegularHue":
                        {
                            defaultHue = GetHue(xml.Value);
                            continue;
                        }
                    case "OverHue":
                        {
                            focusHue = GetHue(xml.Value);
                            continue;
                        }
                    case "Font":
                        {
                            defaultFont = GetFont(xml.Value);
                            continue;
                        }
                    case "X":
                        {
                            x = GetInt(xml.Value);
                            continue;
                        }
                    case "Y":
                        {
                            y = GetInt(xml.Value);
                            continue;
                        }
                    case "OnClick":
                        {
                            method = GetString(xml.Value);
                            continue;
                        }
                    case "Tooltip":
                        {
                            tooltip = new Tooltip(GetString(xml.Value));
                            continue;
                        }
                }
                return false;
            }
            OnClick onClick = null;
            try
            {
                if (method.Length > 0)
                {
                    onClick = (OnClick)Delegate.CreateDelegate(typeof(OnClick), typeof(Engine), method);
                }
            }
            catch
            {
                onClick = null;
            }
            GTextButton toAdd = new GTextButton(text, defaultFont, defaultHue, focusHue, x, y, onClick);
            toAdd.Tooltip = tooltip;
            Parent.Children.Add(toAdd);
            if (flag)
            {
                return true;
            }
            while (xml.Read())
            {
                XmlNodeType nodeType = xml.NodeType;
                if (nodeType != XmlNodeType.Element)
                {
                    if (nodeType != XmlNodeType.Comment)
                    {
                        return (nodeType == XmlNodeType.EndElement);
                    }
                }
                else if (!Parse_Element(xml, toAdd, Name))
                {
                    return false;
                }
            }
            return false;
        }

        private static bool Parse_Tooltip(XmlTextReader xml, Gump Parent)
        {
            string text = "";
            bool flag = xml.IsEmptyElement;
            while (xml.MoveToNextAttribute())
            {
                string str2;
                if (((str2 = xml.Name) != null) && (string.IsInterned(str2) == "Text"))
                {
                    text = GetString(xml.Value);
                }
                else
                {
                    return false;
                }
            }
            Parent.Tooltip = new Tooltip(text);
            if (!flag)
            {
                Skip(xml);
            }
            return true;
        }

        private static bool Parse_xGumps(XmlTextReader xml)
        {
            while (xml.Read())
            {
                switch (xml.NodeType)
                {
                    case XmlNodeType.Element:
                        string str;
                        if (((str = xml.Name) != null) && (string.IsInterned(str) == "Gumps"))
                        {
                            if (!Parse_xGumps_Gumps(xml))
                            {
                                return false;
                            }
                            continue;
                        }
                        return false;

                    case XmlNodeType.Comment:
                    case XmlNodeType.XmlDeclaration:
                        {
                            continue;
                        }
                }
                return false;
            }
            return true;
        }

        private static bool Parse_xGumps_Gump(XmlTextReader xml)
        {
            string key = "";
            string str2 = "";
            bool flag = xml.IsEmptyElement;
            while (xml.MoveToNextAttribute())
            {
                switch (xml.Name)
                {
                    case "Name":
                        {
                            key = xml.Value;
                            continue;
                        }
                    case "File":
                        {
                            str2 = xml.Value;
                            continue;
                        }
                }
                return false;
            }
            if (((key != null) && (key.Length > 0)) && ((str2 != null) && (str2.Length > 0)))
            {
                m_xFiles.Add(key, str2);
            }
            if (!flag)
            {
                Skip(xml);
                return false;
            }
            return true;
        }

        private static bool Parse_xGumps_Gumps(XmlTextReader xml)
        {
            if (!xml.IsEmptyElement)
            {
                while (xml.Read())
                {
                    XmlNodeType nodeType = xml.NodeType;
                    switch (nodeType)
                    {
                        case XmlNodeType.Element:
                            string str;
                            if (((str = xml.Name) != null) && (string.IsInterned(str) == "Gump"))
                            {
                                if (!Parse_xGumps_Gump(xml))
                                {
                                    return false;
                                }
                                continue;
                            }
                            return false;

                        case XmlNodeType.Comment:
                            {
                                continue;
                            }
                    }
                    return (nodeType == XmlNodeType.EndElement);
                }
            }
            return false;
        }

        public static void SetVariable(string Name, string Value)
        {
            m_Variables[Name] = Value;
        }

        private static string SizeOfHeight_Replace(Match m)
        {
            try
            {
                string str = m.Value.Substring(8);
                int index = GetInt(str.Substring(0, str.Length - 9));
                int num2 = Engine.m_Gumps.m_Index[index].m_Extra & 0xffff;
                return num2.ToString();
            }
            catch
            {
                return "";
            }
        }

        private static string SizeOfWidth_Replace(Match m)
        {
            try
            {
                string str = m.Value.Substring(8);
                int index = GetInt(str.Substring(0, str.Length - 8));
                int num2 = (Engine.m_Gumps.m_Index[index].m_Extra >> 0x10) & 0xffff;
                return num2.ToString();
            }
            catch
            {
                return "";
            }
        }

        private static void Skip(XmlTextReader xml)
        {
            int num = 0;
            while (xml.Read())
            {
                if ((xml.NodeType == XmlNodeType.Element) && !xml.IsEmptyElement)
                {
                    num++;
                }
                else if (xml.NodeType == XmlNodeType.EndElement)
                {
                    num--;
                }
                if (num < 0)
                {
                    break;
                }
            }
        }
    }
}