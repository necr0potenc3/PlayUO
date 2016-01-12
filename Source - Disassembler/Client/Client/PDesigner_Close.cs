namespace Client
{
    public class PDesigner_Close : Packet
    {
        public PDesigner_Close(Item house) : base(0xd7, "Designer: Close")
        {
            base.m_Stream.Write(house.Serial);
            base.m_Stream.Write((short)12);
        }
    }
}