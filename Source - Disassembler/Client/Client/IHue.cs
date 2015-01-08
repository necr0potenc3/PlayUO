namespace Client
{
    using System;

    public interface IHue
    {
        void Apply(LockData ld);
        void Apply(Texture Target);
        unsafe void CopyPixels(void* pvSrc, void* pvDest, int Pixels);
        void Dispose();
        unsafe void FillLine(ushort* pSrc, ushort* pDest, ushort* pEnd);
        unsafe void FillPixels(void* pvDest, int Color, int Pixels);
        Frames GetAnimation(int RealID);
        Texture GetGump(int GumpID);
        Texture GetItem(int ItemID);
        Texture GetLand(int LandID);
        Texture GetTexture(int TextureID);
        int HueID();
        ushort Pixel(ushort input);
    }
}

