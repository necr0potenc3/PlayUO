namespace Client
{
    using Microsoft.DirectX.Direct3D;
    using System;
    using System.Collections;

    public class StaticItem : IItem, ITile, ICell, IDisposable
    {
        public bool m_bAlpha;
        public bool m_bDraw;
        public bool m_bInit;
        public byte m_Height;
        public IHue m_Hue;
        public short m_ID;
        private static Queue m_InstancePool = new Queue();
        private Client.Texture m_LastImage;
        private IHue m_LastImageHue;
        private short m_LastImageID;
        public short m_RealID;
        public Client.Texture m_sDraw;
        public int m_SortInfluence;
        public byte m_vAlpha;
        public CustomVertex.TransformedColoredTextured[] m_vPool;
        public sbyte m_Z;
        private static Type MyType = typeof(StaticItem);
        public int Serial;

        private StaticItem(HuedTile tile, int influence, int serial)
        {
            this.m_ID = (short)tile.ID;
            this.m_Z = (sbyte)tile.Z;
            this.m_RealID = (short)((tile.ID & 0x3fff) | 0x4000);
            this.m_ID = (short)(this.m_ID & 0x3fff);
            this.m_ID = (short)(this.m_ID + 0x4000);
            this.m_Hue = Hues.GetItemHue(this.m_ID, tile.Hue);
            this.m_Height = Map.GetHeight(this.m_ID);
            this.m_SortInfluence = influence;
            this.Serial = serial;
            this.m_vPool = VertexConstructor.Create();
        }

        private unsafe StaticItem(byte* pvSrc, int si, int serial)
        {
            this.m_ID = *((short*)pvSrc);
            this.m_Z = *((sbyte*)(pvSrc + 4));
            this.m_RealID = (short)((this.m_ID & 0x3fff) | 0x4000);
            this.m_ID = (short)(this.m_ID & 0x3fff);
            this.m_ID = (short)(this.m_ID + 0x4000);
            this.m_Hue = Hues.GetItemHue(this.m_ID, *((ushort*)(pvSrc + 5)));
            this.m_Height = Map.GetHeight(this.m_ID);
            this.m_SortInfluence = si;
            this.Serial = serial;
            this.m_vPool = VertexConstructor.Create();
        }

        private StaticItem(short ItemID, sbyte Z, int serial)
        {
            this.m_ID = ItemID;
            this.m_RealID = (short)((this.m_ID & 0x3fff) | 0x4000);
            this.m_ID = (short)(this.m_ID & 0x3fff);
            this.m_ID = (short)(this.m_ID + 0x4000);
            this.m_Z = Z;
            this.m_Height = Map.GetHeight(this.m_ID);
            this.m_Hue = Hues.Default;
            this.Serial = serial;
            this.m_vPool = VertexConstructor.Create();
        }

        public Client.Texture GetItem(IHue hue, short itemID)
        {
            if ((this.m_LastImageHue != hue) || (this.m_LastImageID != itemID))
            {
                this.m_LastImageHue = hue;
                this.m_LastImageID = itemID;
                this.m_LastImage = hue.GetItem(itemID);
            }
            return this.m_LastImage;
        }

        public static StaticItem Instantiate(HuedTile tile, int influence, int serial)
        {
            if (m_InstancePool.Count > 0)
            {
                StaticItem item = (StaticItem)m_InstancePool.Dequeue();
                item.m_RealID = (short)((tile.ID & 0x3fff) | 0x4000);
                item.m_ID = (short)tile.ID;
                item.m_ID = (short)(item.m_ID & 0x3fff);
                item.m_ID = (short)(item.m_ID + 0x4000);
                item.m_Z = (sbyte)tile.Z;
                item.m_Hue = Hues.GetItemHue(item.m_ID, tile.Hue);
                item.m_Height = Map.GetHeight(item.m_ID);
                item.m_SortInfluence = influence;
                item.Serial = serial;
                item.m_LastImage = null;
                item.m_LastImageHue = null;
                item.m_LastImageID = 0;
                item.m_vAlpha = 0;
                item.m_bAlpha = false;
                item.m_bDraw = false;
                item.m_bInit = false;
                return item;
            }
            return new StaticItem(tile, influence, serial);
        }

        public static unsafe StaticItem Instantiate(byte* pSrc, int sortInfluence, int serial)
        {
            if (m_InstancePool.Count > 0)
            {
                StaticItem item = (StaticItem)m_InstancePool.Dequeue();
                item.m_RealID = (short)((*(((short*)pSrc)) & 0x3fff) | 0x4000);
                item.m_ID = *((short*)pSrc);
                item.m_ID = (short)(item.m_ID & 0x3fff);
                item.m_ID = (short)(item.m_ID + 0x4000);
                item.m_Z = *((sbyte*)(pSrc + 4));
                item.m_Hue = Hues.GetItemHue(item.m_ID, *((ushort*)(pSrc + 5)));
                item.m_Height = Map.GetHeight(item.m_ID);
                item.m_SortInfluence = sortInfluence;
                item.Serial = serial;
                item.m_LastImage = null;
                item.m_LastImageHue = null;
                item.m_LastImageID = 0;
                item.m_vAlpha = 0;
                item.m_bAlpha = false;
                item.m_bDraw = false;
                item.m_bInit = false;
                return item;
            }
            return new StaticItem(pSrc, sortInfluence, serial);
        }

        public static StaticItem Instantiate(short itemID, sbyte z, int serial)
        {
            if (m_InstancePool.Count > 0)
            {
                StaticItem item = (StaticItem)m_InstancePool.Dequeue();
                item.m_RealID = (short)((itemID & 0x3fff) | 0x4000);
                item.m_ID = itemID;
                item.m_ID = (short)(item.m_ID & 0x3fff);
                item.m_ID = (short)(item.m_ID + 0x4000);
                item.m_Z = z;
                item.m_Hue = Hues.Default;
                item.m_Height = Map.GetHeight(item.m_ID);
                item.m_SortInfluence = 0;
                item.Serial = serial;
                item.m_LastImage = null;
                item.m_LastImageHue = null;
                item.m_LastImageID = 0;
                item.m_vAlpha = 0;
                item.m_bAlpha = false;
                item.m_bDraw = false;
                item.m_bInit = false;
                return item;
            }
            return new StaticItem(itemID, z, serial);
        }

        public static StaticItem Instantiate(short itemID, short realID, sbyte z, int serial)
        {
            if (m_InstancePool.Count > 0)
            {
                StaticItem item = (StaticItem)m_InstancePool.Dequeue();
                item.m_RealID = (short)((realID & 0x3fff) | 0x4000);
                item.m_ID = itemID;
                item.m_ID = (short)(item.m_ID & 0x3fff);
                item.m_ID = (short)(item.m_ID + 0x4000);
                item.m_Z = z;
                item.m_Hue = Hues.Default;
                item.m_Height = Map.GetHeight(item.m_ID);
                item.m_SortInfluence = 0;
                item.Serial = serial;
                item.m_LastImage = null;
                item.m_LastImageHue = null;
                item.m_LastImageID = 0;
                item.m_vAlpha = 0;
                item.m_bAlpha = false;
                item.m_bDraw = false;
                item.m_bInit = false;
                return item;
            }
            return new StaticItem(itemID, z, serial);
        }

        void IDisposable.Dispose()
        {
            m_InstancePool.Enqueue(this);
        }

        public byte CalcHeight
        {
            get
            {
                if (Map.m_ItemFlags[this.m_ID & 0x3fff][TileFlag.Bridge])
                {
                    return (byte)(this.m_Height / 2);
                }
                return this.m_Height;
            }
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
        }

        public IHue Hue
        {
            get
            {
                return this.m_Hue;
            }
        }

        public short ID
        {
            get
            {
                return this.m_ID;
            }
        }

        public int SortInfluence
        {
            get
            {
                return this.m_SortInfluence;
            }
        }

        public sbyte SortZ
        {
            get
            {
                return this.m_Z;
            }
            set
            {
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