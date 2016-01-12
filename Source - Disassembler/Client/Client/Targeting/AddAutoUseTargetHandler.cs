namespace Client.Targeting
{
    using Client;

    public class AddAutoUseTargetHandler : ITargetHandler
    {
        public void OnCancel(TargetCancelType type)
        {
            Engine.AddTextMessage("Request to add a use-once item canceled.");
        }

        public void OnTarget(object o)
        {
            if (o is Item)
            {
                Item item = (Item)o;
                if (World.CharData.AutoUse.Contains(item))
                {
                    item.OverrideHue(-1);
                    World.CharData.AutoUse.Remove(item);
                }
                else
                {
                    item.OverrideHue(0x22);
                    World.CharData.AutoUse.Add(item);
                }
                World.CharData.Save();
                if ((item.Parent != null) && (item.Parent.Container != null))
                {
                    item.Parent.Container.OnItemRemove(item);
                    item.Parent.Container.OnItemAdd(item);
                }
                else if (item.InWorld)
                {
                    item.Update();
                }
                else if (item.IsEquip && (item.EquipParent is Mobile))
                {
                    Mobile equipParent = (Mobile)item.EquipParent;
                    if (equipParent.Paperdoll != null)
                    {
                        Gumps.OpenPaperdoll(equipParent, equipParent.PaperdollName, equipParent.PaperdollCanDrag);
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