namespace Client
{
    using System;

    public class ReagentValidator : ItemIDValidator
    {
        public static readonly ReagentValidator Validator = new ReagentValidator();

        public ReagentValidator() : this(null)
        {
        }

        public ReagentValidator(IItemValidator parent) : base(parent, new int[] { 0xf7a, 0xf7b, 0xf84, 0xf85, 0xf86, 0xf88, 0xf8c, 0xf8d })
        {
        }
    }
}

