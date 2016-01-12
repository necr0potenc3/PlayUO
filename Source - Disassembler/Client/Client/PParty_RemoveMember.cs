namespace Client
{
    public class PParty_RemoveMember : Packet
    {
        public PParty_RemoveMember() : base(0xbf, "Remove Party Member")
        {
            base.m_Stream.Write((short)6);
            base.m_Stream.Write((byte)2);
            base.m_Stream.Write(0);
        }
    }
}