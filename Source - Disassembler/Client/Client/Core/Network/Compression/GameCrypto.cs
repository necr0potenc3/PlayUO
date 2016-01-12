namespace Client
{
    using System.IO;

    public sealed class GameCrypto : BaseCrypto
    {
        private UnpackLeaf m_Leaf;

        public GameCrypto(uint seed) : base(seed)
        {
        }

        public override unsafe int Decrypt(byte[] input, int inputStart, int count, byte[] output, int outputStart)
        {
            fixed (byte* numRef = output)
            {
                byte* numPtr = numRef + outputStart;
                fixed (UnpackCacheEntry* entryRef = Network.m_CacheEntries)
                {
                    fixed (byte* numRef2 = Network.m_OutputBuffer)
                    {
                        fixed (byte* numRef3 = input)
                        {
                            UnpackLeaf leaf = this.m_Leaf;
                            UnpackLeaf[] leaves = Network.m_Leaves;
                            byte* numPtr3 = numRef3;
                            byte* numPtr4 = numPtr3 + count;
                            while (numPtr3 < numPtr4)
                            {
                                UnpackCacheEntry entry = entryRef[leaf.m_Cache[*(numPtr3++)]];
                                leaf = leaves[entry.m_NextIndex];
                                byte* numPtr2 = numRef2 + entry.m_ByteIndex;
                                byte* numPtr5 = numPtr + entry.m_ByteCount;
                                while (numPtr < numPtr5)
                                {
                                    *(numPtr++) = *(numPtr2++);
                                }
                            }
                            this.m_Leaf = leaf;
                            if (numPtr >= (numRef + output.Length))
                            {
                                throw new InternalBufferOverflowException("Network::Decompress(): Buffer overflow.");
                            }
                        }
                    }
                }
                return (int)((long)(((numPtr - outputStart) - numRef) / 1));
            }
        }

        public override void Encrypt(byte[] buffer, int start, int count)
        {
        }

        protected override void InitKeys(uint seed)
        {
            this.m_Leaf = Network.m_Tree;
        }
    }
}