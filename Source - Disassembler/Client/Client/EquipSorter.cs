namespace Client
{
    using System;
    using System.Collections;

    public class EquipSorter : IComparer
    {
        public const int LAYER_ARMS = 0x13;
        public const int LAYER_BACK = 20;
        public const int LAYER_BACKPACK = 0x15;
        public const int LAYER_BRACELET = 14;
        public const int LAYER_EARRINGS = 0x12;
        public const int LAYER_FACIALHAIR = 0x10;
        public const int LAYER_GLOVES = 7;
        public const int LAYER_HAIR = 11;
        public const int LAYER_HELM = 6;
        public const int LAYER_LEGSINNER = 0x18;
        public const int LAYER_LEGSOUTER = 0x17;
        public const int LAYER_MOUNT = 0x19;
        public const int LAYER_NECK = 10;
        public const int LAYER_NPCBUYNORES = 0x1b;
        public const int LAYER_NPCBUYRES = 0x1a;
        public const int LAYER_NPCSELL = 0x1c;
        public const int LAYER_ONEHANDED = 1;
        public const int LAYER_PANTS = 4;
        public const int LAYER_PCBANK = 0x1d;
        public const int LAYER_RING = 8;
        public const int LAYER_SHIRT = 5;
        public const int LAYER_SHOES = 3;
        public const int LAYER_TORSOINNER = 13;
        public const int LAYER_TORSOMIDDLE = 0x11;
        public const int LAYER_TORSOOUTER = 0x16;
        public const int LAYER_TWOHANDED = 2;
        public const int LAYER_WAIST = 12;
        public int[] m_Table;

        public int Compare(object x, object y)
        {
            int num = this.m_Table[(int) ((EquipEntry) x).m_Layer];
            int num2 = this.m_Table[(int) ((EquipEntry) y).m_Layer];
            if (num < num2)
            {
                return -1;
            }
            if (num > num2)
            {
                return 1;
            }
            return 0;
        }

        public bool IsValidLayer(Layer layer)
        {
            return (this.m_Table[(int) layer] != -1);
        }
    }
}

