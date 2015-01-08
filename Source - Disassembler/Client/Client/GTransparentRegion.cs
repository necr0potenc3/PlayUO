namespace Client
{
    using System;

    public class GTransparentRegion : GEmpty
    {
        private GServerGump m_Owner;

        public GTransparentRegion(GServerGump owner, int x, int y, int w, int h) : base(x, y, w, h)
        {
            this.m_Owner = owner;
        }

        protected internal override bool HitTest(int x, int y)
        {
            return false;
        }
    }
}

