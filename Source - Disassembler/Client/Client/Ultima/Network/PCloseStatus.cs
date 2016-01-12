namespace Client
{
    public class PCloseStatus : Packet
    {
        public PCloseStatus(Mobile Target) : this(Target.Serial)
        {
        }

        public PCloseStatus(int Serial) : base(0xbf, "Close Status")
        {
            base.m_Stream.Write((short)12);
            base.m_Stream.Write(Serial);
        }
    }
}