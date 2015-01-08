namespace Client
{
    using System;
    using System.Collections;

    public class TransparentDraw
    {
        public bool m_bAlpha;
        public bool m_Double;
        public float m_fAlpha;
        private static Queue m_Pool;
        public Texture m_Texture;
        public int m_X;
        public int m_Y;

        private TransparentDraw(Texture tex, int x, int y, bool hasAlpha, float theAlpha, bool xDouble)
        {
            this.m_Texture = tex;
            this.m_X = x;
            this.m_Y = y;
            if (!hasAlpha)
            {
                theAlpha = 1f;
            }
            theAlpha *= 0.5f;
            this.m_bAlpha = true;
            this.m_fAlpha = theAlpha;
            this.m_Double = xDouble;
        }

        public void Dispose()
        {
            m_Pool.Enqueue(this);
        }

        public static TransparentDraw PoolInstance(Texture tex, int x, int y, bool hasAlpha, float theAlpha, bool xDouble)
        {
            if (m_Pool == null)
            {
                m_Pool = new Queue();
            }
            if (m_Pool.Count > 0)
            {
                TransparentDraw draw = (TransparentDraw) m_Pool.Dequeue();
                draw.m_Texture = tex;
                draw.m_X = x;
                draw.m_Y = y;
                if (!hasAlpha)
                {
                    theAlpha = 1f;
                }
                theAlpha *= 0.5f;
                draw.m_bAlpha = true;
                draw.m_fAlpha = theAlpha;
                draw.m_Double = xDouble;
                return draw;
            }
            return new TransparentDraw(tex, x, y, hasAlpha, theAlpha, xDouble);
        }
    }
}

