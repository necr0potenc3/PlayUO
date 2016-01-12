namespace Client
{
    public class PBuyItems : Packet
    {
        public PBuyItems(int serial, BuyInfo[] info) : base(0x3b, "Buy Items")
        {
            base.m_Stream.Write(serial);
            base.m_Stream.Write((byte)2);
            for (int i = 0; i < info.Length; i++)
            {
                if (info[i].ToBuy > 0)
                {
                    base.m_Stream.Write((byte)0x1a);
                    base.m_Stream.Write(info[i].Item.Serial);
                    base.m_Stream.Write((short)info[i].ToBuy);
                }
            }
        }
    }
}