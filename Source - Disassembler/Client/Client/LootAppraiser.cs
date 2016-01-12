namespace Client
{
    public class LootAppraiser : AosAppraiser
    {
        public static readonly AosAppraiser Instance = new LootAppraiser();

        private LootAppraiser()
        {
        }

        protected override void DoAppraise(Item item, AosAttributes attrs)
        {
            base.AddWorth(attrs.ArtifactRarity);
            if (attrs.FasterCasting == 1)
            {
                if (attrs.FasterCastRecovery == 3)
                {
                    base.AddWorth(3);
                }
                else if (!AosAppraiser.m_BlueCorpse && (attrs.FasterCastRecovery == 2))
                {
                    base.AddWorth(2);
                }
                else
                {
                    base.AddWorth(0);
                }
            }
            else
            {
                base.AddWorth(0);
            }
            if (!AosAppraiser.m_BlueCorpse)
            {
                if (attrs.DexterityBonus == 8)
                {
                    base.AddWorth(attrs.DexterityBonus);
                }
                else
                {
                    base.AddWorth(0);
                }
                if (attrs.StrengthBonus == 8)
                {
                    base.AddWorth(attrs.StrengthBonus);
                }
                else
                {
                    base.AddWorth(0);
                }
                if (attrs.IntelligenceBonus == 8)
                {
                    base.AddWorth(attrs.IntelligenceBonus);
                }
                else
                {
                    base.AddWorth(0);
                }
                if (attrs.StaminaIncrease == 8)
                {
                    base.AddWorth(attrs.StaminaIncrease);
                }
                else
                {
                    base.AddWorth(0);
                }
                if (attrs.HitPointIncrease == 5)
                {
                    base.AddWorth(attrs.HitPointIncrease);
                }
                else
                {
                    base.AddWorth(0);
                }
                if (attrs.ManaIncrease == 8)
                {
                    base.AddWorth(attrs.ManaIncrease);
                }
                else
                {
                    base.AddWorth(0);
                }
                if (attrs.DefenseChanceIncrease >= 15)
                {
                    base.AddWorth(attrs.DefenseChanceIncrease);
                }
                else
                {
                    base.AddWorth(0);
                }
                if (attrs.HitChanceIncrease >= 15)
                {
                    base.AddWorth(attrs.HitChanceIncrease);
                }
                else
                {
                    base.AddWorth(0);
                }
                if (((attrs.SpellChanneling > 0) && (attrs.FasterCasting >= 0)) && (attrs.DefenseChanceIncrease >= 12))
                {
                    base.AddWorth(attrs.DefenseChanceIncrease);
                }
                else
                {
                    base.AddWorth(0);
                }
                if (((attrs.SpellChanneling > 0) && (attrs.FasterCasting >= 0)) && (attrs.HitChanceIncrease >= 12))
                {
                    base.AddWorth(attrs.HitChanceIncrease);
                }
                else
                {
                    base.AddWorth(0);
                }
                if (attrs.LowerManaCost >= 6)
                {
                    base.AddWorth(attrs.LowerManaCost);
                }
                else
                {
                    base.AddWorth(0);
                }
                if (attrs.SwingSpeedIncrease >= 30)
                {
                    base.AddWorth(attrs.SwingSpeedIncrease);
                }
                else
                {
                    base.AddWorth(0);
                }
                if (attrs.HitLightning >= 30)
                {
                    base.AddWorth(attrs.HitLightning);
                }
                else
                {
                    base.AddWorth(0);
                }
                if (attrs.HitFireball >= 40)
                {
                    base.AddWorth(attrs.HitFireball);
                }
                else
                {
                    base.AddWorth(0);
                }
                if (attrs.DamageIncrease >= 0x2d)
                {
                    base.AddWorth(attrs.DamageIncrease);
                }
                else
                {
                    base.AddWorth(0);
                }
                if (attrs.DamageIncrease2 >= 0x2d)
                {
                    base.AddWorth(attrs.DamageIncrease2);
                }
                else
                {
                    base.AddWorth(0);
                }
                if (attrs.SelfRepair >= 5)
                {
                    base.AddWorth(attrs.SelfRepair);
                }
                else
                {
                    base.AddWorth(0);
                }
                if ((((attrs.GetAttribute((AosAttribute)0x102e6c) != 0) || (attrs.GetAttribute((AosAttribute)0x102e7f) != 0)) || ((attrs.GetAttribute((AosAttribute)0x102e78) != 0) || (attrs.GetAttribute((AosAttribute)0x102e70) != 0))) || (((attrs.GetAttribute((AosAttribute)0x102e6d) != 0) || (attrs.GetAttribute((AosAttribute)0x102e6a) != 0)) || ((attrs.GetAttribute((AosAttribute)0x102e79) != 0) || (attrs.GetAttribute((AosAttribute)0x102e6e) != 0))))
                {
                    base.AddWorth(1);
                }
                else
                {
                    base.AddWorth(0);
                }
                if (attrs.GetSkillBonus(SkillName.Stealing) >= 15)
                {
                    base.AddWorth(attrs.GetSkillBonus(SkillName.Stealing));
                }
                else
                {
                    base.AddWorth(0);
                }
                if (attrs.GetSkillBonus(SkillName.AnimalTaming) >= 15)
                {
                    base.AddWorth(attrs.GetSkillBonus(SkillName.AnimalTaming));
                }
                else
                {
                    base.AddWorth(0);
                }
                if (attrs.GetSkillBonus(SkillName.ResistingSpells) >= 15)
                {
                    base.AddWorth(attrs.GetSkillBonus(SkillName.ResistingSpells));
                }
                else
                {
                    base.AddWorth(0);
                }
                if (attrs.Luck >= 80)
                {
                    base.AddWorth(attrs.Luck);
                }
                else
                {
                    base.AddWorth(0);
                }
                int val = (((attrs.PhysicalResist + attrs.ColdResist) + attrs.EnergyResist) + attrs.PoisonResist) + attrs.FireResist;
                if (val >= 0x23)
                {
                    base.AddWorth(val);
                }
                else
                {
                    base.AddWorth(0);
                }
                if (attrs.LowerReagentCost >= 0x10)
                {
                    base.AddWorth(attrs.LowerReagentCost);
                }
                else
                {
                    base.AddWorth(0);
                }
                if (attrs.GetMaxSkillBonus() >= 15)
                {
                    base.AddWorth(attrs.GetMaxSkillBonus());
                }
                else
                {
                    base.AddWorth(0);
                }
                if ((attrs.ManaRegeneration >= 2) && (val >= 0x19))
                {
                    base.AddWorth(attrs.ManaRegeneration);
                    base.AddWorth(val);
                }
                else
                {
                    base.AddWorth(0);
                    base.AddWorth(0);
                }
                if ((attrs.HitPointRegeneration >= 2) && (attrs.ManaRegeneration >= 1))
                {
                    base.AddWorth(attrs.HitPointRegeneration);
                    base.AddWorth(attrs.ManaRegeneration);
                }
                else
                {
                    base.AddWorth(0);
                    base.AddWorth(0);
                }
                if (item.IsJewelry)
                {
                    if (attrs.DamageIncrease >= 0x16)
                    {
                        base.AddWorth(attrs.DamageIncrease);
                    }
                    else
                    {
                        base.AddWorth(0);
                    }
                    if (attrs.DamageIncrease2 >= 0x16)
                    {
                        base.AddWorth(attrs.DamageIncrease2);
                    }
                    else
                    {
                        base.AddWorth(0);
                    }
                    if (attrs.HitChanceIncrease >= 12)
                    {
                        base.AddWorth(attrs.HitChanceIncrease);
                    }
                    else
                    {
                        base.AddWorth(0);
                    }
                    if (attrs.EnhancePotions >= 0x19)
                    {
                        base.AddWorth(attrs.EnhancePotions);
                    }
                    else
                    {
                        base.AddWorth(0);
                    }
                }
                if ((attrs.SpellChanneling > 0) && (attrs.FasterCasting >= 0))
                {
                    base.AddWorth(1);
                }
                else
                {
                    base.AddWorth(0);
                }
                if (World.CharData.LootGold)
                {
                    if ((((item.ID & 0x3fff) == 0xeed) || ((item.ID & 0x3fff) == 0xeee)) || ((item.ID & 0x3fff) == 0xeef))
                    {
                        base.AddWorth(1 + item.Amount);
                    }
                    else
                    {
                        base.AddWorth(0);
                    }
                }
                if (World.CharData.Archery)
                {
                    if (((item.ID & 0x3fff) == 0xf3f) || ((item.ID & 0x3fff) == 0x1bfb))
                    {
                        base.AddWorth(1 + item.Amount);
                    }
                    else
                    {
                        base.AddWorth(0);
                    }
                }
            }
        }
    }
}