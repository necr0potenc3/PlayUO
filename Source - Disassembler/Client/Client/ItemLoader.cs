namespace Client
{
    using System;

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
            Texture item;
            if (!this.m_Grayscale)
            {
                item = Hues.Default.GetItem(this.m_ItemID);
            }
            else
            {
                item = Hues.Grayscale.GetItem(this.m_ItemID);
            }
        }
    }
}

