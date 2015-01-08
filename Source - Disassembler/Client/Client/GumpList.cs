namespace Client
{
    using System;
    using System.Collections;
    using System.Reflection;

    public sealed class GumpList
    {
        private Gump[] m_Array;
        private int m_Count;
        private static Gump[] m_Empty = new Gump[0];
        private ArrayList m_List = new ArrayList(0);
        private Gump m_Owner;

        public GumpList(Gump Owner)
        {
            this.m_Owner = Owner;
            this.m_Array = m_Empty;
        }

        public int Add(Gump ToAdd)
        {
            this.m_Array = null;
            Gumps.Invalidate();
            ToAdd.Parent = this.m_Owner;
            this.m_Count++;
            return this.m_List.Add(ToAdd);
        }

        public void Add(GumpList list)
        {
            Gump[] gumpArray = list.ToArray();
            for (int i = 0; i < gumpArray.Length; i++)
            {
                gumpArray[i].Parent = this.m_Owner;
                this.m_List.Add(gumpArray[i]);
            }
            this.m_Array = null;
            this.m_Count = this.m_List.Count;
            Gumps.Invalidate();
        }

        public void Clear()
        {
            while (this.m_List.Count > 0)
            {
                Gump g = (Gump) this.m_List[0];
                Gumps.Destroy(g);
                this.m_List.Remove(g);
            }
            this.m_Count = 0;
            this.m_Array = null;
        }

        public IEnumerator GetEnumerator()
        {
            return this.m_List.GetEnumerator();
        }

        public IEnumerator GetEnumerator(int index, int count)
        {
            return this.m_List.GetEnumerator(index, count);
        }

        public int IndexOf(Gump Child)
        {
            return this.m_List.IndexOf(Child);
        }

        public void Insert(int Index, Gump ToAdd)
        {
            this.m_Array = null;
            Gumps.Invalidate();
            ToAdd.Parent = this.m_Owner;
            if ((Index >= 0) && (Index < this.m_List.Count))
            {
                this.m_List.Insert(Index, ToAdd);
            }
            else
            {
                this.m_List.Add(ToAdd);
            }
            this.m_Count++;
        }

        public void Remove(Gump ToRemove)
        {
            this.m_Array = null;
            this.m_List.Remove(ToRemove);
            Gumps.Invalidate();
            this.m_Count = this.m_List.Count;
        }

        public void RemoveAt(int index)
        {
            this.m_Array = null;
            this.m_List.RemoveAt(index);
            Gumps.Invalidate();
            this.m_Count--;
        }

        public void Set(GumpList g)
        {
            this.m_List = new ArrayList(g.m_List);
            Gump[] gumpArray = this.ToArray();
            for (int i = 0; i < gumpArray.Length; i++)
            {
                gumpArray[i].Parent = this.m_Owner;
            }
            this.m_Array = null;
            this.m_Count = this.m_List.Count;
            Gumps.Invalidate();
        }

        public Gump[] ToArray()
        {
            if (this.m_Array == null)
            {
                if (this.m_Count == 0)
                {
                    this.m_Array = m_Empty;
                }
                else
                {
                    this.m_Array = (Gump[]) this.m_List.ToArray(typeof(Gump));
                }
            }
            return this.m_Array;
        }

        public int Count
        {
            get
            {
                return this.m_Count;
            }
        }

        public Gump this[int index]
        {
            get
            {
                return (Gump) this.m_List[index];
            }
        }
    }
}

