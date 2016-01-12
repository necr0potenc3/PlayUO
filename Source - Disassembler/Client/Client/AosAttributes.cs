namespace Client
{
    public class AosAttributes
    {
        private ObjectPropertyList m_Props;

        public AosAttributes(ObjectPropertyList props)
        {
            this.m_Props = props;
        }

        public int GetAttribute(AosAttribute attr)
        {
            foreach (ObjectProperty property in this.m_Props.Properties)
            {
                if (property.Number == (int)attr)
                {
                    string arguments = property.Arguments;
                    switch (arguments)
                    {
                        case "":
                        case " ":
                            return 1;
                    }
                    try
                    {
                        return int.Parse(arguments);
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }
            return 0;
        }

        public int GetMaxSkillBonus()
        {
            int num = 0;
            for (int i = 0; i < this.m_Props.Properties.Length; i++)
            {
                ObjectProperty property = this.m_Props.Properties[i];
                if ((property.Number >= 0x102e63) && (property.Number <= 0x102e68))
                {
                    string[] strArray = property.Arguments.Trim().Split(new char[] { '\t' });
                    if (strArray.Length == 2)
                    {
                        int num3 = 0;
                        try
                        {
                            num3 = int.Parse(strArray[1]);
                        }
                        catch
                        {
                        }
                        if (num3 > num)
                        {
                            num = num3;
                        }
                    }
                }
            }
            return num;
        }

        public int GetSkillBonus(SkillName name)
        {
            for (int i = 0; i < this.m_Props.Properties.Length; i++)
            {
                ObjectProperty property = this.m_Props.Properties[i];
                if ((property.Number >= 0x102e63) && (property.Number <= 0x102e68))
                {
                    string[] strArray = property.Arguments.Trim().Split(new char[] { '\t' });
                    if ((strArray.Length == 2) && (strArray[0] == string.Format("#{0}", 0xfee5c + name)))
                    {
                        try
                        {
                            return int.Parse(strArray[1]);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return 0;
        }

        public int ArtifactRarity
        {
            get
            {
                return this.GetAttribute(AosAttribute.ArtifactRarity);
            }
        }

        public int ColdResist
        {
            get
            {
                return this.GetAttribute(AosAttribute.ColdResist);
            }
        }

        public int DamageIncrease
        {
            get
            {
                return this.GetAttribute(AosAttribute.DamageIncrease);
            }
        }

        public int DamageIncrease2
        {
            get
            {
                return this.GetAttribute(AosAttribute.DamageIncrease2);
            }
        }

        public int DefenseChanceIncrease
        {
            get
            {
                return this.GetAttribute(AosAttribute.DefenseChanceIncrease);
            }
        }

        public int DexterityBonus
        {
            get
            {
                return this.GetAttribute(AosAttribute.DexterityBonus);
            }
        }

        public int EnergyResist
        {
            get
            {
                return this.GetAttribute(AosAttribute.EnergyResist);
            }
        }

        public int EnhancePotions
        {
            get
            {
                return this.GetAttribute(AosAttribute.EnhancePotions);
            }
        }

        public int FasterCasting
        {
            get
            {
                return this.GetAttribute(AosAttribute.FasterCasting);
            }
        }

        public int FasterCastRecovery
        {
            get
            {
                return this.GetAttribute(AosAttribute.FasterCastRecovery);
            }
        }

        public int FireResist
        {
            get
            {
                return this.GetAttribute(AosAttribute.FireResist);
            }
        }

        public int HitChanceIncrease
        {
            get
            {
                return this.GetAttribute(AosAttribute.HitChanceIncrease);
            }
        }

        public int HitDispel
        {
            get
            {
                return this.GetAttribute(AosAttribute.HitDispel);
            }
        }

        public int HitFireball
        {
            get
            {
                return this.GetAttribute(AosAttribute.HitFireball);
            }
        }

        public int HitHarm
        {
            get
            {
                return this.GetAttribute(AosAttribute.HitHarm);
            }
        }

        public int HitLightning
        {
            get
            {
                return this.GetAttribute(AosAttribute.HitLightning);
            }
        }

        public int HitMagicArrow
        {
            get
            {
                return this.GetAttribute(AosAttribute.HitMagicArrow);
            }
        }

        public int HitPointIncrease
        {
            get
            {
                return this.GetAttribute(AosAttribute.HitPointIncrease);
            }
        }

        public int HitPointRegeneration
        {
            get
            {
                return this.GetAttribute(AosAttribute.HitPointRegeneration);
            }
        }

        public int IntelligenceBonus
        {
            get
            {
                return this.GetAttribute(AosAttribute.IntelligenceBonus);
            }
        }

        public int LowerManaCost
        {
            get
            {
                return this.GetAttribute(AosAttribute.LowerManaCost);
            }
        }

        public int LowerReagentCost
        {
            get
            {
                return this.GetAttribute(AosAttribute.LowerReagentCost);
            }
        }

        public int Luck
        {
            get
            {
                return this.GetAttribute(AosAttribute.Luck);
            }
        }

        public int ManaIncrease
        {
            get
            {
                return this.GetAttribute(AosAttribute.ManaIncrease);
            }
        }

        public int ManaRegeneration
        {
            get
            {
                return this.GetAttribute(AosAttribute.ManaRegeneration);
            }
        }

        public int PhysicalResist
        {
            get
            {
                return this.GetAttribute(AosAttribute.PhysicalResist);
            }
        }

        public int PoisonResist
        {
            get
            {
                return this.GetAttribute(AosAttribute.PoisonResist);
            }
        }

        public ObjectPropertyList Props
        {
            get
            {
                return this.m_Props;
            }
        }

        public int ReflectPhysicalDamage
        {
            get
            {
                return this.GetAttribute(AosAttribute.ReflectPhysicalDamage);
            }
        }

        public int SelfRepair
        {
            get
            {
                return this.GetAttribute(AosAttribute.SelfRepair);
            }
        }

        public int SpellChanneling
        {
            get
            {
                return this.GetAttribute(AosAttribute.SpellChanneling);
            }
        }

        public int SpellDamageIncrease
        {
            get
            {
                return this.GetAttribute(AosAttribute.SpellDamageIncrease);
            }
        }

        public int StaminaIncrease
        {
            get
            {
                return this.GetAttribute(AosAttribute.StaminaIncrease);
            }
        }

        public int StaminaRegeneration
        {
            get
            {
                return this.GetAttribute(AosAttribute.StaminaRegeneration);
            }
        }

        public int StrengthBonus
        {
            get
            {
                return this.GetAttribute(AosAttribute.StrengthBonus);
            }
        }

        public int SwingSpeedIncrease
        {
            get
            {
                return this.GetAttribute(AosAttribute.SwingSpeedIncrease);
            }
        }
    }
}