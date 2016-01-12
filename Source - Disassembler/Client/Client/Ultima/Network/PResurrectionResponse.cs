namespace Client
{
    public class PResurrectionResponse : Packet
    {
        public PResurrectionResponse(bool ShouldResurrect) : base(0x2c, "Resurrection Response", 2)
        {
            base.m_Stream.Write(ShouldResurrect ? ((byte)1) : ((byte)2));
        }
    }
}