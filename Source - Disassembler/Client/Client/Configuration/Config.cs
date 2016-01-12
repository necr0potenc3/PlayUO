namespace Client
{
    using System;
    using System.Collections;
    using System.IO;

    public class Config
    {
        private static string[] m_FileNames = new string[] {
            "Skills.idx", "Skills.mul", "SoundIdx.mul", "Sound.mul", "LightIdx.mul", "Light.mul", "Fonts.mul", "TileData.mul", "Anim.idx", "Anim.mul", "ArtIdx.mul", "Art.mul", "TexIdx.mul", "TexMaps.mul", "Hues.mul", "Multi.idx",
            "Multi.mul", "Map0.mul", "Map2.mul", "Statics0.mul", "Statics2.mul", "StaIdx0.mul", "StaIdx2.mul", "AnimData.mul", "VerData.mul", "GumpIdx.mul", "GumpArt.mul"
        };

        private static ArrayList m_PaperdollCFG;

        static Config()
        {
            if (!File.Exists(Engine.m_IniPath))
            {
                Engine.WantDirectory(Path.GetDirectoryName(Engine.m_IniPath));
                StreamWriter writer = new StreamWriter(Engine.m_IniPath, false);
                for (int i = 0; i < m_FileNames.Length; i++)
                {
                    writer.WriteLine(m_FileNames[i]);
                }
                writer.Flush();
                writer.Close();
            }
            else
            {
                StreamReader reader = new StreamReader(Engine.m_IniPath);
                for (int j = 0; (j < m_FileNames.Length) && (reader.Peek() != -1); j++)
                {
                    m_FileNames[j] = reader.ReadLine();
                }
                reader.Close();
            }
            if (!File.Exists(Engine.FileManager.ResolveMUL(Files.Verdata)))
            {
                string path = Engine.FileManager.BasePath("Data/Binary/EmptyVerdata.mul");
                m_FileNames[0x18] = path;
                if (!File.Exists(path))
                {
                    using (Stream stream = File.Create(path, 4))
                    {
                        stream.Write(new byte[4], 0, 4);
                        stream.Flush();
                    }
                }
            }
            m_PaperdollCFG = new ArrayList();
            Engine.WantDirectory("Data/Config/");
            if (File.Exists(Engine.FileManager.BasePath("Data/Config/Paperdoll.cfg")))
            {
                StreamReader reader2 = new StreamReader(Engine.FileManager.BasePath("Data/Config/Paperdoll.cfg"));
                string str2 = null;
                while ((str2 = reader2.ReadLine()) != null)
                {
                    string[] strArray = str2.Split(new char[] { '\t' });
                    if (strArray.Length >= 2)
                    {
                        try
                        {
                            m_PaperdollCFG.Add(new PaperdollEntry(Convert.ToInt32(strArray[0], 0x10), Convert.ToInt32(strArray[1], 0x10)));
                            continue;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                reader2.Close();
            }
        }

        public static string GetFile(int FileID)
        {
            return m_FileNames[FileID];
        }

        public static int GetPaperdollGump(int BodyID)
        {
            int count = m_PaperdollCFG.Count;
            for (int i = 0; i < count; i++)
            {
                PaperdollEntry entry = (PaperdollEntry)m_PaperdollCFG[i];
                if (entry.BodyID == BodyID)
                {
                    return entry.GumpID;
                }
            }
            return 0;
        }
    }
}