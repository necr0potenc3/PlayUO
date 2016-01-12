namespace Client
{
    public abstract class AosAppraiser
    {
        private int[] m_Appraisal;
        public static bool m_BlueCorpse;

        protected AosAppraiser()
        {
        }

        protected void AddWorth(int val)
        {
            if (this.m_Appraisal == null)
            {
                this.m_Appraisal = new int[0];
            }
            int[] appraisal = this.m_Appraisal;
            this.m_Appraisal = new int[appraisal.Length + 1];
            for (int i = 0; i < appraisal.Length; i++)
            {
                this.m_Appraisal[i] = appraisal[i];
            }
            this.m_Appraisal[appraisal.Length] = val;
        }

        public Appraisal Appraise(Item item)
        {
            this.m_Appraisal = new int[0];
            ObjectPropertyList propertyList = item.PropertyList;
            if (propertyList != null)
            {
                AosAttributes attrs = new AosAttributes(propertyList);
                this.DoAppraise(item, attrs);
            }
            return new Appraisal(item, this.m_Appraisal);
        }

        protected abstract void DoAppraise(Item item, AosAttributes attrs);
    }
}