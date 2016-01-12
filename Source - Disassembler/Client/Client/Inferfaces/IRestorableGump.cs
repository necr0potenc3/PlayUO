namespace Client
{
    public interface IRestorableGump
    {
        int Extra { get; }

        int Height { get; }

        int Type { get; }

        int Width { get; }

        int X { get; }

        int Y { get; }
    }
}