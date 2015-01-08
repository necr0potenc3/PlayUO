namespace Client
{
    using System;

    public class ServerProfile
    {
        private AccountProfile[] m_Accounts;
        private string m_Address;
        private GMenuItem m_Menu;
        private int m_Port;
        private string m_Title;

        public ServerProfile(string title, string address, int port)
        {
            this.m_Title = title;
            this.m_Address = address;
            this.m_Port = port;
            this.m_Accounts = new AccountProfile[0];
        }

        public void AddAccount(AccountProfile account)
        {
            AccountProfile[] accounts = this.m_Accounts;
            this.m_Accounts = new AccountProfile[accounts.Length + 1];
            for (int i = 0; i < accounts.Length; i++)
            {
                this.m_Accounts[i] = accounts[i];
            }
            this.m_Accounts[accounts.Length] = account;
        }

        public void RemoveAccount(AccountProfile account)
        {
            AccountProfile[] accounts = this.m_Accounts;
            int index = Array.IndexOf(accounts, account);
            if (index != -1)
            {
                this.m_Accounts = new AccountProfile[accounts.Length - 1];
                for (int i = 0; i < index; i++)
                {
                    this.m_Accounts[i] = accounts[i];
                }
                for (int j = index; j < this.m_Accounts.Length; j++)
                {
                    this.m_Accounts[j] = accounts[j + 1];
                }
            }
        }

        public AccountProfile[] Accounts
        {
            get
            {
                return this.m_Accounts;
            }
            set
            {
                this.m_Accounts = value;
            }
        }

        public string Address
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
    }
}

