namespace Client.Targeting
{
    using Client;

    public class StackTargetHandler : ITargetHandler
    {
        public void OnCancel(TargetCancelType type)
        {
            Engine.AddTextMessage("Stack request canceled.");
        }

        public void OnTarget(object o)
        {
            if (o is Item)
            {
                Item item = (Item)o;
                Mobile player = World.Player;
                if (player != null)
                {
                    Item backpack = player.Backpack;
                    if (backpack != null)
                    {
                        if (item.Parent == null)
                        {
                            Engine.AddTextMessage("Target a contained item.");
                            Engine.TargetHandler = this;
                        }
                        else
                        {
                            Item[] itemArray = backpack.FindItems(new ItemIDValidator(new int[] { item.ID }));
                            for (int i = 0; i < itemArray.Length; i++)
                            {
                                if (itemArray[i] != item)
                                {
                                    Engine.QueueAutoMove(itemArray[i], itemArray[i].Amount, item.ContainerX, item.ContainerY, 0, item.Serial);
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