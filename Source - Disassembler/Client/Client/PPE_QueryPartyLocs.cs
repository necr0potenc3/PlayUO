namespace Client
{
    using System;

    public class PPE_QueryPartyLocs : Packet
    {
        public PPE_QueryPartyLocs() : base(240, "Query Party Locations")
        {
            base.m_Stream.Write((byte) 0);
        }
    }
}

