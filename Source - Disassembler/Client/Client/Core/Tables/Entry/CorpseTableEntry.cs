namespace Client
{
    public class CorpseTableEntry
    {
        public int m_NewHue;
        public int m_NewID;
        public int m_OldID;

        public CorpseTableEntry(int oldID, int newID, int newHue)
        {
            this.m_OldID = oldID;
            this.m_NewID = newID;
            this.m_NewHue = newHue;
        }
    }
}