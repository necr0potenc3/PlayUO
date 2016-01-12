namespace Client
{
    using System;
    using System.Collections;

    public class FontCache
    {
        private Hashtable m_Cached;
        private IFontFactory m_Factory;

        public FontCache(IFontFactory Factory)
        {
            if (Factory == null)
            {
                throw new ArgumentNullException("Factory");
            }
            this.m_Factory = Factory;
            this.m_Cached = new Hashtable(0x20, 0.5f);
        }

        public void Dispose()
        {
            IEnumerator enumerator = this.m_Cached.Values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ((Texture)enumerator.Current).Dispose();
            }
            this.m_Cached.Clear();
            this.m_Cached = null;
        }

        public Texture this[string Key, IHue Hue]
        {
            get
            {
                if (Key == null)
                {
                    Debug.Trace("FontCache[] crash averted where key == null");
                    Key = "";
                }
                CacheKey key = new CacheKey(Key, Hue);
                Texture texture = (Texture)this.m_Cached[key];
                if (texture == null)
                {
                    texture = this.m_Factory.CreateInstance(Key, Hue);
                    this.m_Cached.Add(key, texture);
                }
                return texture;
            }
        }

        private class CacheKey
        {
            private int m_Hash;
            private IHue m_Hue;
            private string m_Text;

            public CacheKey(string text, IHue hue)
            {
                this.m_Text = text;
                this.m_Hue = hue;
                this.m_Hash = text.GetHashCode() ^ hue.GetHashCode();
            }

            public override bool Equals(object x)
            {
                FontCache.CacheKey key = (FontCache.CacheKey)x;
                return ((this == key) || (((this.m_Hash == key.m_Hash) && this.m_Hue.Equals(key.m_Hue)) && (this.m_Text == key.m_Text)));
            }

            public override int GetHashCode()
            {
                return this.m_Hash;
            }
        }
    }
}