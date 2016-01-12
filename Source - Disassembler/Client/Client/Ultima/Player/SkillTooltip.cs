namespace Client
{
    public class SkillTooltip : Tooltip
    {
        private float m_LastReal;
        private float m_LastUsed;
        private Skill m_Skill;

        public SkillTooltip(Skill s) : base("Value: 0.0 (0.0)", true)
        {
            this.m_Skill = s;
            base.m_Delay = 0.5f;
        }

        public override Gump GetGump()
        {
            if ((this.m_LastReal != this.m_Skill.Real) || (this.m_LastUsed != this.m_Skill.Value))
            {
                this.m_LastReal = this.m_Skill.Real;
                this.m_LastUsed = this.m_Skill.Value;
                base.Text = string.Format("Value: {0:F1} ({1:F1})", this.m_LastUsed, this.m_LastReal);
            }
            return base.GetGump();
        }
    }
}