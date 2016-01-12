namespace Client
{
    public class GEditAccount : GWindowsForm
    {
        private AccountProfile m_Account;
        private GTextBox m_Password;
        private GTextBox m_Title;
        private GTextBox m_Username;

        public GEditAccount(AccountProfile account) : base(0, 0, 320, 110)
        {
            this.m_Account = account;
            Gumps.Focus = this;
            base.Text = "Edit Account";
            this.m_Title = this.AddTextBox("Title:", 0, account.Title, '\0');
            this.m_Username = this.AddTextBox("Username:", 1, account.Username, '\0');
            this.m_Password = this.AddTextBox("Password:", 2, account.Password, '*');
            this.AddButton("Remove", 0, new OnClick(this.Remove_OnClick));
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

        private GTextBox AddTextBox(string name, int index, string initialText, char pc)
        {
            int y = (30 + (index * 0x19)) - 0x16;
            GLabel toAdd = new GLabel(name, Engine.GetUniFont(2), GumpHues.ControlText, 0, 0);
            toAdd.X = (10 - toAdd.Image.xMin) - 4;
            toAdd.Y = (y + ((0x16 - ((toAdd.Image.yMax - toAdd.Image.yMin) + 1)) / 2)) - toAdd.Image.yMin;
            base.Client.Children.Add(toAdd);
            IHue windowText = GumpHues.WindowText;
            GWindowsTextBox box = new GWindowsTextBox(0x38, y, 200, 0x16, initialText, Engine.GetUniFont(2), windowText, windowText, windowText, pc);
            base.Client.Children.Add(box);
            return box.TextBox;
        }

        private void Cancel_OnClick(Gump g)
        {
            this.Close();
        }

        private void Okay_OnClick(Gump g)
        {
            this.m_Account.Title = this.m_Title.String;
            this.m_Account.Username = this.m_Username.String;
            this.m_Account.Password = this.m_Password.String;
            if ((this.m_Account.Menu != null) && (this.m_Account.Menu != this.m_Account.Server.Menu))
            {
                this.m_Account.Menu.Text = this.m_Account.Title;
            }
            new ServerSync(this.m_Account.Server, this.m_Account, null);
            Profiles.Save();
            this.Close();
        }

        private void Remove_OnClick(Gump g)
        {
            this.m_Account.Server.RemoveAccount(this.m_Account);
            Profiles.Save();
            Engine.UpdateSmartLoginMenu();
            this.Close();
        }
    }
}