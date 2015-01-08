namespace Client
{
    using System;

    public class ChangeProviderMenu : GMenuItem
    {
        private GInfoForm m_Form;
        private InfoProvider m_Provider;

        public ChangeProviderMenu(GInfoForm form, InfoProvider provider) : base(provider.Name)
        {
            this.m_Form = form;
            this.m_Provider = provider;
        }

        public override void OnClick()
        {
            this.m_Form.Provider = this.m_Provider;
        }
    }
}

