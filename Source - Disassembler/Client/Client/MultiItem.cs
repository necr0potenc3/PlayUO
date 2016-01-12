namespace Client
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct MultiItem
    {
        public short ItemID;
        public short X;
        public short Y;
        public short Z;
        public int Flags;
    }
}