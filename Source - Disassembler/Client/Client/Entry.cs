namespace Client
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Entry
    {
        public string AccountName;
        public string Password;
        public int ServerID;
        public string ServerName;
        public int CharID;
        public string CharName;
        public CharacterProfile CharProfile;
    }
}

