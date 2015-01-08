namespace Client
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct LockData
    {
        public unsafe void* pvSrc;
        public int Pitch;
        public int Height;
        public int Width;
    }
}

