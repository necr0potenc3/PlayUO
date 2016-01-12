namespace Client
{
    using System.Collections;

    public class SkillGroup
    {
        public int GroupID;
        public string Name;
        public ArrayList Skills;

        public SkillGroup(string name, int groupID)
        {
            this.Name = name;
            this.GroupID = groupID;
            this.Skills = new ArrayList();
        }
    }
}