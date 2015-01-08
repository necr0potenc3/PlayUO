namespace Client
{
    using System;

    public class MiddleDeathEffect : Fade
    {
        private GLabel m_Label;

        public MiddleDeathEffect() : base(0, 1f, 1f, 1f)
        {
            this.m_Label = new GLabel("You are dead.", Engine.GetFont(3), Hues.Default, 0, 0);
            this.m_Label.Center();
            Gumps.Desktop.Children.Add(this.m_Label);
        }

        protected internal override void OnFadeComplete()
        {
            Cursor.Visible = true;
            Engine.Effects.Add(new EndDeathEffect(this.m_Label));
        }
    }
}

