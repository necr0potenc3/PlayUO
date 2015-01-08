namespace Client
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Text;

    public class Strings
    {
        private static SpeechEntry[] m_Speech;
        private static Hashtable m_Strings;

        public static void Dispose()
        {
            m_Speech = null;
            if (m_Strings != null)
            {
                m_Strings.Clear();
                m_Strings = null;
            }
        }

        public static SpeechEntry[] GetKeywords(string text)
        {
            if (!NewConfig.EncodeSpeech)
            {
                return new SpeechEntry[0];
            }
            if (m_Speech == null)
            {
                LoadSpeechTable();
            }
            text = text.ToLower();
            ArrayList dataStore = Engine.GetDataStore();
            SpeechEntry[] speech = m_Speech;
            int length = speech.Length;
            for (int i = 0; i < length; i++)
            {
                SpeechEntry entry = speech[i];
                if (IsMatch(text, entry.m_Keywords))
                {
                    dataStore.Add(entry);
                }
            }
            dataStore.Sort();
            SpeechEntry[] entryArray2 = (SpeechEntry[]) dataStore.ToArray(typeof(SpeechEntry));
            Engine.ReleaseDataStore(dataStore);
            return entryArray2;
        }

        public static string GetString(string name)
        {
            string str = (string) m_Strings[name];
            if (str == null)
            {
                str = string.Format("<empty:{0}>", name);
            }
            return str;
        }

        public static void Initialize()
        {
            string path = Engine.FileManager.BasePath("Data/Binary/Strings.mul");
            if (File.Exists(path))
            {
                BinaryReader reader = new BinaryReader(File.OpenRead(path));
                int capacity = reader.ReadInt32();
                m_Strings = new Hashtable(capacity);
                for (int i = 0; i < capacity; i++)
                {
                    int count = reader.ReadInt32();
                    byte[] bytes = reader.ReadBytes(count);
                    string key = Encoding.UTF8.GetString(bytes);
                    int num4 = reader.ReadInt32();
                    byte[] buffer2 = reader.ReadBytes(num4);
                    string str3 = Encoding.UTF8.GetString(buffer2);
                    m_Strings.Add(key, str3);
                }
                reader.Close();
            }
            else
            {
                m_Strings = new Hashtable();
            }
        }

        public static bool IsMatch(string input, string[] split)
        {
            int startIndex = 0;
            for (int i = 0; i < split.Length; i++)
            {
                if (split[i].Length > 0)
                {
                    int index = input.IndexOf(split[i], startIndex);
                    if ((index > 0) && (i == 0))
                    {
                        return false;
                    }
                    if (index < 0)
                    {
                        return false;
                    }
                    startIndex = index + split[i].Length;
                }
            }
            return ((split[split.Length - 1].Length <= 0) || (startIndex == input.Length));
        }

        public static unsafe void LoadSpeechTable()
        {
            string path = Engine.FileManager.ResolveMUL("Speech.mul");
            if (!File.Exists(path))
            {
                m_Speech = new SpeechEntry[0];
                Debug.Trace("File '{0}' not found, speech will not be encoded.", path);
            }
            else
            {
                byte[] buffer = new byte[0x400];
                fixed (byte* numRef = buffer)
                {
                    ArrayList list = new ArrayList();
                    FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                    int num = 0;
                    while ((num = Engine.NativeRead(fs, (void*) numRef, 4)) > 0)
                    {
                        int idKeyword = numRef[1] | (numRef[0] << 8);
                        int bytes = numRef[3] | (numRef[2] << 8);
                        if (bytes > 0)
                        {
                            Engine.NativeRead(fs, (void*) numRef, bytes);
                            list.Add(new SpeechEntry(idKeyword, new string((sbyte*) numRef, 0, bytes)));
                        }
                    }
                    fs.Close();
                    m_Speech = (SpeechEntry[]) list.ToArray(typeof(SpeechEntry));
                }
            }
        }
    }
}

