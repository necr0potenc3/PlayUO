namespace Client
{
    public class PAssistVersion : Packet
    {
        public PAssistVersion(int value, string version) : base(190, "Assist Version")
        {
            base.m_Stream.Write(value);
            base.m_Stream.Write(version);
            base.m_Stream.Write((byte)0);
        }
    }
}