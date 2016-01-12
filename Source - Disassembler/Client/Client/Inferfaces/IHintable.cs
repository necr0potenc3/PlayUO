namespace Client
{
    public interface IHintable
    {
        bool HintItem(int ItemID);

        bool HintLand(int LandID);
    }
}