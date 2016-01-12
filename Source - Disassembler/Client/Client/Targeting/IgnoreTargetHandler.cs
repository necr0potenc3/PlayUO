namespace Client.Targeting
{
    using Client;

    public class IgnoreTargetHandler : ITargetHandler
    {
        public void OnCancel(TargetCancelType type)
        {
            Engine.AddTextMessage("Ignore request canceled.");
        }

        public void OnTarget(object o)
        {
            if (o is Mobile)
            {
                Mobile mobile = (Mobile)o;
                mobile.Ignored = !mobile.Ignored;
                string name = mobile.Name;
                string str2 = mobile.Ignored ? "now being ignored." : "no longer being ignored.";
                if (((name != null) && ((name = name.Trim()).Length > 0)) && mobile.HumanOrGhost)
                {
                    Engine.AddTextMessage(string.Format("{0} is {1}", name, str2));
                }
                else
                {
                    Engine.AddTextMessage(string.Format("{0} {1}", mobile.HumanOrGhost ? "They are" : "It is"));
                }
            }
            else
            {
                Engine.TargetHandler = this;
            }
        }
    }
}