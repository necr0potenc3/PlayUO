namespace Client
{
    using Client.Prompts;
    using Client.Targeting;
    using Microsoft.DirectX.Direct3D;
    using Microsoft.Win32;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows.Forms;

    public class Engine
    {
        public static bool amMoving;
        public const float C528Scale = 8.225806f;
        public const float C825Scale = 0.1215686f;
        public const string Category = "UONET19";
        public const string CommandPrefix = ". ";
        public static bool exiting;
        public const int FALSE = 0;
        public const double FlipIt = 3.9269908169872414;
        public static int GameHeight = 480;
        public static int GameWidth = 640;
        public static int GameX = 20;
        public static int GameY = 20;
        public const double HalfPI = 1.5707963267948966;
        private static TimeDelay m_AllNames;
        private static bool m_AlternateFont;
        private static IItemValidator m_Ammo;
        public static Animations m_Animations;
        public static ArrayList m_AutoTarget = new ArrayList();
        public static int m_AutoUseIndex;
        public static Mobile m_BuyHorse;
        public static bool m_CanLevitate;
        private static int m_CharacterCount;
        private static string[] m_CharacterNames;
        private static EventArgs m_ClickArgs;
        private static object[] m_ClickList;
        private static object m_ClickSender;
        private static Timer m_ClickTimer;
        private static Client.ContainerBoundsTable m_ContainerBoundsTable;
        public static bool m_ContainerGrid = true;
        public static int m_ContextQueue;
        private static ConvertStruct[] m_ConvertStructs = new ConvertStruct[] { 
            new ConvertStruct('!', 0xff01, 1), new ConvertStruct('#', 0xff03, 1), new ConvertStruct('$', 0xff04, 1), new ConvertStruct('%', 0xff05, 1), new ConvertStruct('&', 0xff06, 1), new ConvertStruct('(', 0xff08, 1), new ConvertStruct(')', 0xff09, 1), new ConvertStruct('*', 0xff0a, 1), new ConvertStruct('+', 0xff0b, 1), new ConvertStruct(',', 0xff0c, 1), new ConvertStruct('-', 0xff0d, 1), new ConvertStruct('.', 0xff0e, 1), new ConvertStruct('/', 0xff0f, 1), new ConvertStruct('0', 0xff10, 10), new ConvertStruct(':', 0xff1a, 1), new ConvertStruct(';', 0xff1b, 1), 
            new ConvertStruct('<', 0xff1c, 1), new ConvertStruct('=', 0xff1d, 1), new ConvertStruct('>', 0xff1e, 1), new ConvertStruct('?', 0xff1f, 1), new ConvertStruct('@', 0xff20, 1), new ConvertStruct('A', 0xff21, 0x1a), new ConvertStruct('[', 0xff3b, 1), new ConvertStruct('\\', 0xff3c, 1), new ConvertStruct(']', 0xff3d, 1), new ConvertStruct('^', 0xff3e, 1), new ConvertStruct('_', 0xff3f, 1), new ConvertStruct('`', 0xff40, 1), new ConvertStruct('a', 0xff41, 0x1a), new ConvertStruct('{', 0xff5b, 1), new ConvertStruct('|', 0xff5c, 1), new ConvertStruct('}', 0xff5d, 1), 
            new ConvertStruct('~', 0xff5e, 1)
         };
        private static char[] m_ConvertTable;
        public static Mobile m_CriminalAttack;
        private static Queue m_DataStores = new Queue();
        private static IFont m_DefaultFont;
        private static IHue m_DefaultHue;
        public static Device m_Device;
        public static Display m_Display;
        public static int m_dMouse;
        private static ArrayList m_Doors;
        internal static double m_dTicks;
        private static bool m_DyeWindowOpen;
        public static Texture[] m_Edge;
        private static Client.Effects m_Effects;
        private static Regex m_Encoder = new Regex("&#(?<1>[0-9a-fA-F]+);", RegexOptions.None);
        public static bool m_EventOk;
        private static Client.Features m_Features;
        public static Client.FileManager m_FileManager;
        private static bool m_FirstAcctLogin = true;
        private static Font[] m_Font;
        public static Texture m_FormX;
        public static Texture m_Friend;
        public static bool m_Fullscreen = true;
        public static int m_GameHour;
        public static int m_GameMinute;
        public static int m_GameSecond;
        public static bool m_GMPrivs;
        public static Gumps m_Gumps;
        public static Texture m_Halo;
        public static bool m_Healing;
        public static object m_Highlight;
        public static bool m_Ingame;
        public static string m_IniPath;
        public static bool m_InResync;
        private static Client.ItemArt m_ItemArt;
        private static float m_ItemDuration = 5f;
        private static IItemValidator[] m_IVArray;
        public static ArrayList m_Journal;
        private static byte[] m_JournalBuffer;
        public static GJournal m_JournalGump;
        public static bool m_JournalOpen;
        private static int m_KeepAliveBlockIndex;
        private static MapBlock[] m_KeepAliveBlocks = new MapBlock[0x80];
        public static bool m_land = true;
        private static Client.LandArt m_LandArt;
        public static DateTime m_LastAction;
        public static Mobile m_LastAttacker;
        public static object m_LastBenTarget;
        public static string m_LastCommand;
        private static int m_LastDown = -1;
        private static Point m_LastDownPoint;
        public static object m_LastHarmTarget;
        private static DateTime m_LastLeapfrogPickup;
        public static MouseEventArgs m_LastMouseArgs;
        private static TimeDelay m_LastOverCheck;
        private static Server m_LastServer;
        private static DateTime m_LastStealthUse;
        public static object m_LastTarget;
        public static int m_LastTargetID;
        public static Item m_LeapFrog;
        public static bool m_Loading;
        private static Queue m_LoadQueue;
        public static bool m_Locked = true;
        private static Queue m_MapLoadQueue;
        public static bool m_Meditating;
        private static Client.MidiTable m_MidiTable;
        private static float m_MobileDuration = 7.5f;
        private static bool m_MouseMoved;
        public static TimeDelay m_MoveDelay;
        public static int m_MultiID;
        public static ArrayList m_MultiList;
        public static int m_MultiMaxX;
        public static int m_MultiMaxY;
        public static int m_MultiMinX;
        public static int m_MultiMinY;
        public static bool m_MultiPreview;
        private static Client.Multis m_Multis;
        public static int m_MultiSerial;
        private static TimeDelay m_NewFrame;
        public static int m_OkSequence;
        public static int m_OpenedGumps;
        private static bool m_OptionsOpen;
        public static string m_OverrideDataPath;
        public static string m_OverrideServHost;
        public static int m_OverrideServPort = -1;
        private static int m_Ping;
        private static int m_PingID;
        private static Queue m_Pings;
        private static Timer m_PingTimer;
        public static ArrayList m_Plugins;
        private static Timer m_PopupDelay;
        public static PresentParameters m_PresentParams;
        private static IPrompt m_Prompt;
        public static bool m_PumpFPS;
        private static Queue m_QamList = new Queue();
        private static Timer m_QamTimer;
        internal static long m_QPC;
        internal static double m_QPF;
        public static bool m_Quake;
        public static Entry m_QuickEntry;
        public static bool m_QuickLogin;
        public static bool m_radar;
        public static Texture m_Rain;
        private static System.Random m_Random;
        public static bool m_Redraw;
        public static bool m_regMap = true;
        private static IItemValidator m_Regs;
        public static Rectangle m_rRender;
        public static float m_RunSpeed = 0.2f;
        public static bool m_SayMacro;
        public static bool m_Screenshots = true;
        public static int m_Sequence;
        private static Client.ServerFeatures m_ServerFeatures;
        public static string m_ServerName;
        private static Server[] m_Servers;
        internal static bool m_SetTicks;
        public static Texture m_SkillDown;
        public static Texture m_SkillLocked;
        private static Client.Skills m_Skills;
        public static GSkills m_SkillsGump;
        public static bool m_SkillsOpen;
        public static Texture m_SkillUp;
        private static TimeDelay m_SleepMode;
        public static Texture m_Slider;
        public static Texture[] m_Snow;
        private static Client.Sounds m_Sounds;
        public static bool m_statics = true;
        public static bool m_Stealth;
        public static int m_StealthSteps;
        public static bool m_StopQueue;
        private static float m_SystemDuration = 7.5f;
        public static Texture m_TargetCursorImage;
        private static ITargetHandler m_TargetHandler;
        public static Texture m_TargetImage;
        public static object m_TargetQueue;
        private static bool m_TargetRecurse;
        public static object m_TargetSmartObj = new object();
        public static string m_Text = "";
        private static Client.TextureArt m_TextureArt;
        internal static int m_Ticks;
        public static ArrayList m_Timers;
        private static UnicodeFont[] m_UniFont;
        public static VertexBuffer m_VertexBuffer;
        public static int m_WalkAck;
        public static int m_WalkAckSync = 4;
        public static int m_WalkReq;
        public static float m_WalkSpeed = 0.4f;
        public static Stack m_WalkStack = new Stack();
        public static ArrayList m_WalkTimeout;
        public static bool m_Weather = true;
        public static Texture[] m_WinScrolls;
        public static int m_World;
        public static int m_xClick;
        public static int m_xMouse;
        public static int m_xMultiOffset;
        public static int m_yClick;
        public static int m_yMouse;
        public static int m_yMultiOffset;
        public static int m_zMultiOffset;
        public static Direction movingDir;
        public static Direction pointingDir;
        private const int QS_ANYTHING = 0xff;
        private const int QS_HOTKEY = 0x80;
        private const int QS_KEY = 1;
        private const int QS_MOUSEBUTTON = 4;
        private const int QS_MOUSEMOVE = 2;
        private const int QS_PAINT = 0x20;
        private const int QS_POSTMESSAGE = 8;
        private const int QS_SENDMESSAGE = 0x40;
        private const int QS_TIMER = 0x10;
        public static int ScreenHeight;
        public static int ScreenWidth;
        public const int TRUE = 1;
        public const string Version = "4.0.8b";

        public static void AccountArrow_OnClick(Gump Sender)
        {
            Gump gump = Gumps.FindGumpByGUID("Username");
            Gump gump2 = Gumps.FindGumpByGUID("Password");
            if ((((gump != null) && (gump2 != null)) && (gump.GetType() == typeof(GTextBox))) && (gump2.GetType() == typeof(GTextBox)))
            {
                string un = ((GTextBox) gump).String;
                string pw = ((GTextBox) gump2).String;
                Cursor.Hourglass = true;
                Gumps.Desktop.Children.Clear();
                xGumps.Display("Connecting");
                DrawNow();
                if (Network.Connect())
                {
                    Gumps.Desktop.Children.Clear();
                    xGumps.Display("AccountVerify");
                    NewConfig.Username = un;
                    NewConfig.Password = pw;
                    NewConfig.Save();
                    Network.Send(new PLoginSeed());
                    Network.Send(new PAccount(un, pw));
                }
                else
                {
                    Gumps.Desktop.Children.Clear();
                    xGumps.SetVariable("FailMessage", "Couldn't connect to the login server.  Either the server is down, or you've entered an invalid host / port.  Check Client.cfg.");
                    xGumps.Display("ConnectionFailed");
                    Cursor.Hourglass = false;
                }
            }
        }

        public static void AddTarget(Item what, TargetAction action)
        {
            m_AutoTarget.Add(new AutoTargetSession(what, action));
        }

        public static void AddTarget(Mobile who, TargetAction action)
        {
            m_AutoTarget.Add(new AutoTargetSession(who, action));
        }

        public static void AddTargetSelf(TargetAction action)
        {
            AddTarget(World.Player, action);
        }

        public static void AddTextMessage(string Message)
        {
            AddTextMessage(Message, m_SystemDuration, m_DefaultFont, m_DefaultHue);
        }

        public static void AddTextMessage(string Message, IFont Font)
        {
            AddTextMessage(Message, m_SystemDuration, Font, m_DefaultHue);
        }

        public static void AddTextMessage(string Message, float Delay)
        {
            AddTextMessage(Message, Delay, m_DefaultFont, m_DefaultHue);
        }

        public static void AddTextMessage(string Message, IFont Font, IHue Hue)
        {
            AddTextMessage(Message, m_SystemDuration, Font, Hue);
        }

        public static void AddTextMessage(string Message, float Delay, IFont Font)
        {
            AddTextMessage(Message, Delay, Font, m_DefaultHue);
        }

        public static void AddTextMessage(string Message, float Delay, IFont Font, IHue Hue)
        {
            if (m_Ingame)
            {
                Message = Message.TrimEnd(new char[0]);
                if (Message.Length > 0)
                {
                    AddToJournal(new JournalEntry(Message, Hue, -1));
                    Message = WrapText(Message, GameWidth / 2, Font).TrimEnd(new char[0]);
                    if (Message.Length > 0)
                    {
                        MessageManager.AddMessage(new GSystemMessage(Message, Font, Hue, Delay));
                    }
                }
            }
        }

        public static void AddTimer(Timer t)
        {
            m_Timers.Add(t);
        }

        public static void AddToJournal(JournalEntry je)
        {
            if (m_JournalGump != null)
            {
                m_JournalGump.OnEntryAdded();
            }
            m_Journal.Add(je);
        }

        public static void AllNames()
        {
            if ((m_AllNames == null) || m_AllNames.ElapsedReset())
            {
                m_AllNames = new TimeDelay(1f);
                Mobile player = World.Player;
                if (player != null)
                {
                    Item item;
                    IEnumerator enumerator = World.Mobiles.Values.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        Mobile current = (Mobile) enumerator.Current;
                        if ((current.Visible && !current.Player) && World.InRange(current))
                        {
                            current.Look();
                        }
                    }
                    enumerator = World.Items.Values.GetEnumerator();
                    if (((m_TargetHandler == null) && !player.Ghost) && (!player.Flags[MobileFlag.Hidden] && !m_Meditating))
                    {
                        ArrayList dataStore = GetDataStore();
                        while (enumerator.MoveNext())
                        {
                            item = (Item) enumerator.Current;
                            if ((item.Visible && (item.IsCorpse || item.IsBones)) && World.InRange(item))
                            {
                                item.Look();
                                if (item.InSquareRange(player, 2))
                                {
                                    dataStore.Add(item);
                                }
                            }
                        }
                        if (dataStore.Count > 0)
                        {
                            dataStore.Sort(PlayerDistanceSorter.Comparer);
                            ((Item) dataStore[0]).Use();
                        }
                        ReleaseDataStore(dataStore);
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            item = (Item) enumerator.Current;
                            if ((item.Visible && (item.IsCorpse || item.IsBones)) && World.InRange(item))
                            {
                                item.Look();
                            }
                        }
                    }
                }
            }
        }

        public static void AppearanceHuePicker_OnHueRelease(int Hue, Gump Sender)
        {
            GImage tag = (GImage) Sender.GetTag("Image");
            GHuePreview preview = (GHuePreview) Sender.GetTag("Preview");
            if ((((int) tag.GetTag("ItemID")) == 0) || !Map.m_ItemFlags[((int) tag.GetTag("ItemID")) & 0x3fff][TileFlag.PartialHue])
            {
                Hue ^= 0x8000;
            }
            preview.Hue = Hue;
            tag.Hue = Hues.Load(Hue);
            Gumps.Destroy((Gump) Sender.GetTag("Back"));
        }

        public static void AppearanceHuePicker_OnHueSelect(int Hue, Gump Sender)
        {
            GImage tag = (GImage) Sender.GetTag("Image");
            if ((((int) tag.GetTag("ItemID")) == 0) || !Map.m_ItemFlags[((int) tag.GetTag("ItemID")) & 0x3fff][TileFlag.PartialHue])
            {
                Hue ^= 0x8000;
            }
            tag.Hue = Hues.Load(Hue);
        }

        public static void AppearanceHueProperty_OnClick(Gump Sender)
        {
            string tag = (string) Sender.GetTag("Property");
            Gump gump = (Gump) Sender.GetTag("Preview");
            Gump gump2 = (Gump) Sender.GetTag("Image");
            GBackground background = new GBackground(0xe14, 0x97, 310, 0x1db, 0x7d, true);
            Gump toAdd = null;
            string str = tag;
            if (str != null)
            {
                str = string.IsInterned(str);
                if (str == "Skin Tone")
                {
                    toAdd = new GSkinHuePickerAll(background.OffsetX, background.OffsetY, background.UseWidth, background.UseHeight);
                    ((GSkinHuePickerAll) toAdd).OnHueRelease = new OnHueSelect(Engine.AppearanceHuePicker_OnHueRelease);
                    ((GSkinHuePickerAll) toAdd).OnHueSelect = new OnHueSelect(Engine.AppearanceHuePicker_OnHueSelect);
                    toAdd.SetTag("Back", background);
                    toAdd.SetTag("Preview", gump);
                    toAdd.SetTag("Image", gump2);
                    toAdd.SetTag("Sender", Sender);
                }
                else if (str == "Shirt Color")
                {
                    toAdd = new GHuePickerAll(background.OffsetX, background.OffsetY, background.UseWidth, background.UseHeight);
                    ((GHuePickerAll) toAdd).OnHueRelease = new OnHueSelect(Engine.AppearanceHuePicker_OnHueRelease);
                    ((GHuePickerAll) toAdd).OnHueSelect = new OnHueSelect(Engine.AppearanceHuePicker_OnHueSelect);
                    toAdd.SetTag("Back", background);
                    toAdd.SetTag("Preview", gump);
                    toAdd.SetTag("Image", gump2);
                    toAdd.SetTag("Sender", Sender);
                }
                else if (str == "Pants Color")
                {
                    toAdd = new GHuePickerAll(background.OffsetX, background.OffsetY, background.UseWidth, background.UseHeight);
                    ((GHuePickerAll) toAdd).OnHueRelease = new OnHueSelect(Engine.AppearanceHuePicker_OnHueRelease);
                    ((GHuePickerAll) toAdd).OnHueSelect = new OnHueSelect(Engine.AppearanceHuePicker_OnHueSelect);
                    toAdd.SetTag("Back", background);
                    toAdd.SetTag("Preview", gump);
                    toAdd.SetTag("Image", gump2);
                    toAdd.SetTag("Sender", Sender);
                }
                else if (str == "Hair Color")
                {
                    toAdd = new GHairHuePickerAll(background.OffsetX, background.OffsetY, background.UseWidth, background.UseHeight);
                    ((GHairHuePickerAll) toAdd).OnHueRelease = new OnHueSelect(Engine.AppearanceHuePicker_OnHueRelease);
                    ((GHairHuePickerAll) toAdd).OnHueSelect = new OnHueSelect(Engine.AppearanceHuePicker_OnHueSelect);
                    toAdd.SetTag("Back", background);
                    toAdd.SetTag("Preview", gump);
                    toAdd.SetTag("Image", gump2);
                    toAdd.SetTag("Sender", Sender);
                }
                else
                {
                    if (str != "Facial Hair Color")
                    {
                        return;
                    }
                    toAdd = new GHairHuePickerAll(background.OffsetX, background.OffsetY, background.UseWidth, background.UseHeight);
                    ((GHairHuePickerAll) toAdd).OnHueRelease = new OnHueSelect(Engine.AppearanceHuePicker_OnHueRelease);
                    ((GHairHuePickerAll) toAdd).OnHueSelect = new OnHueSelect(Engine.AppearanceHuePicker_OnHueSelect);
                    toAdd.SetTag("Back", background);
                    toAdd.SetTag("Preview", gump);
                    toAdd.SetTag("Image", gump2);
                    toAdd.SetTag("Sender", Sender);
                }
                toAdd.X += (background.UseWidth - toAdd.Width) / 2;
                toAdd.Y += (background.UseHeight - toAdd.Height) / 2;
                background.Children.Add(toAdd);
                Gumps.Desktop.Children.Add(background);
            }
        }

        private static void AttackDialog_YesNo(bool yes)
        {
        }

        public static bool AttackLast()
        {
            return (((m_LastHarmTarget != null) && (m_LastHarmTarget is Mobile)) && ((Mobile) m_LastHarmTarget).Attack());
        }

        public static void AttackModeToggle_OnClick(Gump Sender)
        {
            Network.Send(new PSetWarMode(!World.Player.Flags[MobileFlag.Warmode], 0x20, 0));
        }

        public static bool AttackRed()
        {
            IEnumerator enumerator = World.Mobiles.Values.GetEnumerator();
            ArrayList dataStore = GetDataStore();
            while (enumerator.MoveNext())
            {
                Mobile current = (Mobile) enumerator.Current;
                if ((current.Visible && (current.Notoriety == Notoriety.Murderer)) && (!current.Ghost && World.InRange(current)))
                {
                    dataStore.Add(current);
                }
            }
            if (dataStore.Count > 0)
            {
                dataStore.Sort(PlayerDistanceSorter.Comparer);
                Mobile mobile2 = (Mobile) dataStore[0];
                ReleaseDataStore(dataStore);
                return mobile2.Attack();
            }
            ReleaseDataStore(dataStore);
            return false;
        }

        public static void AutoTarget_Expire(Timer t)
        {
            if (t.HasTag("Session"))
            {
                AutoTargetSession tag = (AutoTargetSession) t.GetTag("Session");
                m_AutoTarget.Remove(tag);
            }
        }

        public static bool BandageSelf()
        {
            Mobile player = World.Player;
            if (player != null)
            {
                if (!player.Ghost)
                {
                    if ((player.HPCur != player.HPMax) || player.Flags[MobileFlag.Poisoned])
                    {
                        Item backpack = player.Backpack;
                        if (backpack != null)
                        {
                            Item[] array = backpack.FindItems(new ItemIDValidator(new int[] { 0xe21, 0xee9 }));
                            if (array.Length > 0)
                            {
                                Array.Sort(array, AmountSorter.Comparer);
                                int num = 0;
                                for (int i = 0; i < array.Length; i++)
                                {
                                    num += (ushort) array[i].Amount;
                                }
                                bool flag = array[0].Use();
                                AddTargetSelf(TargetAction.Bandage);
                                num--;
                                if (num == 0)
                                {
                                    AddTextMessage("That was your last bandage!", DefaultFont, Hues.Load(0x22));
                                    return flag;
                                }
                                if (num <= 5)
                                {
                                    AddTextMessage(string.Format("You are running very low on bandages! There are {0} remaining.", num), DefaultFont, Hues.Load(0x22));
                                    return flag;
                                }
                                if (num <= 10)
                                {
                                    AddTextMessage(string.Format("You are running low on bandages. There are {0} remaining.", num), DefaultFont, Hues.Load(0x22));
                                    return flag;
                                }
                                AddTextMessage(string.Format("You have {0} bandages remaining.", num));
                                return flag;
                            }
                            AddTextMessage("You have no bandages!", DefaultFont, Hues.Load(0x22));
                        }
                        else
                        {
                            AddTextMessage("You do not have a backpack.");
                        }
                    }
                    else
                    {
                        AddTextMessage("You do not need to be bandaged.");
                    }
                }
                else
                {
                    AddTextMessage("You are dead.");
                }
            }
            return false;
        }

        public static int Biggest(int x, int y)
        {
            if (x > y)
            {
                return x;
            }
            return y;
        }

        public static int BitCount(int v)
        {
            int num = 0;
            while (v != 0)
            {
                if ((v & 1) != 0)
                {
                    num++;
                }
                v = v >> 1;
            }
            return num;
        }

        public static GMainMenu BuildSmartLoginMenu()
        {
            ServerProfile[] list = Profiles.List;
            GMainMenu menu = new GMainMenu(0, 270);
            for (int i = 0; i < list.Length; i++)
            {
                ServerProfile server = list[i];
                GMenuItem child = new GNewAccountMenu(server, server.Title);
                server.Menu = child;
                for (int j = 0; j < server.Accounts.Length; j++)
                {
                    GMenuItem item2;
                    AccountProfile account = server.Accounts[j];
                    if (server.Accounts.Length == 1)
                    {
                        item2 = child;
                    }
                    else
                    {
                        item2 = new GAccountMenu(account);
                        child.Add(item2);
                    }
                    account.Menu = item2;
                    for (int k = 0; k < account.Shards.Length; k++)
                    {
                        GMenuItem item3;
                        ShardProfile shard = account.Shards[k];
                        if (account.Shards.Length == 1)
                        {
                            item3 = item2;
                        }
                        else
                        {
                            item3 = new GShardMenu(shard);
                            item2.Add(item3);
                        }
                        shard.Menu = item3;
                        int num4 = 0;
                        for (int m = 0; m < shard.Characters.Length; m++)
                        {
                            if (shard.Characters[m] != null)
                            {
                                GMenuItem item4 = new GPlayCharacterMenu(shard.Characters[m]);
                                shard.Characters[m].Menu = item4;
                                item3.Add(item4);
                                num4++;
                            }
                        }
                    }
                    if (item2 != child)
                    {
                        child.Add(item2);
                    }
                }
                menu.Add(child);
            }
            menu.Add(new GNewServerMenu());
            menu.GUID = "Smart Login";
            return menu;
        }

        public static byte ByteCap(int Value)
        {
            if (Value < 0)
            {
                return 0;
            }
            if (Value > 0xff)
            {
                return 0xff;
            }
            return (byte) Value;
        }

        public static int C16232(int C16)
        {
            float num = (C16 >> 10) & 0x1f;
            float num2 = (C16 >> 5) & 0x1f;
            float num3 = C16 & 0x1f;
            num *= 8.225806f;
            num2 *= 8.225806f;
            num3 *= 8.225806f;
            int num4 = ByteCap((int) num);
            int num5 = ByteCap((int) num2);
            int num6 = ByteCap((int) num3);
            return (((num4 << 0x10) | (num5 << 8)) | num6);
        }

        public static ushort C32216(int c32)
        {
            int num = (c32 >> 0x10) & 0xff;
            int num2 = (c32 >> 8) & 0xff;
            int num3 = c32 & 0xff;
            num *= 0x1f;
            num2 *= 0x1f;
            num3 *= 0x1f;
            num /= 0xff;
            num2 /= 0xff;
            num3 /= 0xff;
            return (ushort) (((0x8000 | (num << 10)) | (num2 << 5)) | num3);
        }

        public static void CancelClick()
        {
            m_ClickTimer.Stop();
        }

        public static void ChangeDir(Direction dir)
        {
            int walkDirection = GetWalkDirection(dir);
            Mobile player = World.Player;
            if ((player != null) && ((player.Direction & 7) != (walkDirection & 7)))
            {
                EquipSort(player, walkDirection);
                player.Direction = (byte) walkDirection;
                SendMovementRequest(walkDirection, player.X, player.Y, player.Z);
            }
        }

        public static void CharCreationAppearanceArrow_OnClick(Gump Sender)
        {
            int tag = (int) Sender.GetTag("Strength");
            int dex = (int) Sender.GetTag("Dexterity");
            int @int = (int) Sender.GetTag("Intelligence");
            int num4 = (int) Sender.GetTag("vSkill1");
            int num5 = (int) Sender.GetTag("vSkill2");
            int num6 = (int) Sender.GetTag("vSkill3");
            int num7 = (int) Sender.GetTag("iSkill1");
            int num8 = (int) Sender.GetTag("iSkill2");
            int num9 = (int) Sender.GetTag("iSkill3");
            int hSkinTone = ((GHuePreview) Sender.GetTag("Skin Tone")).Hue | 0x8000;
            int hShirtColor = ((GHuePreview) Sender.GetTag("Shirt Color")).Hue & 0x7fff;
            int hPantsColor = ((GHuePreview) Sender.GetTag("Pants Color")).Hue & 0x7fff;
            int hHairColor = ((GHuePreview) Sender.GetTag("Hair Color")).Hue & 0x7fff;
            int hFacialHairColor = ((GHuePreview) Sender.GetTag("Facial Hair Color")).Hue & 0x7fff;
            string name = ((GTextBox) Sender.GetTag("Name")).String;
            int gender = (int) Sender.GetTag("Gender");
            if ((((num4 + num5) + num6) == 100) && ((((((tag + dex) + @int) == 80) && (num7 != -1)) && ((num8 != -1) && (num9 != -1))) && (((num7 != num8) && (num7 != num9)) && (num8 != num9))))
            {
                if ((name == null) || (name.Length <= 1))
                {
                    ((GTextBox) Sender.GetTag("Name")).Focus();
                    Cursor.MoveTo((GTextBox) Sender.GetTag("Name"));
                }
                else
                {
                    ShowCharCitySelection(tag, dex, @int, num4, num5, num6, num7, num8, num9, hSkinTone, hShirtColor, hPantsColor, hHairColor, hFacialHairColor, name, gender);
                }
            }
        }

        public static void CharCreationSkillsArrow_OnClick(Gump Sender)
        {
            int str = Convert.ToInt32(((GLabel) Sender.GetTag("Strength")).Text);
            int dex = Convert.ToInt32(((GLabel) Sender.GetTag("Dexterity")).Text);
            int @int = Convert.ToInt32(((GLabel) Sender.GetTag("Intelligence")).Text);
            int num4 = Convert.ToInt32(((GLabel) Sender.GetTag("vSkill1")).Text);
            int num5 = Convert.ToInt32(((GLabel) Sender.GetTag("vSkill2")).Text);
            int num6 = Convert.ToInt32(((GLabel) Sender.GetTag("vSkill3")).Text);
            int tag = (int) ((Gump) Sender.GetTag("iSkill1")).GetTag("Skill");
            int num8 = (int) ((Gump) Sender.GetTag("iSkill2")).GetTag("Skill");
            int num9 = (int) ((Gump) Sender.GetTag("iSkill3")).GetTag("Skill");
            if (((num4 + num5) + num6) != 100)
            {
                Gumps.MessageBoxOk("The total of all your skills must equal 100.0", true, null);
            }
            else if (((str + dex) + @int) != 80)
            {
                Gumps.MessageBoxOk("The total of all your stats must equal 100.0", true, null);
            }
            else if (tag == -1)
            {
                GTextButton sender = (GTextButton) Sender.GetTag("iSkill1");
                CharSkillBox_OnClick(sender);
                Cursor.MoveTo(sender);
            }
            else if (num8 == -1)
            {
                GTextButton button2 = (GTextButton) Sender.GetTag("iSkill2");
                CharSkillBox_OnClick(button2);
                Cursor.MoveTo(button2);
            }
            else if (num9 == -1)
            {
                GTextButton button3 = (GTextButton) Sender.GetTag("iSkill3");
                CharSkillBox_OnClick(button3);
                Cursor.MoveTo(button3);
            }
            else if (tag == num8)
            {
                GTextButton button4 = (GTextButton) Sender.GetTag("iSkill2");
                CharSkillBox_OnClick(button4);
                Cursor.MoveTo(button4);
            }
            else if ((tag == num9) || (num8 == num9))
            {
                GTextButton button5 = (GTextButton) Sender.GetTag("iSkill3");
                CharSkillBox_OnClick(button5);
                Cursor.MoveTo(button5);
            }
            else
            {
                ShowCharAppearance(str, dex, @int, num4, num5, num6, tag, num8, num9);
            }
        }

        private static string CharEntity_Match(Match m)
        {
            try
            {
                int num = Convert.ToInt32(m.Groups[1].Value, 0x10);
                switch (num)
                {
                    case 10:
                    case 13:
                        return m.Groups[0].Value;
                }
                char ch = (char) num;
                return ch.ToString();
            }
            catch
            {
                return m.Groups[0].Value;
            }
        }

        public static void CharGender_OnClick(Gump Sender)
        {
            GImage tag = (GImage) Sender.GetTag("Image");
            int num = (((int) Sender.GetTag("Gender")) == 0) ? 1 : 0;
            GButton button = (GButton) Sender;
            button.SetGumpID(0x710 - (num * 3));
            button.SetTag("Gender", num);
            ((Gump) Sender.GetTag("Arrow")).SetTag("Gender", num);
            int[,] numArray = new int[,] { { 0x761, 0x760 }, { 0x739, 0x714 }, { 0x738, 0x764 }, { 0x753, 0x737 }, { 0x759, 0 }, { 0x762, 0x763 } };
            for (int i = 0; i < 6; i++)
            {
                ((GImage) Sender.GetTag(string.Format("Image[{0}]", i))).GumpID = numArray[i, num];
            }
            bool flag = num == 0;
            ((Gump) Sender.GetTag("HideHS")).Visible = flag;
            ((Gump) Sender.GetTag("HideTB")).Visible = flag;
            ((Gump) Sender.GetTag("HideHP")).Visible = flag;
        }

        public static void CharSkill_OnClick(Gump Sender)
        {
            if (Sender.HasTag("Box"))
            {
                GTextButton tag = (GTextButton) Sender.GetTag("Box");
                Skill skill = Skills[((GListItem) Sender.GetTag("Clicked")).Index];
                tag.SetTag("Skill", ((GListItem) Sender.GetTag("Clicked")).Index);
                tag.Text = skill.Name;
                ((GTextButton) Sender.GetTag("Box")).DefaultHue = Hues.Load(0x76b);
                ((GTextButton) Sender.GetTag("Box")).FocusHue = Hues.Load(0x961);
                Sender.RemoveTag("Box");
            }
        }

        public static void CharSkillBox_OnClick(Gump Sender)
        {
            if (((Gump) Sender.GetTag("List")).HasTag("Box"))
            {
                ((GTextButton) ((Gump) Sender.GetTag("List")).GetTag("Box")).DefaultHue = Hues.Load(0x76b);
                ((GTextButton) ((Gump) Sender.GetTag("List")).GetTag("Box")).FocusHue = Hues.Load(0x961);
            }
            ((Gump) Sender.GetTag("List")).SetTag("Box", Sender);
            ((GTextButton) Sender).DefaultHue = ((GTextButton) Sender).FocusHue = Hues.Load(0x676);
        }

        public static void CharSlot_OnClick(Gump Sender)
        {
            if (Sender.HasTag("CharID"))
            {
                int tag = (int) Sender.GetTag("CharID");
                Entry e = new Entry {
                    AccountName = NewConfig.Username,
                    Password = NewConfig.Password,
                    CharID = tag,
                    CharName = m_CharacterNames[tag],
                    ServerID = NewConfig.LastServerID
                };
                for (int i = 0; i < m_Servers.Length; i++)
                {
                    if (m_Servers[i].ServerID == e.ServerID)
                    {
                        e.ServerName = m_Servers[i].Name;
                    }
                }
                QuickLogin.Add(e);
                Cursor.Hourglass = true;
                Network.Send(new PCharSelect(m_CharacterNames[tag], tag));
                if (Animations.IsLoading)
                {
                    Gumps.Desktop.Children.Clear();
                    xGumps.Display("AnimationLoad");
                    do
                    {
                        DrawNow();
                    }
                    while (!Animations.WaitLoading());
                }
                Gumps.Desktop.Children.Clear();
                xGumps.Display("EnterBritannia");
                DrawNow();
            }
        }

        public static void City_OnClick(Gump Sender)
        {
            int tag = (int) Sender.GetTag("Strength");
            int num2 = (int) Sender.GetTag("Dexterity");
            int num3 = (int) Sender.GetTag("Intelligence");
            int num4 = (int) Sender.GetTag("vSkill1");
            int num5 = (int) Sender.GetTag("vSkill2");
            int num6 = (int) Sender.GetTag("vSkill3");
            int num7 = (int) Sender.GetTag("iSkill1");
            int num8 = (int) Sender.GetTag("iSkill2");
            int num9 = (int) Sender.GetTag("iSkill3");
            int num10 = (int) Sender.GetTag("Skin Tone");
            int num11 = (int) Sender.GetTag("Shirt Color");
            int num12 = (int) Sender.GetTag("Pants Color");
            int num13 = (int) Sender.GetTag("Hair Color");
            int num14 = (int) Sender.GetTag("Facial Hair Color");
            int num15 = (int) Sender.GetTag("CityID");
            string name = (string) Sender.GetTag("Name");
            int num16 = (int) Sender.GetTag("Gender");
            if (((((((num4 + num5) + num6) == 100) && ((num7 != num8) && (num7 != num9))) && ((num8 != num9) && (((tag + num2) + num3) == 80))) && (((name != null) && (name.Length > 1)) && ((num15 >= 0) && (num15 < 9)))) && ((num16 >= 0) && (num16 <= 1)))
            {
                GLabel label;
                Cursor.Hourglass = true;
                int num17 = 0;
                for (int i = 0; i < 5; i++)
                {
                    if ((m_CharacterNames[i] == null) || (m_CharacterNames[i].Length <= 0))
                    {
                        num17 = i;
                        break;
                    }
                }
                Network.Send(new PCreateCharacter(name, (byte) num16, (byte) tag, (byte) num2, (byte) num3, (byte) num7, (byte) num4, (byte) num8, (byte) num5, (byte) num9, (byte) num6, (short) (num10 | 0x8000), 0x203b, (short) (num13 & 0x7fff), 0x2040, (short) (num14 & 0x7fff), (short) num15, (short) num17, Network.ClientIP, (short) (num11 & 0x7fff), (short) (num12 & 0x7fff)));
                if (Animations.IsLoading)
                {
                    Gumps.Desktop.Children.Clear();
                    xGumps.Display("AnimationLoad");
                    do
                    {
                        DrawNow();
                    }
                    while (!Animations.WaitLoading());
                }
                Gumps.Desktop.Children.Clear();
                GBackground toAdd = new GBackground(0xa2c, 0x164, 0xd4, 0x8e, 0x86, true);
                label = new GLabel("Entering Britannia..", GetFont(2), Hues.Load(0x75f), 0x74, 0x2a) {
                    X = (toAdd.Width - label.Width) / 2
                };
                toAdd.Children.Add(label);
                toAdd.Children.Add(new GButton(0x47e, 0xa4, 170, null));
                Gumps.Desktop.Children.Add(new GBackground(0x588, ScreenWidth, ScreenHeight, false));
                Gumps.Desktop.Children.Add(toAdd);
                Gumps.Desktop.Children.Add(new GImage(0x157c, 0, 0));
                Gumps.Desktop.Children.Add(new GImage(0x15a0, 0, 4));
                GButton button = new GButton(0x1589, 0x22b, 4, new OnClick(Engine.Quit_OnClick)) {
                    Tooltip = new Tooltip(Strings.GetString("Tooltips.Quit"))
                };
                Gumps.Desktop.Children.Add(button);
                DrawNow();
            }
        }

        public static void ClearPings()
        {
            m_Pings.Clear();
            m_PingID = 0;
            m_Ping = 0;
            if (m_PingTimer != null)
            {
                m_PingTimer.Delete();
            }
        }

        public static void Click(object sender, EventArgs e)
        {
            if ((m_EventOk && !m_Locked) && !amMoving)
            {
                m_LastDown = -1;
                short tileX = 0;
                short tileY = 0;
                Renderer.ResetHitTest();
                ICell cell = Renderer.FindTileFromXY(m_xClick, m_yClick, ref tileX, ref tileY);
                if ((cell != null) && (m_TargetHandler == null))
                {
                    if (cell.GetType() == typeof(DynamicItem))
                    {
                        ((DynamicItem) cell).m_Item.OnSingleClick();
                    }
                    else if (cell.GetType() == typeof(MobileCell))
                    {
                        ((MobileCell) cell).m_Mobile.OnSingleClick();
                    }
                    else if (cell.GetType() == typeof(CorpseCell))
                    {
                        Network.Send(new PLookRequest((CorpseCell) cell));
                    }
                    else if (cell.GetType() == typeof(StaticItem))
                    {
                        string message = Localization.GetString(0xf9060 + (((StaticItem) cell).ID & 0x3fff)).Trim();
                        if (message.Length > 0)
                        {
                            World.AddStaticMessage(((StaticItem) cell).Serial, message);
                        }
                    }
                    else if (((cell.GetType() == typeof(LandTile)) && (Gumps.Drag != null)) && (Gumps.Drag.GetType() == typeof(GDraggedItem)))
                    {
                        GDraggedItem drag = (GDraggedItem) Gumps.Drag;
                        Network.Send(new PDropItem(drag.Item.Serial, tileX, tileY, (sbyte) (cell.Z + cell.Height), -1));
                        Gumps.Destroy(drag);
                    }
                }
            }
        }

        public static void ClickMessage(object sender, EventArgs e)
        {
            if ((m_EventOk && !m_Locked) && !amMoving)
            {
                m_ClickSender = sender;
                m_ClickArgs = e;
                m_xClick = m_xMouse;
                m_yClick = m_yMouse;
                m_ClickList = Gumps.FindListForSingleClick(m_xMouse, m_yMouse);
                m_ClickTimer.Stop();
                m_ClickTimer.Start(false);
            }
        }

        private static void ClickTimer_OnTick(Timer t)
        {
            if (m_ClickList != null)
            {
                Gump gump = (Gump) m_ClickList[0];
                Point point = (Point) m_ClickList[1];
                gump.OnSingleClick(point.X, point.Y);
            }
            else
            {
                Click(m_ClickSender, m_ClickArgs);
            }
            m_ClickTimer.Stop();
        }

        public static void commandEntered(string cmd)
        {
            if ((cmd.Length > 0) && !m_SayMacro)
            {
                m_LastCommand = cmd;
            }
            if (m_Prompt != null)
            {
                m_Prompt.OnReturn(cmd);
                m_Prompt = null;
            }
            else
            {
                cmd = cmd.Trim();
                if (cmd.Length > 0)
                {
                    int count = m_Plugins.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Plugin plugin = (Plugin) m_Plugins[i];
                        if (plugin.OnCommandEntered(cmd))
                        {
                            return;
                        }
                    }
                    if (cmd.StartsWith("--") && UOAM.Connected)
                    {
                        UOAM.Chat(cmd.Substring(2));
                    }
                    else if (!cmd.StartsWith("/"))
                    {
                        if (!cmd.StartsWith(". "))
                        {
                            Network.Send(new PUnicodeSpeech(cmd));
                        }
                        else
                        {
                            object obj2;
                            string[] strArray = cmd.Substring(". ".Length).Split(new char[] { ' ' });
                            if (((strArray.Length > 0) && ((obj2 = strArray[0].ToLower()) != null)) && ((obj2 = <PrivateImplementationDetails>.$$method0x60008d2-1[obj2]) != null))
                            {
                                switch (((int) obj2))
                                {
                                    case 0:
                                    {
                                        Gump g = Gumps.FindGumpByGUID("Volume");
                                        if (g != null)
                                        {
                                            Gumps.Destroy(g);
                                            break;
                                        }
                                        Gumps.Desktop.Children.Add(new GVolumeControl());
                                        break;
                                    }
                                    case 1:
                                    {
                                        int num4 = 0;
                                        try
                                        {
                                            if (strArray.Length > 1)
                                            {
                                                num4 = Convert.ToInt32(strArray[1]);
                                            }
                                        }
                                        catch
                                        {
                                            num4 = 0;
                                        }
                                        if (num4 < 1)
                                        {
                                            AddTextMessage("Usage: DefaultRegs <amount>");
                                        }
                                        else
                                        {
                                            World.CharData.DefaultRegs = num4;
                                            AddTextMessage(string.Format("Default reagent amount changed to {0}.", num4));
                                        }
                                        break;
                                    }
                                    case 2:
                                    {
                                        bool flag = !World.CharData.AlwaysRun;
                                        if ((strArray.Length <= 1) || !(strArray[0].ToLower() == "on"))
                                        {
                                            if ((strArray.Length > 1) && (strArray[0].ToLower() == "off"))
                                            {
                                                flag = false;
                                            }
                                        }
                                        else
                                        {
                                            flag = true;
                                        }
                                        World.CharData.AlwaysRun = flag;
                                        return;
                                    }
                                    case 3:
                                        TargetHandler = new SetRegBagTargetHandler();
                                        AddTextMessage("Target your destination reagent container.");
                                        break;

                                    case 4:
                                        TargetHandler = new SetRegStockTargetHandler();
                                        AddTextMessage("Target your source reagent container.");
                                        break;

                                    case 5:
                                    {
                                        int num5 = 0;
                                        foreach (Mobile mobile2 in World.Mobiles.Values)
                                        {
                                            if ((World.InRange(mobile2) && mobile2.Visible) && mobile2.m_IsFactionGuard)
                                            {
                                                mobile2.QueryStats();
                                                mobile2.OpenStatus(false);
                                                if (mobile2.StatusBar != null)
                                                {
                                                    mobile2.StatusBar.Gump.X = (GameX + 10) + ((num5 / 6) * (mobile2.StatusBar.Gump.Width + 10));
                                                    mobile2.StatusBar.Gump.Y = (GameY + 10) + ((num5 % 6) * (mobile2.StatusBar.Gump.Height + 10));
                                                    num5++;
                                                }
                                            }
                                        }
                                        break;
                                    }
                                    case 6:
                                    {
                                        Mobile player = World.Player;
                                        if (player != null)
                                        {
                                            for (int j = 0; j < Multis.Items.Count; j++)
                                            {
                                                Item item = (Item) Multis.Items[j];
                                                if (item.InWorld && item.IsMulti)
                                                {
                                                    CustomMultiEntry customMulti = CustomMultiLoader.GetCustomMulti(item.Serial, item.Revision);
                                                    Multi m = null;
                                                    if (customMulti != null)
                                                    {
                                                        m = customMulti.Multi;
                                                    }
                                                    if (m == null)
                                                    {
                                                        m = item.Multi;
                                                    }
                                                    if ((m != null) && Multis.IsInMulti(item, m, player.X, player.Y, player.Z))
                                                    {
                                                        ArrayList dataStore = GetDataStore();
                                                        foreach (Mobile mobile4 in World.Mobiles.Values)
                                                        {
                                                            switch (mobile4.Notoriety)
                                                            {
                                                                case Notoriety.Attackable:
                                                                case Notoriety.Criminal:
                                                                case Notoriety.Enemy:
                                                                case Notoriety.Murderer:
                                                                    if ((((mobile4 != player) && !mobile4.Flags[MobileFlag.YellowHits]) && (!mobile4.m_IsFriend && mobile4.Visible)) && World.InRange(mobile4))
                                                                    {
                                                                        int xReal = mobile4.XReal;
                                                                        int yReal = mobile4.YReal;
                                                                        int zReal = mobile4.ZReal;
                                                                        if (Multis.RunUO_IsInside(item, m, xReal, yReal, zReal))
                                                                        {
                                                                            dataStore.Add(mobile4);
                                                                        }
                                                                    }
                                                                    break;
                                                            }
                                                        }
                                                        if (dataStore.Count > 0)
                                                        {
                                                            dataStore.Sort(TargetSorter.Comparer);
                                                            Mobile mobile5 = (Mobile) dataStore[0];
                                                            Network.Send(new PUnicodeSpeech("; i ban thee"));
                                                            Network.Send(new PTarget_Spoof(0, -559038737, ServerTargetFlags.None, mobile5.Serial, mobile5.X, mobile5.Y, mobile5.Z, mobile5.Body));
                                                            PacketHandlers.m_CancelTarget = true;
                                                        }
                                                        ReleaseDataStore(dataStore);
                                                        break;
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                        break;
                                    }
                                    case 7:
                                    {
                                        if (strArray.Length != 2)
                                        {
                                            break;
                                        }
                                        int level = m_Multis.m_Level;
                                        switch (strArray[1].ToLower())
                                        {
                                            case "0":
                                            case "5":
                                            case "off":
                                                level = 5;
                                                break;

                                            case "1":
                                                level = 1;
                                                break;

                                            case "2":
                                                level = 2;
                                                break;

                                            case "3":
                                                level = 3;
                                                break;

                                            case "4":
                                                level = 4;
                                                break;

                                            case "up":
                                                level++;
                                                break;

                                            case "down":
                                                level--;
                                                break;
                                        }
                                        if (level > 5)
                                        {
                                            level = 5;
                                        }
                                        else if (level < 1)
                                        {
                                            level = 1;
                                        }
                                        if (m_Multis.m_Level != level)
                                        {
                                            m_Multis.m_Level = level;
                                            Map.Invalidate();
                                        }
                                        return;
                                    }
                                    case 8:
                                    {
                                        int defaultRegs = World.CharData.DefaultRegs;
                                        try
                                        {
                                            if (strArray.Length > 1)
                                            {
                                                defaultRegs = Convert.ToInt32(strArray[1]);
                                            }
                                        }
                                        catch
                                        {
                                            defaultRegs = World.CharData.DefaultRegs;
                                        }
                                        Item stock = World.CharData.Stock;
                                        Item regBag = World.CharData.RegBag;
                                        if ((stock != null) && (regBag != null))
                                        {
                                            MakeRegsTargetHandler.Transfer(stock, regBag, defaultRegs);
                                        }
                                        else
                                        {
                                            TargetHandler = new MakeRegsTargetHandler(defaultRegs, stock, regBag);
                                            if (stock == null)
                                            {
                                                AddTextMessage("Target your source reagent container.");
                                            }
                                            else
                                            {
                                                AddTextMessage("Target your destination reagent container.");
                                            }
                                        }
                                        break;
                                    }
                                    case 9:
                                        if (m_QamList.Count <= 0)
                                        {
                                            AddTextMessage("There are no item movements currently queued.");
                                            break;
                                        }
                                        m_QamList.Clear();
                                        if (m_QamTimer != null)
                                        {
                                            m_QamTimer.Stop();
                                        }
                                        m_QamTimer = null;
                                        AddTextMessage("The item movement queue has been cleared.");
                                        break;

                                    case 10:
                                        TargetHandler = new FriendTargetHandler();
                                        AddTextMessage("Target the player to toggle friendship status.", DefaultFont, Hues.Load(0x59));
                                        break;

                                    case 11:
                                        World.CharData.AutoPickup = !World.CharData.AutoPickup;
                                        AddTextMessage(World.CharData.AutoPickup ? "You like good stuff." : "You hate things.");
                                        break;

                                    case 12:
                                        World.CharData.Archery = !World.CharData.Archery;
                                        AddTextMessage(World.CharData.Archery ? "You are an archer." : "You aren't an archer.");
                                        break;

                                    case 13:
                                        World.CharData.LootGold = !World.CharData.LootGold;
                                        AddTextMessage(World.CharData.LootGold ? "You like gold." : "You hate gold.");
                                        break;

                                    case 14:
                                    {
                                        Mobile mob = World.Player;
                                        if (mob != null)
                                        {
                                            string mobilePath = Macros.GetMobilePath(mob);
                                            string path = FileManager.BasePath(string.Format("Data/Plugins/Macros/{0}.txt", mobilePath));
                                            string str8 = "Macros";
                                            string str9 = FileManager.BasePath(string.Format("Data/Plugins/Macros/{0}.txt", str8));
                                            if (GMacroEditorForm.IsOpen)
                                            {
                                                AddTextMessage("Close the macro editor before running this command.");
                                            }
                                            else if (File.Exists(path))
                                            {
                                                AddTextMessage("Macro definitions unique to your character were already found.");
                                            }
                                            else if (!File.Exists(str9))
                                            {
                                                AddTextMessage("No default macro definnitions were found.");
                                            }
                                            else
                                            {
                                                File.Copy(str9, path);
                                                Macros.Load();
                                                AddTextMessage("Default macro definitions have been copied. Any macro changes you make will now be unique to this character.");
                                            }
                                            break;
                                        }
                                        break;
                                    }
                                    case 15:
                                        TargetHandler = new NullTargetHandler();
                                        break;

                                    case 0x10:
                                    {
                                        Mobile p = World.Player;
                                        if (p != null)
                                        {
                                            bool flag2 = false;
                                            foreach (Mobile mobile8 in World.Mobiles.Values)
                                            {
                                                if (mobile8.InSquareRange(p, 8) && ((mobile8.Body == 400) || (mobile8.Body == 0x191)))
                                                {
                                                    Item item4 = mobile8.FindEquip(Layer.Shoes);
                                                    if (((item4 != null) && (item4.Hue == 0)) && (((item4.ID == 0x1711) || (item4.ID == 0x170b)) || (item4.ID == 0x170c)))
                                                    {
                                                        item4 = mobile8.FindEquip(Layer.TwoHanded);
                                                        if (((item4 != null) && (item4.Hue == 0)) && (((item4.ID == 0xe89) || (item4.ID == 0xe8a)) || ((item4.ID == 0xe81) || (item4.ID == 0xe82))))
                                                        {
                                                            m_ContextQueue = 0x17d7;
                                                            m_BuyHorse = mobile8;
                                                            Network.Send(new PPopupRequest(mobile8));
                                                            mobile8.AddTextMessage("Vendor", "Buying horse.", DefaultFont, Hues.Load(0x59), false);
                                                            flag2 = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            if (!flag2)
                                            {
                                                AddTextMessage("Vendor not found.", DefaultFont, Hues.Load(0x26));
                                            }
                                            return;
                                        }
                                        break;
                                    }
                                    case 0x11:
                                    {
                                        Item item5 = World.FindItem(new PlayerDistanceValidator(new ItemIDValidator(new int[] { 0xf6c }), 1));
                                        if (item5 != null)
                                        {
                                            item5.Use();
                                            item5.AddTextMessage("Moongate", "Using.", DefaultFont, Hues.Load(0x59), false);
                                            break;
                                        }
                                        AddTextMessage("Moongate not found.", DefaultFont, Hues.Load(0x26));
                                        break;
                                    }
                                    case 0x12:
                                    {
                                        Gump drag = Gumps.Drag;
                                        if (!(drag is GDraggedItem))
                                        {
                                            m_LeapFrog = null;
                                            AddTextMessage("You are not holding an item. Leapfrog cleared.");
                                            break;
                                        }
                                        m_LeapFrog = ((GDraggedItem) drag).Item;
                                        AddTextMessage("Item set.");
                                        break;
                                    }
                                    case 0x13:
                                    {
                                        IItemValidator check = new PlayerDistanceValidator(new ItemIDValidator(new int[] { 0x2006 }), 2);
                                        Item[] itemArray = World.FindItems(check);
                                        ArrayList list2 = new ArrayList();
                                        AosAppraiser instance = LootAppraiser.Instance;
                                        for (int k = 0; k < itemArray.Length; k++)
                                        {
                                            Item item6 = itemArray[k];
                                            if ((item6.LastTextHue == null) || ((item6.LastTextHue.HueID() & 0x7fff) != 0x59))
                                            {
                                                AosAppraiser.m_BlueCorpse = false;
                                                ArrayList items = item6.Items;
                                                for (int n = 0; n < items.Count; n++)
                                                {
                                                    Item item7 = (Item) items[n];
                                                    Appraisal appraisal = instance.Appraise(item7);
                                                    if (appraisal != null)
                                                    {
                                                        list2.Add(appraisal);
                                                    }
                                                }
                                            }
                                        }
                                        if (list2.Count > 0)
                                        {
                                            list2.Sort();
                                            Appraisal appraisal2 = (Appraisal) list2[0];
                                            Item item8 = appraisal2.Item;
                                            ObjectPropertyList propertyList = item8.PropertyList;
                                            string text = null;
                                            if (propertyList.Properties.Length > 0)
                                            {
                                                text = propertyList.Properties[0].Text;
                                            }
                                            if ((text == null) || (text == ""))
                                            {
                                                text = Localization.GetString(0xf9060 + (item8.ID & 0x3fff));
                                            }
                                            string str11 = string.Format("Looting {0}.", text);
                                            if (list2.Count > 1)
                                            {
                                                str11 = string.Format("{0} There are {1} other valued item{2} to loot.", str11, list2.Count - 1, (list2.Count == 2) ? "" : "s");
                                            }
                                            AddTextMessage(str11, DefaultFont, Hues.Load(0x35));
                                            Mobile mobile9 = World.Player;
                                            if (mobile9 != null)
                                            {
                                                Network.Send(new PPickupItem(item8, item8.Amount));
                                                Network.Send(new PDropItem(item8.Serial, -1, -1, 0, mobile9.Serial));
                                            }
                                        }
                                        else
                                        {
                                            AddTextMessage("Nothing was found to loot.", DefaultFont, Hues.Load(0x22));
                                        }
                                        break;
                                    }
                                    case 20:
                                    {
                                        IItemValidator validator2 = new PlayerDistanceValidator(new ItemIDValidator(new int[] { 0x2006 }), 2);
                                        Item[] itemArray2 = World.FindItems(validator2);
                                        ArrayList list5 = new ArrayList();
                                        AosAppraiser appraiser2 = LootAppraiser.Instance;
                                        for (int num14 = 0; num14 < itemArray2.Length; num14++)
                                        {
                                            Item item9 = itemArray2[num14];
                                            if ((item9.LastTextHue != null) && ((item9.LastTextHue.HueID() & 0x7fff) == 0x59))
                                            {
                                                AosAppraiser.m_BlueCorpse = true;
                                            }
                                            else
                                            {
                                                AosAppraiser.m_BlueCorpse = false;
                                            }
                                            ArrayList list6 = item9.Items;
                                            for (int num15 = 0; num15 < list6.Count; num15++)
                                            {
                                                Item item10 = (Item) list6[num15];
                                                Appraisal appraisal3 = appraiser2.Appraise(item10);
                                                if ((appraisal3 != null) && !appraisal3.IsWorthless)
                                                {
                                                    list5.Add(appraisal3);
                                                }
                                            }
                                        }
                                        if (list5.Count > 0)
                                        {
                                            list5.Sort();
                                            Appraisal appraisal4 = (Appraisal) list5[0];
                                            Item item11 = appraisal4.Item;
                                            ObjectPropertyList list7 = item11.PropertyList;
                                            string str12 = null;
                                            if (list7.Properties.Length > 0)
                                            {
                                                str12 = list7.Properties[0].Text;
                                            }
                                            if ((str12 == null) || (str12 == ""))
                                            {
                                                str12 = Localization.GetString(0xf9060 + (item11.ID & 0x3fff));
                                            }
                                            string str13 = string.Format("Looting {0}.", str12);
                                            if (list5.Count > 1)
                                            {
                                                str13 = string.Format("{0} There are {1} other valued item{2} to loot.", str13, list5.Count - 1, (list5.Count == 2) ? "" : "s");
                                            }
                                            AddTextMessage(str13, DefaultFont, Hues.Load(0x35));
                                            Mobile mobile10 = World.Player;
                                            if (mobile10 != null)
                                            {
                                                Network.Send(new PPickupItem(item11, item11.Amount));
                                                Network.Send(new PDropItem(item11.Serial, -1, -1, 0, mobile10.Serial));
                                            }
                                        }
                                        else
                                        {
                                            AddTextMessage("Nothing was found to loot.", DefaultFont, Hues.Load(0x22));
                                        }
                                        break;
                                    }
                                    case 0x15:
                                    {
                                        IPAddress address;
                                        int num16;
                                        if (strArray.Length != 5)
                                        {
                                            AddTextMessage("Usage: uoam <name> <password> <address> <port>");
                                            break;
                                        }
                                        string username = strArray[1];
                                        string password = strArray[2];
                                        try
                                        {
                                            address = IPAddress.Parse(strArray[3]);
                                            num16 = Convert.ToInt32(strArray[4]);
                                        }
                                        catch
                                        {
                                            AddTextMessage("Usage: uoam <name> <password> <address> <port>");
                                            break;
                                        }
                                        UOAM.Connect(username, password, address, num16);
                                        break;
                                    }
                                    case 0x16:
                                    {
                                        MapPackage cache = Map.GetCache();
                                        if (cache.cells != null)
                                        {
                                            Mobile mobile11 = World.Player;
                                            if (mobile11 != null)
                                            {
                                                int num17 = mobile11.X - cache.CellX;
                                                int num18 = mobile11.Y - cache.CellY;
                                                if (((num17 >= 0) && (num17 < cache.cells.GetLength(0))) && ((num18 >= 0) && (num18 < cache.cells.GetLength(1))))
                                                {
                                                    ArrayList list8 = cache.cells[num17, num18];
                                                    if (list8 != null)
                                                    {
                                                        if (list8.Count <= 0)
                                                        {
                                                            AddTextMessage("Nothing there.");
                                                            break;
                                                        }
                                                        ICell cell = (ICell) list8[list8.Count - 1];
                                                        if (!(cell is MobileCell))
                                                        {
                                                            if (cell is DynamicItem)
                                                            {
                                                                Target(((DynamicItem) cell).m_Item);
                                                            }
                                                            else if (cell is StaticItem)
                                                            {
                                                                Target(new StaticTarget(mobile11.X, mobile11.Y, ((StaticItem) cell).m_Z, ((StaticItem) cell).m_RealID, ((StaticItem) cell).m_RealID, ((StaticItem) cell).m_Hue));
                                                            }
                                                            else if (cell is LandTile)
                                                            {
                                                                Target(new LandTarget(mobile11.X, mobile11.Y, ((LandTile) cell).m_Z));
                                                            }
                                                            break;
                                                        }
                                                        Target(((MobileCell) cell).m_Mobile);
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    }
                                    case 0x17:
                                        m_AlternateFont = !m_AlternateFont;
                                        AddTextMessage(string.Format("Alternate font is now {0}.", m_AlternateFont ? "on" : "off"));
                                        break;

                                    case 0x18:
                                    {
                                        bool flag3 = !m_Sounds.Enabled;
                                        try
                                        {
                                            flag3 = strArray[1].ToLower() != "off";
                                        }
                                        catch
                                        {
                                            flag3 = !m_Sounds.Enabled;
                                        }
                                        m_Sounds.Enabled = flag3;
                                        AddTextMessage(string.Format("Sounds are now {0}.", flag3 ? "on" : "off"));
                                        break;
                                    }
                                    case 0x19:
                                    {
                                        bool flag4 = !World.CharData.QueueTargets;
                                        try
                                        {
                                            flag4 = strArray[1].ToLower() != "off";
                                        }
                                        catch
                                        {
                                            flag4 = !World.CharData.QueueTargets;
                                        }
                                        World.CharData.QueueTargets = flag4;
                                        AddTextMessage(string.Format("Target queueing is now {0}.", flag4 ? "on" : "off"));
                                        break;
                                    }
                                    case 0x1a:
                                    {
                                        IEnumerator enumerator = World.Mobiles.Values.GetEnumerator();
                                        int num19 = 0;
                                        while (enumerator.MoveNext())
                                        {
                                            Mobile current = (Mobile) enumerator.Current;
                                            if (((current.Visible && (current.Notoriety == Notoriety.Murderer)) && (!current.Human && !current.Ghost)) && World.InRange(current))
                                            {
                                                Timer timer = new Timer(new OnTick(Engine.DelayedPackets_OnTick), num19++ * 0x4e2, 1);
                                                timer.SetTag("Packets", new Packet[] { new PAttackRequest(current) });
                                                timer.Start(false);
                                            }
                                        }
                                        break;
                                    }
                                    case 0x1b:
                                        TargetHandler = new MoveTargetHandler();
                                        AddTextMessage("Target one of the items to move.");
                                        break;

                                    case 0x1c:
                                        TargetHandler = new OverrideHueTargetHandler(0x489, "Inflame request canceled.");
                                        AddTextMessage("Target an item.");
                                        break;

                                    case 0x1d:
                                        TargetHandler = new OverrideHueTargetHandler(-1, "Douse request canceled.");
                                        AddTextMessage("Target an item.");
                                        break;

                                    case 30:
                                    {
                                        NotoQueryType on = (NotoQueryType) ((1 + World.CharData.NotoQuery) % 3);
                                        try
                                        {
                                            switch (strArray[1].ToLower())
                                            {
                                                case "on":
                                                    on = NotoQueryType.On;
                                                    break;

                                                case "off":
                                                    on = NotoQueryType.Off;
                                                    break;

                                                case "smart":
                                                    on = NotoQueryType.Smart;
                                                    break;
                                            }
                                        }
                                        catch
                                        {
                                        }
                                        World.CharData.NotoQuery = on;
                                        AddTextMessage(string.Format("Notoriety query is now {0}.", on.ToString().ToLower()));
                                        break;
                                    }
                                    case 0x1f:
                                        Network.Send(new PWrestleDisarm());
                                        break;

                                    case 0x20:
                                        Network.Send(new PWrestleStun());
                                        break;

                                    case 0x21:
                                    {
                                        bool flag5 = !GFader.Fade;
                                        try
                                        {
                                            flag5 = strArray[1].ToLower() != "off";
                                        }
                                        catch
                                        {
                                            flag5 = !GFader.Fade;
                                        }
                                        GFader.Fade = flag5;
                                        AddTextMessage(string.Format("Interface fading is now {0}.", flag5 ? "on" : "off"));
                                        break;
                                    }
                                    case 0x22:
                                        TargetHandler = new DragToBagTargetHandler(false);
                                        AddTextMessage("Target one of the items to move.");
                                        break;

                                    case 0x23:
                                        TargetHandler = new DragToBagTargetHandler(true);
                                        AddTextMessage("Target one of the items to click and move.");
                                        break;

                                    case 0x24:
                                    {
                                        int xOffset = 0;
                                        int yOffset = 0;
                                        if ((strArray.Length == 3) || (strArray.Length == 1))
                                        {
                                            if (strArray.Length == 3)
                                            {
                                                try
                                                {
                                                    xOffset = Convert.ToInt32(strArray[1]);
                                                }
                                                catch
                                                {
                                                }
                                                try
                                                {
                                                    yOffset = Convert.ToInt32(strArray[2]);
                                                }
                                                catch
                                                {
                                                }
                                            }
                                            TargetHandler = new BringToTargetHandler(xOffset, yOffset);
                                            AddTextMessage("Target the destination item.");
                                        }
                                        else
                                        {
                                            AddTextMessage("Format: bringto [x y]");
                                        }
                                        break;
                                    }
                                    case 0x25:
                                        Ignore();
                                        break;

                                    case 0x26:
                                        TargetHandler = new RemoveTargetHandler();
                                        AddTextMessage("Remove what?");
                                        break;

                                    case 0x27:
                                        TargetHandler = new StackTargetHandler();
                                        AddTextMessage("Target the destination item.");
                                        break;

                                    case 40:
                                        TargetHandler = new RegDropTargetHandler();
                                        AddTextMessage("Target the destination container.");
                                        break;

                                    case 0x29:
                                        TargetHandler = new TurnTargetHandler();
                                        AddTextMessage("Turn to where?");
                                        break;

                                    case 0x2a:
                                    {
                                        bool flag6 = !NewConfig.SmoothWalk;
                                        try
                                        {
                                            flag6 = strArray[1].ToLower() != "off";
                                        }
                                        catch
                                        {
                                            flag6 = !NewConfig.SmoothWalk;
                                        }
                                        NewConfig.SmoothWalk = flag6;
                                        AddTextMessage(string.Format("Smooth walking is now {0}.", flag6 ? "on" : "off"));
                                        break;
                                    }
                                    case 0x2b:
                                    {
                                        int num22 = 100;
                                        try
                                        {
                                            num22 = Convert.ToInt32(cmd.Split(new char[] { ' ' })[1]);
                                        }
                                        catch
                                        {
                                            num22 = 100;
                                        }
                                        Timer timer2 = new Timer(new OnTick(Engine.TimeRefresh_OnTick), 1, 1);
                                        timer2.SetTag("Frames", num22);
                                        timer2.Start(false);
                                        break;
                                    }
                                    case 0x2c:
                                    {
                                        string str16 = null;
                                        try
                                        {
                                            str16 = strArray[1];
                                        }
                                        catch
                                        {
                                            AddTextMessage("Format: music <on | off | stop>");
                                            break;
                                        }
                                        switch (str16.ToLower())
                                        {
                                            case "on":
                                                NewConfig.PlayMusic = true;
                                                return;

                                            case "off":
                                                NewConfig.PlayMusic = false;
                                                Music.Stop();
                                                return;

                                            case "stop":
                                                Music.Stop();
                                                return;
                                        }
                                        AddTextMessage("Format: music <on | off | stop>");
                                        break;
                                    }
                                    case 0x2d:
                                        TargetHandler = new TraceTargetHandler();
                                        AddTextMessage("Target the dynamic item or mobile to trace.");
                                        break;

                                    case 0x2e:
                                        TargetHandler = new ExportTargetHandler();
                                        AddTextMessage("Target the first location of a bounding box.");
                                        break;

                                    case 0x2f:
                                    {
                                        if (!GMPrivs)
                                        {
                                            AddTextMessage("You do not have access to this command.");
                                            break;
                                        }
                                        string str17 = null;
                                        try
                                        {
                                            str17 = strArray[1];
                                        }
                                        catch
                                        {
                                            AddTextMessage("Format: ack <number>");
                                            break;
                                        }
                                        try
                                        {
                                            m_WalkAckSync = Convert.ToInt32(str17);
                                        }
                                        catch
                                        {
                                            AddTextMessage("Format: ack <number>");
                                            break;
                                        }
                                        AddTextMessage(string.Format("Maximum outstanding walk requests: {0}", m_WalkAckSync));
                                        break;
                                    }
                                    case 0x30:
                                    {
                                        if (!GMPrivs)
                                        {
                                            AddTextMessage("You do not have access to this command.");
                                            break;
                                        }
                                        string str18 = null;
                                        try
                                        {
                                            str18 = strArray[1];
                                        }
                                        catch
                                        {
                                            AddTextMessage("Format: runspeed <number>");
                                            break;
                                        }
                                        try
                                        {
                                            m_RunSpeed = ((float) Convert.ToInt32(str18)) / 1000f;
                                        }
                                        catch
                                        {
                                            AddTextMessage("Format: runspeed <number>");
                                            break;
                                        }
                                        AddTextMessage(string.Format("Run speed set to: {0} seconds", m_RunSpeed));
                                        break;
                                    }
                                    case 0x31:
                                    {
                                        if (!GMPrivs)
                                        {
                                            AddTextMessage("You do not have access to this command.");
                                            break;
                                        }
                                        string str19 = null;
                                        try
                                        {
                                            str19 = strArray[1];
                                        }
                                        catch
                                        {
                                            AddTextMessage("Format: walkspeed <number>");
                                            break;
                                        }
                                        try
                                        {
                                            m_WalkSpeed = ((float) Convert.ToInt32(str19)) / 1000f;
                                        }
                                        catch
                                        {
                                            AddTextMessage("Format: walkspeed <number>");
                                            break;
                                        }
                                        AddTextMessage(string.Format("Walk speed set to: {0} seconds", m_WalkSpeed));
                                        break;
                                    }
                                    case 50:
                                    {
                                        if (!GMPrivs)
                                        {
                                            AddTextMessage("You do not have access to this command.");
                                            break;
                                        }
                                        string str20 = null;
                                        try
                                        {
                                            str20 = strArray[1];
                                        }
                                        catch
                                        {
                                        }
                                        if (str20 == "on")
                                        {
                                            m_Weather = true;
                                            AddTextMessage("Weather turned on.");
                                        }
                                        else if (str20 == "off")
                                        {
                                            m_Weather = false;
                                            PacketHandlers.ClearWeather();
                                            AddTextMessage("Weather turned off.");
                                        }
                                        else
                                        {
                                            m_Weather = !m_Weather;
                                            if (!m_Weather)
                                            {
                                                PacketHandlers.ClearWeather();
                                            }
                                            AddTextMessage(string.Format("Weather turned {0}.", m_Weather ? "on" : "off"));
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        switch (Party.State)
                        {
                            case PartyState.Alone:
                                if (!cmd.ToLower().StartsWith("/add"))
                                {
                                    AddTextMessage(string.Format("Note to self: {0}", cmd.Substring(1)), DefaultFont, Hues.Load(0x3b2));
                                    break;
                                }
                                Network.Send(new PParty_AddMember());
                                break;

                            case PartyState.Joining:
                            {
                                string str2 = cmd.ToLower();
                                if (!str2.StartsWith("/accept"))
                                {
                                    if (str2.StartsWith("/decline"))
                                    {
                                        Network.Send(new PParty_Decline(Party.Leader));
                                    }
                                    else
                                    {
                                        AddTextMessage("Use '/accept' or '/decline'.", DefaultFont, Hues.Load(0x3b2));
                                    }
                                    break;
                                }
                                Network.Send(new PParty_Accept(Party.Leader));
                                break;
                            }
                            case PartyState.Joined:
                            {
                                string str3 = cmd.ToLower();
                                if (!Party.IsLeader || !str3.StartsWith("/add"))
                                {
                                    if (Party.IsLeader && str3.StartsWith("/rem"))
                                    {
                                        Network.Send(new PParty_RemoveMember());
                                    }
                                    else if (str3.StartsWith("/quit"))
                                    {
                                        Network.Send(new PParty_Quit());
                                    }
                                    else if (str3.StartsWith("/loot on"))
                                    {
                                        Network.Send(new PParty_SetCanLoot(true));
                                    }
                                    else if (str3.StartsWith("/loot off"))
                                    {
                                        Network.Send(new PParty_SetCanLoot(false));
                                    }
                                    else if (str3.StartsWith("/loot"))
                                    {
                                        AddTextMessage("Use '/loot on' or '/loot off'.");
                                    }
                                    else if (Party.Members.Length > 1)
                                    {
                                        if ((cmd.Length >= 2) && char.IsDigit(cmd, 1))
                                        {
                                            try
                                            {
                                                int index = Convert.ToInt32(cmd.Substring(1, 1)) - 1;
                                                if ((index >= 0) && (index < Party.Members.Length))
                                                {
                                                    if (index == Party.Index)
                                                    {
                                                        AddTextMessage(string.Format("Note to self: {0}", cmd.Substring(2)), DefaultFont, Hues.Load(0x3b2));
                                                    }
                                                    else
                                                    {
                                                        string str4;
                                                        Mobile mobile = World.Player;
                                                        if (((mobile == null) || ((str4 = mobile.Name) == null)) || ((str4 = str4.Trim()).Length <= 0))
                                                        {
                                                            str4 = "You";
                                                        }
                                                        AddTextMessage(string.Format("<{0}> {1}", str4, cmd.Substring(2)), DefaultFont, Hues.Load(World.CharData.WhisperHue));
                                                        Network.Send(new PParty_PrivateMessage(Party.Members[index], cmd.Substring(2)));
                                                    }
                                                    break;
                                                }
                                            }
                                            catch
                                            {
                                            }
                                        }
                                        Network.Send(new PParty_PublicMessage(cmd.Substring(1)));
                                    }
                                    else
                                    {
                                        AddTextMessage(string.Format("Note to self: {0}", cmd.Substring(1)), DefaultFont, Hues.Load(0x3b2));
                                    }
                                    break;
                                }
                                Network.Send(new PParty_AddMember());
                                break;
                            }
                        }
                    }
                }
            }
        }

        public static string ConvertFont(string input)
        {
            if (!m_AlternateFont || input.StartsWith("["))
            {
                return input;
            }
            if (m_ConvertTable == null)
            {
                m_ConvertTable = new char[0x10000];
                for (int j = 0; j < m_ConvertStructs.Length; j++)
                {
                    ConvertStruct struct2 = m_ConvertStructs[j];
                    for (int k = 0; k < struct2.m_Count; k++)
                    {
                        m_ConvertTable[k + struct2.m_From] = (char) (k + struct2.m_To);
                    }
                }
            }
            StringBuilder builder = new StringBuilder(input.Length);
            for (int i = 0; i < input.Length; i++)
            {
                char index = input[i];
                char ch2 = m_ConvertTable[index];
                if (ch2 != '\0')
                {
                    index = ch2;
                }
                builder.Append(index);
            }
            return builder.ToString();
        }

        public static void CountAmmo()
        {
            Mobile player = World.Player;
            if (player != null)
            {
                Item backpack = player.Backpack;
                if (backpack != null)
                {
                    int[] numArray = new int[] { 0xf3f, 0x1bfb };
                    string[] strArray = new string[] { "Arrows", "Bolts" };
                    for (int i = 0; i < numArray.Length; i++)
                    {
                        Item[] itemArray = backpack.FindItems(new ItemIDValidator(new int[] { numArray[i] }));
                        int num2 = 0;
                        for (int j = 0; j < itemArray.Length; j++)
                        {
                            num2 += (ushort) itemArray[j].Amount;
                        }
                        AddTextMessage(strArray[i] + ": " + num2, DefaultFont, (num2 < 5) ? Hues.Load(0x22) : DefaultHue);
                    }
                }
            }
        }

        public static int CountBits(int input)
        {
            int num = 0;
            for (int i = 0; i < 0x20; i++)
            {
                if ((input & (((int) 1) << i)) != 0)
                {
                    num++;
                }
            }
            return num;
        }

        public static void CountReagents()
        {
            Mobile player = World.Player;
            if (player != null)
            {
                Item backpack = player.Backpack;
                if (backpack != null)
                {
                    Reagent[] reagents = Spells.Reagents;
                    for (int i = 0; i < reagents.Length; i++)
                    {
                        Item[] itemArray = backpack.FindItems(new ItemIDValidator(new int[] { reagents[i].ItemID }));
                        int num2 = 0;
                        for (int j = 0; j < itemArray.Length; j++)
                        {
                            num2 += (ushort) itemArray[j].Amount;
                        }
                        AddTextMessage(reagents[i].Name + ": " + num2, DefaultFont, (num2 < 5) ? Hues.Load(0x22) : DefaultHue);
                    }
                }
            }
        }

        public static void DeathAnim_OnAnimationEnd(Animation a, Mobile m)
        {
            if (m != null)
            {
                Item item = World.FindItem(m.CorpseSerial);
                m.Visible = false;
                m.CorpseSerial = 0;
                m.Update();
                if (item != null)
                {
                    item.CorpseSerial = 0;
                    item.Direction = m.Direction;
                    item.Visible = true;
                    item.Update();
                }
            }
        }

        public static void DelayedPackets_OnTick(Timer t)
        {
            Packet[] tag = (Packet[]) t.GetTag("Packets");
            for (int i = 0; i < tag.Length; i++)
            {
                Network.Send(tag[i]);
            }
        }

        public static void DestroyDialogShowAcctLogin_OnClick(Gump Sender)
        {
            World.Clear();
            Gump g = null;
            if (Sender.HasTag("Dialog"))
            {
                g = (Gump) Sender.GetTag("Dialog");
            }
            else
            {
                g = Sender.Parent;
            }
            if (g != null)
            {
                Gumps.Destroy(g);
                Network.Disconnect();
                ShowAcctLogin();
            }
        }

        public static void DestroyGump_OnClick(Gump Sender)
        {
            if (Sender.HasTag("Gump"))
            {
                Gump tag = (Gump) Sender.GetTag("Gump");
                if (tag != null)
                {
                    Gumps.Destroy(tag);
                }
            }
        }

        public static void DoEvents()
        {
            if (GetQueueStatus(0xff) != 0)
            {
                Application.DoEvents();
            }
        }

        public static void DoubleClick(object sender, EventArgs e)
        {
            if ((m_EventOk && !m_Locked) && (!amMoving && m_ClickTimer.Enabled))
            {
                m_ClickTimer.Stop();
                if (!Gumps.DoubleClick(m_xMouse, m_yMouse))
                {
                    short tileX = 0;
                    short tileY = 0;
                    Renderer.ResetHitTest();
                    ICell cell = Renderer.FindTileFromXY(m_xMouse, m_yMouse, ref tileX, ref tileY);
                    if ((cell != null) && (m_TargetHandler == null))
                    {
                        if (cell.GetType() == typeof(DynamicItem))
                        {
                            Item item = ((DynamicItem) cell).m_Item;
                            if (item != null)
                            {
                                item.OnDoubleClick();
                            }
                        }
                        else if (cell.GetType() == typeof(MobileCell))
                        {
                            Mobile mobile = ((MobileCell) cell).m_Mobile;
                            if (mobile != null)
                            {
                                mobile.OnDoubleClick();
                            }
                        }
                    }
                }
            }
        }

        public static void DoWalk(Direction d, bool fromRenderer)
        {
            fromRenderer = false;
            if (!m_InResync)
            {
                Mobile player = World.Player;
                if (((player != null) && (player.Walking.Count <= 0)) && ((player.Body == 0x3db) || (m_WalkReq < (m_WalkAck + m_WalkAckSync))))
                {
                    if (m_Stealth && (m_StealthSteps == 0))
                    {
                        if (DateTime.Now >= (m_LastStealthUse + TimeSpan.FromSeconds(2.0)))
                        {
                            Skills[SkillName.Stealth].Use();
                            m_LastStealthUse = DateTime.Now;
                        }
                    }
                    else
                    {
                        GContextMenu.Close();
                        int x = player.X;
                        int y = player.Y;
                        int z = player.Z;
                        bool ghost = player.Ghost;
                        bool flag2 = !ghost && ((player.StamCur <= 0) && (player.StamMax > 0));
                        bool flag3 = !flag2 && ((player.StamCur == 1) && (player.StamMax > 0));
                        if (flag2 || flag3)
                        {
                            flag2 = flag3 = !UsePotion(PotionType.Red);
                        }
                        if (!flag2)
                        {
                            int num4;
                            int num5;
                            if (m_Stealth)
                            {
                                flag3 = true;
                            }
                            if (!Walking.Calculate(x, y, z, d, out num4, out num5))
                            {
                                if ((player.Direction & 7) != (num5 & 7))
                                {
                                    WalkAnimation animation = WalkAnimation.PoolInstance(player, player.X, player.Y, player.Z, num5);
                                    player.Walking.Enqueue(animation);
                                    player.IsMoving = true;
                                    animation.Start();
                                    SendMovementRequest(num5, player.X, player.Y, player.Z);
                                    if (player.Direction != num5)
                                    {
                                        EquipSort(player, num5);
                                        player.Direction = (byte) num5;
                                    }
                                }
                                else
                                {
                                    player.MovedTiles = 0;
                                    player.HorseFootsteps = 0;
                                    player.IsMoving = false;
                                }
                            }
                            else
                            {
                                num5 &= 7;
                                num5 |= (!flag3 && (m_dMouse > (GameWidth / 3))) ? 0x80 : 0;
                                if (!flag3 && World.CharData.AlwaysRun)
                                {
                                    num5 |= 0x80;
                                }
                                int num6 = x;
                                int num7 = y;
                                if (fromRenderer || ((num5 & 7) == (player.Direction & 7)))
                                {
                                    Walking.Offset(num5, ref num6, ref num7);
                                }
                                else
                                {
                                    num4 = player.Z;
                                }
                                if (((m_LeapFrog != null) && !m_LeapFrog.InSquareRange(new Point(num6, num7), 2)) && m_LeapFrog.InSquareRange(x, y, 2))
                                {
                                    if ((m_LastLeapfrogPickup + TimeSpan.FromSeconds(0.1)) < DateTime.Now)
                                    {
                                        m_LastLeapfrogPickup = DateTime.Now;
                                        Walking.Offset(num5, ref num6, ref num7);
                                        Network.Send(new PPickupItem(m_LeapFrog, m_LeapFrog.Amount));
                                        Network.Send(new PDropItem(m_LeapFrog.Serial, (short) num6, (short) num7, (sbyte) num4, -1));
                                    }
                                }
                                else
                                {
                                    WalkAnimation animation2 = WalkAnimation.PoolInstance(player, num6, num7, num4, num5);
                                    player.Walking.Enqueue(animation2);
                                    bool isMoving = player.IsMoving;
                                    player.IsMoving = true;
                                    animation2.Start();
                                    player.SetReal(num6, num7, num4);
                                    if (!isMoving && animation2.Advance)
                                    {
                                        World.Offset(animation2.xOffset, animation2.yOffset);
                                        Effects.Offset(animation2.xOffset, animation2.yOffset);
                                    }
                                    Redraw();
                                    if ((num5 & 7) != (player.Direction & 7))
                                    {
                                        SendMovementRequest(num5, player.X, player.Y, player.Z);
                                        if (!fromRenderer)
                                        {
                                            if (player.Direction != num5)
                                            {
                                                EquipSort(player, num5);
                                                player.Direction = (byte) num5;
                                            }
                                            return;
                                        }
                                    }
                                    if (!ghost && (player.Body != 0x3db))
                                    {
                                        MapPackage cache = Map.GetCache();
                                        ArrayList list = cache.cells[num6 - cache.CellX, num7 - cache.CellY];
                                        for (int i = 0; i < list.Count; i++)
                                        {
                                            ICell cell = (ICell) list[i];
                                            if ((cell is DynamicItem) && ((DynamicItem) cell).m_Item.IsDoor)
                                            {
                                                Network.Send(new POpenDoor());
                                                break;
                                            }
                                        }
                                    }
                                    if (m_Stealth)
                                    {
                                        m_StealthSteps--;
                                    }
                                    SendMovementRequest(num5, num6, num7, num4);
                                    if (player.Direction != num5)
                                    {
                                        EquipSort(player, num5);
                                        player.Direction = (byte) num5;
                                    }
                                    if (((Gumps.Drag == null) || (Gumps.Drag.GetType() != typeof(GDraggedItem))) && ((DateTime.Now - m_LastAction) > TimeSpan.FromSeconds(0.5)))
                                    {
                                        if (m_IVArray == null)
                                        {
                                            m_IVArray = new IItemValidator[2];
                                        }
                                        if (m_Regs == null)
                                        {
                                            m_Regs = new PlayerDistanceValidator(new PickupValidator(ReagentValidator.Validator), 2);
                                            m_IVArray[0] = m_Regs;
                                            m_IVArray[1] = new PlayerDistanceValidator(new PickupValidator(new ItemIDValidator(new int[] { 0x26ac })), 2);
                                        }
                                        IItemValidator check = new PlayerDistanceValidator(new ItemIDValidator(new int[] { 0x2006 }), 2);
                                        Item[] itemArray = World.FindItems(check);
                                        ArrayList list2 = new ArrayList();
                                        AosAppraiser instance = LootAppraiser.Instance;
                                        for (int j = 0; j < itemArray.Length; j++)
                                        {
                                            Item item2 = itemArray[j];
                                            if ((item2.LastTextHue != null) && ((item2.LastTextHue.HueID() & 0x7fff) == 0x59))
                                            {
                                                AosAppraiser.m_BlueCorpse = true;
                                            }
                                            else
                                            {
                                                AosAppraiser.m_BlueCorpse = false;
                                            }
                                            ArrayList items = item2.Items;
                                            for (int k = 0; k < items.Count; k++)
                                            {
                                                Item item = (Item) items[k];
                                                Appraisal appraisal = instance.Appraise(item);
                                                if ((appraisal != null) && !appraisal.IsWorthless)
                                                {
                                                    list2.Add(appraisal);
                                                }
                                            }
                                        }
                                        if (list2.Count > 0)
                                        {
                                            list2.Sort();
                                            Appraisal appraisal2 = (Appraisal) list2[0];
                                            Item item4 = appraisal2.Item;
                                            ObjectPropertyList propertyList = item4.PropertyList;
                                            string text = null;
                                            if (propertyList.Properties.Length > 0)
                                            {
                                                text = propertyList.Properties[0].Text;
                                            }
                                            if ((text == null) || (text == ""))
                                            {
                                                text = Localization.GetString(0xf9060 + (item4.ID & 0x3fff));
                                            }
                                            string str2 = string.Format("Looting {0}.", text);
                                            if (list2.Count > 1)
                                            {
                                                str2 = string.Format("{0} There are {1} other valued item{2} to loot.", str2, list2.Count - 1, (list2.Count == 2) ? "" : "s");
                                            }
                                            AddTextMessage(str2, DefaultFont, Hues.Load(0x35));
                                            Mobile mobile2 = World.Player;
                                            if (mobile2 != null)
                                            {
                                                Network.Send(new PPickupItem(item4, item4.Amount));
                                                Network.Send(new PDropItem(item4.Serial, -1, -1, 0, mobile2.Serial));
                                            }
                                        }
                                        Item item5 = World.CharData.AutoPickup ? World.FindItem(m_IVArray) : null;
                                        if (World.CharData.Archery && (item5 == null))
                                        {
                                            if (m_Ammo == null)
                                            {
                                                m_Ammo = new PlayerDistanceValidator(new PickupValidator(new ItemIDValidator(new int[] { 0xf3f, 0x1bfb })), 2);
                                            }
                                            item5 = World.FindItem(m_Ammo);
                                        }
                                        if (item5 != null)
                                        {
                                            Item regBag = null;
                                            if (ReagentValidator.Validator.IsValid(item5))
                                            {
                                                regBag = World.CharData.RegBag;
                                            }
                                            if (regBag == null)
                                            {
                                                regBag = player.Backpack;
                                            }
                                            if (regBag != null)
                                            {
                                                QueueAutoMove(item5, item5.Amount, -1, -1, 0, regBag.Serial);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void DrawNow()
        {
            DoEvents();
            Renderer.Draw();
            DoEvents();
        }

        private static void EmoteHue_OnClick(Gump g)
        {
            if (!m_DyeWindowOpen)
            {
                OpenDyeWindow(new OnClick(Engine.EmoteHue_OnHueSelect), g);
                m_DyeWindowOpen = true;
            }
        }

        private static void EmoteHue_OnHueSelect(Gump g)
        {
            World.CharData.EmoteHue = ((GHuePicker) g.GetTag("Hue Picker")).Hue;
            Renderer.SetText(m_Text);
            ((GTextButton) g.GetTag("Button")).DefaultHue = Hues.Load(((GHuePicker) g.GetTag("Hue Picker")).Hue);
            Gumps.Destroy(g.Parent);
            m_DyeWindowOpen = false;
        }

        public static string Encode(string Input)
        {
            return m_Encoder.Replace(Input.ToString(), new MatchEvaluator(Engine.CharEntity_Match));
        }

        public static void EquipSort(Mobile m, int newDir)
        {
            if ((m != null) && ((m.Direction & 7) != (newDir & 7)))
            {
                ArrayList equip = m.Equip;
                if (equip != null)
                {
                    equip.Sort(LayerComparer.FromDirection(newDir));
                }
            }
        }

        public static void Evict_OnTick(Timer t)
        {
            m_Device.EvictManagedResources();
        }

        public static void Exception_Unhandled(object Sender, UnhandledExceptionEventArgs e)
        {
            Debug.Trace("Unhandled exception");
            Debug.Trace("Object -> {0}", Sender);
            object exceptionObject = e.ExceptionObject;
            if (exceptionObject is Exception)
            {
                Debug.Error((Exception) exceptionObject);
            }
            else
            {
                Debug.Trace("Exception -> {0}", exceptionObject);
            }
        }

        private static void FindPlugins()
        {
            StreamReader reader = new StreamReader(FileManager.OpenBaseMUL("Data/Plugins/Plugins.def"));
            string message = "";
            Type[] types = null;
            int length = 0;
            string str2 = null;
            while ((message = reader.ReadLine()) != null)
            {
                message = message.Trim();
                if ((message.Length > 0) && !message.StartsWith("//"))
                {
                    if (message[0] == '#')
                    {
                        str2 = message.Substring(1);
                        types = Assembly.LoadFrom(FileManager.BasePath(string.Format("Data/Plugins/{0}", str2))).GetTypes();
                        length = types.Length;
                    }
                    else
                    {
                        for (int i = 0; i < length; i++)
                        {
                            Type type = types[i];
                            try
                            {
                                if ((type != null) && (type.Name == message))
                                {
                                    Plugin plugin = (Plugin) type.GetConstructor(Type.EmptyTypes).Invoke(null);
                                    if (plugin == null)
                                    {
                                        Debug.Trace(message);
                                        Debug.Trace("null");
                                    }
                                    else
                                    {
                                        Exposer e = new Exposer(plugin, str2);
                                        m_Plugins.Add(plugin);
                                        plugin.Run(e);
                                    }
                                }
                            }
                            catch (Exception exception)
                            {
                                Debug.Trace(message);
                                Debug.Error(exception);
                            }
                        }
                    }
                }
            }
            reader.Close();
        }

        public static int GetAnimDirection(byte dir)
        {
            return ((((dir & 7) + 5) % 8) | (dir & 0x80));
        }

        public static ArrayList GetDataStore()
        {
            if (m_DataStores.Count > 0)
            {
                return (ArrayList) m_DataStores.Dequeue();
            }
            return new ArrayList();
        }

        public static Direction GetDirection(int x, int y, ref int distance)
        {
            int num = (GameX + (GameWidth / 2)) - x;
            int num2 = (GameY + (GameHeight / 2)) - y;
            int num3 = Math.Abs(num);
            int num4 = Math.Abs(num2);
            int num5 = (int) ((((double) GameWidth) / ((double) GameHeight)) * num4);
            distance = (int) Math.Sqrt((double) ((num * num) + (num5 * num5)));
            if (((num4 >> 1) - num3) >= 0)
            {
                if (num2 > 0)
                {
                    return Direction.Up;
                }
                return Direction.Down;
            }
            if (((num3 >> 1) - num4) >= 0)
            {
                if (num > 0)
                {
                    return Direction.Left;
                }
                return Direction.Right;
            }
            if ((num >= 0) && (num2 >= 0))
            {
                return Direction.West;
            }
            if ((num >= 0) && (num2 < 0))
            {
                return Direction.South;
            }
            if ((num < 0) && (num2 < 0))
            {
                return Direction.East;
            }
            return Direction.North;
        }

        public static Direction GetDirection(int xFrom, int yFrom, int xTo, int yTo)
        {
            int num = xFrom - xTo;
            int num2 = yFrom - yTo;
            int num3 = (num - num2) * 0x2c;
            int num4 = (num + num2) * 0x2c;
            int num5 = Math.Abs(num3);
            int num6 = Math.Abs(num4);
            if (((num6 >> 1) - num5) >= 0)
            {
                return ((num4 > 0) ? Direction.Up : Direction.Down);
            }
            if (((num5 >> 1) - num6) >= 0)
            {
                return ((num3 > 0) ? Direction.Left : Direction.Right);
            }
            if ((num3 >= 0) && (num4 >= 0))
            {
                return Direction.West;
            }
            if ((num3 >= 0) && (num4 < 0))
            {
                return Direction.South;
            }
            if ((num3 < 0) && (num4 < 0))
            {
                return Direction.East;
            }
            return Direction.North;
        }

        public static Font GetFont(int id)
        {
            if ((id < 0) || (id >= 10))
            {
                id = 0;
            }
            Font font = m_Font[id];
            if (font == null)
            {
                font = m_Font[id] = new Font(id);
            }
            return font;
        }

        [DllImport("User32")]
        private static extern int GetQueueStatus(int flags);
        public static IHue GetRandomBlueHue()
        {
            return Hues.Load(RandomRange(0x515, 0x36));
        }

        public static int GetRandomHairHue()
        {
            return RandomRange(0x44e, 0x30);
        }

        public static int GetRandomHue()
        {
            return RandomRange(2, 0x3e8);
        }

        public static int GetRandomMetalHue()
        {
            return RandomRange(0x961, 30);
        }

        public static int GetRandomNeutralHue()
        {
            return RandomRange(0x709, 0x6c);
        }

        public static IHue GetRandomRedHue()
        {
            return Hues.Load(RandomRange(0x641, 0x36));
        }

        public static int GetRandomSkinHue()
        {
            return RandomRange(0x3ea, 0x38);
        }

        public static int GetRandomYellowHue()
        {
            return RandomRange(0x6a5, 0x36);
        }

        public static DateTime GetTimeStamp(string path)
        {
            return new FileInfo(path).LastWriteTime;
        }

        public static UnicodeFont GetUniFont(int id)
        {
            if ((id < 0) || (id >= 3))
            {
                id = 1;
            }
            UnicodeFont font = m_UniFont[id];
            if (font == null)
            {
                font = m_UniFont[id] = new UnicodeFont(id);
            }
            return font;
        }

        public static byte GetWalkDirection(Direction d)
        {
            return (byte) (((int) Direction.West) & (((int) d) - 1));
        }

        public static int GrayScale(int Color)
        {
            float num = (Color >> 10) & 0x1f;
            float num2 = (Color >> 5) & 0x1f;
            float num3 = Color & 0x1f;
            num *= 8.225806f;
            num2 *= 8.225806f;
            num3 *= 8.225806f;
            float num4 = ((num * 0.299f) + (num2 * 0.587f)) + (num3 * 0.114f);
            num4 *= 0.1215686f;
            int num5 = (int) num4;
            if (num5 < 0)
            {
                return 0;
            }
            if (num5 > 0x1f)
            {
                num5 = 0x1f;
            }
            return num5;
        }

        public static void Help_OnClick(Gump Sender)
        {
            OpenHelp();
        }

        public static void HuePicker_OnHueSelect(int Hue, Gump Sender)
        {
            if ((Sender.HasTag("Dialog") && Sender.HasTag("Item Art")) && Sender.HasTag("ItemID"))
            {
                GItemArt art;
                Gump tag = (Gump) Sender.GetTag("Dialog");
                Gump g = (Gump) Sender.GetTag("Item Art");
                Gumps.Destroy(g);
                art = new GItemArt(0xb7, 3, (int) Sender.GetTag("ItemID"), Hues.GetItemHue((int) Sender.GetTag("ItemID"), Hue)) {
                    X = art.X + (((0x3a - (art.Image.xMax - art.Image.xMin)) / 2) - art.Image.xMin),
                    Y = art.Y + (((0x52 - (art.Image.yMax - art.Image.yMin)) / 2) - art.Image.yMin)
                };
                tag.Children.Add(art);
                Sender.SetTag("Item Art", art);
            }
        }

        public static void HuePickerOk_OnClick(Gump Sender)
        {
            if ((Sender.HasTag("Dialog") && Sender.HasTag("Hue Picker")) && (Sender.HasTag("Serial") && Sender.HasTag("Relay")))
            {
                Gump tag = (Gump) Sender.GetTag("Dialog");
                if (tag != null)
                {
                    GHuePicker picker = (GHuePicker) Sender.GetTag("Hue Picker");
                    if (picker == null)
                    {
                        Gumps.Destroy(tag);
                    }
                    else
                    {
                        int serial = (int) Sender.GetTag("Serial");
                        short relay = (short) Sender.GetTag("Relay");
                        Network.Send(new PSelectHueResponse(serial, relay, (short) picker.Hue));
                        Gumps.Destroy(tag);
                    }
                }
            }
        }

        public static void HuePickerPicker_OnClick(Gump Sender)
        {
            if (Sender.HasTag("Hue Picker") && Sender.HasTag("Brightness Bar"))
            {
                GHuePicker tag = (GHuePicker) Sender.GetTag("Hue Picker");
                GBrightnessBar bar = (GBrightnessBar) Sender.GetTag("Brightness Bar");
                if ((tag != null) && (bar != null))
                {
                    TargetHandler = new HuePickerTargetHandler(tag, bar);
                }
            }
        }

        public static void HuePickerSlider_OnValueChange(double Value, double Old, Gump Sender)
        {
            if (Sender.HasTag("Hue Picker"))
            {
                GHuePicker tag = (GHuePicker) Sender.GetTag("Hue Picker");
                if (tag != null)
                {
                    tag.Brightness = (int) Value;
                }
            }
        }

        public static void Ignore()
        {
            TargetHandler = new IgnoreTargetHandler();
            AddTextMessage("Target a mobile.");
        }

        public static unsafe void InitDX()
        {
            m_Fullscreen = NewConfig.FullScreen;
            PresentParameters parameters = m_PresentParams = new PresentParameters();
            parameters.set_SwapEffect(1);
            parameters.set_EnableAutoDepthStencil(true);
            parameters.set_AutoDepthStencilFormat(80);
            parameters.set_PresentationInterval(-2147483648);
            parameters.set_DeviceWindow(m_Display);
            if (m_Fullscreen)
            {
                parameters.set_Windowed(false);
                ArrayList list = new ArrayList();
                AdapterListEnumerator enumerator = Manager.get_Adapters();
                if ((enumerator != null) && (enumerator.get_Count() > 0))
                {
                    using (IEnumerator enumerator2 = enumerator.get_Item(0).get_SupportedDisplayModes().GetEnumerator())
                    {
                        while (enumerator2.MoveNext())
                        {
                            DisplayMode mode = *((DisplayMode*) enumerator2.Current);
                            list.Add(mode);
                        }
                    }
                    list.Sort(new DisplayModeComparer(ScreenWidth, ScreenHeight, 0x19));
                }
                if (list.Count == 0)
                {
                    throw new Exception("No display modes found");
                }
                DisplayMode mode2 = *((DisplayMode*) list[0]);
                Debug.Trace("Display Mode: {0}x{1}, {2}, {3}hz", new object[] { mode2.get_Width(), mode2.get_Height(), mode2.get_Format(), mode2.get_RefreshRate() });
                parameters.set_BackBufferCount(1);
                parameters.set_SwapEffect(2);
                parameters.set_BackBufferFormat(mode2.get_Format());
                parameters.set_BackBufferWidth(mode2.get_Width());
                parameters.set_BackBufferHeight(mode2.get_Height());
            }
            else
            {
                parameters.set_Windowed(true);
            }
            try
            {
                m_Device = new Device(0, 1, m_Display, 0x40, parameters);
            }
            catch
            {
                m_Device = new Device(0, 1, m_Display, 0x20, parameters);
            }
            Renderer.m_Version++;
            m_Device.add_DeviceCreated(new EventHandler(Engine.OnDeviceCreated));
            m_Device.add_DeviceReset(new EventHandler(Engine.OnDeviceReset));
            m_Device.add_DeviceLost(new EventHandler(Engine.OnDeviceLost));
            m_Device.add_DeviceResizing(new CancelEventHandler(Engine.OnDeviceResizing));
            OnDeviceReset(m_Device, null);
            Debug.Trace("Fullscreen = {0}", m_Fullscreen);
            m_rRender = new Rectangle(0, 0, ScreenWidth, ScreenHeight);
            if (!Texture.CanSysMem && !Texture.CanVidMem)
            {
                throw new Exception("Device does not support textures in video memory nor system memory.");
            }
            Debug.EndTry();
        }

        public static bool IsMoving()
        {
            Mobile player = World.Player;
            return ((player != null) && (player.Walking.Count > 0));
        }

        public static void Journal_OnClick(Gump Sender)
        {
            OpenJournal();
        }

        public static unsafe void KeyDown(object sender, KeyEventArgs e)
        {
            if (m_EventOk)
            {
                if (m_Ingame && Macros.Start(e.KeyCode))
                {
                    e.Handled = true;
                }
                else
                {
                    Keys keyCode = e.KeyCode;
                    bool shift = e.Shift;
                    bool control = e.Control;
                    bool alt = e.Alt;
                    if ((alt && !control) && (!shift && (keyCode == Keys.Enter)))
                    {
                        int w = -1;
                        int h = -1;
                        string[] strArray = NewConfig.ScreenSize.Split(new char[] { 'x' });
                        if (strArray.Length == 2)
                        {
                            w = h = 0;
                            try
                            {
                                w = int.Parse(strArray[0]);
                                h = int.Parse(strArray[1]);
                            }
                            catch
                            {
                            }
                        }
                        if (w < 320)
                        {
                            w = 320;
                        }
                        if (h < 240)
                        {
                            h = 240;
                        }
                        PresentParameters presentParams = m_PresentParams;
                        if (m_Fullscreen)
                        {
                            presentParams.set_Windowed(true);
                            presentParams.set_BackBufferCount(0);
                            presentParams.set_SwapEffect(1);
                        }
                        else
                        {
                            presentParams.set_Windowed(false);
                            ArrayList list = new ArrayList();
                            if (Manager.get_Adapters().get_Count() > 0)
                            {
                                using (IEnumerator enumerator = Manager.get_Adapters().get_Item(0).get_SupportedDisplayModes().GetEnumerator())
                                {
                                    while (enumerator.MoveNext())
                                    {
                                        DisplayMode mode = *((DisplayMode*) enumerator.Current);
                                        list.Add(mode);
                                    }
                                }
                                list.Sort(new DisplayModeComparer(w, h, 0x19));
                            }
                            if (list.Count == 0)
                            {
                                return;
                            }
                            DisplayMode mode2 = *((DisplayMode*) list[0]);
                            presentParams.set_BackBufferCount(1);
                            presentParams.set_SwapEffect(2);
                            presentParams.set_BackBufferWidth(mode2.get_Width());
                            presentParams.set_BackBufferHeight(mode2.get_Height());
                            presentParams.set_BackBufferFormat(mode2.get_Format());
                            presentParams.set_PresentationInterval(-2147483648);
                        }
                        m_EventOk = false;
                        m_Fullscreen = !m_Fullscreen;
                        m_Display.BringToFront();
                        m_Device.Reset(presentParams);
                        m_EventOk = true;
                        if (m_Fullscreen)
                        {
                            m_Display.ClientSize = new Size(w, h);
                            ScreenWidth = w;
                            ScreenHeight = h;
                        }
                        else
                        {
                            m_Display.FormBorderStyle = FormBorderStyle.Sizable;
                            m_Display.WindowState = FormWindowState.Normal;
                            m_Display.ClientSize = new Size(w, h);
                        }
                        e.Handled = true;
                        m_Display.TopMost = false;
                    }
                    else if (((m_Ingame && (World.Player != null)) && (!alt && !control)) && ((!shift && (keyCode == Keys.Tab)) && (Gumps.TextFocus == null)))
                    {
                        e.Handled = Network.Send(new PSetWarMode(true, 0x20, 0));
                    }
                    else
                    {
                        int count = m_Plugins.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (((Plugin) m_Plugins[i]).OnKeyDown(e))
                            {
                                e.Handled = true;
                                return;
                            }
                        }
                        e.Handled = false;
                    }
                }
            }
        }

        public static void KeyUp(KeyEventArgs e)
        {
            if (((m_EventOk && m_Ingame) && ((World.Player != null) && !e.Alt)) && ((!e.Control && !e.Shift) && (e.KeyCode == Keys.Tab)))
            {
                e.Handled = Network.Send(new PSetWarMode(false, 0x20, 0));
            }
        }

        public static void ListView_OnValueChange(double Value, double Old, Gump Sender)
        {
            if (Sender.HasTag("ListBox"))
            {
                GListBox tag = (GListBox) Sender.GetTag("ListBox");
                if (tag != null)
                {
                    tag.StartIndex = (int) Value;
                }
            }
        }

        public static Texture LoadImageAsAlpha(string FileName)
        {
            try
            {
                string path = FileManager.BasePath(string.Format("Data/Images/{0}", FileName));
                if (File.Exists(path))
                {
                    Bitmap bmp = new Bitmap(path);
                    bmp.MakeTransparent(Color.Black);
                    for (int i = 0; i < bmp.Height; i++)
                    {
                        for (int j = 0; j < bmp.Width; j++)
                        {
                            Color pixel = bmp.GetPixel(j, i);
                            bmp.SetPixel(j, i, Color.FromArgb(pixel.B, 0xff, 0xff, 0xff));
                        }
                    }
                    return new Texture(bmp);
                }
            }
            catch
            {
            }
            return Texture.Empty;
        }

        public static void LoadParticles()
        {
            m_Rain = LoadImageAsAlpha("Rain.bmp");
            m_Halo = LoadImageAsAlpha("Halo_2.bmp");
            m_Friend = LoadImageAsAlpha("Friend.bmp");
            m_TargetImage = LoadImageAsAlpha("Target.bmp");
            m_TargetCursorImage = LoadImageAsAlpha("TargetCursor.bmp");
            m_FormX = LoadImageAsAlpha("Form_X.bmp");
            m_Slider = LoadImageAsAlpha("Slider.bmp");
            m_SkillUp = LoadImageAsAlpha("Skill_Up.bmp");
            m_SkillDown = LoadImageAsAlpha("Skill_Down.bmp");
            m_SkillLocked = LoadImageAsAlpha("Skill_Locked.bmp");
            m_Snow = new Texture[12];
            for (int i = 0; i < 12; i++)
            {
                m_Snow[i] = LoadImageAsAlpha(string.Format("Snow_{0}.bmp", i + 1));
            }
            m_Edge = new Texture[8];
            for (int j = 0; j < 8; j++)
            {
                m_Edge[j] = LoadImageAsAlpha(string.Format("Edge_{0}.bmp", j + 1));
            }
            m_WinScrolls = new Texture[] { LoadImageAsAlpha("WinScroll_Up.bmp"), LoadImageAsAlpha("WinScroll_Down.bmp"), LoadImageAsAlpha("WinScroll_Left.bmp"), LoadImageAsAlpha("WinScroll_Right.bmp") };
        }

        public static void LogOut_OnClick(Gump Sender)
        {
            Quit();
        }

        public static void LogOut_YesNo(Gump sender, bool response)
        {
            if (response)
            {
                m_Ingame = false;
                Network.Disconnect();
                ShowAcctLogin();
            }
        }

        [DllImport("Kernel32", EntryPoint="_lread")]
        private static extern unsafe int lread(IntPtr hFile, void* lpBuffer, int wBytes);
        [DllImport("Kernel32", EntryPoint="_lwrite")]
        private static extern unsafe int lwrite(IntPtr hFile, void* lpBuffer, int wBytes);
        private static void MacroEditor_OnClick(Gump g)
        {
            Gumps.Destroy(g.Parent);
            GMacroEditorForm.Open();
        }

        [STAThread]
        public static void Main(string[] Args)
        {
            int num3;
            m_IniPath = Path.Combine(Application.StartupPath, "Data/Config/Client.ini");
            ParseArgs(Args);
            m_FileManager = new Client.FileManager();
            if (m_FileManager.Error)
            {
                m_FileManager = null;
                GC.Collect();
            }
            else
            {
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Engine.Exception_Unhandled);
                long frequency = 0L;
                QueryPerformanceFrequency(ref frequency);
                double num2 = frequency;
                m_QPF = num2 / 1000.0;
                WantDirectory("Data/Config/");
                Debug.Trace("Entered Main()");
                Debug.Block("Environment");
                Debug.Trace("Operating System = '{0}'", Environment.OSVersion);
                Debug.Trace(".NET Framework   = '{0}'", Environment.Version);
                Debug.Trace("Base Directory   = '{0}'", m_FileManager.BasePath(""));
                Debug.Trace("Data Directory   = '{0}'", m_FileManager.ResolveMUL(""));
                Debug.EndBlock();
                Debug.Try("Setting up interop communications");
                Interop.Comm = new ExposerInterop();
                Debug.EndTry();
                Debug.Try("Initializing Strings");
                Strings.Initialize();
                Debug.EndTry();
                Benchmark benchmark = new Benchmark(6);
                Debug.Trace("Benchmarks: {0} resolution", "low");
                benchmark = null;
                m_Timers = new ArrayList();
                m_Plugins = new ArrayList();
                m_WalkTimeout = new ArrayList();
                m_Journal = new ArrayList();
                m_Doors = new ArrayList();
                m_Pings = new Queue();
                m_LoadQueue = new Queue();
                m_MapLoadQueue = new Queue();
                WantDirectory("Data/Logs/");
                Debug.Block("Main()");
                if (m_OverrideServHost != null)
                {
                    NewConfig.ServerHost = m_OverrideServHost;
                }
                if (m_OverrideServPort != -1)
                {
                    NewConfig.ServerPort = m_OverrideServPort;
                }
                switch (NewConfig.GameSize.ToLower().Trim())
                {
                    case "320x240":
                        GameWidth = 320;
                        GameHeight = 240;
                        num3 = 3;
                        goto Label_02E0;

                    case "640x480":
                        GameWidth = 640;
                        GameHeight = 480;
                        num3 = 5;
                        goto Label_02E0;

                    case "800x600":
                        GameWidth = 800;
                        GameHeight = 600;
                        num3 = 7;
                        goto Label_02E0;

                    case "1024x768":
                        GameWidth = 0x400;
                        GameHeight = 0x300;
                        num3 = 7;
                        goto Label_02E0;

                    case "1280x1024":
                        GameWidth = 0x500;
                        GameHeight = 0x400;
                        num3 = 9;
                        goto Label_02E0;

                    case "1600x1200":
                        GameWidth = 0x640;
                        GameHeight = 0x4b0;
                        num3 = 11;
                        goto Label_02E0;
                }
                MessageBox.Show(string.Format("A invalid game size of '{0}' was specified in Client.cfg. Valid sizes are: '320x240' '640x480' '800x600' '1024x768' '1280x1024' '1600x1200'", NewConfig.GameSize));
            }
            return;
        Label_02E0:;
            string[] strArray = NewConfig.ScreenSize.ToLower().Trim().Split(new char[] { 'x' });
            int num4 = -1;
            int num5 = -1;
            if (strArray.Length == 2)
            {
                num4 = num5 = 0;
                try
                {
                    num4 = int.Parse(strArray[0]);
                    num5 = int.Parse(strArray[1]);
                }
                catch
                {
                }
            }
            if ((num4 >= 320) && (num5 >= 240))
            {
                ScreenWidth = num4;
                ScreenHeight = num5;
            }
            else
            {
                MessageBox.Show(string.Format("A invalid screen size of '{0}' was specified in Client.cfg. Make sure the value is formatted correctly ('<Width>x<Height>'), and that it is 320x240 or higher.", NewConfig.ScreenSize));
            }
            GameX = (ScreenWidth - GameWidth) / 2;
            GameY = (ScreenHeight - GameHeight) / 2;
            Renderer.blockWidth = num3;
            Renderer.blockHeight = num3;
            Renderer.cellWidth = num3 << 3;
            Renderer.cellHeight = num3 << 3;
            m_ClickTimer = new Timer(new OnTick(Engine.ClickTimer_OnTick), SystemInformation.DoubleClickTime);
            Debug.Try("Initializing Display");
            m_Display = new Display();
            if (NewConfig.FullScreen)
            {
                m_Display.FormBorderStyle = FormBorderStyle.None;
                m_Display.WindowState = FormWindowState.Maximized;
            }
            m_Display.ClientSize = new Size(ScreenWidth, ScreenHeight);
            m_Display.KeyPreview = true;
            m_Display.Show();
            Debug.EndTry();
            Application.DoEvents();
            Debug.Block("Initializing DirectX");
            InitDX();
            Debug.EndBlock();
            m_Loading = true;
            m_Ingame = false;
            DrawNow();
            Benchmark benchmark2 = new Benchmark(7);
            benchmark2.Start();
            Debug.TimeBlock("Initializing Animations");
            m_Animations = new Animations();
            Debug.EndBlock();
            m_Font = new Font[10];
            m_UniFont = new UnicodeFont[3];
            Debug.TimeBlock("Initializing Gumps");
            m_Gumps = new Gumps();
            Debug.EndBlock();
            m_DefaultFont = GetUniFont(3);
            m_DefaultHue = Hues.Load(0x3b2);
            Renderer.SetText("");
            Debug.TimeBlock("Initializing Plugins");
            FindPlugins();
            Debug.EndBlock();
            Macros.Load();
            LoadParticles();
            Renderer.SetAlphaEnable(true);
            Renderer.SetFilterEnable(false);
            Renderer.AlphaTestEnable = true;
            Renderer.SetTexture(m_Halo);
            try
            {
                m_Device.ValidateDevice();
            }
            catch (Exception exception)
            {
                m_Halo.Dispose();
                m_Halo = Hues.Default.GetGump(0x71);
                m_Rain.Dispose();
                m_Rain = Texture.Empty;
                m_SkillUp.Dispose();
                m_SkillUp = Hues.Default.GetGump(0x983);
                m_SkillDown.Dispose();
                m_SkillDown = Hues.Default.GetGump(0x985);
                m_SkillLocked.Dispose();
                m_SkillLocked = Hues.Default.GetGump(0x82c);
                m_Slider.Dispose();
                m_Slider = Hues.Default.GetGump(0x845);
                for (int i = 0; i < m_Snow.Length; i++)
                {
                    m_Snow[i].Dispose();
                    m_Snow[i] = Texture.Empty;
                }
                for (int j = 0; j < m_Edge.Length; j++)
                {
                    m_Edge[j].Dispose();
                    m_Edge[j] = Texture.Empty;
                }
                Debug.Trace("ValidateDevice() failed on 32-bit textures");
                Debug.Error(exception);
            }
            Renderer.SetTexture(null);
            Renderer.SetAlphaEnable(false);
            m_Effects = new Client.Effects();
            m_Loading = false;
            Point point = m_Display.PointToClient(Cursor.Position);
            m_EventOk = true;
            MouseMove(m_Display, new MouseEventArgs(Control.MouseButtons, 0, point.X, point.Y, 0));
            Network.CheckCache();
            ShowAcctLogin();
            MouseMoveQueue();
            m_EventOk = false;
            DrawNow();
            benchmark2.StopNoLog();
            Debug.Trace("Total -> {0}", Benchmark.Format(benchmark2.Elapsed));
            m_MoveDelay = new TimeDelay(0f);
            m_LastOverCheck = new TimeDelay(0.1f);
            m_NewFrame = new TimeDelay(0.05f);
            m_SleepMode = new TimeDelay(7.5f);
            m_EventOk = true;
            PlayRandomMidi();
            bool flag = false;
            new Timer(new OnTick(Engine.Evict_OnTick), 0x9c4).Start(false);
            Animations.StartLoading();
            Unlock();
            DateTime now = DateTime.Now;
            int num10 = Ticks + 0x1388;
            bool extendProtocol = NewConfig.ExtendProtocol;
            while (!exiting)
            {
                m_SetTicks = false;
                int ticks = Ticks;
                Macros.Slice();
                if (Gumps.Invalidated)
                {
                    if (m_LastMouseArgs != null)
                    {
                        MouseMove(m_Display, m_LastMouseArgs);
                    }
                    Gumps.Invalidated = false;
                }
                if (m_MouseMoved)
                {
                    MouseMoveQueue();
                }
                if (m_NewFrame.ElapsedReset())
                {
                    Renderer.m_Frames++;
                    m_Redraw = false;
                    Renderer.Draw();
                }
                else if ((m_Redraw || m_PumpFPS) || (m_Quake || (amMoving && IsMoving())))
                {
                    m_Redraw = false;
                    Renderer.Draw();
                }
                if ((extendProtocol && m_Ingame) && ((Party.State == PartyState.Joined) && (DateTime.Now >= now)))
                {
                    now = DateTime.Now + TimeSpan.FromSeconds(0.5);
                    for (int k = 0; k < Party.Members.Length; k++)
                    {
                        Mobile mobile = Party.Members[k];
                        if (((mobile != null) && !mobile.Player) && !mobile.Visible)
                        {
                            Network.Send(new PPE_QueryPartyLocs());
                            break;
                        }
                    }
                }
                Thread.Sleep(1);
                if (GetQueueStatus(0xff) != 0)
                {
                    Application.DoEvents();
                }
                UOAM.Slice();
                if (!Network.Slice())
                {
                    flag = true;
                    break;
                }
                Network.Flush();
                TickTimers();
                if (amMoving && m_Ingame)
                {
                    DoWalk(movingDir, false);
                }
                if (m_LoadQueue.Count > 0)
                {
                    for (int m = 0; (m_LoadQueue.Count > 0) && (m < 6); m++)
                    {
                        ((ILoader) m_LoadQueue.Dequeue()).Load();
                    }
                }
                if (m_MapLoadQueue.Count > 0)
                {
                    Preload((Worker) m_MapLoadQueue.Dequeue());
                }
            }
            NewConfig.Save();
            SaveJournal();
            Thread.Sleep(5);
            if ((m_Display != null) && !m_Display.IsDisposed)
            {
                m_Display.Hide();
            }
            Thread.Sleep(5);
            Application.DoEvents();
            Thread.Sleep(5);
            Application.DoEvents();
            m_Animations.Dispose();
            if (m_ItemArt != null)
            {
                m_ItemArt.Dispose();
            }
            if (m_LandArt != null)
            {
                m_LandArt.Dispose();
            }
            if (m_TextureArt != null)
            {
                m_TextureArt.Dispose();
            }
            m_Gumps.Dispose();
            if (m_Sounds != null)
            {
                m_Sounds.Dispose();
            }
            if (m_Multis != null)
            {
                m_Multis.Dispose();
            }
            m_FileManager.Dispose();
            Cursor.Dispose();
            Music.Dispose();
            Hues.Dispose();
            GRadar.Dispose();
            if (m_Plugins != null)
            {
                m_Plugins.Clear();
                m_Plugins = null;
            }
            if (m_Rain != null)
            {
                m_Rain.Dispose();
                m_Rain = null;
            }
            if (m_Slider != null)
            {
                m_Slider.Dispose();
                m_Slider = null;
            }
            if (m_SkillUp != null)
            {
                m_SkillUp.Dispose();
                m_SkillUp = null;
            }
            if (m_SkillDown != null)
            {
                m_SkillDown.Dispose();
                m_SkillDown = null;
            }
            if (m_SkillLocked != null)
            {
                m_SkillLocked.Dispose();
                m_SkillLocked = null;
            }
            if (m_Snow != null)
            {
                for (int n = 0; n < 12; n++)
                {
                    if (m_Snow[n] != null)
                    {
                        m_Snow[n].Dispose();
                        m_Snow[n] = null;
                    }
                }
                m_Snow = null;
            }
            if (m_Edge != null)
            {
                for (int num13 = 0; num13 < 8; num13++)
                {
                    if (m_Edge[num13] != null)
                    {
                        m_Edge[num13].Dispose();
                        m_Edge[num13] = null;
                    }
                }
                m_Edge = null;
            }
            if (m_WinScrolls != null)
            {
                for (int num14 = 0; num14 < m_WinScrolls.Length; num14++)
                {
                    if (m_WinScrolls[num14] != null)
                    {
                        m_WinScrolls[num14].Dispose();
                        m_WinScrolls[num14] = null;
                    }
                }
                m_WinScrolls = null;
            }
            if (m_Halo != null)
            {
                m_Halo.Dispose();
                m_Halo = null;
            }
            if (m_Friend != null)
            {
                m_Friend.Dispose();
                m_Friend = null;
            }
            if (m_FormX != null)
            {
                m_FormX.Dispose();
                m_FormX = null;
            }
            if (m_TargetImage != null)
            {
                m_TargetImage.Dispose();
                m_TargetImage = null;
            }
            if (m_TargetCursorImage != null)
            {
                m_TargetCursorImage.Dispose();
                m_TargetCursorImage = null;
            }
            if (m_Font != null)
            {
                for (int num15 = 0; num15 < 10; num15++)
                {
                    if (m_Font[num15] != null)
                    {
                        m_Font[num15].Dispose();
                        m_Font[num15] = null;
                    }
                }
                m_Font = null;
            }
            if (m_UniFont != null)
            {
                int length = m_UniFont.Length;
                for (int num17 = 0; num17 < length; num17++)
                {
                    if (m_UniFont[num17] != null)
                    {
                        m_UniFont[num17].Dispose();
                        m_UniFont[num17] = null;
                    }
                }
                m_UniFont = null;
            }
            if (m_MidiTable != null)
            {
                m_MidiTable.Dispose();
                m_MidiTable = null;
            }
            if (m_ContainerBoundsTable != null)
            {
                m_ContainerBoundsTable.Dispose();
                m_ContainerBoundsTable = null;
            }
            Texture.DisposeAll();
            Debug.EndBlock();
            if (flag)
            {
                Debug.Trace("Network error caused termination");
            }
            Network.DumpBuffer();
            Network.Close();
            Debug.Dispose();
            Strings.Dispose();
            m_LoadQueue = null;
            m_MapLoadQueue = null;
            m_DefaultFont = null;
            m_DefaultHue = null;
            m_Display = null;
            m_Encoder = null;
            m_Effects = null;
            m_Skills = null;
            m_Features = null;
            m_Animations = null;
            m_LandArt = null;
            m_TextureArt = null;
            m_ItemArt = null;
            m_Gumps = null;
            m_Sounds = null;
            m_Multis = null;
            m_FileManager = null;
            m_Display = null;
            m_Font = null;
            m_UniFont = null;
            m_Device = null;
            m_CharacterNames = null;
            m_MoveDelay = null;
            m_Text = null;
            m_Font = null;
            m_UniFont = null;
            m_NewFrame = null;
            m_SleepMode = null;
            m_Timers = null;
            m_Plugins = null;
            m_SkillsGump = null;
            m_JournalGump = null;
            m_Journal = null;
            m_FileManager = null;
            m_Encoder = null;
            m_DefaultFont = null;
            m_DefaultHue = null;
            m_LastTarget = null;
            m_Random = null;
            m_WalkStack = null;
            m_Prompt = null;
            m_IniPath = null;
            m_Pings = null;
            m_PingTimer = null;
            m_MultiList = null;
            m_WalkTimeout = null;
            m_Servers = null;
            m_AllNames = null;
            m_Doors = null;
            m_LastOverCheck = null;
            m_LastMouseArgs = null;
            m_LastAttacker = null;
            Renderer.m_VertexPool = null;
            try
            {
                Process currentProcess = Process.GetCurrentProcess();
                currentProcess.Kill();
                currentProcess.WaitForExit();
            }
            catch
            {
            }
        }

        public static void MakeDirectory(string Target)
        {
            Directory.CreateDirectory(FileManager.BasePath(Target));
        }

        public static string MakeProperCase(string text)
        {
            StringBuilder builder = new StringBuilder(text);
            for (int i = 0; i < builder.Length; i++)
            {
                if ((i == 0) || (builder[i - 1] == ' '))
                {
                    builder[i] = char.ToUpper(builder[i]);
                }
            }
            return builder.ToString();
        }

        public static void MouseDown(object sender, MouseEventArgs e)
        {
            if (m_EventOk && (e != null))
            {
                m_LastMouseArgs = e;
                m_xMouse = e.X;
                m_yMouse = e.Y;
                if (((!m_Ingame || (e.Button != MouseButtons.Middle)) || !Macros.Start(0x11002)) && !Gumps.MouseDown(e.X, e.Y, e.Button))
                {
                    if (!m_Locked && ((e.Button & MouseButtons.Right) == MouseButtons.Right))
                    {
                        movingDir = GetDirection(e.X, e.Y, ref m_dMouse);
                        amMoving = true;
                    }
                    else if (((e.Button & MouseButtons.Left) == MouseButtons.Left) && ((Control.ModifierKeys & Keys.Shift) == Keys.Shift))
                    {
                        short tileX = 0;
                        short tileY = 0;
                        GContextMenu.Close();
                        Renderer.ResetHitTest();
                        ICell cell = Renderer.FindTileFromXY(e.X, e.Y, ref tileX, ref tileY, false);
                        if ((cell != null) && (cell.GetType() == typeof(MobileCell)))
                        {
                            Network.Send(new PPopupRequest((MobileCell) cell));
                        }
                        else if ((cell != null) && (cell.CellType == typeof(DynamicItem)))
                        {
                            Network.Send(new PPopupRequest(((DynamicItem) cell).m_Item));
                        }
                    }
                }
            }
        }

        public static void MouseMove(object sender, MouseEventArgs e)
        {
            if (m_EventOk && (e != null))
            {
                m_LastMouseArgs = e;
                m_MouseMoved = true;
                if ((m_xMouse != e.X) || (m_yMouse != e.Y))
                {
                    m_Redraw = true;
                }
                m_xMouse = e.X;
                m_yMouse = e.Y;
            }
        }

        public static void MouseMoveQueue()
        {
            if (m_EventOk)
            {
                MouseEventArgs lastMouseArgs = m_LastMouseArgs;
                m_MouseMoved = false;
                pointingDir = GetDirection(lastMouseArgs.X, lastMouseArgs.Y, ref m_dMouse);
                if ((m_xMouse != lastMouseArgs.X) || (m_yMouse != lastMouseArgs.Y))
                {
                    m_Redraw = true;
                }
                m_xMouse = lastMouseArgs.X;
                m_yMouse = lastMouseArgs.Y;
                if (!Gumps.MouseMove(lastMouseArgs.X, lastMouseArgs.Y, lastMouseArgs.Button))
                {
                    if (!m_Locked && amMoving)
                    {
                        GObjectProperties.Hide();
                        if (m_PopupDelay != null)
                        {
                            m_PopupDelay.Stop();
                        }
                        m_PopupDelay = null;
                        movingDir = pointingDir;
                    }
                    else if (amMoving && m_Ingame)
                    {
                        GObjectProperties.Hide();
                        if (m_PopupDelay != null)
                        {
                            m_PopupDelay.Stop();
                        }
                        m_PopupDelay = null;
                    }
                    else if (Gumps.Drag != null)
                    {
                        GObjectProperties.Hide();
                        if (m_PopupDelay != null)
                        {
                            m_PopupDelay.Stop();
                        }
                        m_PopupDelay = null;
                    }
                    else if ((lastMouseArgs.Button == MouseButtons.None) && (World.Serial != 0))
                    {
                        if (Features.AOS)
                        {
                            short tileX = 0;
                            short tileY = 0;
                            ICell cell = Renderer.FindTileFromXY(m_xMouse, m_yMouse, ref tileX, ref tileY, true);
                            if ((World.Player.Flags[MobileFlag.Warmode] && (cell != null)) && (cell.CellType == typeof(MobileCell)))
                            {
                                m_Highlight = ((MobileCell) cell).m_Mobile;
                            }
                            else
                            {
                                m_Highlight = null;
                            }
                            if (cell is DynamicItem)
                            {
                                Item item = ((DynamicItem) cell).m_Item;
                                if (item.Flags[ItemFlag.CanMove] || (Map.GetWeight(item.ID) < 0xff))
                                {
                                    if (item.PropertyList == null)
                                    {
                                        item.QueryProperties();
                                        GObjectProperties.Hide();
                                        if (m_PopupDelay != null)
                                        {
                                            m_PopupDelay.Stop();
                                        }
                                        m_PopupDelay = null;
                                    }
                                    else if ((GObjectProperties.Instance == null) || (GObjectProperties.Instance.Object != item))
                                    {
                                        if (m_PopupDelay == null)
                                        {
                                            m_PopupDelay = new Timer(new OnTick(Engine.PopupDelay_OnTick), 250);
                                            m_PopupDelay.SetTag("object", item);
                                            m_PopupDelay.Start(false);
                                        }
                                        else
                                        {
                                            m_PopupDelay.SetTag("object", item);
                                        }
                                    }
                                }
                                else
                                {
                                    GObjectProperties.Hide();
                                    if (m_PopupDelay != null)
                                    {
                                        m_PopupDelay.Stop();
                                    }
                                    m_PopupDelay = null;
                                }
                            }
                            else if (cell is MobileCell)
                            {
                                Mobile mobile = ((MobileCell) cell).m_Mobile;
                                if (mobile.PropertyList == null)
                                {
                                    mobile.QueryProperties();
                                    GObjectProperties.Hide();
                                    if (m_PopupDelay != null)
                                    {
                                        m_PopupDelay.Stop();
                                    }
                                    m_PopupDelay = null;
                                }
                                else if ((GObjectProperties.Instance == null) || (GObjectProperties.Instance.Object != mobile))
                                {
                                    if (m_PopupDelay == null)
                                    {
                                        m_PopupDelay = new Timer(new OnTick(Engine.PopupDelay_OnTick), 250);
                                        m_PopupDelay.SetTag("object", mobile);
                                        m_PopupDelay.Start(false);
                                    }
                                    else
                                    {
                                        m_PopupDelay.SetTag("object", mobile);
                                    }
                                }
                            }
                            else
                            {
                                GObjectProperties.Hide();
                                if (m_PopupDelay != null)
                                {
                                    m_PopupDelay.Stop();
                                }
                                m_PopupDelay = null;
                            }
                        }
                    }
                    else if ((lastMouseArgs.Button == MouseButtons.Left) && m_Ingame)
                    {
                        GObjectProperties.Hide();
                        if (m_PopupDelay != null)
                        {
                            m_PopupDelay.Stop();
                        }
                        m_PopupDelay = null;
                        if (m_LastDown > 0)
                        {
                            short num3 = 0;
                            short num4 = 0;
                            ICell cell2 = Renderer.FindTileFromXY(m_xMouse, m_yMouse, ref num3, ref num4, true);
                            if (m_LastDown < 0x40000000)
                            {
                                if (((cell2 == null) || (cell2.CellType != typeof(MobileCell))) || ((((MobileCell) cell2).m_Mobile.Serial != m_LastDown) || ((m_LastDownPoint ^ new Point(m_xMouse, m_yMouse)) >= 2)))
                                {
                                    Mobile mobile2 = World.FindMobile(m_LastDown);
                                    if (mobile2 != null)
                                    {
                                        mobile2.QueryStats();
                                        mobile2.OpenStatus(true);
                                    }
                                    m_LastDown = 0;
                                }
                            }
                            else if (((cell2 == null) || (cell2.CellType != typeof(DynamicItem))) || ((((DynamicItem) cell2).Serial != m_LastDown) || ((m_LastDownPoint ^ new Point(m_xMouse, m_yMouse)) >= 2)))
                            {
                                Mobile player = World.Player;
                                if ((player != null) && !player.Ghost)
                                {
                                    Item item2 = World.FindItem(m_LastDown);
                                    if (item2 != null)
                                    {
                                        Gump gump = item2.OnBeginDrag();
                                        if (gump.GetType() == typeof(GDragAmount))
                                        {
                                            ((GDragAmount) gump).ToDestroy = item2;
                                        }
                                        else
                                        {
                                            item2.RestoreInfo = new RestoreInfo(item2);
                                            World.Remove(item2);
                                        }
                                    }
                                }
                                m_LastDown = 0;
                            }
                        }
                        else if (m_LastDown == -1)
                        {
                            short num5 = 0;
                            short num6 = 0;
                            Renderer.ResetHitTest();
                            ICell cell3 = Renderer.FindTileFromXY(m_xMouse, m_yMouse, ref num5, ref num6, false);
                            m_LastDownPoint = new Point(m_xMouse, m_yMouse);
                            if ((cell3 != null) && (cell3.GetType() == typeof(MobileCell)))
                            {
                                m_LastDown = ((MobileCell) cell3).m_Mobile.Serial;
                            }
                            else if ((cell3 != null) && (cell3.GetType() == typeof(DynamicItem)))
                            {
                                Item item3 = ((DynamicItem) cell3).m_Item;
                                if (item3 != null)
                                {
                                    if ((Map.GetWeight(item3.ID) < 0xff) || item3.Flags[ItemFlag.CanMove])
                                    {
                                        m_LastDown = ((DynamicItem) cell3).Serial;
                                    }
                                    else
                                    {
                                        m_LastDown = -1;
                                    }
                                }
                                else
                                {
                                    m_LastDown = -1;
                                }
                            }
                        }
                    }
                    else if ((!m_Locked && amMoving) && (!amMoving || !m_Ingame))
                    {
                        GObjectProperties.Hide();
                        if (m_PopupDelay != null)
                        {
                            m_PopupDelay.Stop();
                        }
                        m_PopupDelay = null;
                        amMoving = false;
                    }
                    else if (!m_Locked && amMoving)
                    {
                        GObjectProperties.Hide();
                        if (m_PopupDelay != null)
                        {
                            m_PopupDelay.Stop();
                        }
                        m_PopupDelay = null;
                        movingDir = pointingDir;
                    }
                    else
                    {
                        GObjectProperties.Hide();
                        if (m_PopupDelay != null)
                        {
                            m_PopupDelay.Stop();
                        }
                        m_PopupDelay = null;
                    }
                }
            }
        }

        public static void MouseUp(object sender, MouseEventArgs e)
        {
            if (m_EventOk && (e != null))
            {
                m_LastMouseArgs = e;
                m_LastDown = -1;
                m_xMouse = e.X;
                m_yMouse = e.Y;
                Gump drag = Gumps.Drag;
                if (!Gumps.MouseUp(e.X, e.Y, e.Button))
                {
                    if ((!m_Locked && (e.Button == MouseButtons.Right)) && ((Control.MouseButtons == MouseButtons.None) || (Gumps.Drag != null)))
                    {
                        amMoving = false;
                    }
                    else if ((drag != null) && (drag.GetType() == typeof(GDraggedItem)))
                    {
                        Renderer.ResetHitTest();
                        GDraggedItem g = (GDraggedItem) drag;
                        g.m_IsDragging = false;
                        Gumps.Drag = null;
                        Gumps.Destroy(g);
                        Item item = g.Item;
                        short tileX = 0;
                        short tileY = 0;
                        ICell cell = Renderer.FindTileFromXY(e.X, e.Y, ref tileX, ref tileY);
                        if (cell != null)
                        {
                            if (cell.CellType == typeof(MobileCell))
                            {
                                Network.Send(new PDropItem(item.Serial, -1, -1, 0, ((MobileCell) cell).m_Mobile.Serial));
                            }
                            else if (cell.CellType == typeof(DynamicItem))
                            {
                                Item item3 = ((DynamicItem) cell).m_Item;
                                TileFlags flags = Map.m_ItemFlags[item3.ID & 0x3fff];
                                if (flags[TileFlag.Container])
                                {
                                    Network.Send(new PDropItem(item.Serial, -1, -1, 0, item3.Serial));
                                }
                                else if ((flags[TileFlag.Generic] && (item.ID == item3.ID)) && (item.Hue == item3.Hue))
                                {
                                    Network.Send(new PDropItem(item.Serial, item3.X, item3.Y, (sbyte) item3.Z, item3.Serial));
                                }
                                else
                                {
                                    Network.Send(new PDropItem(item.Serial, item3.X, item3.Y, (sbyte) item3.Z, -1));
                                }
                            }
                            else
                            {
                                Network.Send(new PDropItem(item.Serial, tileX, tileY, (sbyte) (cell.Z + cell.Height), -1));
                            }
                        }
                    }
                    else
                    {
                        if ((((m_TargetHandler == null) || (drag != null)) || ((Gumps.Drag != null) || (e.Button != MouseButtons.Left))) || (amMoving || (Control.MouseButtons != MouseButtons.None)))
                        {
                            return;
                        }
                        short num3 = 0;
                        short num4 = 0;
                        ICell cell2 = Renderer.FindTileFromXY(e.X, e.Y, ref num3, ref num4);
                        if (cell2 != null)
                        {
                            if (cell2 is MobileCell)
                            {
                                Target(((MobileCell) cell2).m_Mobile);
                            }
                            else if (cell2 is DynamicItem)
                            {
                                Target(((DynamicItem) cell2).m_Item);
                            }
                            else if (cell2 is StaticItem)
                            {
                                Target(new StaticTarget(num3, num4, ((StaticItem) cell2).m_Z, ((StaticItem) cell2).m_RealID, ((StaticItem) cell2).m_RealID, ((StaticItem) cell2).m_Hue));
                            }
                            else if (cell2 is LandTile)
                            {
                                Target(new LandTarget(num3, num4, ((LandTile) cell2).m_Z));
                            }
                        }
                    }
                    CancelClick();
                }
            }
        }

        public static void MouseWheel(object sender, MouseEventArgs e)
        {
            if ((m_EventOk && (((e.Delta <= 0) || !m_Ingame) || !Macros.Start(0x11000))) && (((e.Delta >= 0) || !m_Ingame) || !Macros.Start(0x11001)))
            {
                Gumps.MouseWheel(e.X, e.Y, e.Delta);
            }
        }

        public static unsafe int NativeRead(FileStream fs, void* pBuffer, int bytes)
        {
            return lread(fs.Handle, pBuffer, bytes);
        }

        public static unsafe int NativeRead(FileStream fs, byte[] buffer, int offset, int length)
        {
            fixed (byte* numRef = buffer)
            {
                return NativeRead(fs, (void*) (numRef + offset), length);
            }
        }

        public static unsafe int NativeWrite(FileStream fs, void* pBuffer, int bytes)
        {
            return lwrite(fs.Handle, pBuffer, bytes);
        }

        public static void NewCharacter_OnClick(Gump Sender)
        {
            ShowCharCreationProf();
        }

        public static void OnDeviceCreated(object sender, EventArgs e)
        {
            Renderer.m_Version++;
        }

        public static void OnDeviceLost(object sender, EventArgs e)
        {
            Renderer.m_Version++;
        }

        public static void OnDeviceReset(object sender, EventArgs e)
        {
            Renderer.m_Version++;
            m_VertexBuffer = new VertexBuffer(typeof(CustomVertex.TransformedColoredTextured), 0x8000, m_Device, 520, 0x144, 0);
            m_Device.SetStreamSource(0, m_VertexBuffer, 0);
            m_Device.set_VertexFormat(0x144);
            RenderStates states = m_Device.get_RenderState();
            Caps caps = m_Device.get_DeviceCaps();
            Texture.Square = caps.get_TextureCaps().get_SupportsSquareOnly();
            Texture.Pow2 = caps.get_TextureCaps().get_SupportsPower2();
            Texture.MaxTextureWidth = caps.get_MaxTextureWidth();
            Texture.MaxTextureHeight = caps.get_MaxTextureHeight();
            Texture.MinTextureWidth = 1;
            Texture.MinTextureHeight = 1;
            Texture.CanSysMem = caps.get_DeviceCaps().get_SupportsTextureSystemMemory();
            Texture.CanVidMem = caps.get_DeviceCaps().get_SupportsTextureVideoMemory();
            Texture.MaxAspect = caps.get_MaxTextureAspectRatio();
            Renderer.Init(caps);
            states.set_DitherEnable(false);
            states.set_NormalizeNormals(false);
            states.set_RangeFogEnable(false);
            states.set_StencilEnable(false);
            states.set_ZBufferEnable(true);
            states.set_ZBufferWriteEnable(true);
            states.set_CullMode(2);
            states.set_AntiAliasedLineEnable(false);
            states.set_SpecularEnable(false);
            states.set_ShadeMode(2);
            states.set_Lighting(false);
            states.set_VertexBlend(0);
            states.set_SourceBlend(5);
            states.set_DestinationBlend(6);
            states.set_ReferenceAlpha(1);
            states.set_AlphaFunction(7);
            states.set_AlphaBlendEnable(false);
            states.set_AlphaTestEnable(false);
            TextureState state = m_Device.get_TextureState().get_TextureState(0);
            state.set_AlphaArgument1(2);
            state.set_AlphaArgument2(0);
            state.set_AlphaOperation(4);
            state.set_ColorArgument1(2);
            state.set_ColorArgument2(0);
            state.set_ColorOperation(4);
            IndexBuffer buffer = new IndexBuffer(m_Device, 0x30000, 0, 0, true);
            short[] numArray = new short[0x18000];
            int index = 0;
            for (int i = 0; i < 0x4000; i++)
            {
                numArray[index] = (short) (i * 4);
                numArray[index + 1] = (short) ((i * 4) + 1);
                numArray[index + 2] = (short) ((i * 4) + 2);
                numArray[index + 3] = (short) ((i * 4) + 2);
                numArray[index + 4] = (short) ((i * 4) + 1);
                numArray[index + 5] = (short) ((i * 4) + 3);
                index += 6;
            }
            buffer.SetData(numArray, 0, 0);
            m_Device.set_Indices(buffer);
        }

        public static void OnDeviceResizing(object sender, CancelEventArgs e)
        {
            Renderer.m_Version++;
        }

        public static void OpenBackpack()
        {
            Mobile player = World.Player;
            if (player != null)
            {
                Item backpack = player.Backpack;
                if (backpack != null)
                {
                    backpack.Use();
                }
            }
        }

        public static void OpenBrowser(string url)
        {
            try
            {
                RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command");
                string str = (string) key.GetValue("");
                string fileName = str.Substring(str.IndexOf('"') + 1, (str.LastIndexOf('"') - str.IndexOf('"')) - 1);
                string str3 = str.Substring(str.LastIndexOf('"') + 1);
                key.Close();
                Process.Start(fileName, string.Format("{0} {1}", str3, url));
            }
            catch
            {
                try
                {
                    Process.Start("iexplore.exe", url);
                }
                catch
                {
                    Debug.Trace("Failed launching browser to \"{0}\"", url);
                }
            }
        }

        private static void OpenDyeWindow(OnClick OnClick, Gump g)
        {
            GAlphaBackground background = new GAlphaBackground(0, 0, 0xb8, 110) {
                m_NonRestrictivePicking = true,
                m_CanDrag = false,
                X = 0,
                Y = g.Parent.Height - 1
            };
            GHuePicker toAdd = new GHuePicker(4, 4) {
                Brightness = 1
            };
            toAdd.SetTag("Dialog", background);
            background.Children.Add(toAdd);
            background.Children.Add(new GSingleBorder(3, 3, 0xa2, 0x52));
            background.Children.Add(new GSingleBorder(0xa4, 3, 0x11, 0x52));
            GBrightnessBar bar = new GBrightnessBar(0xa5, 4, 15, 80, toAdd);
            background.Children.Add(bar);
            bar.Refresh();
            GFlatButton okay = new GFlatButton(0x7b, 0x57, 0x3a, 20, "Okay", OnClick);
            okay.SetTag("Hue Picker", toAdd);
            okay.SetTag("Dialog", background);
            okay.SetTag("Button", g);
            background.Children.Add(new GQuickHues(toAdd, bar, okay));
            background.Children.Add(okay);
            g.Parent.Children.Add(background);
        }

        public static void OpenHelp()
        {
            Network.Send(new PRequestHelp());
        }

        public static void OpenJournal()
        {
            if (!m_JournalOpen)
            {
                m_JournalGump = new GJournal();
                m_JournalOpen = true;
                Gumps.Desktop.Children.Add(m_JournalGump);
            }
        }

        public static void OpenOptions()
        {
            GObjectEditor.Open(World.CharData);
        }

        public static void OpenOptionsMacros()
        {
        }

        public static void OpenPaperdoll()
        {
            Network.Send(new POpenPaperdoll());
        }

        public static void OpenRadar()
        {
            GRadar.Open();
        }

        public static void OpenSkills()
        {
            if (!m_SkillsOpen)
            {
                Network.Send(new PQuerySkills());
                m_SkillsOpen = true;
                m_SkillsGump = new GSkills();
                Gumps.Desktop.Children.Add(m_SkillsGump);
            }
        }

        public static void OpenSpellbook(int num)
        {
            Network.Send(new POpenSpellbook(num));
        }

        public static void OpenStatus()
        {
            Mobile player = World.Player;
            if (player != null)
            {
                player.QueryStats();
                player.BigStatus = true;
                player.OpenStatus(false);
            }
        }

        public static void Options_OnClick(Gump g)
        {
            OpenOptions();
        }

        private static void Options_OnDispose(Gump g)
        {
            m_OptionsOpen = false;
            m_DyeWindowOpen = false;
            World.CharData.Save();
        }

        private static void ParseArgs(string[] Args)
        {
            int length = Args.Length;
            int index = 0;
            while (index < length)
            {
                string str = Args[index].ToLower();
                index++;
                string str2 = str;
                if (str2 != null)
                {
                    str2 = string.IsInterned(str2);
                    if (str2 == "-ini")
                    {
                        m_IniPath = Args[index++];
                    }
                    else
                    {
                        if (str2 == "-data")
                        {
                            m_OverrideDataPath = Args[index++];
                            continue;
                        }
                        if (str2 == "-host")
                        {
                            m_OverrideServHost = Args[index++];
                            continue;
                        }
                        if (str2 == "-port")
                        {
                            m_OverrideServPort = Convert.ToInt32(Args[index++]);
                        }
                    }
                }
            }
        }

        public static void Ping_OnTick(Timer t)
        {
            int tickCount = Environment.TickCount;
            if (Network.Send(new PPing(m_PingID)))
            {
                m_Pings.Enqueue(tickCount);
                m_PingID++;
            }
        }

        public static void PingReply()
        {
            try
            {
                int tickCount = Environment.TickCount;
                int num2 = (int) m_Pings.Dequeue();
                m_Ping = tickCount - num2;
            }
            catch
            {
                StartPings();
            }
        }

        private static void PlayRandomMidi()
        {
            if (NewConfig.PlayMusic)
            {
                int[] numArray = new int[] { 1, 2, 3, 5, 7 };
                System.Random random = new System.Random();
                string fileName = MidiTable.Translate(numArray[random.Next(numArray.Length)]);
                if (fileName != null)
                {
                    Music.Play(fileName);
                }
            }
        }

        private static void PopupDelay_OnTick(Timer t)
        {
            GObjectProperties.Display(t.GetTag("object"));
            t.Stop();
            m_PopupDelay = null;
        }

        public static void Preload(Worker w)
        {
            TileMatrix matrix = w.Matrix;
            int blockWidth = matrix.BlockWidth;
            int blockHeight = matrix.BlockHeight;
            if ((((w.X >= 0) && (w.Y >= 0)) && (w.X < blockWidth)) && (w.Y < blockHeight))
            {
                Mobile player = World.Player;
                bool grayscale = (player != null) && player.Ghost;
                IHintable hintable = grayscale ? ((IHintable) Hues.Grayscale) : ((IHintable) Hues.Default);
                MapBlock block = matrix.GetBlock(w.X, w.Y);
                bool flag2 = false;
                for (int i = 0; !flag2 && (i < m_KeepAliveBlocks.Length); i++)
                {
                    flag2 = m_KeepAliveBlocks[i] == block;
                }
                if (!flag2)
                {
                    m_KeepAliveBlocks[m_KeepAliveBlockIndex % m_KeepAliveBlocks.Length] = block;
                    m_KeepAliveBlockIndex++;
                }
                Tile[] tileArray = (block == null) ? matrix.InvalidLandBlock : block.m_LandTiles;
                for (int j = 0; j < tileArray.Length; j++)
                {
                    if (!hintable.HintLand(tileArray[j].ID & 0x3fff))
                    {
                        m_LoadQueue.Enqueue(new LandLoader(tileArray[j].ID & 0x3fff, grayscale));
                    }
                }
                HuedTile[][][] tileArray2 = (block == null) ? matrix.EmptyStaticBlock : block.m_StaticTiles;
                for (int k = 0; k < 8; k++)
                {
                    for (int m = 0; m < 8; m++)
                    {
                        HuedTile[] tileArray3 = tileArray2[k][m];
                        for (int n = 0; n < tileArray3.Length; n++)
                        {
                            if ((tileArray3[n].Hue == 0) && !hintable.HintItem(tileArray3[n].ID & 0x3fff))
                            {
                                m_LoadQueue.Enqueue(new ItemLoader(tileArray3[n].ID & 0x3fff, grayscale));
                            }
                        }
                    }
                }
            }
        }

        public static void PrintQAM()
        {
            if (m_QamList.Count == 0)
            {
                AddTextMessage("No items were found to move.", DefaultFont, Hues.Load(0x35));
            }
            else if (m_QamList.Count == 1)
            {
                AddTextMessage(string.Format("Moving {0} item...", m_QamList.Count), DefaultFont, Hues.Load(0x35));
            }
            else
            {
                AddTextMessage(string.Format("Moving {0} items...", m_QamList.Count), DefaultFont, Hues.Load(0x35));
            }
        }

        public static void Profession_OnClick(Gump Sender)
        {
            ShowCharCreationSkills((int) Sender.GetTag("Strength"), (int) Sender.GetTag("Dexterity"), (int) Sender.GetTag("Intelligence"), (int) Sender.GetTag("vSkill1"), (int) Sender.GetTag("vSkill2"), (int) Sender.GetTag("vSkill3"), (int) Sender.GetTag("iSkill1"), (int) Sender.GetTag("iSkill2"), (int) Sender.GetTag("iSkill3"));
        }

        private static void QamTimer_OnTick(Timer t)
        {
            Mobile player = World.Player;
            if (player != null)
            {
                bool gMPrivs = GMPrivs;
                while (m_QamList.Count > 0)
                {
                    QamEntry entry = (QamEntry) m_QamList.Peek();
                    if (entry.m_Item == null)
                    {
                        m_QamList.Dequeue();
                        if (m_QamList.Count == 0)
                        {
                            AddTextMessage("The queue is now empty.", DefaultFont, Hues.Load(0x35));
                        }
                        else if (m_QamList.Count == 1)
                        {
                            AddTextMessage(string.Format("There is {0} entry left in the queue.", m_QamList.Count), DefaultFont, Hues.Load(0x35));
                        }
                        else
                        {
                            AddTextMessage(string.Format("There are {0} entries left in the queue.", m_QamList.Count), DefaultFont, Hues.Load(0x35));
                        }
                    }
                    else
                    {
                        int num = (entry.m_Item.Parent == null) ? -1 : entry.m_Item.Parent.Serial;
                        if ((!entry.m_Item.Visible || (num == entry.m_Target)) && (((entry.m_X == -1) || (entry.m_Y == -1)) || ((num == -1) ? (((entry.m_Item.X == entry.m_X) && (entry.m_Item.Y == entry.m_Y)) && (entry.m_Item.Z == entry.m_Z)) : ((entry.m_Item.ContainerX == entry.m_X) && (entry.m_Item.ContainerY == entry.m_Y)))))
                        {
                            m_QamList.Dequeue();
                            if (m_QamList.Count == 0)
                            {
                                AddTextMessage("The queue is now empty.", DefaultFont, Hues.Load(0x35));
                            }
                            else if (m_QamList.Count == 1)
                            {
                                AddTextMessage(string.Format("There is {0} entry left in the queue.", m_QamList.Count), DefaultFont, Hues.Load(0x35));
                            }
                            else
                            {
                                AddTextMessage(string.Format("There are {0} entries left in the queue.", m_QamList.Count), DefaultFont, Hues.Load(0x35));
                            }
                        }
                        else if (entry.m_Item.Visible && !entry.m_Item.InAbsSquareRange(player, 2))
                        {
                            m_QamList.Dequeue();
                            if (m_QamList.Count == 0)
                            {
                                AddTextMessage("Item out of range. The queue is now empty.", DefaultFont, Hues.Load(0x35));
                            }
                            else if (m_QamList.Count == 1)
                            {
                                AddTextMessage(string.Format("Item out of range. There is {0} entry left in the queue.", m_QamList.Count), DefaultFont, Hues.Load(0x35));
                            }
                            else
                            {
                                AddTextMessage(string.Format("Item out of range. There are {0} entries left in the queue.", m_QamList.Count), DefaultFont, Hues.Load(0x35));
                            }
                        }
                        else
                        {
                            if (!entry.m_Item.Visible)
                            {
                                break;
                            }
                            Network.Send(new PPickupItem(entry.m_Item, (short) entry.m_Amount));
                            Network.Send(new PDropItem(entry.m_Item.Serial, (short) entry.m_X, (short) entry.m_Y, (sbyte) entry.m_Z, entry.m_Target));
                            if (!gMPrivs)
                            {
                                break;
                            }
                            m_QamList.Dequeue();
                            AddTextMessage("Moving item without delay due to GM status.", DefaultFont, Hues.Load(0x35));
                        }
                        continue;
                    }
                }
                if (m_QamList.Count == 0)
                {
                    m_QamTimer.Stop();
                    m_QamTimer = null;
                }
            }
        }

        [DllImport("Kernel32")]
        public static extern bool QueryPerformanceCounter(ref long Counter);
        [DllImport("Kernel32")]
        private static extern bool QueryPerformanceFrequency(ref long Frequency);
        public static void QueueAutoMove(Item item, int amount, int x, int y, int z, int target)
        {
            int num = (item.Parent == null) ? -1 : item.Parent.Serial;
            if ((item.Visible && (num != target)) || (((x != -1) && (y != -1)) && !((num == -1) ? (((item.X == x) && (item.Y == y)) && (item.Z == z)) : ((item.ContainerX == x) && (item.ContainerY == y)))))
            {
                QamEntry entry = new QamEntry(item, amount, x, y, z, target);
                m_QamList.Enqueue(entry);
                if (m_QamTimer == null)
                {
                    m_QamTimer = new Timer(new OnTick(Engine.QamTimer_OnTick), 200);
                    m_QamTimer.Start(true);
                }
            }
        }

        public static void QueueMapLoad(int xBlock, int yBlock, TileMatrix matrix)
        {
            if (((xBlock >= 0) && (yBlock >= 0)) && ((xBlock < matrix.BlockWidth) && (yBlock < matrix.BlockHeight)))
            {
                int num = (xBlock * 0x200) + yBlock;
                bool ghost = false;
                Mobile player = World.Player;
                if (player != null)
                {
                    ghost = player.Ghost;
                }
                if (!matrix.CheckLoaded(xBlock, yBlock))
                {
                    m_MapLoadQueue.Enqueue(new Worker(xBlock, yBlock, matrix));
                }
            }
        }

        public static void QuickLogin_OnClick(Gump g)
        {
            int tag = (int) g.GetTag("Index");
            Entry entry = (Entry) QuickLogin.Entries[tag];
            m_QuickLogin = true;
            m_QuickEntry = entry;
            Cursor.Hourglass = true;
            Gumps.Desktop.Children.Clear();
            xGumps.Display("Connecting");
            DrawNow();
            if (Network.Connect())
            {
                Gumps.Desktop.Children.Clear();
                xGumps.Display("AccountVerify");
            }
            else
            {
                Gumps.Desktop.Children.Clear();
                xGumps.SetVariable("FailMessage", "Couldn't connect to the login server.  Either the server is down, or you've entered an invalid host / port.  Check Client.cfg.");
                xGumps.Display("ConnectionFailed");
                Cursor.Hourglass = false;
                m_QuickLogin = false;
                return;
            }
            Network.Send(new PLoginSeed());
            Network.Send(new PAccount(entry.AccountName, entry.Password));
        }

        public static void Quit()
        {
            GLogOutQuery.Display();
        }

        public static void Quit_OnClick(Gump Sender)
        {
            exiting = true;
        }

        public static int RandomRange(int start, int count)
        {
            return (start + Random.Next(count));
        }

        public static void Redraw()
        {
            m_Redraw = true;
        }

        public static void ReleaseDataStore(ArrayList list)
        {
            if (list.Count > 0)
            {
                list.Clear();
            }
            m_DataStores.Enqueue(list);
        }

        public static void RemoveTimer(Timer t)
        {
            m_Timers.Remove(t);
        }

        public static void Repeat()
        {
            if ((m_LastCommand != null) && (m_LastCommand.Length > 0))
            {
                commandEntered(m_LastCommand);
            }
        }

        public static void ResetTicks()
        {
            m_SetTicks = false;
        }

        public static void Resurrect_OnAnimationEnd(Animation a, Mobile m)
        {
            if (m != null)
            {
                m.Visible = false;
                m.Update();
            }
        }

        public static bool Resync()
        {
            if (!m_InResync)
            {
                m_InResync = true;
                AddTextMessage("Please wait, resynchronizing.");
                return Network.Send(new PResyncRequest());
            }
            return false;
        }

        public static int RGB(byte r, byte g, byte b)
        {
            return ((b | (g << 8)) | (r << 0x10));
        }

        public static void SaveJournal()
        {
        }

        public static void ScrollDown_OnClick(Gump Sender)
        {
            if (Sender.HasTag("Scroller"))
            {
                GVSlider tag = (GVSlider) Sender.GetTag("Scroller");
                if (tag != null)
                {
                    tag.SetValue(tag.GetValue() + tag.Increase, true);
                }
            }
        }

        public static void ScrollUp_OnClick(Gump Sender)
        {
            if (Sender.HasTag("Scroller"))
            {
                GVSlider tag = (GVSlider) Sender.GetTag("Scroller");
                if (tag != null)
                {
                    tag.SetValue(tag.GetValue() - tag.Increase, true);
                }
            }
        }

        public static void SendDelayedPacket(int delay, params Packet[] packets)
        {
            if (RealGMPrivs)
            {
                delay = 0;
            }
            Timer timer = new Timer(new OnTick(Engine.DelayedPackets_OnTick), delay, 1);
            timer.SetTag("Packets", packets);
            timer.Start(false);
        }

        public static void SendMovementRequest(int dir, int x, int y, int z)
        {
            int key = (m_WalkStack.Count > 0) ? ((int) m_WalkStack.Pop()) : 0;
            Network.Send(new PMoveRequest((byte) dir, (byte) m_Sequence, key, x, y, z));
            m_WalkReq++;
            m_Sequence++;
            if (m_Sequence == 0x100)
            {
                m_Sequence = 1;
            }
        }

        public static void Server_OnClick(Gump Sender)
        {
            Cursor.Hourglass = true;
            NewConfig.LastServerID = (int) Sender.GetTag("ServerID");
            NewConfig.Save();
            Network.Send(new PHardwareInfo());
            Network.Send(new PServerSelection((int) Sender.GetTag("ServerID")));
            for (int i = 0; i < m_Servers.Length; i++)
            {
                if (m_Servers[i].ServerID == NewConfig.LastServerID)
                {
                    m_ServerName = m_Servers[i].Name;
                }
            }
            Gumps.Desktop.Children.Clear();
            xGumps.Display("Connecting");
            Macros.Load();
            DrawNow();
        }

        public static void ShowAcctLogin()
        {
            Cursor.Gold = false;
            m_LastAttacker = null;
            Renderer.AlwaysHighlight = 0;
            World.CharData.Save();
            World.CharData = null;
            m_WalkStack = new Stack();
            m_WalkStack.Push(-1163005939);
            m_WalkStack.Push(-1163005939);
            m_WalkStack.Push(-1163005939);
            m_WalkStack.Push(-1163005939);
            m_WalkStack.Push(-1163005939);
            m_GMPrivs = false;
            SaveJournal();
            m_Journal.Clear();
            Renderer.DrawFPS = false;
            m_Ingame = false;
            m_WalkAck = 0;
            m_WalkReq = 0;
            Macros.StopAll();
            World.Clear();
            Cursor.Hourglass = false;
            m_QuickLogin = false;
            m_TargetHandler = null;
            Gumps.Desktop.Children.Clear();
            Benchmark benchmark = new Benchmark(7);
            benchmark.Start();
            xGumps.Display("AccountLogin");
            benchmark.StopNoLog();
            Debug.Trace("Display( \"AccountLogin\" ) -> {0}", Benchmark.Format(benchmark.Elapsed));
            benchmark.Start();
            Gump child = BuildSmartLoginMenu();
            xGumps.AddGumpTo("AccountLogin", child);
            xGumps.AddGumpTo("AccountLogin", new GQuickLogin());
            benchmark.StopNoLog();
            Debug.Trace("AddGumpTo( \"AccountLogin\" ) -> {0}", Benchmark.Format(benchmark.Elapsed));
            if (!m_FirstAcctLogin)
            {
                PlayRandomMidi();
            }
            while ((child != null) && (child != Gumps.Desktop))
            {
                child.m_NonRestrictivePicking = true;
                child = child.Parent;
            }
            m_FirstAcctLogin = false;
        }

        public static void ShowCharAppearance(int Str, int Dex, int Int, int vSkill1, int vSkill2, int vSkill3, int iSkill1, int iSkill2, int iSkill3)
        {
            Cursor.Hourglass = false;
            Gumps.Desktop.Children.Clear();
            Gumps.Desktop.Children.Add(new GBackground(0x588, ScreenWidth, ScreenHeight, false));
            Gumps.Desktop.Children.Add(new GImage(0x157c, 0, 0));
            Gumps.Desktop.Children.Add(new GImage(0x15a0, 0, 4));
            GButton toAdd = new GButton(0x1589, 0x22b, 4, new OnClick(Engine.Quit_OnClick)) {
                Tooltip = new Tooltip(Strings.GetString("Tooltips.Quit"))
            };
            Gumps.Desktop.Children.Add(toAdd);
            Gumps.Desktop.Children.Add(new GImage(0x709, 280, 0x35));
            GTextBox box = new GTextBox(0, false, 0xf8, 0x4b, 0xd7, 0x10, "Name", GetFont(5), Hues.Load(0x76b), Hues.Load(0x835), Hues.Load(0x25)) {
                Tooltip = new Tooltip(Strings.GetString("Tooltips.CharCreateName"))
            };
            Gumps.Desktop.Children.Add(new GImage(0x70a, 240, 0x49));
            Gumps.Desktop.Children.Add(new GBackground(0x70b, 0xd7, 0x10, 0xf8, 0x49, false));
            Gumps.Desktop.Children.Add(new GImage(0x70c, 0x1cf, 0x49));
            Gumps.Desktop.Children.Add(box);
            GImage image = new GImage(0x708, 0xee, 0x62);
            GButton button2 = new GButton(0x710, 0x48, 320, new OnClick(Engine.CharGender_OnClick));
            button2.SetTag("Gender", 0);
            image.Children.Add(button2);
            Gumps.Desktop.Children.Add(image);
            Gumps.Desktop.Children.Add(new GBackground(0xe14, 0x97, 310, 0x52, 0x7d, true));
            GBackground background = new GBackground(0xe14, 0x97, 310, 0x1db, 0x7d, true);
            string[] strArray = new string[] { "Skin Tone", "Shirt Color", "Pants Color", "Hair Color", "Facial Hair Color" };
            string[] strArray2 = new string[] { Strings.GetString("Tooltips.CharCreateSkinTone"), Strings.GetString("Tooltips.CharCreateShirtHue"), Strings.GetString("Tooltips.CharCreatePantsHue"), Strings.GetString("Tooltips.CharCreateHairHue"), Strings.GetString("Tooltips.CharCreateFHairHue") };
            int[] numArray = new int[] { GetRandomSkinHue(), GetRandomHue(), GetRandomHue(), GetRandomHairHue(), GetRandomHairHue(), GetRandomYellowHue() };
            numArray[0] ^= 0x8000;
            if (!Map.m_ItemFlags[0x1517][TileFlag.PartialHue])
            {
                numArray[1] ^= 0x8000;
            }
            if (!Map.m_ItemFlags[0x152e][TileFlag.PartialHue])
            {
                numArray[2] ^= 0x8000;
            }
            if (!Map.m_ItemFlags[0x203b][TileFlag.PartialHue])
            {
                numArray[3] ^= 0x8000;
            }
            if (!Map.m_ItemFlags[0x2040][TileFlag.PartialHue])
            {
                numArray[4] ^= 0x8000;
            }
            if (!Map.m_ItemFlags[0x170f][TileFlag.PartialHue])
            {
                numArray[5] ^= 0x8000;
            }
            Gump[] gumpArray = new Gump[] { new GImage(0x761, Hues.Load(numArray[0]), 0, 0), new GImage(0x739, Hues.Load(numArray[1]), 0, 0), new GImage(0x738, Hues.Load(numArray[2]), 0, 0), new GImage(0x753, Hues.Load(numArray[3]), 0, 0), new GImage(0x759, Hues.Load(numArray[4]), 0, 0), new GImage(0x762, Hues.Load(numArray[5]), 0, 0) };
            gumpArray[0].SetTag("ItemID", 0);
            gumpArray[1].SetTag("ItemID", 0x1517);
            gumpArray[2].SetTag("ItemID", 0x152e);
            gumpArray[3].SetTag("ItemID", 0x203b);
            gumpArray[4].SetTag("ItemID", 0x2040);
            gumpArray[5].SetTag("ItemID", 0x170f);
            button2.SetTag("Image", gumpArray[0]);
            int offsetY = background.OffsetY;
            image.Children.Add(gumpArray[0]);
            image.Children.Add(gumpArray[5]);
            image.Children.Add(gumpArray[1]);
            image.Children.Add(gumpArray[2]);
            image.Children.Add(gumpArray[4]);
            image.Children.Add(gumpArray[3]);
            GButton button3 = new GButton(0x15a4, 610, 0x1bd, new OnClick(Engine.CharCreationAppearanceArrow_OnClick));
            button2.SetTag("Image[5]", gumpArray[5]);
            button2.SetTag("Arrow", button3);
            UnicodeFont uniFont = GetUniFont(0);
            for (int i = 0; i < 5; i++)
            {
                GTextButton button4 = new GTextButton(strArray[i], uniFont, Hues.Bright, Hues.Load(0x26), background.OffsetX, offsetY, new OnClick(Engine.AppearanceHueProperty_OnClick)) {
                    SpaceWidth = 6
                };
                offsetY += button4.Height - 2;
                GHuePreview preview = new GHuePreview(background.OffsetX, offsetY, 100, 0x10, numArray[i], false);
                offsetY += 0x10;
                button4.Tooltip = new Tooltip(strArray2[i]);
                button4.SetTag("Property", strArray[i]);
                button4.SetTag("Preview", preview);
                button4.SetTag("Image", gumpArray[i]);
                button2.SetTag(string.Format("Image[{0}]", i), gumpArray[i]);
                button3.SetTag(strArray[i], preview);
                background.Children.Add(button4);
                background.Children.Add(preview);
                GHotspot hotspot = new GHotspot(button4.X, button4.Y, Biggest(button4.Width, preview.Width), (preview.Y + preview.Height) - button4.Y, button4) {
                    Tooltip = new Tooltip(strArray2[i])
                };
                background.Children.Add(hotspot);
                if (i == 4)
                {
                    button2.SetTag("HideHS", hotspot);
                    button2.SetTag("HideTB", button4);
                    button2.SetTag("HideHP", preview);
                }
            }
            Gumps.Desktop.Children.Add(background);
            button3.SetTag("Strength", Str);
            button3.SetTag("Dexterity", Dex);
            button3.SetTag("Intelligence", Int);
            button3.SetTag("vSkill1", vSkill1);
            button3.SetTag("vSkill2", vSkill2);
            button3.SetTag("vSkill3", vSkill3);
            button3.SetTag("iSkill1", iSkill1);
            button3.SetTag("iSkill2", iSkill2);
            button3.SetTag("iSkill3", iSkill3);
            button3.SetTag("Name", box);
            button3.SetTag("Gender", 0);
            Gumps.Desktop.Children.Add(button3);
        }

        public static void ShowCharCitySelection(int Str, int Dex, int Int, int vSkill1, int vSkill2, int vSkill3, int iSkill1, int iSkill2, int iSkill3, int hSkinTone, int hShirtColor, int hPantsColor, int hHairColor, int hFacialHairColor, string Name, int Gender)
        {
            Cursor.Hourglass = false;
            Point[] pointArray = new Point[] { new Point(0x2c, 0x5e), new Point(190, 0x2f), new Point(0x74, 0x9a), new Point(0x152, 0x6f), new Point(0x8f, 0x102), new Point(0x116, 0xcb), new Point(0x68, 0x15b), new Point(0x2a, 0xc6), new Point(220, 0x54) };
            string[] strArray = new string[] { "Yew", "Minoc", "Britain", "Moonglow", "Trinsic", "Magincia", "Jhelom", "Skara Brae", "Vesper" };
            Gumps.Desktop.Children.Clear();
            Gumps.Desktop.Children.Add(new GBackground(0x588, ScreenWidth, ScreenHeight, false));
            Gumps.Desktop.Children.Add(new GImage(0x157c, 0, 0));
            Gumps.Desktop.Children.Add(new GImage(0x15a0, 0, 4));
            GButton toAdd = new GButton(0x1589, 0x22b, 4, new OnClick(Engine.Quit_OnClick)) {
                Tooltip = new Tooltip(Strings.GetString("Tooltips.Quit"))
            };
            Gumps.Desktop.Children.Add(toAdd);
            Gumps.Desktop.Children.Add(new GBackground(0xbbc, 0x9e, 0x16f, 0x1c4, 60, true));
            GImage image = new GImage(0x1598, 0x39, 0x31);
            for (int i = 0; i < 9; i++)
            {
                GTextButton button2;
                GImage image2 = new GImage(0x4b9, pointArray[i].X, pointArray[i].Y);
                image.Children.Add(image2);
                button2 = new GTextButton(strArray[i], GetUniFont(0), Hues.Load(0x58), Hues.Load(0x35), pointArray[i].X, 0, new OnClick(Engine.City_OnClick)) {
                    Y = (pointArray[i].Y - button2.Height) - 1
                };
                if (i == 3)
                {
                    button2.X = (pointArray[i].X + 14) - button2.Width;
                }
                button2.SetTag("Strength", Str);
                button2.SetTag("Dexterity", Dex);
                button2.SetTag("Intelligence", Int);
                button2.SetTag("vSkill1", vSkill1);
                button2.SetTag("vSkill2", vSkill2);
                button2.SetTag("vSkill3", vSkill3);
                button2.SetTag("iSkill1", iSkill1);
                button2.SetTag("iSkill2", iSkill2);
                button2.SetTag("iSkill3", iSkill3);
                button2.SetTag("Skin Tone", hSkinTone);
                button2.SetTag("Shirt Color", hShirtColor);
                button2.SetTag("Pants Color", hPantsColor);
                button2.SetTag("Hair Color", hHairColor);
                button2.SetTag("Facial Hair Color", hFacialHairColor);
                button2.SetTag("CityID", i);
                button2.SetTag("Name", Name);
                button2.SetTag("Gender", Gender);
                image.Children.Add(button2);
                int x = Smallest(image2.X, button2.X);
                int y = Smallest(image2.Y, button2.Y);
                int width = Biggest(image2.X + image2.Width, button2.X + button2.Width) - x;
                int height = Biggest(image2.Y + image2.Height, button2.Y + button2.Height) - y;
                GHotspot hotspot = new GHotspot(x, y, width, height, button2) {
                    Tooltip = new Tooltip("Click here to enter this city")
                };
                image.Children.Add(hotspot);
            }
            Gumps.Desktop.Children.Add(image);
        }

        public static void ShowCharCreationProf()
        {
            Cursor.Hourglass = false;
            Gumps.Desktop.Children.Clear();
            GBackground toAdd = new GBackground(0xa2c, 0x222, 0x160, 80, 80, true);
            toAdd.Children.Add(new GImage(0x58b, 0x42, -22));
            toAdd.Children.Add(new GImage(0x589, 0x8e, -38));
            toAdd.Children.Add(new GImage(0x1580, 0x97, -30));
            toAdd.Children.Add(new GButton(0x119c, 120, 0x114, null));
            toAdd.Children.Add(new GBackground(0xbbc, 0xcc, 0xd6, 40, 0x39, true));
            int num = ((toAdd.UseHeight - 0x13c) + 1) / 2;
            num += toAdd.OffsetY;
            string[] strArray = new string[] { "Warrior", "Magician", "Blacksmith", "Advanced" };
            int[] numArray = new int[] { 0x15c9, 0x15c1, 0x15b3, 0x15a9 };
            int[] numArray2 = new int[] { 0x23, 0x23, 10, 0x19, 10, 0x2d, 60, 10, 10, 60, 10, 10 };
            int[] numArray3 = new int[] { 50, 0x2d, 5, 50, 50, 0, 50, 0x2d, 5, 50, 50, 0 };
            int[] numArray4 = new int[] { 0x1b, 0x11, 40, 0x19, 0x2e, 0x2b, 7, 0x25, 0x2d, -1, -1, -1 };
            for (int i = 0; i < 4; i++)
            {
                GLabel label;
                GButton target = new GButton(numArray[i], numArray[i], numArray[i] + 1, 0x1ad, (num + (i * 0x4f)) + 8, new OnClick(Engine.Profession_OnClick));
                target.SetTag("Strength", numArray2[i * 3]);
                target.SetTag("Dexterity", numArray2[(i * 3) + 1]);
                target.SetTag("Intelligence", numArray2[(i * 3) + 2]);
                target.SetTag("vSkill1", numArray3[i * 3]);
                target.SetTag("vSkill2", numArray3[(i * 3) + 1]);
                target.SetTag("vSkill3", numArray3[(i * 3) + 2]);
                target.SetTag("iSkill1", numArray4[i * 3]);
                target.SetTag("iSkill2", numArray4[(i * 3) + 1]);
                target.SetTag("iSkill3", numArray4[(i * 3) + 2]);
                toAdd.Children.Add(new GMouseRouter(0x589, 420, num + (i * 0x4f), target));
                toAdd.Children.Add(target);
                label = new GLabel(strArray[i], GetFont(9), Hues.Load(0x76b), 0, 0) {
                    X = 410 - label.Width,
                    Y = (num + (i * 0x4f)) + ((80 - label.Height) / 2)
                };
                toAdd.Children.Add(label);
            }
            Gumps.Desktop.Children.Add(new GBackground(0x588, ScreenWidth, ScreenHeight, false));
            Gumps.Desktop.Children.Add(toAdd);
            Gumps.Desktop.Children.Add(new GImage(0x157c, 0, 0));
            Gumps.Desktop.Children.Add(new GImage(0x15a0, 0, 4));
            GButton button2 = new GButton(0x1589, 0x22b, 4, new OnClick(Engine.Quit_OnClick)) {
                Tooltip = new Tooltip(Strings.GetString("Tooltips.Quit"))
            };
            Gumps.Desktop.Children.Add(button2);
        }

        public static void ShowCharCreationSkills(int Str, int Dex, int Int, int vSkill1, int vSkill2, int vSkill3, int iSkill1, int iSkill2, int iSkill3)
        {
            GButton button2;
            GVSlider slider;
            GTextButton button3;
            GTextButton button4;
            GTextButton button5;
            Cursor.Hourglass = false;
            GBackground toAdd = new GBackground(0xa2c, 0x222, 0x160, 80, 80, true);
            toAdd.Children.Add(new GImage(0x58b, 0x42, -22));
            toAdd.Children.Add(new GImage(0x589, 0x8e, -38));
            toAdd.Children.Add(new GImage(0x1580, 0x97, -30));
            toAdd.Children.Add(new GButton(0x119c, 120, 0x114, null));
            GListBox box = new GListBox(GetFont(9), Hues.Load(0x76b), Hues.Load(0x961), 0xbbc, 40, 0x39, 0xcc, 0xd6, true);
            Client.Skills skills = Skills;
            for (int i = 0; i < 0x100; i++)
            {
                Skill skill = skills[i];
                if (skill == null)
                {
                    break;
                }
                box.AddItem(skill.Name);
            }
            box.OnClick = new OnClick(Engine.CharSkill_OnClick);
            toAdd.Children.Add(box);
            int num2 = 0x31 - box.ItemCount;
            if (num2 < 0)
            {
                num2 = 0;
            }
            GButton button = new GButton(250, 250, 0xfb, box.X + box.Width, box.Y, new OnClick(Engine.ScrollUp_OnClick));
            toAdd.Children.Add(button);
            button2 = new GButton(0xfc, 0xfc, 0xfd, box.X + box.Width, box.Y + box.Height, new OnClick(Engine.ScrollDown_OnClick)) {
                Y = button2.Y - button2.Height
            };
            toAdd.Children.Add(button2);
            toAdd.Children.Add(new GBackground(0x100, button.Width, button2.Y - (button.Y + button.Height), button.X, button.Y + button.Height, false));
            slider = new GVSlider(0xfe, (box.X + box.Width) + 1, button.Y + button.Height, 13, button2.Y - (button.Y + button.Height), 0.0, 0.0, (double) num2, 1.0) {
                Y = slider.Y + slider.HalfHeight,
                Height = slider.Height - (slider.HalfHeight * 2),
                OnValueChange = new OnValueChange(Engine.ListView_OnValueChange)
            };
            slider.SetTag("ListBox", box);
            slider.SetValue(0.0, false);
            toAdd.Children.Add(slider);
            button.SetTag("Scroller", slider);
            button2.SetTag("Scroller", slider);
            toAdd.Children.Add(new GBackground(0xbbc, 0x69, 0x19, 270, 0xba, true));
            toAdd.Children.Add(new GBackground(0xbbc, 0x69, 0x19, 270, 0xd8, true));
            toAdd.Children.Add(new GBackground(0xbbc, 0x69, 0x19, 270, 0xf6, true));
            toAdd.Children.Add(new GLabel("Strength", GetFont(1), Hues.Load(1), 280, 0x38));
            toAdd.Children.Add(new GLabel("Dexterity", GetFont(1), Hues.Load(1), 280, 0x56));
            toAdd.Children.Add(new GLabel("Intelligence", GetFont(1), Hues.Load(1), 280, 0x74));
            int[] numArray = new int[] { 0x39, 0x57, 0x75, 0xc1, 0xdf, 0xfd };
            int[] numArray2 = new int[] { Str, Dex, Int, vSkill1, vSkill2, vSkill3 };
            double[] numArray3 = new double[] { 10.0, 10.0, 10.0, 0.0, 0.0, 0.0 };
            double[] numArray4 = new double[] { 60.0, 60.0, 60.0, 50.0, 50.0, 50.0 };
            double[] numArray5 = new double[] { 80.0, 80.0, 80.0, 100.0, 100.0, 100.0 };
            GSlider[] sliderArray = new GSlider[6];
            GLabel[] labelArray = new GLabel[6];
            for (int j = 0; j < 6; j++)
            {
                toAdd.Children.Add(new GImage(0xd5, 420, numArray[j]));
                toAdd.Children.Add(new GBackground(0xd6, 0x4b, 14, 0x1b1, numArray[j], false));
                toAdd.Children.Add(new GImage(0xd7, 0x1fc, numArray[j]));
                GLabel label = new GLabel(numArray2[j].ToString(), GetFont(1), Hues.Load(1), 380, numArray[j] - 1);
                GSlider slider2 = new GSlider(0xd8, 0x1a8, numArray[j], 0x5d, 14, (double) numArray2[j], numArray3[j], numArray4[j], 1.0) {
                    OnValueChange = new OnValueChange(Engine.UpdateStaticSlider_OnValueChange)
                };
                slider2.SetTag("Static", label);
                slider2.SetTag("Font", GetFont(1));
                slider2.SetTag("Hue", Hues.Load(1));
                slider2.SetTag("Max", numArray5[j]);
                sliderArray[j] = slider2;
                labelArray[j] = label;
                toAdd.Children.Add(label);
                toAdd.Children.Add(slider2);
            }
            for (int k = 0; k < 3; k++)
            {
                string name = string.Format("Slider{0}", k + 1);
                sliderArray[0].SetTag(name, sliderArray[k]);
                sliderArray[1].SetTag(name, sliderArray[k]);
                sliderArray[2].SetTag(name, sliderArray[k]);
                sliderArray[3].SetTag(name, sliderArray[k + 3]);
                sliderArray[4].SetTag(name, sliderArray[k + 3]);
                sliderArray[5].SetTag(name, sliderArray[k + 3]);
            }
            OnClick onClick = new OnClick(Engine.CharSkillBox_OnClick);
            button3 = new GTextButton((iSkill1 == -1) ? "Click Here" : Skills[iSkill1].Name, GetFont(9), Hues.Load(0x76b), Hues.Load(0x961), 0x113, 0xbf, onClick) {
                X = 0x112,
                Y = 0xd1 - button3.Height
            };
            button3.SetTag("List", box);
            button3.SetTag("Skill", iSkill1);
            toAdd.Children.Add(button3);
            button4 = new GTextButton((iSkill2 == -1) ? "Click Here" : Skills[iSkill2].Name, GetFont(9), Hues.Load(0x76b), Hues.Load(0x961), 0x113, 0xbf, onClick) {
                X = 0x112,
                Y = 0xef - button4.Height
            };
            button4.SetTag("List", box);
            button4.SetTag("Skill", iSkill2);
            toAdd.Children.Add(button4);
            button5 = new GTextButton((iSkill3 == -1) ? "Click Here" : Skills[iSkill3].Name, GetFont(9), Hues.Load(0x76b), Hues.Load(0x961), 0x113, 0xbf, onClick) {
                X = 0x112,
                Y = 0x10d - button5.Height
            };
            button5.SetTag("List", box);
            button5.SetTag("Skill", iSkill3);
            toAdd.Children.Add(button5);
            toAdd.Children.Add(new GHotspot(270, 0xba, 0x69, 0x19, button3));
            toAdd.Children.Add(new GHotspot(270, 0xd8, 0x69, 0x19, button4));
            toAdd.Children.Add(new GHotspot(270, 0xf6, 0x69, 0x19, button5));
            Gumps.Desktop.Children.Clear();
            Gumps.Desktop.Children.Add(new GBackground(0x588, ScreenWidth, ScreenHeight, false));
            Gumps.Desktop.Children.Add(new GImage(0x157c, 0, 0));
            Gumps.Desktop.Children.Add(new GImage(0x15a0, 0, 4));
            GButton button6 = new GButton(0x1589, 0x22b, 4, new OnClick(Engine.Quit_OnClick)) {
                Tooltip = new Tooltip(Strings.GetString("Tooltips.Quit"))
            };
            Gumps.Desktop.Children.Add(button6);
            GButton button7 = new GButton(0x15a4, 610, 0x1bd, new OnClick(Engine.CharCreationSkillsArrow_OnClick));
            button7.SetTag("Strength", labelArray[0]);
            button7.SetTag("Dexterity", labelArray[1]);
            button7.SetTag("Intelligence", labelArray[2]);
            button7.SetTag("vSkill1", labelArray[3]);
            button7.SetTag("vSkill2", labelArray[4]);
            button7.SetTag("vSkill3", labelArray[5]);
            button7.SetTag("iSkill1", button3);
            button7.SetTag("iSkill2", button4);
            button7.SetTag("iSkill3", button5);
            Gumps.Desktop.Children.Add(button7);
            Gumps.Desktop.Children.Add(toAdd);
        }

        public static void Skills_OnClick(Gump Sender)
        {
            OpenSkills();
        }

        public static int Smallest(int x, int y)
        {
            if (x < y)
            {
                return x;
            }
            return y;
        }

        public static bool SmartPotion()
        {
            Mobile player = World.Player;
            if (player != null)
            {
                if (!player.Ghost)
                {
                    if (player.Flags[MobileFlag.Poisoned])
                    {
                        if (!UsePotion(PotionType.Orange))
                        {
                            AddTextMessage("You do not have any orange potions!", DefaultFont, Hues.Load(0x22));
                            return false;
                        }
                    }
                    else if ((player.HPCur < player.HPMax) && !UsePotion(PotionType.Yellow))
                    {
                        AddTextMessage("You do not have any yellow potions!", DefaultFont, Hues.Load(0x22));
                        return false;
                    }
                    return true;
                }
                AddTextMessage("A potion can not help you at this point.");
            }
            return false;
        }

        public static void StartPings()
        {
            ClearPings();
            m_PingTimer = new Timer(new OnTick(Engine.Ping_OnTick), 0x1388);
            m_PingTimer.Start(false);
        }

        public static void Status_OnClick(Gump Sender)
        {
            int tag = (int) Sender.GetTag("Serial");
            Mobile mobile = World.FindMobile(tag);
            if (mobile != null)
            {
                mobile.QueryStats();
                mobile.OpenStatus(false);
            }
        }

        public static void StringQueryCancel_OnClick(Gump Sender)
        {
            if ((Sender.HasTag("Dialog") && Sender.HasTag("Serial")) && Sender.HasTag("Type"))
            {
                Gumps.Destroy((Gump) Sender.GetTag("Dialog"));
                Network.Send(new PStringQueryCancel((int) Sender.GetTag("Serial"), (short) Sender.GetTag("Type")));
            }
        }

        public static void StringQueryOkay_OnClick(Gump Sender)
        {
            if ((Sender.HasTag("Dialog") && Sender.HasTag("Serial")) && (Sender.HasTag("Type") && Sender.HasTag("Text")))
            {
                Gumps.Destroy((Gump) Sender.GetTag("Dialog"));
                Network.Send(new PStringQueryResponse((int) Sender.GetTag("Serial"), (short) Sender.GetTag("Type"), ((GTextBox) Sender.GetTag("Text")).String));
            }
        }

        public static string Swap(string str, int c1, int c2)
        {
            if (c1 > c2)
            {
                int num = c2;
                c2 = c1;
                c1 = num;
            }
            if (c1 == c2)
            {
                return str;
            }
            return string.Concat(new object[] { str.Substring(0, c1), str[c2], str.Substring(c1 + 1, (c2 - c1) - 1), str[c1], str.Substring(c2 + 1) });
        }

        public static void Target(object o)
        {
            if (m_TargetHandler != null)
            {
                ITargetHandler targetHandler = m_TargetHandler;
                m_TargetHandler = null;
                targetHandler.OnTarget(o);
                if (m_TargetHandler != targetHandler)
                {
                    m_TargetQueue = null;
                    if (!(o is Mobile) || !((Mobile) o).Player)
                    {
                        m_LastTarget = o;
                    }
                    if (targetHandler is ServerTargetHandler)
                    {
                        ServerTargetHandler handler2 = (ServerTargetHandler) targetHandler;
                        if ((((handler2.Flags & ServerTargetFlags.Harmful) != ServerTargetFlags.None) && (o is Mobile)) && !((Mobile) o).Player)
                        {
                            m_LastHarmTarget = o;
                        }
                        if ((((handler2.Flags & ServerTargetFlags.Beneficial) != ServerTargetFlags.None) && (o is Mobile)) && !((Mobile) o).Player)
                        {
                            m_LastBenTarget = o;
                        }
                    }
                    else if (((targetHandler is NullTargetHandler) && (o is Mobile)) && !((Mobile) o).Player)
                    {
                        m_LastHarmTarget = o;
                    }
                }
                Redraw();
            }
        }

        public static void TargetAquire()
        {
            Mobile player = World.Player;
            if (player != null)
            {
                Mobile[] members = Party.Members;
                ArrayList dataStore = GetDataStore();
                IEnumerator enumerator = World.Mobiles.Values.GetEnumerator();
                MapPackage cache = Map.GetCache();
                int xyRange = 12;
                bool flag = (m_TargetHandler is ServerTargetHandler) && (((ServerTargetHandler) m_TargetHandler).Action == TargetAction.Bola);
                if (flag)
                {
                    xyRange = 8;
                }
                while (enumerator.MoveNext())
                {
                    Mobile current = (Mobile) enumerator.Current;
                    if (((current.Visible && !current.Player) && (!current.m_IsFriend && player.InSquareRange(current, xyRange))) && (((Array.IndexOf(members, current) == -1) && Map.LineOfSight(player, current)) && (!current.Ghost && !current.Bonded)))
                    {
                        if (current.Notoriety == Notoriety.Innocent)
                        {
                            if (player.Notoriety != Notoriety.Murderer)
                            {
                                continue;
                            }
                            int num2 = current.X - cache.CellX;
                            int num3 = current.Y - cache.CellY;
                            if ((((num2 >= 0) && (num2 < Renderer.cellWidth)) && ((num3 >= 0) && (num3 < Renderer.cellHeight))) && cache.landTiles[num2, num3].m_Guarded)
                            {
                                continue;
                            }
                        }
                        else if ((current.Notoriety == Notoriety.Ally) || (current.Notoriety == Notoriety.Vendor))
                        {
                            continue;
                        }
                        if (!flag || (current.FindEquip(Layer.Mount) != null))
                        {
                            dataStore.Add(current);
                        }
                    }
                }
                if (dataStore.Count > 0)
                {
                    dataStore.Sort(TargetSorter.Comparer);
                    Mobile o = (Mobile) dataStore[0];
                    if (m_TargetHandler != null)
                    {
                        Target(o);
                    }
                    else if (World.CharData.QueueTargets)
                    {
                        if ((o != null) && (m_TargetQueue != o))
                        {
                            o.AddTextMessage("", "Target queued.", GetFont(3), Hues.Load(0x59), true);
                        }
                        m_TargetQueue = o;
                    }
                }
                ReleaseDataStore(dataStore);
            }
        }

        public static void TargetGray()
        {
            ArrayList dataStore = GetDataStore();
            IEnumerator enumerator = World.Mobiles.Values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Mobile current = (Mobile) enumerator.Current;
                if ((current.Visible && ((current.Notoriety == Notoriety.Criminal) || (current.Notoriety == Notoriety.Attackable))) && ((!current.m_IsFriend && !current.Player) && World.InRange(current)))
                {
                    dataStore.Add(current);
                }
            }
            if (dataStore.Count > 0)
            {
                dataStore.Sort(TargetSorter.Comparer);
                Target(dataStore[0]);
            }
            ReleaseDataStore(dataStore);
        }

        public static void TargetLast()
        {
            if (m_LastTarget != null)
            {
                if (m_TargetHandler != null)
                {
                    Target(m_LastTarget);
                }
                else if (World.CharData.QueueTargets)
                {
                    Mobile player = World.Player;
                    if ((player != null) && (m_TargetQueue != m_LastTarget))
                    {
                        player.AddTextMessage("", "Last target queued.", GetFont(3), Hues.Load(0x59), true);
                    }
                    m_TargetQueue = m_LastTarget;
                }
            }
        }

        public static void TargetNonFriend()
        {
            Mobile player = World.Player;
            if (player != null)
            {
                Mobile[] members = Party.Members;
                ArrayList dataStore = GetDataStore();
                IEnumerator enumerator = World.Mobiles.Values.GetEnumerator();
                MapPackage cache = Map.GetCache();
                while (enumerator.MoveNext())
                {
                    Mobile current = (Mobile) enumerator.Current;
                    if ((current.Visible && !current.Player) && (!current.m_IsFriend && player.InSquareRange(current, 12)))
                    {
                        if (current.Notoriety == Notoriety.Innocent)
                        {
                            int num = current.X - cache.CellX;
                            int num2 = current.Y - cache.CellY;
                            if (((num >= 0) && (num < Renderer.cellWidth)) && (((num2 >= 0) && (num2 < Renderer.cellHeight)) && cache.landTiles[num, num2].m_Guarded))
                            {
                                continue;
                            }
                        }
                        dataStore.Add(current);
                    }
                }
                if (dataStore.Count > 0)
                {
                    dataStore.Sort(NameSorter.Comparer);
                    Mobile o = (Mobile) dataStore[0];
                    if (m_TargetHandler != null)
                    {
                        Target(o);
                    }
                    else if (World.CharData.QueueTargets)
                    {
                        if ((o != null) && (m_TargetQueue != o))
                        {
                            o.AddTextMessage("", "Target queued.", GetFont(3), Hues.Load(0x59), true);
                        }
                        m_TargetQueue = o;
                    }
                }
                ReleaseDataStore(dataStore);
            }
        }

        public static void TargetNonParty()
        {
            Mobile player = World.Player;
            if (player != null)
            {
                Mobile[] members = Party.Members;
                ArrayList dataStore = GetDataStore();
                IEnumerator enumerator = World.Mobiles.Values.GetEnumerator();
                MapPackage cache = Map.GetCache();
                while (enumerator.MoveNext())
                {
                    Mobile current = (Mobile) enumerator.Current;
                    if ((current.Visible && !current.Player) && (!current.m_IsFriend && player.InSquareRange(current, 12)))
                    {
                        if (current.Notoriety == Notoriety.Innocent)
                        {
                            int num = current.X - cache.CellX;
                            int num2 = current.Y - cache.CellY;
                            if (((num >= 0) && (num < Renderer.cellWidth)) && (((num2 >= 0) && (num2 < Renderer.cellHeight)) && cache.landTiles[num, num2].m_Guarded))
                            {
                                continue;
                            }
                        }
                        bool flag = false;
                        for (int i = 0; !flag && (i < members.Length); i++)
                        {
                            flag = members[i] == current;
                        }
                        if (!flag)
                        {
                            dataStore.Add(current);
                        }
                    }
                }
                if (dataStore.Count > 0)
                {
                    dataStore.Sort(NameSorter.Comparer);
                    Mobile o = (Mobile) dataStore[0];
                    if (m_TargetHandler != null)
                    {
                        Target(o);
                    }
                    else if (World.CharData.QueueTargets)
                    {
                        if ((o != null) && (m_TargetQueue != o))
                        {
                            o.AddTextMessage("", "Target queued.", GetFont(3), Hues.Load(0x59), true);
                        }
                        m_TargetQueue = o;
                    }
                }
                ReleaseDataStore(dataStore);
            }
        }

        public static void TargetNoto(Notoriety noto)
        {
            Mobile player = World.Player;
            if (player != null)
            {
                ArrayList dataStore = GetDataStore();
                IEnumerator enumerator = World.Mobiles.Values.GetEnumerator();
                MapPackage cache = Map.GetCache();
                bool flag = (noto != Notoriety.Innocent) && (noto != Notoriety.Ally);
                while (enumerator.MoveNext())
                {
                    Mobile current = (Mobile) enumerator.Current;
                    if (((current.Visible && (current.Notoriety == noto)) && (!flag || !current.m_IsFriend)) && (!current.Player && player.InSquareRange(current, 12)))
                    {
                        if (current.Notoriety == Notoriety.Innocent)
                        {
                            int num = current.X - cache.CellX;
                            int num2 = current.Y - cache.CellY;
                            if (((num >= 0) && (num < Renderer.cellWidth)) && (((num2 >= 0) && (num2 < Renderer.cellHeight)) && cache.landTiles[num, num2].m_Guarded))
                            {
                                continue;
                            }
                        }
                        dataStore.Add(current);
                    }
                }
                if (dataStore.Count > 0)
                {
                    dataStore.Sort(TargetSorter.Comparer);
                    Mobile o = (Mobile) dataStore[0];
                    if (m_TargetHandler != null)
                    {
                        Target(o);
                    }
                    else if (World.CharData.QueueTargets)
                    {
                        if ((o != null) && (m_TargetQueue != o))
                        {
                            o.AddTextMessage("", "Target queued.", GetFont(3), Hues.Load(0x59), true);
                        }
                        m_TargetQueue = o;
                    }
                }
                ReleaseDataStore(dataStore);
            }
        }

        public static void TargetSelf()
        {
            if (m_TargetHandler != null)
            {
                Target(World.Player);
            }
            else if (World.CharData.QueueTargets)
            {
                Mobile player = World.Player;
                if ((player != null) && (m_TargetQueue != player))
                {
                    player.AddTextMessage("", "Target self queued.", GetFont(3), Hues.Load(0x59), true);
                }
                m_TargetQueue = player;
            }
        }

        public static void TargetSmart()
        {
            if (m_TargetHandler is ServerTargetHandler)
            {
                ServerTargetHandler targetHandler = (ServerTargetHandler) m_TargetHandler;
                if ((targetHandler.Flags & ServerTargetFlags.Harmful) != ServerTargetFlags.None)
                {
                    if (m_LastHarmTarget != null)
                    {
                        Target(m_LastHarmTarget);
                    }
                }
                else if ((targetHandler.Flags & ServerTargetFlags.Beneficial) != ServerTargetFlags.None)
                {
                    if (m_LastBenTarget != null)
                    {
                        Target(m_LastBenTarget);
                    }
                }
                else
                {
                    TargetLast();
                }
            }
            else if ((m_LastTarget != null) && (m_TargetHandler != null))
            {
                TargetLast();
            }
            else if (World.CharData.QueueTargets && !m_StopQueue)
            {
                Mobile player = World.Player;
                if ((player != null) && (m_TargetQueue != m_TargetSmartObj))
                {
                    player.AddTextMessage("", "Smart last target queued.", GetFont(3), Hues.Load(0x59), true);
                }
                m_TargetQueue = m_TargetSmartObj;
            }
        }

        private static void TextHue_OnClick(Gump g)
        {
            if (!m_DyeWindowOpen)
            {
                OpenDyeWindow(new OnClick(Engine.TextHue_OnHueSelect), g);
                m_DyeWindowOpen = true;
            }
        }

        private static void TextHue_OnHueSelect(Gump g)
        {
            World.CharData.TextHue = ((GHuePicker) g.GetTag("Hue Picker")).Hue;
            Renderer.SetText(m_Text);
            ((GTextButton) g.GetTag("Button")).DefaultHue = Hues.Load(((GHuePicker) g.GetTag("Hue Picker")).Hue);
            Gumps.Destroy(g.Parent);
            m_DyeWindowOpen = false;
        }

        private static void TickTimers()
        {
            int count = m_Timers.Count;
            int index = 0;
            while (index < count)
            {
                Timer timer = (Timer) m_Timers[index];
                if (!timer.Tick())
                {
                    m_Timers.RemoveAt(index);
                    count = m_Timers.Count;
                }
                else
                {
                    index++;
                }
            }
        }

        private static void TimeRefresh_OnTick(Timer t)
        {
            int tag = (int) t.GetTag("Frames");
            double num2 = tag;
            double d = 0.0;
            double num4 = 6.2831853071795862 / num2;
            double num5 = ScreenWidth / 2;
            double num6 = ScreenHeight / 2;
            double num7 = num6 * 0.75;
            int xMouse = m_xMouse;
            int yMouse = m_yMouse;
            Cursor.Hourglass = true;
            m_SetTicks = false;
            double dTicks = Engine.dTicks;
            while (--tag >= 0)
            {
                m_xMouse = (int) (num5 + (num7 * Math.Cos(d)));
                m_yMouse = (int) (num6 - (num7 * Math.Sin(d)));
                d += num4;
                Renderer.Draw();
            }
            m_SetTicks = false;
            double num11 = Engine.dTicks;
            Cursor.Hourglass = false;
            AddTextMessage(string.Format("Time Refresh: {0} frames in {1:F2} seconds: {2:F2} FPS", num2, (num11 - dTicks) * 0.001, num2 / ((num11 - dTicks) * 0.001)));
            m_xMouse = xMouse;
            m_yMouse = yMouse;
        }

        public static void Unlock()
        {
            m_Locked = false;
        }

        public static void UpdateSmartLoginMenu()
        {
            GMainMenu g = Gumps.FindGumpByGUID("Smart Login") as GMainMenu;
            if (g != null)
            {
                Gump parent = g.Parent;
                Gumps.Destroy(g);
                parent.Children.Add(BuildSmartLoginMenu());
            }
        }

        public static void UpdateStaticSlider_OnValueChange(double Value, double Old, Gump Sender)
        {
            if ((((Sender.HasTag("Static") && Sender.HasTag("Font")) && (Sender.HasTag("Hue") && Sender.HasTag("Slider1"))) && (Sender.HasTag("Slider2") && Sender.HasTag("Slider3"))) && Sender.HasTag("Max"))
            {
                GLabel tag = (GLabel) Sender.GetTag("Static");
                IFont font = (IFont) Sender.GetTag("Font");
                IHue hue = (IHue) Sender.GetTag("Hue");
                GSlider[] sliderArray = new GSlider[3];
                for (int i = 0; i < 3; i++)
                {
                    sliderArray[i] = (GSlider) Sender.GetTag(string.Format("Slider{0}", i + 1));
                }
                double num2 = (double) Sender.GetTag("Max");
                if (((tag != null) && (font != null)) && (hue != null))
                {
                    GSlider slider = null;
                    GSlider slider2 = null;
                    GSlider slider3 = null;
                    double num3 = -1000.0;
                    double num4 = 1000.0;
                    for (int j = 0; j < 3; j++)
                    {
                        if ((sliderArray[j] != Sender) && (sliderArray[j].GetValue() < num4))
                        {
                            num4 = sliderArray[j].GetValue();
                            slider2 = sliderArray[j];
                        }
                        else if (sliderArray[j] == Sender)
                        {
                            slider3 = sliderArray[j];
                        }
                    }
                    for (int k = 0; k < 3; k++)
                    {
                        if (((sliderArray[k] != Sender) && (sliderArray[k] != slider2)) && (sliderArray[k].GetValue() > num3))
                        {
                            num3 = sliderArray[k].GetValue();
                            slider = sliderArray[k];
                        }
                    }
                    if (((slider3 == null) || (slider == null)) || (slider2 == null))
                    {
                        tag.Text = Value.ToString();
                    }
                    else
                    {
                        slider2.SetValue(num2 - (Value + slider.GetValue()), false);
                        slider.SetValue(num2 - (Value + slider2.GetValue()), false);
                        slider3.SetValue(num2 - (slider2.GetValue() + slider.GetValue()), false);
                        Value = slider3.GetValue();
                        slider2.SetValue(num2 - (Value + slider.GetValue()), false);
                        slider.SetValue(num2 - (Value + slider2.GetValue()), false);
                        slider3.SetValue(num2 - (slider2.GetValue() + slider.GetValue()), false);
                        Value = slider3.GetValue();
                        tag.Text = slider3.GetValue().ToString();
                        if (((slider2.GetValue() + slider.GetValue()) + slider3.GetValue()) != num2)
                        {
                            tag.Hue = Hues.Load(0x66d);
                        }
                        else
                        {
                            tag.Hue = hue;
                        }
                        ((GLabel) slider2.GetTag("Static")).Text = slider2.GetValue().ToString();
                        ((GLabel) slider.GetTag("Static")).Text = slider.GetValue().ToString();
                    }
                }
            }
        }

        public static void URLButton_OnClick(Gump Sender)
        {
            if (Sender.HasTag("URL"))
            {
                OpenBrowser((string) Sender.GetTag("URL"));
            }
        }

        public static bool UsePotion(PotionType type)
        {
            Mobile player = World.Player;
            if ((player != null) && !player.Ghost)
            {
                Item backpack = player.Backpack;
                if (backpack != null)
                {
                    Item[] itemArray = backpack.FindItems(new ItemIDValidator(new int[] { 0xf06 + type }));
                    int index = 0;
                    while (index < itemArray.Length)
                    {
                        Item item2 = itemArray[index];
                        return item2.Use();
                    }
                }
            }
            return false;
        }

        public static void ValidateHandlers()
        {
            for (int i = 0; i < PacketHandlers.m_Handlers.Length; i++)
            {
                PacketHandler handler = PacketHandlers.m_Handlers[i];
                if (((handler != null) && (handler.Callback != null)) && (handler.Callback.Method.DeclaringType.Assembly != Assembly.GetCallingAssembly()))
                {
                    handler.Length = 0x7b;
                    m_QPF *= 8.0;
                }
            }
            if ((PacketHandlers.m_Handlers[0x4e] == null) || (PacketHandlers.m_Handlers[0x4e].Callback != new PacketCallback(PacketHandlers.Light_Personal)))
            {
                m_QPF *= 8.0;
            }
            if ((PacketHandlers.m_Handlers[0x4f] == null) || (PacketHandlers.m_Handlers[0x4f].Callback != new PacketCallback(PacketHandlers.Light_Global)))
            {
                m_QPF *= 8.0;
            }
        }

        public static void WalkTimeout(int Serial)
        {
            m_WalkTimeout.Add(Serial);
        }

        public static void WantDirectory(string Target)
        {
            string path = FileManager.BasePath(Target);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static void WhisperHue_OnClick(Gump g)
        {
            if (!m_DyeWindowOpen)
            {
                OpenDyeWindow(new OnClick(Engine.WhisperHue_OnHueSelect), g);
                m_DyeWindowOpen = true;
            }
        }

        private static void WhisperHue_OnHueSelect(Gump g)
        {
            World.CharData.WhisperHue = ((GHuePicker) g.GetTag("Hue Picker")).Hue;
            Renderer.SetText(m_Text);
            ((GTextButton) g.GetTag("Button")).DefaultHue = Hues.Load(((GHuePicker) g.GetTag("Hue Picker")).Hue);
            Gumps.Destroy(g.Parent);
            m_DyeWindowOpen = false;
        }

        public static string WrapText(string text, int width, IFont f)
        {
            WrapKey key = new WrapKey(text, width);
            object obj2 = f.WrapCache[key];
            if (obj2 != null)
            {
                return (string) obj2;
            }
            if (f.GetStringWidth(text) <= width)
            {
                f.WrapCache.Add(key, text);
                return text;
            }
            string[] strArray = text.Split(new char[] { ' ' });
            StringBuilder builder = new StringBuilder();
            ArrayList dataStore = GetDataStore();
            for (int i = 0; i < strArray.Length; i++)
            {
                if (f.GetStringWidth(builder.ToString() + strArray[i]) > width)
                {
                    if (f.GetStringWidth(strArray[i]) > width)
                    {
                        builder.Append(strArray[i]);
                        while ((builder.Length > 1) && (f.GetStringWidth(builder.ToString()) > width))
                        {
                            StringBuilder builder2 = new StringBuilder();
                            builder2.Append(builder[0]);
                            for (int k = 1; k < builder.Length; k++)
                            {
                                if (f.GetStringWidth(builder2.ToString() + builder[k]) > width)
                                {
                                    dataStore.Add(builder2);
                                    builder = new StringBuilder(builder.ToString().Substring(builder2.Length));
                                    continue;
                                }
                                builder2.Append(builder[k]);
                            }
                        }
                        if (i < (strArray.Length - 1))
                        {
                            builder.Append(' ');
                        }
                        continue;
                    }
                    if (builder.Length > 0)
                    {
                        dataStore.Add(builder);
                    }
                    builder = new StringBuilder(strArray[i]);
                    if (i < (strArray.Length - 1))
                    {
                        builder.Append(' ');
                    }
                    continue;
                }
                builder.Append(strArray[i]);
                if (i < (strArray.Length - 1))
                {
                    builder.Append(' ');
                }
            }
            if (builder.Length > 0)
            {
                while ((builder.Length > 1) && (f.GetStringWidth(builder.ToString()) > width))
                {
                    StringBuilder builder3 = new StringBuilder();
                    builder3.Append(builder[0]);
                    for (int m = 1; m < builder.Length; m++)
                    {
                        if (f.GetStringWidth(builder3.ToString() + builder[m]) > width)
                        {
                            dataStore.Add(builder3);
                            builder = new StringBuilder(builder.ToString().Substring(builder3.Length));
                            continue;
                        }
                        builder3.Append(builder[m]);
                    }
                }
                if (builder.Length > 0)
                {
                    dataStore.Add(builder);
                }
            }
            StringBuilder builder4 = new StringBuilder();
            int count = dataStore.Count;
            for (int j = 0; j < count; j++)
            {
                builder4.Append(((StringBuilder) dataStore[j]).ToString());
                if (j < (count - 1))
                {
                    builder4.Append('\n');
                }
            }
            string str = builder4.ToString();
            f.WrapCache.Add(key, str);
            ReleaseDataStore(dataStore);
            return str;
        }

        private static void YellHue_OnClick(Gump g)
        {
            if (!m_DyeWindowOpen)
            {
                OpenDyeWindow(new OnClick(Engine.YellHue_OnHueSelect), g);
                m_DyeWindowOpen = true;
            }
        }

        private static void YellHue_OnHueSelect(Gump g)
        {
            World.CharData.YellHue = ((GHuePicker) g.GetTag("Hue Picker")).Hue;
            Renderer.SetText(m_Text);
            ((GTextButton) g.GetTag("Button")).DefaultHue = Hues.Load(((GHuePicker) g.GetTag("Hue Picker")).Hue);
            Gumps.Destroy(g.Parent);
            m_DyeWindowOpen = false;
        }

        public static int CharacterCount
        {
            get
            {
                return m_CharacterCount;
            }
            set
            {
                m_CharacterCount = value;
            }
        }

        public static string[] CharacterNames
        {
            get
            {
                return m_CharacterNames;
            }
            set
            {
                m_CharacterNames = value;
            }
        }

        public static Client.ContainerBoundsTable ContainerBoundsTable
        {
            get
            {
                if (m_ContainerBoundsTable == null)
                {
                    m_ContainerBoundsTable = new Client.ContainerBoundsTable();
                }
                return m_ContainerBoundsTable;
            }
        }

        public static IFont DefaultFont
        {
            get
            {
                return m_DefaultFont;
            }
        }

        public static IHue DefaultHue
        {
            get
            {
                return m_DefaultHue;
            }
        }

        public static ArrayList Doors
        {
            get
            {
                return m_Doors;
            }
        }

        public static double dTicks
        {
            get
            {
                if (!m_SetTicks)
                {
                    QueryPerformanceCounter(ref m_QPC);
                    double qPC = m_QPC;
                    qPC /= m_QPF;
                    qPC = qPC % 2147483648;
                    m_Ticks = (int) (qPC + 0.5);
                    m_dTicks = qPC;
                    m_SetTicks = true;
                }
                return m_dTicks;
            }
        }

        public static Client.Effects Effects
        {
            get
            {
                return m_Effects;
            }
        }

        public static Client.Features Features
        {
            get
            {
                if (m_Features == null)
                {
                    m_Features = new Client.Features();
                }
                return m_Features;
            }
        }

        public static Client.FileManager FileManager
        {
            get
            {
                return m_FileManager;
            }
        }

        public static bool FPS
        {
            get
            {
                return Renderer.DrawFPS;
            }
            set
            {
                Renderer.DrawFPS = value;
            }
        }

        public static bool GMPrivs
        {
            get
            {
                return ((World.Player != null) && (World.Player.Body == 0x3db));
            }
        }

        public static bool Grid
        {
            get
            {
                return Renderer.DrawGrid;
            }
            set
            {
                Renderer.DrawGrid = value;
            }
        }

        public static Client.ItemArt ItemArt
        {
            get
            {
                if (m_ItemArt == null)
                {
                    m_ItemArt = new Client.ItemArt();
                }
                return m_ItemArt;
            }
        }

        public static float ItemDuration
        {
            get
            {
                return m_ItemDuration;
            }
        }

        public static Client.LandArt LandArt
        {
            get
            {
                if (m_LandArt == null)
                {
                    m_LandArt = new Client.LandArt();
                }
                return m_LandArt;
            }
        }

        public static Server LastServer
        {
            get
            {
                return m_LastServer;
            }
            set
            {
                m_LastServer = value;
            }
        }

        public static Client.MidiTable MidiTable
        {
            get
            {
                if (m_MidiTable == null)
                {
                    m_MidiTable = new Client.MidiTable();
                }
                return m_MidiTable;
            }
        }

        public static bool MiniHealth
        {
            get
            {
                return Renderer.MiniHealth;
            }
            set
            {
                Renderer.MiniHealth = value;
            }
        }

        public static float MobileDuration
        {
            get
            {
                return m_MobileDuration;
            }
        }

        public static Client.Multis Multis
        {
            get
            {
                if (m_Multis == null)
                {
                    m_Multis = new Client.Multis();
                }
                return m_Multis;
            }
        }

        public static int Ping
        {
            get
            {
                return m_Ping;
            }
        }

        public static IPrompt Prompt
        {
            get
            {
                return m_Prompt;
            }
            set
            {
                if (m_Prompt != value)
                {
                    if ((m_Prompt != null) && (value != null))
                    {
                        m_Prompt.OnCancel(PromptCancelType.NewPrompt);
                    }
                    m_Prompt = value;
                }
            }
        }

        public static System.Random Random
        {
            get
            {
                if (m_Random == null)
                {
                    m_Random = new System.Random();
                }
                return m_Random;
            }
        }

        public static bool RealGMPrivs
        {
            get
            {
                Mobile player = World.Player;
                if (player == null)
                {
                    return false;
                }
                return ((player.Body == 0x3db) || player.Flags[MobileFlag.YellowHits]);
            }
        }

        public static Client.ServerFeatures ServerFeatures
        {
            get
            {
                if (m_ServerFeatures == null)
                {
                    m_ServerFeatures = new Client.ServerFeatures();
                }
                return m_ServerFeatures;
            }
        }

        public static Server[] Servers
        {
            get
            {
                return m_Servers;
            }
            set
            {
                m_Servers = value;
            }
        }

        public static Client.Skills Skills
        {
            get
            {
                if (m_Skills == null)
                {
                    Debug.TimeBlock("Initializing Skills");
                    m_Skills = new Client.Skills();
                    Debug.EndBlock();
                }
                return m_Skills;
            }
        }

        public static Client.Sounds Sounds
        {
            get
            {
                if (m_Sounds == null)
                {
                    Debug.TimeBlock("Initializing Sounds");
                    m_Sounds = new Client.Sounds();
                    Debug.EndBlock();
                }
                return m_Sounds;
            }
        }

        public static float SystemDuration
        {
            get
            {
                return m_SystemDuration;
            }
        }

        public static ITargetHandler TargetHandler
        {
            get
            {
                return m_TargetHandler;
            }
            set
            {
                if (((m_TargetHandler != null) && (m_TargetHandler != value)) && (value != null))
                {
                    m_TargetHandler.OnCancel(TargetCancelType.NewTarget);
                }
                m_TargetHandler = value;
                if (!m_TargetRecurse)
                {
                    m_TargetRecurse = true;
                    if (m_TargetQueue != null)
                    {
                        if (m_TargetQueue == m_TargetSmartObj)
                        {
                            TargetSmart();
                        }
                        else
                        {
                            Target(m_TargetQueue);
                        }
                    }
                    m_TargetRecurse = false;
                }
            }
        }

        public static Client.TextureArt TextureArt
        {
            get
            {
                if (m_TextureArt == null)
                {
                    m_TextureArt = new Client.TextureArt();
                }
                return m_TextureArt;
            }
        }

        public static int Ticks
        {
            get
            {
                if (!m_SetTicks)
                {
                    QueryPerformanceCounter(ref m_QPC);
                    double qPC = m_QPC;
                    qPC /= m_QPF;
                    qPC = qPC % 2147483648;
                    m_Ticks = (int) (qPC + 0.5);
                    m_dTicks = qPC;
                    m_SetTicks = true;
                }
                return m_Ticks;
            }
        }

        public static bool Warmode
        {
            get
            {
                Mobile player = World.Player;
                return ((player != null) && player.Flags[MobileFlag.Warmode]);
            }
            set
            {
                if (World.Player != null)
                {
                    Network.Send(new PSetWarMode(value, 0x20, 0));
                    if (!value)
                    {
                        m_Highlight = null;
                    }
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct ConvertStruct
        {
            public char m_From;
            public char m_To;
            public int m_Count;
            public ConvertStruct(char from, char to, int count)
            {
                this.m_From = from;
                this.m_To = to;
                this.m_Count = count;
            }
        }

        private class DictionaryComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                DictionaryEntry entry = (DictionaryEntry) x;
                DictionaryEntry entry2 = (DictionaryEntry) y;
                return (((int) entry2.Key) - ((int) entry.Key));
            }
        }

        private class DisplayModeComparer : IComparer
        {
            private Format m_WantFormat;
            private int m_WantHeight;
            private int m_WantWidth;

            public DisplayModeComparer(int w, int h, Format f)
            {
                this.m_WantWidth = w;
                this.m_WantHeight = h;
                this.m_WantFormat = f;
            }

            public unsafe int Compare(object x, object y)
            {
                DisplayMode mode = *((DisplayMode*) x);
                DisplayMode mode2 = *((DisplayMode*) y);
                int introduced6 = mode.get_Width();
                int num = Math.Abs((int) ((introduced6 * mode.get_Height()) - (this.m_WantWidth * this.m_WantHeight)));
                int introduced7 = mode2.get_Width();
                int num2 = Math.Abs((int) ((introduced7 * mode2.get_Height()) - (this.m_WantWidth * this.m_WantHeight)));
                int num3 = num - num2;
                if (num3 != 0)
                {
                    return num3;
                }
                if (mode.get_Format() == this.m_WantFormat)
                {
                    num = 0;
                }
                else if (mode.get_Format() == 0x19)
                {
                    num = 1;
                }
                else if (mode.get_Format() == 0x17)
                {
                    num = 2;
                }
                else
                {
                    num = 3;
                }
                if (mode2.get_Format() == this.m_WantFormat)
                {
                    num2 = 0;
                }
                else if (mode2.get_Format() == 0x19)
                {
                    num2 = 1;
                }
                else if (mode2.get_Format() == 0x17)
                {
                    num2 = 2;
                }
                else
                {
                    num2 = 3;
                }
                return (num - num2);
            }
        }

        private class QamEntry
        {
            public int m_Amount;
            public Item m_Item;
            public int m_Target;
            public int m_X;
            public int m_Y;
            public int m_Z;

            public QamEntry(Item item, int amount, int x, int y, int z, int target)
            {
                this.m_Item = item;
                this.m_Amount = amount;
                this.m_X = x;
                this.m_Y = y;
                this.m_Z = z;
                this.m_Target = target;
            }
        }
    }
}

