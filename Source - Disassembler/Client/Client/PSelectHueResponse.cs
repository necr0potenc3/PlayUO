namespace Client
{
    using System;

    public class PSelectHueResponse : Packet
    {
        public PSelectHueResponse(int Serial, short Relay, short Hue) : base(0x95, "Select Hue Response", 9)
        {
            base.m_Stream.Write(Serial);
            base.m_Stream.Write(Relay);
            base.m_Stream.Write(Hue);
        }
    }
}

