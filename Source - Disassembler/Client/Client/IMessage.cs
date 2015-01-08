namespace Client
{
    using System;
    using System.Drawing;

    public interface IMessage
    {
        System.Drawing.Rectangle OnBeginRender();

        float Alpha { get; }

        System.Drawing.Rectangle ImageRect { get; }

        System.Drawing.Rectangle Rectangle { get; }

        bool Visible { get; }
    }
}

