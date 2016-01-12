namespace Client
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct HueData
    {
        public ushort[] colors;
        public short tableEnd;
        public short tableStart;
    }
}