namespace Client.Targeting
{
    using Client;
    using System;

    public class RegDropTargetHandler : ITargetHandler
    {
        public void OnCancel(TargetCancelType type)
        {
            Engine.AddTextMessage("Reagent drop request canceled.");
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
                    Mobile player = World.Player;
                    if (player != null)
                    {
                        Item backpack = player.Backpack;
                        if (backpack != null)
                        {
                            Item[] itemArray = backpack.FindItems(ReagentValidator.Validator);
                            if (itemArray.Length == 0)
                            {
                                Engine.AddTextMessage("You do not have any reagents.");
                            }
                            else
                            {
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
            }
            else
            {
                Engine.TargetHandler = this;
            }
        }
    }
}

