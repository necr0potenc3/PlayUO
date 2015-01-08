namespace Client
{
    using System;
    using System.Collections;

    public sealed class TargetSorter : IComparer
    {
        public static readonly IComparer Comparer = new TargetSorter();

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
            bool flag = (x is Mobile) && ((Mobile) x).Human;
            bool flag2 = (y is Mobile) && ((Mobile) y).Human;
            if (flag && !flag2)
            {
                return -1;
            }
            if (flag2 && !flag)
            {
                return 1;
            }
            Mobile player = World.Player;
            return player.DistanceSqrt(p).CompareTo(player.DistanceSqrt(pointd2));
        }
    }
}

