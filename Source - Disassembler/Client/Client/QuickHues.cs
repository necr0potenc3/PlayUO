namespace Client
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Text;

    public class QuickHues
    {
        private static ArrayList m_Entries;

        public static void Add(QuickHueEntry e)
        {
            Load();
            for (int i = 0; i < m_Entries.Count; i++)
            {
                QuickHueEntry entry = (QuickHueEntry) m_Entries[i];
                if ((entry.Name == e.Name) || (entry.Hue == e.Hue))
                {
                    m_Entries.RemoveAt(i);
                    m_Entries.Insert(0, e);
                    Save();
                    return;
                }
            }
            m_Entries.Insert(0, e);
            Save();
        }

        public static void Load()
        {
            m_Entries = new ArrayList();
            string path = Engine.FileManager.BasePath("Data/Binary/QuickHues.mul");
            if (File.Exists(path))
            {
                BinaryReader bin = new BinaryReader(File.OpenRead(path));
                int num = bin.ReadInt32();
                for (int i = 0; i < num; i++)
                {
                    QuickHueEntry entry = new QuickHueEntry {
                        Name = ReadString(bin),
                        Hue = bin.ReadInt32()
                    };
                    m_Entries.Add(entry);
                }
                bin.Close();
            }
            Validate();
        }

        private static string ReadString(BinaryReader bin)
        {
            int count = bin.ReadInt32();
            byte[] bytes = bin.ReadBytes(count);
            return Encoding.UTF8.GetString(bytes);
        }

        public static void Remove(int Index)
        {
            Load();
            m_Entries.RemoveAt(Index);
            Save();
        }

        public static void Save()
        {
            Validate();
            string path = Engine.FileManager.BasePath("Data/Binary/QuickHues.mul");
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            BinaryWriter bin = new BinaryWriter(File.Create(path));
            bin.Write(m_Entries.Count);
            for (int i = 0; i < m_Entries.Count; i++)
            {
                QuickHueEntry entry = (QuickHueEntry) m_Entries[i];
                WriteString(entry.Name, bin);
                bin.Write(entry.Hue);
            }
            bin.Close();
        }

        public static void Validate()
        {
            for (int i = 0; i < m_Entries.Count; i++)
            {
                Validate((QuickHueEntry) m_Entries[i], i);
            }
        }

        private static void Validate(QuickHueEntry e, int index)
        {
            int count = m_Entries.Count;
            int num2 = index + 1;
            while (num2 < count)
            {
                QuickHueEntry entry = (QuickHueEntry) m_Entries[num2];
                if ((entry.Name == e.Name) || (entry.Hue == e.Hue))
                {
                    m_Entries.RemoveAt(num2);
                    count--;
                }
                else
                {
                    num2++;
                }
            }
        }

        private static void WriteString(string toWrite, BinaryWriter bin)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(toWrite);
            int length = bytes.Length;
            bin.Write(length);
            bin.Write(bytes);
        }

        public static ArrayList Entries
        {
            get
            {
                return m_Entries;
            }
        }
    }
}

