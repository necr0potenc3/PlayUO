namespace Client
{
    using Client.Targeting;
    using System;

    public class SetItemPropertyTarget : ITargetHandler
    {
        private GPropertyEntry m_Entry;

        public SetItemPropertyTarget(GPropertyEntry entry)
        {
            this.m_Entry = entry;
        }

        public void OnCancel(TargetCancelType why)
        {
        }

        public void OnTarget(object targeted)
        {
            if (targeted is Item)
            {
                this.m_Entry.SetValue(targeted);
            }
            else
            {
                Engine.TargetHandler = this;
            }
        }
    }
}

