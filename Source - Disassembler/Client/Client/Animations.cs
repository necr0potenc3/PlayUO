namespace Client
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Threading;

    public class Animations
    {
        private const int DoubleOpaque = -2147450880;
        private const int DoubleXor = -2145386496;
        private int m_Action;
        private static Hashtable m_ActionDef1;
        private static Hashtable m_ActionDef2;
        private int m_BodyID;
        private int m_Count;
        private int m_Count2;
        private int m_Count3;
        private int m_Count4;
        private byte[] m_Data;
        public ArrayList m_Frames = new ArrayList();
        public Entry3D[] m_Index;
        public Entry3D[] m_Index2;
        public Entry3D[] m_Index3;
        public Entry3D[] m_Index4;
        private static Loader m_Loader;
        private static Loader m_Loader2;
        private static Loader m_Loader3;
        private static Loader m_Loader4;
        private Client.MountTable m_MountTable;
        private short[] m_Palette = new short[0x100];
        private int m_SA_Body;
        private int m_SA_Dir;
        private static Stream m_Stream;
        private static Stream m_Stream2;
        private static Stream m_Stream3;
        private static Stream m_Stream4;
        private int[] m_Table;
        private static BodyType[] m_Types;

        public Animations()
        {
            if (File.Exists("Data/Binary/BodyTypes.mul"))
            {
                using (BinaryReader reader = new BinaryReader(new FileStream("Data/Binary/BodyTypes.mul", FileMode.Open, FileAccess.Read, FileShare.Read)))
                {
                    m_Types = new BodyType[(int) reader.BaseStream.Length];
                    for (int i = 0; i < m_Types.Length; i++)
                    {
                        m_Types[i] = (BodyType) reader.ReadByte();
                    }
                    if (970 < m_Types.Length)
                    {
                        m_Types[970] = BodyType.Human;
                    }
                }
            }
            else
            {
                Console.WriteLine("Warning: Data/Binary/BodyTypes.mul does not exist");
                m_Types = new BodyType[0];
            }
            try
            {
                m_Stream = Engine.FileManager.OpenMUL(Files.AnimMul);
            }
            catch
            {
            }
            try
            {
                m_Stream2 = Engine.FileManager.OpenMUL("Anim2.mul");
            }
            catch
            {
            }
            try
            {
                m_Stream3 = Engine.FileManager.OpenMUL("Anim3.mul");
            }
            catch
            {
            }
            try
            {
                m_Stream4 = Engine.FileManager.OpenMUL("Anim4.mul");
            }
            catch
            {
            }
            m_Loader = new Loader(this, 1);
            m_Loader2 = new Loader(this, 2);
            m_Loader3 = new Loader(this, 3);
            m_Loader4 = new Loader(this, 4);
        }

        public int ConvertAction(int BodyID, int Serial, int X, int Y, int Direction, GenericAction g, Mobile m)
        {
            this.Translate(ref BodyID);
            BodyType bodyType = this.GetBodyType(BodyID);
            this.m_SA_Body = BodyID;
            this.m_SA_Dir = Direction;
            if (bodyType == BodyType.Monster)
            {
                switch (g)
                {
                    case GenericAction.Die:
                        return this.SafeAction(2 + ((Direction >> 7) & 1), 2, 3);

                    case GenericAction.MountedWalk:
                        return 0;

                    case GenericAction.MountedRun:
                        return 0;

                    case GenericAction.Walk:
                        return 0;

                    case GenericAction.Run:
                        return 0;

                    case GenericAction.MountedStand:
                        return 1;

                    case GenericAction.Stand:
                        return 1;
                }
            }
            else if ((bodyType == BodyType.Animal) || (bodyType == BodyType.Sea))
            {
                switch (g)
                {
                    case GenericAction.Die:
                        return this.SafeAction(8 + (((Direction >> 7) & 1) * 4), 8, 12);

                    case GenericAction.MountedWalk:
                        return 0;

                    case GenericAction.MountedRun:
                        return this.SafeAction(1, 0);

                    case GenericAction.Walk:
                        return 0;

                    case GenericAction.Run:
                        return this.SafeAction(1, 0);

                    case GenericAction.MountedStand:
                        return 2;

                    case GenericAction.Stand:
                        return 2;
                }
            }
            else
            {
                switch (bodyType)
                {
                    case BodyType.Human:
                    case BodyType.Equipment:
                        switch (g)
                        {
                            case GenericAction.Die:
                                return this.SafeAction(0x15 + ((Direction >> 7) & 1), 0x15, 0x16);

                            case GenericAction.MountedWalk:
                                return 0x17;

                            case GenericAction.MountedRun:
                                return 0x18;

                            case GenericAction.Walk:
                                if ((m == null) || !m.Warmode)
                                {
                                    if (m.UsingTwoHandedWeapon())
                                    {
                                        return this.SafeAction(1, 0);
                                    }
                                    return this.SafeAction(0, 1, 15);
                                }
                                return this.SafeAction(15, m.UsingTwoHandedWeapon() ? 1 : 0, 0);

                            case GenericAction.Run:
                                if ((m == null) || !m.UsingTwoHandedWeapon())
                                {
                                    return this.SafeAction(2, 3);
                                }
                                return this.SafeAction(3, 2);

                            case GenericAction.MountedStand:
                                return 0x19;

                            case GenericAction.Stand:
                                if ((m == null) || !m.Warmode)
                                {
                                    return this.SafeAction(4, 7, 8);
                                }
                                if (!m.UsingTwoHandedWeapon())
                                {
                                    return this.SafeAction(7, 8, 4);
                                }
                                return this.SafeAction(8, 7, 4);
                        }
                        break;
                }
            }
            return 0;
        }

        public int ConvertMountItemToBody(int itemID)
        {
            if (this.m_MountTable == null)
            {
                this.m_MountTable = new Client.MountTable();
            }
            return this.m_MountTable.Translate(itemID);
        }

        public int ConvertRealID(ref int realID)
        {
            int num;
            int num2;
            int num3;
            if (realID >= 0x88b8)
            {
                num = 400 + ((realID - 0x88b8) / 0xaf);
                num2 = ((realID - 0x88b8) % 0xaf) / 5;
                num3 = ((realID - 0x88b8) % 0xaf) % 5;
            }
            else if (realID >= 0x55f0)
            {
                num = 200 + ((realID - 0x55f0) / 0x41);
                num2 = ((realID - 0x55f0) % 0x41) / 5;
                num3 = ((realID - 0x55f0) % 0x41) % 5;
            }
            else
            {
                num = realID / 110;
                num2 = (realID % 110) / 5;
                num3 = (realID % 110) % 5;
            }
            int num4 = BodyConverter.Convert(ref num);
            switch (num4)
            {
                case 2:
                    if (num < 200)
                    {
                        realID = ((num * 110) + (num2 * 5)) + num3;
                        return num4;
                    }
                    realID = ((0x55f0 + ((num - 200) * 0x41)) + (num2 * 5)) + num3;
                    return num4;

                case 3:
                    if (num < 300)
                    {
                        realID = ((num * 0x41) + (num2 * 5)) + num3;
                        return num4;
                    }
                    if (num < 400)
                    {
                        realID = ((0x80e8 + ((num - 300) * 110)) + (num2 * 5)) + num3;
                        return num4;
                    }
                    realID = ((0x88b8 + ((num - 400) * 0xaf)) + (num2 * 5)) + num3;
                    return num4;
            }
            if (num4 == 4)
            {
                if (num < 200)
                {
                    realID = ((num * 110) + (num2 * 5)) + num3;
                    return num4;
                }
                if (num < 400)
                {
                    realID = ((0x55f0 + ((num - 200) * 0x41)) + (num2 * 5)) + num3;
                    return num4;
                }
                realID = ((0x88b8 + ((num - 400) * 0xaf)) + (num2 * 5)) + num3;
            }
            return num4;
        }

        public unsafe object Create(int realID, IHue hue)
        {
            int length;
            int lookup;
            int num3;
            Stream stream;
            switch (this.ConvertRealID(ref realID))
            {
                case 1:
                {
                    if (((realID < 0) || (realID >= this.m_Count)) || (realID >= this.m_Index.Length))
                    {
                        return Frames.Empty;
                    }
                    Entry3D entryd = this.m_Index[realID];
                    length = entryd.m_Length;
                    lookup = entryd.m_Lookup;
                    num3 = entryd.m_Extra & 0xff;
                    stream = m_Stream;
                    break;
                }
                case 2:
                {
                    if (((realID < 0) || (realID >= this.m_Count2)) || (realID >= this.m_Index2.Length))
                    {
                        return Frames.Empty;
                    }
                    Entry3D entryd2 = this.m_Index2[realID];
                    length = entryd2.m_Length;
                    lookup = entryd2.m_Lookup;
                    num3 = entryd2.m_Extra & 0xff;
                    stream = m_Stream2;
                    break;
                }
                case 3:
                {
                    if (((realID < 0) || (realID >= this.m_Count3)) || (realID >= this.m_Index3.Length))
                    {
                        return Frames.Empty;
                    }
                    Entry3D entryd3 = this.m_Index3[realID];
                    length = entryd3.m_Length;
                    lookup = entryd3.m_Lookup;
                    num3 = entryd3.m_Extra & 0xff;
                    stream = m_Stream3;
                    break;
                }
                default:
                {
                    if (((realID < 0) || (realID >= this.m_Count4)) || (realID >= this.m_Index4.Length))
                    {
                        return Frames.Empty;
                    }
                    Entry3D entryd4 = this.m_Index4[realID];
                    length = entryd4.m_Length;
                    lookup = entryd4.m_Lookup;
                    num3 = entryd4.m_Extra & 0xff;
                    stream = m_Stream4;
                    break;
                }
            }
            if (((lookup < 0) || (length <= 0)) || ((num3 <= 0) || (stream == null)))
            {
                return Frames.Empty;
            }
            if ((this.m_Data == null) || (length > this.m_Data.Length))
            {
                this.m_Data = new byte[length];
            }
            stream.Seek((long) lookup, SeekOrigin.Begin);
            stream.Read(this.m_Data, 0, length);
            fixed (short* numRef = this.m_Palette)
            {
                short* numPtr = numRef;
                fixed (byte* numRef2 = this.m_Data)
                {
                    if (hue.HueID() == 0)
                    {
                        numPtr = (short*) numRef2;
                        int* numPtr2 = (int*) numPtr;
                        int* numPtr3 = numPtr2 + 0x80;
                        while (numPtr2 < numPtr3)
                        {
                            numPtr2[0] |= -2147450880;
                            int* numPtr1 = numPtr2 + 1;
                            numPtr1[0] |= -2147450880;
                            int* numPtr9 = numPtr2 + 2;
                            numPtr9[0] |= -2147450880;
                            int* numPtr10 = numPtr2 + 3;
                            numPtr10[0] |= -2147450880;
                            numPtr2 += 4;
                        }
                    }
                    else
                    {
                        hue.CopyPixels((void*) numRef2, (void*) numPtr, 0x100);
                    }
                    Frames frames = new Frames {
                        FrameCount = num3,
                        FrameList = new Frame[num3]
                    };
                    for (int i = 0; i < num3; i++)
                    {
                        int num6 = (numRef2 + 0x204)[i << 2];
                        byte* numPtr4 = (numRef2 + 0x200) + num6;
                        short* numPtr5 = (short*) numPtr4;
                        int num7 = numPtr5[0];
                        int num8 = numPtr5[1];
                        int width = numPtr5[2];
                        int height = numPtr5[3];
                        numPtr4 += 8;
                        frames.FrameList[i] = new Frame();
                        frames.FrameList[i].CenterX = num7;
                        frames.FrameList[i].CenterY = num8;
                        if ((width <= 0) || (height <= 0))
                        {
                            frames.FrameList[i].Image = Texture.Empty;
                        }
                        else
                        {
                            Texture texture = new Texture(width, height, true);
                            if (texture.IsEmpty())
                            {
                                frames.FrameList[i].Image = Texture.Empty;
                            }
                            else
                            {
                                int num11 = 0;
                                short* numPtr6 = null;
                                int num12 = num7 - 0x200;
                                int num13 = (num8 + height) - 0x200;
                                LockData data = texture.Lock(LockFlags.WriteOnly);
                                short* pvSrc = (short*) data.pvSrc;
                                int num14 = data.Pitch >> 1;
                                pvSrc += num12;
                                pvSrc += num13 * num14;
                                while ((num11 = *((int*) numPtr4)) != 0x7fff7fff)
                                {
                                    numPtr4 += 4;
                                    num11 ^= -2145386496;
                                    numPtr6 = pvSrc + ((((num11 >> 12) & 0x3ff) * num14) + ((num11 >> 0x16) & 0x3ff));
                                    short* numPtr8 = numPtr6 + (num11 & 0xffc);
                                    while (numPtr6 < numPtr8)
                                    {
                                        numPtr6[0] = numPtr[numPtr4[0]];
                                        numPtr6[1] = numPtr[numPtr4[1]];
                                        numPtr6[2] = numPtr[numPtr4[2]];
                                        numPtr6[3] = numPtr[numPtr4[3]];
                                        numPtr6 += 4;
                                        numPtr4 += 4;
                                    }
                                    switch ((num11 & 3))
                                    {
                                        case 1:
                                            goto Label_04A6;

                                        case 2:
                                            break;

                                        case 3:
                                            numPtr6[2] = numPtr[numPtr4[2]];
                                            break;

                                        default:
                                            goto Label_04B5;
                                    }
                                    numPtr6[1] = numPtr[numPtr4[1]];
                                Label_04A6:
                                    numPtr6[0] = numPtr[numPtr4[0]];
                                Label_04B5:
                                    numPtr4 += num11 & 3;
                                }
                                texture.Unlock();
                                texture.SetPriority(0);
                                frames.FrameList[i].Image = texture;
                            }
                        }
                    }
                    this.m_Frames.Add(frames);
                    return frames;
                }
            }
        }

        public void Dispose()
        {
            m_Loader.Stop();
            m_Loader2.Stop();
            m_Loader3.Stop();
            m_Loader4.Stop();
            if (m_Stream != null)
            {
                m_Stream.Close();
                m_Stream = null;
            }
            if (m_Stream2 != null)
            {
                m_Stream2.Close();
                m_Stream2 = null;
            }
            if (m_Stream3 != null)
            {
                m_Stream3.Close();
                m_Stream3 = null;
            }
            if (m_Stream4 != null)
            {
                m_Stream4.Close();
                m_Stream4 = null;
            }
            if (m_ActionDef1 != null)
            {
                m_ActionDef1.Clear();
                m_ActionDef1 = null;
            }
            if (m_ActionDef2 != null)
            {
                m_ActionDef2.Clear();
                m_ActionDef2 = null;
            }
            if (this.m_MountTable != null)
            {
                this.m_MountTable.Dispose();
                this.m_MountTable = null;
            }
            this.m_Table = null;
            this.m_Data = null;
            this.m_Index = null;
            this.m_Palette = null;
        }

        public void DisposeInstance(object Anim)
        {
            Frames frames = (Frames) Anim;
            if ((frames != null) && (frames.FrameList != null))
            {
                int length = frames.FrameList.Length;
                for (int i = 0; i < length; i++)
                {
                    if ((frames.FrameList[i] != null) && (frames.FrameList[i].Image != null))
                    {
                        frames.FrameList[i].Image.Dispose();
                        frames.FrameList[i].Image = null;
                    }
                }
            }
            Anim = null;
        }

        public void FullCleanup(int timeNow)
        {
            int num = timeNow - 0x3a98;
            for (int i = this.m_Frames.Count - 1; i >= 0; i--)
            {
                Frames frames = (Frames) this.m_Frames[i];
                if (frames.Disposed || (frames.LastAccessTime < num))
                {
                    TextureFactory.m_Disposing.Enqueue(frames);
                }
            }
        }

        public BodyType GetBodyType(int body)
        {
            if ((body >= 0) && (body < m_Types.Length))
            {
                return m_Types[body];
            }
            return BodyType.Empty;
        }

        public int GetCount(int index)
        {
            switch (index)
            {
                case 2:
                    return this.m_Count2;

                case 3:
                    return this.m_Count3;

                case 4:
                    return this.m_Count4;
            }
            return this.m_Count;
        }

        public Frame GetFrame(IAnimationOwner owner, int BodyID, int ActionID, int Direction, int Frame, int xCenter, int yCenter, IHue h, ref int TextureX, ref int TextureY, bool preserveHue)
        {
            Frames animation;
            if (BodyID <= 0)
            {
                return Frame.Empty;
            }
            this.m_Action = ActionID;
            Direction &= 7;
            int direction = Direction;
            if (Direction > 4)
            {
                direction -= (Direction - 4) * 2;
            }
            this.Translate(ref BodyID, ref h);
            this.m_BodyID = BodyID;
            int realID = this.GetRealIDNoMap(BodyID, ActionID, direction);
            if ((realID < 0) || (realID >= this.m_Count))
            {
                return Frame.Empty;
            }
            if (owner == null)
            {
                animation = h.GetAnimation(realID);
            }
            else
            {
                animation = owner.GetOwnedFrames(h, realID);
            }
            if ((Frame >= animation.FrameCount) || (Frame < 0))
            {
                return Frame.Empty;
            }
            Frame frame = animation.FrameList[Frame];
            if (((frame != null) && (frame.Image != null)) && !frame.Image.IsEmpty())
            {
                if (Direction > 4)
                {
                    TextureX = xCenter + (frame.CenterX - frame.Image.Width);
                }
                else
                {
                    TextureX = xCenter - frame.CenterX;
                }
                TextureY = (yCenter - frame.Image.Height) - frame.CenterY;
            }
            frame.Image.Flip = Direction > 4;
            return frame;
        }

        public int GetFrameCount(int realID)
        {
            int index = this.ConvertRealID(ref realID);
            Entry3D[] entrydArray = this.GetIndex(index);
            int count = this.GetCount(index);
            if (((realID < 0) || (realID >= count)) || (realID >= entrydArray.Length))
            {
                return 0;
            }
            return (entrydArray[realID].m_Extra & 0xff);
        }

        public int GetFrameCount(int bodyID, int actionID, int direction)
        {
            direction &= 7;
            int realID = this.GetRealID(bodyID, actionID, direction);
            int index = this.ConvertRealID(ref realID);
            Entry3D[] entrydArray = this.GetIndex(index);
            int count = this.GetCount(index);
            if (((realID < 0) || (realID >= count)) || (realID >= entrydArray.Length))
            {
                return 0;
            }
            return (entrydArray[realID].m_Extra & 0xff);
        }

        public int GetHeight(int realID)
        {
            int index = this.ConvertRealID(ref realID);
            Entry3D[] entrydArray = this.GetIndex(index);
            int count = this.GetCount(index);
            if (((realID < 0) || (realID >= count)) || (realID >= entrydArray.Length))
            {
                return 0;
            }
            return ((entrydArray[realID].m_Extra & -256) >> 8);
        }

        public int GetHeight(int bodyID, int actionID, int direction)
        {
            direction &= 7;
            int realID = this.GetRealID(bodyID, actionID, direction);
            int index = this.ConvertRealID(ref realID);
            Entry3D[] entrydArray = this.GetIndex(index);
            int count = this.GetCount(index);
            if (((realID < 0) || (realID >= count)) || (realID >= entrydArray.Length))
            {
                return 0;
            }
            return ((entrydArray[realID].m_Extra & -256) >> 8);
        }

        public Entry3D[] GetIndex(int index)
        {
            switch (index)
            {
                case 2:
                    return this.m_Index2;

                case 3:
                    return this.m_Index3;

                case 4:
                    return this.m_Index4;
            }
            return this.m_Index;
        }

        public int GetRealID(int BodyID, int ActionID, int Direction)
        {
            int num;
            Direction &= 7;
            this.Translate(ref BodyID);
            if (BodyID >= 400)
            {
                num = ((BodyID - 400) * 0xaf) + 0x88b8;
            }
            else if (BodyID >= 200)
            {
                num = ((BodyID - 200) * 0x41) + 0x55f0;
            }
            else
            {
                num = BodyID * 110;
            }
            if (Direction > 4)
            {
                Direction -= (Direction - 4) * 2;
            }
            return (num + ((ActionID * 5) + Direction));
        }

        public int GetRealIDNoMap(int body, int action, int direction)
        {
            int num;
            direction &= 7;
            if (body >= 400)
            {
                num = ((body - 400) * 0xaf) + 0x88b8;
            }
            else if (body >= 200)
            {
                num = ((body - 200) * 0x41) + 0x55f0;
            }
            else
            {
                num = body * 110;
            }
            if (direction > 4)
            {
                direction -= (direction - 4) * 2;
            }
            return (num + ((action * 5) + direction));
        }

        public bool IsValid(int bodyID, int action, int direction)
        {
            int realID = this.GetRealID(bodyID, action, direction);
            switch (this.ConvertRealID(ref realID))
            {
                case 1:
                    return (((realID >= 0) && (realID < this.m_Index.Length)) && (this.m_Index[realID].m_Lookup >= 0));

                case 2:
                    return (((realID >= 0) && (realID < this.m_Index2.Length)) && (this.m_Index2[realID].m_Lookup >= 0));

                case 3:
                    return (((realID >= 0) && (realID < this.m_Index3.Length)) && (this.m_Index3[realID].m_Lookup >= 0));
            }
            return (((realID >= 0) && (realID < this.m_Index4.Length)) && (this.m_Index4[realID].m_Lookup >= 0));
        }

        private Hashtable LoadActionDef(string fileName)
        {
            Hashtable hashtable = new Hashtable();
            string path = Engine.FileManager.ResolveMUL(fileName);
            if (File.Exists(path))
            {
                string str2;
                StreamReader reader = new StreamReader(path);
                while ((str2 = reader.ReadLine()) != null)
                {
                    if (((str2 = str2.Trim()).Length != 0) && !str2.StartsWith("#"))
                    {
                        try
                        {
                            int index = str2.IndexOf('{');
                            int num2 = str2.IndexOf('}');
                            string str3 = str2.Substring(0, index).Trim();
                            string str4 = str2.Substring(index + 1, (num2 - index) - 1).Trim();
                            int num3 = Convert.ToInt32(str3);
                            int num4 = Convert.ToInt32(str4);
                            hashtable[num3] = num4;
                            continue;
                        }
                        catch
                        {
                            Debug.Error("Bad def format");
                            continue;
                        }
                    }
                }
            }
            return hashtable;
        }

        private void LoadTable()
        {
            int num = 400 + ((this.m_Index.Length - 0x88b8) / 0xaf);
            this.m_Table = new int[num];
            for (int i = 0; i < num; i++)
            {
                object obj2 = BodyTable.m_Entries[i];
                if ((obj2 == null) || BodyConverter.Contains(i))
                {
                    this.m_Table[i] = i;
                }
                else
                {
                    BodyTableEntry entry = (BodyTableEntry) obj2;
                    this.m_Table[i] = (entry.m_OldID | -2147483648) | (((entry.m_NewHue ^ 0x8000) & 0xffff) << 15);
                }
            }
        }

        public int SafeAction(int desired, int fb1)
        {
            if (this.IsValid(this.m_SA_Body, desired, this.m_SA_Dir))
            {
                return desired;
            }
            return fb1;
        }

        public int SafeAction(int desired, int fb1, int fb2)
        {
            if (this.IsValid(this.m_SA_Body, desired, this.m_SA_Dir))
            {
                return desired;
            }
            if (this.IsValid(this.m_SA_Body, fb1, this.m_SA_Dir))
            {
                return fb1;
            }
            return fb2;
        }

        public static void StartLoading()
        {
            m_Loader.Start();
            m_Loader2.Start();
            m_Loader3.Start();
            m_Loader4.Start();
        }

        public void Translate(ref int bodyID)
        {
            if (this.m_Table == null)
            {
                this.LoadTable();
            }
            if ((bodyID <= 0) || (bodyID >= this.m_Table.Length))
            {
                bodyID = 0;
            }
            else
            {
                bodyID = this.m_Table[bodyID] & 0x7fff;
            }
        }

        public void Translate(ref int bodyID, ref IHue h)
        {
            if (this.m_Table == null)
            {
                this.LoadTable();
            }
            if ((bodyID <= 0) || (bodyID >= this.m_Table.Length))
            {
                bodyID = 0;
            }
            else
            {
                int num = this.m_Table[bodyID];
                if ((num & -2147483648) != 0)
                {
                    bodyID = num & 0x7fff;
                    if (h == Hues.Default)
                    {
                        h = Hues.Load((num >> 15) & 0xffff);
                    }
                }
            }
        }

        public void Translate(ref int bodyID, ref int hue)
        {
            if (this.m_Table == null)
            {
                this.LoadTable();
            }
            if ((bodyID <= 0) || (bodyID >= this.m_Table.Length))
            {
                bodyID = 0;
            }
            else
            {
                int num = this.m_Table[bodyID];
                if ((num & -2147483648) != 0)
                {
                    bodyID = num & 0x7fff;
                    if (hue == 0)
                    {
                        hue = (num >> 15) & 0xffff;
                    }
                }
            }
        }

        public void UpdateInstance(long SeedID, object Anim)
        {
        }

        public static bool WaitLoading()
        {
            if (m_Loader.IsAlive && !m_Loader.Wait())
            {
                return false;
            }
            if (m_Loader2.IsAlive && !m_Loader2.Wait())
            {
                return false;
            }
            if (m_Loader3.IsAlive && !m_Loader3.Wait())
            {
                return false;
            }
            if (m_Loader4.IsAlive && !m_Loader4.Wait())
            {
                return false;
            }
            return true;
        }

        public Hashtable ActionDef1
        {
            get
            {
                if (m_ActionDef1 == null)
                {
                    m_ActionDef1 = this.LoadActionDef("Anim1.def");
                }
                return m_ActionDef1;
            }
        }

        public Hashtable ActionDef2
        {
            get
            {
                if (m_ActionDef2 == null)
                {
                    m_ActionDef2 = this.LoadActionDef("Anim2.def");
                }
                return m_ActionDef2;
            }
        }

        public static bool IsLoading
        {
            get
            {
                return (((m_Loader.IsAlive || m_Loader2.IsAlive) || m_Loader3.IsAlive) || m_Loader4.IsAlive);
            }
        }

        public Client.MountTable MountTable
        {
            get
            {
                if (this.m_MountTable == null)
                {
                    this.m_MountTable = new Client.MountTable();
                }
                return this.m_MountTable;
            }
        }

        private class Loader
        {
            private int m_Index;
            private Animations m_Owner;
            private Thread m_Thread;

            public Loader(Animations owner, int index)
            {
                this.m_Owner = owner;
                this.m_Index = index;
                this.m_Thread = new Thread(new ThreadStart(this.Thread_Start));
                this.m_Thread.Name = "Background Animation Loader";
            }

            public void Start()
            {
                if (this.m_Thread != null)
                {
                    this.m_Thread.Start();
                }
            }

            public void Stop()
            {
                if ((this.m_Thread != null) && this.m_Thread.IsAlive)
                {
                    this.m_Thread.Abort();
                }
            }

            private unsafe void Thread_Start()
            {
                string path = Engine.FileManager.BasePath(string.Format("Data/QuickLoad/Anim{0}.mul", this.m_Index));
                string str2 = Engine.FileManager.ResolveMUL(string.Format("Anim{0}.mul", (this.m_Index == 1) ? "" : this.m_Index.ToString()));
                string str3 = Engine.FileManager.ResolveMUL(string.Format("Anim{0}.idx", (this.m_Index == 1) ? "" : this.m_Index.ToString()));
                if (!File.Exists(str2) || !File.Exists(str3))
                {
                    if (this.m_Index == 1)
                    {
                        this.m_Owner.m_Index = new Entry3D[0];
                        this.m_Owner.m_Count = 0;
                    }
                    else if (this.m_Index == 2)
                    {
                        this.m_Owner.m_Index2 = new Entry3D[0];
                        this.m_Owner.m_Count2 = 0;
                    }
                    else if (this.m_Index == 3)
                    {
                        this.m_Owner.m_Index3 = new Entry3D[0];
                        this.m_Owner.m_Count3 = 0;
                    }
                    else
                    {
                        this.m_Owner.m_Index4 = new Entry3D[0];
                        this.m_Owner.m_Count4 = 0;
                    }
                }
                else
                {
                    if (File.Exists(path))
                    {
                        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            if (stream.Length >= 0x15L)
                            {
                                BinaryReader reader = new BinaryReader(stream);
                                if (reader.ReadBoolean())
                                {
                                    DateTime timeStamp = Engine.GetTimeStamp(str2);
                                    DateTime time2 = Engine.GetTimeStamp(str3);
                                    DateTime time3 = DateTime.FromFileTime(reader.ReadInt64());
                                    DateTime time4 = DateTime.FromFileTime(reader.ReadInt64());
                                    if ((timeStamp == time3) && (time2 == time4))
                                    {
                                        int num = reader.ReadInt32();
                                        if (reader.BaseStream.Length >= (0x15 + (num * 12)))
                                        {
                                            Entry3D[] entrydArray = new Entry3D[num];
                                            entrydArray = new Entry3D[num];
                                            fixed (Entry3D* entrydRef = entrydArray)
                                            {
                                                Engine.NativeRead((FileStream) reader.BaseStream, (void*) entrydRef, num * 12);
                                            }
                                            if (this.m_Index == 1)
                                            {
                                                this.m_Owner.m_Index = entrydArray;
                                                this.m_Owner.m_Count = num;
                                            }
                                            else if (this.m_Index == 2)
                                            {
                                                this.m_Owner.m_Index2 = entrydArray;
                                                this.m_Owner.m_Count2 = num;
                                            }
                                            else if (this.m_Index == 3)
                                            {
                                                this.m_Owner.m_Index3 = entrydArray;
                                                this.m_Owner.m_Count3 = num;
                                            }
                                            else
                                            {
                                                this.m_Owner.m_Index4 = entrydArray;
                                                this.m_Owner.m_Count4 = num;
                                            }
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    using (FileStream stream2 = new FileStream(str3, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        int num2 = (int) (stream2.Length / 12L);
                        Entry3D[] entrydArray2 = new Entry3D[num2];
                        fixed (Entry3D* entrydRef2 = entrydArray2)
                        {
                            Engine.NativeRead(stream2, (void*) entrydRef2, num2 * 12);
                            using (FileStream stream3 = new FileStream(str2, FileMode.Open, FileAccess.Read, FileShare.Read))
                            {
                                BinaryReader reader2 = new BinaryReader(stream3);
                                Entry3D* entrydPtr = entrydRef2;
                                Entry3D* entrydPtr2 = entrydRef2 + num2;
                                while (entrydPtr < entrydPtr2)
                                {
                                    if (entrydPtr->m_Lookup >= 0)
                                    {
                                        reader2.BaseStream.Seek((long) (entrydPtr->m_Lookup + 0x200), SeekOrigin.Begin);
                                        int num3 = reader2.ReadInt32() & 0xff;
                                        int num4 = 0;
                                        int num5 = -10000;
                                        while (num4 < num3)
                                        {
                                            reader2.BaseStream.Seek((long) ((entrydPtr->m_Lookup + 0x204) + (num4 << 2)), SeekOrigin.Begin);
                                            reader2.BaseStream.Seek((long) ((entrydPtr->m_Lookup + 0x202) + reader2.ReadInt32()), SeekOrigin.Begin);
                                            int num6 = reader2.ReadInt16();
                                            int num7 = reader2.ReadInt32() >> 0x10;
                                            if ((num7 + num6) > num5)
                                            {
                                                num5 = num7 + num6;
                                            }
                                            num4++;
                                        }
                                        entrydPtr->m_Extra = num3 | (num5 << 8);
                                    }
                                    entrydPtr++;
                                }
                            }
                            using (FileStream stream4 = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                BinaryWriter writer = new BinaryWriter(stream4);
                                writer.Write(false);
                                writer.Write(Engine.GetTimeStamp(str2).ToFileTime());
                                writer.Write(Engine.GetTimeStamp(str3).ToFileTime());
                                writer.Write(num2);
                                Engine.NativeWrite((FileStream) writer.BaseStream, (void*) entrydRef2, num2 * 12);
                                writer.Seek(0, SeekOrigin.Begin);
                                writer.Write(true);
                            }
                        }
                        if (this.m_Index == 1)
                        {
                            this.m_Owner.m_Index = entrydArray2;
                            this.m_Owner.m_Count = num2;
                        }
                        else if (this.m_Index == 2)
                        {
                            this.m_Owner.m_Index2 = entrydArray2;
                            this.m_Owner.m_Count2 = num2;
                        }
                        else if (this.m_Index == 3)
                        {
                            this.m_Owner.m_Index3 = entrydArray2;
                            this.m_Owner.m_Count3 = num2;
                        }
                        else
                        {
                            this.m_Owner.m_Index4 = entrydArray2;
                            this.m_Owner.m_Count4 = num2;
                        }
                    }
                }
            }

            public bool Wait()
            {
                return ((this.m_Thread == null) || this.m_Thread.Join(10));
            }

            public bool IsAlive
            {
                get
                {
                    return ((this.m_Thread != null) && this.m_Thread.IsAlive);
                }
            }
        }
    }
}

