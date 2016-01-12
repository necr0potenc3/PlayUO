namespace Client
{
    public class POpenSpellbook : Packet
    {
        public POpenSpellbook(int num) : base(0x12, "Open Spellbook")
        {
            base.m_Stream.Write((byte)0x43);
            base.m_Stream.Write(num.ToString());
            base.m_Stream.Write((byte)0);
        }
    }
}