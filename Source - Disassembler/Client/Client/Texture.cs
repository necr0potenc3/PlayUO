namespace Client
{
    using Microsoft.DirectX;
    using Microsoft.DirectX.Direct3D;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    public class Texture
    {
        private const int DoubleOpaque = -2147450880;
        public int Height;
        protected static int[] m_2Pow;
        private static CustomVertex.TransformedColoredTextured[] m_BadClipperPool = VertexConstructor.Create();
        private bool m_bDrawXY;
        protected static bool m_CanSysMem;
        protected static bool m_CanVidMem;
        private bool m_Disposed;
        private static Client.Texture m_Empty;
        public TextureFactory m_Factory;
        public object[] m_FactoryArgs;
        private bool m_fDrawXY;
        protected float m_fHeight;
        protected bool m_Flip;
        private bool m_FourBPP;
        protected float m_fWidth;
        public int m_LastAccess;
        private GraphicsStream m_LockStream;
        protected static int m_MaxAspect;
        protected static int m_MaxTextureHeight;
        protected static int m_MaxTextureWidth;
        protected float m_MaxTU;
        protected float m_MaxTV;
        protected static int m_MinTextureHeight;
        protected static int m_MinTextureWidth;
        private static CustomVertex.TransformedColoredTextured[] m_PoolClipped;
        private static CustomVertex.TransformedColoredTextured[] m_PoolRotated;
        private static CustomVertex.TransformedColoredTextured[] m_PoolXYWH;
        protected static bool m_Pow2;
        protected static bool m_Square;
        public Microsoft.DirectX.Direct3D.Texture m_Surface;
        protected int m_TexHeight;
        public static ArrayList m_Textures;
        protected int m_TexWidth;
        public TextureVB[] m_VBs;
        private CustomVertex.TransformedColoredTextured[] m_vDrawXY;
        private int m_xDrawXY;
        private int m_yDrawXY;
        public const ushort Opaque = 0x8000;
        public const ushort Transparent = 0;
        public int Width;
        public int xMax;
        public int xMin;
        public int yMax;
        public int yMin;

        static Texture()
        {
            int num = 0x18;
            m_Textures = new ArrayList(0x80);
            m_2Pow = new int[num];
            for (int i = 0; i < num; i++)
            {
                m_2Pow[i] = ((int)1) << i;
            }
            m_PoolXYWH = VertexConstructor.Create();
            m_PoolClipped = VertexConstructor.Create();
        }

        protected Texture()
        {
            this.m_xDrawXY = 0x7fffffff;
            this.m_yDrawXY = 0x7fffffff;
        }

        public Texture(Bitmap bmp)
        {
            this.m_xDrawXY = 0x7fffffff;
            this.m_yDrawXY = 0x7fffffff;
            this.Width = bmp.Width;
            this.Height = bmp.Height;
            this.m_Surface = Microsoft.DirectX.Direct3D.Texture.FromBitmap(Engine.m_Device, bmp, Usage.None, Pool.Managed);
            SurfaceDescription levelDescription = this.m_Surface.GetLevelDescription(0);
            this.m_FourBPP = levelDescription.Format == Format.A8R8G8B8;
            this.m_TexWidth = levelDescription.Width;
            this.m_TexHeight = levelDescription.Height;
            this.m_MaxTU = (float)(((double)this.Width) / ((double)this.m_TexWidth));
            this.m_MaxTV = (float)(((double)this.Height) / ((double)this.m_TexHeight));
            this.m_fWidth = this.Width;
            this.m_fHeight = this.Height;
            this.xMax = this.Width - 1;
            this.yMax = this.Height - 1;
            m_Textures.Add(this);
        }

        public Texture(int Width, int Height, bool VideoMemory) : this(Width, Height, VideoMemory, Format.A1R5G5B5)
        {
        }

        public Texture(int Width, int Height, bool VideoMemory, Format fmt) : this(Width, Height, VideoMemory, fmt, Pool.Managed)
        {
        }

        public Texture(int Width, int Height, bool VideoMemory, Format fmt, Pool pool) : this(Width, Height, VideoMemory, fmt, pool, false)
        {
        }

        public Texture(int Width, int Height, bool VideoMemory, Format fmt, Pool pool, bool isReconstruct)
        {
            this.m_xDrawXY = 0x7fffffff;
            this.m_yDrawXY = 0x7fffffff;
            if (VideoMemory && !m_CanVidMem)
            {
                VideoMemory = false;
            }
            else if ((!VideoMemory && !m_CanSysMem) && m_CanVidMem)
            {
                VideoMemory = true;
            }
            else if (!m_CanVidMem && !m_CanSysMem)
            {
                return;
            }
            int minTextureWidth = 0;
            int minTextureHeight = 0;
            if (m_Pow2)
            {
                int num3 = 0;
                while (minTextureWidth < Width)
                {
                    minTextureWidth = m_2Pow[num3++];
                }
                num3 = 0;
                while (minTextureHeight < Height)
                {
                    minTextureHeight = m_2Pow[num3++];
                }
            }
            else
            {
                minTextureWidth = Width;
                minTextureHeight = Height;
            }
            if (m_MaxAspect != 0)
            {
                double num4 = 0.0;
                if (minTextureWidth > minTextureHeight)
                {
                    num4 = ((double)minTextureWidth) / ((double)minTextureHeight);
                }
                else
                {
                    num4 = ((double)minTextureHeight) / ((double)minTextureWidth);
                }
                if (num4 > m_MaxAspect)
                {
                    if (minTextureWidth > minTextureHeight)
                    {
                        minTextureHeight = minTextureWidth / m_MaxAspect;
                    }
                    else
                    {
                        minTextureWidth = minTextureHeight / m_MaxAspect;
                    }
                }
            }
            if (minTextureWidth < m_MinTextureWidth)
            {
                minTextureWidth = m_MinTextureWidth;
            }
            if (minTextureHeight < m_MinTextureHeight)
            {
                minTextureHeight = m_MinTextureHeight;
            }
            if (m_Square)
            {
                if (minTextureWidth > minTextureHeight)
                {
                    minTextureHeight = minTextureWidth;
                }
                else if (minTextureWidth < minTextureHeight)
                {
                    minTextureWidth = minTextureHeight;
                }
            }
            if ((minTextureWidth <= m_MaxTextureWidth) && (minTextureHeight <= m_MaxTextureHeight))
            {
                this.Width = Width;
                this.Height = Height;
                this.m_TexWidth = minTextureWidth;
                this.m_TexHeight = minTextureHeight;
                this.m_MaxTU = (float)(((double)Width) / ((double)minTextureWidth));
                this.m_MaxTV = (float)(((double)Height) / ((double)minTextureHeight));
                this.m_fWidth = Width;
                this.m_fHeight = Height;
                this.m_Surface = new Microsoft.DirectX.Direct3D.Texture(Engine.m_Device, this.m_TexWidth, this.m_TexHeight, 1, Usage.None, fmt, pool);
                this.xMax = Width - 1;
                this.yMax = Height - 1;
                if (!isReconstruct)
                {
                    m_Textures.Add(this);
                }
            }
        }

        public void Clear()
        {
            this.Clear(this.Lock(Client.LockFlags.WriteOnly));
            this.Unlock();
        }

        public unsafe void Clear(LockData ld)
        {
            int num = ld.Pitch * ld.Height;
            int num2 = num >> 2;
            num &= 3;
            int* pvSrc = (int*)ld.pvSrc;
            while (--num2 >= 0)
            {
                pvSrc++;
                pvSrc[0] = 0;
            }
            if (num != 0)
            {
                byte* numPtr2 = (byte*)ld.pvSrc;
                while (--num != 0)
                {
                    *(numPtr2++) = 0;
                }
            }
        }

        public unsafe void Clear(ushort Color)
        {
            LockData data = this.Lock(Client.LockFlags.WriteOnly);
            ushort* pvSrc = (ushort*)data.pvSrc;
            int num = this.m_TexHeight * (data.Pitch >> 1);
            while (num-- != 0)
            {
                pvSrc++;
                pvSrc[0] = Color;
            }
            this.Unlock();
        }

        public static unsafe void ClearPixels(void* pvClear, int Pixels)
        {
            int num = Pixels >> 1;
            int* numPtr = (int*)pvClear;
            while (--num >= 0)
            {
                numPtr++;
                numPtr[0] = 0;
            }
            if ((Pixels & 1) != 0)
            {
                *((short*)numPtr) = 0;
            }
        }

        public static unsafe void CopyPixels(void* pvSrc, void* pvDest, int Pixels)
        {
            int num = Pixels >> 1;
            int* numPtr = (int*)pvSrc;
            int* numPtr2 = (int*)pvDest;
            int num2 = num & 7;
            num = num >> 3;
            while (--num >= 0)
            {
                numPtr2[0] = numPtr[0] | -2147450880;
                numPtr2[1] = numPtr[1] | -2147450880;
                numPtr2[2] = numPtr[2] | -2147450880;
                numPtr2[3] = numPtr[3] | -2147450880;
                numPtr2[4] = numPtr[4] | -2147450880;
                numPtr2[5] = numPtr[5] | -2147450880;
                numPtr2[6] = numPtr[6] | -2147450880;
                numPtr2[7] = numPtr[7] | -2147450880;
                numPtr2 += 8;
                numPtr += 8;
            }
            while (--num2 >= 0)
            {
                numPtr2++;
                numPtr++;
                numPtr2[0] = numPtr[0] | -2147450880;
            }
            if ((Pixels & 1) != 0)
            {
                *((short*)numPtr2) = (short)(0x8000 | *(((ushort*)numPtr)));
            }
        }

        protected Microsoft.DirectX.Direct3D.Texture CoreGetSurface()
        {
            this.m_LastAccess = Engine.Ticks;
            if (this.m_Surface == null)
            {
                return null;
            }
            if (this.m_Surface.Disposed)
            {
                return (this.m_Surface = this.CoreReconstruct());
            }
            return this.m_Surface;
        }

        protected Microsoft.DirectX.Direct3D.Texture CoreReconstruct()
        {
            if (this.m_Factory == null)
            {
                return null;
            }
            return this.m_Factory.Reconstruct(this.m_FactoryArgs).m_Surface;
        }

        public void Dispose()
        {
            if (!this.m_Disposed)
            {
                this.m_Disposed = true;
                if (this.m_Surface != null)
                {
                    this.m_Surface.Dispose();
                }
                this.m_Surface = null;
                this.m_vDrawXY = null;
            }
        }

        public static void DisposeAll()
        {
            StreamWriter writer = null;
            Client.Texture[] textureArray = (Client.Texture[])m_Textures.ToArray(typeof(Client.Texture));
            for (int i = 0; i < textureArray.Length; i++)
            {
                Client.Texture texture = textureArray[i];
                if (texture != null)
                {
                    if (texture.m_Surface != null)
                    {
                        string message = "Texture leak found";
                        Debug.Trace(message);
                        if (writer == null)
                        {
                            writer = new StreamWriter(Engine.FileManager.CreateUnique("Data/Logs/Textures", ".log"));
                        }
                        writer.WriteLine(message);
                        writer.Flush();
                    }
                    if (!texture.m_Disposed)
                    {
                        texture.Dispose();
                    }
                    textureArray[i] = null;
                }
            }
            m_Textures.Clear();
            m_Textures = null;
            m_2Pow = null;
            if (writer != null)
            {
                writer.Close();
            }
        }

        public unsafe void Draw(int X, int Y)
        {
            if (this.m_Surface != null)
            {
                if (((this.m_xDrawXY == X) && (this.m_yDrawXY == Y)) && (this.m_fDrawXY == this.m_Flip))
                {
                    if (this.m_bDrawXY)
                    {
                        fixed (CustomVertex.TransformedColoredTextured* texturedRef = this.m_vDrawXY)
                        {
                            int quadColor = Renderer.GetQuadColor(0xffffff);
                            texturedRef->Color = quadColor;
                            texturedRef[1].Color = quadColor;
                            texturedRef[2].Color = quadColor;
                            texturedRef[3].Color = quadColor;
                            Renderer.SetTexture(this);
                            Renderer.DrawQuadPrecalc(texturedRef);
                        }
                    }
                }
                else
                {
                    this.m_xDrawXY = X;
                    this.m_yDrawXY = Y;
                    this.m_fDrawXY = this.m_Flip;
                    this.m_bDrawXY = (((Y < Engine.ScreenHeight) && ((Y + this.Height) > 0)) && (X < Engine.ScreenWidth)) && ((X + this.Width) > 0);
                    if (this.m_bDrawXY)
                    {
                        this.m_vDrawXY = new CustomVertex.TransformedColoredTextured[4];
                        fixed (CustomVertex.TransformedColoredTextured* texturedRef2 = this.m_vDrawXY)
                        {
                            float num2 = -0.5f + X;
                            float num3 = -0.5f + Y;
                            texturedRef2->X = texturedRef2[1].X = num2 + this.m_fWidth;
                            texturedRef2->Y = texturedRef2[2].Y = num3 + this.m_fHeight;
                            texturedRef2[1].Y = texturedRef2[3].Y = num3;
                            texturedRef2[2].X = texturedRef2[3].X = num2;
                            int num4 = Renderer.GetQuadColor(0xffffff);
                            texturedRef2->Color = num4;
                            texturedRef2[1].Color = num4;
                            texturedRef2[2].Color = num4;
                            texturedRef2[3].Color = num4;
                            texturedRef2->Rhw = 1f;
                            texturedRef2[1].Rhw = 1f;
                            texturedRef2[2].Rhw = 1f;
                            texturedRef2[3].Rhw = 1f;
                            float maxTU = this.m_MaxTU;
                            float maxTV = this.m_MaxTV;
                            if (!this.m_Flip)
                            {
                                texturedRef2->Tv = maxTV;
                                texturedRef2->Tu = maxTU;
                                texturedRef2[1].Tu = maxTU;
                                texturedRef2[2].Tv = maxTV;
                            }
                            else
                            {
                                texturedRef2->Tv = this.m_MaxTV;
                                texturedRef2[2].Tv = this.m_MaxTV;
                                texturedRef2[2].Tu = maxTU;
                                texturedRef2[3].Tu = maxTU;
                            }
                            Renderer.SetTexture(this);
                            Renderer.DrawQuadPrecalc(texturedRef2);
                        }
                    }
                }
            }
        }

        public unsafe bool Draw(int x, int y, CustomVertex.TransformedColoredTextured* pVertex, bool tr = false)
        {
            if (this.m_Surface == null)
            {
                return false;
            }
            if ((((y >= Engine.ScreenHeight) || ((y + this.Height) <= 0)) || (x >= Engine.ScreenWidth)) || ((x + this.Width) <= 0))
            {
                return false;
            }
            float num = -0.5f + x;
            float num2 = -0.5f + y;
            pVertex->X = pVertex[1].X = num + this.m_fWidth;
            pVertex->Y = pVertex[2].Y = num2 + this.m_fHeight;
            pVertex[1].Y = pVertex[3].Y = num2;
            pVertex[2].X = pVertex[3].X = num;
            int quadColor = Renderer.GetQuadColor(0xffffff);
            pVertex->Color = quadColor;
            pVertex[1].Color = quadColor;
            pVertex[2].Color = quadColor;
            pVertex[3].Color = quadColor;
            float maxTU = this.m_MaxTU;
            float maxTV = this.m_MaxTV;
            if (!this.m_Flip)
            {
                pVertex->Tu = maxTU;
                pVertex->Tv = maxTV;
                pVertex[1].Tu = maxTU;
                pVertex[2].Tv = maxTV;
                pVertex[1].Tv = 0f;
                pVertex[2].Tu = 0f;
                pVertex[3].Tu = 0f;
                pVertex[3].Tv = 0f;
            }
            else
            {
                pVertex->Tv = maxTV;
                pVertex[2].Tv = maxTV;
                pVertex[2].Tu = maxTU;
                pVertex[3].Tu = maxTU;
                pVertex->Tu = 0f;
                pVertex[1].Tu = 0f;
                pVertex[1].Tv = 0f;
                pVertex[3].Tv = 0f;
            }
            Renderer.SetTexture(this);
            Renderer.DrawQuadPrecalc(pVertex);
            return true;
        }

        public bool Draw(int x, int y, CustomVertex.TransformedColoredTextured[] pool)
        {
            if (this.m_Surface == null)
            {
                return false;
            }
            if ((((y >= Engine.ScreenHeight) || ((y + this.Height) <= 0)) || (x >= Engine.ScreenWidth)) || ((x + this.Width) <= 0))
            {
                return false;
            }
            float num = -0.5f + x;
            float num2 = -0.5f + y;
            pool[0].X = pool[1].X = num + this.m_fWidth;
            pool[0].Y = pool[2].Y = num2 + this.m_fHeight;
            pool[1].Y = pool[3].Y = num2;
            pool[2].X = pool[3].X = num;
            pool[0].Color = pool[1].Color = pool[2].Color = pool[3].Color = Renderer.GetQuadColor(0xffffff);
            if (!this.m_Flip)
            {
                pool[0].Tu = pool[1].Tu = this.m_MaxTU;
                pool[0].Tv = pool[2].Tv = this.m_MaxTV;
                pool[3].Tu = pool[3].Tv = pool[2].Tu = pool[1].Tv = 0f;
            }
            else
            {
                pool[0].Tv = pool[2].Tv = this.m_MaxTV;
                pool[2].Tu = pool[3].Tu = this.m_MaxTU;
                pool[0].Tu = 0f;
                pool[1].Tu = 0f;
                pool[1].Tv = 0f;
                pool[3].Tv = 0f;
            }
            Renderer.SetTexture(this);
            Renderer.DrawQuadPrecalc(pool);
            return true;
        }

        public void Draw(int X, int Y, int Color)
        {
            if (this.m_Surface != null)
            {
                if (((this.m_xDrawXY == X) && (this.m_yDrawXY == Y)) && (this.m_fDrawXY == this.m_Flip))
                {
                    if (this.m_bDrawXY)
                    {
                        this.m_vDrawXY[0].Color = this.m_vDrawXY[1].Color = this.m_vDrawXY[2].Color = this.m_vDrawXY[3].Color = Renderer.GetQuadColor(Color);
                        Renderer.SetTexture(this);
                        Renderer.DrawQuadPrecalc(this.m_vDrawXY);
                    }
                }
                else
                {
                    this.m_xDrawXY = X;
                    this.m_yDrawXY = Y;
                    this.m_fDrawXY = this.m_Flip;
                    this.m_bDrawXY = (((Y < Engine.ScreenHeight) && ((Y + this.Height) > 0)) && (X < Engine.ScreenWidth)) && ((X + this.Width) > 0);
                    if (this.m_bDrawXY)
                    {
                        this.m_vDrawXY = new CustomVertex.TransformedColoredTextured[4];
                        float num = -0.5f + X;
                        float num2 = -0.5f + Y;
                        this.m_vDrawXY[0].X = this.m_vDrawXY[1].X = num + this.m_fWidth;
                        this.m_vDrawXY[0].Y = this.m_vDrawXY[2].Y = num2 + this.m_fHeight;
                        this.m_vDrawXY[1].Y = this.m_vDrawXY[3].Y = num2;
                        this.m_vDrawXY[2].X = this.m_vDrawXY[3].X = num;
                        this.m_vDrawXY[0].Color = this.m_vDrawXY[1].Color = this.m_vDrawXY[2].Color = this.m_vDrawXY[3].Color = Renderer.GetQuadColor(Color);
                        this.m_vDrawXY[0].Rhw = this.m_vDrawXY[1].Rhw = this.m_vDrawXY[2].Rhw = this.m_vDrawXY[3].Rhw = 1f;
                        if (!this.m_Flip)
                        {
                            this.m_vDrawXY[0].Tu = this.m_vDrawXY[1].Tu = this.m_MaxTU;
                            this.m_vDrawXY[0].Tv = this.m_vDrawXY[2].Tv = this.m_MaxTV;
                        }
                        else
                        {
                            this.m_vDrawXY[0].Tv = this.m_vDrawXY[2].Tv = this.m_MaxTV;
                            this.m_vDrawXY[2].Tu = this.m_vDrawXY[3].Tu = this.m_MaxTU;
                        }
                        Renderer.SetTexture(this);
                        Renderer.DrawQuadPrecalc(this.m_vDrawXY);
                    }
                }
            }
        }

        public bool Draw(int x, int y, int color, CustomVertex.TransformedColoredTextured[] pool)
        {
            if ((((this.m_Surface != null) && (y < Engine.ScreenHeight)) && (((y + this.Height) > 0) && (x < Engine.ScreenWidth))) && ((x + this.Width) > 0))
            {
                float num = -0.5f + x;
                float num2 = -0.5f + y;
                pool[0].X = pool[1].X = num + this.m_fWidth;
                pool[0].Y = pool[2].Y = num2 + this.m_fHeight;
                pool[1].Y = pool[3].Y = num2;
                pool[2].X = pool[3].X = num;
                pool[0].Color = pool[1].Color = pool[2].Color = pool[3].Color = Renderer.GetQuadColor(color);
                if (!this.m_Flip)
                {
                    pool[0].Tu = pool[1].Tu = this.m_MaxTU;
                    pool[0].Tv = pool[2].Tv = this.m_MaxTV;
                    pool[3].Tu = pool[3].Tv = pool[2].Tu = pool[1].Tv = 0f;
                }
                else
                {
                    pool[0].Tv = pool[2].Tv = this.m_MaxTV;
                    pool[2].Tu = pool[3].Tu = this.m_MaxTU;
                }
                Renderer.SetTexture(this);
                Renderer.DrawQuadPrecalc(pool);
                return true;
            }
            return false;
        }

        public void Draw(int X, int Y, int Width, int Height)
        {
            this.Draw(X, Y, Width, Height, 0xffffff);
        }

        public void Draw(int xScreen, int yScreen, int xWidth, int yHeight, int vColor)
        {
            if (((this.m_Surface != null) && (xWidth > 0)) && (yHeight > 0))
            {
                int num7;
                int num8;
                int num9;
                int num10;
                CustomVertex.TransformedColoredTextured[] poolXYWH = m_PoolXYWH;
                poolXYWH[0].Color = poolXYWH[1].Color = poolXYWH[2].Color = poolXYWH[3].Color = Renderer.GetQuadColor(vColor);
                float num = -0.5f + xScreen;
                float num2 = -0.5f + yScreen;
                int num3 = xWidth / this.Width;
                int num4 = yHeight / this.Height;
                int num5 = xWidth % this.Width;
                int num6 = yHeight % this.Height;
                int screenWidth = Engine.ScreenWidth;
                int screenHeight = Engine.ScreenHeight;
                float num13 = (float)((((double)num5) / ((double)this.Width)) * this.m_MaxTU);
                float num14 = (float)((((double)num6) / ((double)this.Height)) * this.m_MaxTV);
                Renderer.SetTexture(this);
                if ((num3 > 0) && (num4 > 0))
                {
                    num7 = xScreen;
                    num8 = xScreen + this.Width;
                    poolXYWH[0].X = poolXYWH[1].X = num + this.m_fWidth;
                    poolXYWH[2].X = poolXYWH[3].X = num;
                    poolXYWH[0].Tu = poolXYWH[1].Tu = this.m_MaxTU;
                    poolXYWH[0].Tv = poolXYWH[2].Tv = this.m_MaxTV;
                    int num15 = 0;
                    while (num15 < num3)
                    {
                        poolXYWH[0].Y = poolXYWH[2].Y = num2 + this.m_fHeight;
                        poolXYWH[1].Y = poolXYWH[3].Y = num2;
                        num9 = yScreen;
                        num10 = yScreen + this.Height;
                        int num16 = 0;
                        while (num16 < num4)
                        {
                            if (((num8 > 0) && (num7 <= screenWidth)) && ((num10 > 0) && (num9 <= screenHeight)))
                            {
                                Renderer.DrawQuadPrecalc(poolXYWH);
                            }
                            num16++;
                            poolXYWH[0].Y += this.m_fHeight;
                            poolXYWH[1].Y += this.m_fHeight;
                            poolXYWH[2].Y += this.m_fHeight;
                            poolXYWH[3].Y += this.m_fHeight;
                            num9 += this.Height;
                            num10 += this.Height;
                        }
                        num15++;
                        poolXYWH[0].X += this.m_fWidth;
                        poolXYWH[1].X += this.m_fWidth;
                        poolXYWH[2].X += this.m_fWidth;
                        poolXYWH[3].X += this.m_fWidth;
                        num7 += this.Width;
                        num8 += this.Width;
                    }
                }
                if ((num3 > 0) && (num6 > 0))
                {
                    num7 = xScreen;
                    num8 = xScreen + this.Width;
                    num9 = yScreen + (num4 * this.Height);
                    num10 = num9 + num6;
                    poolXYWH[0].X = poolXYWH[1].X = num + this.m_fWidth;
                    poolXYWH[0].Y = poolXYWH[2].Y = -0.5f + num10;
                    poolXYWH[1].Y = poolXYWH[3].Y = -0.5f + num9;
                    poolXYWH[2].X = poolXYWH[3].X = num;
                    poolXYWH[0].Tu = poolXYWH[1].Tu = this.m_MaxTU;
                    poolXYWH[0].Tv = poolXYWH[2].Tv = num14;
                    int num17 = 0;
                    while (num17 < num3)
                    {
                        if (((num8 > 0) && (num7 <= screenWidth)) && ((num10 > 0) && (num9 <= screenHeight)))
                        {
                            Renderer.DrawQuadPrecalc(poolXYWH);
                        }
                        num17++;
                        poolXYWH[0].X += this.m_fWidth;
                        poolXYWH[1].X += this.m_fWidth;
                        poolXYWH[2].X += this.m_fWidth;
                        poolXYWH[3].X += this.m_fWidth;
                        num7 += this.Width;
                        num8 += this.Width;
                    }
                }
                if ((num4 > 0) && (num5 > 0))
                {
                    num7 = xScreen + (num3 * this.Width);
                    num8 = num7 + num5;
                    num9 = yScreen;
                    num10 = yScreen + this.Height;
                    poolXYWH[0].X = poolXYWH[1].X = -0.5f + num8;
                    poolXYWH[0].Y = poolXYWH[2].Y = num2 + this.m_fHeight;
                    poolXYWH[1].Y = poolXYWH[3].Y = num2;
                    poolXYWH[2].X = poolXYWH[3].X = -0.5f + num7;
                    poolXYWH[0].Tu = poolXYWH[1].Tu = num13;
                    poolXYWH[0].Tv = poolXYWH[2].Tv = this.m_MaxTV;
                    int num18 = 0;
                    while (num18 < num4)
                    {
                        if (((num8 > 0) && (num7 <= screenWidth)) && ((num10 > 0) && (num9 <= screenHeight)))
                        {
                            Renderer.DrawQuadPrecalc(poolXYWH);
                        }
                        num18++;
                        poolXYWH[0].Y += this.m_fHeight;
                        poolXYWH[1].Y += this.m_fHeight;
                        poolXYWH[2].Y += this.m_fHeight;
                        poolXYWH[3].Y += this.m_fHeight;
                        num9 += this.Height;
                        num10 += this.Height;
                    }
                }
                if ((num5 > 0) && (num6 > 0))
                {
                    num7 = xScreen + (num3 * this.Width);
                    num8 = num7 + num5;
                    num9 = yScreen + (num4 * this.Height);
                    num10 = num9 + num6;
                    if (((num8 > 0) && (num7 <= screenWidth)) && ((num10 > 0) && (num9 <= screenHeight)))
                    {
                        poolXYWH[0].X = poolXYWH[1].X = -0.5f + num8;
                        poolXYWH[0].Y = poolXYWH[2].Y = -0.5f + num10;
                        poolXYWH[1].Y = poolXYWH[3].Y = -0.5f + num9;
                        poolXYWH[2].X = poolXYWH[3].X = -0.5f + num7;
                        poolXYWH[0].Tu = poolXYWH[1].Tu = num13;
                        poolXYWH[0].Tv = poolXYWH[2].Tv = num14;
                        Renderer.DrawQuadPrecalc(poolXYWH);
                    }
                }
            }
        }

        public void Draw(int X, int Y, int Width, int Height, float tltu, float tltv, float trtu, float trtv, float brtu, float brtv, float bltu, float bltv)
        {
            if (this.m_Surface != null)
            {
                CustomVertex.TransformedColoredTextured[] v = new CustomVertex.TransformedColoredTextured[4];
                float num = -0.5f + X;
                float num2 = -0.5f + Y;
                float num3 = Width;
                float num4 = Height;
                v[3].X = num;
                v[3].Y = num2;
                v[1].X = num + num3;
                v[1].Y = num2;
                v[0].X = num + num3;
                v[0].Y = num2 + num4;
                v[2].X = num;
                v[2].Y = num2 + num4;
                v[0].Color = v[1].Color = v[2].Color = v[3].Color = Renderer.GetQuadColor(0xffffff);
                v[0].Rhw = v[1].Rhw = v[2].Rhw = v[3].Rhw = 1f;
                v[3].Tu = tltu;
                v[3].Tv = tltv;
                v[1].Tu = trtu;
                v[1].Tv = trtv;
                v[0].Tu = brtu;
                v[0].Tv = brtv;
                v[2].Tu = bltu;
                v[2].Tv = bltv;
                Renderer.SetTexture(this);
                Renderer.DrawQuadPrecalc(v);
            }
        }

        public void DrawClipped(int X, int Y, Clipper Clipper)
        {
            if (Clipper == null)
            {
                this.Draw(X, Y, m_BadClipperPool);
            }
            else if (this.m_Surface != null)
            {
                CustomVertex.TransformedColoredTextured[] poolClipped = m_PoolClipped;
                if (Clipper.Clip(X, Y, this.Width, this.Height, poolClipped))
                {
                    poolClipped[0].Color = poolClipped[1].Color = poolClipped[2].Color = poolClipped[3].Color = Renderer.GetQuadColor(0xffffff);
                    if (this.m_Flip)
                    {
                        poolClipped[3].Tu = poolClipped[2].Tu = 1f - poolClipped[3].Tu;
                        poolClipped[1].Tu = poolClipped[0].Tu = 1f - poolClipped[1].Tu;
                    }
                    poolClipped[0].Tu *= this.m_MaxTU;
                    poolClipped[1].Tu *= this.m_MaxTU;
                    poolClipped[2].Tu *= this.m_MaxTU;
                    poolClipped[3].Tu *= this.m_MaxTU;
                    poolClipped[0].Tv *= this.m_MaxTV;
                    poolClipped[1].Tv *= this.m_MaxTV;
                    poolClipped[2].Tv *= this.m_MaxTV;
                    poolClipped[3].Tv *= this.m_MaxTV;
                    Renderer.SetTexture(this);
                    Renderer.DrawQuadPrecalc(poolClipped);
                }
            }
        }

        public unsafe void DrawGame(int X, int Y)
        {
            if (this.m_Surface != null)
            {
                if (((this.m_xDrawXY == X) && (this.m_yDrawXY == Y)) && (this.m_fDrawXY == this.m_Flip))
                {
                    if (this.m_bDrawXY)
                    {
                        fixed (CustomVertex.TransformedColoredTextured* texturedRef = this.m_vDrawXY)
                        {
                            int quadColor = Renderer.GetQuadColor(0xffffff);
                            texturedRef->Color = quadColor;
                            texturedRef[1].Color = quadColor;
                            texturedRef[2].Color = quadColor;
                            texturedRef[3].Color = quadColor;
                            Renderer.SetTexture(this);
                            Renderer.DrawQuadPrecalc(texturedRef);
                        }
                    }
                }
                else
                {
                    this.m_xDrawXY = X;
                    this.m_yDrawXY = Y;
                    this.m_fDrawXY = this.m_Flip;
                    this.m_bDrawXY = (((Y < (Engine.GameY + Engine.GameHeight)) && ((Y + this.Height) > Engine.GameY)) && (X < (Engine.GameX + Engine.GameWidth))) && ((X + this.Width) > Engine.GameX);
                    if (this.m_bDrawXY)
                    {
                        this.m_vDrawXY = new CustomVertex.TransformedColoredTextured[4];
                        fixed (CustomVertex.TransformedColoredTextured* texturedRef2 = this.m_vDrawXY)
                        {
                            float num2 = -0.5f + X;
                            float num3 = -0.5f + Y;
                            texturedRef2->X = texturedRef2[1].X = num2 + this.m_fWidth;
                            texturedRef2->Y = texturedRef2[2].Y = num3 + this.m_fHeight;
                            texturedRef2[1].Y = texturedRef2[3].Y = num3;
                            texturedRef2[2].X = texturedRef2[3].X = num2;
                            int num4 = Renderer.GetQuadColor(0xffffff);
                            texturedRef2->Color = num4;
                            texturedRef2[1].Color = num4;
                            texturedRef2[2].Color = num4;
                            texturedRef2[3].Color = num4;
                            texturedRef2->Rhw = 1f;
                            texturedRef2[1].Rhw = 1f;
                            texturedRef2[2].Rhw = 1f;
                            texturedRef2[3].Rhw = 1f;
                            float maxTU = this.m_MaxTU;
                            float maxTV = this.m_MaxTV;
                            if (!this.m_Flip)
                            {
                                texturedRef2->Tv = maxTV;
                                texturedRef2->Tu = maxTU;
                                texturedRef2[1].Tu = maxTU;
                                texturedRef2[2].Tv = maxTV;
                            }
                            else
                            {
                                texturedRef2->Tv = this.m_MaxTV;
                                texturedRef2[2].Tv = this.m_MaxTV;
                                texturedRef2[2].Tu = maxTU;
                                texturedRef2[3].Tu = maxTU;
                            }
                            Renderer.SetTexture(this);
                            Renderer.DrawQuadPrecalc(texturedRef2);
                        }
                    }
                }
            }
        }

        public unsafe bool DrawGame(int x, int y, CustomVertex.TransformedColoredTextured* pVertex, bool tr = true)
        {
            if (this.m_Surface == null)
            {
                return false;
            }
            if ((((y >= (Engine.GameY + Engine.GameHeight)) || ((y + this.Height) <= Engine.GameY)) || (x >= (Engine.GameX + Engine.GameWidth))) || ((x + this.Width) <= Engine.GameX))
            {
                return false;
            }
            float num = -0.5f + x;
            float num2 = -0.5f + y;
            pVertex->X = pVertex[1].X = num + this.m_fWidth;
            pVertex->Y = pVertex[2].Y = num2 + this.m_fHeight;
            pVertex[1].Y = pVertex[3].Y = num2;
            pVertex[2].X = pVertex[3].X = num;
            int quadColor = Renderer.GetQuadColor(0xffffff);
            pVertex->Color = quadColor;
            pVertex[1].Color = quadColor;
            pVertex[2].Color = quadColor;
            pVertex[3].Color = quadColor;
            float maxTU = this.m_MaxTU;
            float maxTV = this.m_MaxTV;
            if (!this.m_Flip)
            {
                pVertex->Tu = maxTU;
                pVertex->Tv = maxTV;
                pVertex[1].Tu = maxTU;
                pVertex[2].Tv = maxTV;
                pVertex[1].Tv = 0f;
                pVertex[2].Tu = 0f;
                pVertex[3].Tu = 0f;
                pVertex[3].Tv = 0f;
            }
            else
            {
                pVertex->Tv = maxTV;
                pVertex[2].Tv = maxTV;
                pVertex[2].Tu = maxTU;
                pVertex[3].Tu = maxTU;
                pVertex->Tu = 0f;
                pVertex[1].Tu = 0f;
                pVertex[1].Tv = 0f;
                pVertex[3].Tv = 0f;
            }
            Renderer.SetTexture(this);
            Renderer.DrawQuadPrecalc(pVertex);
            return true;
        }

        public bool DrawGame(int x, int y, CustomVertex.TransformedColoredTextured[] pool)
        {
            if (this.m_Surface == null)
            {
                return false;
            }
            if ((((y >= (Engine.GameY + Engine.GameHeight)) || ((y + this.Height) <= Engine.GameY)) || (x >= (Engine.GameX + Engine.GameWidth))) || ((x + this.Width) <= Engine.GameX))
            {
                return false;
            }
            float num = -0.5f + x;
            float num2 = -0.5f + y;
            pool[0].X = pool[1].X = num + this.m_fWidth;
            pool[0].Y = pool[2].Y = num2 + this.m_fHeight;
            pool[1].Y = pool[3].Y = num2;
            pool[2].X = pool[3].X = num;
            pool[0].Color = pool[1].Color = pool[2].Color = pool[3].Color = Renderer.GetQuadColor(0xffffff);
            if (!this.m_Flip)
            {
                pool[0].Tu = pool[1].Tu = this.m_MaxTU;
                pool[0].Tv = pool[2].Tv = this.m_MaxTV;
                pool[3].Tu = pool[3].Tv = pool[2].Tu = pool[1].Tv = 0f;
            }
            else
            {
                pool[0].Tv = pool[2].Tv = this.m_MaxTV;
                pool[2].Tu = pool[3].Tu = this.m_MaxTU;
                pool[0].Tu = 0f;
                pool[1].Tu = 0f;
                pool[1].Tv = 0f;
                pool[3].Tv = 0f;
            }
            Renderer.SetTexture(this);
            Renderer.DrawQuadPrecalc(pool);
            return true;
        }

        public void DrawGame(int X, int Y, int Color)
        {
            if (this.m_Surface != null)
            {
                if (((this.m_xDrawXY == X) && (this.m_yDrawXY == Y)) && (this.m_fDrawXY == this.m_Flip))
                {
                    if (this.m_bDrawXY)
                    {
                        this.m_vDrawXY[0].Color = this.m_vDrawXY[1].Color = this.m_vDrawXY[2].Color = this.m_vDrawXY[3].Color = Renderer.GetQuadColor(Color);
                        Renderer.SetTexture(this);
                        Renderer.DrawQuadPrecalc(this.m_vDrawXY);
                    }
                }
                else
                {
                    this.m_xDrawXY = X;
                    this.m_yDrawXY = Y;
                    this.m_fDrawXY = this.m_Flip;
                    this.m_bDrawXY = (((Y < (Engine.GameY + Engine.GameHeight)) && ((Y + this.Height) > Engine.GameY)) && (X < (Engine.GameX + Engine.GameWidth))) && ((X + this.Width) > Engine.GameX);
                    if (this.m_bDrawXY)
                    {
                        this.m_vDrawXY = new CustomVertex.TransformedColoredTextured[4];
                        float num = -0.5f + X;
                        float num2 = -0.5f + Y;
                        this.m_vDrawXY[0].X = this.m_vDrawXY[1].X = num + this.m_fWidth;
                        this.m_vDrawXY[0].Y = this.m_vDrawXY[2].Y = num2 + this.m_fHeight;
                        this.m_vDrawXY[1].Y = this.m_vDrawXY[3].Y = num2;
                        this.m_vDrawXY[2].X = this.m_vDrawXY[3].X = num;
                        this.m_vDrawXY[0].Color = this.m_vDrawXY[1].Color = this.m_vDrawXY[2].Color = this.m_vDrawXY[3].Color = Renderer.GetQuadColor(Color);
                        this.m_vDrawXY[0].Rhw = this.m_vDrawXY[1].Rhw = this.m_vDrawXY[2].Rhw = this.m_vDrawXY[3].Rhw = 1f;
                        if (!this.m_Flip)
                        {
                            this.m_vDrawXY[0].Tu = this.m_vDrawXY[1].Tu = this.m_MaxTU;
                            this.m_vDrawXY[0].Tv = this.m_vDrawXY[2].Tv = this.m_MaxTV;
                        }
                        else
                        {
                            this.m_vDrawXY[0].Tv = this.m_vDrawXY[2].Tv = this.m_MaxTV;
                            this.m_vDrawXY[2].Tu = this.m_vDrawXY[3].Tu = this.m_MaxTU;
                        }
                        Renderer.SetTexture(this);
                        Renderer.DrawQuadPrecalc(this.m_vDrawXY);
                    }
                }
            }
        }

        public bool DrawGame(int x, int y, int color, CustomVertex.TransformedColoredTextured[] pool)
        {
            if ((((this.m_Surface != null) && (y < (Engine.GameY + Engine.GameHeight))) && (((y + this.Height) > Engine.GameY) && (x < (Engine.GameX + Engine.GameWidth)))) && ((x + this.Width) > Engine.GameX))
            {
                float num = -0.5f + x;
                float num2 = -0.5f + y;
                pool[0].X = pool[1].X = num + this.m_fWidth;
                pool[0].Y = pool[2].Y = num2 + this.m_fHeight;
                pool[1].Y = pool[3].Y = num2;
                pool[2].X = pool[3].X = num;
                pool[0].Color = pool[1].Color = pool[2].Color = pool[3].Color = Renderer.GetQuadColor(color);
                if (!this.m_Flip)
                {
                    pool[0].Tu = pool[1].Tu = this.m_MaxTU;
                    pool[0].Tv = pool[2].Tv = this.m_MaxTV;
                    pool[3].Tu = pool[3].Tv = pool[2].Tu = pool[1].Tv = 0f;
                }
                else
                {
                    pool[0].Tv = pool[2].Tv = this.m_MaxTV;
                    pool[2].Tu = pool[3].Tu = this.m_MaxTU;
                }
                Renderer.SetTexture(this);
                Renderer.DrawQuadPrecalc(pool);
                return true;
            }
            return false;
        }

        public void DrawRotated(int X, int Y, double Angle)
        {
            this.DrawRotated(X, Y, Angle, 0xffffff);
        }

        public void DrawRotated(int X, int Y, double Angle, int Color)
        {
            if (this.m_Surface != null)
            {
                CustomVertex.TransformedColoredTextured[] poolRotated = m_PoolRotated;
                if (poolRotated == null)
                {
                    poolRotated = m_PoolRotated = new CustomVertex.TransformedColoredTextured[4];
                }
                double num = -0.5f + X;
                double num2 = -0.5f + Y;
                double width = this.Width;
                double height = this.Height;
                double x = 0.0;
                double y = 0.0;
                double num7 = width / 2.0;
                double num8 = height / 2.0;
                double num9 = 0.0;
                double num10 = 0.0;
                x = 0.0 - num7;
                y = 0.0 - num8;
                num9 = Math.Atan2(y, x);
                num10 = Math.Sqrt((x * x) + (y * y));
                poolRotated[3].X = (float)(num + (num10 * Math.Cos(Angle + num9)));
                poolRotated[3].Y = (float)(num2 + (num10 * Math.Sin(Angle + num9)));
                x = width - num7;
                y = 0.0 - num8;
                num9 = Math.Atan2(y, x);
                num10 = Math.Sqrt((x * x) + (y * y));
                poolRotated[1].X = (float)(num + (num10 * Math.Cos(Angle + num9)));
                poolRotated[1].Y = (float)(num2 + (num10 * Math.Sin(Angle + num9)));
                x = width - num7;
                y = height - num8;
                num9 = Math.Atan2(y, x);
                num10 = Math.Sqrt((x * x) + (y * y));
                poolRotated[0].X = (float)(num + (num10 * Math.Cos(Angle + num9)));
                poolRotated[0].Y = (float)(num2 + (num10 * Math.Sin(Angle + num9)));
                x = 0.0 - num7;
                y = height - num8;
                num9 = Math.Atan2(y, x);
                num10 = Math.Sqrt((x * x) + (y * y));
                poolRotated[2].X = (float)(num + (num10 * Math.Cos(Angle + num9)));
                poolRotated[2].Y = (float)(num2 + (num10 * Math.Sin(Angle + num9)));
                poolRotated[0].Color = poolRotated[1].Color = poolRotated[2].Color = poolRotated[3].Color = Renderer.GetQuadColor(Color);
                poolRotated[0].Rhw = poolRotated[1].Rhw = poolRotated[2].Rhw = poolRotated[3].Rhw = 1f;
                if (!this.m_Flip)
                {
                    poolRotated[0].Tu = poolRotated[1].Tu = this.m_MaxTU;
                    poolRotated[0].Tv = poolRotated[2].Tv = this.m_MaxTV;
                }
                else
                {
                    poolRotated[2].Tu = poolRotated[3].Tu = this.m_MaxTU;
                    poolRotated[0].Tv = poolRotated[2].Tv = this.m_MaxTV;
                }
                Renderer.SetTexture(this);
                Renderer.DrawQuadPrecalc(poolRotated);
            }
        }

        public static unsafe void FillPixels(void* pvDest, int Color, int Pixels)
        {
            int num = Pixels >> 1;
            int* numPtr = (int*)pvDest;
            int num2 = ((Color << 0x10) | Color) | -2147450880;
            while (--num >= 0)
            {
                numPtr++;
                numPtr[0] = num2;
            }
            if ((Pixels & 1) != 0)
            {
                *((short*)numPtr) = (short)(Color | 0x8000);
            }
        }

        ~Texture()
        {
            this.Dispose();
        }

        public TextureVB GetVB(bool alphaEnable, bool lineStrip, bool alphaTest, int alpha)
        {
            if (this.m_VBs == null)
            {
                this.m_VBs = new TextureVB[8];
            }
            int index = 0;
            if (alphaTest)
            {
                index |= 1;
            }
            if (lineStrip)
            {
                index |= 2;
            }
            if (alphaEnable)
            {
                index |= 4;
            }
            if (index > this.m_VBs.Length)
            {
                TextureVB[] vBs = this.m_VBs;
                this.m_VBs = new TextureVB[0x800];
                for (int i = 0; i < vBs.Length; i++)
                {
                    this.m_VBs[i] = vBs[i];
                }
            }
            TextureVB evb = this.m_VBs[index];
            if (evb != null)
            {
                return evb;
            }
            return (this.m_VBs[index] = new TextureVB());
        }

        public virtual unsafe bool HitTest(int x, int y)
        {
            if (this.CoreGetSurface() == null)
            {
                return false;
            }
            if (x < 0)
            {
                x = (this.Width - 1) - (-x % this.Width);
            }
            else
            {
                x = x % this.Width;
            }
            if (y < 0)
            {
                y = (this.Height - 1) - (-y % this.Height);
            }
            else
            {
                y = y % this.Height;
            }
            if (((x < this.xMin) || (x > this.xMax)) || ((y < this.yMin) || (y > this.yMax)))
            {
                return false;
            }
            if (this.m_FourBPP)
            {
                return true;
            }
            LockData data = this.Lock(Client.LockFlags.ReadOnly);
            //bool flag = ((((int)data.pvSrc) + (y * data.Pitch))[x << 1] & 0x8000) != 0;
            bool flag = (((((int)data.pvSrc) + (y * data.Pitch)) & 0x8000) != 0);
            this.Unlock();
            return flag;
        }

        public bool IsEmpty()
        {
            return (this.m_Surface == null);
        }

        public virtual unsafe LockData Lock(Client.LockFlags flags)
        {
            Microsoft.DirectX.Direct3D.Texture texture = this.CoreGetSurface();
            if (texture == null)
            {
                return new LockData();
            }
            Microsoft.DirectX.Direct3D.LockFlags noSystemLock = Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock;
            if (flags == Client.LockFlags.ReadOnly)
            {
                noSystemLock |= Microsoft.DirectX.Direct3D.LockFlags.ReadOnly;
            }
            int num = 0;
            GraphicsStream stream = texture.LockRectangle(0, noSystemLock, out num);
            LockData data = new LockData();
            data.Pitch = num;
            data.pvSrc = (void*)stream.InternalData;
            data.Height = this.Height;
            data.Width = this.Width;
            this.m_LockStream = stream;
            return data;
        }

        public static unsafe explicit operator Client.Texture(Bitmap bmp)
        {
            int width = bmp.Width;
            int height = bmp.Height;
            Client.Texture texture = new Client.Texture(width, height, true);
            LockData data = texture.Lock(Client.LockFlags.WriteOnly);
            BitmapData bitmapdata = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
            short* numPtr = (short*)bitmapdata.Scan0.ToPointer();
            short* pvSrc = (short*)data.pvSrc;
            int num3 = (bitmapdata.Stride >> 1) - width;
            int num4 = (data.Pitch >> 1) - width;
            while (--height >= 0)
            {
                int num5 = width;
                while (--num5 >= 0)
                {
                    pvSrc++;
                    numPtr++;
                    pvSrc[0] = numPtr[0];
                }
                numPtr += num3;
                pvSrc += num4;
            }
            bmp.UnlockBits(bitmapdata);
            texture.Unlock();
            return texture;
        }

        public void SetPriority(int newPriority)
        {
        }

        public unsafe Bitmap ToBitmap()
        {
            Bitmap bitmap = new Bitmap(this.Width, this.Height, PixelFormat.Format16bppArgb1555);
            LockData data = this.Lock(Client.LockFlags.ReadOnly);
            BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, this.Width, this.Height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
            for (int i = 0; i < this.Height; i++)
            {
                ushort* numPtr = (ushort*)(((int)data.pvSrc) + (i * data.Pitch));
                ushort* numPtr2 = (ushort*)(bitmapdata.Scan0.ToInt32() + (i * bitmapdata.Stride));
                int num2 = 0;
                while (num2++ < this.Width)
                {
                    numPtr++;
                    ushort num3 = numPtr[0];
                    numPtr2++;
                    numPtr2[0] = num3;
                }
            }
            this.Unlock();
            bitmap.UnlockBits(bitmapdata);
            return bitmap;
        }

        public void Unlock()
        {
            Microsoft.DirectX.Direct3D.Texture texture = this.CoreGetSurface();
            if (texture != null)
            {
                if (this.m_LockStream != null)
                {
                    this.m_LockStream.Close();
                }
                this.m_LockStream = null;
                texture.UnlockRectangle(0);
            }
        }

        public bool bDrawXY
        {
            get
            {
                return this.m_bDrawXY;
            }
        }

        public static bool CanSysMem
        {
            get
            {
                return m_CanSysMem;
            }
            set
            {
                m_CanSysMem = value;
            }
        }

        public static bool CanVidMem
        {
            get
            {
                return m_CanVidMem;
            }
            set
            {
                m_CanVidMem = value;
            }
        }

        public static Client.Texture Empty
        {
            get
            {
                if (m_Empty == null)
                {
                    m_Empty = new Client.Texture();
                }
                return m_Empty;
            }
        }

        public bool Flip
        {
            get
            {
                return this.m_Flip;
            }
            set
            {
                this.m_Flip = value;
            }
        }

        public static int MaxAspect
        {
            get
            {
                return m_MaxAspect;
            }
            set
            {
                m_MaxAspect = value;
            }
        }

        public static int MaxTextureHeight
        {
            get
            {
                return m_MaxTextureHeight;
            }
            set
            {
                m_MaxTextureHeight = value;
            }
        }

        public static int MaxTextureWidth
        {
            get
            {
                return m_MaxTextureWidth;
            }
            set
            {
                m_MaxTextureWidth = value;
            }
        }

        public float MaxTU
        {
            get
            {
                return this.m_MaxTU;
            }
        }

        public float MaxTV
        {
            get
            {
                return this.m_MaxTV;
            }
        }

        public static int MinTextureHeight
        {
            get
            {
                return m_MinTextureHeight;
            }
            set
            {
                m_MinTextureHeight = value;
            }
        }

        public static int MinTextureWidth
        {
            get
            {
                return m_MinTextureWidth;
            }
            set
            {
                m_MinTextureWidth = value;
            }
        }

        public static bool Pow2
        {
            get
            {
                return m_Pow2;
            }
            set
            {
                m_Pow2 = value;
            }
        }

        public static bool Square
        {
            get
            {
                return m_Square;
            }
            set
            {
                m_Square = value;
            }
        }

        public Microsoft.DirectX.Direct3D.Texture Surface
        {
            get
            {
                return this.CoreGetSurface();
            }
        }

        public CustomVertex.TransformedColoredTextured[] vDrawXY
        {
            get
            {
                return this.m_vDrawXY;
            }
        }
    }
}