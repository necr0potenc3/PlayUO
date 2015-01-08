namespace Client
{
    using System;
    using System.IO;

    public class TileMatrixPatch
    {
        private int[][] m_LandBlockRefs;
        private FileStream m_LandData;
        private int[][] m_StaticBlockRefs;
        private FileStream m_StaticData;
        private FileStream m_StaticLookup;
        private BinaryReader m_StaticLookupReader;

        public TileMatrixPatch(TileMatrix matrix, int index)
        {
            string path = Path.Combine(Engine.FileManager.FilePath, string.Format("mapdif{0}.mul", index));
            string str2 = Path.Combine(Engine.FileManager.FilePath, string.Format("mapdifl{0}.mul", index));
            if (File.Exists(path) && File.Exists(str2))
            {
                this.m_LandData = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                using (FileStream stream = new FileStream(str2, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    this.m_LandBlockRefs = this.SlurpRefs(matrix.BlockWidth, matrix.BlockHeight, stream);
                }
            }
            string str3 = Path.Combine(Engine.FileManager.FilePath, string.Format("stadif{0}.mul", index));
            string str4 = Path.Combine(Engine.FileManager.FilePath, string.Format("stadifl{0}.mul", index));
            string str5 = Path.Combine(Engine.FileManager.FilePath, string.Format("stadifi{0}.mul", index));
            if ((File.Exists(str3) && File.Exists(str4)) && File.Exists(str5))
            {
                this.m_StaticData = new FileStream(str3, FileMode.Open, FileAccess.Read, FileShare.Read);
                this.m_StaticLookup = new FileStream(str5, FileMode.Open, FileAccess.Read, FileShare.Read);
                this.m_StaticLookupReader = new BinaryReader(this.m_StaticLookup);
                using (FileStream stream2 = new FileStream(str4, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    this.m_StaticBlockRefs = this.SlurpRefs(matrix.BlockWidth, matrix.BlockHeight, stream2);
                }
            }
        }

        public int[][] SlurpRefs(int width, int height, FileStream fs)
        {
            BinaryReader reader = new BinaryReader(fs);
            int num = (int) (fs.Length / 4L);
            int[][] numArray = new int[width][];
            for (int i = 0; i < num; i++)
            {
                int num3 = reader.ReadInt32();
                int index = num3 / height;
                int num5 = num3 % height;
                if (((index >= 0) && (index < width)) && ((num5 >= 0) && (num5 < height)))
                {
                    int[] numArray2 = numArray[index];
                    if (numArray2 == null)
                    {
                        numArray[index] = numArray2 = new int[height];
                    }
                    numArray2[num5] = ~i;
                }
            }
            return numArray;
        }

        public int[][] LandBlockRefs
        {
            get
            {
                return this.m_LandBlockRefs;
            }
        }

        public FileStream LandData
        {
            get
            {
                return this.m_LandData;
            }
        }

        public int[][] StaticBlockRefs
        {
            get
            {
                return this.m_StaticBlockRefs;
            }
        }

        public FileStream StaticData
        {
            get
            {
                return this.m_StaticData;
            }
        }

        public FileStream StaticLookup
        {
            get
            {
                return this.m_StaticLookup;
            }
        }

        public BinaryReader StaticLookupReader
        {
            get
            {
                return this.m_StaticLookupReader;
            }
        }
    }
}

