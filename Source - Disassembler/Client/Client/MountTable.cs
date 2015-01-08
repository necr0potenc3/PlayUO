namespace Client
{
    using System;
    using System.Collections;
    using System.IO;

    public class MountTable
    {
        private int m_Default;
        private bool m_Disposed;
        private Hashtable m_Entries;

        public MountTable()
        {
            string path = Engine.FileManager.BasePath("Data/Config/Mounts.cfg");
            if (File.Exists(path))
            {
                this.Load(path);
            }
            else
            {
                this.Default();
                this.Save(path);
            }
        }

        private void Default()
        {
            int[,] numArray = new int[,] { 
                { 0x3ea0, 0xe2 }, { 0x3ea1, 0xe4 }, { 0x3ea2, 0xcc }, { 0x3ea3, 210 }, { 0x3ea4, 0xda }, { 0x3ea5, 0xdb }, { 0x3ea6, 220 }, { 0x3ea7, 0x74 }, { 0x3ea8, 0x75 }, { 0x3ea9, 0x72 }, { 0x3eaa, 0x73 }, { 0x3eab, 170 }, { 0x3eac, 0xab }, { 0x3ead, 0x84 }, { 0x3eaf, 120 }, { 0x3eb0, 0x79 }, 
                { 0x3eb1, 0x77 }, { 0x3eb2, 0x76 }, { 0x3eb3, 0x90 }, { 0x3eb4, 0x7a }, { 0x3eb5, 0xb1 }, { 0x3eb6, 0xb2 }, { 0x3eb7, 0xb3 }, { 0x3eb8, 0xbc }, { 0x3eba, 0xbb }, { 0x3ebb, 0x319 }, { 0x3ebc, 0x317 }, { 0x3ebd, 0x31a }, { 0x3ebe, 0x31f }
             };
            int length = numArray.GetLength(0);
            this.m_Entries = new Hashtable(length);
            for (int i = 0; i < length; i++)
            {
                this.m_Entries[numArray[i, 0]] = numArray[i, 1];
            }
            this.m_Default = 200;
        }

        public void Dispose()
        {
            if (!this.m_Disposed)
            {
                this.m_Disposed = true;
                this.m_Entries.Clear();
                this.m_Entries = null;
            }
        }

        public bool IsMount(int body)
        {
            return this.m_Entries.ContainsValue(body);
        }

        private void Load(string filePath)
        {
            this.m_Entries = new Hashtable();
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string str;
                    int num = 0;
                    while ((str = reader.ReadLine()) != null)
                    {
                        str = str.Trim();
                        if ((str.Length > 0) && (str[0] != '#'))
                        {
                            try
                            {
                                int num3;
                                int num4;
                                int index = str.IndexOf('\t');
                                string str2 = str.Substring(0, index);
                                string str3 = str.Substring(index + 1);
                                bool flag = false;
                                if (str2.StartsWith("0x"))
                                {
                                    num3 = Convert.ToInt32(str2.Substring(2), 0x10);
                                }
                                else if (str2 == "default")
                                {
                                    flag = true;
                                    num3 = 0;
                                }
                                else
                                {
                                    num3 = Convert.ToInt32(str2);
                                }
                                if (str3.StartsWith("0x"))
                                {
                                    num4 = Convert.ToInt32(str3.Substring(2), 0x10);
                                }
                                else
                                {
                                    num4 = Convert.ToInt32(str3);
                                }
                                if (!flag)
                                {
                                    this.m_Entries[num3 & 0x3fff] = num4;
                                }
                                else
                                {
                                    this.m_Default = num4;
                                }
                            }
                            catch
                            {
                                Debug.Trace("Improper line in '{0}': '{1}' (line {2})", filePath, str, num);
                            }
                            num++;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Trace("Error loading '{0}':", filePath);
                Debug.Error(exception);
            }
        }

        private void Save(string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    writer.WriteLine("# Defines the table used to translate internal mount items to their corresponding body numbers");
                    writer.WriteLine("# All lines are trimmed. Empty lines, and lines starting with '#' are ignored.");
                    writer.WriteLine("# Format: <item number><tab><body number>");
                    writer.WriteLine("# Parser supports hex or decimal numbers. Any numbers prefixed with \"0x\" are treated as hex.");
                    writer.WriteLine("# Any lines improperly formatted, the parser will ignore.");
                    writer.WriteLine("# The \"default\" item number is a special case, item numbers which are not in the table fall into this category.");
                    writer.WriteLine("# Generated on {0}", DateTime.Now);
                    writer.WriteLine();
                    foreach (DictionaryEntry entry in this.m_Entries)
                    {
                        writer.Write("0x");
                        int key = (int) entry.Key;
                        writer.Write(key.ToString("X"));
                        writer.Write("\t0x");
                        writer.WriteLine(((int) entry.Value).ToString("X"));
                    }
                    writer.WriteLine("default\t0x{0:X}", this.m_Default);
                }
            }
            catch (Exception exception)
            {
                Debug.Trace("Error saving '{0}':", filePath);
                Debug.Error(exception);
            }
        }

        public int Translate(int itemID)
        {
            itemID &= 0x3fff;
            object obj2 = this.m_Entries[itemID];
            if (obj2 != null)
            {
                return (int) obj2;
            }
            return this.m_Default;
        }
    }
}

