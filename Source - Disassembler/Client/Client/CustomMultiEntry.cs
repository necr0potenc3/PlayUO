namespace Client
{
    using System;
    using System.Collections;
    using System.Runtime.InteropServices;

    public class CustomMultiEntry
    {
        private static byte[] m_InflateBuffer;
        private Client.Multi m_Multi;
        private int m_Revision;
        private int m_Serial;

        public CustomMultiEntry(int ser, int rev, Client.Multi baseMulti, int compressionType, byte[] buffer)
        {
            int num;
            int num2;
            int num3;
            int num4;
            this.m_Serial = ser;
            this.m_Revision = rev;
            baseMulti.GetBounds(out num, out num2, out num3, out num4);
            ArrayList list = new ArrayList();
            try
            {
                switch (compressionType)
                {
                    case 0:
                        LoadUncompressed(buffer, list);
                        goto Label_0060;

                    case 3:
                        break;

                    default:
                        goto Label_0060;
                }
                LoadDeflated(num, num2, num3, num4, buffer, list);
            }
            catch (Exception exception)
            {
                Debug.Error(exception);
            }
        Label_0060:
            this.m_Multi = new Client.Multi(list);
        }

        public static unsafe void LoadDeflated(int xMin, int yMin, int xMax, int yMax, byte[] buffer, ArrayList list)
        {
            int num = (yMax - yMin) + 1;
            fixed (byte* numRef = buffer)
            {
                byte* source = numRef;
                int num2 = *(source++);
                for (int i = 0; i < num2; i++)
                {
                    int num4 = (source[0] >> 4) & 15;
                    int num5 = source[0] & 15;
                    int destLength = source[1] | ((source[3] << 4) & 0xf00);
                    int sourceLength = source[2] | ((source[3] << 8) & 0xf00);
                    source += 4;
                    if ((m_InflateBuffer == null) || (m_InflateBuffer.Length < destLength))
                    {
                        m_InflateBuffer = new byte[destLength];
                    }
                    fixed (byte* numRef2 = m_InflateBuffer)
                    {
                        MultiItem item;
                        int num8;
                        int num9;
                        int num10;
                        int num11;
                        int num12;
                        byte* dest = numRef2;
                        uncompress(dest, ref destLength, source, sourceLength);
                        source += sourceLength;
                        byte* numPtr3 = dest + destLength;
                        switch (num4)
                        {
                            case 0:
                                goto Label_014A;

                            case 1:
                                num8 = 0;
                                switch (num5)
                                {
                                    case 0:
                                        goto Label_018C;

                                    case 1:
                                    case 5:
                                        goto Label_0191;

                                    case 2:
                                    case 6:
                                        goto Label_0196;

                                    case 3:
                                    case 7:
                                        goto Label_019C;

                                    case 4:
                                    case 8:
                                        goto Label_01A2;
                                }
                                goto Label_0226;

                            case 2:
                                num9 = 0;
                                switch (num5)
                                {
                                    case 0:
                                        goto Label_0268;

                                    case 1:
                                    case 5:
                                        goto Label_026D;

                                    case 2:
                                    case 6:
                                        goto Label_0272;

                                    case 3:
                                    case 7:
                                        goto Label_0278;

                                    case 4:
                                    case 8:
                                        goto Label_027E;
                                }
                                goto Label_0284;

                            default:
                                {
                                    continue;
                                }
                        }
                    Label_00CA:
                        item = new MultiItem();
                        item.Flags = 1;
                        item.ItemID = (short)(0x4000 | (((dest[0] << 8) | dest[1]) & 0x3fff));
                        item.X = (sbyte)dest[2];
                        item.Y = (sbyte)dest[3];
                        item.Z = (sbyte)dest[4];
                        dest += 5;
                        if (item.ItemID != 0x4000)
                        {
                            list.Add(item);
                        }
                    Label_014A:
                        if (dest < numPtr3)
                        {
                            goto Label_00CA;
                        }
                        continue;
                    Label_018C:
                        num8 = 0;
                        goto Label_0226;
                    Label_0191:
                        num8 = 7;
                        goto Label_0226;
                    Label_0196:
                        num8 = 0x1b;
                        goto Label_0226;
                    Label_019C:
                        num8 = 0x2f;
                        goto Label_0226;
                    Label_01A2:
                        num8 = 0x43;
                    Label_0226:
                        while (dest < numPtr3)
                        {
                            MultiItem item2 = new MultiItem();
                            item2.Flags = 1;
                            item2.ItemID = (short)(0x4000 | (((dest[0] << 8) | dest[1]) & 0x3fff));
                            item2.X = (sbyte)dest[2];
                            item2.Y = (sbyte)dest[3];
                            item2.Z = (sbyte)num8;
                            dest += 4;
                            if (item2.ItemID != 0x4000)
                            {
                                list.Add(item2);
                            }
                        }
                        continue;
                    Label_0268:
                        num9 = 0;
                        goto Label_0284;
                    Label_026D:
                        num9 = 7;
                        goto Label_0284;
                    Label_0272:
                        num9 = 0x1b;
                        goto Label_0284;
                    Label_0278:
                        num9 = 0x2f;
                        goto Label_0284;
                    Label_027E:
                        num9 = 0x43;
                    Label_0284:
                        if (num5 <= 0)
                        {
                            num10 = xMin;
                            num11 = yMin;
                            num12 = num + 1;
                        }
                        else if (num5 <= 4)
                        {
                            num10 = xMin + 1;
                            num11 = yMin + 1;
                            num12 = num - 1;
                        }
                        else
                        {
                            num10 = xMin;
                            num11 = yMin;
                            num12 = num;
                        }
                        int num13 = 0;
                        while (dest < numPtr3)
                        {
                            short num14 = (short)(0x4000 | (((dest[0] << 8) | dest[1]) & 0x3fff));
                            num13++;
                            dest += 2;
                            if (num14 != 0x4000)
                            {
                                MultiItem item3 = new MultiItem();
                                item3.Flags = 1;
                                item3.ItemID = num14;
                                item3.X = (short)(num10 + ((num13 - 1) / num12));
                                item3.Y = (sbyte)(num11 + ((num13 - 1) % num12));
                                item3.Z = (sbyte)num9;
                                list.Add(item3);
                            }
                        }
                    }
                }
            }
        }

        public static void LoadUncompressed(byte[] buffer, ArrayList list)
        {
            int num = buffer.Length / 5;
            int num2 = 0;
            for (int i = 0; i < num; i++)
            {
                int num4 = (buffer[num2++] << 8) | buffer[num2++];
                int num5 = (sbyte)buffer[num2++];
                int num6 = (sbyte)buffer[num2++];
                int num7 = (sbyte)buffer[num2++];
                MultiItem item = new MultiItem();
                item.Flags = 1;
                item.ItemID = (short)num4;
                item.X = (short)num5;
                item.Y = (short)num6;
                item.Z = (short)num7;
                list.Add(item);
            }
        }

        [DllImport("zlib.dll")]
        public static extern unsafe int uncompress(byte* dest, ref int destLength, byte* source, int sourceLength);

        public Client.Multi Multi
        {
            get
            {
                return this.m_Multi;
            }
        }

        public int Revision
        {
            get
            {
                return this.m_Revision;
            }
        }

        public int Serial
        {
            get
            {
                return this.m_Serial;
            }
        }
    }
}