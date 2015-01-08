namespace Client
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class LayoutEntry
    {
        private Hashtable m_Attributes;
        private string m_Name;
        private int[] m_Parameters;

        public LayoutEntry(string format)
        {
            string[] strArray = format.Split(new char[] { ' ' });
            if (strArray.Length > 0)
            {
                this.m_Name = strArray[0];
                ArrayList dataStore = Engine.GetDataStore();
                this.m_Attributes = new Hashtable(0);
                for (int i = 1; i < strArray.Length; i++)
                {
                    try
                    {
                        int num2 = Convert.ToInt32(strArray[i]);
                        dataStore.Add(num2);
                    }
                    catch
                    {
                        int index = strArray[i].IndexOf('=');
                        if (index > 0)
                        {
                            try
                            {
                                string str = strArray[i].Substring(0, index);
                                string str2 = strArray[i].Substring(index + 1);
                                this.m_Attributes[str] = str2;
                            }
                            catch
                            {
                            }
                        }
                    }
                }
                this.m_Parameters = (int[]) dataStore.ToArray(typeof(int));
            }
        }

        public string GetAttribute(string name)
        {
            return (string) this.m_Attributes[name];
        }

        public Hashtable Attributes
        {
            get
            {
                return this.m_Attributes;
            }
        }

        public int this[int index]
        {
            get
            {
                if (index >= this.m_Parameters.Length)
                {
                    return 0;
                }
                return this.m_Parameters[index];
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }

        public int[] Parameters
        {
            get
            {
                return this.m_Parameters;
            }
        }
    }
}

