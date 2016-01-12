namespace Client.Targeting
{
    using Client;

    public class RemoveTargetHandler : ITargetHandler
    {
        public void OnCancel(TargetCancelType type)
        {
            Engine.AddTextMessage("Remove request canceled.");
        }

        public void OnTarget(object o)
        {
            if (o is Item)
            {
                Item item = (Item)o;
                World.Remove(item);
            }
            else if (o is Mobile)
            {
                Mobile m = (Mobile)o;
                if (!m.Player)
                {
                    World.Remove(m);
                }
            }
            else
            {
                Engine.TargetHandler = this;
            }
        }
    }
}