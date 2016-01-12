namespace Client
{
    using System.IO;

    public class PLoginSeed : Packet
    {
        public PLoginSeed() : base(0, "Login Seed", 4)
        {
            base.m_Encode = false;
            int clientIP = Network.ClientIP;
            if (NewConfig.EncryptionFix)
            {
                clientIP = 0;
            }
            base.m_Stream.Seek(0L, SeekOrigin.Begin);
            base.m_Stream.Write(clientIP);
            Network.SetupCrypto((uint)clientIP);
        }
    }
}