namespace Client
{
    using System;

    public class PlayerDistanceValidator : IItemValidator
    {
        private IItemValidator m_Parent;
        private int m_Range;

        public PlayerDistanceValidator(int range) : this(null, range)
        {
        }

        public PlayerDistanceValidator(IItemValidator parent, int range)
        {
            this.m_Parent = parent;
            this.m_Range = range;
        }

        public bool IsValid(Item check)
        {
            return (((this.m_Parent == null) || this.m_Parent.IsValid(check)) && check.InSquareRange(World.Player, this.m_Range));
        }
    }
}

