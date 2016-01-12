namespace Client
{
    using System;
    using System.Text;
    using System.Windows.Forms;

    public class Spells
    {
        private static SpellList m_NecromancerList;
        private static string[] m_Numbers;
        private static SpellList m_PaladinList;
        private static Reagent[] m_Reagents;
        private static SpellList m_RegularList;

        static Spells()
        {
            Debug.TimeBlock("Initializing Spells");
            m_Reagents = new Reagent[] { new Reagent("Black pearl", 0xf7a), new Reagent("Bloodmoss", 0xf7b), new Reagent("Garlic", 0xf84), new Reagent("Ginseng", 0xf85), new Reagent("Mandrake root", 0xf86), new Reagent("Nightshade", 0xf88), new Reagent("Sulfurous ash", 0xf8c), new Reagent("Spiders' silk", 0xf8d), new Reagent("Bat wing", 0xf78), new Reagent("Grave dust", 0xf8f), new Reagent("Daemon blood", 0xf7d), new Reagent("Nox crystal", 0xf8e), new Reagent("Pig iron", 0xf8a) };
            m_Numbers = new string[] { "First", "Second", "Third", "Fourth", "Fifth", "Sixth", "Seventh", "Eighth" };
            Debug.EndBlock();
        }

        private static void ChangeCircle_OnClick(Gump sender)
        {
            Gump parent = sender.Parent;
            Item tag = (Item)parent.GetTag("Container");
            object obj2 = sender.GetTag("Circle");
            if (tag != null)
            {
                int circle = (obj2 == null) ? 0 : ((int)obj2);
                int x = parent.X;
                int y = parent.Y;
                Gumps.Destroy(parent);
                Gump toAdd = OpenSpellbook(circle, tag.LastSpell, tag);
                toAdd.X = x;
                toAdd.Y = y;
                Gumps.Desktop.Children.Add(toAdd);
            }
        }

        public static int GetBookIcon(int itemID)
        {
            switch (GetBookType(itemID))
            {
                case 2:
                    return 0x2b03;

                case 3:
                    return 0x2b04;
            }
            return 0x8ba;
        }

        public static int GetBookIndex(int itemID)
        {
            switch (GetBookType(itemID))
            {
                case 2:
                    return 0x2b00;

                case 3:
                    return 0x2b01;
            }
            return 0x8ac;
        }

        public static int GetBookOffset(int itemID)
        {
            switch (GetBookType(itemID))
            {
                case 2:
                    return 0x65;

                case 3:
                    return 0xc9;
            }
            return 1;
        }

        public static int GetBookType(int itemID)
        {
            switch (itemID)
            {
                case 0x2252:
                    return 3;

                case 0x2253:
                    return 2;

                case 0xefa:
                case 0xe3b:
                    return 1;
            }
            return 0;
        }

        public static Reagent GetReagent(string Name)
        {
            switch (Name)
            {
                case "Black pearl":
                    return m_Reagents[0];

                case "Bloodmoss":
                    return m_Reagents[1];

                case "Garlic":
                    return m_Reagents[2];

                case "Ginseng":
                    return m_Reagents[3];

                case "Mandrake root":
                    return m_Reagents[4];

                case "Nightshade":
                    return m_Reagents[5];

                case "Sulfurous ash":
                    return m_Reagents[6];

                case "Spiders' silk":
                    return m_Reagents[7];

                case "Bat wing":
                    return m_Reagents[8];

                case "Grave dust":
                    return m_Reagents[9];

                case "Daemon blood":
                    return m_Reagents[10];

                case "Nox crystal":
                    return m_Reagents[11];

                case "Pig iron":
                    return m_Reagents[12];
            }
            throw new ArgumentException("Invalid reagent name. Valid values are: [\"Black pearl\", \"Bloodmoss\", \"Garlic\", \"Ginseng\", \"Mandrake root\", \"Nightshade\", \"Sulfurous ash\", \"Spiders' silk\"].", Name);
        }

        public static Spell GetSpellByID(int spellID)
        {
            Spell spellByID = GetSpellByID(RegularList, spellID);
            if (spellByID == null)
            {
                spellByID = GetSpellByID(PaladinList, spellID);
            }
            if (spellByID == null)
            {
                spellByID = GetSpellByID(NecromancerList, spellID);
            }
            return spellByID;
        }

        public static Spell GetSpellByID(SpellList list, int spellID)
        {
            if (spellID < list.Start)
            {
                return null;
            }
            spellID -= list.Start;
            if (spellID >= list.Spells.Length)
            {
                return null;
            }
            return list.Spells[spellID];
        }

        public static Spell GetSpellByName(string name)
        {
            Spell spellByName = GetSpellByName(RegularList, name);
            if (spellByName == null)
            {
                spellByName = GetSpellByName(PaladinList, name);
            }
            if (spellByName == null)
            {
                spellByName = GetSpellByName(NecromancerList, name);
            }
            return spellByName;
        }

        public static Spell GetSpellByName(SpellList list, string name)
        {
            for (int i = 0; i < list.Spells.Length; i++)
            {
                if (list.Spells[i].Name == name)
                {
                    return list.Spells[i];
                }
            }
            return null;
        }

        public static Spell GetSpellByPower(string power)
        {
            Spell spellByPower = GetSpellByPower(RegularList, power);
            if (spellByPower == null)
            {
                spellByPower = GetSpellByPower(PaladinList, power);
            }
            if (spellByPower == null)
            {
                spellByPower = GetSpellByPower(NecromancerList, power);
            }
            return spellByPower;
        }

        public static Spell GetSpellByPower(SpellList list, string power)
        {
            for (int i = 0; i < list.Spells.Length; i++)
            {
                if (list.Spells[i].FullPower == power)
                {
                    return list.Spells[i];
                }
            }
            return null;
        }

        public static Gump OpenSpellbook(Item container)
        {
            Gump toAdd = OpenSpellbook(container.Circle, container.LastSpell, container);
            toAdd.X = (Engine.ScreenWidth - toAdd.Width) / 2;
            toAdd.Y = (Engine.ScreenHeight - toAdd.Height) / 2;
            Gumps.Desktop.Children.Add(toAdd);
            return toAdd;
        }

        public static Gump OpenSpellbook(int circle, int lastSpell, Item container)
        {
            SpellList necromancerList;
            container.OpenSB = true;
            container.Circle = circle;
            container.LastSpell = lastSpell;
            circle &= -2;
            Engine.Sounds.PlaySound(0x55);
            Engine.DoEvents();
            GDragable dragable = new GDragable(GetBookIndex(container.ID), 0, 0);
            dragable.SetTag("Container", container);
            dragable.SetTag("Dispose", "Spellbook");
            dragable.Children.Add(new GMinimizer());
            if (container.SpellbookOffset == 0x65)
            {
                necromancerList = NecromancerList;
            }
            else if (container.SpellbookOffset == 0xc9)
            {
                necromancerList = PaladinList;
            }
            else
            {
                necromancerList = RegularList;
            }
            if ((lastSpell >= necromancerList.Start) && (lastSpell < (necromancerList.Start + necromancerList.Spells.Length)))
            {
                int num = (lastSpell - necromancerList.Start) / necromancerList.SpellsPerCircle;
                int num2 = (lastSpell - necromancerList.Start) % necromancerList.SpellsPerCircle;
                if ((num >= 0) && (num < necromancerList.Circles))
                {
                    if (num == circle)
                    {
                        dragable.Children.Add(new GImage(0x8ad, 0xb8, 2));
                        dragable.Children.Add(new GImage(0x8af, 0xb7, 0x34 + (num2 * 15)));
                    }
                    else if (num == (circle + 1))
                    {
                        dragable.Children.Add(new GImage(0x8ae, 0xcc, 3));
                        dragable.Children.Add(new GImage(0x8b0, 0xcc, 0x34 + (num2 * 15)));
                    }
                }
            }
            dragable.Children.Add(new GLabel("INDEX", Engine.GetFont(6), Hues.Default, 0x6a, 10));
            dragable.Children.Add(new GLabel("INDEX", Engine.GetFont(6), Hues.Default, 0x10d, 10));
            OnClick clickHandler = new OnClick(Spells.ChangeCircle_OnClick);
            int[] numArray = new int[] { 0x3a, 0x5d, 130, 0xa4, 0xe3, 260, 0x129, 0x14c };
            int[] numArray2 = new int[] { 0x34, 0x34 };
            if (necromancerList.DisplayIndex)
            {
                for (int j = 0; j < necromancerList.Circles; j++)
                {
                    GButton toAdd = new GButton(0x8b1 + j, 0x8b1 + j, 0x8b1 + j, numArray[j], 0xaf, clickHandler);
                    toAdd.SetTag("Circle", j);
                    dragable.Children.Add(toAdd);
                }
            }
            if (necromancerList.DisplayCircles)
            {
                if (circle > 0)
                {
                    GButton button2 = new GButton(0x8bb, 0x8bb, 0x8bb, 50, 8, clickHandler);
                    button2.SetTag("Circle", circle - 1);
                    dragable.Children.Add(button2);
                }
                if (circle < ((necromancerList.Circles - 1) & -2))
                {
                    GButton button3 = new GButton(0x8bc, 0x8bc, 0x8bc, 0x141, 8, clickHandler);
                    button3.SetTag("Circle", circle + 2);
                    dragable.Children.Add(button3);
                }
                for (int k = circle; k < (circle + 2); k++)
                {
                    string str;
                    int x = ((k & 1) == 0) ? 0x3e : 0xe1;
                    if ((k < 0) || (k >= m_Numbers.Length))
                    {
                        str = "Bad";
                    }
                    else
                    {
                        str = m_Numbers[k];
                    }
                    dragable.Children.Add(new GLabel(string.Format("{0} Circle", str), Engine.GetFont(6), Hues.Default, x, 30));
                }
            }
            int num6 = circle * necromancerList.SpellsPerCircle;
            int num7 = (circle + 2) * necromancerList.SpellsPerCircle;
            for (int i = num6; i < num7; i++)
            {
                if (((i >= num6) && (i < num7)) && container.GetSpellContained(i))
                {
                    int num9 = i / necromancerList.SpellsPerCircle;
                    Spell spellByID = GetSpellByID(container.SpellbookOffset + i);
                    if (spellByID != null)
                    {
                        IntPtr ptr;
                        GSpellName name = new GSpellName(container.SpellbookOffset + i, spellByID.Name, Engine.GetFont(9), Hues.Load(0x288), Hues.Load(0x28b), 0x3e + ((num9 & 1) * 0xa3), numArray2[num9 & 1]);
                        numArray2[(int)(ptr = (IntPtr)(num9 & 1))] = numArray2[(int)ptr] + 15;
                        string str2 = string.Format("{0}\n", spellByID.Name);
                        StringBuilder builder = new StringBuilder();
                        builder.Append(spellByID.Name);
                        builder.Append('\n');
                        for (int m = 0; m < spellByID.Power.Length; m++)
                        {
                            builder.Append(spellByID.Power[m].Name);
                            builder.Append(' ');
                        }
                        for (int n = 0; n < spellByID.Reagents.Count; n++)
                        {
                            builder.Append('\n');
                            Reagent reagent = (Reagent)spellByID.Reagents[n];
                            builder.Append(reagent.Name);
                        }
                        if (spellByID.Tithing > 0)
                        {
                            builder.Append('\n');
                            builder.AppendFormat("Tithing: {0}", spellByID.Tithing);
                        }
                        if (spellByID.Mana > 0)
                        {
                            builder.Append('\n');
                            builder.AppendFormat("Mana: {0}", spellByID.Mana);
                        }
                        if (spellByID.Skill > 0)
                        {
                            builder.Append('\n');
                            builder.AppendFormat("Skill: {0}", spellByID.Skill);
                        }
                        Tooltip tooltip = new Tooltip(builder.ToString(), true);
                        name.Tooltip = tooltip;
                        dragable.Children.Add(name);
                    }
                }
            }
            return dragable;
        }

        public static SpellList NecromancerList
        {
            get
            {
                if (m_NecromancerList == null)
                {
                    m_NecromancerList = new SpellList("necromancer");
                }
                return m_NecromancerList;
            }
        }

        public static SpellList PaladinList
        {
            get
            {
                if (m_PaladinList == null)
                {
                    m_PaladinList = new SpellList("paladin");
                }
                return m_PaladinList;
            }
        }

        public static Reagent[] Reagents
        {
            get
            {
                return m_Reagents;
            }
        }

        public static SpellList RegularList
        {
            get
            {
                if (m_RegularList == null)
                {
                    m_RegularList = new SpellList("spells");
                }
                return m_RegularList;
            }
        }

        private class GMinimizer : GRegion
        {
            public GMinimizer() : base(4, 0x65, 0x11, 0x11)
            {
                base.m_Tooltip = new Tooltip("Minimize");
            }

            protected internal override void OnMouseUp(int x, int y, MouseButtons mb)
            {
                if ((mb & MouseButtons.Left) != MouseButtons.None)
                {
                    base.m_Parent.Visible = false;
                    Gumps.Desktop.Children.Add(new GSpellbookIcon(base.m_Parent, (Item)base.m_Parent.GetTag("Container")));
                }
            }
        }
    }
}