namespace Client
{
    using System.Collections;

    public class EquipManager
    {
        private Hashtable m_AutoEquip = new Hashtable();

        public void Dequip()
        {
            Mobile player = World.Player;
            if (player != null)
            {
                if (player.Ghost)
                {
                    Engine.AddTextMessage("You are dead.");
                }
                else if ((Gumps.Drag != null) && (Gumps.Drag.GetType() == typeof(GDraggedItem)))
                {
                    Engine.AddTextMessage("You are already dragging an item.");
                }
                else
                {
                    Item item = player.FindEquip(Layer.TwoHanded);
                    if (item == null)
                    {
                        item = player.FindEquip(Layer.OneHanded);
                    }
                    if (item == null)
                    {
                        Engine.AddTextMessage("You are not holding anything.");
                    }
                    else
                    {
                        Network.Send(new PPickupItem(item, item.Amount));
                        Network.Send(new PDropItem(item.Serial, -1, -1, 0, player.Serial));
                    }
                }
            }
        }

        public void Equip(int index)
        {
            Mobile player = World.Player;
            if (player != null)
            {
                if (player.Ghost)
                {
                    Engine.AddTextMessage("You are dead.");
                }
                else if ((Gumps.Drag != null) && (Gumps.Drag.GetType() == typeof(GDraggedItem)))
                {
                    Engine.AddTextMessage("You are already dragging an item.");
                }
                else
                {
                    object obj2 = this.m_AutoEquip[index];
                    if (obj2 != null)
                    {
                        Item check = World.FindItem((int)obj2);
                        if (check == null)
                        {
                            Engine.AddTextMessage("Equipment not found.");
                        }
                        else if (!check.IsEquip || !player.HasEquip(check))
                        {
                            Network.Send(new PPickupItem(check, check.Amount));
                            Network.Send(new PEquipItem(check, World.Player));
                        }
                    }
                }
            }
        }

        public Hashtable AutoEquip
        {
            get
            {
                return this.m_AutoEquip;
            }
        }
    }
}