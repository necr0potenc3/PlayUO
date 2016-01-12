namespace Client
{
    public class PProfileRequest : Packet
    {
        public PProfileRequest(Mobile owner) : base(0xb8, "Profile Request")
        {
            base.m_Stream.Write((byte)0);
            base.m_Stream.Write(owner.Serial);
        }
    }
}