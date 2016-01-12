namespace Client
{
    public class GSellClear : GRegion
    {
        private GSellGump m_Owner;

        public GSellClear(GSellGump owner) : base(0xa9, 0xc7, 0x37, 0x23)
        {
            this.m_Owner = owner;
        }

        protected internal override void OnSingleClick(int x, int y)
        {
            this.m_Owner.Clear();
        }
    }
}