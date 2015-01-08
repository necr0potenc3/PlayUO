namespace Client
{
    using System;

    public class PParty_SetCanLoot : Packet
    {
        public PParty_SetCanLoot(bool val) : base(0xbf, "Party Set Can Loot")
        {
            base.m_Stream.Write((short) 6);
            base.m_Stream.Write((byte) 6);
            base.m_Stream.Write(val);
        }
    }
}

