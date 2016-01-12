namespace Client
{
    public class PParty_AddMember : Packet
    {
        public PParty_AddMember() : base(0xbf, "Add Party Member")
        {
            base.m_Stream.Write((short)6);
            base.m_Stream.Write((byte)1);
            base.m_Stream.Write(0);
        }
    }
}