namespace Client
{
    using System;

    public class TextMessage : IComparable
    {
        protected TimeDelay m_Delay;
        protected bool m_Disposing;
        protected Texture m_Image;
        protected TimeSync m_Sync;
        protected int m_Timestamp;
        private VertexCache m_vCache;
        private static VertexCachePool m_vPool = new VertexCachePool();
        protected int m_X;
        protected int m_Y;

        public TextMessage(string Message) : this(Message, Engine.SystemDuration, Engine.DefaultFont, Engine.DefaultHue)
        {
        }

        public TextMessage(string Message, float Delay) : this(Message, Delay, Engine.DefaultFont, Engine.DefaultHue)
        {
        }

        public TextMessage(string Message, float Delay, IFont Font) : this(Message, Delay, Font, Engine.DefaultHue)
        {
        }

        public TextMessage(string Message, float Delay, IFont Font, IHue Hue)
        {
            this.m_Timestamp = Engine.Ticks;
            this.m_Image = Font.GetString(Message, Hue);
            this.m_Delay = new TimeDelay(Delay);
        }

        public int CompareTo(object a)
        {
            if (a == null)
            {
                return -1;
            }
            if (a != this)
            {
                TextMessage message = (TextMessage)a;
                if (this.m_Timestamp < message.m_Timestamp)
                {
                    return -1;
                }
                if (this.m_Timestamp > message.m_Timestamp)
                {
                    return 1;
                }
            }
            return 0;
        }

        public void Dispose()
        {
            this.m_Disposing = true;
            this.m_Sync = new TimeSync(1.0);
        }

        public void Draw(int x, int y)
        {
            if (this.m_vCache == null)
            {
                this.m_vCache = this.VCPool.GetInstance();
            }
            this.m_vCache.Draw(this.m_Image, x, y);
        }

        public void OnRemove()
        {
            this.VCPool.ReleaseInstance(this.m_vCache);
            this.m_vCache = null;
        }

        public float Alpha
        {
            get
            {
                if (!this.m_Disposing)
                {
                    return 1f;
                }
                return (float)(1.0 - this.m_Sync.Normalized);
            }
        }

        public bool Disposing
        {
            get
            {
                return this.m_Disposing;
            }
        }

        public bool Elapsed
        {
            get
            {
                return (this.m_Disposing || this.m_Delay.Elapsed);
            }
        }

        public Texture Image
        {
            get
            {
                return this.m_Image;
            }
        }

        protected VertexCachePool VCPool
        {
            get
            {
                return m_vPool;
            }
        }

        public virtual int X
        {
            get
            {
                return this.m_X;
            }
            set
            {
                this.m_X = value;
            }
        }

        public virtual int Y
        {
            get
            {
                return this.m_Y;
            }
            set
            {
                this.m_Y = value;
            }
        }
    }
}