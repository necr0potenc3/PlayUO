namespace Client
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Entry3D
    {
        public int m_Lookup;
        public int m_Length;
        public int m_Extra;
    }
}