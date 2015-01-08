namespace Client.Targeting
{
    using Client;
    using System;

    public class NullTargetHandler : ITargetHandler
    {
        public void OnCancel(TargetCancelType type)
        {
        }

        public void OnTarget(object o)
        {
            if (o is Mobile)
            {
                ((Mobile) o).AddTextMessage("", "Last target set.", Engine.DefaultFont, Hues.Load(0x59), false);
                if (((Party.State == PartyState.Joined) && (((Mobile) o).Name != null)) && (((Mobile) o).Name.Length > 0))
                {
                    Network.Send(new PParty_PublicMessage("Changing last target to " + ((Mobile) o).Name));
                }
            }
            else if (o is Item)
            {
                ((Item) o).AddTextMessage("", "Last target set.", Engine.DefaultFont, Hues.Load(0x59), false);
            }
            else
            {
                Engine.AddTextMessage("Last target set.", Engine.DefaultFont, Hues.Load(0x59));
            }
        }
    }
}

