namespace Client
{
    using System.Collections;

    public class LandCache
    {
        private Hashtable m_Cache = new Hashtable();
        private IHue m_Hue;

        public LandCache(IHue hue)
        {
            this.m_Hue = hue;
        }

        public void Dispose()
        {
            IEnumerator enumerator = this.m_Cache.Values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ((Texture)enumerator.Current).Dispose();
            }
            this.m_Cache.Clear();
            this.m_Cache = null;
            this.m_Hue = null;
        }

        public Texture this[int landID]
        {
            get
            {
                landID &= 0x3fff;
                object obj2 = this.m_Cache[landID];
                if (obj2 == null)
                {
                    obj2 = Engine.LandArt.ReadFromDisk(landID, this.m_Hue);
                    this.m_Cache.Add(landID, obj2);
                }
                return (Texture)obj2;
            }
        }
    }
}