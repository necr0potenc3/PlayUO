namespace Client.Targeting
{
    using Client;
    using System;

    public class MoveTargetHandler : ITargetHandler
    {
        private Item m_Item;

        public void OnCancel(TargetCancelType type)
        {
            Engine.AddTextMessage("Move request canceled.");
        }

        public void OnTarget(object o)
        {
            if (this.m_Item == null)
            {
                if (o is Item)
                {
                    this.m_Item = (Item) o;
                    Engine.AddTextMessage("Target the destination container.");
                }
                Engine.TargetHandler = this;
            }
            else if (o is Item)
            {
                Item item = (Item) o;
                if (!Map.m_ItemFlags[item.ID & 0x3fff][TileFlag.Container])
                {
                    Engine.AddTextMessage("That is not a container.");
                    Engine.TargetHandler = this;
                }
                else
                {
                    Mobile player = World.Player;
                    if (player != null)
                    {
                        Item backpack = player.Backpack;
                        if (backpack != null)
                        {
                            Item[] itemArray = backpack.FindItems(new ItemIDValidator(new int[] { this.m_Item.ID }));
                            for (int i = 0; i < itemArray.Length; i++)
                            {
                                if (itemArray[i].Parent != item)
                                {
                                    Engine.QueueAutoMove(itemArray[i], itemArray[i].Amount, -1, -1, 0, item.Serial);
                                }
                            }
                            Engine.PrintQAM();
                        }
                    }
                }
            }
            else
            {
                Engine.TargetHandler = this;
            }
        }
    }
}

