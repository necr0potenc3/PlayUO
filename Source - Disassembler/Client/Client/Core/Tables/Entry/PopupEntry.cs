namespace Client
{
    public class PopupEntry
    {
        private int m_EntryID;
        private int m_Flags;
        private string m_Text;

        public PopupEntry(int EntryID, int StringID, int Flags)
        {
            this.m_EntryID = EntryID;
            this.m_Flags = Flags;
            this.m_Text = Localization.GetString(0x2dc6c0 + StringID);
            if ((Flags & -34) != 0)
            {
                this.m_Text = string.Format("0x{0:X4} {1}", Flags, this.m_Text);
            }
        }

        public int EntryID
        {
            get
            {
                return this.m_EntryID;
            }
        }

        public int Flags
        {
            get
            {
                return this.m_Flags;
            }
        }

        public string Text
        {
            get
            {
                return this.m_Text;
            }
        }
    }
}