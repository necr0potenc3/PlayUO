namespace Client
{
    using System;

    public interface IItem : ITile, ICell, IDisposable
    {
        Texture GetItem(IHue hue, short itemID);

        IHue Hue { get; }
    }
}