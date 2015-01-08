namespace Client
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Reagent
    {
        private string m_Name;
        private int m_ItemID;
        public Reagent(string Name)
        {
            this.m_Name = Name;
            this.m_ItemID = Spells.GetReagent(Name).ItemID;
        }

        public Reagent(string Name, int ItemID)
        {
            this.m_Name = Name;
            this.m_ItemID = ItemID;
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }
        public int ItemID
        {
            get
            {
                return this.m_ItemID;
            }
        }
    }
}

