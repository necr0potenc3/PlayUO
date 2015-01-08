namespace Client
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    public class ServerSync
    {
        private AccountProfile m_Account;
        private byte[] m_Buffer;
        private BaseCrypto m_CryptoProvider;
        private int m_Length;
        private ServerProfile m_Server;
        private ShardProfile m_Shard;
        private Socket m_Socket;

        public ServerSync(ServerProfile server, AccountProfile account, ShardProfile shard)
        {
            this.m_Server = server;
            this.m_Account = account;
            this.m_Shard = shard;
            this.m_Buffer = new byte[0x800];
            this.m_CryptoProvider = new LoginCrypto((uint) Network.ClientIP);
            Dns.BeginResolve(server.Address, new AsyncCallback(this.OnResolve), null);
        }

        public void Connect(IPEndPoint ipep)
        {
            this.m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.m_Socket.BeginConnect(ipep, new AsyncCallback(this.OnConnect), null);
        }

        public void OnConnect(IAsyncResult result)
        {
            try
            {
                this.m_Socket.EndConnect(result);
                this.Send(new PLoginSeed());
                this.Send(new PAccount(this.m_Account.Username, this.m_Account.Password));
                this.m_Socket.BeginReceive(this.m_Buffer, 0, this.m_Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), null);
            }
            catch
            {
            }
        }

        public void OnReceive(IAsyncResult result)
        {
            int num = this.m_Socket.EndReceive(result);
            switch (num)
            {
                case 0:
                case -1:
                    this.m_Socket.Close();
                    return;

                default:
                    this.m_Length += num;
                    for (int i = 0; this.m_Length > 0; i = 0)
                    {
                        if (this.m_Buffer[0] == 0xa8)
                        {
                            if (this.m_Length >= 3)
                            {
                                i = (this.m_Buffer[1] << 8) | this.m_Buffer[2];
                                if (this.m_Length < i)
                                {
                                    i = 0;
                                }
                                else
                                {
                                    PacketReader pvSrc = new PacketReader(this.m_Buffer, 0, i, false, 0xa8, "Shard List");
                                    this.ShardList(pvSrc);
                                }
                            }
                        }
                        else if (this.m_Buffer[0] == 140)
                        {
                            i = 11;
                            if (this.m_Length < i)
                            {
                                i = 0;
                            }
                            else
                            {
                                PacketReader reader2 = new PacketReader(this.m_Buffer, 0, i, true, 140, "Shard Relay");
                                this.ShardRelay(reader2);
                                this.m_Socket.Close();
                                return;
                            }
                        }
                        if (i == 0)
                        {
                            break;
                        }
                        this.m_Length -= i;
                        for (int j = 0; j < this.m_Length; j++)
                        {
                            this.m_Buffer[j] = this.m_Buffer[j + i];
                        }
                    }
                    break;
            }
            this.m_Socket.BeginReceive(this.m_Buffer, this.m_Length, this.m_Buffer.Length - this.m_Length, SocketFlags.None, new AsyncCallback(this.OnReceive), null);
        }

        public void OnResolve(IAsyncResult result)
        {
            IPHostEntry entry = Dns.EndResolve(result);
            if (entry.AddressList.Length > 0)
            {
                this.Connect(new IPEndPoint(entry.AddressList[0], this.m_Server.Port));
            }
        }

        public void Send(Packet p)
        {
            byte[] buffer = p.Compile();
            if (p.Encode)
            {
                this.m_CryptoProvider.Encrypt(buffer, 0, buffer.Length);
            }
            this.m_Socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        public void ShardList(PacketReader pvSrc)
        {
            pvSrc.ReadByte();
            int num = pvSrc.ReadInt16();
            if (num > 0)
            {
                int port = this.m_Server.Port;
                switch (port)
                {
                    case 0x1e5f:
                    case 0x1e60:
                        port = 0x1389;
                        break;
                }
                for (int i = 0; i < num; i++)
                {
                    int index = pvSrc.ReadInt16();
                    string name = pvSrc.ReadString(0x20);
                    int percentFull = pvSrc.ReadByte();
                    int timeZone = pvSrc.ReadSByte();
                    IPAddress address = new IPAddress((long) pvSrc.ReadUInt32());
                    if (this.m_Shard == null)
                    {
                        ShardProfile profile = null;
                        for (int j = 0; (profile == null) && (j < this.m_Account.Shards.Length); j++)
                        {
                            if (this.m_Account.Shards[j].Name == name)
                            {
                                profile = this.m_Account.Shards[j];
                            }
                        }
                        if (profile == null)
                        {
                            this.m_Account.AddShard(profile = new ShardProfile(this.m_Account, address, port, index, timeZone, percentFull, 0xbadf00d, name));
                        }
                        else
                        {
                            profile.Index = index;
                            profile.Address = address;
                            profile.Port = port;
                            profile.TimeZone = timeZone;
                            profile.PercentFull = percentFull;
                        }
                        if (i == 0)
                        {
                            new Timer(new OnTick(this.Update_OnTick), 0, 1).Start(false);
                        }
                    }
                }
                ShardProfile[] shards = this.m_Account.Shards;
                if (shards.Length > 0)
                {
                    if (this.m_Shard == null)
                    {
                        Array.Sort(shards, new ShardComparer());
                        this.m_Shard = shards[0];
                    }
                    this.Send(new PServerSelection(this.m_Shard.Index));
                }
            }
        }

        public void ShardRelay(PacketReader pvSrc)
        {
            int num = ((pvSrc.ReadByte() | (pvSrc.ReadByte() << 8)) | (pvSrc.ReadByte() << 0x10)) | (pvSrc.ReadByte() << 0x18);
            int num2 = pvSrc.ReadInt16();
            int num3 = pvSrc.ReadInt32();
            if (this.m_Shard != null)
            {
                this.m_Shard.Auth = num3;
                this.m_Shard.Address = new IPAddress((long) ((ulong) num));
                this.m_Shard.Port = num2;
                new ShardSync(this.m_Server, this.m_Account, this.m_Shard);
            }
        }

        public void Update_OnTick(Timer t)
        {
            Profiles.Save();
            GMenuItem item = this.m_Account.Menu;
            if ((item == null) || ((item == this.m_Server.Menu) && (this.m_Account.Shards.Length != 1)))
            {
                Engine.UpdateSmartLoginMenu();
            }
            else if (this.m_Account.Shards.Length > 1)
            {
                for (int i = 0; i < this.m_Account.Shards.Length; i++)
                {
                    GMenuItem item2 = this.m_Account.Shards[i].Menu;
                    if (item2 == null)
                    {
                        item.Add(this.m_Account.Shards[i].Menu = item2 = new GShardMenu(this.m_Account.Shards[i]));
                    }
                    else
                    {
                        item2.Text = this.m_Account.Shards[i].Name;
                    }
                }
                Gump[] gumpArray = item.Children.ToArray();
                for (int j = 0; j < gumpArray.Length; j++)
                {
                    GShardMenu child = gumpArray[j] as GShardMenu;
                    if ((child != null) && !this.m_Account.Contains(child.Shard))
                    {
                        item.Remove(child);
                    }
                }
            }
        }
    }
}

