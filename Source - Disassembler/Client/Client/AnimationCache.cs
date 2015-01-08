namespace Client
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class AnimationCache
    {
        private Hashtable m_Cache = new Hashtable();
        private IHue m_Hue;

        public AnimationCache(IHue hue)
        {
            this.m_Hue = hue;
        }

        public void Dispose()
        {
            IEnumerator enumerator = this.m_Cache.Values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Frames current = (Frames) enumerator.Current;
                for (int i = 0; i < current.FrameCount; i++)
                {
                    Frame frame = current.FrameList[i];
                    if ((frame != null) && (frame.Image != null))
                    {
                        frame.Image.Dispose();
                    }
                }
            }
            this.m_Cache.Clear();
            this.m_Cache = null;
            this.m_Hue = null;
        }

        public Frames this[int realID]
        {
            get
            {
                object obj2 = this.m_Cache[realID];
                if ((obj2 == null) || ((Frames) obj2).Disposed)
                {
                    obj2 = Engine.m_Animations.Create(realID, this.m_Hue);
                    this.m_Cache[realID] = obj2;
                }
                return (Frames) obj2;
            }
        }
    }
}

