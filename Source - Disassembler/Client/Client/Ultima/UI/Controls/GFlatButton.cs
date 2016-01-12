namespace Client
{
    public class GFlatButton : GAlphaBackground
    {
        protected OnClick m_OnClick;

        public GFlatButton(int X, int Y, int Width, int Height, string Text, OnClick OnClick) : base(X, Y, Width, Height)
        {
            this.m_OnClick = OnClick;
            base.m_CanDrag = false;
            GTextButton toAdd = new GTextButton(Text, Engine.GetUniFont(0), Hues.Default, Hues.Load(0x35), 0, 0, new OnClick(this.Route_OnClick));
            base.m_Children.Add(toAdd);
            toAdd.Center();
            base.m_Children.Add(new GHotspot(0, 0, Width, Height, toAdd));
        }

        public void Click()
        {
            if (this.m_OnClick != null)
            {
                this.m_OnClick(this);
            }
        }

        private void Route_OnClick(Gump g)
        {
            if (this.m_OnClick != null)
            {
                this.m_OnClick(this);
            }
        }
    }
}