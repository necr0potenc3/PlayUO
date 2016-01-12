namespace Client
{
    public class Features
    {
        private bool m_AOS;
        private bool m_Chat;
        private bool m_LBR;

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

        public bool Chat
        {
            get
            {
                return this.m_Chat;
            }
            set
            {
                this.m_Chat = value;
            }
        }

        public bool LBR
        {
            get
            {
                return this.m_LBR;
            }
            set
            {
                this.m_LBR = value;
            }
        }
    }
}