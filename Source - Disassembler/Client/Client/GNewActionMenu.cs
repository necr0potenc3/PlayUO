namespace Client
{
    using System;
    using System.Collections;

    public class GNewActionMenu : GMenuItem
    {
        private ActionHandler m_Action;
        private Macro m_Macro;
        private GMacroEditorPanel m_Panel;

        public GNewActionMenu(GMacroEditorPanel panel, Macro macro, ActionHandler action) : base(action.Name)
        {
            this.m_Panel = panel;
            this.m_Macro = macro;
            this.m_Action = action;
            if ((this.m_Action.Params != null) && (this.m_Action.Params.Length > 0))
            {
                base.Tooltip = new Tooltip("Choose a parameter from the menu to the right, or just click here to add the instruction with a default parameter.", false, 200);
            }
            else
            {
                base.Tooltip = new Tooltip("Click here to add this instruction.", false, 200);
            }
            base.Tooltip.Delay = 2f;
        }

        private string FindFirst(ParamNode[] nodes)
        {
            string str = null;
            for (int i = 0; ((nodes != null) && (str == null)) && (i < nodes.Length); i++)
            {
                str = this.FindFirst(nodes[i]);
            }
            return str;
        }

        private string FindFirst(ParamNode node)
        {
            if (node.Param != null)
            {
                return node.Param;
            }
            return this.FindFirst(node.Nodes);
        }

        public override void OnClick()
        {
            ArrayList list = new ArrayList(this.m_Macro.Actions);
            list.Add(new Action(string.Format("{0} {1}", this.m_Action.Action, this.FindFirst(this.m_Action.Params))));
            this.m_Macro.Actions = (Action[]) list.ToArray(typeof(Action));
            GMacroEditorForm parent = this.m_Panel.Parent.Parent as GMacroEditorForm;
            if (parent != null)
            {
                parent.Current = parent.Current;
            }
        }
    }
}

