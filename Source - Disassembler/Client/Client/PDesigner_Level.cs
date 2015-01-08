namespace Client
{
    using System;

    public class PDesigner_Level : Packet
    {
        public PDesigner_Level(Item house, int level) : base(0xd7, "Designer: Level")
        {
            base.m_Stream.Write(house.Serial);
            base.m_Stream.Write((short) 0x12);
            base.m_Stream.WriteEncoded(level);
        }
    }
}

