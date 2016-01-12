namespace Client
{
    public class PDropItem : Packet
    {
        public PDropItem(int Serial, short X, short Y, sbyte Z, int DestSerial) : base(8, "Drop Item", 14)
        {
            base.m_Stream.Write(Serial);
            base.m_Stream.Write(X);
            base.m_Stream.Write(Y);
            base.m_Stream.Write(Z);
            base.m_Stream.Write(DestSerial);
        }
    }
}