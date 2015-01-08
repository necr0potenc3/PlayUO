namespace Client
{
    using System;

    public interface IPoint3D : IPoint2D
    {
        int Z { get; }
    }
}

