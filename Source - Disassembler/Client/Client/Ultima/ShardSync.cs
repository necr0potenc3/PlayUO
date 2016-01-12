namespace Client
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    public class ShardSync
    {
        private AccountProfile m_Account;
        private byte[] m_Buffer;
        private byte[] m_Compressed;
        private BaseCrypto m_CryptoProvider;
        private int m_Length;
        private ServerProfile m_Server;
        private ShardProfile m_Shard;
        private Socket m_Socket;

        public ShardSync(ServerProfile server, AccountProfile account, ShardProfile shard)
        {
            this.m_Server = server;
            this.m_Account = account;
            this.m_Shard = shard;
            this.m_Compressed = new byte[0x800];
            this.m_Buffer = new byte[0x800];
            this.m_CryptoProvider = new GameCrypto((uint)shard.Auth);
            this.Connect(new IPEndPoint(shard.Address, shard.Port));
        }

        public void CharacterList(PacketReader pvSrc)
        {
            int num = pvSrc.ReadByte();
            for (int i = 0; i < 5; i++)
            {
                string name = pvSrc.ReadString(60);
                if (name.Length > 0)
                {
                    CharacterProfile profile = null;
                    for (int j = 0; (profile == null) && (j < this.m_Shard.Characters.Length); j++)
                    {
                        if (this.m_Shard.Characters[j].Index == i)
                        {
                            profile = this.m_Shard.Characters[j];
                        }
                    }
                    if (profile != null)
                    {
                        profile.Name = name;
                    }
                    else
                    {
                        this.m_Shard.AddCharacter(new CharacterProfile(this.m_Shard, name, i));
                    }
                }
                else
                {
                    CharacterProfile character = null;
                    for (int k = 0; (character == null) && (k < this.m_Shard.Characters.Length); k++)
                    {
                        if (this.m_Shard.Characters[k].Index == i)
                        {
                            character = this.m_Shard.Characters[k];
                        }
                    }
                    if (character != null)
                    {
                        this.m_Shard.RemoveCharacter(character);
                    }
                }
            }
            Array.Sort(this.m_Shard.Characters, new CharacterComparer());
            new Client.Timer(new OnTick(this.Update_OnTick), 0, 1).Start(false);
        }

        public void Connect(IPEndPoint ipep)
        {
            this.m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.m_Socket.BeginConnect(ipep, new AsyncCallback(this.OnConnect), null);
        }

        public void OnConnect(IAsyncResult result)
        {
            this.m_Socket.EndConnect(result);
            this.Send(new PGameSeed(this.m_Shard.Auth));
            this.Send(new PGameLogin(this.m_Shard.Auth, this.m_Account.Username, this.m_Account.Password));
            this.m_Socket.BeginReceive(this.m_Compressed, 0, this.m_Compressed.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), null);
        }

        public void OnReceive(IAsyncResult result)
        {
            int count = this.m_Socket.EndReceive(result);
            switch (count)
            {
                case 0:
                case -1:
                    this.m_Socket.Close();
                    return;

                default:
                    count = this.m_CryptoProvider.Decrypt(this.m_Compressed, 0, count, this.m_Buffer, this.m_Length);
                    this.m_Length += count;
                    for (int i = 0; this.m_Length > 0; i = 0)
                    {
                        if (this.m_Buffer[0] == 0xa9)
                        {
                            if (this.m_Length >= 3)
                            {
                                i = (this.m_Buffer[1] << 8) | this.m_Buffer[2];
                                if (this.m_Length >= i)
                                {
                                    PacketReader pvSrc = new PacketReader(this.m_Buffer, 0, i, false, 0xa8, "Character List");
                                    this.CharacterList(pvSrc);
                                    this.m_Socket.Close();
                                    ShardProfile[] shards = this.m_Account.Shards;
                                    int index = Array.IndexOf(shards, this.m_Shard) + 1;
                                    if ((index >= 0) && (index < shards.Length))
                                    {
                                        new ServerSync(this.m_Server, this.m_Account, shards[index]);
                                    }
                                    return;
                                }
                                i = 0;
                            }
                        }
                        else if (this.m_Buffer[0] == 0xb9)
                        {
                            i = 3;
                            if (this.m_Length < i)
                            {
                                i = 0;
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
            this.m_Socket.BeginReceive(this.m_Compressed, 0, this.m_Compressed.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), null);
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

        public void Update_OnTick(Client.Timer t)
        {
            Profiles.Save();
            GMenuItem item = this.m_Shard.Menu;
            if (item == null)
            {
                Engine.UpdateSmartLoginMenu();
            }
            else
            {
                for (int i = 0; i < this.m_Shard.Characters.Length; i++)
                {
                    GMenuItem item2 = this.m_Shard.Characters[i].Menu;
                    if (item2 == null)
                    {
                        item.Add(this.m_Shard.Characters[i].Menu = item2 = new GPlayCharacterMenu(this.m_Shard.Characters[i]));
                    }
                    else
                    {
                        item2.Text = this.m_Shard.Characters[i].Name;
                    }
                }
                Gump[] gumpArray = item.Children.ToArray();
                for (int j = 0; j < gumpArray.Length; j++)
                {
                    GPlayCharacterMenu child = gumpArray[j] as GPlayCharacterMenu;
                    if ((child != null) && !this.m_Shard.Contains(child.Character))
                    {
                        item.Remove(child);
                    }
                }
            }
        }
    }
}