namespace Client
{
    public class GLogOutQuery : GMessageBoxYesNo
    {
        private static GLogOutQuery m_Instance;

        private GLogOutQuery() : base("Quit\nUltima Online?", false, null)
        {
        }

        public static void Display()
        {
            if (m_Instance == null)
            {
                m_Instance = new GLogOutQuery();
                Gumps.Desktop.Children.Add(m_Instance);
            }
        }

        protected internal override void OnDispose()
        {
            m_Instance = null;
        }

        protected override void OnSignal(bool response)
        {
            if (response)
            {
                Engine.m_Ingame = false;
                Network.Send(new PDisconnect());
                Network.Disconnect();
                Engine.ShowAcctLogin();
            }
        }
    }
}