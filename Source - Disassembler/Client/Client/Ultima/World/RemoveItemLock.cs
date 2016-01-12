namespace Client
{
    public class RemoveItemLock : ILocked
    {
        private Item m_Item;

        public RemoveItemLock(Item item)
        {
            this.m_Item = item;
        }

        public void Invoke()
        {
            Map.RemoveItem(this.m_Item);
        }
    }
}