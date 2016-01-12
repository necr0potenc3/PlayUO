namespace Client
{
    using System;

    public interface IAnimatedCell : ICell, IDisposable
    {
        void GetPackage(ref int Body, ref int Action, ref int Direction, ref int Frame, ref int Hue);
    }
}