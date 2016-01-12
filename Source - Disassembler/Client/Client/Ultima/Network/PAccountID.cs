namespace Client
{
    public class PAccountID : Packet
    {
        public PAccountID() : base(0xbb, "Account ID", 7)
        {
            base.m_Stream.Write(World.Serial);
            base.m_Stream.Write(World.Serial);
        }
    }
}