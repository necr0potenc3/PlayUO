namespace Client
{
    public abstract class InfoProvider
    {
        private Client.Gump m_Gump;
        private InfoInput[] m_Inputs;
        private string m_Name;

        public InfoProvider(string name)
        {
            this.m_Name = name;
            this.m_Inputs = this.CreateInputs();
            for (int i = 0; i < this.m_Inputs.Length; i++)
            {
                this.m_Inputs[i].Provider = this;
            }
        }

        public abstract Client.Gump CreateGump();

        public abstract InfoInput[] CreateInputs();

        public void Update()
        {
            if (this.m_Gump != null)
            {
                this.UpdateGump(this.m_Gump);
            }
        }

        public abstract void UpdateGump(Client.Gump g);

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

        public InfoInput[] Inputs
        {
            get
            {
                return this.m_Inputs;
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }
    }
}