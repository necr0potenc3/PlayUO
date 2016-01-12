namespace Client
{
    using Client.Targeting;

    public class PTarget_Cancel : Packet
    {
        public PTarget_Cancel(ServerTargetHandler handler) : base(0x6c, "Target Cancel", 0x13)
        {
            base.m_Stream.Write((byte)0);
            base.m_Stream.Write(handler.TargetID);
            base.m_Stream.Write((byte)handler.Flags);
            base.m_Stream.Write(0);
            base.m_Stream.Write(-1);
            base.m_Stream.Write(0);
        }
    }
}