namespace Client
{
    using Microsoft.DirectX.AudioVideoPlayback;
    using System;
    using System.IO;

    public class Music
    {
        private static Audio m_Audio;
        private static string m_FileName;

        public static void Dispose()
        {
            if (m_Audio != null)
            {
                m_Audio.Dispose();
            }
            m_Audio = null;
        }

        public static void Play(string fileName)
        {
            if (m_FileName != fileName)
            {
                string path = Engine.FileManager.ResolveMUL(string.Format("music/{0}", fileName));
                if (File.Exists(path))
                {
                    Stop();
                    m_FileName = fileName;
                    if (m_Audio == null)
                    {
                        m_Audio = new Audio(path);
                    }
                    else
                    {
                        m_Audio.Open(path);
                    }
                    m_Audio.set_Volume(-10000 + (ScaledVolume * 100));
                    m_Audio.Play();
                }
            }
        }

        public static void Stop()
        {
            if (m_Audio != null)
            {
                m_Audio.Stop();
                m_Audio.Dispose();
                m_Audio = null;
                m_FileName = null;
            }
        }

        public static int ScaledVolume
        {
            get
            {
                int music = VolumeControl.Music;
                return (100 - (((((100 - music) * (100 - music)) * (100 - music)) * (100 - music)) / 0xf4240));
            }
        }
    }
}

