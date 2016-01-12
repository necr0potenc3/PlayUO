namespace Client.Targeting
{
    using Client;
    using System;

    public class TargetActions
    {
        private static DateTime m_Creation;
        private static TargetAction m_Lookahead;
        public static TimeSpan MarginOfError = TimeSpan.FromSeconds(2.5);

        public static ServerTargetFlags GetFlags(TargetAction action)
        {
            if (action < TargetAction.DetectHidden)
            {
                if (action >= TargetAction.Bola)
                {
                    return ServerTargetFlags.Harmful;
                }
                if (action >= TargetAction.Bandage)
                {
                    return ServerTargetFlags.Beneficial;
                }
                switch (action)
                {
                    case TargetAction.Clumsy:
                    case TargetAction.Feeblemind:
                    case TargetAction.MagicArrow:
                    case TargetAction.Weaken:
                    case TargetAction.Harm:
                    case TargetAction.Fireball:
                    case TargetAction.Poison:
                    case TargetAction.Curse:
                    case TargetAction.Lightning:
                    case TargetAction.ManaDrain:
                    case TargetAction.MindBlast:
                    case TargetAction.Paralyze:
                    case TargetAction.Dispel:
                    case TargetAction.EnergyBolt:
                    case TargetAction.Explosion:
                    case TargetAction.FlameStrike:
                    case TargetAction.ManaVampire:
                        return ServerTargetFlags.Harmful;

                    case TargetAction.Heal:
                    case TargetAction.Agility:
                    case TargetAction.Cunning:
                    case TargetAction.Cure:
                    case TargetAction.Strength:
                    case TargetAction.Bless:
                    case TargetAction.GreaterHeal:
                    case TargetAction.Invisibility:
                    case TargetAction.Resurrection:
                        return ServerTargetFlags.Beneficial;

                    case TargetAction.ArchCure:
                    case TargetAction.ArchProtection:
                    case TargetAction.FireField:
                    case TargetAction.PoisonField:
                    case TargetAction.ParalyzeField:
                    case TargetAction.Reveal:
                    case TargetAction.ChainLightning:
                    case TargetAction.EnergyField:
                    case TargetAction.MeteorSwarm:
                        return ServerTargetFlags.None;
                }
            }
            return ServerTargetFlags.None;
        }

        public static void Identify()
        {
            if (m_Lookahead != TargetAction.Unknown)
            {
                ServerTargetHandler targetHandler = Engine.TargetHandler as ServerTargetHandler;
                if ((((m_Creation + MarginOfError) > DateTime.Now) && (targetHandler != null)) && (targetHandler.Flags == GetFlags(m_Lookahead)))
                {
                    targetHandler.Action = m_Lookahead;
                }
                m_Lookahead = TargetAction.Unknown;
            }
        }

        public static void Identify(TargetAction action)
        {
            ServerTargetHandler targetHandler = Engine.TargetHandler as ServerTargetHandler;
            if (((targetHandler != null) && ((targetHandler.Creation + MarginOfError) > DateTime.Now)) && (targetHandler.Flags == GetFlags(action)))
            {
                targetHandler.Action = action;
            }
            m_Lookahead = TargetAction.Unknown;
        }

        public static TargetAction Lookahead
        {
            get
            {
                return m_Lookahead;
            }
            set
            {
                m_Lookahead = value;
                m_Creation = DateTime.Now;
            }
        }
    }
}