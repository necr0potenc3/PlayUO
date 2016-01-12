namespace Client
{
    public class PDesigner_Clear : Packet
    {
        public PDesigner_Clear(Item house) : base(0xd7, "Designer: Clear")
        {
            base.m_Stream.Write(house.Serial);
            base.m_Stream.Write((short)0x10);
        }
    }
}