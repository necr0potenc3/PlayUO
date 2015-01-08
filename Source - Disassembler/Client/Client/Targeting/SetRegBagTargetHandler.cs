namespace Client.Targeting
{
    using Client;
    using System;

    public class SetRegBagTargetHandler : ITargetHandler
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
                    World.CharData.RegBag = item;
                    Engine.AddTextMessage("Destination reagent container set.");
                }
            }
            else
            {
                Engine.TargetHandler = this;
            }
        }
    }
}

