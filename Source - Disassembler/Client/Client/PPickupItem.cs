namespace Client
{
    using System;

    public class PPickupItem : Packet
    {
        public static Item m_Item;

        public PPickupItem(Item item, short amount) : base(7, "Pickup Item", 7)
        {
            m_Item = item;
            Engine.m_LastAction = DateTime.Now;
            base.m_Stream.Write(item.Serial);
            base.m_Stream.Write(amount);
        }
    }
}

