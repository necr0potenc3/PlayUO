namespace Client
{
    public class PDesigner_Restore : Packet
    {
        public PDesigner_Restore(Item house) : base(0xd7, "Designer: Restore")
        {
            base.m_Stream.Write(house.Serial);
            base.m_Stream.Write((short)3);
        }
    }
}