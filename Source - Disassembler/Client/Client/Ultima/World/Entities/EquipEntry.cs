namespace Client
{
    using Microsoft.DirectX.Direct3D;

    public class EquipEntry : IEntity
    {
        public short m_Animation;
        public Item m_Item;
        public Layer m_Layer;
        private static CustomVertex.TransformedColoredTextured[] m_vPool = VertexConstructor.Create();

        public EquipEntry(Item item, short animation, Layer layer)
        {
            this.m_Item = item;
            this.m_Animation = animation;
            this.m_Layer = layer;
        }

        public unsafe void Draw(Client.Texture t, int x, int y)
        {
            fixed (CustomVertex.TransformedColoredTextured* texturedRef = m_vPool)
            {
                texturedRef->Tu = 0f;
                texturedRef->Tv = 0f;
                texturedRef[1].Tu = 0f;
                texturedRef[1].Tv = 0f;
                texturedRef[2].Tu = 0f;
                texturedRef[2].Tv = 0f;
                texturedRef[3].Tu = 0f;
                texturedRef[3].Tv = 0f;
                t.Draw(x, y, texturedRef);
            }
        }

        public unsafe void DrawGame(Client.Texture t, int x, int y)
        {
            fixed (CustomVertex.TransformedColoredTextured* texturedRef = m_vPool)
            {
                texturedRef->Tu = 0f;
                texturedRef->Tv = 0f;
                texturedRef[1].Tu = 0f;
                texturedRef[1].Tv = 0f;
                texturedRef[2].Tu = 0f;
                texturedRef[2].Tv = 0f;
                texturedRef[3].Tu = 0f;
                texturedRef[3].Tv = 0f;
                t.DrawGame(x, y, texturedRef);
            }
        }

        int IEntity.Serial
        {
            get
            {
                return this.m_Item.Serial;
            }
        }
    }
}