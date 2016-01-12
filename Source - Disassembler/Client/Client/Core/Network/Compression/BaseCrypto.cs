namespace Client
{
    public abstract class BaseCrypto
    {
        private uint m_Seed;

        public BaseCrypto(uint seed)
        {
            this.m_Seed = seed;
            this.InitKeys(seed);
        }

        public abstract int Decrypt(byte[] input, int inputStart, int count, byte[] output, int outputStart);

        public abstract void Encrypt(byte[] buffer, int start, int count);

        protected abstract void InitKeys(uint seed);

        public uint Seed
        {
            get
            {
                return this.m_Seed;
            }
        }
    }
}