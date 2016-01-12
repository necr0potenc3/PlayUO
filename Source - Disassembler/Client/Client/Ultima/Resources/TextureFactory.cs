namespace Client
{
    using Microsoft.DirectX.Direct3D;
    using System.Collections;

    public abstract class TextureFactory
    {
        public static Queue m_Disposing = new Queue();
        private static ArrayList m_Factories = new ArrayList();
        private ArrayList m_Textures = new ArrayList();

        public TextureFactory()
        {
            m_Factories.Add(this);
        }

        public void Cleanup(int timeNow)
        {
            int num = timeNow - 0x3a98;
            for (int i = 0; i < this.m_Textures.Count; i++)
            {
                Client.Texture texture = (Client.Texture)this.m_Textures[i];
                if ((texture.m_Surface == null) || texture.m_Surface.Disposed)
                {
                    this.m_Textures.RemoveAt(i--);
                }
                else if (texture.m_LastAccess <= num)
                {
                    m_Disposing.Enqueue(texture);
                }
            }
        }

        protected unsafe Client.Texture Construct(bool isReconstruct)
        {
            int num;
            int num2;
            if (!this.CoreLookup())
            {
                return Client.Texture.Empty;
            }
            this.CoreGetDimensions(out num, out num2);
            Client.Texture tex = new Client.Texture(num, num2, true, Format.A1R5G5B5, Pool.Managed, isReconstruct);
            if (tex.IsEmpty())
            {
                return Client.Texture.Empty;
            }
            LockData data = tex.Lock(Client.LockFlags.WriteOnly);
            this.CoreProcessImage(data.Width, data.Height, data.Pitch, (ushort*)data.pvSrc, (ushort*)((int)data.pvSrc + (data.Width * 2)), (ushort*)((int)data.pvSrc + (data.Height * data.Pitch)), (data.Pitch >> 1) - data.Width, data.Pitch >> 1);
            tex.Unlock();
            this.CoreAssignArgs(tex);
            this.m_Textures.Add(tex);
            return tex;
        }

        protected abstract void CoreAssignArgs(Client.Texture tex);

        protected abstract void CoreGetDimensions(out int width, out int height);

        protected abstract bool CoreLookup();

        protected abstract unsafe void CoreProcessImage(int width, int height, int stride, ushort* pLine, ushort* pLineEnd, ushort* pImageEnd, int lineDelta, int lineEndDelta);

        public static void FullCleanup(int timeNow)
        {
            for (int i = 0; i < m_Factories.Count; i++)
            {
                ((TextureFactory)m_Factories[i]).Cleanup(timeNow);
            }
        }

        public abstract Client.Texture Reconstruct(object[] args);

        public void Remove(Client.Texture tex)
        {
            this.m_Textures.Remove(tex);
        }

        public static void StippleDispose(int timeNow)
        {
            int num = timeNow - 0x3a98;
            for (int i = 0; (i < 0x1388) && (m_Disposing.Count > 0); i++)
            {
                object obj2 = m_Disposing.Dequeue();
                if (obj2 is Frames)
                {
                    Frames frames = (Frames)obj2;
                    if (frames.Disposed || (frames.LastAccessTime <= num))
                    {
                        frames.Dispose();
                        Engine.m_Animations.m_Frames.Remove(frames);
                        i += frames.FrameCount;
                    }
                }
                else
                {
                    Client.Texture texture = (Client.Texture)obj2;
                    if (!texture.m_Surface.Disposed)
                    {
                        texture.m_Surface.Dispose();
                    }
                    if (texture.m_Factory != null)
                    {
                        texture.m_Factory.m_Textures.Remove(texture);
                    }
                }
            }
        }
    }
}