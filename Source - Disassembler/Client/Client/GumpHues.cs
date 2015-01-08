namespace Client
{
    using System;

    public class GumpHues
    {
        private static IHue[] m_Hues = new IHue[0x1b];

        public static IHue GetHue(int c, int idx)
        {
            if (m_Hues[idx] == null)
            {
                m_Hues[idx] = new Hues.HFill(c);
            }
            return m_Hues[idx];
        }

        public static void Invalidate()
        {
            for (int i = 0; i < m_Hues.Length; i++)
            {
                m_Hues[i] = null;
            }
        }

        public static IHue ActiveBorder
        {
            get
            {
                return GetHue(GumpColors.ActiveBorder, 15);
            }
        }

        public static IHue ActiveCaption
        {
            get
            {
                return GetHue(GumpColors.ActiveCaption, 0x12);
            }
        }

        public static IHue ActiveCaptionText
        {
            get
            {
                return GetHue(GumpColors.ActiveCaptionText, 0x17);
            }
        }

        public static IHue AppWorkspace
        {
            get
            {
                return GetHue(GumpColors.AppWorkspace, 0x10);
            }
        }

        public static IHue Control
        {
            get
            {
                return GetHue(GumpColors.Control, 3);
            }
        }

        public static IHue ControlAlternate
        {
            get
            {
                return GetHue(GumpColors.ControlAlternate, 0x1a);
            }
        }

        public static IHue ControlDark
        {
            get
            {
                return GetHue(GumpColors.ControlDark, 12);
            }
        }

        public static IHue ControlDarkDark
        {
            get
            {
                return GetHue(GumpColors.ControlDarkDark, 0x15);
            }
        }

        public static IHue ControlLight
        {
            get
            {
                return GetHue(GumpColors.ControlLight, 0x11);
            }
        }

        public static IHue ControlLightLight
        {
            get
            {
                return GetHue(GumpColors.ControlLightLight, 0x18);
            }
        }

        public static IHue ControlText
        {
            get
            {
                return GetHue(GumpColors.ControlText, 13);
            }
        }

        public static IHue Desktop
        {
            get
            {
                return GetHue(GumpColors.Desktop, 4);
            }
        }

        public static IHue GrayText
        {
            get
            {
                return GetHue(GumpColors.GrayText, 5);
            }
        }

        public static IHue Highlight
        {
            get
            {
                return GetHue(GumpColors.Highlight, 9);
            }
        }

        public static IHue HighlightText
        {
            get
            {
                return GetHue(GumpColors.HighlightText, 0x13);
            }
        }

        public static IHue HotTrack
        {
            get
            {
                return GetHue(GumpColors.HotTrack, 6);
            }
        }

        public static IHue InactiveBorder
        {
            get
            {
                return GetHue(GumpColors.InactiveBorder, 20);
            }
        }

        public static IHue InactiveCaption
        {
            get
            {
                return GetHue(GumpColors.InactiveCaption, 0x16);
            }
        }

        public static IHue InactiveCaptionText
        {
            get
            {
                return GetHue(GumpColors.InactiveCaptionText, 0x19);
            }
        }

        public static IHue Info
        {
            get
            {
                return GetHue(GumpColors.Info, 0);
            }
        }

        public static IHue InfoText
        {
            get
            {
                return GetHue(GumpColors.InfoText, 7);
            }
        }

        public static IHue Menu
        {
            get
            {
                return GetHue(GumpColors.Menu, 1);
            }
        }

        public static IHue MenuText
        {
            get
            {
                return GetHue(GumpColors.MenuText, 8);
            }
        }

        public static IHue ScrollBar
        {
            get
            {
                return GetHue(GumpColors.ScrollBar, 10);
            }
        }

        public static IHue Window
        {
            get
            {
                return GetHue(GumpColors.Window, 2);
            }
        }

        public static IHue WindowFrame
        {
            get
            {
                return GetHue(GumpColors.WindowFrame, 14);
            }
        }

        public static IHue WindowText
        {
            get
            {
                return GetHue(GumpColors.WindowText, 11);
            }
        }
    }
}

