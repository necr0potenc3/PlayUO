namespace Client
{
    public class PServerSelection : Packet
    {
        public PServerSelection(Server server) : base(160, string.Format("Server Selection ({0})", server.Name), 3)
        {
            base.m_Stream.Write((short)server.ServerID);
        }

        public PServerSelection(int serverID) : base(160, "Automated Server Selection", 3)
        {
            base.m_Stream.Write((short)serverID);
        }
    }
}