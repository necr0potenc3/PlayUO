namespace Client
{
    using System;

    public interface IAnimationOwner
    {
        Frames GetOwnedFrames(IHue hue, int realID);
    }
}

