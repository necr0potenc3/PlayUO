namespace Client
{
    public class PVirtueItemTrigger : Packet
    {
        public PVirtueItemTrigger(GServerGump owner, int gumpID) : base(0xb1, "Virtue Item Trigger")
        {
            base.m_Stream.Write(owner.Serial);
            base.m_Stream.Write(0x1cd);
            base.m_Stream.Write(gumpID);
        }
    }
}