namespace Client
{
    using System.Collections;

    public class GHyperLink : GTextButton
    {
        private string m_Url;
        private static Hashtable m_Visited = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
        private static IHue RegularHue = new Hues.HFill(0xff);
        private static IHue VisitedHue = new Hues.HFill(0xff0000);

        public GHyperLink(string url, string text, IFont font, int x, int y) : base(text, font, m_Visited.Contains(url) ? VisitedHue : RegularHue, m_Visited.Contains(url) ? VisitedHue : RegularHue, x, y, null)
        {
            base.Underline = true;
            this.m_Url = url;
            base.OnClick = new OnClick(this.Button_OnClick);
        }

        private void Button_OnClick(Gump g)
        {
            Engine.OpenBrowser(this.m_Url);
            m_Visited[this.m_Url] = true;
            base.DefaultHue = base.FocusHue = VisitedHue;
        }
    }
}