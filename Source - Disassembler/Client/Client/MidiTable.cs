namespace Client
{
    using System;
    using System.Collections;
    using System.IO;

    public class MidiTable
    {
        private bool m_Disposed;
        private Hashtable m_Entries;
        private Hashtable m_Overwrite;

        public MidiTable()
        {
            string path = Engine.FileManager.BasePath("Data/Config/Midis.cfg");
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
            object[,] objArray = new object[,] { 
                { 0, "oldult01.mid" }, { 1, "create1.mid" }, { 2, "draglift.mid" }, { 3, "oldult02.mid" }, { 4, "oldult03.mid" }, { 5, "oldult04.mid" }, { 6, "oldult05.mid" }, { 7, "oldult06.mid" }, { 8, "stones2.mid" }, { 9, "britain1.mid" }, { 10, "britain2.mid" }, { 11, "bucsden.mid" }, { 12, "jhelom.mid" }, { 13, "lbcastle.mid" }, { 14, "linelle.mid" }, { 15, "magincia.mid" }, 
                { 0x10, "minoc.mid" }, { 0x11, "ocllo.mid" }, { 0x12, "samlethe.mid" }, { 0x13, "serpents.mid" }, { 20, "skarabra.mid" }, { 0x15, "trinsic.mid" }, { 0x16, "vesper.mid" }, { 0x17, "wind.mid" }, { 0x18, "yew.mid" }, { 0x19, "cave01.mid" }, { 0x1a, "dungeon9.mid" }, { 0x1b, "forest_a.mid" }, { 0x1c, "intown01.mid" }, { 0x1d, "jungle_a.mid" }, { 30, "mountn_a.mid" }, { 0x1f, "plains_a.mid" }, 
                { 0x20, "sailing.mid" }, { 0x21, "swamp_a.mid" }, { 0x22, "tavern01.mid" }, { 0x23, "tavern02.mid" }, { 0x24, "tavern03.mid" }, { 0x25, "tavern04.mid" }, { 0x26, "combat1.mid" }, { 0x27, "combat2.mid" }, { 40, "combat3.mid" }, { 0x29, "approach.mid" }, { 0x2a, "death.mid" }, { 0x2b, "victory.mid" }, { 0x2c, "btcastle.mid" }, { 0x2d, "nujelm.mid" }, { 0x2e, "dungeon2.mid" }, { 0x2f, "cove.mid" }, 
                { 0x30, "moonglow.mid" }
             };
            int length = objArray.GetLength(0);
            this.m_Entries = new Hashtable(length);
            for (int i = 0; i < length; i++)
            {
                this.m_Entries[objArray[i, 0]] = objArray[i, 1];
            }
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
                                int index = str.IndexOf('\t');
                                string str2 = str.Substring(0, index);
                                string str3 = str.Substring(index + 1);
                                if (str2.StartsWith("0x"))
                                {
                                    num3 = Convert.ToInt32(str2.Substring(2), 0x10);
                                }
                                else
                                {
                                    num3 = Convert.ToInt32(str2);
                                }
                                this.m_Entries[num3] = str3;
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

        public void LoadMP3Table()
        {
            this.m_Overwrite = new Hashtable();
            string path = Engine.FileManager.ResolveMUL("music/digital/config.txt");
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string str2;
                    while ((str2 = reader.ReadLine()) != null)
                    {
                        str2 = str2.Trim();
                        if (str2.Length == 0)
                        {
                            return;
                        }
                        string[] strArray = str2.Split(new char[] { ' ' });
                        if (strArray.Length < 2)
                        {
                            return;
                        }
                        try
                        {
                            int num = int.Parse(strArray[0]);
                            string str3 = strArray[1];
                            int index = str3.IndexOf(',');
                            if (index >= 0)
                            {
                                str3 = str3.Substring(0, index);
                            }
                            this.m_Overwrite[num] = "digital/" + str3 + ".mp3";
                            continue;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
        }

        private void Save(string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false))
                {
                    writer.WriteLine("# Defines the table used to translate internal midi numbers to their corresponding file names");
                    writer.WriteLine("# All lines are trimmed. Empty lines, and lines starting with '#' are ignored.");
                    writer.WriteLine("# Format: <midi number><tab><file name>");
                    writer.WriteLine("# Parser supports hex or decimal numbers. Any numbers prefixed with \"0x\" are treated as hex.");
                    writer.WriteLine("# Any lines improperly formatted, the parser will ignore.");
                    writer.WriteLine("# Generated on {0}", DateTime.Now);
                    writer.WriteLine();
                    foreach (DictionaryEntry entry in this.m_Entries)
                    {
                        writer.Write(entry.Key);
                        writer.Write('\t');
                        writer.WriteLine(entry.Value);
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Trace("Error saving '{0}':", filePath);
                Debug.Error(exception);
            }
        }

        public string Translate(int midiID)
        {
            if (this.m_Overwrite == null)
            {
                this.LoadMP3Table();
            }
            string str = (string) this.m_Overwrite[midiID];
            if (str == null)
            {
                str = (string) this.m_Entries[midiID];
            }
            return str;
        }
    }
}

