namespace Client
{
    using System;

    public class PEquipItem : Packet
    {
        public PEquipItem(Item toEquip, Mobile target) : base(0x13, "Equip Item", 10)
        {
            base.m_Stream.Write(toEquip.Serial);
            base.m_Stream.Write(Map.GetQuality(toEquip.ID & 0x3fff));
            base.m_Stream.Write(target.Serial);
        }
    }
}

