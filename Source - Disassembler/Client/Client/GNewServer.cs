namespace Client
{
    using System;

    public class GNewServer : GWindowsForm
    {
        private GTextBox m_Address;
        private GTextBox m_Port;
        private GTextBox m_Title;

        public GNewServer() : base(0, 0, 320, 110)
        {
            Gumps.Focus = this;
            base.Text = "New Server";
            this.m_Title = this.AddTextBox("Title:", 0);
            this.m_Address = this.AddTextBox("Address:", 1);
            this.m_Port = this.AddTextBox("Port:", 2);
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

        private GTextBox AddTextBox(string name, int index)
        {
            int y = (30 + (index * 0x19)) - 0x16;
            GLabel toAdd = new GLabel(name, Engine.GetUniFont(2), GumpHues.ControlText, 0, 0);
            toAdd.X = (10 - toAdd.Image.xMin) - 4;
            toAdd.Y = (y + ((0x16 - ((toAdd.Image.yMax - toAdd.Image.yMin) + 1)) / 2)) - toAdd.Image.yMin;
            base.Client.Children.Add(toAdd);
            IHue windowText = GumpHues.WindowText;
            GWindowsTextBox box = new GWindowsTextBox(0x38, y, 200, 0x16, "", Engine.GetUniFont(2), windowText, windowText, windowText, '\0');
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
            string address = this.m_Address.String;
            string str3 = this.m_Port.String;
            Profiles.AddServer(new ServerProfile(title, address, Convert.ToInt32(str3)));
            Profiles.Save();
            Engine.UpdateSmartLoginMenu();
            this.Close();
        }
    }
}