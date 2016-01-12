namespace Client
{
    public class ServerFeatures
    {
        private bool m_AOS = false;
        private bool m_ContextMenus = false;
        private bool m_SingleChar = false;

        public bool AOS
        {
            get
            {
                return this.m_AOS;
            }
            set
            {
                this.m_AOS = value;
            }
        }

        public bool ContextMenus
        {
            get
            {
                return this.m_ContextMenus;
            }
            set
            {
                this.m_ContextMenus = value;
            }
        }

        public bool SingleChar
        {
            get
            {
                return this.m_SingleChar;
            }
            set
            {
                this.m_SingleChar = value;
            }
        }
    }
}