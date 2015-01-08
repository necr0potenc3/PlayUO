namespace Client
{
    using Microsoft.DirectX.Direct3D;
    using System;

    public class VertexCache
    {
        private bool m_bPool;
        private int m_RenderVersion;
        private CustomVertex.TransformedColoredTextured[] m_vPool;
        private int m_xPool;
        private int m_yPool;

        public VertexCache() : this(VertexConstructor.Create())
        {
        }

        public VertexCache(CustomVertex.TransformedColoredTextured[] v)
        {
            this.m_vPool = v;
            this.m_xPool = this.m_yPool = -2147483648;
        }

        public unsafe void Draw(Texture t, int x, int y)
        {
            if (((this.m_xPool != x) || (this.m_yPool != y)) || (this.m_RenderVersion != Renderer.m_Version))
            {
                this.m_RenderVersion = Renderer.m_Version;
                this.m_xPool = x;
                this.m_yPool = y;
                this.m_bPool = t.Draw(x, y, this.m_vPool);
            }
            else if (this.m_bPool)
            {
                fixed (CustomVertex.TransformedColoredTextured* texturedRef = this.m_vPool)
                {
                    int quadColor = Renderer.GetQuadColor(0xffffff);
                    texturedRef->Color = quadColor;
                    texturedRef[1].Color = quadColor;
                    texturedRef[2].Color = quadColor;
                    texturedRef[3].Color = quadColor;
                    Renderer.SetTexture(t);
                    Renderer.DrawQuadPrecalc(texturedRef);
                }
            }
        }

        public unsafe void Draw(Texture t, int x, int y, int color)
        {
            if (((this.m_xPool != x) || (this.m_yPool != y)) || (this.m_RenderVersion != Renderer.m_Version))
            {
                this.m_RenderVersion = Renderer.m_Version;
                this.m_xPool = x;
                this.m_yPool = y;
                this.m_bPool = t.Draw(x, y, color, this.m_vPool);
            }
            else if (this.m_bPool)
            {
                fixed (CustomVertex.TransformedColoredTextured* texturedRef = this.m_vPool)
                {
                    color = Renderer.GetQuadColor(color);
                    texturedRef->Color = color;
                    texturedRef[1].Color = color;
                    texturedRef[2].Color = color;
                    texturedRef[3].Color = color;
                    Renderer.SetTexture(t);
                    Renderer.DrawQuadPrecalc(texturedRef);
                }
            }
        }

        public unsafe void DrawGame(Texture t, int x, int y)
        {
            if (((this.m_xPool != x) || (this.m_yPool != y)) || (this.m_RenderVersion != Renderer.m_Version))
            {
                this.m_RenderVersion = Renderer.m_Version;
                this.m_xPool = x;
                this.m_yPool = y;
                this.m_bPool = t.DrawGame(x, y, this.m_vPool);
            }
            else if (this.m_bPool)
            {
                fixed (CustomVertex.TransformedColoredTextured* texturedRef = this.m_vPool)
                {
                    int quadColor = Renderer.GetQuadColor(0xffffff);
                    texturedRef->Color = quadColor;
                    texturedRef[1].Color = quadColor;
                    texturedRef[2].Color = quadColor;
                    texturedRef[3].Color = quadColor;
                    Renderer.SetTexture(t);
                    Renderer.DrawQuadPrecalc(texturedRef);
                }
            }
        }

        public void Invalidate()
        {
            this.m_xPool = this.m_yPool = -2147483648;
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

