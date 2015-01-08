namespace Client
{
    using System;

    public class UpdateItemLock : ILocked
    {
        private Item m_Item;

        public UpdateItemLock(Item item)
        {
            this.m_Item = item;
        }

        public void Invoke()
        {
            Map.UpdateItem(this.m_Item);
        }
    }
}

