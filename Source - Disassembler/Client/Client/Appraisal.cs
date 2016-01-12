namespace Client
{
    using System;

    public class Appraisal : IComparable
    {
        private Client.Item m_Item;
        private int[] m_Worth;

        public Appraisal(Client.Item item, int[] worth)
        {
            this.m_Item = item;
            this.m_Worth = worth;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return -1;
            }
            Appraisal appraisal = (Appraisal)obj;
            int[] worth = this.m_Worth;
            int[] numArray2 = appraisal.m_Worth;
            int num = 0;
            for (int i = 0; ((num == 0) && (i < worth.Length)) && (i < numArray2.Length); i++)
            {
                num = numArray2[i] - worth[i];
            }
            if (num == 0)
            {
                num = numArray2.Length - worth.Length;
            }
            return num;
        }

        public bool IsWorthless
        {
            get
            {
                bool flag = true;
                for (int i = 0; flag && (i < this.m_Worth.Length); i++)
                {
                    flag = this.m_Worth[i] == 0;
                }
                return flag;
            }
        }

        public Client.Item Item
        {
            get
            {
                return this.m_Item;
            }
        }

        public int[] Worth
        {
            get
            {
                return this.m_Worth;
            }
        }
    }
}