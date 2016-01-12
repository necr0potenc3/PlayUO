namespace Client.Targeting
{
    using Client;
    using System;

    public class ServerTargetHandler : ITargetHandler
    {
        private TargetAction m_Action = TargetAction.Unknown;
        private DateTime m_Creation;
        private bool m_DisallowGround;
        private ServerTargetFlags m_Flags;
        private int m_TargetID;

        public ServerTargetHandler(int targetID, bool disallowGround, ServerTargetFlags flags)
        {
            this.m_TargetID = targetID;
            this.m_DisallowGround = disallowGround;
            this.m_Flags = flags;
            this.m_Creation = DateTime.Now;
        }

        public void OnCancel(TargetCancelType type)
        {
            if (type == TargetCancelType.UserCancel)
            {
                Network.Send(new PTarget_Cancel(this));
            }
        }

        public void OnTarget(object targeted)
        {
            Packet packet;
            if (targeted is Mobile)
            {
                Mobile m = (Mobile)targeted;
                if ((!m.Player && ((World.CharData.NotoQuery == NotoQueryType.On) || ((World.CharData.NotoQuery == NotoQueryType.Smart) && (m.CheckGuarded() || World.Player.CheckGuarded())))) && ((((this.m_Flags & ServerTargetFlags.Harmful) != ServerTargetFlags.None) && (m.Notoriety == Notoriety.Innocent)) || (((this.m_Flags & ServerTargetFlags.Beneficial) != ServerTargetFlags.None) && ((m.Notoriety == Notoriety.Criminal) || (m.Notoriety == Notoriety.Murderer)))))
                {
                    Gumps.Desktop.Children.Add(new GCriminalTargetQuery(m, this));
                    return;
                }
                switch (this.m_Action)
                {
                    case TargetAction.Poison:
                        if (!World.CharData.RestrictCures || !m.Flags[MobileFlag.Poisoned])
                        {
                            break;
                        }
                        Engine.TargetHandler = this;
                        return;

                    case TargetAction.GreaterHeal:
                    case TargetAction.Heal:
                        if (World.CharData.RestrictHeals && m.Flags[MobileFlag.Poisoned])
                        {
                            Engine.TargetHandler = this;
                            return;
                        }
                        break;

                    case TargetAction.Cure:
                        if (!World.CharData.RestrictCures || m.Flags[MobileFlag.Poisoned])
                        {
                            break;
                        }
                        Engine.TargetHandler = this;
                        return;
                }
                packet = new PTarget_Response(0, this, m.Serial, m.X, m.Y, m.Z, m.Body);
                if (Party.State == PartyState.Joined)
                {
                    string format = null;
                    if (this.m_Action != TargetAction.Unknown)
                    {
                        string str2 = null;
                        switch (this.m_Action)
                        {
                            case TargetAction.Clumsy:
                                str2 = "Clumsy";
                                break;

                            case TargetAction.Feeblemind:
                                str2 = "Feeblemind";
                                break;

                            case TargetAction.Heal:
                                str2 = "Heal";
                                break;

                            case TargetAction.MagicArrow:
                                str2 = "Magic Arrow";
                                break;

                            case TargetAction.Weaken:
                                str2 = "Weaken";
                                break;

                            case TargetAction.Agility:
                                str2 = "Agility";
                                break;

                            case TargetAction.Cunning:
                                str2 = "Cunning";
                                break;

                            case TargetAction.Cure:
                                str2 = "Cure";
                                break;

                            case TargetAction.Harm:
                                str2 = "Harm";
                                break;

                            case TargetAction.Strength:
                                str2 = "Strength";
                                break;

                            case TargetAction.Bless:
                                str2 = "Bless";
                                break;

                            case TargetAction.Fireball:
                                str2 = "Fireball";
                                break;

                            case TargetAction.Poison:
                                str2 = "Poison";
                                break;

                            case TargetAction.Curse:
                                str2 = "Curse";
                                break;

                            case TargetAction.GreaterHeal:
                                str2 = "Greater Heal";
                                break;

                            case TargetAction.Lightning:
                                str2 = "Lightning";
                                break;

                            case TargetAction.ManaDrain:
                                str2 = "Mana Drain";
                                break;

                            case TargetAction.MindBlast:
                                str2 = "Mind Blast";
                                break;

                            case TargetAction.Paralyze:
                                str2 = "Paralyze";
                                break;

                            case TargetAction.Dispel:
                                str2 = "Dispel";
                                break;

                            case TargetAction.EnergyBolt:
                                str2 = "Energy Bolt";
                                break;

                            case TargetAction.Explosion:
                                str2 = "Explosion";
                                break;

                            case TargetAction.Invisibility:
                                str2 = "Invisibility";
                                break;

                            case TargetAction.FlameStrike:
                                str2 = "Flame Strike";
                                break;

                            case TargetAction.ManaVampire:
                                str2 = "Mana Vampire";
                                break;

                            case TargetAction.Resurrection:
                                str2 = "Resurrection";
                                break;

                            case TargetAction.Bola:
                                str2 = "a Bola";
                                break;
                        }
                        if (str2 != null)
                        {
                            format = "Targeting {0} with " + str2.ToLower();
                        }
                    }
                    if ((format != null) && (m.Player || ((m.Name != null) && (m.Name.Length > 0))))
                    {
                        Network.Send(new PParty_PublicMessage(string.Format(format, m.Player ? "myself" : m.Name)));
                    }
                }
            }
            else if (targeted is Item)
            {
                int x;
                int y;
                int z;
                Item item = (Item)targeted;
                if (item.InWorld)
                {
                    x = item.X;
                    y = item.Y;
                    z = item.Z;
                }
                else
                {
                    x = item.X;
                    y = item.Y;
                    z = 0;
                }
                packet = new PTarget_Response(0, this, item.Serial, x, y, z, item.ID & 0x3fff);
            }
            else if (targeted is StaticTarget)
            {
                StaticTarget target = (StaticTarget)targeted;
                packet = new PTarget_Response(1, this, 0, target.X, target.Y, target.Z, target.ID & 0x3fff);
            }
            else if (targeted is LandTarget)
            {
                if (this.m_DisallowGround)
                {
                    Engine.TargetHandler = this;
                    return;
                }
                LandTarget target2 = (LandTarget)targeted;
                packet = new PTarget_Response(1, this, 0, target2.X, target2.Y, target2.Z, 0);
            }
            else
            {
                Engine.TargetHandler = this;
                return;
            }
            Network.Send(packet);
        }

        public TargetAction Action
        {
            get
            {
                return this.m_Action;
            }
            set
            {
                this.m_Action = value;
            }
        }

        public DateTime Creation
        {
            get
            {
                return this.m_Creation;
            }
        }

        public ServerTargetFlags Flags
        {
            get
            {
                return this.m_Flags;
            }
        }

        public int TargetID
        {
            get
            {
                return this.m_TargetID;
            }
        }
    }
}