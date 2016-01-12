namespace Client
{
    using Client.Targeting;

    public class PTarget_Response : Packet
    {
        public PTarget_Response(int type, ServerTargetHandler handler, int serial, int x, int y, int z, int id) : base(0x6c, "Target Response", 0x13)
        {
            base.m_Stream.Write((byte)type);
            base.m_Stream.Write(handler.TargetID);
            base.m_Stream.Write((byte)handler.Flags);
            base.m_Stream.Write(serial);
            base.m_Stream.Write((short)x);
            base.m_Stream.Write((short)y);
            base.m_Stream.Write((short)z);
            base.m_Stream.Write((short)id);
        }
    }
}