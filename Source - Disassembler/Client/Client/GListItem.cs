namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GListItem : GTextButton
    {
        private int m_Index;
        private GListBox m_Owner;

        public GListItem(string Text, int Index, GListBox Owner) : base(Text, Owner.Font, Owner.HRegular, Owner.HOver, Owner.OffsetX, Owner.OffsetY, null)
        {
            this.m_Index = Index;
            this.m_Owner = Owner;
            this.Layout();
        }

        public void Layout()
        {
            base.m_Visible = (this.m_Index >= this.m_Owner.StartIndex) && (this.m_Index < (this.m_Owner.StartIndex + this.m_Owner.ItemCount));
            base.m_Y = this.m_Owner.OffsetY + ((this.m_Index - this.m_Owner.StartIndex) * 0x12);
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            this.m_Owner.OnListItemClick(this);
        }

        public int Index
        {
            get
            {
                return this.m_Index;
            }
        }
    }
}

