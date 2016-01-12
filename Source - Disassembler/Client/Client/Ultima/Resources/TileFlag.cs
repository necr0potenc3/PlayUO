namespace Client
{
    using System;

    [Flags]
    public enum TileFlag
    {
        Animation = 0x1000000,
        Armor = 0x8000000,
        ArticleA = 0x4000,
        ArticleAn = 0x8000,
        Background = 1,
        Bridge = 0x400,
        Container = 0x200000,
        Damaging = 0x20,
        Door = 0x20000000,
        Foliage = 0x20000,
        Generic = 0x800,
        Impassable = 0x40,
        Internal = 0x10000,
        LightSource = 0x800000,
        Map = 0x100000,
        NoDiagonal = 0x2000000,
        NoShoot = 0x2000,
        PartialHue = 0x40000,
        Roof = 0x10000000,
        StairBack = 0x40000000,
        StairRight = -2147483648,
        Surface = 0x200,
        Translucent = 8,
        Transparent = 4,
        Unknown1 = 0x100,
        Unknown2 = 0x80000,
        Unknown3 = 0x4000000,
        Wall = 0x10,
        Weapon = 2,
        Wearable = 0x400000,
        Wet = 0x80,
        Window = 0x1000
    }
}