namespace Client
{
    using System;

    public interface IHintable
    {
        bool HintItem(int ItemID);
        bool HintLand(int LandID);
    }
}

