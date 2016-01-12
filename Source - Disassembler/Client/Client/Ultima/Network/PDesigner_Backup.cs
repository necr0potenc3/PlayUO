namespace Client
{
    public class PDesigner_Backup : Packet
    {
        public PDesigner_Backup(Item house) : base(0xd7, "Designer: Backup")
        {
            base.m_Stream.Write(house.Serial);
            base.m_Stream.Write((short)2);
        }
    }
}