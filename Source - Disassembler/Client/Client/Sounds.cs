namespace Client
{
    using Microsoft.DirectX.DirectSound;
    using System;
    using System.IO;

    public class Sounds
    {
        private byte[] m_Buffer;
        private static SoundCache[] m_Cache = new SoundCache[0x1000];
        public static Microsoft.DirectX.DirectSound.Device m_Device;
        private bool m_Enabled = true;
        public static WaveFormat m_Format;
        private static BinaryReader m_Index = new BinaryReader(Engine.FileManager.OpenMUL(Files.SoundIdx));
        private int[] m_SCC_SoundTable = new int[] { 0x58, 0x2e, 0x2c };
        private int[] m_SCC_TransTable = new int[] { 
            0, 0, 1, 0, 3, 0, 2, 2, 2, 3, 3, 3, 1, 2, 2, 2, 
            2, 1, 2, 2, 3, 1
         };
        private int[] m_SOC_SoundTable = new int[] { 0x48, 0x2f, 0x4f, 0x2d };
        private int[] m_SOC_TransTable = new int[] { 
            0, 0, 1, 2, 4, 2, 3, 3, 3, 4, 4, 4, 1, 3, 3, 3, 
            3, 1, 3, 3, 4, 1
         };
        private static Stream m_Stream = Engine.FileManager.OpenMUL(Files.SoundMul);

        public Sounds()
        {
            try
            {
                m_Device = new Microsoft.DirectX.DirectSound.Device();
                m_Device.SetCooperativeLevel(Engine.m_Display, 2);
                m_Format = new WaveFormat();
                m_Format.set_Channels(1);
                m_Format.set_FormatTag(1);
                m_Format.set_BlockAlign(2);
                m_Format.set_BitsPerSample(0x10);
                m_Format.set_SamplesPerSecond(0x5622);
                m_Format.set_AverageBytesPerSecond(0xac44);
            }
            catch (Exception exception)
            {
                Debug.Trace("Error constructing sound factory");
                Debug.Error(exception);
                m_Device = null;
            }
            this.m_Buffer = new byte[0x2000];
        }

        public void Dispose()
        {
            if (m_Cache != null)
            {
                for (int i = 0; i < 0x1000; i++)
                {
                    if (m_Cache[i] != null)
                    {
                        m_Cache[i].Dispose();
                        m_Cache[i] = null;
                    }
                }
                m_Cache = null;
            }
            m_Stream.Close();
            m_Stream = null;
            m_Index.Close();
            m_Index = null;
            m_Device = null;
            this.m_Buffer = null;
        }

        public void PlayContainerClose(int GumpID)
        {
            if (GumpID == 0x2a63)
            {
                Engine.Sounds.PlaySound(0x1c9);
            }
            else
            {
                GumpID -= 60;
                if ((GumpID >= 0) && (GumpID <= 0x15))
                {
                    int index = this.m_SCC_TransTable[GumpID];
                    if (index < this.m_SCC_SoundTable.Length)
                    {
                        this.PlaySound(this.m_SCC_SoundTable[index]);
                    }
                }
            }
        }

        public void PlayContainerOpen(int GumpID)
        {
            if (GumpID == 0x2a63)
            {
                Engine.Sounds.PlaySound(0x187);
            }
            else
            {
                GumpID -= 60;
                if ((GumpID >= 0) && (GumpID <= 0x15))
                {
                    int index = this.m_SOC_TransTable[GumpID];
                    if (index < this.m_SOC_SoundTable.Length)
                    {
                        this.PlaySound(this.m_SOC_SoundTable[index]);
                    }
                }
            }
        }

        public void PlaySound(int SoundID)
        {
            this.PlaySound(SoundID, -1, -1, -1);
        }

        public void PlaySound(int SoundID, int X, int Y, int Z)
        {
            this.PlaySound(SoundID, X, Y, Z, 1f);
        }

        public void PlaySound(int SoundID, int X, int Y, int Z, float Volume)
        {
            if ((m_Device != null) && this.m_Enabled)
            {
                SoundID &= 0xfff;
                SoundCache cache = m_Cache[SoundID];
                if (cache == null)
                {
                    cache = m_Cache[SoundID] = new SoundCache(SoundID);
                }
                try
                {
                    SecondaryBuffer buffer = cache.GetBuffer(this);
                    if (buffer != null)
                    {
                        Mobile player = World.Player;
                        if ((((X == -1) && (Y == -1)) && (Z == -1)) || (player == null))
                        {
                            try
                            {
                                buffer.set_Pan(0);
                            }
                            catch
                            {
                            }
                            try
                            {
                                buffer.set_Volume(-10000 + (this.ScaledVolume * 100));
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            int num = Math.Abs((int) ((X - player.X) * 11));
                            int num2 = Math.Abs((int) ((Y - player.Y) * 11));
                            int num3 = Math.Abs((int) (Z - player.Z));
                            int num4 = (int) Math.Sqrt((double) (((num * num) + (num2 * num2)) + (num3 * num3)));
                            int num5 = (X - Y) - (player.X - player.Y);
                            num5 *= 350;
                            num4 *= 10;
                            num4 = -num4;
                            num4 -= (int) (5000f * (1f - Volume));
                            int scaledVolume = this.ScaledVolume;
                            num4 = ((-10000 * (100 - scaledVolume)) + (num4 * scaledVolume)) / 100;
                            if (num4 > 0)
                            {
                                num4 = 0;
                            }
                            else if (num4 < -10000)
                            {
                                num4 = -10000;
                            }
                            if (num5 > 0x2710)
                            {
                                num5 = 0x2710;
                            }
                            else if (num5 < -10000)
                            {
                                num5 = -10000;
                            }
                            try
                            {
                                buffer.set_Pan(num5);
                            }
                            catch
                            {
                            }
                            try
                            {
                                buffer.set_Volume(num4);
                            }
                            catch
                            {
                            }
                        }
                        buffer.SetCurrentPosition(0);
                        buffer.Play(0, 0);
                    }
                }
                catch (Exception exception)
                {
                    Debug.Error(exception);
                }
            }
        }

        public SecondaryBuffer ReadFromDisk(int SoundID)
        {
            if (m_Device == null)
            {
                return null;
            }
            if (SoundID < 0)
            {
                return null;
            }
            m_Index.BaseStream.Seek((long) (SoundID * 12), SeekOrigin.Begin);
            int num = m_Index.ReadInt32();
            int num2 = m_Index.ReadInt32();
            int num3 = m_Index.ReadInt32();
            if ((num < 0) || (num2 <= 0))
            {
                if (!this.Translate(ref SoundID))
                {
                    return null;
                }
                m_Index.BaseStream.Seek((long) (SoundID * 12), SeekOrigin.Begin);
                num = m_Index.ReadInt32();
                num2 = m_Index.ReadInt32();
                num3 = m_Index.ReadInt32();
            }
            if ((num < 0) || (num2 <= 0))
            {
                return null;
            }
            num2 -= 40;
            m_Stream.Seek((long) (num + 40), SeekOrigin.Begin);
            BufferDescription description = new BufferDescription(m_Format);
            description.set_BufferBytes(num2);
            description.set_ControlPan(true);
            description.set_ControlVolume(true);
            SecondaryBuffer buffer = new SecondaryBuffer(description, m_Device);
            buffer.Write(0, m_Stream, num2, 2);
            return buffer;
        }

        private bool Translate(ref int index)
        {
            object obj2 = SoundTable.m_Map[(int) index];
            if (obj2 != null)
            {
                index = (int) obj2;
            }
            return (obj2 != null);
        }

        public Microsoft.DirectX.DirectSound.Device Device
        {
            get
            {
                return m_Device;
            }
        }

        public bool Enabled
        {
            get
            {
                return this.m_Enabled;
            }
            set
            {
                this.m_Enabled = value;
            }
        }

        public int ScaledVolume
        {
            get
            {
                int sound = VolumeControl.Sound;
                return (100 - (((((100 - sound) * (100 - sound)) * (100 - sound)) * (100 - sound)) / 0xf4240));
            }
        }
    }
}

