namespace Client
{
    using System;

    public class LandLoader : ILoader
    {
        private bool m_Grayscale;
        private int m_LandID;

        public LandLoader(int LandID, bool Grayscale)
        {
            this.m_LandID = LandID;
            this.m_Grayscale = Grayscale;
        }

        public void Load()
        {
            Texture land = null;
            Texture texture = null;
            if (!this.m_Grayscale)
            {
                MapLighting.CheckStretchTable();
                if (!MapLighting.m_AlwaysStretch[this.m_LandID & 0x3fff])
                {
                    land = Hues.Default.GetLand(this.m_LandID);
                }
                int textureID = Map.GetTexture(this.m_LandID);
                if ((textureID > 0) && (textureID < 0x1000))
                {
                    texture = Hues.Default.GetTexture(textureID);
                }
            }
            else
            {
                MapLighting.CheckStretchTable();
                if (!MapLighting.m_AlwaysStretch[this.m_LandID & 0x3fff])
                {
                    land = Hues.Grayscale.GetLand(this.m_LandID);
                }
                int num2 = Map.GetTexture(this.m_LandID);
                if ((num2 > 0) && (num2 < 0x1000))
                {
                    texture = Hues.Grayscale.GetTexture(num2);
                }
            }
        }
    }
}

