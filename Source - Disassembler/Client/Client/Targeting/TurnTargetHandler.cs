namespace Client.Targeting
{
    using Client;

    public class TurnTargetHandler : ITargetHandler
    {
        public void OnCancel(TargetCancelType type)
        {
            Engine.AddTextMessage("Turn request canceled.");
        }

        public void OnTarget(object o)
        {
            IPoint3D pointd = o as IPoint3D;
            if (pointd != null)
            {
                Mobile player = World.Player;
                if (player != null)
                {
                    int newDir = Engine.GetWalkDirection(Engine.GetDirection(player.X, player.Y, pointd.X, pointd.Y)) & 7;
                    if ((player.Direction & 7) != newDir)
                    {
                        Engine.EquipSort(player, newDir);
                        player.Direction = (byte)newDir;
                        Engine.SendMovementRequest(newDir, player.X, player.Y, player.Z);
                    }
                }
            }
        }
    }
}