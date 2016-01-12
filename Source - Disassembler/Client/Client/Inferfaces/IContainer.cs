namespace Client
{
    public interface IContainer
    {
        void Close();

        void OnItemAdd(Item added);

        void OnItemRefresh(Item refreshed);

        void OnItemRemove(Item removed);

        Client.Gump Gump { get; }
    }
}