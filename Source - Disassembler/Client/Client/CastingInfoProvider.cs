namespace Client
{
    using System;

    public class CastingInfoProvider : InfoProvider
    {
        public CastingInfoProvider() : base("Casting Speeds")
        {
        }

        public override Gump CreateGump()
        {
            return new GEmpty(0, 0, 380, 380);
        }

        public override InfoInput[] CreateInputs()
        {
            InfoInputTree tree = new InfoInputTree("Spell Type", new Table2DInfoNode[] { new Table2DInfoNode("Magery", new TableDescriptor2D[] { new TableDescriptor2D(new string[] { "FC-3", "FC-2", "FC-1", "FC0", "FC1", "FC2", "FC3", "FC4" }, new string[] { "First", "Second", "Third", "Fourth", "Fifth", "Sixth", "Seventh", "Eighth" }, new TableComputeFunction(this.MageryCasting)), new TableDescriptor2D(new string[] { "FC-3", "FC-2", "FC-1", "FC0", "FC1", "FC2", "FC3", "FC4" }, new string[] { "Summons" }, new TableComputeFunction(this.MagerySummoning)), new TableDescriptor2D(new string[] { "FCR0", "FCR1", "FCR2", "FCR3", "FCR4", "FCR5", "FCR6", "FCR7" }, new string[] { "Recovery" }, new TableComputeFunction(this.MageryRecovery)) }), new Table2DInfoNode("Chivalry", new TableDescriptor2D[] { new TableDescriptor2D(new string[] { "Spell Names" }, new string[] { "First", "Second", "Third", "Fourth", "Fifth" }, new TableComputeFunction(this.PaladinNames)), new TableDescriptor2D(new string[] { "FC-3", "FC-2", "FC-1", "FC0", "FC1", "FC2", "FC3", "FC4" }, new string[] { "First", "Second", "Third", "Fourth", "Fifth" }, new TableComputeFunction(this.PaladinCasting)), new TableDescriptor2D(new string[] { "FCR0", "FCR1", "FCR2", "FCR3", "FCR4", "FCR5", "FCR6", "FCR7" }, new string[] { "Recovery" }, new TableComputeFunction(this.PaladinRecovery)) }), new Table2DInfoNode("Necromancy", new TableDescriptor2D[] { new TableDescriptor2D(new string[] { "Spell Names" }, new string[] { "First", "Second", "Third", "Fourth", "\"", "\"" }, new TableComputeFunction(this.NecromancerNames)), new TableDescriptor2D(new string[] { "FC-3", "FC-2", "FC-1", "FC0", "FC1", "FC2", "FC3", "FC4" }, new string[] { "First", "Second", "Third", "Fourth", "Fifth" }, new TableComputeFunction(this.NecromancerCasting)), new TableDescriptor2D(new string[] { "FCR0", "FCR1", "FCR2", "FCR3", "FCR4", "FCR5", "FCR6", "FCR7" }, new string[] { "Recovery" }, new TableComputeFunction(this.MageryRecovery)) }) });
            return new InfoInput[] { tree };
        }

        private object MageryCasting(int row, int column)
        {
            int num = column - 3;
            int num2 = row + 1;
            int num3 = (3 + num2) - num;
            if (num3 < 1)
            {
                num3 = 1;
            }
            return (((double) num3) / 4.0);
        }

        private object MageryRecovery(int row, int column)
        {
            int num = column;
            int num2 = 6 - num;
            if (num2 < 0)
            {
                num2 = 0;
            }
            return (((double) num2) / 4.0);
        }

        private object MagerySummoning(int row, int column)
        {
            int num = column - 3;
            int num2 = 0x24 - (num * 5);
            if (num2 < 1)
            {
                num2 = 1;
            }
            return (((double) num2) / 4.0);
        }

        private object NecromancerCasting(int row, int column)
        {
            int num2;
            int num = column - 3;
            switch (row)
            {
                case 1:
                    num2 = 2;
                    break;

                case 2:
                    num2 = 4;
                    break;

                case 3:
                case 4:
                case 5:
                    num2 = 6;
                    break;

                default:
                    num2 = 1;
                    break;
            }
            int num3 = 3 + num2;
            if (num3 < 1)
            {
                num3 = 1;
            }
            return (((double) num3) / 4.0);
        }

        private object NecromancerNames(int row, int column)
        {
            switch (row)
            {
                case 1:
                    return "Pain Spike";

                case 2:
                    return "Blood Oath, Mind Rot, Animate Dead, Corpse Skin";

                case 3:
                    return "Horrific Beast, Lich Form, Poison Strike, Strangle,";

                case 4:
                    return "Summon Familiar, Vampiric Embrace,";

                case 5:
                    return "Vengeful Spirit, Wraith Form";
            }
            return "Wither, Curse Weapon, Evil Omen";
        }

        private object PaladinCasting(int row, int column)
        {
            int num;
            switch (row)
            {
                case 1:
                    num = 3;
                    break;

                case 2:
                    num = 4;
                    break;

                case 3:
                    num = 5;
                    break;

                case 4:
                    num = 7;
                    break;

                default:
                    num = 2;
                    break;
            }
            int num2 = column - 3;
            int num3 = num - num2;
            if (num3 < 1)
            {
                num3 = 1;
            }
            return (((double) num3) / 4.0);
        }

        private object PaladinNames(int row, int column)
        {
            switch (row)
            {
                case 1:
                    return "Consecrate Weapon, Enemy of One";

                case 2:
                    return "Holy Light";

                case 3:
                    return "Divine Fury, Cleanse by Fire";

                case 4:
                    return "Close Wounds, Sacred Journey, Noble Sacrifice, Rmv Curse";
            }
            return "Dispel Evil";
        }

        private object PaladinRecovery(int row, int column)
        {
            int num = column;
            int num2 = 7 - num;
            if (num2 < 0)
            {
                num2 = 0;
            }
            return (((double) num2) / 4.0);
        }

        public override void UpdateGump(Gump g)
        {
            g.Children.Clear();
            Table2DInfoNode active = base.Inputs[0].Active as Table2DInfoNode;
            if (active == null)
            {
                GLabel toAdd = new GLabel("Select a spell type from the list above.", Engine.GetUniFont(1), GumpHues.WindowText, 0, 0);
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

