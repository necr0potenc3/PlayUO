namespace Client
{
    using System;
    using System.Collections;

    public sealed class NameSorter : IComparer
    {
        public static readonly IComparer Comparer = new NameSorter();

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
            Mobile mobile = x as Mobile;
            Mobile mobile2 = y as Mobile;
            if ((mobile == null) || (mobile2 == null))
            {
                throw new ArgumentException();
            }
            bool human = mobile.Human;
            bool flag2 = mobile2.Human;
            if (human && !flag2)
            {
                return -1;
            }
            if (flag2 && !human)
            {
                return 1;
            }
            return mobile.Serial.CompareTo(mobile2.Serial);
        }
    }
}

