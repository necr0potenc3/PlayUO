namespace Client
{
    using System;

    public class Skill
    {
        public bool Action;
        public SkillGroup Group;
        public int ID;
        public SkillLock Lock;
        public string Name;
        public float Real;
        public float Value;

        public Skill(int id, bool action, string name)
        {
            this.ID = id;
            this.Action = action;
            this.Name = name;
            this.Value = 0f;
            this.Real = 0f;
            this.Group = null;
            this.Lock = SkillLock.Up;
        }

        public void Use()
        {
            Network.Send(new PUseSkill(this));
        }
    }
}

