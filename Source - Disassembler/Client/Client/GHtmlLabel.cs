namespace Client
{
    using System;
    using System.Collections;

    public class GHtmlLabel : Gump
    {
        private static object[,] m_ColorTable = new object[,] { 
            { "black", 0 }, { "red", 0xff0000 }, { "lime", 0xff00 }, { "blue", 0xff }, { "yellow", 0xffff00 }, { "magenta", 0xff00ff }, { "fuchsia", 0xff00ff }, { "cyan", 0xffff }, { "aqua", 0xffff }, { "white", 0xffffff }, { "darkred", 0x7f0000 }, { "maroon", 0x7f0000 }, { "green", 0x7f00 }, { "darkgreen", 0x5a00 }, { "darkblue", 0x7f00 }, { "navy", 0x7f00 }, 
            { "darkyellow", 0x7f7f00 }, { "olive", 0x7f7f00 }, { "darkmagenta", 0x7f007f }, { "purple", 0x7f007f }, { "darkcyan", 0x7f7f }, { "teal", 0x7f7f }, { "gray", 0x7f7f7f }, { "grey", 0x7f7f7f }
         };
        private int m_Height;
        private int m_Width;

        public GHtmlLabel(string text, UnicodeFont initialFont, int initialColor, int x, int y, int width) : base(x, y)
        {
            this.m_Width = width;
            this.Update(text, initialFont, initialColor);
        }

        public void Scissor(int x, int y, int w, int h)
        {
            foreach (GLabel label in base.m_Children.ToArray())
            {
                label.Scissor(x - label.X, y - label.Y, w, h);
            }
        }

        public void Update(string text, UnicodeFont initialFont, int initialColor)
        {
            int width = this.m_Width;
            Stack stack = new Stack();
            FontInfo info = new FontInfo(initialFont, initialColor);
            int num2 = 0;
            int num3 = 0;
            Stack stack2 = new Stack();
            Stack stack3 = new Stack();
            text = text.Replace("\r", "");
            text = text.Replace("\n", "<br>");
            HtmlElement[] elements = HtmlElement.GetElements(text);
            int num4 = 0;
            int y = 0;
            for (int i = 0; i < elements.Length; i++)
            {
                string name;
                bool flag;
                int stringWidth;
                string str7;
                string str8;
                string attribute;
                string str10;
                HtmlElement element = elements[i];
                FontInfo info2 = (stack.Count > 0) ? ((FontInfo) stack.Peek()) : info;
                HtmlAlignment alignment = (stack2.Count > 0) ? ((HtmlAlignment) stack2.Peek()) : HtmlAlignment.Normal;
                string url = (stack3.Count > 0) ? ((string) stack3.Peek()) : null;
                switch (element.Type)
                {
                    case ElementType.Text:
                        name = element.Name;
                        flag = false;
                        goto Label_02F6;

                    case ElementType.Start:
                    {
                        str10 = element.Name.ToLower();
                        if (str10 != null)
                        {
                            str10 = string.IsInterned(str10);
                            if (str10 == "br")
                            {
                                num4 = 0;
                                y += 0x12;
                            }
                            else
                            {
                                if (str10 == "u")
                                {
                                    goto Label_03A3;
                                }
                                if (str10 == "i")
                                {
                                    goto Label_03AC;
                                }
                                if (str10 == "a")
                                {
                                    goto Label_03B7;
                                }
                                if (str10 == "basefont")
                                {
                                    goto Label_03E5;
                                }
                                if (str10 == "center")
                                {
                                    goto Label_048A;
                                }
                                if (str10 == "div")
                                {
                                    goto Label_049C;
                                }
                            }
                        }
                        continue;
                    }
                    case ElementType.End:
                    {
                        str10 = element.Name.ToLower();
                        if (str10 != null)
                        {
                            str10 = string.IsInterned(str10);
                            if (str10 == "u")
                            {
                                num2--;
                                if (num2 < 0)
                                {
                                    num2 = 0;
                                }
                            }
                            else
                            {
                                if (str10 == "i")
                                {
                                    goto Label_0610;
                                }
                                if (str10 == "a")
                                {
                                    goto Label_0620;
                                }
                                if (str10 == "basefont")
                                {
                                    goto Label_0634;
                                }
                                if ((str10 == "div") || (str10 == "center"))
                                {
                                    goto Label_0646;
                                }
                            }
                        }
                        continue;
                    }
                    default:
                    {
                        continue;
                    }
                }
            Label_00EB:
                stringWidth = num4;
                switch ((alignment & ((HtmlAlignment) 0xff)))
                {
                    case HtmlAlignment.Center:
                        stringWidth = info2.Font.GetStringWidth(name);
                        if (stringWidth > width)
                        {
                            string[] strArray = Engine.WrapText(name, width, info2.Font).Split(new char[] { '\n' });
                            stringWidth = info2.Font.GetStringWidth(strArray[0]);
                        }
                        stringWidth = ((width - (stringWidth - 1)) + 1) / 2;
                        break;

                    case HtmlAlignment.Left:
                        stringWidth = ((int) alignment) >> 8;
                        break;

                    case HtmlAlignment.Right:
                        stringWidth = info2.Font.GetStringWidth(name);
                        if (stringWidth > width)
                        {
                            string[] strArray2 = Engine.WrapText(name, width, info2.Font).Split(new char[] { '\n' });
                            stringWidth = info2.Font.GetStringWidth(strArray2[0]);
                        }
                        stringWidth = (((int) alignment) >> 8) - stringWidth;
                        break;
                }
                string[] strArray3 = name.Split(new char[] { ' ' });
                int num8 = width - stringWidth;
                if (!flag && (info2.Font.GetStringWidth(strArray3[0]) > num8))
                {
                    flag = true;
                    num4 = 0;
                    y += 0x12;
                }
                else
                {
                    flag = false;
                    strArray3 = Engine.WrapText(name, num8, info2.Font).Split(new char[] { '\n' });
                    string str6 = strArray3[0];
                    if (strArray3.Length > 1)
                    {
                        str6 = str6.TrimEnd(new char[0]);
                    }
                    GLabel toAdd = (url == null) ? new GLabel(str6, info2.Font, info2.Hue, stringWidth, y) : new GHyperLink(url, str6, info2.Font, stringWidth, y);
                    if (url == null)
                    {
                        toAdd.Underline = num2 > 0;
                    }
                    base.m_Children.Add(toAdd);
                    if (strArray3.Length > 1)
                    {
                        name = name.Remove(0, strArray3[0].Length);
                        num4 = 0;
                        y += 0x12;
                    }
                    else
                    {
                        num4 = (toAdd.X + toAdd.Width) - 1;
                        continue;
                    }
                }
            Label_02F6:
                if (name.Length > 0)
                {
                    goto Label_00EB;
                }
                continue;
            Label_03A3:
                num2++;
                continue;
            Label_03AC:
                num3++;
                continue;
            Label_03B7:
                str7 = element.GetAttribute("href");
                if ((str7 != null) && !str7.StartsWith("?"))
                {
                    stack3.Push(str7);
                }
                continue;
            Label_03E5:
                str8 = element.GetAttribute("color");
                if (str8 == null)
                {
                    continue;
                }
                int color = 0;
                if (str8.StartsWith("#"))
                {
                    color = Convert.ToInt32(str8.Substring(1), 0x10);
                }
                else
                {
                    for (int j = 0; j < m_ColorTable.GetLength(0); j++)
                    {
                        if (str8.ToLower() == ((string) m_ColorTable[j, 0]))
                        {
                            color = (int) m_ColorTable[j, 1];
                            break;
                        }
                    }
                }
                stack.Push(new FontInfo(info2.Font, color));
                continue;
            Label_048A:
                stack2.Push(HtmlAlignment.Center);
                continue;
            Label_049C:
                attribute = element.GetAttribute("align");
                if (attribute == null)
                {
                    attribute = element.GetAttribute("alignleft");
                    if (attribute != null)
                    {
                        try
                        {
                            int num11 = int.Parse(attribute);
                            stack2.Push(HtmlAlignment.Left | ((HtmlAlignment) (num11 << 8)));
                        }
                        catch
                        {
                        }
                    }
                    attribute = element.GetAttribute("alignright");
                    if (attribute != null)
                    {
                        try
                        {
                            int num12 = int.Parse(attribute);
                            stack2.Push(HtmlAlignment.Right | ((HtmlAlignment) (num12 << 8)));
                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    switch (attribute.ToLower())
                    {
                        case "center":
                            stack2.Push(HtmlAlignment.Center);
                            break;

                        case "right":
                            stack2.Push(HtmlAlignment.Right | ((HtmlAlignment) (width << 8)));
                            break;

                        case "left":
                            stack2.Push(HtmlAlignment.Left);
                            break;
                    }
                }
                continue;
            Label_0610:
                num3--;
                if (num3 < 0)
                {
                    num3 = 0;
                }
                continue;
            Label_0620:
                if (stack3.Count > 0)
                {
                    stack3.Pop();
                }
                continue;
            Label_0634:
                if (stack.Count > 0)
                {
                    stack.Pop();
                }
                continue;
            Label_0646:
                if (stack2.Count > 0)
                {
                    stack2.Pop();
                }
            }
            this.m_Height = y + 0x12;
        }

        public override int Height
        {
            get
            {
                return this.m_Height;
            }
            set
            {
                this.m_Height = value;
            }
        }

        public override int Width
        {
            get
            {
                return this.m_Width;
            }
            set
            {
                this.m_Width = value;
            }
        }
    }
}

