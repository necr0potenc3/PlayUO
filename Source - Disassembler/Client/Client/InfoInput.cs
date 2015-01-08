namespace Client
{
    using System;

    public abstract class InfoInput
    {
        private object m_Active;
        private Client.Gump m_Gump;
        private string m_Name;
        private InfoProvider m_Provider;

        public InfoInput(string name)
        {
            this.m_Name = name;
        }

        public abstract Client.Gump CreateGump();
        public abstract void UpdateGump(Client.Gump g);

        public object Active
        {
            get
            {
                return this.m_Active;
            }
            set
            {
                if (this.m_Active != value)
                {
                    this.m_Active = value;
                    if (this.m_Gump != null)
                    {
                        this.UpdateGump(this.m_Gump);
                    }
                    if (this.m_Provider != null)
                    {
                        this.m_Provider.Update();
                    }
                }
            }
        }

        public Client.Gump Gump
        {
            get
            {
                if ((this.m_Gump == null) || this.m_Gump.Disposed)
                {
                    this.m_Gump = this.CreateGump();
                }
                this.UpdateGump(this.m_Gump);
                return this.m_Gump;
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }

        public InfoProvider Provider
        {
            get
            {
                return this.m_Provider;
            }
            set
            {
                this.m_Provider = value;
            }
        }
    }
}

