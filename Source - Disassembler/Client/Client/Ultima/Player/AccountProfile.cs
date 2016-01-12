namespace Client
{
    using System;

    public class AccountProfile
    {
        private GMenuItem m_Menu;
        private string m_Password;
        private ServerProfile m_Server;
        private ShardProfile[] m_Shards;
        private string m_Title;
        private string m_Username;

        public AccountProfile(ServerProfile server, string title, string username, string password)
        {
            this.m_Server = server;
            this.m_Title = title;
            this.m_Username = username;
            this.m_Password = password;
            this.m_Shards = new ShardProfile[0];
        }

        public void AddShard(ShardProfile shard)
        {
            ShardProfile[] shards = this.m_Shards;
            this.m_Shards = new ShardProfile[shards.Length + 1];
            for (int i = 0; i < shards.Length; i++)
            {
                this.m_Shards[i] = shards[i];
            }
            this.m_Shards[shards.Length] = shard;
        }

        public bool Contains(ShardProfile shard)
        {
            return (Array.IndexOf(this.m_Shards, shard) >= 0);
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

        public string Password
        {
            get
            {
                return this.m_Password;
            }
            set
            {
                this.m_Password = value;
            }
        }

        public ServerProfile Server
        {
            get
            {
                return this.m_Server;
            }
        }

        public ShardProfile[] Shards
        {
            get
            {
                return this.m_Shards;
            }
            set
            {
                this.m_Shards = value;
            }
        }

        public string Title
        {
            get
            {
                return this.m_Title;
            }
            set
            {
                this.m_Title = value;
            }
        }

        public string Username
        {
            get
            {
                return this.m_Username;
            }
            set
            {
                this.m_Username = value;
            }
        }
    }
}