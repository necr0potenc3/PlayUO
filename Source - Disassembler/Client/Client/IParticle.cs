namespace Client
{
    public interface IParticle
    {
        void Destroy();

        void Invalidate();

        bool Offset(int xDelta, int yDelta);

        bool Slice();
    }
}