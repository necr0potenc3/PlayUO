namespace Client
{
    using System;

    public interface IRelayedSwitch
    {
        bool Active { get; }

        int RelayID { get; }
    }
}

