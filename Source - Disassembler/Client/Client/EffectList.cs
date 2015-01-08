namespace Client
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class EffectList : IEnumerable
    {
        private ArrayList m_List = new ArrayList(0);

        public int Add(Effect ToAdd)
        {
            return this.m_List.Add(ToAdd);
        }

        public void Clear()
        {
            this.m_List.Clear();
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_List.GetEnumerator();
        }

        public int IndexOf(Effect Child)
        {
            return this.m_List.IndexOf(Child);
        }

        public void Insert(int Index, Effect ToAdd)
        {
            if ((Index >= 0) && (Index < this.m_List.Count))
            {
                this.m_List.Insert(Index, ToAdd);
            }
            else
            {
                this.m_List.Add(ToAdd);
            }
        }

        public void Remove(Effect ToRemove)
        {
            this.m_List.Remove(ToRemove);
        }

        public void RemoveAt(int Index)
        {
            this.m_List.RemoveAt(Index);
        }

        public int Count
        {
            get
            {
                return this.m_List.Count;
            }
        }

        public Effect this[int Index]
        {
            get
            {
                return (Effect) this.m_List[Index];
            }
        }
    }
}

