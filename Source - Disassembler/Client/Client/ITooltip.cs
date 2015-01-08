namespace Client
{
    using System;

    public interface ITooltip
    {
        Gump GetGump();

        float Delay { get; set; }
    }
}

