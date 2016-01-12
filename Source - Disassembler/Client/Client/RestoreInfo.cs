namespace Client
{
    using System.Collections;

    public class RestoreInfo
    {
        public int m_cX;
        public int m_cY;
        public EquipEntry m_EquipEntry;
        public object m_EquipParent;
        public bool m_InWorld;
        public bool m_IsEquip;
        public Item m_Parent;
        public int m_X;
        public int m_Y;
        public int m_Z;

        public RestoreInfo(Item item)
        {
            this.m_X = item.X;
            this.m_Y = item.Y;
            this.m_Z = item.Z;
            this.m_cX = item.ContainerX;
            this.m_cY = item.ContainerY;
            this.m_InWorld = item.InWorld;
            this.m_IsEquip = item.IsEquip;
            this.m_Parent = item.Parent;
            this.m_EquipParent = item.EquipParent;
            ArrayList equip = null;
            if (this.m_EquipParent is Mobile)
            {
                equip = ((Mobile)this.m_EquipParent).Equip;
            }
            else if (this.m_EquipParent is Item)
            {
                equip = ((Item)this.m_EquipParent).Equip;
            }
            if (equip != null)
            {
                for (int i = 0; i < equip.Count; i++)
                {
                    EquipEntry entry = (EquipEntry)equip[i];
                    if (entry.m_Item == item)
                    {
                        this.m_EquipEntry = entry;
                        break;
                    }
                }
            }
        }
    }
}