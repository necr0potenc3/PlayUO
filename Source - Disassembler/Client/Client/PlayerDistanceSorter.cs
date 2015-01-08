namespace Client
{
    using System;
    using System.Collections;

    public sealed class PlayerDistanceSorter : IComparer
    {
        public static readonly IComparer Comparer = new PlayerDistanceSorter();

        public int Compare(object x, object y)
        {
            if ((x == null) && (y == null))
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            IPoint2D p = x as IPoint2D;
            IPoint2D pointd2 = y as IPoint2D;
            if ((p == null) || (pointd2 == null))
            {
                throw new ArgumentException();
            }
            Mobile player = World.Player;
            return player.DistanceSqrt(p).CompareTo(player.DistanceSqrt(pointd2));
        }
    }
}

