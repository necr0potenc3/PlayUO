namespace Client
{
    using System;

    public class PPopupRequest : Packet
    {
        public PPopupRequest(Item Target) : this(Target.Serial)
        {
        }

        public PPopupRequest(Mobile Target) : this(Target.Serial)
        {
        }

        public PPopupRequest(MobileCell Target) : this(Target.m_Mobile.Serial)
        {
        }

        protected PPopupRequest(int Serial) : base(0xbf, "Popup Request")
        {
            base.m_Stream.Write((short) 0x13);
            base.m_Stream.Write(Serial);
        }
    }
}

