namespace Client
{
    public interface IItemGump
    {
        Client.Item Item { get; }

        int xOffset { get; }

        int yBottom { get; }

        int yOffset { get; }
    }
}