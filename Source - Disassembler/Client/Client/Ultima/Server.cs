namespace Client
{
    using System;
    using System.Net;

    public class Server : IComparable
    {
        private IPAddress m_Address;
        private string m_Name;
        private int m_PercentFull;
        private int m_ServerID;
        private int m_TimeZone;

        public Server(int serverID, string name, int percentFull, int timeZone, IPAddress address)
        {
            this.m_Name = name;
            this.m_ServerID = serverID;
            this.m_Address = address;
            this.m_TimeZone = timeZone;
            this.m_PercentFull = percentFull;
        }

        public void Select()
        {
            Engine.m_ServerName = this.m_Name;
            Macros.Load();
            Network.Send(new PHardwareInfo());
            Network.Send(new PServerSelection(this));
        }

        int IComparable.CompareTo(object o)
        {
            if (o == null)
            {
                return 1;
            }
            Server server = o as Server;
            if (server == null)
            {
                throw new ArgumentException();
            }
            System.TimeZone currentTimeZone = System.TimeZone.CurrentTimeZone;
            DateTime now = DateTime.Now;
            int hours = currentTimeZone.GetUtcOffset(now).Hours;
            int num2 = -this.m_TimeZone;
            int num3 = -server.m_TimeZone;
            int num4 = Math.Abs((int)(hours - num2));
            int num5 = Math.Abs((int)(hours - num3));
            int num6 = num4.CompareTo(num5);
            if (num6 == 0)
            {
                num6 = server.m_PercentFull.CompareTo(this.m_PercentFull);
                if (num6 == 0)
                {
                    num6 = this.m_Name.CompareTo(server.m_Name);
                    if (num6 == 0)
                    {
                        num6 = this.m_ServerID.CompareTo(server.m_ServerID);
                    }
                }
            }
            return num6;
        }

        public IPAddress Address
        {
            get
            {
                return this.m_Address;
            }
        }

        public string Name
        {
            get
            {
                return this.m_Name;
            }
        }

        public int PercentFull
        {
            get
            {
                return this.m_PercentFull;
            }
        }

        public int ServerID
        {
            get
            {
                return this.m_ServerID;
            }
        }

        public int TimeZone
        {
            get
            {
                return this.m_TimeZone;
            }
        }
    }
}