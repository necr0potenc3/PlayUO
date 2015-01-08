namespace Client
{
    using System;

    public interface ITile : ICell, IDisposable
    {
        short ID { get; }
    }
}

