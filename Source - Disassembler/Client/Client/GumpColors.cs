namespace Client
{
    using Microsoft.Win32;
    using System.Drawing;

    public class GumpColors
    {
        private static int m_ActiveCaptionGradient = -1;
        private static int m_ControlAlternate = -1;
        private static int m_InactiveCaptionGradient = -1;

        public static void Invalidate()
        {
            m_ControlAlternate = -1;
            m_ActiveCaptionGradient = -1;
            m_InactiveCaptionGradient = -1;
        }

        private static System.Drawing.Color ReadRegistryColor(string name)
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Colors", false))
                {
                    string[] strArray = (key.GetValue(name) as string).Split(new char[] { ' ' });
                    return System.Drawing.Color.FromArgb(int.Parse(strArray[0]), int.Parse(strArray[1]), int.Parse(strArray[2]));
                }
            }
            catch
            {
            }
            return System.Drawing.Color.White;
        }

        public static int ActiveBorder
        {
            get
            {
                return (SystemColors.ActiveBorder.ToArgb() & 0xffffff);
            }
        }

        public static int ActiveCaption
        {
            get
            {
                return (SystemColors.ActiveCaption.ToArgb() & 0xffffff);
            }
        }

        public static int ActiveCaptionGradient
        {
            get
            {
                if (m_ActiveCaptionGradient >= 0)
                {
                    return m_ActiveCaptionGradient;
                }
                return (m_ActiveCaptionGradient = ReadRegistryColor("GradientActiveTitle").ToArgb() & 0xffffff);
            }
        }

        public static int ActiveCaptionText
        {
            get
            {
                return (SystemColors.ActiveCaptionText.ToArgb() & 0xffffff);
            }
        }

        public static int AppWorkspace
        {
            get
            {
                return (SystemColors.AppWorkspace.ToArgb() & 0xffffff);
            }
        }

        public static int Control
        {
            get
            {
                return (SystemColors.Control.ToArgb() & 0xffffff);
            }
        }

        public static int ControlAlternate
        {
            get
            {
                if (m_ControlAlternate >= 0)
                {
                    return m_ControlAlternate;
                }
                return (m_ControlAlternate = ReadRegistryColor("ButtonAlternateFace").ToArgb() & 0xffffff);
            }
        }

        public static int ControlDark
        {
            get
            {
                return (SystemColors.ControlDark.ToArgb() & 0xffffff);
            }
        }

        public static int ControlDarkDark
        {
            get
            {
                return (SystemColors.ControlDarkDark.ToArgb() & 0xffffff);
            }
        }

        public static int ControlLight
        {
            get
            {
                return (SystemColors.ControlLight.ToArgb() & 0xffffff);
            }
        }

        public static int ControlLightLight
        {
            get
            {
                return (SystemColors.ControlLightLight.ToArgb() & 0xffffff);
            }
        }

        public static int ControlText
        {
            get
            {
                return (SystemColors.ControlText.ToArgb() & 0xffffff);
            }
        }

        public static int Desktop
        {
            get
            {
                return (SystemColors.Desktop.ToArgb() & 0xffffff);
            }
        }

        public static int GrayText
        {
            get
            {
                return (SystemColors.GrayText.ToArgb() & 0xffffff);
            }
        }

        public static int Highlight
        {
            get
            {
                return (SystemColors.Highlight.ToArgb() & 0xffffff);
            }
        }

        public static int HighlightText
        {
            get
            {
                return (SystemColors.HighlightText.ToArgb() & 0xffffff);
            }
        }

        public static int HotTrack
        {
            get
            {
                return (SystemColors.HotTrack.ToArgb() & 0xffffff);
            }
        }

        public static int InactiveBorder
        {
            get
            {
                return (SystemColors.InactiveBorder.ToArgb() & 0xffffff);
            }
        }

        public static int InactiveCaption
        {
            get
            {
                return (SystemColors.InactiveCaption.ToArgb() & 0xffffff);
            }
        }

        public static int InactiveCaptionGradient
        {
            get
            {
                if (m_InactiveCaptionGradient >= 0)
                {
                    return m_InactiveCaptionGradient;
                }
                return (m_InactiveCaptionGradient = ReadRegistryColor("GradientInactiveTitle").ToArgb() & 0xffffff);
            }
        }

        public static int InactiveCaptionText
        {
            get
            {
                return (SystemColors.InactiveCaptionText.ToArgb() & 0xffffff);
            }
        }

        public static int Info
        {
            get
            {
                return (SystemColors.Info.ToArgb() & 0xffffff);
            }
        }

        public static int InfoText
        {
            get
            {
                return (SystemColors.InfoText.ToArgb() & 0xffffff);
            }
        }

        public static int Menu
        {
            get
            {
                return (SystemColors.Menu.ToArgb() & 0xffffff);
            }
        }

        public static int MenuText
        {
            get
            {
                return (SystemColors.MenuText.ToArgb() & 0xffffff);
            }
        }

        public static int ScrollBar
        {
            get
            {
                return (SystemColors.ScrollBar.ToArgb() & 0xffffff);
            }
        }

        public static int Window
        {
            get
            {
                return (SystemColors.Window.ToArgb() & 0xffffff);
            }
        }

        public static int WindowFrame
        {
            get
            {
                return (SystemColors.WindowFrame.ToArgb() & 0xffffff);
            }
        }

        public static int WindowText
        {
            get
            {
                return (SystemColors.WindowText.ToArgb() & 0xffffff);
            }
        }
    }
}