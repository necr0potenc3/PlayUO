namespace Client
{
    using System;

    public sealed class LoginCrypto : BaseCrypto
    {
        public LoginCrypto(uint seed) : base(seed)
        {
        }

        public override int Decrypt(byte[] input, int inputStart, int count, byte[] output, int outputStart)
        {
            for (int i = 0; i < count; i++)
            {
                output[i + outputStart] = input[i + inputStart];
            }
            return count;
        }

        public override void Encrypt(byte[] buffer, int start, int count)
        {
        }

        protected override void InitKeys(uint seed)
        {
        }
    }
}

