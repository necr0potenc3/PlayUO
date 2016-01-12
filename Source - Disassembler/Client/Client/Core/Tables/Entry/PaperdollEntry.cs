namespace Client
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct PaperdollEntry
    {
        public int BodyID;
        public int GumpID;

        public PaperdollEntry(int BodyID, int GumpID)
        {
            this.BodyID = BodyID;
            this.GumpID = GumpID;
        }
    }
}