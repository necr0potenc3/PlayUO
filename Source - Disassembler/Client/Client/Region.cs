namespace Client
{
    using System.Collections;
    using System.IO;

    public class Region
    {
        private int m_EndZ;
        private static Region[] m_GuardedRegions;
        private int m_Height;
        private int m_StartZ;
        private int m_Width;
        private RegionWorld m_World;
        private int m_X;
        private int m_Y;

        public Region(string line)
        {
            string[] strArray = line.Split(new char[] { ' ' });
            this.m_X = int.Parse(strArray[0]);
            this.m_Y = int.Parse(strArray[1]);
            this.m_Width = int.Parse(strArray[2]);
            this.m_Height = int.Parse(strArray[3]);
            this.m_StartZ = int.Parse(strArray[4]);
            this.m_EndZ = int.Parse(strArray[5]);
            if (strArray.Length >= 7)
            {
                switch (strArray[6])
                {
                    case "B":
                        this.m_World = RegionWorld.Britannia;
                        break;

                    case "F":
                        this.m_World = RegionWorld.Felucca;
                        break;

                    case "T":
                        this.m_World = RegionWorld.Trammel;
                        break;

                    case "I":
                        this.m_World = RegionWorld.Ilshenar;
                        break;

                    case "M":
                        this.m_World = RegionWorld.Malas;
                        break;

                    case "K":
                        this.m_World = RegionWorld.Tokuno;
                        break;
                }
            }
        }

        public Region(int x, int y, int width, int height, int startZ, int endZ, RegionWorld world)
        {
            this.m_X = x;
            this.m_Y = y;
            this.m_Width = width;
            this.m_Height = height;
            this.m_StartZ = startZ;
            this.m_EndZ = endZ;
            this.m_World = world;
        }

        public static Region Find(Region[] regs, int x, int y, int z, int w)
        {
            for (int i = 0; i < regs.Length; i++)
            {
                Region region = regs[i];
                RegionWorld world = region.m_World;
                bool flag = false;
                switch (world)
                {
                    case RegionWorld.Britannia:
                        flag = (w == 0) || (w == 1);
                        break;

                    case RegionWorld.Felucca:
                        flag = w == 0;
                        break;

                    case RegionWorld.Trammel:
                        flag = w == 1;
                        break;

                    case RegionWorld.Ilshenar:
                        flag = w == 2;
                        break;

                    case RegionWorld.Malas:
                        flag = w == 3;
                        break;

                    case RegionWorld.Tokuno:
                        flag = w == 4;
                        break;
                }
                if (((flag && (((x - region.m_X)) < region.m_Width)) && ((((y - region.m_Y)) < region.m_Height) && (z >= region.m_StartZ))) && (z <= region.m_EndZ))
                {
                    return region;
                }
            }
            return null;
        }

        public static Region[] Load(string path)
        {
            if (!File.Exists(path))
            {
                return new Region[0];
            }
            ArrayList list = new ArrayList();
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string str;
                    while ((str = reader.ReadLine()) != null)
                    {
                        if ((str.Length != 0) && !str.StartsWith("#"))
                        {
                            list.Add(new Region(str));
                        }
                    }
                }
            }
            catch
            {
            }
            return (Region[])list.ToArray(typeof(Region));
        }

        public static Region[] GuardedRegions
        {
            get
            {
                if (m_GuardedRegions == null)
                {
                    m_GuardedRegions = Load("guardlines.def");
                }
                return m_GuardedRegions;
            }
        }

        public int Height
        {
            get
            {
                return this.m_Height;
            }
        }

        public int Width
        {
            get
            {
                return this.m_Width;
            }
        }

        public RegionWorld World
        {
            get
            {
                return this.m_World;
            }
        }

        public int X
        {
            get
            {
                return this.m_X;
            }
        }

        public int Y
        {
            get
            {
                return this.m_Y;
            }
        }
    }
}