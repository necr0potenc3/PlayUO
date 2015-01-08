namespace Client
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Xml;

    public class Profiles
    {
        private static ServerProfile[] m_List;

        public static void AddServer(ServerProfile server)
        {
            ServerProfile[] list = List;
            m_List = new ServerProfile[list.Length + 1];
            for (int i = 0; i < list.Length; i++)
            {
                m_List[i] = list[i];
            }
            m_List[list.Length] = server;
        }

        private static ServerProfile[] Load()
        {
            if (!File.Exists(Engine.FileManager.BasePath("Data/Profiles.xml")))
            {
                return new ServerProfile[0];
            }
            XmlDocument document = new XmlDocument();
            document.Load(Engine.FileManager.BasePath("Data/Profiles.xml"));
            ArrayList list = new ArrayList();
            foreach (XmlElement element in document.GetElementsByTagName("server"))
            {
                string attribute = element.GetAttribute("title");
                string address = element.GetAttribute("address");
                string s = element.GetAttribute("port");
                ServerProfile server = new ServerProfile(attribute, address, XmlConvert.ToInt32(s));
                foreach (XmlElement element2 in element.GetElementsByTagName("account"))
                {
                    string title = element2.GetAttribute("title");
                    string username = element2.GetAttribute("username");
                    string password = element2.GetAttribute("password");
                    AccountProfile account = new AccountProfile(server, title, username, password);
                    foreach (XmlElement element3 in element2.GetElementsByTagName("shard"))
                    {
                        string name = element3.GetAttribute("name");
                        string ipString = element3.GetAttribute("address");
                        string str10 = element3.GetAttribute("port");
                        string str11 = element3.GetAttribute("index");
                        string str12 = element3.GetAttribute("timeZone");
                        string str13 = element3.GetAttribute("percentFull");
                        string str14 = element3.GetAttribute("auth");
                        ShardProfile shard = new ShardProfile(account, IPAddress.Parse(ipString), XmlConvert.ToInt32(str10), XmlConvert.ToInt32(str11), XmlConvert.ToInt32(str12), XmlConvert.ToInt32(str13), XmlConvert.ToInt32(str14), name);
                        foreach (XmlElement element4 in element3.GetElementsByTagName("character"))
                        {
                            string str15 = element4.GetAttribute("index");
                            string str16 = element4.GetAttribute("name");
                            shard.AddCharacter(new CharacterProfile(shard, str16, XmlConvert.ToInt32(str15)));
                        }
                        account.AddShard(shard);
                    }
                    server.AddAccount(account);
                }
                list.Add(server);
            }
            return (ServerProfile[]) list.ToArray(typeof(ServerProfile));
        }

        public static void Save()
        {
            ServerProfile[] list = m_List;
            if (list != null)
            {
                XmlTextWriter writer = new XmlTextWriter(Engine.FileManager.BasePath("Data/Profiles.xml"), Encoding.UTF8) {
                    Indentation = 3,
                    IndentChar = ' ',
                    Formatting = Formatting.Indented
                };
                writer.WriteStartDocument(true);
                writer.WriteStartElement("profiles");
                foreach (ServerProfile profile in list)
                {
                    writer.WriteStartElement("server");
                    writer.WriteAttributeString("title", profile.Title);
                    writer.WriteAttributeString("address", profile.Address);
                    writer.WriteAttributeString("port", XmlConvert.ToString(profile.Port));
                    foreach (AccountProfile profile2 in profile.Accounts)
                    {
                        writer.WriteStartElement("account");
                        writer.WriteAttributeString("title", profile2.Title);
                        writer.WriteAttributeString("username", profile2.Username);
                        writer.WriteAttributeString("password", profile2.Password);
                        foreach (ShardProfile profile3 in profile2.Shards)
                        {
                            writer.WriteStartElement("shard");
                            writer.WriteAttributeString("name", profile3.Name);
                            writer.WriteAttributeString("address", profile3.Address.ToString());
                            writer.WriteAttributeString("port", XmlConvert.ToString(profile3.Port));
                            writer.WriteAttributeString("index", XmlConvert.ToString(profile3.Index));
                            writer.WriteAttributeString("timeZone", XmlConvert.ToString(profile3.TimeZone));
                            writer.WriteAttributeString("percentFull", XmlConvert.ToString(profile3.PercentFull));
                            writer.WriteAttributeString("auth", XmlConvert.ToString(profile3.Auth));
                            foreach (CharacterProfile profile4 in profile3.Characters)
                            {
                                writer.WriteStartElement("character");
                                writer.WriteAttributeString("index", XmlConvert.ToString(profile4.Index));
                                writer.WriteAttributeString("name", profile4.Name);
                                writer.WriteEndElement();
                            }
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.Close();
            }
        }

        public static ServerProfile[] List
        {
            get
            {
                if (m_List == null)
                {
                    m_List = Load();
                }
                return m_List;
            }
            set
            {
                m_List = value;
            }
        }
    }
}

