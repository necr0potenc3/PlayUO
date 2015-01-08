namespace Client
{
    using Microsoft.DirectX.Direct3D;
    using System;

    public class AnimationVertexCache
    {
        private bool m_bPool;
        private bool m_fPool;
        private Texture m_tPool;
        private CustomVertex.TransformedColoredTextured[] m_vPool;
        private int m_xPool;
        private int m_yPool;

        public AnimationVertexCache() : this(VertexConstructor.Create())
        {
        }

        public AnimationVertexCache(CustomVertex.TransformedColoredTextured[] v)
        {
            this.m_vPool = v;
            this.m_xPool = this.m_yPool = -2147483648;
        }

        public unsafe void Draw(Texture t, int x, int y)
        {
            if (((this.m_xPool != x) || (this.m_yPool != y)) || ((this.m_tPool != t) || (this.m_fPool != t.Flip)))
            {
                fixed (CustomVertex.TransformedColoredTextured* texturedRef = this.m_vPool)
                {
                    texturedRef->Tu = 0f;
                    texturedRef->Tv = 0f;
                    texturedRef[1].Tu = 0f;
                    texturedRef[1].Tv = 0f;
                    texturedRef[2].Tu = 0f;
                    texturedRef[2].Tv = 0f;
                    texturedRef[3].Tu = 0f;
                    texturedRef[3].Tv = 0f;
                    this.m_tPool = t;
                    this.m_xPool = x;
                    this.m_yPool = y;
                    this.m_fPool = t.Flip;
                    this.m_bPool = t.Draw(x, y, texturedRef);
                }
            }
            else if (this.m_bPool)
            {
                fixed (CustomVertex.TransformedColoredTextured* texturedRef2 = this.m_vPool)
                {
                    int quadColor = Renderer.GetQuadColor(0xffffff);
                    texturedRef2->Color = quadColor;
                    texturedRef2[1].Color = quadColor;
                    texturedRef2[2].Color = quadColor;
                    texturedRef2[3].Color = quadColor;
                    Renderer.SetTexture(t);
                    Renderer.DrawQuadPrecalc(texturedRef2);
                }
            }
        }

        public unsafe void DrawGame(Texture t, int x, int y)
        {
            if (((this.m_xPool != x) || (this.m_yPool != y)) || ((this.m_tPool != t) || (this.m_fPool != t.Flip)))
            {
                fixed (CustomVertex.TransformedColoredTextured* texturedRef = this.m_vPool)
                {
                    texturedRef->Tu = 0f;
                    texturedRef->Tv = 0f;
                    texturedRef[1].Tu = 0f;
                    texturedRef[1].Tv = 0f;
                    texturedRef[2].Tu = 0f;
                    texturedRef[2].Tv = 0f;
                    texturedRef[3].Tu = 0f;
                    texturedRef[3].Tv = 0f;
                    this.m_tPool = t;
                    this.m_xPool = x;
                    this.m_yPool = y;
                    this.m_fPool = t.Flip;
                    this.m_bPool = t.DrawGame(x, y, texturedRef);
                }
            }
            else if (this.m_bPool)
            {
                fixed (CustomVertex.TransformedColoredTextured* texturedRef2 = this.m_vPool)
                {
                    int quadColor = Renderer.GetQuadColor(0xffffff);
                    texturedRef2->Color = quadColor;
                    texturedRef2[1].Color = quadColor;
                    texturedRef2[2].Color = quadColor;
                    texturedRef2[3].Color = quadColor;
                    Renderer.SetTexture(t);
                    Renderer.DrawQuadPrecalc(texturedRef2);
                }
            }
        }

        public void Invalidate()
        {
            this.m_xPool = this.m_yPool = -2147483648;
            this.m_tPool = null;
        }

        public CustomVertex.TransformedColoredTextured[] Vertices
        {
            get
            {
                return this.m_vPool;
            }
        }
    }
}

