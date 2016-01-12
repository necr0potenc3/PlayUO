namespace Client
{
    public class PStringQueryResponse : Packet
    {
        public PStringQueryResponse(int Serial, short Type, string Response) : base(0xac, "String Query Response")
        {
            base.m_Stream.Write(Serial);
            base.m_Stream.Write(Type);
            base.m_Stream.Write(true);
            base.m_Stream.Write((short)(Response.Length + 1));
            base.m_Stream.Write(Response);
            base.m_Stream.Write((byte)0);
        }
    }
}