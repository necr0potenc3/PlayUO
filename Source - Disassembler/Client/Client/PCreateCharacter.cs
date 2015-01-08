namespace Client
{
    using System;

    public class PCreateCharacter : Packet
    {
        public PCreateCharacter(string Name, byte Gender, byte Strength, byte Dexterity, byte Intelligence, byte iSkill1, byte vSkill1, byte iSkill2, byte vSkill2, byte iSkill3, byte vSkill3, short hSkinTone, short HairStyle, short hHairColor, short FacialHairStyle, short hFacialHairColor, short CityID, short CharSlot, int ClientIP, short hShirtColor, short hPantsColor) : base(0, "Create Character", NewConfig.OldCharCreate ? 100 : 0x68)
        {
            if (NewConfig.OldCharCreate)
            {
                double num = ((double) (Strength - 10)) / 50.0;
                double num2 = ((double) (Dexterity - 10)) / 50.0;
                double num3 = ((double) (Intelligence - 10)) / 50.0;
                Strength = (byte) (10.0 + (num * 30.0));
                Dexterity = (byte) (10.0 + (num2 * 30.0));
                Intelligence = (byte) (10.0 + (num3 * 30.0));
                if (((Strength + Dexterity) + Intelligence) > 60)
                {
                    while (((Strength + Dexterity) + Intelligence) > 60)
                    {
                        if ((Strength > 10) && (((Strength + Dexterity) + Intelligence) > 60))
                        {
                            Strength = (byte) (Strength - 1);
                        }
                        if ((Dexterity > 10) && (((Strength + Dexterity) + Intelligence) > 60))
                        {
                            Dexterity = (byte) (Dexterity - 1);
                        }
                        if ((Intelligence > 10) && (((Strength + Dexterity) + Intelligence) > 60))
                        {
                            Intelligence = (byte) (Intelligence - 1);
                        }
                    }
                }
                else if (((Strength + Dexterity) + Intelligence) < 60)
                {
                    while (((Strength + Dexterity) + Intelligence) < 60)
                    {
                        if ((Strength < 40) && (((Strength + Dexterity) + Intelligence) < 60))
                        {
                            Strength = (byte) (Strength + 1);
                        }
                        if ((Dexterity < 40) && (((Strength + Dexterity) + Intelligence) < 60))
                        {
                            Dexterity = (byte) (Dexterity + 1);
                        }
                        if ((Intelligence < 40) && (((Strength + Dexterity) + Intelligence) < 60))
                        {
                            Intelligence = (byte) (Intelligence + 1);
                        }
                    }
                }
            }
            base.m_Stream.Write(-303174163);
            base.m_Stream.Write(-1);
            base.m_Stream.Write((byte) 0);
            base.m_Stream.Write(Name, 30);
            base.m_Stream.Write("", 30);
            base.m_Stream.Write(Gender);
            base.m_Stream.Write(Strength);
            base.m_Stream.Write(Dexterity);
            base.m_Stream.Write(Intelligence);
            base.m_Stream.Write(iSkill1);
            base.m_Stream.Write(vSkill1);
            base.m_Stream.Write(iSkill2);
            base.m_Stream.Write(vSkill2);
            base.m_Stream.Write(iSkill3);
            base.m_Stream.Write(vSkill3);
            base.m_Stream.Write(hSkinTone);
            base.m_Stream.Write(HairStyle);
            base.m_Stream.Write(hHairColor);
            base.m_Stream.Write(FacialHairStyle);
            base.m_Stream.Write(hFacialHairColor);
            base.m_Stream.Write(CityID);
            base.m_Stream.Write((int) CharSlot);
            base.m_Stream.Write(ClientIP);
            if (!NewConfig.OldCharCreate)
            {
                base.m_Stream.Write(hShirtColor);
                base.m_Stream.Write(hPantsColor);
            }
        }
    }
}

