namespace Client
{
    public enum MobileDirection : byte
    {
        Down = 3,
        East = 2,
        Left = 5,
        Mask = 7,
        North = 0,
        Right = 1,
        Running = 0x80,
        South = 4,
        Up = 7,
        ValueMask = 0x87,
        West = 6
    }
}