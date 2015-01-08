namespace Client
{
    using System;
    using System.Collections;

    public class PGumpButton : Packet
    {
        public PGumpButton(GServerGump gump, int buttonID) : base(0xb1, "Gump Response")
        {
            base.m_Stream.Write(gump.Serial);
            base.m_Stream.Write(gump.DialogID);
            base.m_Stream.Write(buttonID);
            ArrayList dataStore = Engine.GetDataStore();
            ArrayList list = Engine.GetDataStore();
            Gump[] gumpArray = gump.Children.ToArray();
            for (int i = 0; i < gumpArray.Length; i++)
            {
                if (gumpArray[i] is IRelayedSwitch)
                {
                    IRelayedSwitch switch2 = (IRelayedSwitch) gumpArray[i];
                    if (switch2.Active)
                    {
                        dataStore.Add(switch2.RelayID);
                    }
                }
                else if (gumpArray[i] is GServerTextBox)
                {
                    list.Add(gumpArray[i]);
                }
            }
            base.m_Stream.Write(dataStore.Count);
            for (int j = 0; j < dataStore.Count; j++)
            {
                base.m_Stream.Write((int) dataStore[j]);
            }
            base.m_Stream.Write(list.Count);
            for (int k = 0; k < list.Count; k++)
            {
                GServerTextBox box = (GServerTextBox) list[k];
                base.m_Stream.Write((short) box.RelayID);
                base.m_Stream.Write((short) box.String.Length);
                base.m_Stream.WriteUnicode(box.String);
            }
            Engine.ReleaseDataStore(list);
            Engine.ReleaseDataStore(dataStore);
        }

        public PGumpButton(int serial, int dialogID, int buttonID) : base(0xb1, "Gump Response")
        {
            base.m_Stream.Write(serial);
            base.m_Stream.Write(dialogID);
            base.m_Stream.Write(buttonID);
            base.m_Stream.Write(0);
            base.m_Stream.Write(0);
        }
    }
}

