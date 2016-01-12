namespace Client
{
    public interface IAnimationOwner
    {
        Frames GetOwnedFrames(IHue hue, int realID);
    }
}