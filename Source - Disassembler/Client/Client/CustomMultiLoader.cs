namespace Client
{
    using System;
    using System.Collections;

    public class CustomMultiLoader
    {
        private static Hashtable m_Hashtable = new Hashtable();

        public static CustomMultiEntry GetCustomMulti(int serial, int revision)
        {
            ArrayList list = (ArrayList) m_Hashtable[serial];
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    CustomMultiEntry entry = (CustomMultiEntry) list[i];
                    if (entry.Revision == revision)
                    {
                        return entry;
                    }
                }
            }
            return null;
        }

        public static void SetCustomMulti(int serial, int revision, Multi baseMulti, int compressionType, byte[] buffer)
        {
            ArrayList list = (ArrayList) m_Hashtable[serial];
            if (list == null)
            {
                m_Hashtable[serial] = list = new ArrayList();
            }
            CustomMultiEntry entry = new CustomMultiEntry(serial, revision, baseMulti, compressionType, buffer);
            for (int i = 0; i < list.Count; i++)
            {
                CustomMultiEntry entry2 = (CustomMultiEntry) list[i];
                if (entry2.Revision == revision)
                {
                    list[i] = entry;
                    return;
                }
            }
            list.Add(entry);
            Map.Invalidate();
            GRadar.Invalidate();
        }
    }
}

