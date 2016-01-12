namespace Client
{
    using System.IO;

    public class MapOverlay
    {
        private byte[] m_Buffer;
        private int m_Height;
        private int m_Width;

        public MapOverlay(string fileName)
        {
            BinaryReader reader = new BinaryReader(Engine.FileManager.OpenMUL(fileName));
            int num = reader.ReadInt32();
            int num2 = reader.ReadInt32();
            int num3 = 0;
            num3++;
        }
    }
}