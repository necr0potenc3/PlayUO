namespace Client
{
    public class CharacterProfile
    {
        private int m_Index;
        private GMenuItem m_Menu;
        private string m_Name;
        private ShardProfile m_Shard;

        public CharacterProfile(ShardProfile shard, string name, int index)
        {
            this.m_Shard = shard;
            this.m_Name = name;
            this.m_Index = index;
        }

        public int Index
        {
            get
            {
                return this.m_Index;
            }
            set
            {
                this.m_Index = value;
            }
        }

        public GMenuItem Menu
        {
            get
            {
                return this.m_Menu;
            }
            set
            {
                this.m_Menu = value;
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
            set
            {
                this.m_Name = value;
            }
        }

        public ShardProfile Shard
        {
            get
            {
                return this.m_Shard;
            }
        }
    }
}