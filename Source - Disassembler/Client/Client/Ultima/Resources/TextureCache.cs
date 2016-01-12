namespace Client
{
    using System.Collections;

    public class TextureCache
    {
        private Hashtable m_Cache = new Hashtable();
        private IHue m_Hue;

        public TextureCache(IHue hue)
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

        public Texture this[int textureID]
        {
            get
            {
                textureID &= 0xfff;
                object obj2 = this.m_Cache[textureID];
                if (obj2 == null)
                {
                    obj2 = Engine.TextureArt.ReadFromDisk(textureID, this.m_Hue);
                    this.m_Cache.Add(textureID, obj2);
                }
                return (Texture)obj2;
            }
        }
    }
}