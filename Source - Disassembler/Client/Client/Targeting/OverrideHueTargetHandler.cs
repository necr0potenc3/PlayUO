namespace Client.Targeting
{
    using Client;

    public class OverrideHueTargetHandler : ITargetHandler
    {
        private string m_CancelMessage;
        private int m_Hue;

        public OverrideHueTargetHandler(int hue, string cancelMessage)
        {
            this.m_Hue = hue;
            this.m_CancelMessage = cancelMessage;
        }

        public void OnCancel(TargetCancelType type)
        {
            Engine.AddTextMessage(this.m_CancelMessage);
        }

        public void OnTarget(object o)
        {
            if (o is Item)
            {
                Item removed = (Item)o;
                removed.OverrideHue(this.m_Hue);
                if ((removed.Parent != null) && (removed.Parent.Container != null))
                {
                    removed.Parent.Container.OnItemRemove(removed);
                    removed.Parent.Container.OnItemAdd(removed);
                }
                else if (removed.InWorld)
                {
                    removed.Update();
                }
                else if (removed.IsEquip && (removed.EquipParent is Mobile))
                {
                    Mobile equipParent = (Mobile)removed.EquipParent;
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