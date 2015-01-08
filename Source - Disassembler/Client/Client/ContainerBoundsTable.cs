namespace Client
{
    using System;
    using System.Collections;
    using System.Drawing;
    using System.IO;

    public class ContainerBoundsTable
    {
        private Rectangle m_Default;
        private bool m_Disposed;
        private Hashtable m_Entries;

        public ContainerBoundsTable()
        {
            string path = Engine.FileManager.BasePath("Data/Config/ContainerBounds.cfg");
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
                { 7, 30, 30, 240, 140 }, { 9, 20, 0x55, 0x68, 0x6f }, { 60, 0x2c, 0x41, 0x8e, 0x5e }, { 0x3d, 0x1d, 0x22, 0x6c, 0x5e }, { 0x3e, 0x21, 0x24, 0x6d, 0x70 }, { 0x3f, 0x13, 0x2f, 0xa3, 0x4c }, { 0x40, 0x10, 0x26, 0x88, 0x57 }, { 0x41, 0x23, 0x26, 110, 0x4e }, { 0x42, 0x12, 0x69, 0x90, 0x49 }, { 0x43, 0x10, 0x33, 0xa8, 0x49 }, { 0x44, 20, 10, 150, 90 }, { 0x47, 0x10, 10, 0x84, 0x80 }, { 0x48, 0x10, 10, 0x8a, 0x54 }, { 0x49, 0x12, 0x69, 0x90, 0x49 }, { 0x4a, 0x12, 0x69, 0x90, 0x49 }, { 0x4b, 0x10, 0x33, 0xa8, 0x49 }, 
                { 0x4c, 0x2e, 0x4a, 150, 110 }, { 0x4d, 0x4c, 12, 0x40, 0x38 }, { 0x4e, 0x18, 0x60, 0xac, 0x38 }, { 0x4f, 0x18, 0x60, 0xac, 0x38 }, { 0x51, 0x10, 10, 0x8a, 0x54 }, { 0x52, 0, 0, 110, 0x3e }, { 0x91a, 0, 0, 0x11a, 230 }, { 0x92e, 0, 0, 0x11a, 210 }, { 0x2a63, 60, 0x21, 400, 0x13b }
             };
            int length = numArray.GetLength(0);
            this.m_Entries = new Hashtable(length);
            for (int i = 0; i < length; i++)
            {
                this.m_Entries[numArray[i, 0]] = new Rectangle(numArray[i, 1], numArray[i, 2], numArray[i, 3], numArray[i, 4]);
            }
            this.m_Default = new Rectangle(0, 0, 200, 200);
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

        private int IntConvert(string s)
        {
            if (s.StartsWith("0x"))
            {
                return Convert.ToInt32(s.Substring(2), 0x10);
            }
            return Convert.ToInt32(s);
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
                                string[] strArray = str.Split(new char[] { '\t' });
                                if (strArray[0] == "default")
                                {
                                    this.m_Default = new Rectangle(this.IntConvert(strArray[1]), this.IntConvert(strArray[2]), this.IntConvert(strArray[3]), this.IntConvert(strArray[4]));
                                }
                                else
                                {
                                    this.m_Entries[this.IntConvert(strArray[0]) & 0x3fff] = new Rectangle(this.IntConvert(strArray[1]), this.IntConvert(strArray[2]), this.IntConvert(strArray[3]), this.IntConvert(strArray[4]));
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
                    writer.WriteLine("# Defines the table used for container bounds");
                    writer.WriteLine("# All lines are trimmed. Empty lines, and lines starting with '#' are ignored.");
                    writer.WriteLine("# Format: <gump number><tab><x><tab><y><tab><width><tab><height>");
                    writer.WriteLine("# Parser supports hex or decimal numbers. Any numbers prefixed with \"0x\" are treated as hex.");
                    writer.WriteLine("# Any lines improperly formatted, the parser will ignore.");
                    writer.WriteLine("# The \"default\" gump number is a special case, gump numbers which are not in the table fall into this category.");
                    writer.WriteLine("# Generated on {0}", DateTime.Now);
                    writer.WriteLine();
                    foreach (DictionaryEntry entry in this.m_Entries)
                    {
                        int key = (int) entry.Key;
                        Rectangle rectangle = (Rectangle) entry.Value;
                        writer.WriteLine("0x{0:X4}\t{1}\t{2}\t{3}\t{4}", new object[] { key, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height });
                    }
                    writer.WriteLine("default\t{0}\t{1}\t{2}\t{3}", new object[] { this.m_Default.Left, this.m_Default.Top, this.m_Default.Width, this.m_Default.Height });
                }
            }
            catch (Exception exception)
            {
                Debug.Trace("Error saving '{0}':", filePath);
                Debug.Error(exception);
            }
        }

        public Rectangle Translate(int gumpID)
        {
            gumpID &= 0x3fff;
            object obj2 = this.m_Entries[gumpID];
            if (obj2 != null)
            {
                return (Rectangle) obj2;
            }
            return this.m_Default;
        }
    }
}

