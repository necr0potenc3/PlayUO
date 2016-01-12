namespace Client
{
    public class PPrompt_Cancel_ASCII : Packet
    {
        public PPrompt_Cancel_ASCII(int serial, int prompt) : base(0x9a, "ASCII Prompt Cancel")
        {
            base.m_Stream.Write(serial);
            base.m_Stream.Write(prompt);
            base.m_Stream.Write(0);
            base.m_Stream.Write((byte)0);
        }
    }
}