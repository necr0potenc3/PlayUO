namespace Client
{
    public class POpenDoor : Packet
    {
        public POpenDoor() : base(0x12, "Open Door")
        {
            base.m_Stream.Write((byte)0x58);
            base.m_Stream.Write((byte)0);
        }
    }
}