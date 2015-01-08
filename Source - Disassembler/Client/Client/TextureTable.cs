namespace Client
{
    using System;
    using System.IO;

    public class TextureTable
    {
        public static int[] m_Table = new int[0x4000];

        static TextureTable()
        {
            for (int i = 0; i < m_Table.Length; i++)
            {
                m_Table[i] = i;
            }
            string path = Engine.FileManager.ResolveMUL("Art.def");
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
                            int num3 = str2.IndexOf('}');
                            string str3 = str2.Substring(0, index).Trim();
                            string str4 = str2.Substring(index + 1, (num3 - index) - 1).Trim();
                            string str5 = str2.Substring(num3 + 1).Trim();
                            int num4 = Convert.ToInt32(str3);
                            int num5 = Convert.ToInt32(str4);
                            int num6 = Convert.ToInt32(str5);
                            if ((num4 < 0x4000) && (num6 < 0x4000))
                            {
                                m_Table[num4 & 0x3fff] = num5 & 0x3fff;
                            }
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

