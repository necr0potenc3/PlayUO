namespace Client
{
    using System;

    [Flags]
    public enum MacroModifiers
    {
        All = 1,
        Alt = 4,
        Ctrl = 2,
        None = 0,
        Shift = 8
    }
}