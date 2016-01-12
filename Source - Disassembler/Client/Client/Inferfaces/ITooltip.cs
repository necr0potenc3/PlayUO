namespace Client
{
    public interface ITooltip
    {
        Gump GetGump();

        float Delay { get; set; }
    }
}