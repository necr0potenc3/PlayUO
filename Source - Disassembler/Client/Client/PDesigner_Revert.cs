namespace Client
{
    using System;

    public class PDesigner_Revert : Packet
    {
        public PDesigner_Revert(Item house) : base(0xd7, "Designer: Revert")
        {
            base.m_Stream.Write(house.Serial);
            base.m_Stream.Write((short) 0x1a);
        }
    }
}

