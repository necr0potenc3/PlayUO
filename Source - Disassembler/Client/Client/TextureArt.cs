namespace Client
{
    using System.IO;

    public class TextureArt
    {
        private TexMapFactory m_Factory;
        private int[] m_Lookup = new int[0x1000];
        private Stream m_Stream;

        public unsafe TextureArt()
        {
            Stream stream = Engine.FileManager.OpenMUL(Files.TexIdx);
            byte[] buffer = new byte[0xc000];
            Engine.NativeRead((FileStream)stream, buffer, 0, buffer.Length);
            stream.Close();
            fixed (byte* numRef = buffer)
            {
                int* numPtr = (int*)numRef;
                int index = 0;
                do
                {
                    this.m_Lookup[index] = numPtr[0] | (numPtr[2] << 0x1f);
                    numPtr += 3;
                }
                while (++index < 0x1000);
            }
            this.m_Stream = Engine.FileManager.OpenMUL(Files.TexMul);
        }

        public void Dispose()
        {
            this.m_Lookup = null;
            this.m_Stream.Close();
            this.m_Stream = null;
        }

        public Texture ReadFromDisk(int TextureID, IHue Hue)
        {
            if (this.m_Factory == null)
            {
                this.m_Factory = new TexMapFactory(this);
            }
            return this.m_Factory.Load(TextureID, Hue);
        }

        private class TexMapFactory : TextureFactory
        {
            private byte[] m_Buffer = new byte[0x8000];
            private IHue m_Hue;
            private int m_TextureID;
            private TextureArt m_Textures;

            public TexMapFactory(TextureArt textures)
            {
                this.m_Textures = textures;
            }

            protected override void CoreAssignArgs(Texture tex)
            {
                tex.m_Factory = this;
                tex.m_FactoryArgs = new object[] { this.m_TextureID, this.m_Hue };
            }

            protected override void CoreGetDimensions(out int width, out int height)
            {
                int num = this.m_Textures.m_Lookup[this.m_TextureID];
                if (num < 0)
                {
                    width = height = 0x80;
                }
                else
                {
                    width = height = 0x40;
                }
            }

            protected override bool CoreLookup()
            {
                int num = this.m_Textures.m_Lookup[this.m_TextureID];
                return (num != -1);
            }

            protected override unsafe void CoreProcessImage(int width, int height, int stride, ushort* pLine, ushort* pLineEnd, ushort* pImageEnd, int lineDelta, int lineEndDelta)
            {
                int num = this.m_Textures.m_Lookup[this.m_TextureID];
                if (num != -1)
                {
                    int length = (width * height) * 2;
                    this.m_Textures.m_Stream.Seek((long)(num & 0x7fffffff), SeekOrigin.Begin);
                    Engine.NativeRead((FileStream)this.m_Textures.m_Stream, this.m_Buffer, 0, length);
                    fixed (byte* numRef = this.m_Buffer)
                    {
                        this.m_Hue.CopyPixels((void*)numRef, (void*)pLine, width * height);
                    }
                }
            }

            public Texture Load(int textureID, IHue hue)
            {
                this.m_TextureID = textureID & 0xfff;
                this.m_Hue = hue;
                return base.Construct(false);
            }

            public override Texture Reconstruct(object[] args)
            {
                this.m_TextureID = (int)args[0];
                this.m_Hue = (IHue)args[1];
                return base.Construct(true);
            }
        }
    }
}