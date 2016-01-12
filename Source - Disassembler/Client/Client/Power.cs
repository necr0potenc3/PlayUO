namespace Client
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Power
    {
        private string m_Name;
        private char m_Symbol;

        public Power(string Name)
        {
            this.m_Name = Name;
            if (Name.Length > 0)
            {
                this.m_Symbol = Name[0];
            }
            else
            {
                this.m_Symbol = '\0';
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }

        public char Symbol
        {
            get
            {
                return this.m_Symbol;
            }
        }

        public static Power[] Parse(string Words)
        {
            string[] strArray = Words.Split(new char[] { ' ' });
            int length = strArray.Length;
            Power[] powerArray = new Power[length];
            for (int i = 0; i < length; i++)
            {
                powerArray[i] = new Power(strArray[i]);
            }
            return powerArray;
        }
    }
}