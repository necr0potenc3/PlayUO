namespace Client
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct StaticTile
    {
        public short m_ID;
        public byte m_X;
        public byte m_Y;
        public sbyte m_Z;
        public short m_Hue;
    }
}