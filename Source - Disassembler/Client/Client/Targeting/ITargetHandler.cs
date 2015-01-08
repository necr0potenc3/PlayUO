namespace Client.Targeting
{
    using System;

    public interface ITargetHandler
    {
        void OnCancel(TargetCancelType why);
        void OnTarget(object targeted);
    }
}

