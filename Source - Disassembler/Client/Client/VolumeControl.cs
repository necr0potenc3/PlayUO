namespace Client
{
    using Microsoft.Win32;
    using System;

    public class VolumeControl
    {
        private static int m_Music = -2147483648;
        private static int m_Sound = -2147483648;

        public static int Music
        {
            get
            {
                if (m_Music == -2147483648)
                {
                    m_Music = 100;
                    try
                    {
                        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\KUOC"))
                        {
                            if (key != null)
                            {
                                m_Music = (int) key.GetValue("Music Volume", 100);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                return m_Music;
            }
            set
            {
                if (m_Music != value)
                {
                    m_Music = value;
                    try
                    {
                        RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\KUOC", true);
                        if (key == null)
                        {
                            key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\KUOC");
                        }
                        key.SetValue("Music Volume", value);
                        key.Close();
                    }
                    catch
                    {
                    }
                }
            }
        }

        public static int Sound
        {
            get
            {
                if (m_Sound == -2147483648)
                {
                    m_Sound = 100;
                    try
                    {
                        using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\KUOC"))
                        {
                            if (key != null)
                            {
                                m_Sound = (int) key.GetValue("Sound Volume", 100);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                return m_Sound;
            }
            set
            {
                if (m_Sound != value)
                {
                    m_Sound = value;
                    try
                    {
                        RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\KUOC", true);
                        if (key == null)
                        {
                            key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\KUOC");
                        }
                        key.SetValue("Sound Volume", value);
                        key.Close();
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}

