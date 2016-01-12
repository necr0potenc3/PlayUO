namespace Client
{
    using System;
    using System.Runtime.InteropServices;

    public class Memory
    {
        public static unsafe void* Alloc(int Size)
        {
            return (void*)Marshal.AllocHGlobal(Size);
        }

        public static unsafe void Free(void* Data)
        {
            Marshal.FreeHGlobal((IntPtr)Data);
        }
    }
}