namespace Client
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;

    public class UOAM
    {
        private static IPAddress m_Address;
        private static byte[] m_Buffer = new byte[0x1000];
        private static DateTime m_LastPost;
        private static DateTime m_LastQuery;
        private static bool m_Log;
        private static int m_Offset;
        private static string m_Password;
        private static TimeSpan m_PostDelay = TimeSpan.FromSeconds(0.5);
        private static TimeSpan m_QueryDelay = TimeSpan.FromSeconds(0.5);
        private static int m_Sequence;
        private static Socket m_Socket;
        private static string m_Username;

        public static bool Chat(string text)
        {
            text = text.Trim();
            if (text.Length == 0)
            {
                return false;
            }
            UOAMPacket p = new UOAMPacket();
            p.Write((short) 5);
            p.Write((short) 0x300);
            p.Write(0x10);
            p.Write(0);
            p.Write(m_Sequence++);
            p.Write(0);
            p.Write((byte) 1);
            p.Write((byte) 0);
            p.Write((byte) 0);
            p.Write((byte) 0);
            p.Write(m_Password, 20);
            p.Write(m_Address);
            p.Write(0);
            p.Write(1);
            p.Write((ushort) 0xe390);
            p.Write((short) 0xdb);
            p.Write((int) (m_Username.Length + 1));
            p.Write(0);
            p.Write((int) (m_Username.Length + 1));
            p.Write(m_Username, m_Username.Length + 1);
            p.Align(4);
            p.Write((int) (text.Length + 1));
            p.Write((short) 0x4480);
            p.Write((short) 0xd6);
            p.Write((int) (text.Length + 1));
            p.Write(text, text.Length + 1);
            p.Seek(0x10L, SeekOrigin.Begin);
            p.Write((int) (p.Length - 0x18L));
            return Send(p);
        }

        public static bool Connect(string username, string password, IPAddress ipaddr, int port)
        {
            Disconnect();
            m_Username = username;
            m_Password = password;
            m_Address = ipaddr;
            try
            {
                m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                m_Socket.Connect(new IPEndPoint(ipaddr, port));
                m_Socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.Debug, 1);
            }
            catch
            {
                m_Socket = null;
                return false;
            }
            if (!DoConnectSequence())
            {
                return false;
            }
            Engine.AddTextMessage(string.Format("UOAM: Connected to {0}.", ipaddr), Engine.DefaultFont, Hues.Load(0x59));
            return true;
        }

        public static void Disconnect()
        {
            if (m_Socket != null)
            {
                Engine.AddTextMessage("UOAM: Disconnected", Engine.DefaultFont, Hues.Load(0x59));
                try
                {
                    m_Socket.Shutdown(SocketShutdown.Both);
                }
                catch
                {
                }
                try
                {
                    m_Socket.Close();
                }
                catch
                {
                }
                m_Socket = null;
            }
        }

        public static void DoChat(UOAMPacketReader reader)
        {
            try
            {
                int num = reader.ReadInt32();
                for (int i = 0; i < num; i++)
                {
                    if (reader.ReadInt32() == 0)
                    {
                        return;
                    }
                    int length = reader.ReadInt32();
                    if (reader.ReadInt32() != 0)
                    {
                        m_Log = true;
                    }
                    if (reader.ReadInt32() != length)
                    {
                        m_Log = true;
                    }
                    string str = reader.ReadString(length);
                    reader.Align(4);
                    length = reader.ReadInt32();
                    int num3 = reader.ReadInt32();
                    if (reader.ReadInt32() != length)
                    {
                        m_Log = true;
                    }
                    string str2 = reader.ReadString(length);
                    if ((str.Length > 0) && (str2.Length > 0))
                    {
                        Engine.AddTextMessage(string.Format("{0}: {1}", str, str2), Engine.DefaultFont, Hues.Load(0x59));
                    }
                }
            }
            catch
            {
                Disconnect();
            }
        }

        public static bool DoConnectSequence()
        {
            return (((SendConnectPacket() && Post()) && (SendLoginPacket2() && SendLoginPacket3())) && SendLoginPacket4());
        }

        public static void DoPositions(UOAMPacketReader reader)
        {
            try
            {
                int num = reader.ReadInt32();
                if (num != 0)
                {
                    if (reader.ReadInt32() != 1)
                    {
                        m_Log = true;
                    }
                    if (reader.ReadInt32() != num)
                    {
                        m_Log = true;
                    }
                    for (int i = 0; i < num; i++)
                    {
                        string name = reader.ReadString(0x4f);
                        int num3 = reader.ReadByte();
                        int x = reader.ReadInt32();
                        int y = reader.ReadInt32();
                        int num6 = reader.ReadInt32();
                        if (name != m_Username)
                        {
                            GRadar.AddTag(x, y, name);
                        }
                    }
                }
            }
            catch
            {
                Disconnect();
            }
        }

        public static bool Post()
        {
            char ch;
            Mobile player = World.Player;
            if (player == null)
            {
                return false;
            }
            if (Engine.m_World == 0)
            {
                ch = 'f';
            }
            else if (Engine.m_World == 1)
            {
                ch = 't';
            }
            else
            {
                ch = 'i';
            }
            m_LastPost = DateTime.Now;
            UOAMPacket p = new UOAMPacket();
            p.Write((short) 5);
            p.Write((short) 0x300);
            p.Write(0x10);
            p.Write(0);
            p.Write(m_Sequence++);
            p.Write(0);
            p.Write(0);
            p.Write(m_Password, 20);
            p.Write(m_Address);
            p.Write(m_Username, 0x4f);
            p.Write((byte) ch);
            p.Write((int) player.X);
            p.Write((int) player.Y);
            p.Write(0);
            p.Seek(0x10L, SeekOrigin.Begin);
            p.Write((int) (p.Length - 0x18L));
            return Send(p);
        }

        public static bool Query()
        {
            m_LastQuery = DateTime.Now;
            UOAMPacket p = new UOAMPacket();
            p.Write((short) 5);
            p.Write((short) 0x300);
            p.Write(0x10);
            p.Write(0);
            p.Write(m_Sequence++);
            p.Write(0);
            p.Write((byte) 0);
            p.Write((byte) 0);
            p.Write((byte) 2);
            p.Write((byte) 0);
            p.Write(m_Password, 20);
            p.Write(m_Address);
            p.Seek(0x10L, SeekOrigin.Begin);
            p.Write((int) (p.Length - 0x18L));
            return Send(p);
        }

        public static bool Query2()
        {
            UOAMPacket p = new UOAMPacket();
            p.Write((short) 5);
            p.Write((short) 0x300);
            p.Write(0x10);
            p.Write(0);
            p.Write(m_Sequence++);
            p.Write(0);
            p.Write((byte) 1);
            p.Write((byte) 0);
            p.Write((byte) 1);
            p.Write((byte) 0);
            p.Write(m_Password, 20);
            p.Write(m_Address);
            p.Write((short) 0x70);
            p.Write((short) 0xd7);
            p.Write((int) (m_Username.Length + 1));
            p.Write(0);
            p.Write((int) (m_Username.Length + 1));
            p.Write(m_Username, m_Username.Length + 1);
            p.Seek(0x10L, SeekOrigin.Begin);
            p.Write((int) (p.Length - 0x18L));
            return Send(p);
        }

        public static bool Send(UOAMPacket p)
        {
            if (m_Socket == null)
            {
                return false;
            }
            try
            {
                byte[] buffer = p.Compile();
                m_Socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
                return true;
            }
            catch
            {
                Disconnect();
                return false;
            }
        }

        public static bool SendConnectPacket()
        {
            byte[] buffer = new byte[] { 
                5, 0, 11, 3, 0x10, 0, 0, 0, 0x48, 0, 0, 0, 1, 0, 0, 0, 
                0xd0, 0x16, 0xd0, 0x16, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 
                0x10, 0x66, 0x15, 0x9e, 0x5c, 0x7b, 210, 0x11, 0xb8, 0xcf, 0, 0x80, 0xc7, 0x97, 0x1b, 0xe1, 
                1, 0, 0, 0, 4, 0x5d, 0x88, 0x8a, 0xeb, 0x1c, 0xc9, 0x11, 0x9f, 0xe8, 8, 0, 
                0x2b, 0x10, 0x48, 0x60, 2, 0, 0, 0
             };
            UOAMPacket p = new UOAMPacket();
            p.Write(buffer);
            return Send(p);
        }

        public static bool SendLoginPacket2()
        {
            byte[] buffer = new byte[] { 
                5, 0, 14, 3, 0x10, 0, 0, 0, 0x48, 0, 0, 0, 2, 0, 0, 0, 
                0xd0, 0x16, 0xd0, 0x16, 0x39, 0x79, 0x53, 0x30, 1, 0, 0, 0, 1, 0, 1, 0, 
                0xa2, 160, 110, 0xd7, 240, 0x2c, 0xb9, 70, 0xa8, 0x7b, 0xe4, 0x22, 0x77, 0xbc, 30, 0xf2, 
                1, 0, 0, 0, 4, 0x5d, 0x88, 0x8a, 0xeb, 0x1c, 0xc9, 0x11, 0x9f, 0xe8, 8, 0, 
                0x2b, 0x10, 0x48, 0x60, 2, 0, 0, 0
             };
            UOAMPacket p = new UOAMPacket();
            p.Write(buffer);
            return Send(p);
        }

        public static bool SendLoginPacket3()
        {
            UOAMPacket p = new UOAMPacket();
            p.Write((short) 5);
            p.Write((short) 0x300);
            p.Write(0x10);
            p.Write(0);
            p.Write(m_Sequence++);
            p.Write(0);
            p.Write(1);
            p.Write(m_Password, 20);
            p.Write(m_Address);
            p.Write(0);
            p.Write(2);
            p.Write((short) 0x70);
            p.Write((short) 0xd5);
            p.Write((int) (m_Username.Length + 1));
            p.Write(0);
            p.Write((int) (m_Username.Length + 1));
            p.Write(m_Username, m_Username.Length + 1);
            p.Seek(0x10L, SeekOrigin.Begin);
            p.Write((int) (p.Length - 0x18L));
            return Send(p);
        }

        public static bool SendLoginPacket4()
        {
            UOAMPacket p = new UOAMPacket();
            p.Write((short) 5);
            p.Write((short) 0x300);
            p.Write(0x10);
            p.Write(0);
            p.Write(m_Sequence++);
            p.Write(0);
            p.Write(1);
            p.Write(m_Password, 20);
            p.Write(m_Address);
            p.Write(0);
            p.Write(3);
            p.Write((short) 0x76e8);
            p.Write((short) 0xdb);
            p.Write((int) (m_Username.Length + 1));
            p.Write(0);
            p.Write((int) (m_Username.Length + 1));
            p.Write(m_Username, m_Username.Length + 1);
            p.Align(4);
            p.Write(0x40);
            p.Write((short) 0x4390);
            p.Write((short) 0xd8);
            p.Write(0x40);
            p.Write((ushort) 0xf200);
            p.Write((short) 0xff);
            p.Write((ushort) 0xfff6);
            p.Write((ushort) 0xffff);
            p.Write(0);
            p.Write(0);
            p.Write(0);
            p.Write(0);
            p.Write(0);
            p.Write(0);
            p.Write("Comic Sans MS", 0x20);
            p.Seek(0x10L, SeekOrigin.Begin);
            p.Write((int) (p.Length - 0x18L));
            return Send(p);
        }

        public static void Slice()
        {
            if (m_Socket != null)
            {
                try
                {
                    if ((DateTime.Now > (m_LastQuery + m_QueryDelay)) && (!Query() || !Query2()))
                    {
                        Disconnect();
                    }
                    else if ((DateTime.Now > (m_LastPost + m_PostDelay)) && !Post())
                    {
                        Disconnect();
                    }
                    else if (m_Socket.Available > 0)
                    {
                        int num = m_Socket.Receive(m_Buffer, 0, m_Buffer.Length - m_Offset, SocketFlags.None);
                        if (num <= 0)
                        {
                            Disconnect();
                        }
                        else
                        {
                            m_Offset += num;
                        }
                        while (m_Offset >= 0x18)
                        {
                            int count = ((m_Buffer[8] | (m_Buffer[9] << 8)) | (m_Buffer[10] << 0x10)) | (m_Buffer[11] << 0x18);
                            if (m_Offset < count)
                            {
                                return;
                            }
                            if (count == 0)
                            {
                                m_Offset = 0;
                                return;
                            }
                            m_Offset -= count;
                            byte[] dst = new byte[count];
                            Buffer.BlockCopy(m_Buffer, 0, dst, 0, count);
                            Buffer.BlockCopy(m_Buffer, count, m_Buffer, 0, m_Offset);
                            if (count >= 0x18)
                            {
                                m_Log = false;
                                if (dst[20] == 0)
                                {
                                    DoPositions(new UOAMPacketReader(dst));
                                }
                                else if (dst[20] == 1)
                                {
                                    DoChat(new UOAMPacketReader(dst));
                                }
                                else
                                {
                                    m_Log = true;
                                }
                                if (m_Log)
                                {
                                    Network.Log(dst, null);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    Disconnect();
                }
            }
        }

        public static bool Connected
        {
            get
            {
                return (m_Socket != null);
            }
        }
    }
}

