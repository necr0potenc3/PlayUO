namespace Client
{
    using System.Collections;
    using System.Windows.Forms;

    public class GRadioButton : GImage
    {
        protected int[] m_GumpIDs;
        protected Gump m_ParentOverride;
        protected bool m_State;

        public GRadioButton(int inactiveID, int activeID, bool initialState, int x, int y) : base(initialState ? activeID : inactiveID, x, y)
        {
            this.m_GumpIDs = new int[] { inactiveID, activeID };
            this.m_State = initialState;
        }

        protected internal override bool HitTest(int x, int y)
        {
            if (base.m_Invalidated)
            {
                base.Refresh();
            }
            return ((base.m_Draw && ((base.m_Clipper == null) || base.m_Clipper.Evaluate(base.PointToScreen(new Point(x, y))))) && base.m_Image.HitTest(x, y));
        }

        protected internal override void OnMouseUp(int x, int y, MouseButtons mb)
        {
            this.State = true;
        }

        public Gump ParentOverride
        {
            get
            {
                return this.m_ParentOverride;
            }
            set
            {
                this.m_ParentOverride = value;
            }
        }

        public bool State
        {
            get
            {
                return this.m_State;
            }
            set
            {
                if (this.m_State != value)
                {
                    this.m_State = value;
                    base.GumpID = this.m_GumpIDs[value ? 1 : 0];
                    if (value && ((base.m_Parent != null) || (this.m_ParentOverride != null)))
                    {
                        Stack stack = new Stack();
                        stack.Push((this.m_ParentOverride != null) ? this.m_ParentOverride : base.m_Parent);
                        while (stack.Count > 0)
                        {
                            Gump gump = (Gump)stack.Pop();
                            foreach (Gump gump1 in gump.Children.ToArray())
                            {
                                if ((gump1 is GRadioButton) && (gump1 != this))
                                {
                                    GRadioButton button = (GRadioButton)gump1;
                                    if (button.State)
                                    {
                                        button.State = false;
                                    }
                                }
                                if (gump1.Children.Count > 0)
                                {
                                    stack.Push(gump1);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}