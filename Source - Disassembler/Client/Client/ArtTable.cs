namespace Client
{
    using System;
    using System.Collections;
    using System.IO;

    public class ArtTable
    {
        public static ArtTableEntry[] m_Entries;

        static ArtTable()
        {
            string path = Engine.FileManager.ResolveMUL("Art.def");
            if (!File.Exists(path))
            {
                m_Entries = new ArtTableEntry[0];
            }
            else
            {
                string str2;
                StreamReader reader = new StreamReader(path);
                ArrayList list = new ArrayList();
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
                            int newID = Convert.ToInt32(str3);
                            int oldID = Convert.ToInt32(str4);
                            int newHue = Convert.ToInt32(str5);
                            list.Add(new ArtTableEntry(oldID, newID, newHue));
                            continue;
                        }
                        catch
                        {
                            Debug.Error("Bad def format");
                            continue;
                        }
                    }
                }
                m_Entries = (ArtTableEntry[]) list.ToArray(typeof(ArtTableEntry));
            }
        }
    }
}

