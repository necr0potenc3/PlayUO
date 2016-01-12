namespace Client
{
    using System.Windows.Forms;

    public class GCheckBox : GImage
    {
        protected bool m_Checked;
        protected int[] m_GumpIDs;

        public GCheckBox(int inactiveID, int activeID, bool initialState, int x, int y) : base(initialState ? activeID : inactiveID, x, y)
        {
            this.m_GumpIDs = new int[] { inactiveID, activeID };
            this.m_Checked = initialState;
        }

        protected internal override bool HitTest(int x, int y)
        {
            if (base.m_Invalidated)
            {
                base.Refresh();
            }
            return (base.m_Draw && base.m_Image.HitTest(x, y));
        }

        protected internal override void OnMouseUp(int x, int y, MouseButtons mb)
        {
            this.Checked = !this.Checked;
        }

        public bool Checked
        {
            get
            {
                return this.m_Checked;
            }
            set
            {
                if (this.m_Checked != value)
                {
                    this.m_Checked = value;
                    base.GumpID = this.m_GumpIDs[value ? 1 : 0];
                }
            }
        }
    }
}