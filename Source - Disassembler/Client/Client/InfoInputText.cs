namespace Client
{
    using System;

    public class InfoInputText : InfoInput
    {
        public InfoInputText(string name) : base(name)
        {
        }

        public override Gump CreateGump()
        {
            GEmpty empty = new GEmpty(0, 0, 140, 0x18);
            GLabel toAdd = new GLabel(base.Name + ":", Engine.GetUniFont(2), GumpHues.ControlText, 0, 0);
            empty.Children.Add(toAdd);
            toAdd.Center();
            toAdd.X = 0x41 - toAdd.Image.xMax;
            GAlphaBackground background = new GAlphaBackground(70, 0, 60, 0x18) {
                FillAlpha = 1f,
                FillColor = GumpColors.Window,
                ShouldHitTest = false
            };
            empty.Children.Add(background);
            GTextBox box = new GTextBox(0, false, 70, 0, 60, 0x18, "0", Engine.GetUniFont(2), GumpHues.WindowText, GumpHues.WindowText, GumpHues.WindowText) {
                OnTextChange = new Client.OnTextChange(this.OnTextChange)
            };
            empty.Children.Add(box);
            return empty;
        }

        private void OnTextChange(string text, Gump g)
        {
            base.Active = text;
        }

        public override void UpdateGump(Gump g)
        {
        }
    }
}

