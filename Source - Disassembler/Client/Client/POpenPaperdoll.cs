namespace Client
{
    using System;

    public class POpenPaperdoll : Packet
    {
        public POpenPaperdoll() : this(World.Serial)
        {
        }

        public POpenPaperdoll(int serial) : base(6, "Open Paperdoll", 5)
        {
            base.m_Stream.Write((int) (serial | -2147483648));
        }
    }
}

