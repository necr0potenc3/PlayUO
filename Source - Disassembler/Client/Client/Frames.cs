namespace Client
{
    using System;

    public class Frames
    {
        public int FrameCount;
        public Frame[] FrameList;
        private static Frames m_Empty;

        public void Dispose()
        {
            for (int i = 0; i < this.FrameList.Length; i++)
            {
                if ((this.FrameList[i].Image.m_Surface != null) && !this.FrameList[i].Image.m_Surface.get_Disposed())
                {
                    this.FrameList[i].Image.m_Surface.Dispose();
                    Texture.m_Textures.Remove(this.FrameList[i].Image);
                }
            }
        }

        public bool Disposed
        {
            get
            {
                for (int i = 0; i < this.FrameList.Length; i++)
                {
                    if ((this.FrameList[i].Image.m_Surface != null) && this.FrameList[i].Image.m_Surface.get_Disposed())
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public static Frames Empty
        {
            get
            {
                if (m_Empty == null)
                {
                    m_Empty = new Frames();
                    m_Empty.FrameList = new Frame[0];
                }
                return m_Empty;
            }
        }

        public int LastAccessTime
        {
            get
            {
                int lastAccess = 0;
                for (int i = 0; i < this.FrameList.Length; i++)
                {
                    if (this.FrameList[i].Image.m_LastAccess > lastAccess)
                    {
                        lastAccess = this.FrameList[i].Image.m_LastAccess;
                    }
                }
                return lastAccess;
            }
        }
    }
}

