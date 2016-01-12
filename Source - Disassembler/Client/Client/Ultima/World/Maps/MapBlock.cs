namespace Client
{
    public class MapBlock
    {
        public Tile[] m_LandTiles;
        public HuedTile[][][] m_StaticTiles;

        public MapBlock(Tile[] landTiles, HuedTile[][][] staticTiles)
        {
            this.m_LandTiles = landTiles;
            this.m_StaticTiles = staticTiles;
        }
    }
}