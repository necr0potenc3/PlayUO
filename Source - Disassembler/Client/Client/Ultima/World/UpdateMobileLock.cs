namespace Client
{
    public class UpdateMobileLock : ILocked
    {
        private Mobile m_Mobile;

        public UpdateMobileLock(Mobile mobile)
        {
            this.m_Mobile = mobile;
        }

        public void Invoke()
        {
            Map.UpdateMobile(this.m_Mobile);
        }
    }
}