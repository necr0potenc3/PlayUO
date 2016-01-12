namespace Client
{
    using System;
    using System.Collections;
    using System.IO;

    public class SoundTable
    {
        public static Hashtable m_Map = new Hashtable();

        static SoundTable()
        {
            string path = Engine.FileManager.ResolveMUL("Sound.def");
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
                            int num3 = Convert.ToInt32(str3);
                            int num4 = Convert.ToInt32(str4);
                            m_Map[num3] = num4;
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