namespace Client
{
    using System;

    [Flags]
    public enum ItemFlag
    {
        CanMove = 0x20,
        Hidden = 0x80
    }
}