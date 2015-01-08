namespace Client
{
    using System;
    using System.Collections;

    public class CategorySorter : IComparer
    {
        public int Compare(object x, object y)
        {
            DictionaryEntry entry = (DictionaryEntry) x;
            DictionaryEntry entry2 = (DictionaryEntry) y;
            return ((string) entry.Key).CompareTo((string) entry2.Key);
        }
    }
}

