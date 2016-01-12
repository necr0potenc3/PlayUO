namespace Client
{
    public class TileList
    {
        private int m_Count = 0;
        private static HuedTile[] m_Empty = new HuedTile[0];
        private HuedTile[] m_Tiles = new HuedTile[8];

        public void Add(short id, short hue, sbyte z)
        {
            if ((this.m_Count + 1) > this.m_Tiles.Length)
            {
                HuedTile[] tiles = this.m_Tiles;
                this.m_Tiles = new HuedTile[tiles.Length * 2];
                for (int i = 0; i < tiles.Length; i++)
                {
                    this.m_Tiles[i] = tiles[i];
                }
            }
            this.m_Tiles[this.m_Count++].Set(id, hue, z);
        }

        public HuedTile[] ToArray()
        {
            if (this.m_Count == 0)
            {
                return m_Empty;
            }
            HuedTile[] tileArray = new HuedTile[this.m_Count];
            for (int i = 0; i < this.m_Count; i++)
            {
                tileArray[i] = this.m_Tiles[i];
            }
            this.m_Count = 0;
            return tileArray;
        }

        public int Count
        {
            get
            {
                return this.m_Count;
            }
        }
    }
}