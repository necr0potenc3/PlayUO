namespace Client
{
    public class PSetActiveAbility : Packet
    {
        public PSetActiveAbility(int index) : base(0xd7, "Set Active Ability")
        {
            base.m_Stream.Write(World.Serial);
            base.m_Stream.Write((short)0x19);
            base.m_Stream.Write(0);
            base.m_Stream.Write((byte)index);
            base.m_Stream.Write((byte)7);
        }
    }
}