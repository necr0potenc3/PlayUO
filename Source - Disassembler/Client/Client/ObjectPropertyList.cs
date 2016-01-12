namespace Client
{
    using System.Collections;

    public class ObjectPropertyList
    {
        private int m_Number;
        private ObjectProperty[] m_Props;
        private int m_Serial;
        private static Hashtable m_Table = new Hashtable();

        public ObjectPropertyList(int serial, int number, ObjectProperty[] props)
        {
            this.m_Serial = serial;
            this.m_Number = number;
            this.m_Props = props;
            m_Table[GetKey(serial, number)] = this;
        }

        public static ObjectPropertyList Find(int serial, int number)
        {
            return (ObjectPropertyList)m_Table[GetKey(serial, number)];
        }

        public static long GetKey(int serial, int number)
        {
            return ((serial << 0x20) | ((long)((ulong)number)));
        }

        public int Number
        {
            get
            {
                return this.m_Number;
            }
        }

        public ObjectProperty[] Properties
        {
            get
            {
                return this.m_Props;
            }
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