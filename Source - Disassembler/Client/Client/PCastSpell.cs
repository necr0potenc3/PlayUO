namespace Client
{
    using System;

    public class PCastSpell : Packet
    {
        private static int m_LastSpellID = -1;

        public PCastSpell(int SpellID) : base(0x12, "Cast Spell")
        {
            m_LastSpellID = SpellID;
            base.m_Stream.Write((byte) 0x56);
            base.m_Stream.Write(SpellID.ToString());
            base.m_Stream.Write((byte) 0);
        }

        public static void SendLast()
        {
            if (m_LastSpellID >= 0)
            {
                Network.Send(new PCastSpell(m_LastSpellID));
            }
        }
    }
}

