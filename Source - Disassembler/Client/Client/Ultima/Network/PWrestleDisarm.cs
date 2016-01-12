namespace Client
{
    public class PWrestleDisarm : Packet
    {
        public PWrestleDisarm() : base(0xbf, "Wrestle Disarm")
        {
            base.m_Stream.Write((short)9);
        }
    }
}