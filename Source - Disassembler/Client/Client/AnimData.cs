namespace Client
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct AnimData
    {
        public unsafe sbyte* pvFrames;
        public byte unknown;
        public byte frameCount;
        public byte frameInterval;
        public byte frameStartInterval;
        public sbyte this[int index]
        {
            get
            {
                return this.pvFrames[index & 0x3f];
            }
        }
    }
}

