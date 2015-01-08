namespace Client.Targeting
{
    using Client;
    using System;

    public class SetEquipTargetHandler : ITargetHandler
    {
        private int m_Index;

        public SetEquipTargetHandler(int index)
        {
            this.m_Index = index;
        }

        public void OnCancel(TargetCancelType type)
        {
            Engine.AddTextMessage("Request to set auto-equip canceled.");
        }

        public void OnTarget(object o)
        {
            if (o is Item)
            {
                Item item = (Item) o;
                if (Map.m_ItemFlags[item.ID & 0x3fff][TileFlag.Wearable])
                {
                    switch (Map.GetQuality(item.ID))
                    {
                        case 1:
                        case 2:
                            World.CharData.Equip.AutoEquip[this.m_Index] = item.Serial;
                            World.CharData.Save();
                            return;
                    }
                }
            }
            Engine.TargetHandler = this;
        }
    }
}

