namespace Client
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Entry2D
    {
        public int m_Lookup;
        public int m_Length;
    }
}