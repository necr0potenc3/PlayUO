namespace Client
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

    public class Network
    {
        private const int BufferLength = 0x10000;
        private static byte[] m_Buffer;
        public static UnpackCacheEntry[] m_CacheEntries;
        private static int m_ClientIP = -1;
        public static BaseCrypto m_CryptoProvider;
        public static int m_CurrFilled;
        private static bool m_Decompress;
        private static TimeDelay m_LastNetworkActivity = new TimeDelay(0x7530);
        public static DateTime m_LastPacket;
        public static UnpackLeaf[] m_Leaves;
        private static StreamWriter m_Logger;
        public static byte[] m_OutputBuffer;
        public static int m_OutputIndex;
        private static StreamWriter m_Protocol;
        private static byte[] m_SendBuffer = new byte[0x200];
        private static int m_SendLength;
        public static Socket m_Server;
        private static string m_ServerHost = NewConfig.ServerHost;
        private static IPAddress m_ServerIP;
        private static IPEndPoint m_ServerIPEP;
        private static int m_ServerPort = NewConfig.ServerPort;
        public static bool m_SoftDisconnect;

        private static short[] m_Table = new short[] {
            2, 0x1f5, 0x116, 0x167, 0x577, 0x56, 0x376, 0x267, 120, 0x468, 0x357, 0x9e8, 0x1739, 0xe98, 0x156, 0x757,
            0x8e8, 0xda8, 0x679, 0xe58, 0x527, 0x797, 0x668, 0xbe8, 0x1139, 0xe79, 0x7e7, 0x129, 0x9a8, 0xde8, 0x898, 0x638,
            0x2d6, 0xc39, 0xf68, 0x1929, 0xcea, 0x5a8, 0x1d6a, 0xea, 0x259, 0xa59, 0x829, 0x3e6a, 0x11d9, 0x99, 0x1ce9, 0x33ea,
            0x1a59, 0x19, 0x139, 0x326, 0x10d9, 0x1499, 0x1999, 0x1539, 0x19d9, 0xa79, 0xc29, 0x12f9, 0x1b69, 0x6f9, 0x1a29, 0x1429,
            0xa5, 0x698, 920, 0x1879, 0x739, 0x1e29, 0x1e39, 0x1129, 0x1c29, 0xc69, 0x1dd9, 0x1229, 0xd69, 0x629, 0xef9, 0x1af9,
            0x429, 0x4d9, 0x15d9, 0x1679, 0x7a8, 0x229, 0x1a99, 0x9d9, 0xa19, 0x1629, 0xb99, 0x719, 0x21ea, 490, 0x1c39, 0xeea,
            0x3eea, 0x177, 0x1f39, 0xb18, 0x278, 0x4f7, 0xae9, 0x10e9, 40, 0x417, 0x1a19, 0x1a9, 0x568, 0x838, 0x477, 0xf7,
            0x8d8, 0x659, 0x617, 0x6e8, 0x337, 0x597, 0xa29, 0x3d6, 0x938, 0xe69, 0x1259, 0xd9, 0x176a, 0x1e6a, 0x3e7a, 0x14e9,
            0x539, 0x439, 0xc99, 0x67, 0x368, 0x1099, 0x2f9, 0x20ea, 0x2e2a, 0x31aa, 0x287a, 0x36fa, 0x2faa, 0x182a, 0x3a7a, 0x1c9a,
            0x3a7, 0x198, 0xf3a, 0x1d3a, 0x25ea, 0x1fa9, 0x3efa, 0x2eea, 0x2d3a, 0x2dda, 0x343a, 0x382a, 0x5da, 0x1efa, 0x292a, 0x5ea,
            0x12e9, 0x2cea, 0x2b6a, 0x14d9, 0x131a, 0x21da, 0x3aeb, 0x31eb, 0x23eb, 0x281a, 0x165a, 0x49a, 0x2cda, 0x371a, 0x51eb, 0x229a,
            0x25da, 0x129a, 0xaf9, 0x2eb, 0x44eb, 0xf1a, 0x24eb, 0x55eb, 0x331a, 0xcda, 0x4eb, 0x376a, 0x42eb, 0x11eb, 0x221a, 0x301a,
            0xa99, 0x231a, 0x3eb, 0x365a, 0x75eb, 0x101a, 0x5aeb, 0x321a, 0x329a, 0xd3a, 0x2f1a, 0x3f1a, 0x3d3a, 0xe2a, 0x87a, 0xe3a,
            0xfaa, 0x143a, 0x1da, 0x1b9a, 0x29a, 0x63eb, 0x1a7a, 0x3c6a, 0x81a, 0x181a, 0x31a, 0x299a, 0x121a, 0x1f1a, 0x1aeb, 0x64eb,
            0x43eb, 0x1eea, 0x35eb, 0x2e3a, 0x53eb, 0x13eb, 0x381a, 0x71eb, 0x3b9a, 0x15eb, 0x38, 0x11aa, 0x16fa, 0x92a, 0xb6a, 0x99a,
            0x2f3a, 0x22ea, 0x3d6a, 0x1e7a, 0x3c9a, 0x676b, 0x249a, 0x276b, 0x1cd9, 0x21a, 0x171a, 0x76a, 0x7aeb, 0x1c6a, 0xdda, 0x237,
            180
        };

        public static UnpackLeaf m_Tree;
        private static byte[] m_UnpackBuffer;
        private static bool m_UnpackCacheLoaded;
        internal static Client.OnPacketHandle OnPacketHandle;
        public static NetworkHandler OnRecv;
        public static NetworkHandler OnSend;

        static unsafe Network()
        {
            Engine.WantDirectory("Data/Logs/Network/");
            m_Leaves = new UnpackLeaf[0x201];
            int index = 0;
            m_Tree = new UnpackLeaf(index);
            m_Leaves[index++] = m_Tree;
            fixed (short* numRef = m_Table)
            {
                short* numPtr = numRef;
                for (short i = 0; i <= 0x100; i = (short)(i + 1))
                {
                    UnpackLeaf tree = m_Tree;
                    numPtr++;
                    int num3 = numPtr[0];
                    int num4 = num3 & 15;
                    for (int j = num3 >> 4; --num4 >= 0; j = j >> 1)
                    {
                        switch ((j & 1))
                        {
                            case 0:
                                if (tree.m_Left == null)
                                {
                                    tree.m_Left = new UnpackLeaf(index);
                                    m_Leaves[index++] = tree.m_Left;
                                }
                                tree = tree.m_Left;
                                break;

                            case 1:
                                if (tree.m_Right == null)
                                {
                                    tree.m_Right = new UnpackLeaf(index);
                                    m_Leaves[index++] = tree.m_Right;
                                }
                                tree = tree.m_Right;
                                break;
                        }
                    }
                    tree.m_Value = i;
                }
            }
            m_Table = null;
        }

        public static void CheckCache()
        {
            if (!m_UnpackCacheLoaded)
            {
                ManualResetEvent state = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem(new WaitCallback(Network.Thread_LoadUnpackCache), state);
                do
                {
                    Engine.DrawNow();
                }
                while (!state.WaitOne(10, false));
                state.Close();
                m_UnpackCacheLoaded = true;
            }
        }

        public static void Close()
        {
            Flush();
            if (m_Server != null)
            {
                try
                {
                    m_Server.Shutdown(SocketShutdown.Both);
                }
                catch
                {
                }
                try
                {
                    m_Server.Close();
                }
                catch
                {
                }
                m_Server = null;
            }
            if (m_Logger != null)
            {
                m_Logger.WriteLine();
                m_Logger.WriteLine("#####\tEnding packet log on {0}\t#####", DateTime.Now);
                m_Logger.Flush();
                m_Logger.Close();
                m_Logger = null;
            }
            if (m_Protocol != null)
            {
                m_Protocol.WriteLine();
                m_Protocol.WriteLine("#####\tEnding protocol log on {0}\t#####", DateTime.Now);
                m_Protocol.Flush();
                m_Protocol.Close();
                m_Protocol = null;
            }
            m_ServerIP = null;
            m_ServerIPEP = null;
            m_ServerHost = null;
            m_Tree = null;
            m_Buffer = null;
            OnRecv = null;
            OnSend = null;
            OnPacketHandle = null;
            m_Table = null;
            m_UnpackBuffer = null;
            m_OutputBuffer = null;
            m_CryptoProvider = null;
        }

        public static bool Connect()
        {
            m_ServerHost = NewConfig.ServerHost;
            m_ServerPort = NewConfig.ServerPort;
            try
            {
                Debug.Try("Parsing IP ( \"{0}\" )", m_ServerHost);
                m_ServerIP = IPAddress.Parse(m_ServerHost);
                m_ServerIPEP = new IPEndPoint(m_ServerIP, m_ServerPort);
                Debug.EndTry("( {0} )", m_ServerIPEP);
            }
            catch
            {
                Debug.FailTry();
                try
                {
                    Debug.Try("Resolving");
                    IAsyncResult asyncResult = Dns.BeginResolve(m_ServerHost, null, null);
                    do
                    {
                        Engine.DrawNow();
                    }
                    while (!asyncResult.AsyncWaitHandle.WaitOne(10, false));
                    IPHostEntry entry = Dns.EndResolve(asyncResult);
                    if (entry.AddressList.Length == 0)
                    {
                        Debug.FailTry("( AddressList is empty )");
                        return false;
                    }
                    m_ServerIP = entry.AddressList[0];
                    m_ServerIPEP = new IPEndPoint(m_ServerIP, m_ServerPort);
                    Debug.EndTry("( {0} )", m_ServerIPEP);
                }
                catch (Exception exception)
                {
                    Debug.FailTry();
                    Debug.Error(exception);
                    return false;
                }
            }
            Flush();
            Engine.ValidateHandlers();
            m_Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Debug.Try("Connecting to login server '{0}'", m_ServerIPEP);
            try
            {
                IAsyncResult result2 = m_Server.BeginConnect(m_ServerIPEP, null, null);
                do
                {
                    Engine.DrawNow();
                }
                while (!result2.AsyncWaitHandle.WaitOne(10, false));
                m_Server.EndConnect(result2);
            }
            catch (Exception exception2)
            {
                Debug.FailTry();
                Debug.Error(exception2);
                return false;
            }
            Debug.EndTry();
            return true;
        }

        public static bool Connect(int ServerIP, int Port)
        {
            Flush();
            m_ServerPort = Port;
            Engine.ValidateHandlers();
            m_Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            m_ServerIP = new IPAddress((long)((ulong)ServerIP));
            m_ServerIPEP = new IPEndPoint(m_ServerIP, Port);
            Debug.Try("Connecting to game server '{0}'", m_ServerIPEP);
            try
            {
                IAsyncResult asyncResult = m_Server.BeginConnect(m_ServerIPEP, null, null);
                do
                {
                    Engine.DrawNow();
                }
                while (!asyncResult.AsyncWaitHandle.WaitOne(10, false));
                m_Server.EndConnect(asyncResult);
            }
            catch (Exception exception)
            {
                Debug.FailTry();
                Debug.Error(exception);
                return false;
            }
            Debug.EndTry();
            return true;
        }

        public static void Disconnect()
        {
            Disconnect(true);
        }

        public static void Disconnect(bool flush)
        {
            Engine.ClearPings();
            m_Decompress = false;
            m_CurrFilled = 0;
            if (flush)
            {
                Flush();
            }
            if (m_Server != null)
            {
                try
                {
                    m_Server.Shutdown(SocketShutdown.Both);
                }
                catch
                {
                }
                try
                {
                    m_Server.Close();
                }
                catch
                {
                }
                m_Server = null;
            }
        }

        public static void DumpBuffer()
        {
            GetLogger();
            m_Logger.WriteLine();
            if ((m_CurrFilled == 0) || (m_Buffer == null))
            {
                m_Logger.WriteLine("Packet buffer is empty.");
            }
            else
            {
                m_Logger.WriteLine("Packet buffer contains {0} ( 0x{0:X} ) bytes", m_CurrFilled);
                Log(m_Buffer, 0, m_CurrFilled, null);
            }
        }

        public static void EnableUnpacking()
        {
            m_Decompress = true;
            m_CryptoProvider = null;
        }

        public static void Flush()
        {
            try
            {
                if ((m_Server != null) && (m_SendLength > 0))
                {
                    m_Server.Send(m_SendBuffer, 0, m_SendLength, SocketFlags.None);
                }
            }
            catch (SocketException exception)
            {
                Debug.Trace("SocketException caught in Network.Flush()");
                Debug.Trace("Error Code: 0x{0:X}", exception.ErrorCode);
                Debug.Trace("Native Error Code: 0x{0:X}", exception.NativeErrorCode);
                Debug.Error(exception);
                Gumps.MessageBoxOk("Connection lost", true, new OnClick(Engine.DestroyDialogShowAcctLogin_OnClick));
                Disconnect(false);
                Cursor.Hourglass = false;
                m_SoftDisconnect = false;
                Engine.amMoving = false;
            }
            m_SendLength = 0;
        }

        private static void GetLogger()
        {
            if (m_Logger == null)
            {
                m_Logger = new StreamWriter(Engine.FileManager.CreateUnique("Data/Logs/Network/Packet", ".log"));
                m_Logger.AutoFlush = true;
                m_Logger.WriteLine("#####\tStarting packet log on {0}\t#####", DateTime.Now);
            }
        }

        private static void GetProtocol()
        {
            if (m_Protocol == null)
            {
                m_Protocol = new StreamWriter(Engine.FileManager.CreateUnique("Data/Logs/Network/Protocol", ".log"));
                m_Protocol.AutoFlush = true;
                m_Protocol.WriteLine("#####\tStarting protocol log on {0}\t#####");
            }
        }

        public static unsafe void LoadUnpackCache()
        {
            m_OutputBuffer = new byte[0x18975];
            m_CacheEntries = new UnpackCacheEntry[0x10000];
            fixed (UnpackCacheEntry* entryRef = m_CacheEntries)
            {
                UnpackCacheEntry* entryPtr = entryRef;
                fixed (byte* numRef = m_OutputBuffer)
                {
                    Stack stack = new Stack();
                    stack.Push(m_Tree);
                    while (stack.Count > 0)
                    {
                        UnpackLeaf leaf2 = (UnpackLeaf)stack.Pop();
                        if (leaf2.m_Left != null)
                        {
                            stack.Push(leaf2.m_Left);
                        }
                        if (leaf2.m_Right != null)
                        {
                            stack.Push(leaf2.m_Right);
                        }
                        int[] numArray = new int[0x100];
                        for (int i = 0; i < 0x100; i++)
                        {
                            byte* numPtr = numRef + m_OutputIndex;
                            UnpackLeaf left = leaf2;
                            int num2 = 0;
                            int num = 8;
                            while (--num >= 0)
                            {
                                switch (((i >> num) & 1))
                                {
                                    case 0:
                                        left = left.m_Left;
                                        break;

                                    case 1:
                                        left = left.m_Right;
                                        break;
                                }
                                if ((left != null) && (left.m_Value != -1))
                                {
                                    switch ((left.m_Value >> 8))
                                    {
                                        case 0:
                                            numPtr[num2++] = (byte)left.m_Value;
                                            break;

                                        case 1:
                                            num = 0;
                                            break;
                                    }
                                    left = m_Tree;
                                }
                            }
                            entryPtr->m_NextIndex = left.m_Index;
                            entryPtr->m_ByteIndex = m_OutputIndex;
                            entryPtr->m_ByteCount = num2;
                            numArray[i] = (int)((long)((entryPtr - entryRef) / sizeof(UnpackCacheEntry)));
                            entryPtr++;
                            m_OutputIndex += num2;
                        }
                        leaf2.m_Cache = numArray;
                    }
                }
            }
        }

        public static void Log(byte[] data, string header)
        {
            Log(data, 0, data.Length, header);
        }

        public static void Log(TextWriter tw, byte[] data, int offset, int count)
        {
            int num = count;
            int num2 = num >> 4;
            int capacity = num & 15;
            int index = offset;
            int num5 = 0;
            if (tw != null)
            {
                tw.WriteLine("        0  1  2  3  4  5  6  7   8  9  A  B  C  D  E  F");
            }
            if (tw != null)
            {
                tw.WriteLine("       -- -- -- -- -- -- -- --  -- -- -- -- -- -- -- --");
            }
            int num6 = 0;
            while (num6 < num2)
            {
                StringBuilder builder = new StringBuilder(0x31);
                StringBuilder builder2 = new StringBuilder(0x10);
                if (tw != null)
                {
                    tw.Write(num5.ToString("X4"));
                }
                if (tw != null)
                {
                    tw.Write("   ");
                }
                int num7 = 0;
                while (num7 < 0x10)
                {
                    byte num8 = data[index];
                    builder.Append(num8.ToString("X2"));
                    builder.Append(' ');
                    if (num7 == 7)
                    {
                        builder.Append(' ');
                    }
                    if ((num8 >= 0x20) && (num8 < 0x80))
                    {
                        builder2.Append((char)num8);
                    }
                    else
                    {
                        builder2.Append('.');
                    }
                    num7++;
                    index++;
                }
                if (tw != null)
                {
                    tw.Write(builder.ToString());
                    tw.Write("  ");
                    tw.WriteLine(builder2.ToString());
                }
                num6++;
                num5 += 0x10;
            }
            if (capacity > 0)
            {
                StringBuilder builder3 = new StringBuilder(0x31);
                StringBuilder builder4 = new StringBuilder(capacity);
                if (tw != null)
                {
                    tw.Write(num5.ToString("X4"));
                    tw.Write("   ");
                }
                int num9 = 0;
                while (num9 < 0x10)
                {
                    if (num9 < capacity)
                    {
                        byte num10 = data[index];
                        builder3.Append(num10.ToString("X2"));
                        builder3.Append(' ');
                        if ((num10 >= 0x20) && (num10 < 0x80))
                        {
                            builder4.Append((char)num10);
                        }
                        else
                        {
                            builder4.Append('.');
                        }
                    }
                    else
                    {
                        builder3.Append("   ");
                    }
                    if (num9 == 7)
                    {
                        builder3.Append(' ');
                    }
                    num9++;
                    index++;
                }
                if (tw != null)
                {
                    tw.Write(builder3.ToString());
                    tw.Write("  ");
                    tw.WriteLine(builder4.ToString());
                }
            }
            if (tw != null)
            {
                tw.Flush();
            }
        }

        public static void Log(byte[] data, int offset, int count, string header)
        {
            GetLogger();
            StringWriter tw = new StringWriter();
            if (header != null)
            {
                tw.WriteLine();
                tw.WriteLine(header);
                tw.WriteLine();
            }
            Log(tw, data, offset, count);
            string str = tw.ToString();
            m_Logger.WriteLine(str);
        }

        public static int Read(byte[] buffer, int offset)
        {
            int num;
            if (m_UnpackBuffer == null)
            {
                m_UnpackBuffer = new byte[0x800];
            }
            try
            {
                num = m_Server.Receive(m_UnpackBuffer, 0, 0x800, SocketFlags.None);
            }
            catch (SocketException exception)
            {
                Debug.Trace("SocketException caught in Network.Read()");
                Debug.Trace("Error Code: 0x{0:X}", exception.ErrorCode);
                Debug.Trace("Native Error Code: 0x{0:X}", exception.NativeErrorCode);
                Debug.Error(exception);
                Gumps.MessageBoxOk("Connection lost", true, new OnClick(Engine.DestroyDialogShowAcctLogin_OnClick));
                Disconnect();
                Cursor.Hourglass = false;
                m_SoftDisconnect = false;
                Engine.amMoving = false;
                return -1;
            }
            if (num <= 0)
            {
                if (num == 0)
                {
                    Debug.Trace("Soft disconnect caught in Network.Read()");
                }
                else
                {
                    Debug.Trace("Hard disconnect caught in Network.Read()");
                    Debug.Trace("Return value: {0} (0x{0:X})", num);
                }
                Gumps.MessageBoxOk("Connection lost", true, new OnClick(Engine.DestroyDialogShowAcctLogin_OnClick));
                Disconnect();
                Cursor.Hourglass = false;
                m_SoftDisconnect = false;
                Engine.amMoving = false;
                return -1;
            }
            return m_CryptoProvider.Decrypt(m_UnpackBuffer, 0, num, buffer, offset);
        }

        public static bool Send(Packet p)
        {
            if ((m_Server == null) || (p == null))
            {
                return false;
            }
            byte[] buffer = p.Compile();
            if (OnSend != null)
            {
                OnSend(buffer.Length);
            }
            if (p.Encode)
            {
                m_CryptoProvider.Encrypt(buffer, 0, buffer.Length);
            }
            try
            {
                if (buffer.Length > (m_SendBuffer.Length - m_SendLength))
                {
                    if (m_SendLength > 0)
                    {
                        m_Server.Send(m_SendBuffer, 0, m_SendLength, SocketFlags.None);
                    }
                    m_SendLength = 0;
                    if (buffer.Length >= m_SendBuffer.Length)
                    {
                        m_Server.Send(buffer, 0, buffer.Length, SocketFlags.None);
                    }
                    else
                    {
                        Buffer.BlockCopy(buffer, 0, m_SendBuffer, 0, buffer.Length);
                        m_SendLength += buffer.Length;
                    }
                }
                else
                {
                    Buffer.BlockCopy(buffer, 0, m_SendBuffer, m_SendLength, buffer.Length);
                    m_SendLength += buffer.Length;
                }
                m_LastNetworkActivity.Reset();
            }
            catch (SocketException exception)
            {
                Debug.Trace("SocketException caught in Network.Send()");
                Debug.Trace("Error Code: 0x{0:X}", exception.ErrorCode);
                Debug.Trace("Native Error Code: 0x{0:X}", exception.NativeErrorCode);
                Debug.Error(exception);
                Gumps.MessageBoxOk("Connection lost", true, new OnClick(Engine.DestroyDialogShowAcctLogin_OnClick));
                Disconnect();
                Cursor.Hourglass = false;
                m_SoftDisconnect = false;
                Engine.amMoving = false;
                p.Dispose();
                return false;
            }
            p.Dispose();
            return true;
        }

        public static void SetupCrypto(uint seed)
        {
            if (m_Decompress)
            {
                m_CryptoProvider = new GameCrypto(seed);
            }
            else
            {
                m_CryptoProvider = new LoginCrypto(seed);
            }
        }

        public static unsafe bool Slice()
        {
            if (m_Server == null)
            {
                Thread.Sleep(1);
                return true;
            }
            if ((m_LastNetworkActivity.Elapsed && m_Server.Poll(1, SelectMode.SelectRead)) && (m_Server.Available == 0))
            {
                Debug.Trace("Disconnected");
                if (!m_SoftDisconnect)
                {
                    Gumps.MessageBoxOk("Connection lost", true, new OnClick(Engine.DestroyDialogShowAcctLogin_OnClick));
                }
                Disconnect();
                Cursor.Hourglass = false;
                m_SoftDisconnect = false;
                Engine.amMoving = false;
                return true;
            }
            if (m_Server.Available > 0)
            {
                if (m_Buffer == null)
                {
                    m_Buffer = new byte[0x10000];
                }
                int num = Read(m_Buffer, m_CurrFilled);
                if (num == -1)
                {
                    return true;
                }
                m_CurrFilled += num;
                int currFilled = m_CurrFilled;
                int index = 0;
                while (index < currFilled)
                {
                    byte num4 = m_Buffer[index];
                    PacketHandler ph = PacketHandlers.m_Handlers[num4];
                    if (ph == null)
                    {
                        PacketReader.Initialize(m_Buffer, index, m_CurrFilled - index, true, num4, "Unknown").Trace();
                        m_CurrFilled = 0;
                        return true;
                    }
                    int length = ph.Length;
                    if (length == -1)
                    {
                        if ((currFilled - index) < 3)
                        {
                            break;
                        }
                        length = m_Buffer[index + 2] | (m_Buffer[index + 1] << 8);
                    }
                    if ((currFilled - index) < length)
                    {
                        break;
                    }
                    if (OnRecv != null)
                    {
                        OnRecv(length);
                    }
                    PacketReader pvSrc = PacketReader.Initialize(m_Buffer, index, length, ph.Length != -1, num4, ph.Name);
                    ph.Handle(pvSrc);
                    m_LastNetworkActivity.Reset();
                    if (OnPacketHandle != null)
                    {
                        OnPacketHandle(ph);
                    }
                    index += length;
                    if (m_CurrFilled != currFilled)
                    {
                        return true;
                    }
                }
                m_CurrFilled -= index;
                if (m_CurrFilled > 0)
                {
                    fixed (byte* numRef = m_Buffer)
                    {
                        byte* numPtr = numRef;
                        byte* numPtr2 = numRef + index;
                        byte* numPtr3 = numPtr + m_CurrFilled;
                        while (numPtr < numPtr3)
                        {
                            *(numPtr++) = *(numPtr2++);
                        }
                    }
                }
            }
            return true;
        }

        private static void Thread_LoadUnpackCache(object state)
        {
            ManualResetEvent event2 = (ManualResetEvent)state;
            LoadUnpackCache();
            event2.Set();
        }

        public static void Trace(string msg)
        {
            GetProtocol();
            m_Protocol.WriteLine(msg);
        }

        public static void Trace(string Format, object Obj0)
        {
            Trace(string.Format(Format, Obj0));
        }

        public static void Trace(string Format, params object[] Params)
        {
            Trace(string.Format(Format, Params));
        }

        public static void Trace(string Format, object Obj0, object Obj1)
        {
            Trace(string.Format(Format, Obj0, Obj1));
        }

        public static void Trace(string Format, object Obj0, object Obj1, object Obj2)
        {
            Trace(string.Format(Format, Obj0, Obj1, Obj2));
        }

        public static int ClientIP
        {
            get
            {
                if (m_ClientIP == -1)
                {
                    try
                    {
                        IPHostEntry hostByName = Dns.GetHostByName(Dns.GetHostName());
                        if (hostByName.AddressList.Length > 0)
                        {
                            m_ClientIP = (int)hostByName.AddressList[0].Address;
                            m_ClientIP = ((((m_ClientIP & 0xff) << 0x18) | ((m_ClientIP & 0xff00) << 8)) | ((m_ClientIP >> 8) & 0xff00)) | ((m_ClientIP >> 0x18) & 0xff);
                        }
                        else
                        {
                            m_ClientIP = 0x100007f;
                        }
                    }
                    catch
                    {
                        m_ClientIP = 0x100007f;
                    }
                }
                return m_ClientIP;
            }
        }

        public static IPAddress ServerIP
        {
            get
            {
                return m_ServerIP;
            }
        }

        public static IPEndPoint ServerIPEP
        {
            get
            {
                return m_ServerIPEP;
            }
        }
    }
}