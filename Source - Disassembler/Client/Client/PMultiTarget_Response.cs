namespace Client
{
    using System;

    public class PMultiTarget_Response : Packet
    {
        public PMultiTarget_Response(int targetID, int x, int y, int z, int id) : base(0x6c, "Multi Target Response", 0x13)
        {
            base.m_Stream.Write((byte) 1);
            base.m_Stream.Write(targetID);
            base.m_Stream.Write((byte) 0);
            base.m_Stream.Write(0);
            base.m_Stream.Write((short) x);
            base.m_Stream.Write((short) y);
            base.m_Stream.Write((short) z);
            base.m_Stream.Write((short) id);
            PacketHandlers.m_LastMultiX = x - Engine.m_xMultiOffset;
            PacketHandlers.m_LastMultiY = y - Engine.m_yMultiOffset;
        }
    }
}

