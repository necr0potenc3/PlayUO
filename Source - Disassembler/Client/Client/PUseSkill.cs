namespace Client
{
    using System;

    public class PUseSkill : Packet
    {
        private static Skill m_LastSkill;

        public PUseSkill(Skill skill) : base(0x12, string.Format("Use Skill '{0}'", skill.Name))
        {
            m_LastSkill = skill;
            base.m_Stream.Write((byte) 0x24);
            base.m_Stream.Write(string.Format("{0} 0", skill.ID));
            base.m_Stream.Write((byte) 0);
        }

        public static void SendLast()
        {
            if (m_LastSkill != null)
            {
                Network.Send(new PUseSkill(m_LastSkill));
            }
        }
    }
}

