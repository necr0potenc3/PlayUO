namespace Client
{
    using System;

    public class PMoveRequest : Packet
    {
        public PMoveRequest(int dir, int seq, int key, int x, int y, int z) : base(2, "Movement Request", NewConfig.OldMovement ? 3 : 7)
        {
            base.m_Stream.Write((byte) dir);
            base.m_Stream.Write((byte) seq);
            if (!NewConfig.OldMovement)
            {
                base.m_Stream.Write(key);
            }
            PacketHandlers.AddSequence(seq, x, y, z);
        }
    }
}

