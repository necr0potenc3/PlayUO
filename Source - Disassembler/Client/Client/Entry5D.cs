namespace Client
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Entry5D
    {
        public int m_FileID;
        public int m_BlockID;
        public int m_Lookup;
        public int m_Length;
        public int m_Extra;
    }
}

