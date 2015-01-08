namespace Client
{
    using System;
    using System.IO;

    public class BodyConverter
    {
        private static int[] m_Table_AOS;
        private static int[] m_Table_AOW;
        private static int[] m_Table_LBR;

        static BodyConverter()
        {
            string path = Engine.FileManager.ResolveMUL("bodyconv.def");
            if (File.Exists(path))
            {
                m_Table_LBR = new int[0x800];
                m_Table_AOS = new int[0x800];
                m_Table_AOW = new int[0x800];
                for (int i = 0; i < m_Table_LBR.Length; i++)
                {
                    m_Table_LBR[i] = -1;
                }
                for (int j = 0; j < m_Table_AOS.Length; j++)
                {
                    m_Table_AOS[j] = -1;
                }
                for (int k = 0; k < m_Table_AOW.Length; k++)
                {
                    m_Table_AOW[k] = -1;
                }
                using (StreamReader reader = new StreamReader(path))
                {
                    string str2;
                    while ((str2 = reader.ReadLine()) != null)
                    {
                        if (((str2 = str2.Trim()).Length != 0) && !str2.StartsWith("#"))
                        {
                            try
                            {
                                string[] strArray = str2.Split(new char[] { '\t' });
                                int index = System.Convert.ToInt32(strArray[0]);
                                int num5 = System.Convert.ToInt32(strArray[1]);
                                int num6 = -1;
                                int num7 = -1;
                                try
                                {
                                    num6 = System.Convert.ToInt32(strArray[2]);
                                }
                                catch
                                {
                                }
                                try
                                {
                                    num7 = System.Convert.ToInt32(strArray[3]);
                                }
                                catch
                                {
                                }
                                if ((index >= 0) && (index < m_Table_LBR.Length))
                                {
                                    if (num5 == 0x44)
                                    {
                                        num5 = 0x7a;
                                    }
                                    m_Table_LBR[index] = num5;
                                }
                                if ((index >= 0) && (index < m_Table_AOS.Length))
                                {
                                    m_Table_AOS[index] = num6;
                                }
                                if ((index >= 0) && (index < m_Table_AOW.Length))
                                {
                                    m_Table_AOW[index] = num7;
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

        public static bool Contains(int bodyID)
        {
            return ((((m_Table_LBR != null) && (bodyID >= 0)) && ((bodyID < m_Table_LBR.Length) && (m_Table_LBR[bodyID] != -1))) || ((((m_Table_AOS != null) && (bodyID >= 0)) && ((bodyID < m_Table_AOS.Length) && (m_Table_AOS[bodyID] != -1))) || (((m_Table_AOW != null) && (bodyID >= 0)) && ((bodyID < m_Table_AOW.Length) && (m_Table_AOW[bodyID] != -1)))));
        }

        public static int Convert(ref int bodyID)
        {
            if (((m_Table_LBR != null) && (bodyID >= 0)) && (bodyID < m_Table_LBR.Length))
            {
                int num = m_Table_LBR[bodyID];
                if (num != -1)
                {
                    bodyID = num;
                    return 2;
                }
            }
            if (((m_Table_AOS != null) && (bodyID >= 0)) && (bodyID < m_Table_AOS.Length))
            {
                int num2 = m_Table_AOS[bodyID];
                if (num2 != -1)
                {
                    bodyID = num2;
                    return 3;
                }
            }
            if (((m_Table_AOW != null) && (bodyID >= 0)) && (bodyID < m_Table_AOW.Length))
            {
                int num3 = m_Table_AOW[bodyID];
                if (num3 != -1)
                {
                    bodyID = num3;
                    return 4;
                }
            }
            return 1;
        }

        public static int GetFileSet(int bodyID)
        {
            if (((m_Table_LBR != null) && (bodyID >= 0)) && (bodyID < m_Table_LBR.Length))
            {
                int num = m_Table_LBR[bodyID];
                if (num != -1)
                {
                    return 2;
                }
            }
            if (((m_Table_AOS != null) && (bodyID >= 0)) && (bodyID < m_Table_AOS.Length))
            {
                int num2 = m_Table_AOS[bodyID];
                if (num2 != -1)
                {
                    return 3;
                }
            }
            if (((m_Table_AOW != null) && (bodyID >= 0)) && (bodyID < m_Table_AOW.Length))
            {
                int num3 = m_Table_AOW[bodyID];
                if (num3 != -1)
                {
                    return 4;
                }
            }
            return 1;
        }
    }
}

