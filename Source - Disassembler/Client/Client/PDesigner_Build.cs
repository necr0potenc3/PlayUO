namespace Client
{
    using System;

    public class PDesigner_Build : Packet
    {
        public PDesigner_Build(Item house, int x, int y, int itemID) : base(0xd7, "Designer: Build")
        {
            base.m_Stream.Write(house.Serial);
            base.m_Stream.Write((short) 6);
            base.m_Stream.WriteEncoded(itemID);
            base.m_Stream.WriteEncoded(x);
            base.m_Stream.WriteEncoded(y);
        }
    }
}

