namespace Client
{
    using System;

    public class ArtifactValidator : IItemValidator
    {
        private IItemValidator m_Parent;

        public ArtifactValidator() : this(null)
        {
        }

        public ArtifactValidator(IItemValidator parent)
        {
            this.m_Parent = parent;
        }

        public bool IsValid(Item check)
        {
            if ((this.m_Parent == null) || this.m_Parent.IsValid(check))
            {
                ObjectPropertyList propertyList = check.PropertyList;
                if (propertyList == null)
                {
                    return false;
                }
                for (int i = 0; i < propertyList.Properties.Length; i++)
                {
                    if (propertyList.Properties[i].Number == 0x1030d6)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

