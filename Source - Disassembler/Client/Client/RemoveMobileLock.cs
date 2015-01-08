namespace Client
{
    using System;

    public class RemoveMobileLock : ILocked
    {
        private Mobile m_Mobile;

        public RemoveMobileLock(Mobile mobile)
        {
            this.m_Mobile = mobile;
        }

        public void Invoke()
        {
            Map.RemoveMobile(this.m_Mobile);
        }
    }
}

