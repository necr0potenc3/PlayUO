namespace Client
{
    public class GNewServerMenu : GMenuItem
    {
        public GNewServerMenu() : base("New Server...")
        {
        }

        public override void OnClick()
        {
            Gumps.Desktop.Children.Add(new GNewServer());
        }
    }
}