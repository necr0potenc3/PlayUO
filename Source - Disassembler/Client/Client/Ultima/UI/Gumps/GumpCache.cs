namespace Client
{
    using System.Collections;

    public class GumpCache
    {
        private Hashtable m_Cache = new Hashtable();
        private IHue m_Hue;

        public GumpCache(IHue hue)
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

        public Texture this[int gumpID]
        {
            get
            {
                gumpID &= 0xffff;
                object obj2 = this.m_Cache[gumpID];
                if (obj2 == null)
                {
                    obj2 = Engine.m_Gumps.ReadFromDisk(gumpID, this.m_Hue);
                    this.m_Cache.Add(gumpID, obj2);
                }
                return (Texture)obj2;
            }
        }
    }
}