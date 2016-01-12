namespace Client.Targeting
{
    using Client;

    public class DragToBagTargetHandler : ITargetHandler
    {
        private bool m_Click;

        public DragToBagTargetHandler(bool click)
        {
            this.m_Click = click;
        }

        public void OnCancel(TargetCancelType type)
        {
            Engine.AddTextMessage("Drag request canceled.");
        }

        public void OnTarget(object o)
        {
            if (o is Item)
            {
                Item e = (Item)o;
                Mobile player = World.Player;
                if ((player != null) && ((Map.GetWeight(e.ID) != 0xff) || e.Flags[ItemFlag.CanMove]))
                {
                    if (this.m_Click)
                    {
                        Network.Send(new PLookRequest(e));
                    }
                    Item regBag = null;
                    if (ReagentValidator.Validator.IsValid(e))
                    {
                        regBag = World.CharData.RegBag;
                    }
                    if (regBag == null)
                    {
                        regBag = player.Backpack;
                    }
                    if (regBag != null)
                    {
                        Engine.QueueAutoMove(e, e.Amount, -1, -1, 0, regBag.Serial);
                        Engine.PrintQAM();
                    }
                    else
                    {
                        Engine.AddTextMessage("Unable to find target container.");
                    }
                    return;
                }
            }
            Engine.AddTextMessage(PacketHandlers.IPFReason[0], Engine.GetFont(3), Hues.Default);
        }
    }
}