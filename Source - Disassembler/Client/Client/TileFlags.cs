﻿namespace Client
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct TileFlags
    {
        public int Value;

        public TileFlags(int Value)
        {
            this.Value = Value;
        }

        public override string ToString()
        {
            return ((TileFlag)this.Value).ToString();
        }

        public bool this[TileFlag flag]
        {
            get
            {
                return ((this.Value & (int)flag) != 0);
            }
        }
    }
}