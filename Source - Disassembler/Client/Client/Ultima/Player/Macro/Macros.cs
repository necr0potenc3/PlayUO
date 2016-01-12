namespace Client
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Windows.Forms;

    public class Macros
    {
        private static ArrayList m_List;
        private static ArrayList m_Running;

        public static void Cleanup()
        {
            for (int i = 0; i < m_List.Count; i++)
            {
                Macro macro = (Macro)m_List[i];
                if (macro.Actions.Length == 0)
                {
                    m_List.RemoveAt(i--);
                }
            }
        }

        public static bool Exists(string filename)
        {
            return File.Exists(Engine.FileManager.BasePath(string.Format("Data/Plugins/Macros/{0}.txt", filename)));
        }

        public static string GetMobilePath(Mobile mob)
        {
            string serverName = Engine.m_ServerName;
            if (serverName == null)
            {
                serverName = "";
            }
            int num = serverName.GetHashCode() ^ mob.Serial;
            return num.ToString("X8");
        }

        public static void Load()
        {
            Mobile player = World.Player;
            if ((player != null) && Exists(GetMobilePath(player)))
            {
                Load(GetMobilePath(player));
            }
            else
            {
                Load("Macros");
            }
        }

        public static void Load(string filename)
        {
            m_List = new ArrayList();
            m_Running = new ArrayList();
            string path = Engine.FileManager.BasePath(string.Format("Data/Plugins/Macros/{0}.txt", filename));
            if (File.Exists(path))
            {
                int num = 0;
                using (StreamReader reader = new StreamReader(path))
                {
                    while (true)
                    {
                        string str4;
                        string line = ReadLine(reader);
                        if (line == null)
                        {
                            break;
                        }
                        if (line.Length != 5)
                        {
                            Skip(line, reader);
                            break;
                        }
                        string[] strArray = line.Split(new char[] { ' ' });
                        if (strArray.Length != 3)
                        {
                            Skip(line, reader);
                            break;
                        }
                        bool flag = true;
                        for (int i = 0; flag && (i < strArray.Length); i++)
                        {
                            flag = (strArray[i] == "0") || (strArray[i] == "1");
                        }
                        if (!flag)
                        {
                            Skip(line, reader);
                            break;
                        }
                        Keys none = Keys.None;
                        if (strArray[0] != "0")
                        {
                            none |= Keys.Control;
                        }
                        if (strArray[1] != "0")
                        {
                            none |= Keys.Alt;
                        }
                        if (strArray[2] != "0")
                        {
                            none |= Keys.Shift;
                        }
                        string str3 = ReadLine(reader);
                        if (str3 == null)
                        {
                            break;
                        }
                        Keys key = ~Keys.None;
                        switch (str3)
                        {
                            case "WheelUp":
                            case "Wheel Up":
                                key = (Keys)0x11000;
                                break;

                            case "WheelDown":
                            case "Wheel Down":
                                key = (Keys)0x11001;
                                break;

                            case "WheelPress":
                            case "Wheel Press":
                                key = (Keys)0x11002;
                                break;

                            default:
                                try
                                {
                                    key = (Keys)Enum.Parse(typeof(Keys), str3, true);
                                }
                                catch
                                {
                                }
                                break;
                        }
                        if (key == ~Keys.None)
                        {
                            Skip(str3, reader);
                            break;
                        }
                        ArrayList dataStore = Engine.GetDataStore();
                        while ((str4 = ReadLine(reader)) != null)
                        {
                            if (str4.StartsWith("#"))
                            {
                                break;
                            }
                            Client.Action action = new Client.Action(str4);
                            if (action.Handler == null)
                            {
                                Debug.Trace("Bad macro action: {0}", str4);
                            }
                            dataStore.Add(action);
                        }
                        Macro macro = new Macro(key, none, (Client.Action[])dataStore.ToArray(typeof(Client.Action)));
                        macro.FileIndex = num++;
                        m_List.Add(macro);
                        Engine.ReleaseDataStore(dataStore);
                    }
                    m_List.Sort();
                }
            }
        }

        public static string ReadLine(StreamReader ip)
        {
            string str;
            while ((str = ip.ReadLine()) != null)
            {
                str = str.Trim();
                if ((str.Length != 0) && !str.StartsWith(";"))
                {
                    return str;
                }
            }
            return str;
        }

        public static void Save()
        {
            Mobile player = World.Player;
            if ((player != null) && Exists(GetMobilePath(player)))
            {
                Save(player, GetMobilePath(player));
            }
            else
            {
                Save(null, "Macros");
            }
        }

        public static void Save(Mobile who, string filename)
        {
            if (m_List != null)
            {
                Engine.WantDirectory("Data/Plugins/Macros");
                using (StreamWriter writer = new StreamWriter(Engine.FileManager.BasePath(string.Format("Data/Plugins/Macros/{0}.txt", filename))))
                {
                    if (who == null)
                    {
                        writer.WriteLine("; Default macro definitions file");
                    }
                    else
                    {
                        writer.WriteLine("; Macro definition file for {0} on {1}", who.Name, Engine.m_ServerName);
                    }
                    writer.WriteLine("; Format:");
                    writer.WriteLine("; Control Alt Shift");
                    writer.WriteLine("; Key");
                    writer.WriteLine("; [list]");
                    writer.WriteLine("; <name> [param]");
                    writer.WriteLine("; #####");
                    writer.WriteLine();
                    Cleanup();
                    for (int i = 0; i < m_List.Count; i++)
                    {
                        Macro macro = (Macro)m_List[i];
                        writer.WriteLine("{0} {1} {2}", macro.Control ? "1" : "0", macro.Alt ? "1" : "0", macro.Shift ? "1" : "0");
                        switch (macro.Key)
                        {
                            case (Keys)0x11000:
                                writer.WriteLine("Wheel Up");
                                break;

                            case (Keys)0x11001:
                                writer.WriteLine("Wheel Down");
                                break;

                            case (Keys)0x11002:
                                writer.WriteLine("Wheel Press");
                                break;

                            default:
                                writer.WriteLine(macro.Key);
                                break;
                        }
                        for (int j = 0; j < macro.Actions.Length; j++)
                        {
                            Client.Action action = macro.Actions[j];
                            if (action.Handler == null)
                            {
                                writer.WriteLine(action.Line);
                            }
                            else if (action.Param.Length > 0)
                            {
                                writer.WriteLine("{0} {1}", action.Handler.Action, action.Param);
                            }
                            else
                            {
                                writer.WriteLine(action.Handler.Action);
                            }
                        }
                        if (i == (m_List.Count - 1))
                        {
                            writer.Write("#####");
                        }
                        else
                        {
                            writer.WriteLine("#####");
                        }
                    }
                }
            }
        }

        public static void Skip(string line, StreamReader ip)
        {
            Debug.Trace("Skipping improperly formatted line in macros.txt: {0}", line);
            do
            {
                line = line.Trim();
            }
            while (!line.StartsWith("#") && ((line = ip.ReadLine()) != null));
        }

        public static void Slice()
        {
            if (m_Running != null)
            {
                for (int i = 0; i < m_Running.Count; i++)
                {
                    Macro macro = (Macro)m_Running[i];
                    if (!macro.Slice())
                    {
                        m_Running.RemoveAt(i--);
                    }
                }
            }
        }

        public static bool Start(Keys key)
        {
            for (int i = 0; i < m_List.Count; i++)
            {
                Macro macro = (Macro)m_List[i];
                if (macro.CheckKey(key))
                {
                    if (macro.Running)
                    {
                        macro.Stop();
                    }
                    macro.Start();
                    return true;
                }
            }
            return false;
        }

        public static void StopAll()
        {
            if (m_Running != null)
            {
                for (int i = m_Running.Count - 1; i >= 0; i--)
                {
                    if (i < m_Running.Count)
                    {
                        ((Macro)m_Running[i]).Stop();
                    }
                }
                m_Running.Clear();
            }
        }

        public static ArrayList List
        {
            get
            {
                if (m_List == null)
                {
                    Load();
                }
                return m_List;
            }
        }

        public static ArrayList Running
        {
            get
            {
                if (m_Running == null)
                {
                    Load();
                }
                return m_Running;
            }
        }
    }
}