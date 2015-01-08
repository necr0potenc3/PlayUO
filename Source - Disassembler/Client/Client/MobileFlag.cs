namespace Client
{
    using System;

    [Flags]
    public enum MobileFlag
    {
        FactionShop = 0x10,
        Female = 2,
        Hidden = 0x80,
        InvalidMask = -223,
        None = 0,
        Poisoned = 4,
        ValidMask = 0xde,
        Warmode = 0x40,
        YellowHits = 8
    }
}

