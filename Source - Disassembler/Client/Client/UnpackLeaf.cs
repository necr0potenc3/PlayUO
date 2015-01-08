namespace Client
{
    using System;

    public class UnpackLeaf
    {
        public int[] m_Cache;
        public short m_Index;
        public UnpackLeaf m_Left;
        public UnpackLeaf m_Right;
        public short m_Value = -1;

        public UnpackLeaf(int index)
        {
            this.m_Index = (short) index;
        }
    }
}

