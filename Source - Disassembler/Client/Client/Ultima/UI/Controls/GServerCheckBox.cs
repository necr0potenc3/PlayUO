namespace Client
{
    using System.Windows.Forms;

    public class GServerCheckBox : GCheckBox, IRelayedSwitch
    {
        private GServerGump m_Owner;
        private int m_RelayID;

        public GServerCheckBox(GServerGump owner, LayoutEntry le) : base(le[2], le[3], le[4] != 0, le[0], le[1])
        {
            this.m_Owner = owner;
            this.m_RelayID = le[5];
        }

        protected internal override void OnMouseDown(int x, int y, MouseButtons mb)
        {
            this.m_Owner.BringToTop();
            base.OnMouseDown(x, y, mb);
        }

        bool IRelayedSwitch.Active
        {
            get
            {
                return base.Checked;
            }
        }

        int IRelayedSwitch.RelayID
        {
            get
            {
                return this.m_RelayID;
            }
        }
    }
}