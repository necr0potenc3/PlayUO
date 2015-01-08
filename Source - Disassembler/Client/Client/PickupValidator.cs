namespace Client
{
    using System;

    public class PickupValidator : IItemValidator
    {
        private IItemValidator m_Parent;

        public PickupValidator() : this(null)
        {
        }

        public PickupValidator(IItemValidator parent)
        {
            this.m_Parent = parent;
        }

        public bool IsValid(Item check)
        {
            if ((this.m_Parent != null) && !this.m_Parent.IsValid(check))
            {
                return false;
            }
            return ((Map.GetWeight(check.ID) < 0xff) || check.Flags[ItemFlag.CanMove]);
        }
    }
}

