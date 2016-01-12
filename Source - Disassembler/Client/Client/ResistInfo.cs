namespace Client
{
    public class ResistInfo
    {
        public static ResistInfo[] m_Armor = new ResistInfo[] { new ResistInfo(2, 4, 3, 3, 3, new string[] { "leather ", "female leather " }), new ResistInfo(2, 4, 3, 3, 4, new string[] { "studded ", "female studded ", " ranger armor" }), new ResistInfo(3, 3, 1, 5, 3, new string[] { "ringmail " }), new ResistInfo(5, 3, 2, 3, 2, new string[] { "platemail " }), new ResistInfo(3, 3, 3, 3, 3, new string[] { "dragon " }), new ResistInfo(6, 6, 7, 5, 7, new string[] { "daemon bone " }), new ResistInfo(4, 4, 4, 1, 2, new string[] { "chainmail " }), new ResistInfo(3, 3, 4, 2, 4, new string[] { "bone " }), new ResistInfo(7, 2, 2, 2, 2, new string[] { "bascinet" }), new ResistInfo(3, 3, 3, 3, 3, new string[] { "close helm" }), new ResistInfo(2, 4, 4, 3, 2, new string[] { "helmet" }), new ResistInfo(4, 1, 4, 4, 2, new string[] { "norse helm" }), new ResistInfo(3, 1, 3, 3, 4, new string[] { "orc helm" }) };
        public int m_Cold;
        public int m_Energy;
        public int m_Fire;

        public static ResistInfo[] m_Materials = new ResistInfo[] {
            new ResistInfo(6, 0, 0, 0, 0, new string[] { "dull copper " }), new ResistInfo(2, 1, 0, 0, 5, new string[] { "shadow iron " }), new ResistInfo(1, 1, 0, 5, 2, new string[] { "copper " }), new ResistInfo(3, 0, 5, 1, 1, new string[] { "bronze " }), new ResistInfo(1, 1, 2, 0, 2, new string[] { "golden " }), new ResistInfo(2, 3, 2, 2, 2, new string[] { "agapite " }), new ResistInfo(3, 3, 2, 3, 1, new string[] { "verite " }), new ResistInfo(4, 0, 3, 3, 3, new string[] { "valorite " }), new ResistInfo(5, 0, 0, 0, 0, new string[] { "spined " }), new ResistInfo(2, 3, 2, 2, 2, new string[] { "horned " }), new ResistInfo(2, 1, 2, 3, 4, new string[] { "barbed " }), new ResistInfo(0, 10, -3, 0, 0, new string[] { "red dragon " }), new ResistInfo(-3, 0, 0, 0, 0, new string[] { "yellow dragon " }), new ResistInfo(10, 0, 0, 0, -3, new string[] { "black dragon " }), new ResistInfo(0, -3, 0, 10, 0, new string[] { "green dragon " }), new ResistInfo(-3, 0, 10, 0, 0, new string[] { "white dragon " }),
            new ResistInfo(0, 0, 0, -3, 10, new string[] { "blue dragon " })
        };

        public string[] m_Names;
        public int m_Physical;
        public int m_Poison;

        public ResistInfo(int physical, int fire, int cold, int poison, int energy, params string[] names)
        {
            this.m_Physical = physical;
            this.m_Fire = fire;
            this.m_Cold = cold;
            this.m_Poison = poison;
            this.m_Energy = energy;
            this.m_Names = names;
        }

        public static ResistInfo Find(string text, ResistInfo[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                for (int j = 0; j < list[i].m_Names.Length; j++)
                {
                    if (text.IndexOf(list[i].m_Names[j]) >= 0)
                    {
                        return list[i];
                    }
                }
            }
            return null;
        }
    }
}