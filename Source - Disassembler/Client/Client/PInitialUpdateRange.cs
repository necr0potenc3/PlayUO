namespace Client
{
    using System;

    public class PInitialUpdateRange : Packet
    {
        public PInitialUpdateRange() : this(0x12)
        {
        }

        public PInitialUpdateRange(int range) : base(200, "Initial Update Range", 2)
        {
            World.Range = range;
            base.m_Stream.Write((byte) range);
        }
    }
}

