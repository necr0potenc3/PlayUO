namespace Client
{
    using System;
    using System.Collections;
    using System.IO;

    public class NewConfig
    {
        private static Hashtable m_Entries = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);

        static NewConfig()
        {
            AddValue("ScreenSize", typeof(string), "640x480");
            AddValue("GameSize", typeof(string), "640x480");
            AddValue("FullScreen", typeof(bool), false);
            AddValue("ServerHost", typeof(string), "127.0.0.1");
            AddValue("ServerPort", typeof(int), 0xa21);
            AddValue("Username", typeof(string), "");
            AddValue("Password", typeof(string), "");
            AddValue("PlayMusic", typeof(bool), true);
            AddValue("SmoothWalk", typeof(bool), true);
            AddValue("EncryptionFix", typeof(bool), false);
            AddValue("IncomingFix", typeof(bool), false);
            AddValue("OldMovement", typeof(bool), false);
            AddValue("OldCharCreate", typeof(bool), false);
            AddValue("EncodeSpeech", typeof(bool), true);
            AddValue("SendUpdateRange", typeof(bool), true);
            AddValue("LastServerID", typeof(int), -1);
            AddValue("ExtendProtocol", typeof(bool), false);
            Load();
        }

        public static void AddValue(string name, Type type, object defaultValue)
        {
            m_Entries[name] = new ConfigEntry(name, type, defaultValue);
        }

        public static object GetValue(string name)
        {
            return ((ConfigEntry) m_Entries[name]).Value;
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
                                            Debug.Trace("Config: Bad integer value (Name={0}; Value={1})", entry.Name, str3);
                                        }
                                    }
                                    else if (entry.Type == typeof(bool))
                                    {
                                        switch (str3.Trim().ToLower())
                                        {
                                            case "true":
                                            case "yes":
                                            case "on":
                                            case "1":
                                            {
                                                entry.Value = true;
                                                continue;
                                            }
                                            case "false":
                                            case "off":
                                            case "no":
                                            case "0":
                                            {
                                                entry.Value = false;
                                                continue;
                                            }
                                        }
                                        Debug.Trace("Config: Bad boolean value (Name={0}; Value={1}). Valid values are: true, yes, on, 1, false, off, no, 0.", entry.Name, str3);
                                    }
                                    else if (entry.Type == typeof(string))
                                    {
                                        entry.Value = str3;
                                    }
                                }
                                else
                                {
                                    Debug.Trace("Config: Bad name (Name={0}; Value={1})", str2, str3);
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
                ConfigEntry[] array = new ConfigEntry[m_Entries.Values.Count];
                m_Entries.Values.CopyTo(array, 0);
                Array.Sort(array);
                for (int i = 0; i < array.Length; i++)
                {
                    ConfigEntry entry = array[i];
                    writer.WriteLine("{0}={1}", entry.Name, entry.Value);
                }
            }
        }

        public static void SetValue(string name, object value)
        {
            ((ConfigEntry) m_Entries[name]).Value = value;
        }

        public static bool EncodeSpeech
        {
            get
            {
                return (bool) GetValue("EncodeSpeech");
            }
            set
            {
                SetValue("EncodeSpeech", value);
            }
        }

        public static bool EncryptionFix
        {
            get
            {
                return (bool) GetValue("EncryptionFix");
            }
            set
            {
                SetValue("EncryptionFix", value);
            }
        }

        public static bool ExtendProtocol
        {
            get
            {
                return (bool) GetValue("ExtendProtocol");
            }
            set
            {
                SetValue("ExtendProtocol", value);
            }
        }

        public static bool FullScreen
        {
            get
            {
                return (bool) GetValue("FullScreen");
            }
            set
            {
                SetValue("FullScreen", value);
            }
        }

        public static string GameSize
        {
            get
            {
                return (string) GetValue("GameSize");
            }
            set
            {
                SetValue("GameSize", value);
            }
        }

        public static bool IncomingFix
        {
            get
            {
                return (bool) GetValue("IncomingFix");
            }
            set
            {
                SetValue("IncomingFix", value);
            }
        }

        public static int LastServerID
        {
            get
            {
                return (int) GetValue("LastServerID");
            }
            set
            {
                SetValue("LastServerID", value);
            }
        }

        public static bool OldCharCreate
        {
            get
            {
                return (bool) GetValue("OldCharCreate");
            }
            set
            {
                SetValue("OldCharCreate", value);
            }
        }

        public static bool OldMovement
        {
            get
            {
                return (bool) GetValue("OldMovement");
            }
            set
            {
                SetValue("OldMovement", value);
            }
        }

        public static string Password
        {
            get
            {
                return (string) GetValue("Password");
            }
            set
            {
                SetValue("Password", value);
            }
        }

        public static bool PlayMusic
        {
            get
            {
                return (bool) GetValue("PlayMusic");
            }
            set
            {
                SetValue("PlayMusic", value);
            }
        }

        public static string ScreenSize
        {
            get
            {
                return (string) GetValue("ScreenSize");
            }
            set
            {
                SetValue("ScreenSize", value);
            }
        }

        public static bool SendUpdateRange
        {
            get
            {
                return (bool) GetValue("SendUpdateRange");
            }
            set
            {
                SetValue("SendUpdateRange", value);
            }
        }

        public static string ServerHost
        {
            get
            {
                return (string) GetValue("ServerHost");
            }
            set
            {
                SetValue("ServerHost", value);
            }
        }

        public static int ServerPort
        {
            get
            {
                return (int) GetValue("ServerPort");
            }
            set
            {
                SetValue("ServerPort", value);
            }
        }

        public static bool SmoothWalk
        {
            get
            {
                return (bool) GetValue("SmoothWalk");
            }
            set
            {
                SetValue("SmoothWalk", value);
            }
        }

        public static string Username
        {
            get
            {
                return (string) GetValue("Username");
            }
            set
            {
                SetValue("Username", value);
            }
        }

        private class ConfigEntry : IComparable
        {
            private static int m_Count;
            private int m_Index = m_Count++;
            private string m_Name;
            private System.Type m_Type;
            private object m_Value;

            public ConfigEntry(string name, System.Type type, object value)
            {
                this.m_Name = name;
                this.m_Type = type;
                this.m_Value = value;
            }

            int IComparable.CompareTo(object o)
            {
                if (o == null)
                {
                    return 1;
                }
                NewConfig.ConfigEntry entry = o as NewConfig.ConfigEntry;
                if (entry == null)
                {
                    throw new ArgumentException();
                }
                return this.m_Index.CompareTo(entry.m_Index);
            }

            public int Index
            {
                get
                {
                    return this.m_Index;
                }
            }

            public string Name
            {
                get
                {
                    return this.m_Name;
                }
            }

            public System.Type Type
            {
                get
                {
                    return this.m_Type;
                }
            }

            public object Value
            {
                get
                {
                    return this.m_Value;
                }
                set
                {
                    if ((value == null) && this.m_Type.IsValueType)
                    {
                        throw new ArgumentNullException("value", "Value types can not be null.");
                    }
                    this.m_Value = value;
                }
            }
        }
    }
}

