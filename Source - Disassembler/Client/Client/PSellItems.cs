namespace Client
{
    using System;
    using System.Collections;

    public class PSellItems : Packet
    {
        public PSellItems(int serial, SellInfo[] info) : base(0x9f, "Sell Items")
        {
            ArrayList dataStore = Engine.GetDataStore();
            for (int i = 0; i < info.Length; i++)
            {
                if (info[i].ToSell > 0)
                {
                    dataStore.Add(info[i]);
                }
            }
            base.m_Stream.Write(serial);
            base.m_Stream.Write((ushort) dataStore.Count);
            for (int j = 0; j < dataStore.Count; j++)
            {
                SellInfo info2 = (SellInfo) dataStore[j];
                base.m_Stream.Write(info2.Item.Serial);
                base.m_Stream.Write((ushort) info2.ToSell);
            }
            Engine.ReleaseDataStore(dataStore);
        }
    }
}

