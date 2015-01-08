namespace Config
{
    using System;
    using System.Collections;
    using System.IO;

    public class NewConfig
    {
        private static Hashtable m_Entries = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);

        static NewConfig()
        {
            AddValue("ScreenSize", "Screen Size", "Screen size.", typeof(string), "640x480");
            AddValue("GameSize", "Game Size", "Game size.", typeof(string), "640x480");
            AddValue("FullScreen", "Full Screen", "Whether or not the client should be in fullscreen.", typeof(bool), false);
            AddValue("ServerHost", "Server Host", "Login server host name or IP address.", typeof(string), "127.0.0.1");
            AddValue("ServerPort", "Server Port", "Login server port.", typeof(int), 0xa21);
            AddValue("Username", "Username", "The last used account name.", typeof(string), "");
            AddValue("Password", "Password", "The last used account password.", typeof(string), "");
            AddValue("PlayMusic", "Play Music", "Play music?", typeof(bool), true);
            AddValue("SmoothWalk", "Smooth Movement", "Smooth movement?", typeof(bool), true);
            AddValue("EncryptionFix", "Encryption Fix", "Some older servers require this to log in.", typeof(bool), false);
            AddValue("IncomingFix", "Incoming Fix", "Enabling this may fix movement issues with some Wolfpack servers.", typeof(bool), false);
            AddValue("OldMovement", "Old Movement", "Enabling this may fix movement issues with older servers.", typeof(bool), false);
            AddValue("OldCharCreate", "Old Char Create", "Enabling this may fix character creation with older servers.", typeof(bool), false);
            AddValue("EncodeSpeech", "Encode Speech", "Disabling this may fix speech issues with older servers.", typeof(bool), true);
            AddValue("SendUpdateRange", "Send Update Range", "Disabling this may fix packet errors with older servers.", typeof(bool), true);
            AddValue("LastServerID", "Last Server", "The last played server.", typeof(int), -1);
            AddValue("ExtendProtocol", "Extend Protocol", "Enabling this enables advanced featuers on supporting servers.", typeof(bool), false);
            Load();
            Save();
        }

        public static void AddValue(string name, string friendlyName, string comment, Type type, object defaultValue)
        {
            m_Entries[name] = new ConfigEntry(name, friendlyName, comment, type, defaultValue);
        }

        public static ConfigEntry[] GetEntries()
        {
            ConfigEntry[] array = new ConfigEntry[m_Entries.Values.Count];
            m_Entries.Values.CopyTo(array, 0);
            Array.Sort(array);
            return array;
        }

        public static ConfigEntry GetEntry(string name)
        {
            return (ConfigEntry) m_Entries[name];
        }

        public static object GetValue(string name)
        {
            return GetEntry(name).Value;
        }

        public static void Load()
        {
            if (File.Exists("Client.cfg"))
            {
                using (StreamReader reader = new StreamReader("Client.cfg"))
                {
                    string str;
                    while ((str = reader.ReadLine()) != null)
                    {
                        str = str.Trim();
                        if ((str.Length > 0) && (str[0] != '#'))
                        {
                            int index = str.IndexOf('=');
                            if (index > 0)
                            {
                                string str2 = str.Substring(0, index);
                                string str3 = str.Substring(index + 1);
                                ConfigEntry entry = (ConfigEntry) m_Entries[str2.Trim()];
                                if (entry != null)
                                {
                                    if (entry.Type == typeof(int))
                                    {
                                        try
                                        {
                                            entry.Value = Convert.ToInt32(str3.Trim());
                                        }
                                        catch
                                        {
                                        }
                                    }
                                    else
                                    {
                                        if (entry.Type == typeof(bool))
                                        {
                                            switch (str3.Trim().ToLower())
                                            {
                                                case "true":
                                                case "yes":
                                                case "on":
                                                case "1":
                                                    entry.Value = true;
                                                    break;

                                                case "false":
                                                case "off":
                                                case "no":
                                                case "0":
                                                    entry.Value = false;
                                                    break;
                                            }
                                            continue;
                                        }
                                        if (entry.Type == typeof(string))
                                        {
                                            entry.Value = str3;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void Save()
        {
            using (StreamWriter writer = new StreamWriter("Client.cfg", false))
            {
                foreach (ConfigEntry entry in GetEntries())
                {
                    writer.WriteLine("{0}={1}", entry.Name, entry.Value);
                }
            }
        }

        public static void SetValue(string name, object value)
        {
            GetEntry(name).Value = value;
        }
    }
}

