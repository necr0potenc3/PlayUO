namespace Client.Targeting
{
    using Client;

    public class FriendTargetHandler : ITargetHandler
    {
        public void OnCancel(TargetCancelType type)
        {
            Engine.AddTextMessage("Friend request canceled.");
        }

        public void OnTarget(object o)
        {
            if (o is Mobile)
            {
                CharData charData = World.CharData;
                if (charData != null)
                {
                    Mobile item = (Mobile)o;
                    if (charData.Friends.Contains(item))
                    {
                        charData.Friends.Remove(item);
                        item.m_IsFriend = false;
                        Engine.AddTextMessage("They have been removed from the friends list.", Engine.DefaultFont, Hues.Load(0x22));
                    }
                    else
                    {
                        charData.Friends.Add(item);
                        item.m_IsFriend = true;
                        Engine.AddTextMessage("They have been added to the friends list.", Engine.DefaultFont, Hues.Load(0x59));
                    }
                    charData.Save();
                }
            }
            else
            {
                Engine.TargetHandler = this;
            }
        }
    }
}