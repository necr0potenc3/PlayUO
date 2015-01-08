namespace Client
{
    using System;
    using System.Collections;

    public class DynamicItem : IItem, ITile, ICell, IDisposable, IEntity
    {
        public byte m_Height;
        public IHue m_Hue;
        public short m_ID;
        private static Queue m_InstancePool = new Queue();
        public Item m_Item;
        private Texture m_LastImage;
        private IHue m_LastImageHue;
        private short m_LastImageID;
        public sbyte m_Z;
        private static Type MyType = typeof(DynamicItem);

        private DynamicItem(Item i)
        {
            this.m_Item = i;
            this.m_ID = i.ID;
            this.m_ID = (short) (this.m_ID & 0x3fff);
            this.m_ID = (short) (this.m_ID + 0x4000);
            this.m_Z = (sbyte) i.Z;
            this.m_Hue = Hues.GetItemHue(this.m_ID, i.Hue);
            this.m_Height = Map.GetHeight(this.m_ID);
        }

        public Texture GetItem(IHue hue, short itemID)
        {
            if ((this.m_LastImageHue != hue) || (this.m_LastImageID != itemID))
            {
                this.m_LastImageHue = hue;
                this.m_LastImageID = itemID;
                this.m_LastImage = hue.GetItem(itemID);
            }
            return this.m_LastImage;
        }

        public static DynamicItem Instantiate(Item item)
        {
            if (m_InstancePool.Count > 0)
            {
                DynamicItem item2 = (DynamicItem) m_InstancePool.Dequeue();
                item2.m_Item = item;
                item2.m_ID = item.ID;
                item2.m_ID = (short) (item2.m_ID & 0x3fff);
                item2.m_ID = (short) (item2.m_ID + 0x4000);
                item2.m_Z = (sbyte) item.Z;
                item2.m_Hue = Hues.GetItemHue(item2.m_ID, item.Hue);
                item2.m_Height = Map.GetHeight(item2.m_ID);
                item2.m_LastImage = null;
                item2.m_LastImageHue = null;
                item2.m_LastImageID = 0;
                return item2;
            }
            return new DynamicItem(item);
        }

        void IDisposable.Dispose()
        {
            m_InstancePool.Enqueue(this);
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

        public int Serial
        {
            get
            {
                return this.m_Item.Serial;
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

