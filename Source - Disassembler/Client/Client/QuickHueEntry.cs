namespace Client
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct QuickHueEntry
    {
        public string Name;
        public int Hue;
    }
}

