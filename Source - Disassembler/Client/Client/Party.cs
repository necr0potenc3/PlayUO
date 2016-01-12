namespace Client
{
    using System;

    public class Party
    {
        private static Mobile m_Leader;
        private static Mobile[] m_Members = new Mobile[0];
        private static PartyState m_State;

        public static int Index
        {
            get
            {
                return Array.IndexOf(m_Members, World.Player);
            }
        }

        public static bool IsLeader
        {
            get
            {
                return ((m_Leader != null) && m_Leader.Player);
            }
        }

        public static Mobile Leader
        {
            get
            {
                return m_Leader;
            }
            set
            {
                m_Leader = value;
                if (m_Members.Length > 0)
                {
                    m_Members[0] = m_Leader;
                }
            }
        }

        public static Mobile[] Members
        {
            get
            {
                return m_Members;
            }
            set
            {
                m_Members = value;
                m_Leader = (m_Members.Length > 0) ? m_Members[0] : null;
                m_State = ((m_Members.Length > 0) && (Index >= 0)) ? PartyState.Joined : PartyState.Alone;
            }
        }

        public static PartyState State
        {
            get
            {
                return m_State;
            }
            set
            {
                m_State = value;
                if (m_State != PartyState.Joined)
                {
                    m_Members = new Mobile[0];
                    m_Leader = null;
                }
            }
        }
    }
}