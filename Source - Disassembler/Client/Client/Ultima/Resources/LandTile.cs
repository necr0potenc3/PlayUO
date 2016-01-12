namespace Client
{
    using Microsoft.DirectX.Direct3D;
    using System;

    public class LandTile : ITile, ICell, IDisposable
    {
        public bool m_bDraw;
        public bool m_bFilter;
        public bool m_bInit;
        public bool m_FoldLeftRight;
        public bool m_Guarded;
        public byte m_Height;
        public short m_ID;
        public Client.Texture m_sDraw;
        private sbyte m_SortZ;
        public CustomVertex.TransformedColoredTextured[] m_vDraw;
        public sbyte m_Z;
        private static Type MyType = typeof(LandTile);

        public static unsafe void Initialize(LandTile t, byte* pSrc)
        {
            t.m_ID = *((short*)pSrc);
            t.m_ID = (short)(t.m_ID & 0x3fff);
            t.m_ID = (short)TextureTable.m_Table[t.m_ID];
            t.m_SortZ = t.m_Z = *((sbyte*)(pSrc + 2));
            t.m_bDraw = false;
            t.m_bInit = false;
            t.m_bFilter = false;
            t.m_Height = 0;
            t.m_sDraw = null;
            t.m_Guarded = false;
            t.m_FoldLeftRight = false;
        }

        public static void Initialize(LandTile t, short id, sbyte z)
        {
            t.m_ID = id;
            t.m_ID = (short)(t.m_ID & 0x3fff);
            t.m_ID = (short)TextureTable.m_Table[t.m_ID];
            t.m_SortZ = t.m_Z = z;
            t.m_bDraw = false;
            t.m_bInit = false;
            t.m_bFilter = false;
            t.m_Height = 0;
            t.m_sDraw = null;
            t.m_Guarded = false;
            t.m_FoldLeftRight = false;
        }

        void IDisposable.Dispose()
        {
        }

        public Type CellType
        {
            get
            {
                return MyType;
            }
        }

        public byte Height
        {
            get
            {
                return this.m_Height;
            }
            set
            {
                this.m_Height = value;
            }
        }

        public short ID
        {
            get
            {
                return this.m_ID;
            }
        }

        public bool Ignored
        {
            get
            {
                return (((this.m_ID == 2) || (this.m_ID == 0x1db)) || ((this.m_ID >= 430) && (this.m_ID <= 0x1b5)));
            }
        }

        public sbyte SortZ
        {
            get
            {
                return this.m_SortZ;
            }
            set
            {
                this.m_SortZ = value;
            }
        }

        public sbyte Z
        {
            get
            {
                return this.m_Z;
            }
        }
    }
}