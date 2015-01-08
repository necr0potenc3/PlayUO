namespace Client.Targeting
{
    using Client;
    using System;

    public class SetRegStockTargetHandler : ITargetHandler
    {
        public void OnCancel(TargetCancelType type)
        {
        }

        public void OnTarget(object o)
        {
            if (o is Item)
            {
                Item item = (Item) o;
                if (!Map.m_ItemFlags[item.ID & 0x3fff][TileFlag.Container])
                {
                    Engine.AddTextMessage("That is not a container.");
                    Engine.TargetHandler = this;
                }
                else
                {
                    World.CharData.Stock = item;
                    Engine.AddTextMessage("Source reagent container set.");
                }
            }
            else
            {
                Engine.TargetHandler = this;
            }
        }
    }
}

