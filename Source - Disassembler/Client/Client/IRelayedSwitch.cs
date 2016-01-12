namespace Client
{
    public interface IRelayedSwitch
    {
        bool Active { get; }

        int RelayID { get; }
    }
}