namespace Client
{
    using System;
    using System.Windows.Forms;

    public class GNetwork : GAlphaBackground
    {
        private static GLabel m_InSpeed;
        private static GLabel m_InTotal;
        private static bool m_Open;
        private static GLabel m_OutSpeed;
        private static GLabel m_OutTotal;
        private static int m_pcBytesIn;
        private static int m_pcBytesOut;
        private static GLabel m_Ping;
        private int m_Start;

        static GNetwork()
        {
            Network.OnSend = (NetworkHandler) Delegate.Combine(Network.OnSend, new NetworkHandler(GNetwork.OnSend));
            Network.OnRecv = (NetworkHandler) Delegate.Combine(Network.OnRecv, new NetworkHandler(GNetwork.OnRecv));
        }

        private GNetwork() : base(Engine.ScreenWidth - 200, 0, 200, 400)
        {
            m_pcBytesIn = 0;
            m_pcBytesOut = 0;
            this.m_Start = Environment.TickCount;
            IFont uniFont = Engine.GetUniFont(1);
            IHue bright = Hues.Bright;
            GLabel toAdd = new GLabel("Network Statistics", uniFont, bright, 0, 0);
            base.m_Children.Add(toAdd);
            toAdd.Center();
            toAdd.Y = 3;
            int y = (toAdd.Y + toAdd.Height) + 3;
            m_Ping = new GLabel("<-> Ping:", uniFont, bright, 3, y);
            base.m_Children.Add(m_Ping);
            y += m_Ping.Height + 3;
            m_OutSpeed = new GLabel("--> Kbps:", uniFont, bright, 3, y);
            base.m_Children.Add(m_OutSpeed);
            base.m_Children.Add(m_OutTotal = new GLabel("", uniFont, bright, 3, y));
            y += m_OutSpeed.Height + 3;
            m_InSpeed = new GLabel("<-- Kbps:", uniFont, bright, 3, y);
            base.m_Children.Add(m_InSpeed);
            base.m_Children.Add(m_InTotal = new GLabel("", uniFont, bright, 3, y));
            y += m_InSpeed.Height + 3;
            GPacketStats stats = new GPacketStats {
                Y = y
            };
            base.m_Children.Add(stats);
            y += stats.Height + 3;
            base.m_Height = y;
        }

        protected internal override void Draw(int X, int Y)
        {
            double num = Environment.TickCount - this.m_Start;
            num /= 1000.0;
            num *= 1024.0;
            m_Ping.Text = string.Format("<-> Ping: {0}", Engine.Ping);
            m_OutSpeed.Text = string.Format("--> Kbps: {0:F2}", ((double) m_pcBytesOut) / num);
            m_OutTotal.Text = this.Measure(m_pcBytesOut);
            m_OutTotal.X = (base.m_Width - 4) - m_OutTotal.Width;
            m_InSpeed.Text = string.Format("<-- Kbps: {0:F2}", ((double) m_pcBytesIn) / num);
            m_InTotal.Text = this.Measure(m_pcBytesIn);
            m_InTotal.X = (base.m_Width - 4) - m_InTotal.Width;
            base.Draw(X, Y);
        }

        private string Measure(int Bytes)
        {
            double num = ((double) Bytes) / 1024.0;
            num = ((double) ((int) (num * 100.0))) / 100.0;
            if (num == 0.0)
            {
                return "None";
            }
            if (num >= 1000.0)
            {
                double num2 = num / 1024.0;
                if (num2 >= 1000.0)
                {
                    double num3 = num2 / 1024.0;
                    return string.Format("{0:F2}gb", num3);
                }
                return string.Format("{0:F2}mb", num2);
            }
            return string.Format("{0:F2}kb", num);
        }

        protected internal override void OnDispose()
        {
            m_Open = false;
        }

        protected internal override void OnMouseUp(int X, int Y, MouseButtons mb)
        {
            if (mb == MouseButtons.Right)
            {
                Gumps.Destroy(this);
            }
        }

        private static void OnRecv(int Length)
        {
            m_pcBytesIn += Length;
        }

        private static void OnSend(int Length)
        {
            m_pcBytesOut += Length;
        }

        public static void Open()
        {
            if (!m_Open)
            {
                Gumps.Desktop.Children.Add(new GNetwork());
            }
            m_Open = true;
        }
    }
}

