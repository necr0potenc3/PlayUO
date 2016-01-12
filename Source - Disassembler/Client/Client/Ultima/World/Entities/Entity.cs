namespace Client
{
    public sealed class Entity : IEntity
    {
        private int m_Serial;

        public Entity(int serial)
        {
            this.m_Serial = serial;
        }

        public static implicit operator Entity(int serial)
        {
            return new Entity(serial);
        }

        public int Serial
        {
            get
            {
                return this.m_Serial;
            }
        }
    }
}