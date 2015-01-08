namespace Client
{
    using System;

    public class PDesigner_Commit : Packet
    {
        public PDesigner_Commit(Item house) : base(0xd7, "Designer: Commit")
        {
            base.m_Stream.Write(house.Serial);
            base.m_Stream.Write((short) 4);
        }
    }
}

