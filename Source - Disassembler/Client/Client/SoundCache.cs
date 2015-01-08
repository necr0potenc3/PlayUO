namespace Client
{
    using Microsoft.DirectX.DirectSound;
    using System;
    using System.Collections;

    public class SoundCache
    {
        private ArrayList m_Buffers = new ArrayList();
        private int m_Count;
        private bool m_Empty;
        private int m_SoundID;

        public SoundCache(int soundID)
        {
            this.m_SoundID = soundID;
            this.m_Count = 0;
            this.m_Empty = false;
        }

        public void Dispose()
        {
            while (this.m_Count > 0)
            {
                this.m_Buffers[0] = null;
                this.m_Buffers.RemoveAt(0);
                this.m_Count--;
            }
            this.m_Buffers = null;
        }

        public SecondaryBuffer GetBuffer(Sounds parent)
        {
            if (this.m_Empty)
            {
                return null;
            }
            SecondaryBuffer buffer = null;
            for (int i = 0; i < this.m_Count; i++)
            {
                SecondaryBuffer buffer2 = (SecondaryBuffer) this.m_Buffers[i];
                BufferStatus status = buffer2.get_Status();
                if (!status.get_Playing() && !status.get_BufferLost())
                {
                    return buffer2;
                }
                if (status.get_BufferLost())
                {
                    this.m_Buffers.RemoveAt(i);
                    i--;
                    this.m_Count--;
                }
                else
                {
                    buffer = buffer2;
                }
            }
            if (buffer != null)
            {
                try
                {
                    SecondaryBuffer buffer3 = buffer.Clone(parent.Device);
                    if (buffer3 != null)
                    {
                        this.m_Buffers.Add(buffer3);
                        this.m_Count++;
                        return buffer3;
                    }
                }
                catch (Exception exception)
                {
                    Debug.Error(exception);
                }
            }
            SecondaryBuffer buffer4 = parent.ReadFromDisk(this.m_SoundID);
            if (buffer4 != null)
            {
                this.m_Buffers.Add(buffer4);
                this.m_Count++;
            }
            else
            {
                this.m_Empty = true;
            }
            return buffer4;
        }
    }
}

