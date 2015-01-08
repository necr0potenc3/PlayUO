namespace Client
{
    using System;
    using System.Collections;

    public interface IFont
    {
        Texture GetString(string String, IHue Hue);
        int GetStringWidth(string String);

        Hashtable WrapCache { get; }
    }
}

