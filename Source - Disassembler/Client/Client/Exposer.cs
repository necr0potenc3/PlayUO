namespace Client
{
    using Client.Targeting;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Windows.Forms;

    public class Exposer : IExposed
    {
        private Plugin m_Plugin;

        public Exposer(Plugin Plugin, string f)
        {
            this.m_Plugin = Plugin;
        }

        public void Ability(string type)
        {
            switch (type.ToLower())
            {
                case "primary":
                    AbilityInfo.Active = AbilityInfo.Primary;
                    break;

                case "secondary":
                    AbilityInfo.Active = AbilityInfo.Secondary;
                    break;

                case "none":
                    AbilityInfo.Active = null;
                    break;
            }
        }

        public void AddTextMessage(string msg)
        {
            Engine.AddTextMessage(msg);
        }

        public void AddTextMessage(string msg, TimeSpan ts)
        {
            Engine.AddTextMessage(msg, (float)ts.TotalSeconds);
        }

        public void AddTextMessage(string msg, TimeSpan ts, int font)
        {
            Engine.AddTextMessage(msg, (float)ts.TotalSeconds, Engine.GetFont(font));
        }

        public void AddTimer(Client.Timer t)
        {
            Engine.AddTimer(t);
        }

        public void AllNames()
        {
            Engine.AllNames();
        }

        public bool Attack(string what)
        {
            switch (what.ToLower())
            {
                case "last":
                    return Engine.AttackLast();

                case "red":
                    return Engine.AttackRed();
            }
            return false;
        }

        public void AutoUse()
        {
            ArrayList autoUse = World.CharData.AutoUse;
            if (autoUse.Count <= 0)
            {
                Engine.AddTextMessage("There are no items in your use-once list.");
            }
            else
            {
                Mobile player = World.Player;
                if (player != null)
                {
                    Item backpack = player.Backpack;
                    if (backpack != null)
                    {
                        for (int i = 0; i < autoUse.Count; i++)
                        {
                            Engine.m_AutoUseIndex++;
                            Engine.m_AutoUseIndex = Engine.m_AutoUseIndex % autoUse.Count;
                            Item check = (Item)autoUse[Engine.m_AutoUseIndex];
                            if (backpack.ContainsItem(check) || (check.IsEquip && (check.EquipParent == player)))
                            {
                                check.Use();
                                return;
                            }
                        }
                        Engine.AddTextMessage("No use-once items were found on your person.");
                    }
                }
            }
        }

        public bool BandageSelf()
        {
            return Engine.BandageSelf();
        }

        public void CastSpell(int SpellID)
        {
            if ((((SpellID >= 0) && (SpellID < 0x40)) || ((SpellID >= 100) && (SpellID < 0x74))) || ((SpellID >= 200) && (SpellID < 210)))
            {
                Network.Send(new PCastSpell(SpellID + 1));
            }
        }

        public void CastSpell(string Name)
        {
            Spell spellByName = Spells.GetSpellByName(Name);
            if (spellByName != null)
            {
                spellByName.Cast();
            }
        }

        public void ClearLastTarget()
        {
            Engine.m_LastTarget = null;
        }

        public void ClearScreen()
        {
            Gumps.Desktop.Children.Clear();
            Gumps.Desktop.Children.Add(new GPingDisplay());
            Gumps.Desktop.Children.Add(new GParticleCounter());
            Gumps.Desktop.Children.Add(new GTransparencyGump());
        }

        public void ClearTargetQueue()
        {
            Mobile player = World.Player;
            if (player != null)
            {
                player.AddTextMessage("", "Cleared target queue.", Engine.GetFont(3), Hues.Load(0x59), true);
            }
            Engine.m_TargetQueue = null;
        }

        public void Count(string what)
        {
            switch (what.ToLower())
            {
                case "regs":
                    Engine.CountReagents();
                    break;

                case "ammo":
                    Engine.CountAmmo();
                    break;
            }
        }

        public bool DelayMacro(string args)
        {
            try
            {
                return Macro.Delay(int.Parse(args));
            }
            catch
            {
            }
            return true;
        }

        public void Dequip()
        {
            World.CharData.Equip.Dequip();
        }

        public void Dismount()
        {
            Mobile player = World.Player;
            if ((player != null) && (player.FindEquip(Layer.Mount) != null))
            {
                player.Use();
            }
        }

        public void DoAction(string Action)
        {
            Network.Send(new PAction(Action));
        }

        public void Equip(int index)
        {
            World.CharData.Equip.Equip(index);
        }

        public Bitmap GetGump(int GumpID, int HueID)
        {
            Texture gump = Hues.Load(HueID).GetGump(GumpID);
            if ((gump == null) || gump.IsEmpty())
            {
                return null;
            }
            return gump.ToBitmap();
        }

        public Bitmap GetItem(int ItemID, int HueID)
        {
            Texture texture = Hues.Load(HueID).GetItem(ItemID);
            if ((texture == null) || texture.IsEmpty())
            {
                return null;
            }
            return texture.ToBitmap();
        }

        public Bitmap GetLand(int LandID, int HueID)
        {
            Texture land = Hues.Load(HueID).GetLand(LandID);
            if ((land == null) || land.IsEmpty())
            {
                return null;
            }
            return land.ToBitmap();
        }

        public void Last(string what)
        {
            switch (what)
            {
                case "Object":
                    PUseRequest.SendLast();
                    break;

                case "Spell":
                    PCastSpell.SendLast();
                    break;

                case "Skill":
                    PUseSkill.SendLast();
                    break;
            }
        }

        public void Open(string What)
        {
            switch (What)
            {
                case "Help":
                    Engine.OpenHelp();
                    break;

                case "Options":
                    Engine.OpenOptions();
                    break;

                case "Journal":
                    Engine.OpenJournal();
                    break;

                case "Skills":
                    Engine.OpenSkills();
                    break;

                case "Status":
                    Engine.OpenStatus();
                    break;

                case "Spellbook":
                    Engine.OpenSpellbook(1);
                    break;

                case "NecroSpellbook":
                    Engine.OpenSpellbook(2);
                    break;

                case "PaladinSpellbook":
                    Engine.OpenSpellbook(3);
                    break;

                case "Paperdoll":
                    Engine.OpenPaperdoll();
                    break;

                case "Backpack":
                    Engine.OpenBackpack();
                    break;

                case "Radar":
                    GRadar.Open();
                    break;

                case "NetStats":
                    GNetwork.Open();
                    break;

                case "Abilities":
                    GCombatGump.Open();
                    break;

                case "Macros":
                    GMacroEditorForm.Open();
                    break;

                case "InfoBrowser":
                    GInfoForm.Open();
                    break;
            }
        }

        public void Paste()
        {
            try
            {
                IDataObject dataObject = Clipboard.GetDataObject();
                if (dataObject.GetDataPresent(DataFormats.UnicodeText))
                {
                    string data = (string)dataObject.GetData(DataFormats.UnicodeText);
                    this.Paste(data);
                }
                else if (dataObject.GetDataPresent(DataFormats.Text))
                {
                    string toPaste = (string)dataObject.GetData(DataFormats.Text);
                    this.Paste(toPaste);
                }
            }
            catch (Exception exception)
            {
                Debug.Error(exception);
            }
        }

        public void Paste(string ToPaste)
        {
            string[] strArray = (Engine.m_Text + ToPaste.Replace("\r\n", "\n")).Split(new char[] { '\n' });
            int length = strArray.Length;
            for (int i = 0; i < length; i++)
            {
                strArray[i] = strArray[i].Trim();
                if (i < (length - 1))
                {
                    Engine.commandEntered(strArray[i]);
                }
                else
                {
                    Engine.m_Text = strArray[i];
                    Renderer.SetText(strArray[i]);
                }
            }
        }

        public void Quit()
        {
            Engine.Quit();
        }

        public void RegisterAsMacro(string action)
        {
            this.RegisterAsMacro(action, (ParamNode[])null);
        }

        public void RegisterAsMacro(string action, params ParamNode[] options)
        {
            ActionHandler.Register(action, options, this.m_Plugin);
        }

        public void RegisterAsMacro(string action, params string[] list)
        {
            ParamNode[] options = new ParamNode[list.Length];
            for (int i = 0; i < options.Length; i++)
            {
                options[i] = new ParamNode(list[i], list[i]);
            }
            this.RegisterAsMacro(action, options);
        }

        public void RegisterAsMacro(string action, string[,] list)
        {
            ParamNode[] options = new ParamNode[list.GetLength(0)];
            for (int i = 0; i < options.Length; i++)
            {
                options[i] = new ParamNode(list[i, 0], list[i, 1]);
            }
            this.RegisterAsMacro(action, options);
        }

        public void Remount()
        {
            Mobile player = World.Player;
            if ((player != null) && (player.FindEquip(Layer.Mount) == null))
            {
                MountTable mountTable = Engine.m_Animations.MountTable;
                foreach (Mobile mobile2 in World.Mobiles.Values)
                {
                    if ((player.InSquareRange(mobile2.XReal, mobile2.YReal, 1) && !mobile2.Bonded) && mountTable.IsMount(mobile2.Body))
                    {
                        if ((mobile2.Name == null) || (mobile2.Name.Length == 0))
                        {
                            mobile2.QueryStats();
                        }
                        else if (mobile2.IsPet)
                        {
                            mobile2.Use();
                            break;
                        }
                    }
                }
            }
        }

        public void RemoveTimer(Client.Timer t)
        {
            Engine.RemoveTimer(t);
        }

        public void RepeatMacro()
        {
            Macro.Repeat();
        }

        public void RepeatSpeech()
        {
            Engine.Repeat();
        }

        public bool Resync()
        {
            return Engine.Resync();
        }

        public void Say(string Text)
        {
            Engine.m_SayMacro = true;
            Engine.commandEntered(Engine.Encode(Text));
            Engine.m_SayMacro = false;
        }

        public void SetAutoUse()
        {
            Engine.TargetHandler = new AddAutoUseTargetHandler();
        }

        public void SetEquip(int index)
        {
            Engine.TargetHandler = new SetEquipTargetHandler(index);
        }

        public void SetText(string text)
        {
            Renderer.SetText(text);
            Engine.m_Text = text;
        }

        public bool SmartPotion()
        {
            return Engine.SmartPotion();
        }

        public void StopMacros()
        {
            Macros.StopAll();
        }

        public void Target(string What)
        {
            switch (What)
            {
                case "Self":
                    Engine.TargetSelf();
                    break;

                case "Last":
                    Engine.TargetLast();
                    break;

                case "Innocent":
                    Engine.TargetNoto(Notoriety.Innocent);
                    break;

                case "Ally":
                    Engine.TargetNoto(Notoriety.Ally);
                    break;

                case "Enemy":
                    Engine.TargetNoto(Notoriety.Enemy);
                    break;

                case "Murderer":
                    Engine.TargetNoto(Notoriety.Murderer);
                    break;

                case "Criminal":
                    Engine.TargetNoto(Notoriety.Criminal);
                    break;

                case "Gray":
                    Engine.TargetGray();
                    break;

                case "Smart":
                    Engine.TargetSmart();
                    break;

                case "NonParty":
                    Engine.TargetNonParty();
                    break;

                case "NonFriend":
                    Engine.TargetNonFriend();
                    break;

                case "Aquire":
                    Engine.TargetAquire();
                    break;
            }
        }

        public void UseItemByType(int[] itemIDs)
        {
            Mobile player = World.Player;
            if (player != null)
            {
                Item backpack = player.Backpack;
                if (backpack != null)
                {
                    Item item2 = backpack.FindItem(new ItemIDValidator(itemIDs));
                    if (item2 != null)
                    {
                        item2.Use();
                    }
                }
            }
        }

        public bool UsePotion(PotionType type)
        {
            bool flag = Engine.UsePotion(type);
            if (!flag)
            {
                Engine.AddTextMessage(string.Format("You do not have any {0} potions!", type.ToString().ToLower()), Engine.DefaultFont, Hues.Load(0x22));
            }
            return flag;
        }

        public void UseSkill(string Name)
        {
            Skill skill = Engine.Skills[Engine.Skills.GetSkill(Name)];
            if (skill != null)
            {
                skill.Use();
            }
            else
            {
                Engine.AddTextMessage(string.Format("Unknown skill '{0}'", Name));
            }
        }

        public bool WrestleMove(WrestleType type)
        {
            Packet p = null;
            switch (type)
            {
                case WrestleType.Disarm:
                    p = new PWrestleDisarm();
                    break;

                case WrestleType.Stun:
                    p = new PWrestleStun();
                    break;
            }
            return ((p != null) && Network.Send(p));
        }

        public bool CanTargetLast
        {
            get
            {
                return (Engine.m_LastTarget != null);
            }
        }

        public bool ContainerGrid
        {
            get
            {
                return Engine.m_ContainerGrid;
            }
            set
            {
                Engine.m_ContainerGrid = value;
            }
        }

        public bool FPS
        {
            get
            {
                return Engine.FPS;
            }
            set
            {
                Engine.FPS = value;
            }
        }

        public bool Grid
        {
            get
            {
                return Engine.Grid;
            }
            set
            {
                Engine.Grid = value;
            }
        }

        public bool Halos
        {
            get
            {
                return World.CharData.Halos;
            }
            set
            {
                World.CharData.Halos = value;
            }
        }

        public bool HasTarget
        {
            get
            {
                return (Engine.TargetHandler != null);
            }
        }

        public bool LastTargetIsDynamic
        {
            get
            {
                return (Engine.m_LastTarget is Item);
            }
        }

        public bool LastTargetIsLand
        {
            get
            {
                return (Engine.m_LastTarget is LandTarget);
            }
        }

        public bool LastTargetIsMobile
        {
            get
            {
                return (Engine.m_LastTarget is Mobile);
            }
        }

        public bool LastTargetIsSelf
        {
            get
            {
                return ((Engine.m_LastTarget is Mobile) && ((Mobile)Engine.m_LastTarget).Player);
            }
        }

        public bool LastTargetIsStatic
        {
            get
            {
                return (Engine.m_LastTarget is StaticTarget);
            }
        }

        public bool MiniHealth
        {
            get
            {
                return Engine.MiniHealth;
            }
            set
            {
                Engine.MiniHealth = value;
            }
        }

        public bool ParticleCount
        {
            get
            {
                return Renderer.DrawPCount;
            }
            set
            {
                Renderer.DrawPCount = value;
            }
        }

        public bool Ping
        {
            get
            {
                return Renderer.DrawPing;
            }
            set
            {
                Renderer.DrawPing = value;
            }
        }

        public int Player
        {
            get
            {
                return World.Serial;
            }
        }

        public bool PumpFPS
        {
            get
            {
                return Engine.m_PumpFPS;
            }
            set
            {
                Engine.m_PumpFPS = value;
            }
        }

        public bool RegCounter
        {
            get
            {
                return GItemCounters.Active;
            }
            set
            {
                GItemCounters.Active = value;
            }
        }

        public bool Screenshots
        {
            get
            {
                return Engine.m_Screenshots;
            }
            set
            {
                if (Engine.m_Screenshots != value)
                {
                    Engine.m_Screenshots = value;
                    Engine.AddTextMessage(string.Format("Death screenshots are now {0}.", value ? "on" : "off"));
                }
            }
        }

        public bool Temperature
        {
            get
            {
                return Engine.Effects.DrawTemperature;
            }
            set
            {
                Engine.Effects.DrawTemperature = value;
            }
        }

        public bool Transparency
        {
            get
            {
                return Renderer.Transparency;
            }
            set
            {
                Renderer.Transparency = value;
            }
        }

        public bool Warmode
        {
            get
            {
                return Engine.Warmode;
            }
            set
            {
                Engine.Warmode = value;
            }
        }
    }
}