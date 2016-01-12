namespace Client
{
    using Client.Prompts;
    using Client.Targeting;
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;

    public class PacketHandlers
    {
        private const string DefaultALFReason = "There is some problem communicating with Origin. Please restart Ultima Online and try again.";
        private const string DefaultLRReason = "The client could not attach to the game server.  It must have been taken down, please wait a few minutes and try again.";
        private static string[] m_ALFReasons = new string[] { "Either the Account Name or Password you provided were incorrect. If this is a new account your account may not be active yet. Please try again shortly.", "Someone is already using this account.", "Your account has been blocked. Please visit http://ultima-registration.com for information on reactivating your account.", "Your account credentials are invalid. Check your user ID and password and try again.", "The IGR concurrency limit has been met.", "The IGR time limit has been met.", "A general IGR authentication failure has occured." };
        private static Regex m_ArgReplace = new Regex(@"~(?<1>\d+).*?~", RegexOptions.Singleline);
        private static string[] m_Args;
        private static string m_BookAuthor;
        private static string m_BookTitle;
        private static string[] m_BuyMenuNames;
        private static int[] m_BuyMenuPrices;
        private static int m_BuyMenuSerial;
        internal static bool m_CancelTarget;
        private static int m_CastCount;
        private static TimeSpan m_CastDelay;
        private static DateTime m_EffectTime;
        internal static DateTime m_GumpTime;
        internal static PacketHandler[] m_Handlers = new PacketHandler[0x100];
        private static DateTime m_HealStart;
        private static string[] m_IPFReason = new string[] { "You can not pick that up.", "That is too far away.", "That is out of sight.", "That item dose not belong to you. You'll have to steal it.", "You are already holding an item." };
        internal static DateTime m_LastMultiMove;
        internal static int m_LastMultiX;
        internal static int m_LastMultiY;
        private static string m_LastSpell;
        private static DateTime m_LastTime;
        private static int m_LastWorld = -1;
        private static string[] m_LoginRejectReasons = new string[] { "Incorrect password.", "This character does not exist anymore.  You will have to recreate it.", "This character already exists.", "The client could not attach to the game server.  It must have been taken down, please wait a few minutes and try again.", "The client could not attach to the game server.  It must have been taken down, please wait a few minutes and try again.", "Another character from this account is currently online in this world.  You must either log in with that character or wait for it to time out.", "An error has occurred in the synchronization between the login servers and this world.  Please close your client and try again." };
        internal static int m_MultiSequenceCount = -20;
        private static int m_PathfindIndex = 20;
        private static int m_RepeatCast;
        private static System.Collections.Queue m_Sequences = new System.Collections.Queue();
        internal static DateTime m_StartMultiSequence;
        private static DateTime m_TimeLastCast;
        private static DateTime m_TimeLastSpell;
        private static string[] m_WorldNames = new string[] { "Felucca", "Trammel", "Ilshenar", "Malas", "Tokuno Islands" };
        private static int m_xMapLeft;
        private static int m_xMapRight;
        private static int m_xMapWidth;
        private static int m_yMapBottom;
        private static int m_yMapHeight;
        private static int m_yMapTop;

        static PacketHandlers()
        {
            Register(130, "Account Login Failed", 2, new PacketCallback(PacketHandlers.AccountLoginFail));
            Register(0x1b, "Login Confirm", 0x25, new PacketCallback(PacketHandlers.LoginConfirm));
            Register(0x55, "Login Completed", 1, new PacketCallback(PacketHandlers.LoginComplete));
            Register(140, "Server Relay", 11, new PacketCallback(PacketHandlers.ServerRelay));
            Register(0xa8, "Server List", -1, new PacketCallback(PacketHandlers.ServerList));
            Register(0xa9, "Char/City List", -1, new PacketCallback(PacketHandlers.CharacterList));
            Register(0x81, "Change Character", -1, new PacketCallback(PacketHandlers.ChangeCharacter));
            Register(0x53, "Login Rejected", 2, new PacketCallback(PacketHandlers.LoginReject));
            Register(50, "Unknown", 2, new PacketCallback(PacketHandlers.Unk32));
            Register(0x1c, "ASCII Message", -1, new PacketCallback(PacketHandlers.Message_ASCII));
            Register(0xae, "Unicode Message", -1, new PacketCallback(PacketHandlers.Message_Unicode));
            Register(0xc1, "Localized Message", -1, new PacketCallback(PacketHandlers.Message_Localized));
            Register(0xcc, "Localized Message Affix", -1, new PacketCallback(PacketHandlers.Message_Localized_Affix));
            Register(0xc2, "Unicode Prompt", -1, new PacketCallback(PacketHandlers.Prompt_Unicode));
            Register(0x9a, "ASCII Prompt", -1, new PacketCallback(PacketHandlers.Prompt_ASCII));
            Register(0xd6, "Property List Content", -1, new PacketCallback(PacketHandlers.PropertyListContent));
            Register(0x11, "Mobile Status", -1, new PacketCallback(PacketHandlers.Mobile_Status));
            Register(0x20, "Mobile Update", 0x13, new PacketCallback(PacketHandlers.Mobile_Update));
            Register(0x77, "Mobile Moving", 0x11, new PacketCallback(PacketHandlers.Mobile_Moving));
            Register(120, "Mobile Incoming", -1, new PacketCallback(PacketHandlers.Mobile_Incoming));
            Register(0xa1, "Mobile Hit Points", 9, new PacketCallback(PacketHandlers.Mobile_Attributes_HitPoints));
            Register(0xa2, "Mobile Mana", 9, new PacketCallback(PacketHandlers.Mobile_Attributes_Mana));
            Register(0xa3, "Mobile Stamina", 9, new PacketCallback(PacketHandlers.Mobile_Attributes_Stamina));
            Register(0x2d, "Mobile Attributes", 0x11, new PacketCallback(PacketHandlers.Mobile_Attributes));
            Register(110, "Mobile Animation", 14, new PacketCallback(PacketHandlers.Mobile_Animation));
            Register(0xaf, "Mobile Death", 13, new PacketCallback(PacketHandlers.Mobile_Death));
            Register(11, "Mobile Damage", 7, new PacketCallback(PacketHandlers.Mobile_Damage));
            Register(0x2e, "Equip Item", 15, new PacketCallback(PacketHandlers.EquipItem));
            Register(0x88, "Display Paperdoll", 0x42, new PacketCallback(PacketHandlers.DisplayPaperdoll));
            Register(0xb8, "Display Profile", -1, new PacketCallback(PacketHandlers.DisplayProfile));
            Register(0x1a, "World Item", -1, new PacketCallback(PacketHandlers.WorldItem));
            Register(0x24, "Container Open", 7, new PacketCallback(PacketHandlers.Container_Open));
            Register(0x25, "Container Item", 20, new PacketCallback(PacketHandlers.Container_Item));
            Register(60, "Container Items", -1, new PacketCallback(PacketHandlers.Container_Items));
            Register(0x29, "Drop Accept", 1, new PacketCallback(PacketHandlers.Drop_Accept));
            Register(40, "Drop Reject", 5, new PacketCallback(PacketHandlers.Drop_Reject));
            Register(0x1d, "Delete Object", 5, new PacketCallback(PacketHandlers.DeleteObject));
            Register(0x21, "Movement Reject", 8, new PacketCallback(PacketHandlers.Movement_Reject));
            Register(0x22, "Movement Accept", 3, new PacketCallback(PacketHandlers.Movement_Accept));
            Register(0x7c, "Display Question Menu", -1, new PacketCallback(PacketHandlers.DisplayQuestionMenu));
            Register(0x95, "Select Hue", 9, new PacketCallback(PacketHandlers.SelectHue));
            Register(0xa6, "Scroll Message", -1, new PacketCallback(PacketHandlers.ScrollMessage));
            Register(0xab, "String Query", -1, new PacketCallback(PacketHandlers.StringQuery));
            Register(0xb0, "Display Gump", -1, new PacketCallback(PacketHandlers.DisplayGump));
            Register(0x54, "Play Sound", 12, new PacketCallback(PacketHandlers.PlaySound));
            Register(0x23, "Drag Item", 0x1a, new PacketCallback(PacketHandlers.DragItem));
            Register(0x70, "Standard Effect", 0x1c, new PacketCallback(PacketHandlers.StandardEffect));
            Register(0xc0, "Hued Effect", 0x24, new PacketCallback(PacketHandlers.HuedEffect));
            Register(0xc7, "Particle Effect", 0x31, new PacketCallback(PacketHandlers.ParticleEffect));
            Register(0x4e, "Personal Light Level", 6, new PacketCallback(PacketHandlers.Light_Personal));
            Register(0x4f, "Global Light Level", 2, new PacketCallback(PacketHandlers.Light_Global));
            Register(0x5b, "Game Time", 4, new PacketCallback(PacketHandlers.GameTime));
            Register(0x65, "Weather", 4, new PacketCallback(PacketHandlers.Weather));
            Register(0x6d, "Play Music", 3, new PacketCallback(PacketHandlers.PlayMusic));
            Register(0xd4, "Open Book", -1, new PacketCallback(PacketHandlers.Book_Open));
            Register(0x66, "Book Page Info", -1, new PacketCallback(PacketHandlers.Book_PageInfo));
            Register(0xd8, "Customized House Content", -1, new PacketCallback(PacketHandlers.CustomizedHouseContent));
            Register(0x71, "Bulletin Board", -1, new PacketCallback(PacketHandlers.BulletinBoard));
            Register(0x74, "Shop Content", -1, new PacketCallback(PacketHandlers.ShopContent));
            Register(0x9e, "Sell Content", -1, new PacketCallback(PacketHandlers.SellContent));
            Register(0x3b, "Close Shop Dialog", -1, new PacketCallback(PacketHandlers.CloseShopDialog));
            Register(170, "Current Target", 5, new PacketCallback(PacketHandlers.CurrentTarget));
            Register(0x72, "Warmode Status", 5, new PacketCallback(PacketHandlers.WarmodeStatus));
            Register(240, "Custom", -1, new PacketCallback(PacketHandlers.Custom));
            Register(0x6c, "Target", 0x13, new PacketCallback(PacketHandlers.Target));
            Register(0x27, "Item Pickup Failed", 2, new PacketCallback(PacketHandlers.ItemPickupFailed));
            Register(0xbf, "Extended Command", -1, new PacketCallback(PacketHandlers.Command));
            Register(0x2c, "Request Resurrection", 2, new PacketCallback(PacketHandlers.RequestResurrection));
            Register(0xb9, "Features", 3, new PacketCallback(PacketHandlers.Features));
            Register(0x33, "Pause", 2, new PacketCallback(PacketHandlers.Pause));
            Register(0x89, "Corspe Equip", -1, new PacketCallback(PacketHandlers.CorpseEquip));
            Register(0xa5, "Launch Browser", -1, new PacketCallback(PacketHandlers.LaunchBrowser));
            Register(0x2f, "Fight Occurring", 10, new PacketCallback(PacketHandlers.FightOccurring));
            Register(0x3a, "Skills", -1, new PacketCallback(PacketHandlers.Skills));
            Register(0x73, "Ping Reply", 2, new PacketCallback(PacketHandlers.PingReply));
            Register(0x99, "Multi Target", 0x1a, new PacketCallback(PacketHandlers.MultiTarget));
            Register(0x6f, "Secure Trade", -1, new PacketCallback(PacketHandlers.SecureTrade));
            Register(0xba, "Quest Arrow", 6, new PacketCallback(PacketHandlers.QuestArrow));
            Register(0x76, "Server Change", 0x10, new PacketCallback(PacketHandlers.ServerChange));
            Register(200, "Revise Update Range", 2, new PacketCallback(PacketHandlers.ReviseUpdateRange));
            Register(0xcb, "GQ Count", 7, new PacketCallback(PacketHandlers.GQCount));
            Register(0xbd, "Client Version Request", -1, new PacketCallback(PacketHandlers.VersionRequest_Client));
            Register(190, "Assist Version Request", -1, new PacketCallback(PacketHandlers.VersionRequest_Assist));
            Register(220, "Property List Hash", 9, new PacketCallback(PacketHandlers.PropertyListHash));
            Register(0xda, "MahJong", -1, new PacketCallback(PacketHandlers.Trace));
            Register(0xbc, "Season", 3, new PacketCallback(PacketHandlers.Season));
            Register(0x69, "Friends (Party?)", -1, Unhandled);
            Register(0xbb, "Account ID", 9, Unhandled);
            Register(0xd7, "AOS Packet 0xD7", -1, new PacketCallback(PacketHandlers.Trace));
            Register(0x56, "Map Command", 11, new PacketCallback(PacketHandlers.MapCommand));
            Register(0x90, "Map Window", 0x13, new PacketCallback(PacketHandlers.MapWindow));
            Register(0x7b, "Sequence", 2, new PacketCallback(PacketHandlers.Sequence));
            Register(0x3e, "Versions", 0x25, new PacketCallback(PacketHandlers.Trace));
            Register(0x3f, "Update Object Chunk", -1, new PacketCallback(PacketHandlers.Trace));
            Register(0x40, "Update Terrain Chunk", 0xc9, new PacketCallback(PacketHandlers.Trace));
            Register(0x41, "Update Tiledata", -1, new PacketCallback(PacketHandlers.Trace));
            Register(0x42, "Update Art", -1, new PacketCallback(PacketHandlers.Trace));
            Register(0x43, "Update Anim", 0x229, new PacketCallback(PacketHandlers.Trace));
            Register(0x44, "Update Hues", 0x2c9, new PacketCallback(PacketHandlers.Trace));
            Register(0x45, "Version Ok", 5, new PacketCallback(PacketHandlers.Trace));
            Register(0xc3, "GQ Response", -1, new PacketCallback(PacketHandlers.Trace));
            Register(0x98, "Mobile Name", -1, new PacketCallback(PacketHandlers.Trace));
            Register(0xb2, "Chat Action", -1, Unhandled);
            Register(0x38, "Pathfind", 7, new PacketCallback(PacketHandlers.Pathfind));
            Register(0xd1, "Logout Ok", 2, new PacketCallback(PacketHandlers.Trace));
        }

        private static void AccountLoginFail(PacketReader pvSrc)
        {
            Network.m_SoftDisconnect = true;
            Cursor.Hourglass = false;
            int index = pvSrc.ReadByte();
            if (index == 0xfe)
            {
                Engine.ShowAcctLogin();
            }
            else
            {
                string str = (index < m_ALFReasons.Length) ? m_ALFReasons[index] : "There is some problem communicating with Origin. Please restart Ultima Online and try again.";
                Gumps.Desktop.Children.Clear();
                xGumps.SetVariable("FailMessage", str);
                xGumps.Display("ConnectionFailed");
            }
        }

        private static void AddGuardline(int x, int y, object val)
        {
        }

        private static void AddMessage(int serial, IFont font, IHue hue, int type, string name, string text)
        {
            AddMessage(serial, font, hue, type, name, text, 0);
        }

        private static void AddMessage(int serial, IFont font, IHue hue, int type, string name, string text, int number)
        {
            name = name.Trim();
            text = text.Trim();
            if (number == 0xf51ed)
            {
                Mobile player = World.Player;
                if (Party.State == PartyState.Joined)
                {
                    Network.Send(new PParty_PublicMessage(string.Format("I stunned {0} !!", (Engine.m_LastAttacker == null) ? "someone" : (((Engine.m_LastAttacker.Name != null) && (Engine.m_LastAttacker.Name.Length > 0)) ? Engine.m_LastAttacker.Name : "someone"))));
                }
                else if (player != null)
                {
                    Engine.Sounds.PlaySound(0x1e1, player.X, player.Y, player.Z);
                }
            }
            else if (number == 0xf51ee)
            {
                Mobile mobile2 = World.Player;
                if (Party.State == PartyState.Joined)
                {
                    Network.Send(new PParty_PublicMessage("I'm stunned !!"));
                }
                else if (mobile2 != null)
                {
                    Engine.Sounds.PlaySound(0x157, mobile2.X, mobile2.Y, mobile2.Z);
                }
            }
            switch (number)
            {
                case 0x1003c5:
                    TargetActions.Lookahead = TargetAction.Discord;
                    break;

                case 0x100420:
                    TargetActions.Identify(TargetAction.Bola);
                    break;

                case 0x7a453:
                    TargetActions.Lookahead = TargetAction.DetectHidden;
                    break;

                case 0x7a4d4:
                    TargetActions.Lookahead = TargetAction.Bandage;
                    break;
            }
            switch (type)
            {
                case 0:
                case 2:
                case 3:
                case 4:
                case 7:
                case 8:
                case 9:
                case 10:
                    {
                        if ((type == 3) || (type == 4))
                        {
                            StreamWriter writer = new StreamWriter("Messages.log", true);
                            writer.WriteLine("Serial = 0x{0:X8}", serial);
                            writer.WriteLine("Font = {0}", font);
                            writer.WriteLine("Hue = {0}", hue);
                            writer.WriteLine("Type = {0}", type);
                            writer.WriteLine("Name = \"{0}\"", name);
                            writer.WriteLine("Text = \"{0}\"", text);
                            writer.WriteLine(new string('#', 20));
                            writer.Flush();
                            writer.Close();
                        }
                        bool flag = false;
                        if ((type == 10) || ((number >= 0x102f6e) && (number <= 0x102f77)))
                        {
                            Spell spellByPower = Spells.GetSpellByPower(text);
                            if (spellByPower == null)
                            {
                                text = text + " - Unknown";
                            }
                            else
                            {
                                if (serial == World.Serial)
                                {
                                    m_LastSpell = spellByPower.Name;
                                    m_TimeLastSpell = DateTime.Now;
                                    TargetActions.Lookahead = (TargetAction)(spellByPower.SpellID - 1);
                                }
                                text = text + " - " + spellByPower.Name;
                            }
                            flag = true;
                        }
                        Mobile owner = World.FindMobile(serial);
                        if (owner != null)
                        {
                            if (flag && !owner.Player)
                            {
                                hue = Hues.GetNotoriety(owner.Notoriety, false);
                            }
                            if (type == 7)
                            {
                                MessageManager.ClearMessages(owner);
                            }
                            owner.AddTextMessage(name, text, font, hue, (type == 10) || (type == 7));
                        }
                        else
                        {
                            Item item = World.FindItem(serial);
                            if (item != null)
                            {
                                if (type == 7)
                                {
                                    MessageManager.ClearMessages(owner);
                                }
                                item.AddTextMessage(name, text, font, hue, (type == 10) || (type == 7));
                            }
                            else
                            {
                                Engine.AddTextMessage(text, font, hue);
                            }
                        }
                        return;
                    }
                case 1:
                    if (name.Length <= 0)
                    {
                        Engine.AddTextMessage(text, font, hue);
                        return;
                    }
                    Engine.AddTextMessage(name + ": " + text, font, hue);
                    return;

                case 6:
                    {
                        Mobile mobile4 = World.FindMobile(serial);
                        if (mobile4 == null)
                        {
                            Item item2 = World.FindItem(serial);
                            if (item2 != null)
                            {
                                item2.AddTextMessage("You see", text, font, hue, false);
                            }
                            else
                            {
                                Engine.AddTextMessage(text, font, hue);
                            }
                            return;
                        }
                        mobile4.AddTextMessage("You see", text, font, hue, false);
                        return;
                    }
            }
            StreamWriter writer2 = new StreamWriter("Messages.log", true);
            writer2.WriteLine("Serial = 0x{0:X8}", serial);
            writer2.WriteLine("Font = {0}", font);
            writer2.WriteLine("Hue = {0}", hue);
            writer2.WriteLine("Type = {0}", type);
            writer2.WriteLine("Name = \"{0}\"", name);
            writer2.WriteLine("Text = \"{0}\"", text);
            writer2.WriteLine(new string('#', 20));
            writer2.Flush();
            writer2.Close();
        }

        internal static void AddSequence(int seq, int x, int y, int z)
        {
            m_Sequences.Enqueue(new int[] { seq, x, y, z });
        }

        private static string ArgReplace_Eval(Match m)
        {
            try
            {
                int index = Convert.ToInt32(m.Groups[1].Value) - 1;
                return m_Args[index];
            }
            catch
            {
                return m.Value;
            }
        }

        private static void Book_Open(PacketReader pvSrc)
        {
            int num = pvSrc.ReadInt32();
            bool flag = pvSrc.ReadBoolean();
            bool flag2 = pvSrc.ReadBoolean();
            int num2 = pvSrc.ReadInt16();
            int fixedLength = pvSrc.ReadInt16();
            string str = pvSrc.ReadString(fixedLength);
            int num4 = pvSrc.ReadInt16();
            string str2 = pvSrc.ReadString(num4);
            m_BookTitle = str;
            m_BookAuthor = str2;
            Engine.AddTextMessage("Books are not currently supported.");
        }

        private static void Book_PageInfo(PacketReader pvSrc)
        {
            int num = pvSrc.ReadInt32();
            int num2 = pvSrc.ReadUInt16();
        }

        private static void BulletinBoard(PacketReader pvSrc)
        {
            switch (pvSrc.ReadByte())
            {
                case 0:
                    BulletinBoard_Display(pvSrc);
                    break;

                case 1:
                    BulletinBoard_SetHeader(pvSrc);
                    break;

                case 2:
                    BulletinBoard_SetBody(pvSrc);
                    break;

                default:
                    pvSrc.Trace();
                    break;
            }
        }

        private static void BulletinBoard_Display(PacketReader pvSrc)
        {
            if (World.FindItem(pvSrc.ReadInt32()) != null)
            {
            }
        }

        private static string BulletinBoard_ReadString(PacketReader pvSrc)
        {
            int length = pvSrc.ReadByte();
            byte[] bytes = pvSrc.ReadBytes(length);
            for (int i = 0; i < length; i++)
            {
                if (bytes[i] == 0)
                {
                    length = i;
                    break;
                }
            }
            return Encoding.UTF8.GetString(bytes, 0, length);
        }

        private static void BulletinBoard_SetBody(PacketReader pvSrc)
        {
            Item board = World.FindItem(pvSrc.ReadInt32());
            if (board != null)
            {
                Item item2 = World.WantItem(pvSrc.ReadInt32());
                if (item2 != null)
                {
                    int serial = pvSrc.ReadInt32();
                    Item thread = null;
                    if (serial >= 0x40000000)
                    {
                        thread = World.WantItem(serial);
                    }
                    string poster = BulletinBoard_ReadString(pvSrc);
                    string subject = BulletinBoard_ReadString(pvSrc);
                    string time = BulletinBoard_ReadString(pvSrc);
                    item2.BulletinHeader = new BBMessageHeader(board, thread, poster, subject, time);
                    int body = pvSrc.ReadUInt16();
                    int hue = pvSrc.ReadUInt16();
                    BBPAItem[] items = new BBPAItem[pvSrc.ReadByte()];
                    for (int i = 0; i < items.Length; i++)
                    {
                        items[i].ItemID = pvSrc.ReadUInt16();
                        items[i].Hue = pvSrc.ReadUInt16();
                    }
                    string[] lines = new string[pvSrc.ReadByte()];
                    for (int j = 0; j < lines.Length; j++)
                    {
                        lines[j] = BulletinBoard_ReadString(pvSrc);
                    }
                    item2.BulletinBody = new BBMessageBody(new BBPosterAppearance(body, hue, items), lines);
                }
            }
        }

        private static void BulletinBoard_SetHeader(PacketReader pvSrc)
        {
            Item board = World.FindItem(pvSrc.ReadInt32());
            if (board != null)
            {
                Item item2 = World.WantItem(pvSrc.ReadInt32());
                if (item2 != null)
                {
                    int serial = pvSrc.ReadInt32();
                    Item thread = null;
                    if (serial >= 0x40000000)
                    {
                        thread = World.WantItem(serial);
                    }
                    string poster = BulletinBoard_ReadString(pvSrc);
                    string subject = BulletinBoard_ReadString(pvSrc);
                    string time = BulletinBoard_ReadString(pvSrc);
                    item2.BulletinHeader = new BBMessageHeader(board, thread, poster, subject, time);
                }
            }
        }

        private static void ChangeCharacter(PacketReader pvSrc)
        {
            int num = pvSrc.ReadByte();
            int num2 = pvSrc.ReadByte();
            string[] strArray = new string[5];
            for (int i = 0; i < 5; i++)
            {
                strArray[i] = pvSrc.ReadString(30);
                pvSrc.Seek(30, SeekOrigin.Current);
            }
            if (!pvSrc.Finished || (num2 != 0))
            {
                pvSrc.Trace();
            }
            Engine.AddTextMessage("That is not supported.");
        }

        private static void CharacterList(PacketReader pvSrc)
        {
            bool flag;
            byte num = pvSrc.ReadByte();
            Cursor.Hourglass = false;
            Engine.CharacterCount = num;
            string[] strArray = Engine.CharacterNames = new string[5];
            int num2 = 0;
            for (int i = 0; i < 5; i++)
            {
                Engine.CharacterNames[i] = strArray[i] = pvSrc.ReadString(60);
                if (strArray[i].Length > 0)
                {
                    num2++;
                }
            }
            pvSrc.ReadString(60);
            int num4 = pvSrc.ReadByte();
            while (--num4 >= 0)
            {
                byte num5 = pvSrc.ReadByte();
                string str = pvSrc.ReadString(0x1f);
                string str2 = pvSrc.ReadString(0x1f);
            }
            if (!pvSrc.Finished)
            {
                int num6 = pvSrc.ReadInt32();
                Engine.ServerFeatures.SingleChar = (num6 & 4) != 0;
                Engine.ServerFeatures.ContextMenus = (num6 & 8) != 0;
                Engine.ServerFeatures.AOS = (num6 & 0x20) != 0;
                if ((num6 & 4) != 0)
                {
                    flag = num2 < 1;
                }
                else
                {
                    flag = num2 < 5;
                }
            }
            else
            {
                flag = num2 < 5;
            }
            if (Engine.m_QuickLogin)
            {
                CharacterProfile charProfile = Engine.m_QuickEntry.CharProfile;
                if (charProfile != null)
                {
                    ShardProfile shard = charProfile.Shard;
                    for (int j = 0; j < 5; j++)
                    {
                        string name = Engine.CharacterNames[j];
                        if (name.Length > 0)
                        {
                            CharacterProfile profile3 = null;
                            for (int k = 0; (profile3 == null) && (k < shard.Characters.Length); k++)
                            {
                                if (shard.Characters[k].Index == j)
                                {
                                    profile3 = shard.Characters[k];
                                }
                            }
                            if (profile3 != null)
                            {
                                profile3.Name = name;
                            }
                            else
                            {
                                shard.AddCharacter(new CharacterProfile(shard, name, j));
                            }
                        }
                        else
                        {
                            CharacterProfile character = null;
                            for (int m = 0; (character == null) && (m < shard.Characters.Length); m++)
                            {
                                if (shard.Characters[m].Index == j)
                                {
                                    character = shard.Characters[m];
                                }
                            }
                            if (character != null)
                            {
                                shard.RemoveCharacter(character);
                            }
                        }
                    }
                    Array.Sort(shard.Characters, new CharacterComparer());
                    Client.Timer timer = new Client.Timer(new OnTick(PacketHandlers.Update_OnTick), 0, 1);
                    timer.SetTag("shard", shard);
                    timer.Start(false);
                }
                QuickLogin.Add(Engine.m_QuickEntry);
                Network.Send(new PCharSelect(Engine.m_QuickEntry.CharName, Engine.m_QuickEntry.CharID));
                if (Animations.IsLoading)
                {
                    Gumps.Desktop.Children.Clear();
                    xGumps.Display("AnimationLoad");
                    do
                    {
                        Engine.DrawNow();
                    }
                    while (!Animations.WaitLoading());
                }
                Gumps.Desktop.Children.Clear();
                xGumps.Display("EnterBritannia");
                Engine.DrawNow();
            }
            else
            {
                xGumps.SetVariable("CharSlotAvailable", flag ? "1" : "0");
                Gumps.Desktop.Children.Clear();
                xGumps.Display("CharacterList");
                int index = 0;
                int num11 = 0;
                while (index < 5)
                {
                    if (strArray[index].Length > 0)
                    {
                        xGumps.SetVariable("CharName", strArray[index]);
                        xGumps.SetVariable("CharDisplayIndex", num11++.ToString());
                        xGumps.SetVariable("CharIndex", index.ToString());
                        xGumps.Display("CharacterEntry", "CharacterList");
                    }
                    index++;
                }
            }
        }

        internal static void ClearWeather()
        {
            Engine.Effects.ClearParticles();
        }

        private static void CloseShopDialog(PacketReader pvSrc)
        {
            int num = pvSrc.ReadInt32();
            if (pvSrc.ReadByte() != 0)
            {
                pvSrc.Trace();
            }
            Gumps.Destroy(Gumps.FindGumpByGUID(string.Format("GSellGump-{0}", num)));
            Gumps.Destroy(Gumps.FindGumpByGUID(string.Format("GBuyGump-{0}", num)));
        }

        private static void Command(PacketReader pvSrc)
        {
            int num = pvSrc.ReadInt16();
            switch (num)
            {
                case 4:
                    pvSrc.ReturnName = "Close Dialog";
                    Command_CloseDialog(pvSrc);
                    return;

                case 6:
                    Command_Party(pvSrc);
                    return;

                case 8:
                    pvSrc.ReturnName = "Set World";
                    Command_SetWorld(pvSrc);
                    return;

                case 0x10:
                    pvSrc.ReturnName = "Equipment Description";
                    Command_EquipInfo(pvSrc);
                    return;

                case 20:
                    pvSrc.ReturnName = "Mobile Popup";
                    Command_Popup(pvSrc);
                    return;

                case 0x17:
                    pvSrc.ReturnName = "Open Wisdom Codex";
                    Command_OpenWisdomCodex(pvSrc);
                    return;

                case 0x18:
                    pvSrc.ReturnName = "Map Patches";
                    Command_MapPatches(pvSrc);
                    return;

                case 0x19:
                    pvSrc.ReturnName = "Extended Status";
                    Command_ExtendedStatus(pvSrc);
                    return;

                case 0x1b:
                    pvSrc.ReturnName = "Spellbook Content";
                    Command_SpellbookContent(pvSrc);
                    return;

                case 0x1d:
                    pvSrc.ReturnName = "Custom House";
                    Command_CustomHouse(pvSrc);
                    return;

                case 0x20:
                    pvSrc.ReturnName = "Edit Custom House";
                    Command_EditCustomHouse(pvSrc);
                    return;

                case 0x21:
                    pvSrc.ReturnName = "Clear Combat Ability";
                    AbilityInfo.ClearActive();
                    return;

                case 0x22:
                    pvSrc.ReturnName = "Damage";
                    Command_Damage(pvSrc);
                    return;
            }
            Debug.Trace("Unhandled subcommand {0} ( 0x{0:X4} )", num);
            pvSrc.Trace();
        }

        private static void Command_CloseDialog(PacketReader pvSrc)
        {
            int num = pvSrc.ReadInt32();
            int buttonID = pvSrc.ReadInt32();
            Gump[] gumpArray = Gumps.Desktop.Children.ToArray();
            for (int i = 0; i < gumpArray.Length; i++)
            {
                if (gumpArray[i] is GServerGump)
                {
                    GServerGump g = (GServerGump)gumpArray[i];
                    if (g.DialogID == num)
                    {
                        GServerGump.SetCachedLocation(g.DialogID, g.X, g.Y);
                        Gumps.Destroy(g);
                        Network.Send(new PGumpButton(g, buttonID));
                    }
                }
            }
        }

        private static void Command_CustomHouse(PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            int revision = pvSrc.ReadInt32();
            Item item = World.WantItem(serial);
            if (item.Revision != revision)
            {
                item.Revision = revision;
                if (CustomMultiLoader.GetCustomMulti(serial, revision) == null)
                {
                    Network.Send(new PQueryCustomHouse(serial));
                }
                else
                {
                    Map.Invalidate();
                    GRadar.Invalidate();
                }
            }
        }

        private static void Command_Damage(PacketReader pvSrc)
        {
            if (pvSrc.ReadByte() != 1)
            {
                pvSrc.Trace();
            }
            Mobile m = World.FindMobile(pvSrc.ReadInt32());
            if (m != null)
            {
                int damage = pvSrc.ReadByte();
                if (damage > 0)
                {
                    Gumps.Desktop.Children.Add(new GDamageLabel(damage, m));
                }
            }
        }

        private static void Command_EditCustomHouse(PacketReader pvSrc)
        {
            if (World.FindItem(pvSrc.ReadInt32()) != null)
            {
                int num = pvSrc.ReadByte();
            }
        }

        private static void Command_EquipInfo(PacketReader pvSrc)
        {
            if (Engine.Features.AOS)
            {
                int serial = pvSrc.ReadInt32();
                int num2 = pvSrc.ReadInt32();
                Item item = World.FindItem(serial);
                if (item != null)
                {
                    item.PropertyID = num2;
                    if (((item.Parent != null) && ((item.Parent.ID & 0x3fff) == 0x2006)) && (item.PropertyList == null))
                    {
                        item.QueryProperties();
                    }
                }
                Mobile mobile = World.FindMobile(serial);
                if (mobile != null)
                {
                    mobile.PropertyID = num2;
                }
            }
            else
            {
                int num5;
                IFont uniFont = Engine.GetUniFont(3);
                IHue bright = Hues.Bright;
                int num3 = pvSrc.ReadInt32();
                int number = pvSrc.ReadInt32();
                AddMessage(num3, uniFont, bright, 6, "You see", Localization.GetString(number));
                ArrayList dataStore = Engine.GetDataStore();
                while (!pvSrc.Finished && ((num5 = pvSrc.ReadInt32()) != -1))
                {
                    if (num5 < 0)
                    {
                        switch (num5)
                        {
                            case -4:
                                {
                                    AddMessage(num3, uniFont, bright, 6, "", "[" + Localization.GetString(0xfd6b0) + "]");
                                    continue;
                                }
                            case -3:
                                {
                                    int fixedLength = pvSrc.ReadInt16();
                                    string str = pvSrc.ReadString(fixedLength).Trim();
                                    if (str.Length > 0)
                                    {
                                        AddMessage(num3, uniFont, bright, 6, "", Localization.GetString(0xfd2d1) + " " + str);
                                    }
                                    continue;
                                }
                        }
                        Engine.ReleaseDataStore(dataStore);
                        pvSrc.Trace();
                        Engine.AddTextMessage(string.Format("Unknown sub message : {0}", num5));
                        return;
                    }
                    int num7 = pvSrc.ReadInt16();
                    if (num7 != -1)
                    {
                        dataStore.Add(Localization.GetString(num5) + ": " + num7);
                    }
                    else
                    {
                        dataStore.Add(Localization.GetString(num5));
                    }
                }
                if (dataStore.Count > 0)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append('[');
                    for (int i = 0; i < dataStore.Count; i++)
                    {
                        builder.Append(dataStore[i]);
                        if (i != (dataStore.Count - 1))
                        {
                            builder.Append('/');
                        }
                    }
                    builder.Append(']');
                    AddMessage(num3, uniFont, bright, 6, "", builder.ToString());
                }
                if (!pvSrc.Finished)
                {
                    pvSrc.Trace();
                }
                Engine.ReleaseDataStore(dataStore);
            }
        }

        private static void Command_ExtendedStatus(PacketReader pvSrc)
        {
            int num = pvSrc.ReadByte();
            Mobile mobile = World.FindMobile(pvSrc.ReadInt32());
            if (mobile != null)
            {
                mobile.Bonded = pvSrc.ReadBoolean();
                if (num >= 2)
                {
                    int num2 = pvSrc.ReadByte();
                    if (num >= 3)
                    {
                        pvSrc.Trace();
                    }
                }
            }
        }

        private static void Command_MapPatches(PacketReader pvSrc)
        {
            int num = pvSrc.ReadInt32();
            if (num > 5)
            {
                pvSrc.Trace();
            }
            for (int i = 0; i < num; i++)
            {
                int num3 = pvSrc.ReadInt32();
                int num4 = pvSrc.ReadInt32();
            }
        }

        private static void Command_OpenWisdomCodex(PacketReader pvSrc)
        {
            int num = pvSrc.ReadByte();
            int num2 = pvSrc.ReadInt32();
            int num3 = pvSrc.ReadByte();
            if ((num != 1) && (num3 != 1))
            {
                pvSrc.Trace();
            }
        }

        private static void Command_Party(PacketReader pvSrc)
        {
            int num = pvSrc.ReadByte();
            switch (num)
            {
                case 1:
                    {
                        pvSrc.ReturnName = "Party Member List";
                        int num2 = pvSrc.ReadByte();
                        Mobile[] mobileArray = new Mobile[num2];
                        for (int i = 0; i < num2; i++)
                        {
                            mobileArray[i] = World.WantMobile(pvSrc.ReadInt32());
                            mobileArray[i].QueryStats();
                        }
                        Party.State = PartyState.Joined;
                        Party.Members = mobileArray;
                        int num4 = (Engine.GameY + Engine.GameHeight) - 50;
                        for (int j = 0; j < num2; j++)
                        {
                            if (!mobileArray[j].Player)
                            {
                                if (mobileArray[j].StatusBar == null)
                                {
                                    mobileArray[j].OpenStatus(false);
                                    if (mobileArray[j].StatusBar != null)
                                    {
                                        mobileArray[j].StatusBar.Gump.X = ((Engine.GameX + Engine.GameWidth) - 30) - mobileArray[j].StatusBar.Gump.Width;
                                        mobileArray[j].StatusBar.Gump.Y = num4 - mobileArray[j].StatusBar.Gump.Height;
                                        num4 -= mobileArray[j].StatusBar.Gump.Height + 5;
                                    }
                                }
                                else
                                {
                                    num4 -= mobileArray[j].StatusBar.Gump.Height + 5;
                                }
                            }
                        }
                        return;
                    }
                case 2:
                    {
                        pvSrc.ReturnName = "Remove Party Member";
                        int num6 = pvSrc.ReadByte();
                        int num7 = pvSrc.ReadInt32();
                        Mobile[] mobileArray2 = new Mobile[num6];
                        for (int k = 0; k < num6; k++)
                        {
                            mobileArray2[k] = World.WantMobile(pvSrc.ReadInt32());
                            mobileArray2[k].QueryStats();
                        }
                        Party.State = PartyState.Joined;
                        Party.Members = mobileArray2;
                        return;
                    }
                case 3:
                case 4:
                    {
                        string str2;
                        IHue hue;
                        pvSrc.ReturnName = (num == 3) ? "Private Party Message" : "Public Party Message";
                        int serial = pvSrc.ReadInt32();
                        string str = pvSrc.ReadUnicodeString();
                        Mobile mobile = World.FindMobile(serial);
                        if (((mobile == null) || ((str2 = mobile.Name) == null)) || ((str2 = str2.Trim()).Length <= 0))
                        {
                            str2 = "Someone";
                        }
                        if (str == "I'm stunned !!")
                        {
                            hue = Hues.Load(0x22);
                            if (mobile != null)
                            {
                                Engine.Sounds.PlaySound(0x157, mobile.X, mobile.Y, mobile.Z);
                            }
                        }
                        else if (str.StartsWith("I stunned ") && str.EndsWith(" !!"))
                        {
                            hue = Hues.Load(0x22);
                            if (mobile != null)
                            {
                                Engine.Sounds.PlaySound(0x1e1, mobile.X, mobile.Y, mobile.Z);
                            }
                        }
                        else if (str.StartsWith("Changing last target to "))
                        {
                            hue = Hues.Load(0x35);
                        }
                        else if (num == 3)
                        {
                            hue = Hues.Load(World.CharData.WhisperHue);
                        }
                        else
                        {
                            hue = Hues.Load(World.CharData.TextHue);
                        }
                        Engine.AddTextMessage(string.Format("<{0}{1}> {2}", (num == 3) ? "Whisper: " : "", str2, str), Engine.DefaultFont, hue);
                        return;
                    }
                case 7:
                    {
                        pvSrc.ReturnName = "Party Invitation";
                        int num10 = pvSrc.ReadInt32();
                        Party.State = PartyState.Joining;
                        Party.Leader = World.WantMobile(num10);
                        return;
                    }
            }
            pvSrc.ReturnName = "Unknown Party Message";
            pvSrc.Trace();
        }

        private static void Command_Popup(PacketReader pvSrc)
        {
            if (pvSrc.ReadInt16() != 1)
            {
                pvSrc.Trace();
            }
            int serial = pvSrc.ReadInt32();
            int num2 = pvSrc.ReadByte();
            PopupEntry[] list = new PopupEntry[num2];
            object o = World.FindMobile(serial);
            if (o == null)
            {
                o = World.FindItem(serial);
            }
            for (int i = 0; i < num2; i++)
            {
                int entryID = pvSrc.ReadInt16();
                int stringID = pvSrc.ReadInt16();
                int flags = pvSrc.ReadInt16();
                list[i] = new PopupEntry(entryID, stringID, flags);
                if (((flags & 0x20) != 0) && (pvSrc.ReadInt16() != -1))
                {
                }
                if ((stringID == Engine.m_ContextQueue) && (o != null))
                {
                    Network.Send(new PPopupResponse(o, entryID));
                    Engine.m_ContextQueue = -1;
                    return;
                }
            }
            if (o != null)
            {
                GContextMenu.Display(o, list);
            }
        }

        private static void Command_SetWorld(PacketReader pvSrc)
        {
            int index = pvSrc.ReadByte();
            if (index < m_WorldNames.Length)
            {
                Engine.m_World = index;
                Engine.m_regMap = index < 2;
                Cursor.Gold = index > 0;
                if (index != m_LastWorld)
                {
                    if (m_LastWorld != -1)
                    {
                        Engine.AddTextMessage(string.Format("You enter {0}.", m_WorldNames[index]));
                    }
                    m_LastWorld = index;
                }
                Map.Invalidate();
                GRadar.Invalidate();
            }
            else
            {
                pvSrc.Trace();
            }
        }

        private static void Command_SpellbookContent(PacketReader pvSrc)
        {
            if (pvSrc.ReadInt16() != 1)
            {
                pvSrc.Trace();
            }
            Item container = World.FindItem(pvSrc.ReadInt32());
            if ((container != null) && container.QueueOpenSB)
            {
                container.QueueOpenSB = false;
                container.SpellbookGraphic = pvSrc.ReadInt16();
                container.SpellbookOffset = pvSrc.ReadInt16();
                for (int i = 0; i < 8; i++)
                {
                    int num3 = pvSrc.ReadByte();
                    for (int j = 0; j < 8; j++)
                    {
                        container.SetSpellContained((i * 8) + j, (num3 & (((int)1) << j)) != 0);
                    }
                }
                if (!container.OpenSB)
                {
                    container.OpenSB = true;
                    Spells.OpenSpellbook(container);
                }
                else
                {
                    Gump gump = Gumps.FindGumpByGUID(string.Format("Spellbook Icon #{0}", container.Serial));
                    if (gump != null)
                    {
                        ((GSpellbookIcon)gump).OnDoubleClick(gump.Width / 2, gump.Height / 2);
                    }
                }
            }
        }

        private static void Container_Item(PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            int itemID = pvSrc.ReadInt16() + pvSrc.ReadSByte();
            int num3 = pvSrc.ReadInt16();
            int num4 = pvSrc.ReadInt16();
            int num5 = pvSrc.ReadInt16();
            int num6 = pvSrc.ReadInt32();
            int hue = pvSrc.ReadUInt16();
            bool flag = serial < 0x40000000;
            bool flag2 = num6 < 0x40000000;
            if (flag && flag2)
            {
                Mobile mobile = World.FindMobile(serial);
                if ((mobile != null) && mobile.Visible)
                {
                    mobile.Visible = false;
                    mobile.Update();
                }
            }
            else if (!flag && !flag2)
            {
                Item item = World.WantItem(serial);
                Item item2 = World.WantItem(num6);
                item.Query();
                item.Visible = true;
                if (item.InWorld)
                {
                    Map.RemoveItem(item);
                    item.InWorld = false;
                }
                else if (item.IsEquip)
                {
                    item.RemoveEquip();
                }
                else if ((item.Parent != null) && (item.Parent != item2))
                {
                    item.Parent.RemoveItem(item);
                }
                Engine.ItemArt.Translate(ref itemID, ref hue);
                item.ID = (short)itemID;
                item.Hue = (ushort)hue;
                item.Amount = (short)num3;
                item.ContainerX = (short)num4;
                item.ContainerY = (short)num5;
                item2.AddItem(item);
                if (((item.Parent != null) && ((item.Parent.ID & 0x3fff) == 0x2006)) && (item.PropertyList == null))
                {
                    item.QueryProperties();
                }
            }
        }

        private static void Container_Items(PacketReader pvSrc)
        {
            ArrayList dataStore = Engine.GetDataStore();
            int count = pvSrc.ReadInt16();
            for (int i = 0; i < count; i++)
            {
                Item item = World.WantItem(pvSrc.ReadInt32());
                int itemID = pvSrc.ReadInt16() + pvSrc.ReadSByte();
                int num4 = pvSrc.ReadInt16();
                int num5 = pvSrc.ReadInt16();
                int num6 = pvSrc.ReadInt16();
                Item item2 = World.WantItem(pvSrc.ReadInt32());
                item.Query();
                int hue = pvSrc.ReadInt16();
                item.Visible = true;
                if (item.InWorld)
                {
                    Map.RemoveItem(item);
                    item.InWorld = false;
                }
                else if (item.IsEquip)
                {
                    item.RemoveEquip();
                }
                else if ((item.Parent != null) && (item.Parent != item2))
                {
                    item.Parent.RemoveItem(item);
                }
                Engine.ItemArt.Translate(ref itemID, ref hue);
                item.ID = (short)itemID;
                item.Hue = (ushort)hue;
                item.Amount = (short)num4;
                item.ContainerX = (short)num5;
                item.ContainerY = (short)num6;
                item2.AddItem(item);
                if (!dataStore.Contains(item2))
                {
                    dataStore.Add(item2);
                }
                if (((item.Parent != null) && ((item.Parent.ID & 0x3fff) == 0x2006)) && (item.PropertyList == null))
                {
                    item.QueryProperties();
                }
            }
            count = dataStore.Count;
            for (int j = 0; j < count; j++)
            {
                Item container = (Item)dataStore[j];
                if (container.QueueOpenSB)
                {
                    container.QueueOpenSB = false;
                    container.SpellbookGraphic = container.ID;
                    container.SpellbookOffset = Spells.GetBookOffset(container.SpellbookGraphic);
                    container.SpellContained = 0L;
                    for (int k = 0; k < container.Items.Count; k++)
                    {
                        container.SetSpellContained(((Item)container.Items[k]).Amount - container.SpellbookOffset, true);
                    }
                    if (!container.OpenSB)
                    {
                        container.OpenSB = true;
                        Spells.OpenSpellbook(container);
                    }
                    else
                    {
                        Gump gump = Gumps.FindGumpByGUID(string.Format("Spellbook Icon #{0}", container.Serial));
                        if (gump != null)
                        {
                            ((GSpellbookIcon)gump).OnDoubleClick(gump.Width / 2, gump.Height / 2);
                        }
                    }
                }
                MakeRegsTargetHandler.CheckQueue(container);
            }
            Engine.ReleaseDataStore(dataStore);
        }

        private static void Container_Open(PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            int gumpID = pvSrc.ReadInt16();
            int num3 = 0;
            if (gumpID == 0x30)
            {
                num3 = serial;
                serial = m_BuyMenuSerial;
            }
            Item item = World.WantItem(serial);
            int num4 = 10 + ((Engine.m_OpenedGumps++ % 20) * 10);
            if (item.Container != null)
            {
                item.Container.Close();
            }
            switch (gumpID)
            {
                case -1:
                    item.QueueOpenSB = true;
                    break;

                case 8:
                    {
                        Mobile mobile = World.FindMobile(serial);
                        mobile.BigStatus = true;
                        mobile.OpenStatus(false);
                        break;
                    }
                case 0x30:
                    if ((m_BuyMenuPrices != null) && (m_BuyMenuNames != null))
                    {
                        ArrayList list = item.Items;
                        int count = list.Count;
                        if (count > m_BuyMenuPrices.Length)
                        {
                            count = m_BuyMenuPrices.Length;
                        }
                        if (count > m_BuyMenuNames.Length)
                        {
                            count = m_BuyMenuNames.Length;
                        }
                        BuyInfo[] infoArray = new BuyInfo[count];
                        BuyInfo info = null;
                        for (int i = 0; i < count; i++)
                        {
                            infoArray[i] = new BuyInfo((Item)list[i], m_BuyMenuPrices[i], m_BuyMenuNames[i]);
                            if (((infoArray[i].ItemID == 0x2124) && (Engine.m_BuyHorse != null)) && (Engine.m_BuyHorse.Serial == num3))
                            {
                                info = infoArray[i];
                                info.dToBuy = 1.0;
                                info.ToBuy = 1;
                                Engine.m_BuyHorse = null;
                            }
                        }
                        if (info == null)
                        {
                            Gumps.Desktop.Children.Add(new GBuyGump(num3, infoArray));
                        }
                        else
                        {
                            Engine.AddTextMessage("Purchasing items.");
                            Network.Send(new PBuyItems(num3, infoArray));
                        }
                        m_BuyMenuPrices = null;
                        m_BuyMenuNames = null;
                    }
                    break;

                default:
                    IContainer container;
                    Engine.Sounds.PlayContainerOpen(gumpID);
                    if (((gumpID == 9) && (item.LastTextHue != null)) && ((item.LastTextHue.HueID() & 0x7fff) == 0x59))
                    {
                        container = new GContainer(item, gumpID, Hues.GetNotoriety(Notoriety.Innocent));
                    }
                    else
                    {
                        container = new GContainer(item, gumpID);
                    }
                    Gumps.Desktop.Children.Add(container.Gump);
                    item.Container = container;
                    break;
            }
        }

        private static void CorpseEquip(PacketReader pvSrc)
        {
            Layer layer;
            Item item = World.WantItem(pvSrc.ReadInt32());
            item.CorpseEquip.Clear();
            while ((layer = (Layer)pvSrc.ReadByte()) != Layer.Invalid)
            {
                layer = (Layer)(((int)layer) + 1);
                Item item2 = World.FindItem(pvSrc.ReadInt32());
                if ((layer < Layer.Mount) && (item2 != null))
                {
                    item.CorpseEquip.Add(new EquipEntry(item2, Map.GetAnimation(item2.ID), layer));
                }
            }
            item.CorpseEquip.Sort(LayerComparer.FromDirection(item.Direction));
        }

        private static void CurrentTarget(PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            Renderer.AlwaysHighlight = serial;
            if (serial != 0)
            {
                Mobile mobile = World.FindMobile(serial);
                if (mobile != null)
                {
                    Engine.m_LastAttacker = mobile;
                    mobile.QueryStats();
                }
            }
        }

        private static void Custom(PacketReader pvSrc)
        {
            switch (pvSrc.ReadByte())
            {
                case 0:
                    Custom_Accept(pvSrc);
                    break;

                case 1:
                    Custom_AckPartyLocs(pvSrc);
                    break;
            }
        }

        private static void Custom_Accept(PacketReader pvSrc)
        {
            switch (pvSrc.ReadByte())
            {
                case 0:
                    Debug.Trace("Connection rejected.");
                    Network.m_SoftDisconnect = true;
                    Network.Disconnect();
                    Gumps.Desktop.Children.Clear();
                    xGumps.SetVariable("FailMessage", "You are not authorized to use this server.");
                    xGumps.Display("ConnectionFailed");
                    Cursor.Hourglass = false;
                    break;

                case 1:
                    Debug.Trace("Connection accepted, lacking GM privileges.");
                    break;

                case 2:
                    Debug.Trace("GM privileges acknowledged.");
                    Engine.AddTextMessage("Connection accepted, GM privileges acknowledged.");
                    Engine.m_GMPrivs = true;
                    break;
            }
        }

        private static void Custom_AckPartyLocs(PacketReader pvSrc)
        {
            int num;
            while ((num = pvSrc.ReadInt32()) > 0)
            {
                Mobile mobile = World.FindMobile(num);
                int x = pvSrc.ReadInt16();
                int y = pvSrc.ReadInt16();
                int num4 = pvSrc.ReadByte();
                if (mobile != null)
                {
                    mobile.m_KUOC_X = x;
                    mobile.m_KUOC_Y = y;
                    mobile.m_KUOC_F = num4;
                    if (((num4 == Engine.m_World) && (mobile.Name != null)) && (mobile.Name.Length > 0))
                    {
                        GRadar.AddTag(x, y, mobile.Name, mobile.Serial);
                    }
                }
            }
        }

        private static void CustomizedHouseContent(PacketReader pvSrc)
        {
            int compressionType = pvSrc.ReadByte();
            int num2 = pvSrc.ReadByte();
            int serial = pvSrc.ReadInt32();
            int revision = pvSrc.ReadInt32();
            int num5 = pvSrc.ReadUInt16();
            int length = pvSrc.ReadUInt16();
            byte[] buffer = pvSrc.ReadBytes(length);
            Item item = World.FindItem(serial);
            if (((item != null) && (item.Multi != null)) && item.IsMulti)
            {
                CustomMultiLoader.SetCustomMulti(serial, revision, item.Multi, compressionType, buffer);
            }
        }

        private static void DeleteObject(PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            if ((serial & 0x40000000) == 0)
            {
                World.Remove(World.FindMobile(serial));
            }
            else
            {
                World.Remove(World.FindItem(serial));
            }
        }

        private static void DisplayGump(PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            int dialogID = pvSrc.ReadInt32();
            int x = pvSrc.ReadInt32();
            int y = pvSrc.ReadInt32();
            string layout = pvSrc.ReadString(pvSrc.ReadUInt16());
            string[] text = new string[pvSrc.ReadUInt16()];
            for (int i = 0; i < text.Length; i++)
            {
                text[i] = pvSrc.ReadUnicodeString(pvSrc.ReadUInt16());
            }
            GServerGump.GetCachedLocation(dialogID, ref x, ref y);
            GServerGump toAdd = new GServerGump(serial, dialogID, x, y, layout, text);
            Gumps.Desktop.Children.Add(toAdd);
        }

        private static void DisplayPaperdoll(PacketReader pvSrc)
        {
            Mobile m = World.FindMobile(pvSrc.ReadInt32());
            if (m != null)
            {
                m.PaperdollName = pvSrc.ReadString(60);
                byte num = pvSrc.ReadByte();
                m.PaperdollCanDrag = m.Player || ((num & 2) != 0);
                Gumps.OpenPaperdoll(m, m.PaperdollName, m.PaperdollCanDrag);
            }
        }

        private static void DisplayProfile(PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            string header = pvSrc.ReadString();
            string footer = pvSrc.ReadUnicodeString();
            string body = pvSrc.ReadUnicodeString();
            Mobile owner = World.FindMobile(serial);
            if (owner != null)
            {
                Gumps.Desktop.Children.Add(new GCharacterProfile(owner, header, body, footer));
            }
        }

        private static void DisplayQuestionMenu(PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            int menuID = pvSrc.ReadInt16();
            string question = pvSrc.ReadString(pvSrc.ReadByte());
            AnswerEntry[] answers = new AnswerEntry[pvSrc.ReadByte()];
            for (int i = 0; i < answers.Length; i++)
            {
                answers[i] = new AnswerEntry(i, pvSrc.ReadInt16(), pvSrc.ReadUInt16(), pvSrc.ReadString(pvSrc.ReadByte()));
            }
            if ((answers.Length > 0) && (answers[0].ItemID != 0))
            {
                Gumps.Desktop.Children.Add(new GItemList(serial, menuID, question, answers));
            }
            else
            {
                Gumps.Desktop.Children.Add(new GQuestionMenu(serial, menuID, question, answers));
            }
        }

        private static void DragItem(PacketReader pvSrc)
        {
            int itemID = pvSrc.ReadInt16();
            if (pvSrc.ReadByte() != 0)
            {
                pvSrc.Trace();
            }
            ushort hue = pvSrc.ReadUInt16();
            int num3 = pvSrc.ReadUInt16();
            int sourceSerial = pvSrc.ReadInt32();
            int xSource = pvSrc.ReadInt16();
            int ySource = pvSrc.ReadInt16();
            int zSource = pvSrc.ReadSByte();
            int targetSerial = pvSrc.ReadInt32();
            int xTarget = pvSrc.ReadInt16();
            int yTarget = pvSrc.ReadInt16();
            int zTarget = pvSrc.ReadSByte();
            bool shouldDouble = false;
            shouldDouble = Map.m_ItemFlags[itemID & 0x3fff][TileFlag.Generic] && (num3 > 1);
            if ((itemID >= 0xeea) && (itemID <= 0xef2))
            {
                int num12 = (itemID - 0xeea) / 3;
                num12 *= 3;
                num12 += 0xeea;
                shouldDouble = false;
                if (num3 <= 1)
                {
                    itemID = num12;
                }
                else if ((num3 >= 2) && (num3 <= 5))
                {
                    itemID = num12 + 1;
                }
                else
                {
                    itemID = num12 + 2;
                }
            }
            Engine.Effects.Add(new DragEffect(itemID, sourceSerial, xSource, ySource, zSource, targetSerial, xTarget, yTarget, zTarget, Hues.GetItemHue(itemID, hue), shouldDouble));
        }

        private static void Drop_Accept(PacketReader pvSrc)
        {
        }

        private static void Drop_Reject(PacketReader pvSrc)
        {
            int num = pvSrc.ReadInt16();
            int num2 = pvSrc.ReadInt16();
        }

        private static void Effect(PacketReader pvSrc, bool hasHueData, bool hasParticleData)
        {
            Client.Effect effect;
            int num = pvSrc.ReadByte();
            int source = pvSrc.ReadInt32();
            int target = pvSrc.ReadInt32();
            int num4 = pvSrc.ReadInt16();
            int xSource = pvSrc.ReadInt16();
            int ySource = pvSrc.ReadInt16();
            int zSource = pvSrc.ReadSByte();
            int xTarget = pvSrc.ReadInt16();
            int yTarget = pvSrc.ReadInt16();
            int zTarget = pvSrc.ReadSByte();
            int num11 = pvSrc.ReadByte();
            int duration = pvSrc.ReadByte();
            int num13 = pvSrc.ReadByte();
            int num14 = pvSrc.ReadByte();
            bool flag = !pvSrc.ReadBoolean();
            bool flag2 = pvSrc.ReadBoolean();
            int hue = hasHueData ? pvSrc.ReadInt32() : 0;
            int num16 = hasHueData ? pvSrc.ReadInt32() : 0;
            int num17 = hasParticleData ? pvSrc.ReadInt16() : 0;
            int num18 = hasParticleData ? pvSrc.ReadInt16() : 0;
            int num19 = hasParticleData ? pvSrc.ReadInt16() : 0;
            int num20 = hasParticleData ? pvSrc.ReadInt32() : 0;
            EffectLayer layer = hasParticleData ? ((EffectLayer)pvSrc.ReadByte()) : EffectLayer.Head;
            int num21 = hasParticleData ? pvSrc.ReadInt16() : 0;
            if ((((((num17 == 0x139d) || (num17 == 0x13bc)) || ((num17 == 0x13b4) || (num17 == 0xbe3))) || (((num17 == 0x251e) || (num17 == 0x1395)) || ((num == 1) || (num17 == 0xbbe)))) || (num17 == 0x13ae)) || (num4 == 0x113a))
            {
                m_EffectTime = DateTime.Now;
            }
            if ((num4 & 0x3fff) > 1)
            {
                if ((num13 > 1) || (num14 != 0))
                {
                    pvSrc.Trace();
                }
                if (hue > 0)
                {
                    hue++;
                }
                switch (num)
                {
                    case 0:
                        effect = new MovingEffect(source, target, xSource, ySource, zSource, xTarget, yTarget, zTarget, num4, Hues.GetItemHue(num4, hue));
                        ((MovingEffect)effect).m_RenderMode = num16;
                        if (flag2)
                        {
                            effect.Children.Add(new AnimatedItemEffect(target, xTarget, yTarget, zTarget, 0x36cb, Hues.GetItemHue(0x36cb, hue), duration));
                        }
                        goto Label_0274;

                    case 1:
                        effect = new LightningEffect(source, xSource, ySource, zSource, Hues.Load(hue ^ 0x8000));
                        goto Label_0274;

                    case 2:
                        effect = new AnimatedItemEffect(xSource, ySource, zSource, num4, Hues.GetItemHue(num4, hue), duration);
                        ((AnimatedItemEffect)effect).m_RenderMode = num16;
                        goto Label_0274;

                    case 3:
                        effect = new AnimatedItemEffect(source, xSource, ySource, zSource, num4, Hues.GetItemHue(num4, hue), duration);
                        ((AnimatedItemEffect)effect).m_RenderMode = num16;
                        goto Label_0274;
                }
                pvSrc.Trace();
            }
            return;
        Label_0274:
            Engine.Effects.Add(effect);
        }

        private static string EffLay(EffectLayer layer)
        {
            if (System.Enum.IsDefined(typeof(EffectLayer), layer))
            {
                return string.Format("EffectLayer.{0}", layer);
            }
            int num = (int)layer;
            if (num < 0)
            {
                return string.Format("(EffectLayer)({0})", num);
            }
            return string.Format("(EffectLayer){0}", num);
        }

        private static void EquipItem(PacketReader pvSrc)
        {
            Item item = World.WantItem(pvSrc.ReadInt32());
            int itemID = pvSrc.ReadInt16() & 0x3fff;
            item.Query();
            if (pvSrc.ReadByte() != 0)
            {
                pvSrc.Trace();
            }
            Layer layer = (Layer)pvSrc.ReadByte();
            Mobile mobile = World.FindMobile(pvSrc.ReadInt32());
            if (mobile != null)
            {
                short animation;
                int hue = pvSrc.ReadUInt16();
                item.Visible = true;
                if (item.InWorld)
                {
                    Map.RemoveItem(item);
                    item.InWorld = false;
                }
                else if (item.IsEquip)
                {
                    item.RemoveEquip();
                }
                else if (item.Parent != null)
                {
                    item.Parent.RemoveItem(item);
                }
                Engine.ItemArt.Translate(ref itemID, ref hue);
                item.ID = (short)itemID;
                item.Hue = (ushort)hue;
                item.IsEquip = true;
                item.EquipParent = mobile;
                if (layer == Layer.Mount)
                {
                    animation = (short)Engine.m_Animations.ConvertMountItemToBody(itemID);
                }
                else
                {
                    animation = Map.GetAnimation(itemID);
                }
                mobile.AddEquip(new EquipEntry(item, animation, layer));
            }
        }

        private static void Features(PacketReader pvSrc)
        {
            int num = pvSrc.ReadUInt16();
            Engine.Features.Chat = (num & 2) != 0;
            Engine.Features.LBR = (num & 8) != 0;
            Engine.Features.AOS = (num & 0x10) != 0;
        }

        private static void FightOccurring(PacketReader pvSrc)
        {
            if (pvSrc.ReadByte() != 0)
            {
                pvSrc.Trace();
            }
            Mobile mobile = World.FindMobile(pvSrc.ReadInt32());
            Mobile mobile2 = World.FindMobile(pvSrc.ReadInt32());
            if ((mobile != null) && !mobile.Player)
            {
                mobile.QueryStats();
            }
            if ((mobile2 != null) && !mobile2.Player)
            {
                mobile2.QueryStats();
            }
        }

        private static void GameTime(PacketReader pvSrc)
        {
            Engine.m_GameHour = pvSrc.ReadByte();
            Engine.m_GameMinute = pvSrc.ReadByte();
            Engine.m_GameSecond = pvSrc.ReadByte();
        }

        private static void GQCount(PacketReader pvSrc)
        {
            int num = pvSrc.ReadInt16();
            int num2 = pvSrc.ReadInt32();
            switch (num2)
            {
                case 0:
                    Engine.AddTextMessage("There are currently no calls in the global queue which you can answer.", Engine.GetFont(3), Hues.Load(0x22));
                    break;

                case 1:
                    Engine.AddTextMessage("There is currently 1 call in the global queue which you can answer.", Engine.GetFont(3), Hues.Load(0x22));
                    break;

                default:
                    Engine.AddTextMessage(string.Format("There are currently {0} calls in the global queue which you can answer.", num2), Engine.GetFont(3), Hues.Load(0x22));
                    break;
            }
        }

        private static void HuedEffect(PacketReader pvSrc)
        {
            Effect(pvSrc, true, false);
        }

        private static void ItemPickupFailed(PacketReader pvSrc)
        {
            int index = pvSrc.ReadByte();
            if (index < m_IPFReason.Length)
            {
                Engine.AddTextMessage(m_IPFReason[index], Engine.GetFont(3), Hues.Default);
            }
            else if ((index != 5) && (index != 0xff))
            {
                pvSrc.Trace();
            }
            Item item = PPickupItem.m_Item;
            if (item != null)
            {
                if (((Gumps.Drag != null) && (Gumps.Drag.GetType() == typeof(GDraggedItem))) && (((GDraggedItem)Gumps.Drag).Item == item))
                {
                    Gumps.Destroy(Gumps.Drag);
                }
                RestoreInfo restoreInfo = item.RestoreInfo;
                if (restoreInfo != null)
                {
                    if (item.InWorld)
                    {
                        Map.RemoveItem(item);
                        item.InWorld = false;
                    }
                    else if (item.IsEquip)
                    {
                        item.RemoveEquip();
                    }
                    else if (item.Parent != null)
                    {
                        item.Parent.RemoveItem(item);
                    }
                    item.Visible = true;
                    if (restoreInfo.m_InWorld)
                    {
                        item.SetLocation((short)restoreInfo.m_X, (short)restoreInfo.m_Y, (short)restoreInfo.m_Z);
                        item.InWorld = true;
                        item.Update();
                    }
                    else if (restoreInfo.m_Parent != null)
                    {
                        restoreInfo.m_Parent.AddItem(item);
                    }
                    else if (restoreInfo.m_IsEquip && (restoreInfo.m_EquipEntry != null))
                    {
                        if (restoreInfo.m_EquipParent is Mobile)
                        {
                            Mobile equipParent = (Mobile)restoreInfo.m_EquipParent;
                            equipParent.AddEquip(restoreInfo.m_EquipEntry);
                            if (equipParent.Paperdoll != null)
                            {
                                Gumps.OpenPaperdoll(equipParent, equipParent.PaperdollName, equipParent.PaperdollCanDrag);
                            }
                        }
                        else if (restoreInfo.m_EquipParent is Item)
                        {
                            ((Item)restoreInfo.m_EquipParent).Equip.Add(restoreInfo.m_EquipEntry);
                        }
                        item.IsEquip = true;
                    }
                }
                item.RestoreInfo = null;
            }
        }

        private static void LaunchBrowser(PacketReader pvSrc)
        {
            string url = pvSrc.ReadString();
            if (Engine.m_Fullscreen)
            {
                Engine.AddTextMessage("Cannot open browser in fullscreen.");
            }
            else
            {
                Engine.OpenBrowser(url);
            }
        }

        internal static void Light_Global(PacketReader pvSrc)
        {
            Engine.Effects.GlobalLight = pvSrc.ReadSByte();
        }

        internal static void Light_Personal(PacketReader pvSrc)
        {
            Mobile mobile = World.FindMobile(pvSrc.ReadInt32());
            if (mobile != null)
            {
                mobile.LightLevel = pvSrc.ReadSByte();
            }
        }

        private static void LoginComplete(PacketReader pvSrc)
        {
            Music.Stop();
            Engine.Unlock();
            Engine.m_Loading = false;
            Engine.m_Ingame = true;
            Cursor.Hourglass = false;
            Gumps.Desktop.Children.Clear();
            int gameWidth = Engine.GameWidth;
            int gameHeight = Engine.GameHeight;
            int num3 = gameWidth / 0x30;
            int num4 = gameHeight / 0x30;
            int num5 = (num3 * 0x30) - 4;
            int num6 = (num4 * 0x30) - 4;
            int num7 = (gameWidth - num5) / 2;
            int num8 = (gameHeight - num6) / 2;
            for (int i = 0; i < num3; i++)
            {
                Gumps.Desktop.Children.Add(new GSpellPlaceholder(num7 + (i * 0x30), -54));
                Gumps.Desktop.Children.Add(new GSpellPlaceholder(num7 + (i * 0x30), (gameHeight + 6) + 4));
            }
            for (int j = 0; j < num4; j++)
            {
                Gumps.Desktop.Children.Add(new GSpellPlaceholder(-54, num8 + (j * 0x30)));
                Gumps.Desktop.Children.Add(new GSpellPlaceholder((gameWidth + 6) + 4, num8 + (j * 0x30)));
            }
            Gumps.Desktop.Children.Add(new GSpellPlaceholder(-54, -54));
            Gumps.Desktop.Children.Add(new GSpellPlaceholder((gameWidth + 6) + 4, -54));
            Gumps.Desktop.Children.Add(new GSpellPlaceholder(-54, (gameHeight + 6) + 4));
            Gumps.Desktop.Children.Add(new GSpellPlaceholder((gameWidth + 6) + 4, (gameHeight + 6) + 4));
            Gumps.Desktop.Children.Add(new GDesktopBorder());
            Gumps.Desktop.Children.Add(new GBandageTimer());
            Gumps.Desktop.Children.Add(new GMapTracker());
            Gumps.Desktop.Children.Add(new GQuestArrow());
            Gumps.Desktop.Children.Add(new GPingDisplay());
            Gumps.Desktop.Children.Add(new GParticleCounter());
            Gumps.Desktop.Children.Add(new GTransparencyGump());
            Reagent[] reagents = Spells.Reagents;
            int length = reagents.Length;
            if ((Engine.ServerFeatures != null) && !Engine.Features.AOS)
            {
                length = 8;
            }
            PotionType[] typeArray = new PotionType[] { PotionType.Yellow, PotionType.Orange, PotionType.Red, PotionType.Purple };
            ItemIDValidator[] list = new ItemIDValidator[(length + 1) + typeArray.Length];
            for (int k = 0; k < length; k++)
            {
                list[k] = new ItemIDValidator(new int[] { reagents[k].ItemID });
            }
            for (int m = 0; m < typeArray.Length; m++)
            {
                list[length + m] = new ItemIDValidator(new int[] { 0xf06 + (int)typeArray[m] });
            }
            list[length + typeArray.Length] = new ItemIDValidator(new int[] { 0xe21, 0xee9 });
            Gumps.Desktop.Children.Add(new GItemCounters(list));
            CharData charData = World.CharData;
            Mobile player = World.Player;
            if ((player != null) && (player.Name.Length > 0))
            {
                charData.Name = player.Name;
            }
            if ((Engine.m_ServerName != null) && (Engine.m_ServerName.Length > 0))
            {
                charData.Shard = Engine.m_ServerName;
            }
            foreach (GumpLayout layout in charData.Layout)
            {
                switch (layout.Type)
                {
                    case 0:
                        {
                            GSpellIcon toAdd = new GSpellIcon(layout.Extra - 1);
                            toAdd.X = layout.X;
                            toAdd.Y = layout.Y;
                            Gumps.Desktop.Children.Add(toAdd);
                            break;
                        }
                    case 1:
                        if (!Engine.m_SkillsOpen)
                        {
                            Network.Send(new PQuerySkills());
                            Engine.m_SkillsOpen = true;
                            Engine.m_SkillsGump = new GSkills();
                            Engine.m_SkillsGump.X = layout.X;
                            Engine.m_SkillsGump.Y = layout.Y;
                            Engine.m_SkillsGump.Width = layout.Width;
                            Engine.m_SkillsGump.Height = layout.Height;
                            Gumps.Desktop.Children.Add(Engine.m_SkillsGump);
                        }
                        break;

                    case 2:
                        {
                            Mobile mobile2 = World.FindMobile(layout.Extra);
                            if (mobile2 != null)
                            {
                                Network.Send(new POpenPaperdoll(mobile2.Serial));
                                mobile2.PaperdollX = layout.X;
                                mobile2.PaperdollY = layout.Y;
                            }
                            break;
                        }
                    case 3:
                        {
                            Skill skill = Engine.Skills[layout.Extra];
                            if (skill != null)
                            {
                                GSkillIcon icon2 = new GSkillIcon(skill);
                                icon2.X = layout.X;
                                icon2.Y = layout.Y;
                                Gumps.Desktop.Children.Add(icon2);
                            }
                            break;
                        }
                }
            }
            charData.Save();
            Engine.DrawNow();
            Engine.StartPings();
            Network.Send(new POpenPaperdoll());
            World.Player.QueryStats();
        }

        private static void LoginConfirm(PacketReader pvSrc)
        {
            World.Clear();
            Map.Invalidate();
            Mobile mobile = World.WantMobile(pvSrc.ReadInt32());
            World.Serial = mobile.Serial;
            Macros.Load();
            if (pvSrc.ReadInt32() != 0)
            {
                pvSrc.Trace();
            }
            mobile.Body = pvSrc.ReadInt16();
            short x = pvSrc.ReadInt16();
            short y = pvSrc.ReadInt16();
            short z = pvSrc.ReadInt16();
            World.SetLocation(x, y, z);
            mobile.SetLocation(x, y, z);
            mobile.UpdateReal();
            mobile.Direction = pvSrc.ReadByte();
            mobile.Visible = true;
            mobile.Update();
            Network.Send(new PQuerySkills());
            Network.Send(new PClientVersion("4.0.8b"));
            Network.Send(new PScreenSize());
            Network.Send(new PSetLanguage());
            Network.Send(new PUnknownLogin());
            if (NewConfig.SendUpdateRange)
            {
                Network.Send(new PInitialUpdateRange());
            }
            Party.State = PartyState.Alone;
        }

        private static void LoginReject(PacketReader pvSrc)
        {
            int index = pvSrc.ReadByte();
            if (index == 7)
            {
                Gumps.Desktop.Children.Add(new GIdleWarning());
            }
            else
            {
                string str = (index < m_LoginRejectReasons.Length) ? m_LoginRejectReasons[index] : "The client could not attach to the game server.  It must have been taken down, please wait a few minutes and try again.";
                xGumps.SetVariable("FailMessage", str);
                xGumps.Display("ConnectionFailed");
                Cursor.Hourglass = false;
            }
        }

        private static void MapCommand(PacketReader pvSrc)
        {
            int num = pvSrc.ReadInt32();
            int num2 = pvSrc.ReadByte();
            bool flag = pvSrc.ReadBoolean();
            int num3 = pvSrc.ReadInt16();
            int num4 = pvSrc.ReadInt16();
            switch (num2)
            {
                case 1:
                    GMapTracker.MapX = m_xMapLeft + ((int)((m_xMapRight - m_xMapLeft) * (((double)num3) / ((double)m_xMapWidth))));
                    GMapTracker.MapY = m_yMapTop + ((int)((m_yMapBottom - m_yMapTop) * (((double)num4) / ((double)m_yMapHeight))));
                    GRadar.AddTag(GMapTracker.MapX, GMapTracker.MapY, "Treasure Map");
                    Engine.AddTextMessage(string.Format("Map: ({0}, {1})", GMapTracker.MapX, GMapTracker.MapY));
                    break;
            }
        }

        private static void MapWindow(PacketReader pvSrc)
        {
            int num = pvSrc.ReadInt32();
            int num2 = pvSrc.ReadInt16();
            int num3 = pvSrc.ReadInt16();
            int num4 = pvSrc.ReadInt16();
            int num5 = pvSrc.ReadInt16();
            int num6 = pvSrc.ReadInt16();
            int num7 = pvSrc.ReadInt16();
            int num8 = pvSrc.ReadInt16();
            m_xMapLeft = num3;
            m_yMapTop = num4;
            m_xMapRight = num5;
            m_yMapBottom = num6;
            m_xMapWidth = num7;
            m_yMapHeight = num8;
        }

        private static void Message_ASCII(PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            int num2 = pvSrc.ReadInt16();
            int type = pvSrc.ReadByte();
            IHue hue = Hues.Load(pvSrc.ReadInt16());
            IFont font = Engine.GetFont(pvSrc.ReadInt16());
            string name = pvSrc.ReadString(30);
            string text = pvSrc.ReadString();
            AddMessage(serial, font, hue, type, name, text);
        }

        private static void Message_Localized(PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            int num2 = pvSrc.ReadInt16();
            byte type = pvSrc.ReadByte();
            IHue hue = Hues.Load(pvSrc.ReadInt16());
            IFont uniFont = Engine.GetUniFont(pvSrc.ReadInt16());
            int number = pvSrc.ReadInt32();
            string name = pvSrc.ReadString(30);
            string str2 = pvSrc.ReadUnicodeLEString();
            string input = Localization.GetString(number);
            switch (number)
            {
                case 0x7a1a6:
                    Engine.m_Meditating = false;
                    break;

                case 0x7a4b4:
                case 0x7a4e3:
                case 0x7a4e4:
                case 0x7a4e5:
                case 0x7a4e6:
                case 0x7a4e7:
                case 0x7a4e8:
                case 0x7a4e9:
                case 0x7add5:
                case 0x7add6:
                case 0x7adda:
                case 0x7addb:
                case 0xfe68a:
                case 0xfe68c:
                    GBandageTimer.Stop();
                    Engine.m_Healing = false;
                    break;

                case 0x7a4dc:
                case 0x7a4dd:
                case 0x7a4de:
                case 0x7a4df:
                case 0x7a4e0:
                    m_HealStart = DateTime.Now;
                    GBandageTimer.Start();
                    Engine.m_Healing = true;
                    break;

                case 0x7a856:
                case 0x7a85b:
                    Engine.m_Meditating = true;
                    break;

                case 0x7abc5:
                case 0x7abc6:
                case 0x7abc7:
                case 0x7abc8:
                case 0x7abc9:
                case 0x7abcb:
                    Engine.m_Stealth = false;
                    Engine.m_StealthSteps = 0;
                    break;

                case 0x7abca:
                    Engine.m_Stealth = true;
                    if (!Engine.Features.AOS)
                    {
                        Engine.m_StealthSteps = (int)(Engine.Skills[SkillName.Stealth].Value / 10f);
                        break;
                    }
                    Engine.m_StealthSteps = (int)(Engine.Skills[SkillName.Stealth].Value / 5f);
                    break;
            }
            if (str2.Length > 0)
            {
                string[] strArray = str2.Split(new char[] { '\t' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    if ((strArray[i].Length > 1) && strArray[i].StartsWith("#"))
                    {
                        try
                        {
                            strArray[i] = Localization.GetString(Convert.ToInt32(strArray[i].Substring(1)));
                        }
                        catch
                        {
                        }
                    }
                }
                m_Args = strArray;
                input = m_ArgReplace.Replace(input, new MatchEvaluator(PacketHandlers.ArgReplace_Eval));
            }
            AddMessage(serial, uniFont, hue, type, name, input, number);
        }

        private static void Message_Localized_Affix(PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            int num2 = pvSrc.ReadInt16();
            int type = pvSrc.ReadByte();
            IHue hue = Hues.Load(pvSrc.ReadInt16());
            IFont uniFont = Engine.GetUniFont(pvSrc.ReadInt16());
            int number = pvSrc.ReadInt32();
            int num5 = pvSrc.ReadByte();
            string name = pvSrc.ReadString(30);
            string input = Localization.GetString(number);
            string str3 = pvSrc.ReadString();
            string str4 = pvSrc.ReadUnicodeString();
            if (((num5 & -8) != 0) || (((num5 & 2) != 0) && (serial > 0)))
            {
                using (StreamWriter writer = new StreamWriter("Message Localized Affix.log", true))
                {
                    writer.WriteLine("Serial: 0x{0:X8}; Graphic: 0x{1:X4}; Type: {2}; Number: {3}; Flags: 0x{4:X2}; Name: '{5}'; Affix: '{6}'; Args: '{7}'; Text: '{8}';", new object[] { serial, num2, type, number, num5, name, str3, str4, input });
                }
            }
            if (str3.Length > 0)
            {
                switch ((num5 & 1))
                {
                    case 0:
                        input = input + str3;
                        break;

                    case 1:
                        input = str3 + input;
                        break;
                }
            }
            if (str4.Length > 0)
            {
                string[] strArray = str4.Split(new char[] { '\t' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    if ((strArray[i].Length > 1) && strArray[i].StartsWith("#"))
                    {
                        try
                        {
                            strArray[i] = Localization.GetString(Convert.ToInt32(strArray[i].Substring(1)));
                        }
                        catch
                        {
                        }
                    }
                }
                m_Args = strArray;
                input = m_ArgReplace.Replace(input, new MatchEvaluator(PacketHandlers.ArgReplace_Eval));
            }
            if ((num5 & -8) != 0)
            {
                pvSrc.Trace();
                input = string.Format("0x{0:X2}\n{1}", num5, input);
            }
            AddMessage(serial, uniFont, hue, type, name, input, number);
        }

        private static void Message_Unicode(PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            int num2 = pvSrc.ReadInt16();
            int type = pvSrc.ReadByte();
            IHue hue = Hues.Load(pvSrc.ReadInt16());
            IFont uniFont = Engine.GetUniFont(pvSrc.ReadInt16());
            string str = pvSrc.ReadString(4);
            string name = pvSrc.ReadString(30);
            string text = pvSrc.ReadUnicodeString();
            AddMessage(serial, uniFont, hue, type, name, text);
        }

        private static void Mobile_Animation(PacketReader pvSrc)
        {
            int num;
            int num3;
            bool flag;
            bool flag2;
            int num4;
            int num5;
            Mobile mobile = World.FindMobile(pvSrc.ReadInt32());
            if (mobile != null)
            {
                num = pvSrc.ReadInt16();
                int num2 = pvSrc.ReadInt16();
                num3 = pvSrc.ReadInt16();
                flag = !pvSrc.ReadBoolean();
                flag2 = pvSrc.ReadBoolean();
                num4 = pvSrc.ReadByte();
                switch (Engine.m_Animations.GetBodyType(mobile.Body))
                {
                    case BodyType.Monster:
                        num = num % 0x16;
                        goto Label_00D9;

                    case BodyType.Sea:
                    case BodyType.Animal:
                        {
                            object obj2 = Engine.m_Animations.ActionDef1[num];
                            if (obj2 != null)
                            {
                                num = (int)obj2;
                            }
                            num = num % 13;
                            goto Label_00D9;
                        }
                    case BodyType.Human:
                    case BodyType.Equipment:
                        {
                            object obj3 = Engine.m_Animations.ActionDef2[num];
                            if (obj3 != null)
                            {
                                num = (int)obj3;
                            }
                            num = num % 0x23;
                            goto Label_00D9;
                        }
                }
            }
            return;
        Label_00D9:
            num5 = Engine.GetAnimDirection(mobile.Direction) & 7;
            if (Engine.m_Animations.IsValid(mobile.Body, num, num5))
            {
                Animation animation = new Animation();
                animation.Action = num;
                animation.RepeatCount = num3;
                animation.Forward = flag;
                animation.Repeat = flag2;
                animation.Delay = num4;
                mobile.Animation = animation;
                animation.Run();
            }
        }

        private static void Mobile_Attributes(PacketReader pvSrc)
        {
            Mobile mobile = World.FindMobile(pvSrc.ReadInt32());
            if (mobile != null)
            {
                mobile.Refresh = true;
                mobile.HPMax = pvSrc.ReadUInt16();
                mobile.HPCur = pvSrc.ReadUInt16();
                mobile.ManaMax = pvSrc.ReadUInt16();
                mobile.ManaCur = pvSrc.ReadUInt16();
                mobile.StamMax = pvSrc.ReadUInt16();
                mobile.StamCur = pvSrc.ReadUInt16();
                mobile.Refresh = false;
            }
        }

        private static void Mobile_Attributes_HitPoints(PacketReader pvSrc)
        {
            Mobile mobile = World.FindMobile(pvSrc.ReadInt32());
            if (mobile != null)
            {
                mobile.Refresh = true;
                mobile.HPMax = pvSrc.ReadUInt16();
                mobile.HPCur = pvSrc.ReadUInt16();
                mobile.Refresh = false;
            }
        }

        private static void Mobile_Attributes_Mana(PacketReader pvSrc)
        {
            Mobile mobile = World.FindMobile(pvSrc.ReadInt32());
            if (mobile != null)
            {
                mobile.Refresh = true;
                mobile.ManaMax = pvSrc.ReadUInt16();
                mobile.ManaCur = pvSrc.ReadUInt16();
                mobile.Refresh = false;
            }
        }

        private static void Mobile_Attributes_Stamina(PacketReader pvSrc)
        {
            Mobile mobile = World.FindMobile(pvSrc.ReadInt32());
            if (mobile != null)
            {
                mobile.Refresh = true;
                mobile.StamMax = pvSrc.ReadUInt16();
                mobile.StamCur = pvSrc.ReadUInt16();
                mobile.Refresh = false;
            }
        }

        private static void Mobile_Damage(PacketReader pvSrc)
        {
            Mobile m = World.FindMobile(pvSrc.ReadInt32());
            if (m != null)
            {
                int damage = pvSrc.ReadUInt16();
                if (damage > 0)
                {
                    Gumps.Desktop.Children.Add(new GDamageLabel(damage, m));
                }
            }
        }

        private static void Mobile_Death(PacketReader pvSrc)
        {
            Mobile p = World.FindMobile(pvSrc.ReadInt32());
            Item item = World.WantItem(pvSrc.ReadInt32());
            item.Query();
            if (p != null)
            {
                while (p.Walking.Count > 0)
                {
                    WalkAnimation animation = (WalkAnimation)p.Walking.Dequeue();
                    p.SetLocation((short)animation.NewX, (short)animation.NewY, (short)animation.NewZ);
                    p.Direction = (byte)animation.NewDir;
                    animation.Dispose();
                }
                p.UpdateReal();
                p.IsMoving = false;
                item.Query();
                Mobile player = World.Player;
                if ((((Engine.TargetHandler == null) && (p.Notoriety != Notoriety.Innocent)) && ((player != null) && !player.Ghost)) && ((!player.Flags[MobileFlag.Hidden] && player.InSquareRange(p, 2)) && !Engine.m_Meditating))
                {
                    item.Use();
                }
                p.CorpseSerial = item.Serial;
                p.Visible = true;
                p.Update();
                item.Visible = false;
                item.SetLocation(p.X, p.Y, p.Z);
                item.Amount = p.Body;
                item.ID = 0x2006;
                item.CorpseSerial = p.Serial;
                item.Direction = p.Direction;
                item.Update();
                p.Animation = new Animation();
                p.Animation.Action = Engine.m_Animations.ConvertAction(p.Body, p.Serial, p.X, p.Y, p.Direction, GenericAction.Die, p);
                p.Animation.Delay = 0;
                p.Animation.Forward = true;
                p.Animation.Repeat = false;
                Animation animation1 = p.Animation;
                animation1.OnAnimationEnd = (OnAnimationEnd)Delegate.Combine(animation1.OnAnimationEnd, new OnAnimationEnd(Engine.DeathAnim_OnAnimationEnd));
                p.Animation.Run();
                if ((Engine.m_Screenshots && p.Visible) && (p.HumanOrGhost && World.InRange(p)))
                {
                    string name = p.Name;
                    if ((name != null) && ((name = name.Trim()).Length > 0))
                    {
                        int frames = Renderer.m_Frames;
                        object highlight = Engine.m_Highlight;
                        bool fade = GFader.Fade;
                        bool containerGrid = Engine.m_ContainerGrid;
                        GFader.Fade = false;
                        Engine.m_Highlight = p;
                        Renderer.m_Frames += 5;
                        try
                        {
                            Renderer.ScreenShot(name);
                        }
                        finally
                        {
                            Renderer.m_Frames = frames;
                            Engine.m_Highlight = highlight;
                            GFader.Fade = fade;
                        }
                        Engine.AddTextMessage("Screenshot taken.");
                    }
                }
            }
        }

        private static void Mobile_Incoming(PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            if (!NewConfig.IncomingFix || ((serial & 0x3fffffff) != World.Serial))
            {
                int num10;
                if ((serial & -1073741824) != 0)
                {
                    pvSrc.Trace();
                }
                short num2 = pvSrc.ReadInt16();
                if ((serial & 0x80000000L) != 0L)
                {
                    short num3 = pvSrc.ReadInt16();
                }
                short x = pvSrc.ReadInt16();
                short y = pvSrc.ReadInt16();
                sbyte z = pvSrc.ReadSByte();
                byte dir = pvSrc.ReadByte();
                short num8 = pvSrc.ReadInt16();
                byte num9 = pvSrc.ReadByte();
                Notoriety notoriety = (Notoriety)pvSrc.ReadByte();
                ArrayList list = new ArrayList();
                LayerComparer comparer = LayerComparer.FromDirection(dir);
                bool wasFound = false;
                Mobile mobile = World.WantMobile(serial, ref wasFound);
                while ((num10 = pvSrc.ReadInt32()) > 0)
                {
                    Item item = World.WantItem(num10);
                    item.Query();
                    int itemID = pvSrc.ReadInt16();
                    Layer layer = (Layer)pvSrc.ReadByte();
                    int hue = ((itemID & 0x8000) != 0) ? pvSrc.ReadUInt16() : 0;
                    if (comparer.IsValid(layer))
                    {
                        short animation;
                        item.Visible = true;
                        if (item.InWorld)
                        {
                            Map.RemoveItem(item);
                            item.InWorld = false;
                        }
                        else if (item.IsEquip)
                        {
                            item.RemoveEquip();
                        }
                        else if (item.Parent != null)
                        {
                            item.Parent.RemoveItem(item);
                        }
                        itemID &= 0x3fff;
                        Engine.ItemArt.Translate(ref itemID, ref hue);
                        item.ID = (short)itemID;
                        item.Hue = (ushort)hue;
                        if (layer == Layer.Mount)
                        {
                            animation = (short)Engine.m_Animations.ConvertMountItemToBody(itemID);
                        }
                        else
                        {
                            animation = Map.GetAnimation(itemID);
                        }
                        item.IsEquip = true;
                        item.EquipParent = mobile;
                        list.Add(new EquipEntry(item, animation, layer));
                    }
                }
                list.Sort(comparer);
                if (mobile.Player)
                {
                    dir = (byte)(dir & 7);
                    dir = (byte)(dir | ((byte)(mobile.Direction & 0x80)));
                }
                if ((!mobile.Visible && !mobile.Player) && World.CharData.IncomingNames)
                {
                    mobile.Look();
                }
                if ((!mobile.Visible && !mobile.Player) && ((num2 == 400) || (num2 == 0x191)))
                {
                    bool flag2;
                    if ((World.Player != null) && (World.Player.Notoriety == Notoriety.Murderer))
                    {
                        flag2 = ((((notoriety == Notoriety.Innocent) || (notoriety == Notoriety.Ally)) || ((notoriety == Notoriety.Attackable) || (notoriety == Notoriety.Enemy))) || (notoriety == Notoriety.Murderer)) || (notoriety == Notoriety.Criminal);
                    }
                    else
                    {
                        flag2 = (((notoriety == Notoriety.Ally) || (notoriety == Notoriety.Attackable)) || ((notoriety == Notoriety.Enemy) || (notoriety == Notoriety.Murderer))) || (notoriety == Notoriety.Criminal);
                    }
                    if (flag2)
                    {
                        mobile.QueryStats();
                    }
                }
                if (!mobile.Player)
                {
                    mobile.SetLocation(x, y, z);
                    mobile.Direction = dir;
                    mobile.Hue = num8;
                    mobile.Body = num2;
                    mobile.IsMoving = false;
                    mobile.MovedTiles = 0;
                    mobile.HorseFootsteps = 0;
                    mobile.Walking.Clear();
                    mobile.UpdateReal();
                }
                mobile.Equip = list;
                mobile.Flags.Value = num9;
                mobile.Visible = true;
                mobile.Notoriety = notoriety;
                mobile.EquipChanged();
                mobile.Update();
            }
        }

        private static void Mobile_Moving(PacketReader pvSrc)
        {
            Mobile m = World.FindMobile(pvSrc.ReadInt32());
            if (m != null)
            {
                bool flag = false;
                m.Body = pvSrc.ReadInt16();
                if (!m.Player)
                {
                    int x = pvSrc.ReadInt16();
                    int y = pvSrc.ReadInt16();
                    int z = pvSrc.ReadSByte();
                    int dir = pvSrc.ReadByte();
                    m.Walking.Enqueue(WalkAnimation.PoolInstance(m, x, y, z, dir));
                    if (m.Walking.Count > 4)
                    {
                        WalkAnimation animation = (WalkAnimation)m.Walking.Dequeue();
                        m.SetLocation((short)animation.NewX, (short)animation.NewY, (short)animation.NewZ);
                        animation.Dispose();
                        flag = true;
                    }
                    ((WalkAnimation)m.Walking.Peek()).Start();
                    m.SetReal(x, y, z);
                }
                else
                {
                    pvSrc.Seek(6, SeekOrigin.Current);
                }
                m.Hue = pvSrc.ReadInt16();
                m.Flags.Value = pvSrc.ReadByte();
                m.Notoriety = (Notoriety)pvSrc.ReadByte();
                m.IsMoving = !m.Player || Engine.amMoving;
                if (!m.Visible)
                {
                    m.Visible = true;
                    m.Update();
                }
                else if (flag)
                {
                    m.Update();
                }
                if (m.Paperdoll != null)
                {
                    Gumps.OpenPaperdoll(m, m.PaperdollName, m.PaperdollCanDrag);
                }
            }
        }

        private static void Mobile_Status(PacketReader pvSrc)
        {
            Mobile mobile = World.WantMobile(pvSrc.ReadInt32());
            if (mobile != null)
            {
                mobile.Refresh = true;
                mobile.Name = pvSrc.ReadString(30);
                mobile.HPCur = pvSrc.ReadUInt16();
                mobile.HPMax = pvSrc.ReadUInt16();
                mobile.IsPet = pvSrc.ReadBoolean();
                byte num = pvSrc.ReadByte();
                if (num >= 1)
                {
                    mobile.Gender = pvSrc.ReadByte();
                    mobile.Str = pvSrc.ReadUInt16();
                    mobile.Dex = pvSrc.ReadUInt16();
                    mobile.Int = pvSrc.ReadUInt16();
                    mobile.StamCur = pvSrc.ReadUInt16();
                    mobile.StamMax = pvSrc.ReadUInt16();
                    mobile.ManaCur = pvSrc.ReadUInt16();
                    mobile.ManaMax = pvSrc.ReadUInt16();
                    mobile.Gold = pvSrc.ReadInt32();
                    mobile.Armor = pvSrc.ReadUInt16();
                    mobile.Weight = pvSrc.ReadUInt16();
                    if (num >= 2)
                    {
                        mobile.StatCap = pvSrc.ReadUInt16();
                        if (num >= 3)
                        {
                            mobile.FollowersCur = pvSrc.ReadByte();
                            mobile.FollowersMax = pvSrc.ReadByte();
                            if (num >= 4)
                            {
                                mobile.FireResist = pvSrc.ReadInt16();
                                mobile.ColdResist = pvSrc.ReadInt16();
                                mobile.PoisonResist = pvSrc.ReadInt16();
                                mobile.EnergyResist = pvSrc.ReadInt16();
                                mobile.Luck = pvSrc.ReadUInt16();
                                mobile.DamageMin = pvSrc.ReadUInt16();
                                mobile.DamageMax = pvSrc.ReadUInt16();
                                mobile.TithingPoints = pvSrc.ReadInt32();
                                if (num > 4)
                                {
                                    pvSrc.Trace();
                                }
                            }
                            else
                            {
                                mobile.FireResist = 0;
                                mobile.ColdResist = 0;
                                mobile.PoisonResist = 0;
                                mobile.EnergyResist = 0;
                                mobile.Luck = 0;
                                mobile.DamageMin = 0;
                                mobile.DamageMax = 0;
                            }
                        }
                        else
                        {
                            mobile.FollowersCur = 0;
                            mobile.FollowersMax = 5;
                        }
                    }
                    else
                    {
                        mobile.StatCap = 0xe1;
                    }
                }
                mobile.Refresh = false;
            }
        }

        private static void Mobile_Update(PacketReader pvSrc)
        {
            Mobile m = World.WantMobile(pvSrc.ReadInt32());
            short num = pvSrc.ReadInt16();
            byte num2 = pvSrc.ReadByte();
            short num3 = pvSrc.ReadInt16();
            byte num4 = pvSrc.ReadByte();
            short x = pvSrc.ReadInt16();
            short y = pvSrc.ReadInt16();
            short num7 = pvSrc.ReadInt16();
            byte newDir = pvSrc.ReadByte();
            sbyte z = pvSrc.ReadSByte();
            if ((num2 != 0) || (num7 != 0))
            {
                pvSrc.Trace();
            }
            if (m.Player)
            {
                if (Engine.m_InResync)
                {
                    Engine.m_InResync = false;
                    Engine.AddTextMessage("Resynchronization complete.");
                }
                m_Sequences.Clear();
                Engine.m_Sequence = 0;
                Engine.m_WalkAck = 0;
                Engine.m_WalkReq = 0;
            }
            if (m.Player)
            {
                if (((num == 0x192) || (num == 0x193)) && ((m.Body != 0x192) && (m.Body != 0x193)))
                {
                    Network.Send(new PSetWarMode(false, 0x20, 0));
                    Engine.Effects.Add(new DeathEffect());
                }
                else if (((m.Body == 0x192) || (m.Body == 0x193)) && ((num != 0x192) && (num != 0x193)))
                {
                    Animation animation = m.Animation = new Animation();
                    animation.Action = 0x11;
                    animation.Delay = 0;
                    animation.Forward = true;
                    animation.Repeat = false;
                    animation.Run();
                    Engine.Effects.Add(new ResurrectEffect());
                }
            }
            if (m.Player)
            {
                World.SetLocation(x, y, z);
            }
            m.SetLocation(x, y, z);
            m.Body = num;
            m.Hue = num3;
            m.IsMoving = false;
            m.MovedTiles = 0;
            m.HorseFootsteps = 0;
            m.Walking.Clear();
            m.UpdateReal();
            Engine.EquipSort(m, newDir);
            m.Direction = newDir;
            m.Flags.Value = num4;
            if (m.Paperdoll != null)
            {
                Gumps.OpenPaperdoll(m, m.PaperdollName, m.PaperdollCanDrag);
            }
            m.Visible = true;
            m.Update();
        }

        private static void Movement_Accept(PacketReader pvSrc)
        {
            int num = pvSrc.ReadByte();
            Mobile player = World.Player;
            if (player != null)
            {
                player.Notoriety = (Notoriety)pvSrc.ReadByte();
            }
            if (m_Sequences.Count == 0)
            {
                Engine.Resync();
            }
            else
            {
                int[] numArray = (int[])m_Sequences.Dequeue();
                if (num != numArray[0])
                {
                    Engine.Resync();
                }
                else
                {
                    World.SetLocation(numArray[1], numArray[2], numArray[3]);
                }
            }
            Engine.m_WalkAck++;
        }

        private static void Movement_Reject(PacketReader pvSrc)
        {
            m_Sequences.Clear();
            Engine.m_Sequence = 0;
            Engine.m_WalkReq = 0;
            Engine.m_WalkAck = 0;
            Mobile player = World.Player;
            if (player != null)
            {
                pvSrc.ReadByte();
                short x = pvSrc.ReadInt16();
                short y = pvSrc.ReadInt16();
                byte dir = pvSrc.ReadByte();
                if (player.Direction != dir)
                {
                    player.Direction = dir;
                    player.Equip.Sort(LayerComparer.FromDirection(dir));
                }
                sbyte z = pvSrc.ReadSByte();
                World.SetLocation(x, y, z);
                player.SetLocation(x, y, z);
                player.MovedTiles = 0;
                player.HorseFootsteps = 0;
                player.IsMoving = false;
                player.Walking.Clear();
                player.UpdateReal();
                player.Update();
            }
        }

        private static void MultiTarget(PacketReader pvSrc)
        {
            pvSrc.ReadByte();
            Engine.m_MultiPreview = true;
            Engine.m_MultiSerial = pvSrc.ReadInt32();
            Engine.TargetHandler = new MultiTargetHandler(Engine.m_MultiSerial);
            pvSrc.Seek(12, SeekOrigin.Current);
            Engine.m_MultiID = pvSrc.ReadInt16();
            Engine.m_xMultiOffset = pvSrc.ReadInt16();
            Engine.m_yMultiOffset = pvSrc.ReadInt16();
            Engine.m_zMultiOffset = pvSrc.ReadInt16();
            ArrayList list = new ArrayList(Engine.Multis.Load(Engine.m_MultiID));
            int count = list.Count;
            int x = 0x3e8;
            int y = 0x3e8;
            int num4 = -1000;
            int num5 = -1000;
            for (int i = 0; i < count; i++)
            {
                MultiItem item = (MultiItem)list[i];
                if (item.X < x)
                {
                    x = item.X;
                }
                if (item.X > num4)
                {
                    num4 = item.X;
                }
                if (item.Y < y)
                {
                    y = item.Y;
                }
                if (item.Y > num5)
                {
                    num5 = item.Y;
                }
            }
            Engine.m_MultiMinX = x;
            Engine.m_MultiMinY = y;
            Engine.m_MultiMaxX = num4;
            Engine.m_MultiMaxY = num5;
            ArrayList list2 = new ArrayList(list.Count);
            for (int j = x; j <= num4; j++)
            {
                for (int k = y; k <= num5; k++)
                {
                    ArrayList list3 = new ArrayList(8);
                    count = list.Count;
                    int index = 0;
                    while (index < count)
                    {
                        MultiItem item2 = (MultiItem)list[index];
                        if ((item2.X == j) && (item2.Y == k))
                        {
                            list3.Add(StaticItem.Instantiate(item2.ItemID, (sbyte)item2.Z, item2.Flags));
                            list.RemoveAt(index);
                            count--;
                        }
                        else
                        {
                            index++;
                        }
                    }
                    list3.Sort(TileSorter.Comparer);
                    count = list3.Count;
                    for (index = 0; index < count; index++)
                    {
                        StaticItem item3 = (StaticItem)list3[index];
                        MultiItem item4 = new MultiItem();
                        item4.X = (short)j;
                        item4.Y = (short)k;
                        item4.Z = item3.Z;
                        item4.ItemID = (short)((item3.ID & 0x3fff) | 0x4000);
                        item4.Flags = item3.Serial;
                        list2.Add(item4);
                    }
                }
            }
            Engine.m_MultiList = list2;
        }

        private static void P_Unhandled(PacketReader pvSrc)
        {
            Network.Trace("Skipping packet 0x{2:X2} ('{0}') of length {1} ( 0x{1:X} )", pvSrc.Name, pvSrc.Length, pvSrc.Command);
        }

        private static void ParticleEffect(PacketReader pvSrc)
        {
            Effect(pvSrc, true, true);
        }

        private static void Pathfind(PacketReader pvSrc)
        {
            int num = pvSrc.ReadInt16();
            int num2 = pvSrc.ReadInt16();
            int num3 = pvSrc.ReadInt16();
            if (--m_PathfindIndex == 1)
            {
                m_PathfindIndex = 20;
                Engine.AddTextMessage(string.Format("Pathfind to ({0}, {1}, {2})", num, num2, num3), Engine.GetFont(3), Hues.Load(0x3b2));
            }
        }

        private static void Pause(PacketReader pvSrc)
        {
        }

        private static void PingReply(PacketReader pvSrc)
        {
            Engine.PingReply();
        }

        private static void PlayMusic(PacketReader pvSrc)
        {
            if (NewConfig.PlayMusic)
            {
                int midiID = pvSrc.ReadInt16();
                if (midiID < 0)
                {
                    Music.Stop();
                }
                else
                {
                    string fileName = Engine.MidiTable.Translate(midiID);
                    if (fileName != null)
                    {
                        Music.Play(fileName);
                    }
                    else
                    {
                        pvSrc.Trace();
                    }
                }
            }
        }

        private static void PlaySound(PacketReader pvSrc)
        {
            byte num = pvSrc.ReadByte();
            short soundID = pvSrc.ReadInt16();
            short num3 = pvSrc.ReadInt16();
            short x = pvSrc.ReadInt16();
            short y = pvSrc.ReadInt16();
            short z = pvSrc.ReadInt16();
            if (num > 1)
            {
                pvSrc.Trace();
            }
            if (soundID >= 0)
            {
                Engine.Sounds.PlaySound(soundID, x, y, z, 0.75f);
            }
        }

        private static void Prompt_ASCII(PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            int prompt = pvSrc.ReadInt32();
            int num3 = pvSrc.ReadInt32();
            string text = pvSrc.ReadString().Trim();
            Engine.Prompt = new ASCIIPrompt(serial, prompt, text);
        }

        private static void Prompt_Unicode(PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            int prompt = pvSrc.ReadInt32();
            int num3 = pvSrc.ReadInt32();
            pvSrc.Seek(4, SeekOrigin.Current);
            string text = "";
            if (pvSrc.ReadInt16() != 0)
            {
                pvSrc.Trace();
                pvSrc.Seek(-2, SeekOrigin.Current);
                text = pvSrc.ReadUnicodeLEString();
            }
            Engine.Prompt = new UnicodePrompt(serial, prompt, text);
        }

        private static void PropertyListContent(PacketReader pvSrc)
        {
            int num = pvSrc.ReadUInt16();
            int serial = pvSrc.ReadInt32();
            int num3 = pvSrc.ReadByte();
            int num4 = pvSrc.ReadByte();
            int number = pvSrc.ReadInt32();
            if (((num != 1) || (num3 != 0)) || (num4 != 0))
            {
                pvSrc.Trace();
            }
            ArrayList dataStore = Engine.GetDataStore();
            while (true)
            {
                int num6 = pvSrc.ReadInt32();
                if (num6 == 0)
                {
                    break;
                }
                int length = pvSrc.ReadUInt16();
                string arguments = Encoding.Unicode.GetString(pvSrc.ReadBytes(length));
                dataStore.Add(new ObjectProperty(num6, arguments));
            }
            ObjectPropertyList list2 = new ObjectPropertyList(serial, number, (ObjectProperty[])dataStore.ToArray(typeof(ObjectProperty)));
            Engine.ReleaseDataStore(dataStore);
            Item item = World.FindItem(serial);
            if (item != null)
            {
                item.PropertyID = number;
            }
            Mobile mobile = World.FindMobile(serial);
            if (mobile != null)
            {
                mobile.PropertyID = number;
            }
            if (item != null)
            {
                object parent = item;
                bool flag = false;
                while (parent != null)
                {
                    if (!(parent is Item))
                    {
                        break;
                    }
                    Item item2 = (Item)parent;
                    if (((item2.Container != null) && (item2.Container.Gump is GContainer)) && ((GContainer)item2.Container.Gump).m_TradeContainer)
                    {
                        flag = true;
                    }
                    if (flag)
                    {
                        break;
                    }
                    parent = item2.Parent;
                }
                if (flag && (parent is Item))
                {
                    Item item3 = (Item)parent;
                    if ((item3.Container != null) && (item3.Container.Gump.Tooltip is ItemTooltip))
                    {
                        GObjectProperties gump = ((ItemTooltip)item3.Container.Gump.Tooltip).Gump as GObjectProperties;
                        if (gump != null)
                        {
                            gump.SetList(0xf9060 + (item3.ID & 0x3fff), item3.PropertyList);
                        }
                    }
                }
            }
        }

        private static void PropertyListHash(PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            int num2 = pvSrc.ReadInt32();
            Item item = World.FindItem(serial);
            if (item != null)
            {
                item.PropertyID = num2;
                if (((item.Parent != null) && ((item.Parent.ID & 0x3fff) == 0x2006)) && (item.PropertyList == null))
                {
                    item.QueryProperties();
                }
            }
            Mobile mobile = World.FindMobile(serial);
            if (mobile != null)
            {
                mobile.PropertyID = num2;
            }
        }

        private static void QuestArrow(PacketReader pvSrc)
        {
            if (pvSrc.ReadBoolean())
            {
                GQuestArrow.Activate(pvSrc.ReadInt16(), pvSrc.ReadInt16());
            }
            else
            {
                GQuestArrow.Stop();
            }
        }

        private static void Register(int packetID, string name, int length, PacketCallback callback)
        {
            if (m_Handlers[packetID] != null)
            {
                Debug.Trace("Warning: Duplicate packet handlers registered. (packetID=0x{0:X2})", packetID);
            }
            m_Handlers[packetID] = new PacketHandler(packetID, name, length, callback);
        }

        private static void RequestResurrection(PacketReader pvSrc)
        {
        }

        private static void ReviseUpdateRange(PacketReader pvSrc)
        {
            World.Range = pvSrc.ReadByte();
        }

        private static void ScrollMessage(PacketReader pvSrc)
        {
            int num = pvSrc.ReadByte();
            int num2 = pvSrc.ReadInt32();
            string text = pvSrc.ReadString(pvSrc.ReadUInt16());
            if (text != "MISSING UPDATE")
            {
                Gumps.Desktop.Children.Add(new GUpdateScroll(text));
            }
        }

        private static void Season(PacketReader pvSrc)
        {
            int num = pvSrc.ReadByte();
            int num2 = pvSrc.ReadByte();
            if (num > 4)
            {
                pvSrc.Trace();
            }
            else if (num2 > 1)
            {
                pvSrc.Trace();
            }
        }

        private static void SecureTrade(PacketReader pvSrc)
        {
            byte num = pvSrc.ReadByte();
            int serial = pvSrc.ReadInt32();
            switch (num)
            {
                case 0:
                    pvSrc.ReturnName = "Initiate Secure Trade";
                    SecureTrade_Open(serial, pvSrc);
                    break;

                case 1:
                    pvSrc.ReturnName = "Close Secure Trade";
                    SecureTrade_Close(serial, pvSrc);
                    break;

                case 2:
                    pvSrc.ReturnName = "Update Secure Trade Status";
                    SecureTrade_Check(serial, pvSrc);
                    break;

                default:
                    pvSrc.Trace();
                    break;
            }
        }

        private static void SecureTrade_Check(int serial, PacketReader pvSrc)
        {
            bool flag = pvSrc.ReadInt32() != 0;
            bool flag2 = pvSrc.ReadInt32() != 0;
            Item item = World.FindItem(serial);
            if (item != null)
            {
                item.TradeCheck1 = flag;
                item.TradeCheck2 = flag2;
                if (item.Container != null)
                {
                    Gump gump = item.Container.Gump;
                    if (gump != null)
                    {
                        ((GSecureTradeCheck)gump.GetTag("Check1")).Checked = flag;
                        ((GSecureTradeCheck)gump.GetTag("Check2")).Checked = flag2;
                    }
                }
            }
        }

        private static void SecureTrade_Close(int serial, PacketReader pvSrc)
        {
            Item item = World.FindItem(serial);
            if (((item != null) && (item.Container != null)) && (item.Container.Gump != null))
            {
                Gump gump = item.Container.Gump;
                gump.RemoveTag("Dispose");
                Gumps.Destroy(gump.Parent);
            }
        }

        private static void SecureTrade_Open(int serial, PacketReader pvSrc)
        {
            string str;
            string str2;
            int num = pvSrc.ReadInt32();
            int num2 = pvSrc.ReadInt32();
            bool flag = pvSrc.ReadBoolean();
            Mobile player = World.Player;
            Mobile mobile2 = World.FindMobile(serial);
            if (((player == null) || ((str = player.Name) == null)) || ((str = str.Trim()).Length <= 0))
            {
                str = "Me";
            }
            if (flag)
            {
                str2 = pvSrc.ReadString();
            }
            else if (((mobile2 == null) || ((str2 = mobile2.Name) == null)) || ((str2 = str2.Trim()).Length <= 0))
            {
                str2 = "Them";
            }
            GSecureTrade toAdd = new GSecureTrade(num, null, str, str2);
            IFont uniFont = Engine.GetUniFont(1);
            IHue hue = Hues.Load(1);
            IHue hue2 = Hues.Load(0);
            Item item = World.WantItem(num);
            GSecureTradeCheck partner = new GSecureTradeCheck(250, 2, null, null);
            GSecureTradeCheck check2 = new GSecureTradeCheck(2, 2, item, partner);
            toAdd.Children.Add(check2);
            toAdd.Children.Add(partner);
            IContainer container = new GContainer(item, 0x52, hue2);
            toAdd.m_Container = container.Gump;
            container.Gump.X = 13;
            container.Gump.Y = 0x21;
            ((GContainer)container).m_TradeContainer = true;
            container.Gump.SetTag("Check1", check2);
            container.Gump.SetTag("Check2", partner);
            toAdd.Children.Add(container.Gump);
            item.Container = container;
            Item item2 = World.WantItem(num2);
            IContainer container2 = new GContainer(item2, 0x52, hue2);
            container2.Gump.X = 0x8e;
            container2.Gump.Y = 0x21;
            container2.Gump.SetTag("Check1", check2);
            container2.Gump.SetTag("Check2", partner);
            ((GContainer)container2).m_HitTest = false;
            ((GContainer)container2).m_TradeContainer = true;
            toAdd.Children.Add(container2.Gump);
            item2.Container = container2;
            if (Engine.Features.AOS)
            {
                toAdd.Tooltip = new ItemTooltip(item2);
            }
            Gumps.Desktop.Children.Add(toAdd);
        }

        private static void SelectHue(PacketReader pvSrc)
        {
            int num = pvSrc.ReadInt32();
            short num2 = pvSrc.ReadInt16();
            short itemID = pvSrc.ReadInt16();
            GAlphaBackground background = new GAlphaBackground(0, 0, 0xf4, 110);
            background.m_NonRestrictivePicking = true;
            background.Center();
            GItemArt toAdd = new GItemArt(0xb7, 3, itemID);
            toAdd.X += ((0x3a - (toAdd.Image.xMax - toAdd.Image.xMin)) / 2) - toAdd.Image.xMin;
            toAdd.Y += ((0x52 - (toAdd.Image.yMax - toAdd.Image.yMin)) / 2) - toAdd.Image.yMin;
            background.Children.Add(toAdd);
            GHuePicker picker = new GHuePicker(4, 4);
            picker.Brightness = 1;
            picker.SetTag("ItemID", (int)itemID);
            picker.SetTag("Item Art", toAdd);
            picker.SetTag("Dialog", background);
            picker.OnHueSelect = new OnHueSelect(Engine.HuePicker_OnHueSelect);
            background.Children.Add(picker);
            background.Children.Add(new GSingleBorder(3, 3, 0xa2, 0x52));
            background.Children.Add(new GSingleBorder(0xa4, 3, 0x11, 0x52));
            GBrightnessBar bar = new GBrightnessBar(0xa5, 4, 15, 80, picker);
            background.Children.Add(bar);
            bar.Refresh();
            GFlatButton button = new GFlatButton(0x7b, 0x57, 0x3a, 20, "Picker", new OnClick(Engine.HuePickerPicker_OnClick));
            GFlatButton okay = new GFlatButton(0xb7, 0x57, 0x3a, 20, "Okay", new OnClick(Engine.HuePickerOk_OnClick));
            okay.SetTag("Hue Picker", picker);
            okay.SetTag("Dialog", background);
            okay.SetTag("Serial", num);
            okay.SetTag("ItemID", itemID);
            okay.SetTag("Relay", num2);
            button.SetTag("Hue Picker", picker);
            button.SetTag("Brightness Bar", bar);
            background.Children.Add(new GQuickHues(picker, bar, okay));
            background.Children.Add(button);
            background.Children.Add(okay);
            Gumps.Desktop.Children.Add(background);
            Engine.HuePicker_OnHueSelect(picker.Hue, picker);
        }

        private static void SellContent(PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            int num2 = pvSrc.ReadInt16();
            SellInfo[] info = new SellInfo[num2];
            bool flag = false;
            for (int i = 0; i < num2; i++)
            {
                Item item = World.WantItem(pvSrc.ReadInt32());
                info[i] = new SellInfo(item, pvSrc.ReadInt16(), pvSrc.ReadUInt16(), pvSrc.ReadUInt16(), pvSrc.ReadUInt16(), pvSrc.ReadString(pvSrc.ReadUInt16()));
            }
            if (flag)
            {
                Engine.AddTextMessage("Selling items.");
                Network.Send(new PSellItems(serial, info));
            }
            else
            {
                Gumps.Desktop.Children.Add(new GSellGump(serial, info));
            }
        }

        private static void Sequence(PacketReader pvSrc)
        {
            byte num = pvSrc.ReadByte();
            if (num > 1)
            {
                pvSrc.Trace();
            }
            else
            {
                switch (num)
                {
                    case 0:
                        if (Engine.Effects.Locked)
                        {
                            pvSrc.Trace();
                        }
                        else
                        {
                            Engine.Effects.Lock();
                        }
                        break;

                    case 1:
                        if (!Engine.Effects.Locked)
                        {
                            pvSrc.Trace();
                        }
                        else
                        {
                            Engine.Effects.Unlock();
                        }
                        break;
                }
            }
        }

        private static void ServerChange(PacketReader pvSrc)
        {
            Engine.Multis.Clear();
            Mobile player = World.Player;
            if (player != null)
            {
                short x = pvSrc.ReadInt16();
                short y = pvSrc.ReadInt16();
                short z = pvSrc.ReadInt16();
                World.SetLocation(x, y, z);
                player.SetLocation(x, y, z);
                player.UpdateReal();
            }
            else
            {
                pvSrc.Seek(6, SeekOrigin.Current);
            }
        }

        private static void ServerList(PacketReader pvSrc)
        {
            pvSrc.ReadByte();
            int num = pvSrc.ReadInt16();
            if (num <= 0)
            {
                Gumps.Desktop.Children.Clear();
                xGumps.SetVariable("FailMessage", "The Ultima Online servers are currently down. Please try again in a few moments.");
                xGumps.Display("ConnectionFailed");
                Cursor.Hourglass = false;
            }
            else
            {
                Server[] array = new Server[num];
                Server server = null;
                for (int i = 0; i < num; i++)
                {
                    array[i] = new Server(pvSrc.ReadInt16(), pvSrc.ReadString(0x20), pvSrc.ReadByte(), pvSrc.ReadSByte(), new IPAddress((long)pvSrc.ReadUInt32()));
                    if (array[i].ServerID == NewConfig.LastServerID)
                    {
                        server = array[i];
                    }
                }
                Array.Sort(array);
                Engine.Servers = array;
                Engine.LastServer = server;
                if (Engine.m_QuickLogin)
                {
                    for (int j = 0; j < array.Length; j++)
                    {
                        if ((array[j].ServerID == Engine.m_QuickEntry.ServerID) && (array[j].Name == Engine.m_QuickEntry.ServerName))
                        {
                            array[j].Select();
                            Cursor.Hourglass = true;
                            Gumps.Desktop.Children.Clear();
                            xGumps.Display("Connecting");
                            Engine.DrawNow();
                            return;
                        }
                    }
                    Cursor.Hourglass = false;
                    Gumps.Desktop.Children.Clear();
                    xGumps.SetVariable("FailMessage", "That server was not found on the server list.");
                    xGumps.Display("ConnectionFailed");
                    Engine.DrawNow();
                }
                else
                {
                    Cursor.Hourglass = false;
                    Gumps.Desktop.Children.Clear();
                    xGumps.Display("ServerList");
                }
            }
        }

        private static void ServerRelay(PacketReader pvSrc)
        {
            int serverIP = ((pvSrc.ReadByte() | (pvSrc.ReadByte() << 8)) | (pvSrc.ReadByte() << 0x10)) | (pvSrc.ReadByte() << 0x18);
            int port = pvSrc.ReadInt16();
            int gameSeed = pvSrc.ReadInt32();
            Network.Disconnect();
            if (!Network.Connect(serverIP, port))
            {
                Gumps.Desktop.Children.Clear();
                xGumps.SetVariable("FailMessage", "Couldn't connect to the game server.  Either the server is down, or an invalid host / port was specified in the server ini.");
                xGumps.Display("ConnectionFailed");
                Cursor.Hourglass = false;
            }
            else
            {
                Network.EnableUnpacking();
                Network.Send(new PGameSeed(gameSeed));
                if (Engine.m_QuickLogin)
                {
                    Network.Send(new PGameLogin(gameSeed, Engine.m_QuickEntry.AccountName, Engine.m_QuickEntry.Password));
                }
                else
                {
                    Network.Send(new PGameLogin(gameSeed, NewConfig.Username, NewConfig.Password));
                }
                Network.CheckCache();
            }
        }

        private static void ShopContent(PacketReader pvSrc)
        {
            int num = pvSrc.ReadInt32();
            int num2 = pvSrc.ReadByte();
            if (num2 > 0)
            {
                m_BuyMenuSerial = num;
                m_BuyMenuNames = new string[num2];
                m_BuyMenuPrices = new int[num2];
                for (int i = 0; i < num2; i++)
                {
                    m_BuyMenuPrices[i] = pvSrc.ReadInt32();
                    string str = pvSrc.ReadString(pvSrc.ReadByte());
                    try
                    {
                        str = Localization.GetString(Convert.ToInt32(str));
                    }
                    catch
                    {
                    }
                    m_BuyMenuNames[i] = str;
                }
            }
        }

        private static void Skills(PacketReader pvSrc)
        {
            switch (pvSrc.ReadByte())
            {
                case 0:
                    pvSrc.ReturnName = "Skills (Absolute)";
                    Skills_Absolute(pvSrc, false);
                    return;

                case 2:
                    pvSrc.ReturnName = "Skills (Absolute, Capped)";
                    Skills_Absolute(pvSrc, true);
                    return;

                case 0xdf:
                    pvSrc.ReturnName = "Skills (Delta, Capped)";
                    Skills_Delta(pvSrc, true);
                    return;

                case 0xff:
                    pvSrc.ReturnName = "Skills (Delta)";
                    Skills_Delta(pvSrc, false);
                    return;
            }
            pvSrc.Trace();
        }

        private static void Skills_Absolute(PacketReader pvSrc, bool hasCapData)
        {
            int num;
            Client.Skills skills = Engine.Skills;
            while ((num = pvSrc.ReadInt16()) > 0)
            {
                Skill skill = skills[num - 1];
                if (skill != null)
                {
                    skill.Value = ((float)pvSrc.ReadInt16()) / 10f;
                    skill.Real = ((float)pvSrc.ReadInt16()) / 10f;
                    skill.Lock = (SkillLock)pvSrc.ReadByte();
                    if (hasCapData)
                    {
                        pvSrc.Seek(2, SeekOrigin.Current);
                    }
                    if (Engine.m_SkillsOpen && (Engine.m_SkillsGump != null))
                    {
                        Engine.m_SkillsGump.OnSkillChange(skill);
                    }
                }
            }
        }

        private static void Skills_Delta(PacketReader pvSrc, bool hasCapData)
        {
            Skill skill = Engine.Skills[pvSrc.ReadInt16()];
            if (skill != null)
            {
                float num = ((float)pvSrc.ReadInt16()) / 10f;
                if (skill.Value != num)
                {
                    float num2 = num - skill.Value;
                    int num3 = Math.Sign(num2);
                    Engine.AddTextMessage(string.Format("Your skill in {0} has {1} by {2:F1}. Is it now {3:F1}.", new object[] { skill.Name, (num2 > 0f) ? "increased" : "decreased", Math.Abs(num2), num }), Engine.GetFont(3), Hues.Load(0x59));
                    skill.Value = num;
                }
                skill.Real = ((float)pvSrc.ReadInt16()) / 10f;
                skill.Lock = (SkillLock)pvSrc.ReadByte();
                if (hasCapData)
                {
                    pvSrc.Seek(2, SeekOrigin.Current);
                }
                if (Engine.m_SkillsOpen && (Engine.m_SkillsGump != null))
                {
                    Engine.m_SkillsGump.OnSkillChange(skill);
                }
            }
        }

        private static void StandardEffect(PacketReader pvSrc)
        {
            Effect(pvSrc, false, false);
        }

        private static void StringQuery(PacketReader pvSrc)
        {
            int num = pvSrc.ReadInt32();
            short num2 = pvSrc.ReadInt16();
            int fixedLength = pvSrc.ReadInt16();
            string text = pvSrc.ReadString(fixedLength);
            bool flag = pvSrc.ReadBoolean();
            byte num4 = pvSrc.ReadByte();
            int num5 = pvSrc.ReadInt32();
            int num6 = pvSrc.ReadInt16();
            string str2 = pvSrc.ReadString(num6);
            GDragable dragable = new GDragable(0x474, 0, 0);
            dragable.CanClose = false;
            dragable.Modal = true;
            dragable.X = (Engine.ScreenWidth - dragable.Width) / 2;
            dragable.Y = (Engine.ScreenHeight - dragable.Height) / 2;
            GButton toAdd = new GButton(0x47b, 0x47d, 0x47c, 0x75, 190, new OnClick(Engine.StringQueryOkay_OnClick));
            GButton button2 = new GButton(0x478, 0x47a, 0x479, 0xcc, 190, flag ? new OnClick(Engine.StringQueryCancel_OnClick) : null);
            if (!flag)
            {
                button2.Enabled = false;
            }
            GImage image = new GImage(0x477, 60, 0x91);
            GWrappedLabel label = new GWrappedLabel(text, Engine.GetFont(2), Hues.Load(0x455), 60, 0x30, 0x110);
            GWrappedLabel label2 = new GWrappedLabel(str2, Engine.GetFont(2), Hues.Load(0x455), 60, 0x30, 0x110);
            label2.Y = image.Y - label2.Height;
            GTextBox box = new GTextBox(0, false, 0x44, 140, image.Width - 8, image.Height, "", Engine.GetFont(1), Hues.Load(0x455), Hues.Load(0x455), Hues.Load(0x455));
            box.Focus();
            if (num4 == 1)
            {
                box.MaxChars = num5;
            }
            toAdd.SetTag("Dialog", dragable);
            toAdd.SetTag("Serial", num);
            toAdd.SetTag("Type", num2);
            toAdd.SetTag("Text", box);
            button2.SetTag("Dialog", dragable);
            button2.SetTag("Serial", num);
            button2.SetTag("Type", num2);
            dragable.Children.Add(label);
            dragable.Children.Add(label2);
            dragable.Children.Add(image);
            dragable.Children.Add(box);
            dragable.Children.Add(button2);
            dragable.Children.Add(toAdd);
            dragable.m_CanDrag = true;
            Gumps.Desktop.Children.Add(dragable);
        }

        private static void Target(PacketReader pvSrc)
        {
            byte num = pvSrc.ReadByte();
            int targetID = pvSrc.ReadInt32();
            byte num3 = pvSrc.ReadByte();
            int num4 = pvSrc.ReadInt32();
            short num5 = pvSrc.ReadInt16();
            short num6 = pvSrc.ReadInt16();
            byte num7 = pvSrc.ReadByte();
            sbyte num8 = pvSrc.ReadSByte();
            short num9 = pvSrc.ReadInt16();
            if (m_CancelTarget)
            {
                m_CancelTarget = false;
            }
            else if (num3 == 3)
            {
                if (Engine.TargetHandler is ServerTargetHandler)
                {
                    Engine.TargetHandler.OnCancel(TargetCancelType.UserCancel);
                    Engine.TargetHandler = null;
                }
            }
            else
            {
                ServerTargetHandler handler;
                m_TimeLastCast = DateTime.Now;
                Engine.m_LastTargetID = targetID;
                if ((num > 1) || (((num3 != 1) && (num3 != 2)) && (num3 != 0)))
                {
                    pvSrc.Trace();
                }
                Engine.TargetHandler = handler = new ServerTargetHandler(targetID, num != 1, (ServerTargetFlags)num3);
                TargetActions.Identify();
                if (handler.Action != TargetAction.Unknown)
                {
                    for (int i = 0; i < Engine.m_AutoTarget.Count; i++)
                    {
                        AutoTargetSession session = (AutoTargetSession)Engine.m_AutoTarget[i];
                        if (session.m_Action == handler.Action)
                        {
                            session.m_Timer.Delete();
                            Engine.m_AutoTarget.RemoveAt(i);
                            Engine.Target(session.m_Entity);
                            break;
                        }
                    }
                }
            }
        }

        private static void Trace(PacketReader pvSrc)
        {
            pvSrc.Trace();
        }

        private static void Unk32(PacketReader pvSrc)
        {
            Engine.AddTextMessage(pvSrc.ReadBoolean().ToString());
        }

        internal static void Update_OnTick(Client.Timer t)
        {
            ShardProfile tag = (ShardProfile)t.GetTag("shard");
            Profiles.Save();
            GMenuItem item = tag.Menu;
            if (item == null)
            {
                Engine.UpdateSmartLoginMenu();
            }
            else
            {
                for (int i = 0; i < tag.Characters.Length; i++)
                {
                    GMenuItem item2 = tag.Characters[i].Menu;
                    if (item2 == null)
                    {
                        item.Add(tag.Characters[i].Menu = item2 = new GPlayCharacterMenu(tag.Characters[i]));
                    }
                    else
                    {
                        item2.Text = tag.Characters[i].Name;
                    }
                }
                Gump[] gumpArray = item.Children.ToArray();
                for (int j = 0; j < gumpArray.Length; j++)
                {
                    GPlayCharacterMenu child = gumpArray[j] as GPlayCharacterMenu;
                    if ((child != null) && !tag.Contains(child.Character))
                    {
                        item.Remove(child);
                    }
                }
            }
        }

        private static void VersionRequest_Assist(PacketReader pvSrc)
        {
            pvSrc.Trace();
            Engine.AddTextMessage("Server is requesting the assist version.", Engine.GetFont(3), Hues.Load(0x22));
            Network.Send(new PAssistVersion(pvSrc.ReadInt32(), "4.0.8b"));
        }

        private static void VersionRequest_Client(PacketReader pvSrc)
        {
            Engine.AddTextMessage("Server is requesting the client version.", Engine.GetFont(3), Hues.Load(0x22));
            Network.Send(new PClientVersion("4.0.8b"));
        }

        private static void WarmodeStatus(PacketReader pvSrc)
        {
            bool flag = pvSrc.ReadBoolean();
            if (pvSrc.ReadByte() == 0)
            {
                byte num = pvSrc.ReadByte();
                if (((num != 0x20) && (num != 50)) && (num != 0))
                {
                    pvSrc.Trace();
                }
                else if (pvSrc.ReadByte() != 0)
                {
                    pvSrc.Trace();
                }
            }
            Mobile player = World.Player;
            if (player != null)
            {
                player.Flags[MobileFlag.Warmode] = flag;
                if (!flag)
                {
                    Engine.m_Highlight = null;
                }
                if (player.Paperdoll != null)
                {
                    Gumps.OpenPaperdoll(player, player.PaperdollName, player.PaperdollCanDrag);
                }
            }
            Gumps.Invalidate();
            Engine.Redraw();
        }

        private static void Weather(PacketReader pvSrc)
        {
            int num = pvSrc.ReadByte();
            int num2 = pvSrc.ReadByte();
            int num3 = pvSrc.ReadSByte();
        }

        private static void WorldItem(PacketReader pvSrc)
        {
            int serial = pvSrc.ReadInt32();
            int itemID = pvSrc.ReadUInt16();
            int num3 = ((serial & 0x80000000L) != 0L) ? pvSrc.ReadInt16() : 0;
            if ((itemID & 0x8000) != 0)
            {
                itemID &= 0x7fff;
                itemID += pvSrc.ReadSByte();
            }
            int num4 = pvSrc.ReadInt16();
            int num5 = pvSrc.ReadInt16();
            bool flag2 = (num4 & 0x8000) != 0;
            bool flag3 = (num5 & 0x8000) != 0;
            bool flag4 = (num5 & 0x4000) != 0;
            int num6 = flag2 ? pvSrc.ReadByte() : 0;
            int num7 = pvSrc.ReadSByte();
            int hue = flag3 ? pvSrc.ReadUInt16() : 0;
            int num9 = flag4 ? pvSrc.ReadByte() : 0;
            serial &= 0x7fffffff;
            num4 &= 0x7fff;
            num5 &= 0x3fff;
            bool wasFound = false;
            Item i = World.WantItem(serial, ref wasFound);
            bool flag6 = i.IsMulti;
            i.IsMulti = itemID >= 0x4000;
            int id = 0;
            if (i.IsMulti)
            {
                id = itemID & 0x3fff;
                Engine.Multis.Register(i, id);
                itemID = 1;
            }
            else if (flag6)
            {
                Engine.Multis.Unregister(i);
            }
            Engine.ItemArt.Translate(ref itemID, ref hue);
            itemID &= 0x3fff;
            i.SetLocation((short)num4, (short)num5, (short)num7);
            i.ID = (short)itemID;
            i.Amount = (short)num3;
            i.Direction = (byte)num6;
            i.Hue = (ushort)hue;
            i.Flags.Value = num9;
            if ((!i.Visible && (i.IsCorpse || i.IsBones)) && World.CharData.IncomingNames)
            {
                i.Look();
            }
            i.Visible = true;
            i.InWorld = true;
            if (i.IsEquip)
            {
                i.RemoveEquip();
            }
            if (i.Parent != null)
            {
                i.Parent.RemoveItem(i);
            }
            if ((itemID == 0x2006) && (i.CorpseSerial != 0))
            {
                Mobile mobile = World.FindMobile(i.CorpseSerial);
                if (mobile != null)
                {
                    i.Direction = mobile.Direction;
                    i.Visible = false;
                }
            }
            i.Update();
            if (i.IsMulti && Engine.GMPrivs)
            {
                Engine.AddTextMessage(string.Format("House serial: 0x{0:X}", serial));
            }
        }

        public static string[] IPFReason
        {
            get
            {
                return m_IPFReason;
            }
        }

        private static PacketCallback Unhandled
        {
            get
            {
                return new PacketCallback(PacketHandlers.P_Unhandled);
            }
        }

        private enum Condition
        {
            Ageless,
            LikeNew,
            Slightly,
            Somewhat,
            Fairly,
            Greatly,
            IDOC
        }

        private enum EffectLayer
        {
            CenterFeet = 7,
            Head = 0,
            LeftFoot = 4,
            LeftHand = 2,
            RightFoot = 5,
            RightHand = 1,
            Waist = 3
        }

        private enum EffectType
        {
            Moving,
            Lightning,
            Location,
            Fixed
        }
    }
}