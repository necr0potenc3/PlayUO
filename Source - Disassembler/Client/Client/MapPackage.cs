namespace Client
{
    using System.Collections;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct MapPackage
    {
        public LandTile[,] landTiles;
        public ArrayList[,] cells;
        public byte[,] flags;
        public int[,] colorMap;
        public int[,] realColors;
        public int[,] frameColors;
        public int CellX;
        public int CellY;
    }
}