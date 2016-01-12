namespace Client
{
    public class GMacroParamEntry : GTextBox
    {
        private Client.Action m_Action;

        public GMacroParamEntry(Client.Action action, string text, int x, int y, int w, int h) : base(0, false, x, y, w, h, text, Engine.GetUniFont(2), GumpHues.WindowText, GumpHues.WindowText, GumpHues.WindowText)
        {
            this.m_Action = action;
            base.OnTextChange = new OnTextChange(this.UpdateParam);
        }

        private void UpdateParam(string str, Gump g)
        {
            this.m_Action.Param = str;
        }
    }
}