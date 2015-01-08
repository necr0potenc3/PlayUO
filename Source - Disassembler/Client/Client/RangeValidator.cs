namespace Client
{
    using System;

    public class RangeValidator : IItemValidator
    {
        private IItemValidator m_Parent;
        private IPoint2D m_Point;
        private int m_xyRange;

        public RangeValidator(IPoint2D point, int xyRange) : this(null, point, xyRange)
        {
        }

        public RangeValidator(IItemValidator parent, IPoint2D point, int xyRange)
        {
            this.m_Point = point;
            this.m_Parent = parent;
            this.m_xyRange = xyRange;
        }

        public bool IsValid(Item check)
        {
            return (((this.m_Parent == null) || this.m_Parent.IsValid(check)) && check.InSquareRange(this.m_Point, this.m_xyRange));
        }
    }
}

