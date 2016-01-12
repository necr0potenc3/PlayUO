namespace Client
{
    public class PAction : Packet
    {
        public PAction(string Action) : base(0x12, string.Format("Action : \"{0}\"", Action))
        {
            base.m_Stream.Write((byte)0xc7);
            base.m_Stream.Write(Action);
            base.m_Stream.Write((byte)0);
        }
    }
}