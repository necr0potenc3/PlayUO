namespace Client
{
    public class PDesigner_Remove : Packet
    {
        public PDesigner_Remove(Item house, int x, int y, int z, int itemID) : base(0xd7, "Designer: Remove")
        {
            base.m_Stream.Write(house.Serial);
            base.m_Stream.Write((short)5);
            base.m_Stream.WriteEncoded(itemID);
            base.m_Stream.WriteEncoded(x);
            base.m_Stream.WriteEncoded(y);
            base.m_Stream.WriteEncoded(z);
        }
    }
}