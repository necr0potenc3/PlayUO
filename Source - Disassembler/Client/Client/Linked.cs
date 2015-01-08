namespace Client
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Linked
    {
        public GDragable Gump;
        public int Dock;
        public int TheirDock;
        public Linked(GDragable g, int d, int d2)
        {
            this.Gump = g;
            this.Dock = d;
            this.TheirDock = d2;
        }
    }
}

