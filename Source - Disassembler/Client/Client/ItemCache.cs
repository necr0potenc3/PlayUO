namespace Client
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class ItemCache
    {
        private Hashtable m_Cache = new Hashtable();
        private IHue m_Hue;

        public ItemCache(IHue hue)
        {
            this.m_Hue = hue;
        }

        public void Dispose()
        {
            IEnumerator enumerator = this.m_Cache.Values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ((Texture) enumerator.Current).Dispose();
            }
            this.m_Cache.Clear();
            this.m_Cache = null;
            this.m_Hue = null;
        }

        public Texture this[int itemID]
        {
            get
            {
                itemID &= 0x3fff;
                object obj2 = this.m_Cache[itemID];
                if (obj2 == null)
                {
                    obj2 = Engine.ItemArt.ReadFromDisk(itemID, this.m_Hue);
                    this.m_Cache.Add(itemID, obj2);
                }
                return (Texture) obj2;
            }
        }
    }
}

