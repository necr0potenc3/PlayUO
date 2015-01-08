namespace Client
{
    using System;

    public class GPlayCharacterMenu : GMenuItem
    {
        private CharacterProfile m_Character;

        public GPlayCharacterMenu(CharacterProfile character) : base(character.Name)
        {
            this.m_Character = character;
        }

        public override void OnClick()
        {
            Entry entry = new Entry {
                AccountName = this.m_Character.Shard.Account.Username,
                Password = this.m_Character.Shard.Account.Password,
                CharID = this.m_Character.Index,
                CharName = this.m_Character.Name,
                ServerID = this.m_Character.Shard.Index,
                ServerName = this.m_Character.Shard.Name,
                CharProfile = this.m_Character
            };
            Engine.m_QuickLogin = true;
            Engine.m_QuickEntry = entry;
            Cursor.Hourglass = true;
            Gumps.Desktop.Children.Clear();
            xGumps.Display("Connecting");
            Engine.DrawNow();
            string serverHost = NewConfig.ServerHost;
            int serverPort = NewConfig.ServerPort;
            NewConfig.ServerHost = this.m_Character.Shard.Account.Server.Address;
            NewConfig.ServerPort = this.m_Character.Shard.Account.Server.Port;
            if (Network.Connect())
            {
                NewConfig.ServerHost = serverHost;
                NewConfig.ServerPort = serverPort;
                Gumps.Desktop.Children.Clear();
                xGumps.Display("AccountVerify");
            }
            else
            {
                NewConfig.ServerHost = serverHost;
                NewConfig.ServerPort = serverPort;
                Gumps.Desktop.Children.Clear();
                xGumps.SetVariable("FailMessage", "Couldn't connect to the login server.  Either the server is down, or you've entered an invalid host / port.  Check Client.cfg.");
                xGumps.Display("ConnectionFailed");
                Cursor.Hourglass = false;
                Engine.m_QuickLogin = false;
                return;
            }
            Network.Send(new PLoginSeed());
            Network.Send(new PAccount(entry.AccountName, entry.Password));
        }

        public CharacterProfile Character
        {
            get
            {
                return this.m_Character;
            }
        }
    }
}

