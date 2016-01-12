namespace Client
{
    public class GServerTextBox : GTextBox
    {
        private int m_RelayID;

        public GServerTextBox(string initialText, LayoutEntry le) : base(0, false, le[0], le[1], le[2], le[3], initialText, Engine.GetUniFont(1), Hues.Load(le[4] + 1), Hues.Load(le[4] + 1), Hues.Load(le[4] + 1))
        {
            this.m_RelayID = le[5];
            base.MaxChars = 0xef;
        }

        public int RelayID
        {
            get
            {
                return this.m_RelayID;
            }
        }
    }
}