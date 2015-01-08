namespace Client
{
    using System;

    public class MobileTooltip : ITooltip
    {
        private Client.Gump m_Gump;
        private Mobile m_Mobile;

        public MobileTooltip(Mobile mob)
        {
            this.m_Mobile = mob;
        }

        public Client.Gump GetGump()
        {
            if (this.m_Gump != null)
            {
                return this.m_Gump;
            }
            if (this.m_Mobile.PropertyList == null)
            {
                this.m_Mobile.QueryProperties();
                return null;
            }
            return (this.m_Gump = new GObjectProperties(-1, this.m_Mobile, this.m_Mobile.PropertyList));
        }

        public float Delay
        {
            get
            {
                return 0.5f;
            }
            set
            {
            }
        }

        public Client.Gump Gump
        {
            get
            {
                return this.m_Gump;
            }
            set
            {
                this.m_Gump = value;
            }
        }
    }
}

