namespace Client
{
    using System;

    public class GActionMenu : GMenuItem
    {
        private Action m_Action;
        private Macro m_Macro;
        private GMacroEditorPanel m_Panel;

        public GActionMenu(GMacroEditorPanel panel, Macro macro, Action action) : base(action.Handler.Name)
        {
            this.m_Panel = panel;
            this.m_Macro = macro;
            this.m_Action = action;
            base.Tooltip = new Tooltip("Click here to edit this action", true);
        }

        public override void OnClick()
        {
            Gumps.Desktop.Children.Add(new GEditAction(this.m_Panel, this.m_Macro, this.m_Action));
        }
    }
}

