namespace Client
{
    using System;

    public class GQuestionBackground : GBackground
    {
        private GQuestionMenuEntry[] m_Entries;
        private int m_xLast;
        private int m_yLast;

        public GQuestionBackground(GQuestionMenuEntry[] entries, int width, int height, int x, int y) : base(0xbbc, width, height, x, y, true)
        {
            this.m_xLast = -2147483648;
            this.m_yLast = -2147483648;
            this.m_Entries = entries;
        }

        protected internal override void Draw(int x, int y)
        {
            base.Draw(x, y);
            if ((this.m_xLast != x) || (this.m_yLast != y))
            {
                this.m_xLast = x;
                this.m_yLast = y;
                Clipper clipper = new Clipper(x + base.OffsetX, y + base.OffsetY, base.UseWidth, base.UseHeight);
                for (int i = 0; i < this.m_Entries.Length; i++)
                {
                    this.m_Entries[i].Clipper = clipper;
                }
            }
        }

        protected internal override void OnMouseWheel(int delta)
        {
            base.m_Parent.OnMouseWheel(delta);
        }
    }
}

