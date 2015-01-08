namespace Client
{
    using System;

    public interface IMessageOwner : IPoint2D
    {
        void OnDoubleClick();
        void OnSingleClick();
        void OnTarget();

        int MessageFrame { get; set; }

        int MessageX { get; set; }

        int MessageY { get; set; }
    }
}

