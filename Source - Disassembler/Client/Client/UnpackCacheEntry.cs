namespace Client
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct UnpackCacheEntry
    {
        public int m_NextIndex;
        public int m_ByteIndex;
        public int m_ByteCount;
    }
}