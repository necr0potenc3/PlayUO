namespace Client
{
    using System.IO;

    public class PGameSeed : Packet
    {
        public PGameSeed(int gameSeed) : base(0, "Game Seed", 4)
        {
            base.m_Encode = false;
            if (NewConfig.EncryptionFix)
            {
                gameSeed = 0;
            }
            base.m_Stream.Seek(0L, SeekOrigin.Begin);
            base.m_Stream.Write(gameSeed);
            Network.SetupCrypto((uint)gameSeed);
        }
    }
}