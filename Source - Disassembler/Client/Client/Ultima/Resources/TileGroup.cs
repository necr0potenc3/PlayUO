namespace Client
{
    public class TileGroup
    {
        private int m_Count;
        private int m_Start;

        public TileGroup(int start, int count)
        {
            this.m_Start = start;
            this.m_Count = count;
        }

        public static unsafe int GetBrightness(Texture tex, int xStart, int yStart, int xStep, int yStep, int count)
        {
            LockData data = tex.Lock(LockFlags.ReadOnly);
            short* pvSrc = (short*)data.pvSrc;
            int num = data.Pitch >> 1;
            pvSrc += yStart * num;
            pvSrc += xStart;
            num *= yStep;
            num += xStep;
            int num2 = 0;
            for (int i = 0; i < count; i++)
            {
                short num4 = pvSrc[0];
                num2 += (num4 & 0x1f) * 0x72;
                num2 += ((num4 >> 5) & 0x1f) * 0x24b;
                num2 += ((num4 >> 10) & 0x1f) * 0x12b;
                pvSrc += num;
            }
            tex.Unlock();
            num2 = num2 << 3;
            num2 /= count;
            return (num2 / 0x3e8);
        }
    }
}