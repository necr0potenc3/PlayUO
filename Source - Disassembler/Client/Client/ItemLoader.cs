namespace Client
{
    public class ItemLoader : ILoader
    {
        private bool m_Grayscale;
        private int m_ItemID;

        public ItemLoader(int ItemID, bool Grayscale)
        {
            this.m_ItemID = ItemID;
            this.m_Grayscale = Grayscale;
        }

        public void Load()
        {
            Texture texture;
            if (!this.m_Grayscale)
            {
                texture = Hues.Default.GetItem(this.m_ItemID);
            }
            else
            {
                texture = Hues.Grayscale.GetItem(this.m_ItemID);
            }
        }
    }
}