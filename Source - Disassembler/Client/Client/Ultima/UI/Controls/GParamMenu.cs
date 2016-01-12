namespace Client
{
    public class GParamMenu : GMenuItem
    {
        private Client.Action m_Action;
        private ActionHandler m_Handler;
        private ParamNode m_Param;

        public GParamMenu(ParamNode param, ActionHandler handler, Client.Action action) : base(param.Name)
        {
            this.m_Param = param;
            this.m_Handler = handler;
            this.m_Action = action;
            if (this.m_Action == null)
            {
                base.Tooltip = new Tooltip(string.Format("Click here to add the instruction:\n{0} {1}", handler.Name, param.Name), true);
            }
            else
            {
                base.Tooltip = new Tooltip("Click here to change the parameter", true);
            }
            base.Tooltip.Delay = 3f;
        }

        public override void OnClick()
        {
            string param = this.m_Param.Param;
            if (param != null)
            {
                Client.Action action = this.m_Action;
                if (action == null)
                {
                    Gump parent = base.m_Parent;
                    while ((parent != null) && !(parent is GMacroEditorPanel))
                    {
                        parent = parent.Parent;
                    }
                    if (parent is GMacroEditorPanel)
                    {
                        Macro macro = ((GMacroEditorPanel)parent).Macro;
                        macro.AddAction(new Client.Action(string.Format("{0} {1}", this.m_Handler.Action, param)));
                        ((GMacroEditorForm)parent.Parent.Parent).Current = macro;
                    }
                }
                else
                {
                    action.Param = param;
                    GMenuItem item = this;
                    while (item.Parent is GMenuItem)
                    {
                        item = (GMenuItem)item.Parent;
                    }
                    item.Text = this.m_Param.Name;
                }
            }
        }

        public Client.Action Action
        {
            get
            {
                return this.m_Action;
            }
        }

        public ActionHandler Handler
        {
            get
            {
                return this.m_Handler;
            }
        }

        public ParamNode Param
        {
            get
            {
                return this.m_Param;
            }
        }
    }
}