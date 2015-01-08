namespace Client
{
    using System;

    public interface IInterop
    {
        void AddTimer(Timer t);
        object GetMobile(int Serial, string Name);
        int GetTicks();
        void OpenStatus(int Serial, bool Drag);
        void RemoveTimer(Timer t);
        void SetMobile(int Serial, string Name, object Value);
    }
}

