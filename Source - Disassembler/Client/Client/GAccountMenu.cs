namespace Client
{
    using System;

    public class GAccountMenu : GMenuItem
    {
        private AccountProfile m_Account;

        public GAccountMenu(AccountProfile account) : base(account.Title)
        {
            this.m_Account = account;
        }

        public override void OnClick()
        {
            Gumps.Desktop.Children.Add(new GEditAccount(this.m_Account));
        }
    }
}

