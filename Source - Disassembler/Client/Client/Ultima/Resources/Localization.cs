namespace Client
{
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Text;

    public class Localization
    {
        private static byte[] m_Buffer = new byte[0x400];
        private static string m_Cliloc1;
        private static string m_Extension;
        private static Hashtable m_Files;
        private static string m_Language = CultureInfo.CurrentUICulture.ThreeLetterWindowsLanguageName.ToUpper();
        private static Hashtable m_Strings;

        static Localization()
        {
            if (File.Exists(Engine.FileManager.ResolveMUL("cliloc-1." + m_Language)))
            {
                m_Extension = "." + m_Language;
            }
            else
            {
                m_Extension = ".ENU";
            }
            m_Cliloc1 = "cliloc-1" + m_Extension;
            m_Files = new Hashtable();
            m_Strings = new Hashtable();
            LoadCompiledDatabase();
        }

        public static LocalizationFile GetFile(string path)
        {
            LocalizationFile file = (LocalizationFile)m_Files[path];
            if (file == null)
            {
                m_Files[path] = file = new LocalizationFile(path);
            }
            return file;
        }

        public static string GetString(int number)
        {
            string str = (string)m_Strings[number];
            if (str == null)
            {
                string str2;
                int num2;
                int num = number;
                if (number >= 0x2dc6c0)
                {
                    number -= 0x2dc6c0;
                    int num3 = number / 0x3e8;
                    str2 = "intloc" + num3.ToString("D2") + m_Extension;
                    num2 = number % 0x3e8;
                }
                else if (number >= 0xf4240)
                {
                    number -= 0xf4240;
                    str2 = "cliloc" + ((number / 0x3e8)).ToString("D2") + m_Extension;
                    num2 = number % 0x3e8;
                }
                else if (number >= 0x7a120)
                {
                    str2 = m_Cliloc1;
                    num2 = number - 0x7a120;
                }
                else
                {
                    return string.Format("<Localization number invalid: {0}>", num);
                }
                LocalizationFile file = GetFile(Engine.FileManager.ResolveMUL(str2));
                m_Strings[num] = str = file[num2];
            }
            return str;
        }

        private static void LoadCompiledDatabase()
        {
            string path = Engine.FileManager.ResolveMUL("cliloc" + m_Extension);
            if (!File.Exists(path))
            {
                path = Engine.FileManager.ResolveMUL("cliloc.enu");
            }
            if (File.Exists(path))
            {
                using (BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)))
                {
                    reader.ReadInt32();
                    reader.ReadInt16();
                    while (reader.PeekChar() != -1)
                    {
                        int num = reader.ReadInt32();
                        reader.ReadByte();
                        int count = reader.ReadInt16();
                        if (count > m_Buffer.Length)
                        {
                            m_Buffer = new byte[(count + 0x3ff) & -1024];
                        }
                        if (count == 0)
                        {
                            m_Strings[num] = "";
                        }
                        else
                        {
                            reader.Read(m_Buffer, 0, count);
                            m_Strings[num] = Encoding.UTF8.GetString(m_Buffer, 0, count);
                        }
                    }
                }
            }
        }

        public static string Language
        {
            get
            {
                return m_Language;
            }
        }
    }
}