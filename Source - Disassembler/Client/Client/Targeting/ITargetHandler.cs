namespace Client.Targeting
{
    public interface ITargetHandler
    {
        void OnCancel(TargetCancelType why);

        void OnTarget(object targeted);
    }
}