namespace Client
{
    using System;

    public class PaletteTexture : Texture
    {
        public PaletteTexture(int width, int height, bool vidMemory) : base(width, height, vidMemory, 0x29)
        {
        }
    }
}

