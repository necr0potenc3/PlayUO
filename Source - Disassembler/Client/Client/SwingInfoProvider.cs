namespace Client
{
    using System;

    public class SwingInfoProvider : InfoProvider
    {
        public SwingInfoProvider() : base("Swing Speeds")
        {
        }

        public override Gump CreateGump()
        {
            return new GEmpty(0, 0, 380, 380);
        }

        public override InfoInput[] CreateInputs()
        {
            object[] objArray = new object[] { new object[,] { { "Misc@Fists", 50 } }, new object[,] { { "Axes@Axe", 0x25 }, { "Axes@Battle Axe", 0x1f }, { "Axes@Double Axe", 0x21 }, { "Axes@Exec Axe", 0x21 }, { "Axes@Hatchet", 0x29 }, { "Axes@LB Axe", 0x1d }, { "Axes@Pickaxe", 0x23 }, { "Axes@2H Axe", 0x1f }, { "Axes@War Axe", 0x21 } }, new object[,] { { "Knives@Btch Knife", 0x31 }, { "Knives@Cleaver", 0x2e }, { "Knives@Dagger", 0x38 }, { "Knives@Skin Knife", 0x31 } }, new object[,] { { "Bashing@Club", 0x2c }, { "Bashing@Hmmr Pick", 0x1c }, { "Bashing@Mace", 40 }, { "Bashing@Wand", 40 }, { "Bashing@Maul", 0x20 }, { "Bashing@Scepter", 30 }, { "Bashing@War Hmmr", 0x1c }, { "Bashing@War Mace", 0x1a } }, new object[,] { { "Pole Arms@Bardiche", 0x1c }, { "Pole Arms@Halberd", 0x18 }, { "Pole Arms@Scythe", 0x20 }, { "Spears & Forks@Bld Staff", 0x25 }, { "Spears & Forks@Dbl Staff", 0x31 }, { "Spears & Forks@Pike", 0x25 }, { "Spears & Forks@Pitchfork", 0x2b }, { "Spears & Forks@Shrt Spear", 0x37 }, { "Spears & Forks@Spear", 0x2a }, { "Spears & Forks@War Fork", 0x2b } }, new object[,] { { "Ranged@Bow", 0x19 }, { "Ranged@Comp Bow", 0x19 }, { "Ranged@Crossbow", 0x18 }, { "Ranged@Heavy XBow", 0x16 }, { "Ranged@Rpt XBow", 0x29 } }, new object[,] { { "Staves@Black", 0x27 }, { "Staves@Gnarled", 0x21 }, { "Staves@Quarter", 0x30 }, { "Staves@Crook", 40 } }, new object[,] { { "Swords@Bone Hrvst", 0x24 }, { "Swords@Broadsword", 0x21 }, { "Swords@Crscnt Bld", 0x2f }, { "Swords@Cutlass", 0x2c }, { "Swords@Katana", 0x2e }, { "Swords@Kryss", 0x35 }, { "Swords@Lance", 0x18 }, { "Swords@Longsword", 30 }, { "Swords@Scimitar", 0x25 }, { "Swords@Viking Swrd", 0x1c } } };
            Table2DInfoNode[] nodes = new Table2DInfoNode[objArray.Length];
            for (int i = 0; i < objArray.Length; i++)
            {
                object[,] objArray2 = (object[,]) objArray[i];
                string[] cols = new string[] { "0.5", "1.0", "1.5", "2.0", "2.5", "3.0", "3.5", "4.0" };
                string[] rows = new string[objArray2.GetLength(0)];
                int[] speeds = new int[rows.Length];
                for (int j = 0; j < rows.Length; j++)
                {
                    string str = (string) objArray2[j, 0];
                    int num3 = (int) objArray2[j, 1];
                    int num4 = str.IndexOf('@');
                    rows[j] = str.Substring(++num4);
                    speeds[j] = num3;
                }
                string name = (string) objArray2[0, 0];
                int index = name.IndexOf('@');
                name = name.Substring(0, index);
                nodes[i] = new SwingSpeedInfoNode(name, cols, rows, speeds);
            }
            InfoInputTree tree = new InfoInputTree("Weapon Type", nodes);
            return new InfoInput[] { tree, new InfoInputText("Speed Bonus") };
        }

        public override void UpdateGump(Gump g)
        {
            g.Children.Clear();
            Table2DInfoNode active = base.Inputs[0].Active as Table2DInfoNode;
            if (active == null)
            {
                GLabel toAdd = new GLabel("Select a weapon type from the list above.", Engine.GetUniFont(1), GumpHues.WindowText, 0, 0);
                g.Children.Add(toAdd);
            }
            else
            {
                int num = 0;
                for (int i = 0; i < active.Descriptors.Length; i++)
                {
                    TableGump gump = new TableGump(active.Descriptors[i]) {
                        Y = num
                    };
                    num += gump.Height + 10;
                    g.Children.Add(gump);
                }
            }
        }
    }
}

