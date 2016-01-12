namespace Client
{
    using System;
    using System.Net;

    public class ShardProfile
    {
        private AccountProfile m_Account;
        private IPAddress m_Address;
        private int m_Auth;
        private CharacterProfile[] m_Characters;
        private int m_Index;
        private GMenuItem m_Menu;
        private string m_Name;
        private int m_PercentFull;
        private int m_Port;
        private int m_TimeZone;

        public ShardProfile(AccountProfile account, IPAddress address, int port, int index, int timeZone, int percentFull, int auth, string name)
        {
            this.m_Account = account;
            this.m_Address = address;
            this.m_Port = port;
            this.m_Index = index;
            this.m_TimeZone = timeZone;
            this.m_PercentFull = percentFull;
            this.m_Auth = auth;
            this.m_Name = name;
            this.m_Characters = new CharacterProfile[0];
        }

        public void AddCharacter(CharacterProfile character)
        {
            CharacterProfile[] characters = this.m_Characters;
            this.m_Characters = new CharacterProfile[characters.Length + 1];
            for (int i = 0; i < characters.Length; i++)
            {
                this.m_Characters[i] = characters[i];
            }
            this.m_Characters[characters.Length] = character;
        }

        public bool Contains(CharacterProfile character)
        {
            return (Array.IndexOf(this.m_Characters, character) >= 0);
        }

        public void RemoveCharacter(CharacterProfile character)
        {
            CharacterProfile[] characters = this.m_Characters;
            int index = Array.IndexOf(characters, character);
            if (index != -1)
            {
                this.m_Characters = new CharacterProfile[characters.Length - 1];
                for (int i = 0; i < index; i++)
                {
                    this.m_Characters[i] = characters[i];
                }
                for (int j = index; j < this.m_Characters.Length; j++)
                {
                    this.m_Characters[j] = characters[j + 1];
                }
            }
        }

        public AccountProfile Account
        {
            get
            {
                return this.m_Account;
            }
        }

        public IPAddress Address
        {
            get
            {
                return this.m_Address;
            }
            set
            {
                this.m_Address = value;
            }
        }

        public int Auth
        {
            get
            {
                return this.m_Auth;
            }
            set
            {
                this.m_Auth = value;
            }
        }

        public CharacterProfile[] Characters
        {
            get
            {
                return this.m_Characters;
            }
            set
            {
                this.m_Characters = value;
            }
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

        public int PercentFull
        {
            get
            {
                return this.m_PercentFull;
            }
            set
            {
                this.m_PercentFull = value;
            }
        }

        public int Port
        {
            get
            {
                return this.m_Port;
            }
            set
            {
                this.m_Port = value;
            }
        }

        public int TimeZone
        {
            get
            {
                return this.m_TimeZone;
            }
            set
            {
                this.m_TimeZone = value;
            }
        }
    }
}