namespace Client
{
    public class ItemIDValidator : IItemValidator
    {
        private int[] m_List;
        private IItemValidator m_Parent;

        public ItemIDValidator(params int[] list) : this(null, list)
        {
        }

        public ItemIDValidator(IItemValidator parent, params int[] list)
        {
            this.m_Parent = parent;
            this.m_List = list;
        }

        public bool IsValid(Item check)
        {
            if ((this.m_Parent == null) || this.m_Parent.IsValid(check))
            {
                if ((this.m_List == null) || (this.m_List.Length <= 0))
                {
                    return false;
                }
                int num = check.ID & 0x3fff;
                for (int i = 0; i < this.m_List.Length; i++)
                {
                    if (this.m_List[i] == num)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public int[] List
        {
            get
            {
                return this.m_List;
            }
        }
    }
}