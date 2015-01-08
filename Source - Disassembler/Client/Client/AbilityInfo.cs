namespace Client
{
    using System;
    using System.Collections;

    public class AbilityInfo
    {
        private static int[] AxeID = new int[] { 0xf49, 0xf4a };
        private static int[] BardicheID = new int[] { 0xf4d, 0xf4e };
        private static int[] BattleAxeID = new int[] { 0xf47, 0xf48 };
        private static int[] BlackStaffID = new int[] { 0xdf0, 0xdf1 };
        private static int[] BladedStaffID = new int[] { 0x26bd, 0x26c7 };
        private static int[] BoneHarvesterID = new int[] { 0x26bb, 0x26c5 };
        private static int[] BowID = new int[] { 0x13b1, 0x13b2 };
        private static int[] BroadswordID = new int[] { 0xf5e, 0xf5f };
        private static int[] ButcherKnifeID = new int[] { 0x13f6, 0x13f7 };
        private static int[] CleaverID = new int[] { 0xec2, 0xec3 };
        private static int[] ClubID = new int[] { 0x13b3, 0x13b4 };
        private static int[] CompositeBowID = new int[] { 0x26c2, 0x26cc };
        private static int[] CrescentBladeID = new int[] { 0x26c1, 0x26c2 };
        private static int[] CrossbowID = new int[] { 0xf4f, 0xf50 };
        private static int[] CutlassID = new int[] { 0x1440, 0x1441 };
        private static int[] DaggerID = new int[] { 0xf51, 0xf52 };
        private static int[] DoubleAxeID = new int[] { 0xf4b, 0xf4c };
        private static int[] DoubleBladedStaffID = new int[] { 0x26bf, 0x26c9 };
        private static int[] ExecAxeID = new int[] { 0xf45, 0xf46 };
        private static int[] GnarledStaffID = new int[] { 0x13f8, 0x13f9 };
        private static int[] HalberdID = new int[] { 0x143e, 0x143f };
        private static int[] HammerPickID = new int[] { 0x143c, 0x143d };
        private static int[] HatchetID = new int[] { 0xf43, 0xf44 };
        private static int[] HeavyCrossbowID = new int[] { 0x13fc, 0x13fd };
        private static int[] KatanaID = new int[] { 0x13fe, 0x13ff };
        private static int[] KryssID = new int[] { 0x1400, 0x1401 };
        private static int[] LanceID = new int[] { 0x26c0, 0x26ca };
        private static int[] LargeBattleAxeID = new int[] { 0x13fa, 0x13fb };
        private static int[] LongSwordID = new int[] { 0xf60, 0xf61 };
        private static AbilityInfo[] m_Abilities = new AbilityInfo[] { new AbilityInfo(0, new int[][] { HatchetID, LongSwordID, BroadswordID, KatanaID, BladedStaffID, HammerPickID, WarAxeID, KryssID, SpearID, CompositeBowID }), new AbilityInfo(1, new int[][] { CleaverID, LargeBattleAxeID, BattleAxeID, ExecAxeID, CutlassID, ScytheID, WarMaceID, WarAxeID, PitchforkID, WarForkID }), new AbilityInfo(2, new int[][] { LongSwordID, BattleAxeID, HalberdID, MaulID, MaceID, GnarledStaffID, QuarterStaffID, LanceID, CrossbowID }), new AbilityInfo(3, new int[][] { VikingSwordID, AxeID, BroadswordID, ShepherdsCrookID, SmithsHammerID, MaulID, WarMaceID, WarHammerID, ScepterID, SledgeHammerID }), new AbilityInfo(4, new int[][] { ButcherKnifeID, PickaxeID, SkinningKnifeID, HatchetID, WandID, ShepherdsCrookID, MaceID, WarForkID }), new AbilityInfo(5, new int[][] { BardicheID, AxeID, BladedStaffID, WandID, ClubID, PitchforkID, LanceID, HeavyCrossbowID }), new AbilityInfo(6, new int[][] { PickaxeID, TwoHandedAxeID, DoubleAxeID, ScimitarID, KatanaID, CrescentBladeID, QuarterStaffID, DoubleBladedStaffID, RepeatingCrossbowID }), new AbilityInfo(7, new int[][] { ButcherKnifeID, CleaverID, DaggerID, PikeID, KryssID, DoubleBladedStaffID }), new AbilityInfo(8, new int[][] { ExecAxeID, BoneHarvesterID, CrescentBladeID, HammerPickID, ScepterID, ShortSpearID, CrossbowID, BowID }), new AbilityInfo(9, new int[][] { HeavyCrossbowID, CompositeBowID, RepeatingCrossbowID }), new AbilityInfo(10, new int[][] { VikingSwordID, BardicheID, ScimitarID, ScytheID, BoneHarvesterID, GnarledStaffID, BlackStaffID, PikeID, SpearID, BowID }), new AbilityInfo(11, new int[][] { SkinningKnifeID, TwoHandedAxeID, CutlassID, SmithsHammerID, ClubID, DaggerID, ShortSpearID, SledgeHammerID }), new AbilityInfo(12, new int[][] { LargeBattleAxeID, HalberdID, DoubleAxeID, WarHammerID, BlackStaffID }) };
        private static AbilityInfo m_Active;
        private int m_Icon;
        private int m_Index;
        private int m_Name;
        private GTextButton m_NameLabel;
        private static Hashtable m_Table;
        private int m_Tooltip;
        private int[] m_Weapons;
        private static int[] MaceID = new int[] { 0xf5c, 0x45d };
        private static int[] MaulID = new int[] { 0x143a, 0x143b };
        private static int[] PickaxeID = new int[] { 0xe85, 0xe86 };
        private static int[] PikeID = new int[] { 0x26be, 0x26c8 };
        private static int[] PitchforkID = new int[] { 0xe87, 0xe88 };
        private static int[] QuarterStaffID = new int[] { 0xe89, 0xe8a };
        private static int[] RepeatingCrossbowID = new int[] { 0x26c3, 0x26cd };
        private static int[] ScepterID = new int[] { 0x26bc, 0x26c6 };
        private static int[] ScimitarID = new int[] { 0x13b5, 0x13b6 };
        private static int[] ScytheID = new int[] { 0x26ba, 0x26c4 };
        private static int[] ShepherdsCrookID = new int[] { 0xe81, 0xe82 };
        private static int[] ShortSpearID = new int[] { 0x1402, 0x1403 };
        private static int[] SkinningKnifeID = new int[] { 0xec4, 0xec5 };
        private static int[] SledgeHammerID = new int[] { 0xfb4, 0xfb5 };
        private static int[] SmithsHammerID = new int[] { 0x13ec, 0x13e4 };
        private static int[] SpearID = new int[] { 0xf62, 0xf63 };
        private static int[] TwoHandedAxeID = new int[] { 0x1442, 0x1443 };
        private static int[] VikingSwordID = new int[] { 0x13b9, 0x13ba };
        private static int[] WandID = new int[] { 0xdf2, 0xdf3, 0xdf4, 0xdf5 };
        private static int[] WarAxeID = new int[] { 0x13af, 0x13b0 };
        private static int[] WarForkID = new int[] { 0x1404, 0x1405 };
        private static int[] WarHammerID = new int[] { 0x1438, 0x1439 };
        private static int[] WarMaceID = new int[] { 0x1406, 0x1407 };

        public AbilityInfo(int index, params int[][] weapons)
        {
            if (m_Table == null)
            {
                m_Table = new Hashtable();
            }
            this.m_Index = index;
            this.m_Name = 0xfb2e6 + index;
            this.m_Tooltip = 0x10333d + index;
            this.m_Icon = 0x5200 + index;
            int num = 0;
            for (int i = 0; i < weapons.Length; i++)
            {
                num += weapons[i].Length;
            }
            this.m_Weapons = new int[num];
            int num3 = 0;
            int num4 = 0;
            while (num3 < weapons.Length)
            {
                int num5 = 0;
                while (num5 < weapons[num3].Length)
                {
                    this.m_Weapons[num4] = weapons[num3][num5];
                    if (this.m_Weapons[num4] == 0xf51)
                    {
                        int num6 = 0;
                        num6++;
                    }
                    ArrayList list = (ArrayList) m_Table[this.m_Weapons[num4]];
                    if (list == null)
                    {
                        m_Table[this.m_Weapons[num4]] = list = new ArrayList();
                    }
                    list.Add(this);
                    num5++;
                    num4++;
                }
                num3++;
            }
        }

        public static void ClearActive()
        {
            m_Active = null;
            GCombatGump.Update();
        }

        public static AbilityInfo GetAbilityFor(Mobile m, bool primary)
        {
            if (m != null)
            {
                ArrayList list;
                Item item = m.FindEquip(Layer.TwoHanded);
                if (item != null)
                {
                    int num = item.ID & 0x3fff;
                    list = (ArrayList) m_Table[num];
                    if ((list != null) && (list.Count > 0))
                    {
                        return (AbilityInfo) list[primary ? 0 : (list.Count - 1)];
                    }
                }
                item = m.FindEquip(Layer.OneHanded);
                if (item != null)
                {
                    int num2 = item.ID & 0x3fff;
                    list = (ArrayList) m_Table[num2];
                    if ((list != null) && (list.Count > 0))
                    {
                        return (AbilityInfo) list[primary ? 0 : (list.Count - 1)];
                    }
                }
            }
            return m_Abilities[primary ? 4 : 10];
        }

        public static AbilityInfo[] Abilities
        {
            get
            {
                return m_Abilities;
            }
        }

        public static AbilityInfo Active
        {
            get
            {
                return m_Active;
            }
            set
            {
                AbilityInfo info = value;
                AbilityInfo active = m_Active;
                if (info != active)
                {
                    m_Active = info;
                    if (info == null)
                    {
                        Network.Send(new PSetActiveAbility(0));
                    }
                    else
                    {
                        Network.Send(new PSetActiveAbility(info.Index + 1));
                    }
                    GCombatGump.Update();
                }
            }
        }

        public int Icon
        {
            get
            {
                return this.m_Icon;
            }
        }

        public int Index
        {
            get
            {
                return this.m_Index;
            }
        }

        public int Name
        {
            get
            {
                return this.m_Name;
            }
        }

        public GTextButton NameLabel
        {
            get
            {
                return this.m_NameLabel;
            }
            set
            {
                this.m_NameLabel = value;
            }
        }

        public static AbilityInfo Primary
        {
            get
            {
                return GetAbilityFor(World.Player, true);
            }
        }

        public static AbilityInfo Secondary
        {
            get
            {
                return GetAbilityFor(World.Player, false);
            }
        }

        public int Tooltip
        {
            get
            {
                return this.m_Tooltip;
            }
        }
    }
}

