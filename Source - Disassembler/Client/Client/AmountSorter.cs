namespace Client
{
    using System;
    using System.Collections;

    public sealed class AmountSorter : IComparer
    {
        public static readonly IComparer Comparer = new AmountSorter();

        private AmountSorter()
        {
        }

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
            Item item = x as Item;
            Item item2 = y as Item;
            if ((item == null) || (item2 == null))
            {
                throw new ArgumentException();
            }
            ushort amount = (ushort) item.Amount;
            return amount.CompareTo((ushort) item2.Amount);
        }
    }
}

