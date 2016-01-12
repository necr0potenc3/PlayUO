namespace Client
{
    using Microsoft.DirectX.Direct3D;

    public class PaletteTexture : Texture
    {
        public PaletteTexture(int width, int height, bool vidMemory) : base(width, height, vidMemory, Format.P8)
        {
        }
    }
}