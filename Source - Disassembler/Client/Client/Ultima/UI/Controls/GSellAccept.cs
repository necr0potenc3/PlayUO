namespace Client
{
    public class GSellAccept : GRegion
    {
        private GSellGump m_Owner;

        public GSellAccept(GSellGump owner) : base(30, 0xc1, 0x3f, 0x2a)
        {
            this.m_Owner = owner;
        }

        protected internal override void OnSingleClick(int x, int y)
        {
            this.m_Owner.Accept();
        }
    }
}