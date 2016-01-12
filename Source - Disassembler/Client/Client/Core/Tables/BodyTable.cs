namespace Client
{
    using System;
    using System.Collections;
    using System.IO;

    public class BodyTable
    {
        public static Hashtable m_Entries = new Hashtable();

        static BodyTable()
        {
            string path = Engine.FileManager.ResolveMUL("Body.def");
            if (File.Exists(path))
            {
                string str2;
                StreamReader reader = new StreamReader(path);
                while ((str2 = reader.ReadLine()) != null)
                {
                    if (((str2 = str2.Trim()).Length != 0) && !str2.StartsWith("#"))
                    {
                        try
                        {
                            int index = str2.IndexOf('{');
                            int num2 = str2.IndexOf('}');
                            string str3 = str2.Substring(0, index).Trim();
                            string str4 = str2.Substring(index + 1, (num2 - index) - 1).Trim();
                            string str5 = str2.Substring(num2 + 1).Trim();
                            int length = str4.IndexOf(',');
                            if (length > -1)
                            {
                                str4 = str4.Substring(0, length).Trim();
                            }
                            int newID = Convert.ToInt32(str3);
                            int oldID = Convert.ToInt32(str4);
                            int newHue = Convert.ToInt32(str5);
                            m_Entries[newID] = new BodyTableEntry(oldID, newID, newHue);
                            continue;
                        }
                        catch
                        {
                            Debug.Error("Bad def format");
                            continue;
                        }
                    }
                }
            }
        }
    }
}