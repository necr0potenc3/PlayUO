namespace Client
{
    using Microsoft.DirectX;
    using Microsoft.DirectX.Direct3D;

    public class BufferedVertexStream
    {
        private VertexBuffer m_Buffer;
        private int m_SizePerVertex;
        private GraphicsStream m_Stream;
        private int m_VertexBufferLength;
        private int m_VertexBufferOffset;

        public BufferedVertexStream(VertexBuffer buffer, int vertexBufferLength, int sizePerVertex)
        {
            this.m_Buffer = buffer;
            this.m_VertexBufferLength = vertexBufferLength;
            this.m_SizePerVertex = sizePerVertex;
        }

        public int Push(byte[] buffer, int vertexCount, bool unlock)
        {
            int vertexBufferOffset;
            if (this.m_VertexBufferLength >= (this.m_VertexBufferOffset + vertexCount))
            {
                if (this.m_Stream == null)
                {
                    if (unlock)
                    {
                        this.m_Stream = this.m_Buffer.Lock(this.m_VertexBufferOffset * this.m_SizePerVertex, vertexCount * this.m_SizePerVertex, Microsoft.DirectX.Direct3D.LockFlags.NoOverwrite);
                    }
                    else
                    {
                        this.m_Stream = this.m_Buffer.Lock(this.m_VertexBufferOffset * this.m_SizePerVertex, (this.m_VertexBufferLength - this.m_VertexBufferOffset) * this.m_SizePerVertex, Microsoft.DirectX.Direct3D.LockFlags.NoOverwrite);
                    }
                }
                this.m_Stream.Write(buffer, 0, vertexCount * this.m_SizePerVertex);
                vertexBufferOffset = this.m_VertexBufferOffset;
                this.m_VertexBufferOffset += vertexCount;
                if (unlock)
                {
                    this.Unlock();
                }
                return vertexBufferOffset;
            }
            if (vertexCount < this.m_VertexBufferLength)
            {
                this.Unlock();
                if (this.m_Stream == null)
                {
                    if (unlock)
                    {
                        this.m_Stream = this.m_Buffer.Lock(0, vertexCount * this.m_SizePerVertex, Microsoft.DirectX.Direct3D.LockFlags.Discard);
                    }
                    else
                    {
                        this.m_Stream = this.m_Buffer.Lock(0, this.m_VertexBufferLength * this.m_SizePerVertex, Microsoft.DirectX.Direct3D.LockFlags.Discard);
                    }
                }
                this.m_Stream.Write(buffer, 0, vertexCount * this.m_SizePerVertex);
                vertexBufferOffset = 0;
                this.m_VertexBufferOffset = vertexCount;
                if (unlock)
                {
                    this.Unlock();
                }
                return vertexBufferOffset;
            }
            return -1;
        }

        public void Unlock()
        {
            if (this.m_Stream != null)
            {
                try
                {
                    this.m_Stream.Close();
                    this.m_Buffer.Unlock();
                }
                catch
                {
                }
                this.m_Stream = null;
            }
        }
    }
}