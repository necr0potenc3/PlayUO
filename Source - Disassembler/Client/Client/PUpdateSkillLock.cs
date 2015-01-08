namespace Client
{
    using System;

    public class PUpdateSkillLock : Packet
    {
        public PUpdateSkillLock(Skill skill) : base(0x3a, "Update Skill Lock")
        {
            base.m_Stream.Write((short) skill.ID);
            base.m_Stream.Write((byte) skill.Lock);
        }
    }
}

