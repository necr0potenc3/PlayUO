namespace Client
{
    using System;

    public interface IFontFactory
    {
        Texture CreateInstance(string Key, IHue Hue);

        string Name { get; }
    }
}

