namespace Client
{
    public class PDesigner_Sync : Packet
    {
        public PDesigner_Sync(Item house) : base(0xd7, "Designer: Sync")
        {
            base.m_Stream.Write(house.Serial);
            base.m_Stream.Write((short)14);
        }
    }
}