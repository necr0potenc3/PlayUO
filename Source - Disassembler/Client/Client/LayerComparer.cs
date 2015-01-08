namespace Client
{
    using System;
    using System.Collections;

    public class LayerComparer : IComparer
    {
        public static readonly LayerComparer Backward = new LayerComparer(LayerComparerType.Backward);
        private static Layer ChainTunic = ((Layer) 0xfe);
        public static readonly LayerComparer Forward = new LayerComparer(LayerComparerType.Forward);
        private static Layer LeatherShorts = ((Layer) 0xfd);
        private LayerComparerType m_ComparerType;
        private static Layer[] m_DesiredLayerOrder = new Layer[] { 
            Layer.Bracelet, Layer.Ring, Layer.Shirt, Layer.Pants, Layer.InnerLegs, Layer.Shoes, PlateLegs, LeatherShorts, Layer.Arms, Layer.InnerTorso, LeatherShorts, PlateArms, Layer.MiddleTorso, Layer.OuterLegs, Layer.Neck, Layer.Waist, 
            Layer.Gloves, Layer.OuterTorso, Layer.OneHanded, Layer.TwoHanded, Layer.FacialHair, Layer.Hair, Layer.Helm
         };
        private int[] m_TranslationTable;
        public static readonly LayerComparer Paperdoll = Forward;
        private static Layer PlateArms = ((Layer) 0xff);
        private static Layer PlateLegs = ((Layer) 0xfc);

        public LayerComparer(LayerComparerType comparerType)
        {
            this.m_ComparerType = comparerType;
            Layer[] destinationArray = new Layer[m_DesiredLayerOrder.Length + 3];
            destinationArray[0] = Layer.Mount;
            if (comparerType == LayerComparerType.Forward)
            {
                destinationArray[1] = Layer.Cloak;
                Array.Copy(m_DesiredLayerOrder, 0, destinationArray, 2, m_DesiredLayerOrder.Length);
            }
            else
            {
                Array.Copy(m_DesiredLayerOrder, 0, destinationArray, 1, m_DesiredLayerOrder.Length);
                destinationArray[1 + m_DesiredLayerOrder.Length] = Layer.Cloak;
            }
            destinationArray[destinationArray.Length - 1] = Layer.Backpack;
            this.m_TranslationTable = new int[0x100];
            for (int i = 0; i < destinationArray.Length; i++)
            {
                this.m_TranslationTable[(int) destinationArray[i]] = destinationArray.Length - i;
            }
        }

        public int Compare(object x, object y)
        {
            EquipEntry entry = (EquipEntry) x;
            EquipEntry entry2 = (EquipEntry) y;
            Layer oldLayer = entry.m_Layer;
            Layer layer = entry2.m_Layer;
            oldLayer = this.Fix(entry.m_Item.ID, oldLayer);
            layer = this.Fix(entry2.m_Item.ID, layer);
            return (this.m_TranslationTable[(int) layer] - this.m_TranslationTable[(int) oldLayer]);
        }

        public Layer Fix(int itemID, Layer oldLayer)
        {
            itemID &= 0x3fff;
            if ((itemID == 0x1410) || (itemID == 0x1417))
            {
                return PlateArms;
            }
            if ((itemID == 0x13bf) || (itemID == 0x13c4))
            {
                return ChainTunic;
            }
            if ((itemID == 0x1c08) || (itemID == 0x1c09))
            {
                return LeatherShorts;
            }
            if ((itemID == 0x1c00) || (itemID == 0x1c01))
            {
                return LeatherShorts;
            }
            if ((itemID == 0x1411) || (itemID == 0x141a))
            {
                return PlateLegs;
            }
            return oldLayer;
        }

        public static LayerComparer FromDirection(MobileDirection dir)
        {
            dir &= MobileDirection.Up;
            if (dir == MobileDirection.Down)
            {
                return Forward;
            }
            return Backward;
        }

        public static LayerComparer FromDirection(int dir)
        {
            return FromDirection((MobileDirection) ((byte) dir));
        }

        public int GetValue(int itemID, Layer layer)
        {
            return this.m_TranslationTable[(int) this.Fix(itemID, layer)];
        }

        public bool IsValid(EquipEntry entry)
        {
            return this.IsValid(entry.m_Layer);
        }

        public bool IsValid(Layer layer)
        {
            return (this.m_TranslationTable[(int) layer] > 0);
        }

        public int[] TranslationTable
        {
            get
            {
                return this.m_TranslationTable;
            }
        }
    }
}

