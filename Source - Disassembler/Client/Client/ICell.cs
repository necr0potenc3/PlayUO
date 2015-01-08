namespace Client
{
    using System;

    public interface ICell : IDisposable
    {
        Type CellType { get; }

        byte Height { get; }

        sbyte SortZ { get; set; }

        sbyte Z { get; }
    }
}

