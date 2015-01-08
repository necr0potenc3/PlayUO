namespace Client
{
    using System;
    using System.IO;
    using System.Xml;

    public class SpellList
    {
        private int m_Circles;
        private bool m_DisplayCircles;
        private bool m_DisplayIndex;
        private int m_SpellID;
        private Spell[] m_Spells;
        private int m_SpellsPerCircle;
        private int m_Start;

        public SpellList(string name)
        {
            string path = Engine.FileManager.BasePath(string.Format("Data/Spells/{0}.xml", name));
            bool flag = true;
            if (File.Exists(path))
            {
                XmlTextReader xml = new XmlTextReader(path) {
                    WhitespaceHandling = WhitespaceHandling.None
                };
                flag = !this.Parse(xml);
                xml.Close();
            }
            if (flag)
            {
                this.m_Circles = 0;
                this.m_DisplayCircles = false;
                this.m_DisplayIndex = false;
                this.m_Spells = new Spell[0];
                this.m_SpellID = 0;
            }
        }

        private bool Parse(XmlTextReader xml)
        {
            while (xml.Read())
            {
                switch (xml.NodeType)
                {
                    case XmlNodeType.Element:
                        string str;
                        if (((str = xml.Name) != null) && (string.IsInterned(str) == "spells"))
                        {
                            if (!this.Parse_Spells(xml))
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

        private bool Parse_Mana(XmlTextReader xml, Spell spell)
        {
            string str = null;
            while (xml.MoveToNextAttribute())
            {
                string str2;
                if (((str2 = xml.Name) != null) && (string.IsInterned(str2) == "value"))
                {
                    str = xml.Value;
                }
                else
                {
                    return false;
                }
            }
            if (str == null)
            {
                return false;
            }
            spell.Mana = Convert.ToInt32(str);
            return true;
        }

        private bool Parse_Reagent(XmlTextReader xml, Spell spell)
        {
            string name = null;
            while (xml.MoveToNextAttribute())
            {
                string str2;
                if (((str2 = xml.Name) != null) && (string.IsInterned(str2) == "name"))
                {
                    name = xml.Value;
                }
                else
                {
                    return false;
                }
            }
            if (name == null)
            {
                return false;
            }
            spell.Reagents.Add(new Reagent(name));
            return true;
        }

        private bool Parse_Skill(XmlTextReader xml, Spell spell)
        {
            string str = null;
            while (xml.MoveToNextAttribute())
            {
                string str2;
                if (((str2 = xml.Name) != null) && (string.IsInterned(str2) == "value"))
                {
                    str = xml.Value;
                }
                else
                {
                    return false;
                }
            }
            if (str == null)
            {
                return false;
            }
            spell.Skill = Convert.ToInt32(str);
            return true;
        }

        private bool Parse_Spell(XmlTextReader xml)
        {
            string name = null;
            string power = null;
            while (xml.MoveToNextAttribute())
            {
                switch (xml.Name)
                {
                    case "name":
                    {
                        name = xml.Value;
                        continue;
                    }
                    case "power":
                    {
                        power = xml.Value;
                        continue;
                    }
                }
                return false;
            }
            if (name != null)
            {
                if (power == null)
                {
                    power = "";
                }
                Spell spell = new Spell(name, power, this.m_Start + this.m_SpellID);
                this.m_Spells[this.m_SpellID++] = spell;
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
                    else
                    {
                        switch (xml.Name)
                        {
                            case "reagent":
                            {
                                if (!this.Parse_Reagent(xml, spell))
                                {
                                    return false;
                                }
                                continue;
                            }
                            case "tithing":
                            {
                                if (!this.Parse_Tithing(xml, spell))
                                {
                                    return false;
                                }
                                continue;
                            }
                            case "skill":
                            {
                                if (!this.Parse_Skill(xml, spell))
                                {
                                    return false;
                                }
                                continue;
                            }
                            case "mana":
                                if (this.Parse_Mana(xml, spell))
                                {
                                    continue;
                                }
                                return false;
                        }
                        return false;
                    }
                }
            }
            return false;
        }

        private bool Parse_Spells(XmlTextReader xml)
        {
            string name;
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            bool flag = false;
            bool flag2 = false;
            while (xml.MoveToNextAttribute())
            {
                name = xml.Name;
                if (name != null)
                {
                    name = string.IsInterned(name);
                    if (name == "circles")
                    {
                        num = Convert.ToInt32(xml.Value);
                        continue;
                    }
                    if (name == "count")
                    {
                        num2 = Convert.ToInt32(xml.Value);
                        continue;
                    }
                    if (name == "start")
                    {
                        num3 = Convert.ToInt32(xml.Value);
                        continue;
                    }
                    if (name == "spellsPerCircle")
                    {
                        num4 = Convert.ToInt32(xml.Value);
                        continue;
                    }
                    if (name == "displayCircles")
                    {
                        flag = Convert.ToBoolean(xml.Value);
                        continue;
                    }
                    if (name == "displayIndex")
                    {
                        flag2 = Convert.ToBoolean(xml.Value);
                        continue;
                    }
                }
                return false;
            }
            this.m_Circles = num;
            this.m_DisplayCircles = flag;
            this.m_DisplayIndex = flag2;
            this.m_Start = num3;
            this.m_SpellsPerCircle = num4;
            this.m_Spells = new Spell[num2];
            while (xml.Read())
            {
                XmlNodeType nodeType = xml.NodeType;
                switch (nodeType)
                {
                    case XmlNodeType.Element:
                        if (((name = xml.Name) != null) && (string.IsInterned(name) == "spell"))
                        {
                            if (!this.Parse_Spell(xml))
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
            return false;
        }

        private bool Parse_Tithing(XmlTextReader xml, Spell spell)
        {
            string str = null;
            while (xml.MoveToNextAttribute())
            {
                string str2;
                if (((str2 = xml.Name) != null) && (string.IsInterned(str2) == "value"))
                {
                    str = xml.Value;
                }
                else
                {
                    return false;
                }
            }
            if (str == null)
            {
                return false;
            }
            spell.Tithing = Convert.ToInt32(str);
            return true;
        }

        public int Circles
        {
            get
            {
                return this.m_Circles;
            }
        }

        public bool DisplayCircles
        {
            get
            {
                return this.m_DisplayCircles;
            }
        }

        public bool DisplayIndex
        {
            get
            {
                return this.m_DisplayIndex;
            }
        }

        public Spell[] Spells
        {
            get
            {
                return this.m_Spells;
            }
        }

        public int SpellsPerCircle
        {
            get
            {
                return this.m_SpellsPerCircle;
            }
        }

        public int Start
        {
            get
            {
                return this.m_Start;
            }
        }
    }
}

