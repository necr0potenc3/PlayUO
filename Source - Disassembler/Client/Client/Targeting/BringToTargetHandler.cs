namespace Client.Targeting
{
    using Client;
    using System;

    public class BringToTargetHandler : ITargetHandler
    {
        private int m_xOffset;
        private int m_yOffset;

        public BringToTargetHandler(int xOffset, int yOffset)
        {
            this.m_xOffset = xOffset;
            this.m_yOffset = yOffset;
        }

        public void OnCancel(TargetCancelType type)
        {
            Engine.AddTextMessage("Bring request canceled.");
        }

        public void OnTarget(object o)
        {
            if (o is Item)
            {
                Item item = (Item) o;
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
                            int x = item.ContainerX + this.m_xOffset;
                            int y = item.ContainerY + this.m_yOffset;
                            for (int i = 0; i < itemArray.Length; i++)
                            {
                                if ((itemArray[i] != item) && (((itemArray[i].Parent != item.Parent) || (itemArray[i].ContainerX != x)) || (itemArray[i].ContainerY != y)))
                                {
                                    Engine.QueueAutoMove(itemArray[i], itemArray[i].Amount, x, y, 0, item.Parent.Serial);
                                    x += this.m_xOffset;
                                    y += this.m_yOffset;
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

