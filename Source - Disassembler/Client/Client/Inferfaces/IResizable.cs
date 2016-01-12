namespace Client
{
    public interface IResizable
    {
        int Height { get; set; }

        int MaxHeight { get; }

        int MaxWidth { get; }

        int MinHeight { get; }

        int MinWidth { get; }

        int Width { get; set; }
    }
}