namespace Client
{
    using System;
    using System.Collections;

    public class Spell
    {
        private string m_FullPower;
        private int m_Mana;
        private string m_Name;
        private Client.Power[] m_Power;
        private ArrayList m_Reagents;
        private int m_Skill;
        private int m_SpellID;
        private int m_Tithing;

        public Spell(string name, string power, int spellID)
        {
            this.m_Name = name;
            this.m_Power = Client.Power.Parse(power);
            this.m_FullPower = power;
            this.m_SpellID = spellID;
            this.m_Reagents = new ArrayList();
        }

        public void Cast()
        {
            Network.Send(new PCastSpell(this.m_SpellID));
        }

        public string FullPower
        {
            get
            {
                return this.m_FullPower;
            }
        }

        public int Mana
        {
            get
            {
                return this.m_Mana;
            }
            set
            {
                this.m_Mana = value;
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }

        public Client.Power[] Power
        {
            get
            {
                return this.m_Power;
            }
        }

        public ArrayList Reagents
        {
            get
            {
                return this.m_Reagents;
            }
        }

        public int Skill
        {
            get
            {
                return this.m_Skill;
            }
            set
            {
                this.m_Skill = value;
            }
        }

        public int SpellID
        {
            get
            {
                return this.m_SpellID;
            }
        }

        public int Tithing
        {
            get
            {
                return this.m_Tithing;
            }
            set
            {
                this.m_Tithing = value;
            }
        }
    }
}

