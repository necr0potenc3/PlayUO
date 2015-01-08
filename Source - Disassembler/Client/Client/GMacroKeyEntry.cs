namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GMacroKeyEntry : GTextBox
    {
        private bool m_Recording;

        public GMacroKeyEntry(string text, int x, int y, int w, int h) : base(0, false, x, y, w, h, text, Engine.GetUniFont(2), GumpHues.WindowText, GumpHues.WindowText, GumpHues.WindowText)
        {
        }

        public void Finish(Keys key, Keys mods)
        {
            if (this.m_Recording)
            {
                this.m_Recording = false;
                base.String = GMacroEditorPanel.GetKeyName(key);
                GMacroEditorPanel parent = base.m_Parent as GMacroEditorPanel;
                if (parent != null)
                {
                    parent.Macro.Key = key;
                    parent.Macro.Mods = mods;
                    parent.UpdateModifiers();
                    parent.NotifyParent();
                }
            }
        }

        protected internal override bool OnKeyDown(char Key)
        {
            return true;
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            base.OnMouseUp(X, Y, mb);
            if (mb == MouseButtons.Middle)
            {
                this.Start();
                this.Finish(0x11002, Control.ModifierKeys);
            }
        }

        protected internal override void OnMouseWheel(int Delta)
        {
            base.OnMouseWheel(Delta);
            if (Delta > 0)
            {
                this.Start();
                this.Finish(0x11000, Control.ModifierKeys);
            }
            else if (Delta < 0)
            {
                this.Start();
                this.Finish(0x11001, Control.ModifierKeys);
            }
        }

        public void Start()
        {
            if (!this.m_Recording)
            {
                this.m_Recording = true;
            }
        }

        public override bool ShowCaret
        {
            get
            {
                return true;
            }
        }
    }
}

