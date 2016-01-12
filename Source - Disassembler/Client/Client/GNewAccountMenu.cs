namespace Client
{
    public class GNewAccountMenu : GMenuItem
    {
        private ServerProfile m_Server;

        public GNewAccountMenu(ServerProfile server, string text) : base(text)
        {
            this.m_Server = server;
        }

        public override void OnClick()
        {
            Gumps.Desktop.Children.Add(new GNewAccount(this.m_Server));
        }
    }
}