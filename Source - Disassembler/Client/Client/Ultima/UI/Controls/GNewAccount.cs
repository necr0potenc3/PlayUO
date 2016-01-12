namespace Client
{
    public class GNewAccount : GWindowsForm
    {
        private GTextBox m_Password;
        private ServerProfile m_Server;
        private GTextBox m_Title;
        private GTextBox m_Username;

        public GNewAccount(ServerProfile server) : base(0, 0, 320, 110)
        {
            this.m_Server = server;
            Gumps.Focus = this;
            base.Text = "New Account";
            this.m_Title = this.AddTextBox("Title:", 0, '\0');
            this.m_Username = this.AddTextBox("Username:", 1, '\0');
            this.m_Password = this.AddTextBox("Password:", 2, '*');
            this.AddButton("Okay", 1, new OnClick(this.Okay_OnClick));
            this.AddButton("Cancel", 2, new OnClick(this.Cancel_OnClick));
            this.Center();
        }

        private GWindowsButton AddButton(string name, int index, OnClick onClick)
        {
            GWindowsButton toAdd = new GWindowsButton(name, 0x103, (30 + (index * 0x19)) - 0x16, 0x2f, 0x16);
            toAdd.OnClick = onClick;
            base.Client.Children.Add(toAdd);
            return toAdd;
        }

        private GTextBox AddTextBox(string name, int index, char pc)
        {
            int y = (30 + (index * 0x19)) - 0x16;
            GLabel toAdd = new GLabel(name, Engine.GetUniFont(2), GumpHues.ControlText, 0, 0);
            toAdd.X = (10 - toAdd.Image.xMin) - 4;
            toAdd.Y = (y + ((0x16 - ((toAdd.Image.yMax - toAdd.Image.yMin) + 1)) / 2)) - toAdd.Image.yMin;
            base.Client.Children.Add(toAdd);
            IHue windowText = GumpHues.WindowText;
            GWindowsTextBox box = new GWindowsTextBox(0x38, y, 200, 0x16, "", Engine.GetUniFont(2), windowText, windowText, windowText, pc);
            base.Client.Children.Add(box);
            return box.TextBox;
        }

        private void Cancel_OnClick(Gump g)
        {
            this.Close();
        }

        private void Okay_OnClick(Gump g)
        {
            string title = this.m_Title.String;
            string username = this.m_Username.String;
            string password = this.m_Password.String;
            AccountProfile account = new AccountProfile(this.m_Server, title, username, password);
            this.m_Server.AddAccount(account);
            new ServerSync(this.m_Server, account, null);
            Profiles.Save();
            Engine.UpdateSmartLoginMenu();
            this.Close();
        }
    }
}