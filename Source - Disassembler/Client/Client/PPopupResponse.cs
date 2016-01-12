namespace Client
{
    public class PPopupResponse : Packet
    {
        public PPopupResponse(object o, int EntryID) : base(0xbf, "Popup Response")
        {
            base.m_Stream.Write((short)0x15);
            base.m_Stream.Write((o is Item) ? ((Item)o).Serial : ((Mobile)o).Serial);
            base.m_Stream.Write((short)EntryID);
        }
    }
}