namespace Client.Targeting
{
    using Client;
    using System;

    public class MakeRegsTargetHandler : ITargetHandler
    {
        private int m_Amount;
        private static int m_QueueAmount;
        private static Item m_QueuePouch;
        private static Item m_QueueStock;
        private static DateTime m_QueueTime;
        private Item m_Regbag;
        private Item m_Stockpile;

        public MakeRegsTargetHandler(int amount, Item stock, Item regbag)
        {
            this.m_Amount = amount;
            this.m_Stockpile = stock;
            this.m_Regbag = regbag;
            if (amount < 1)
            {
                amount = 1;
            }
        }

        public static void CheckQueue(Item item)
        {
            if (((m_QueueStock != null) && (DateTime.Now <= (m_QueueTime + TimeSpan.FromSeconds(2.0)))) && ((item == m_QueueStock) || (item == m_QueuePouch)))
            {
                Transfer(m_QueueStock, m_QueuePouch, m_QueueAmount);
            }
        }

        public void OnCancel(TargetCancelType type)
        {
        }

        public void OnTarget(object o)
        {
            if (o is Item)
            {
                Item item = (Item)o;
                if (!Map.m_ItemFlags[item.ID & 0x3fff][TileFlag.Container])
                {
                    Engine.AddTextMessage("That is not a container.");
                    Engine.TargetHandler = this;
                }
                else
                {
                    if (this.m_Stockpile == null)
                    {
                        this.m_Stockpile = item;
                        World.CharData.Stock = item;
                        this.m_Regbag = World.CharData.RegBag;
                    }
                    else
                    {
                        this.m_Regbag = item;
                        World.CharData.RegBag = item;
                    }
                    if ((this.m_Stockpile != null) && (this.m_Regbag != null))
                    {
                        Transfer(this.m_Stockpile, this.m_Regbag, this.m_Amount);
                    }
                    else
                    {
                        Engine.TargetHandler = this;
                        Engine.AddTextMessage("Target your destination reagent container.");
                    }
                }
            }
            else
            {
                Engine.TargetHandler = this;
            }
        }

        public static void Transfer(Item stock, Item pouch, int amount)
        {
            if (stock.Items.Count == 0)
            {
                Engine.AddTextMessage("Content of reagent stock unknown. Opening...");
                stock.Use();
                m_QueueStock = stock;
                m_QueuePouch = pouch;
                m_QueueAmount = amount;
                m_QueueTime = DateTime.Now;
            }
            else if (pouch.Items.Count == 0)
            {
                Engine.AddTextMessage("Content of reagent pouch unknown. Opening...");
                pouch.Use();
                m_QueueStock = stock;
                m_QueuePouch = pouch;
                m_QueueAmount = amount;
                m_QueueTime = DateTime.Now;
            }
            else
            {
                m_QueueStock = null;
                m_QueuePouch = null;
                m_QueueAmount = 0;
                m_QueueTime = DateTime.MinValue;
                int[] list = ReagentValidator.Validator.List;
                for (int i = 0; i < list.Length; i++)
                {
                    Transfer(stock, pouch, amount, list[i]);
                }
                Engine.PrintQAM();
            }
        }

        public static void Transfer(Item stock, Item pouch, int amount, int itemID)
        {
            Mobile player = World.Player;
            if (player != null)
            {
                Item backpack = player.Backpack;
                if (backpack != null)
                {
                    IItemValidator check = new ItemIDValidator(new int[] { itemID });
                    Item item = pouch.FindItem(check);
                    int num = 0;
                    if ((item != null) && (item.Parent != pouch))
                    {
                        item = null;
                    }
                    bool flag = false;
                    if (item != null)
                    {
                        flag = true;
                    }
                    Item item3 = stock.FindItem(check);
                    foreach (Item item4 in backpack.FindItems(check))
                    {
                        if (item4 != item)
                        {
                            if (item == null)
                            {
                                item = item4;
                                if (item3 == null)
                                {
                                    Engine.QueueAutoMove(item4, item4.Amount, -1, -1, 0, pouch.Serial);
                                }
                                else
                                {
                                    Engine.QueueAutoMove(item4, item4.Amount, item3.ContainerX, item3.ContainerY, 0, pouch.Serial);
                                }
                            }
                            else
                            {
                                Engine.QueueAutoMove(item4, item4.Amount, -1, -1, 0, item.Serial);
                                num += (ushort)item4.Amount;
                            }
                        }
                    }
                    int num3 = num;
                    if (item != null)
                    {
                        num3 += (ushort)item.Amount;
                    }
                    if (num3 > amount)
                    {
                        Engine.QueueAutoMove(item, num3 - amount, -1, -1, 0, stock.Serial);
                    }
                    else if (num3 < amount)
                    {
                        Item[] itemArray2 = stock.FindItems(check);
                        int num4 = amount - num3;
                        for (int i = 0; i < itemArray2.Length; i++)
                        {
                            Item item5 = itemArray2[i];
                            int num6 = num4;
                            if (num6 > ((ushort)item5.Amount))
                            {
                                num6 = (ushort)item5.Amount;
                            }
                            if (item == null)
                            {
                                item = item5;
                                if (item3 == null)
                                {
                                    Engine.QueueAutoMove(item5, num6, -1, -1, 0, pouch.Serial);
                                }
                                else
                                {
                                    Engine.QueueAutoMove(item5, num6, item3.ContainerX, item3.ContainerY, 0, pouch.Serial);
                                }
                            }
                            else
                            {
                                Engine.QueueAutoMove(item5, num6, -1, -1, 0, item.Serial);
                            }
                            num4 -= num6;
                        }
                    }
                    if (((item != null) && flag) && (item3 != null))
                    {
                        Engine.QueueAutoMove(item, amount, item3.ContainerX, item3.ContainerY, 0, pouch.Serial);
                    }
                }
            }
        }
    }
}