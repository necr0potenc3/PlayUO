namespace Client
{
    using System;

    public class xMobile
    {
        private int m_Serial;

        public xMobile(int Serial)
        {
            this.m_Serial = Serial;
        }

        public void OpenStatus(bool Drag)
        {
            Interop.OpenStatus(this.m_Serial, Drag);
        }

        public int Armor
        {
            get
            {
                return (int) Interop.GetMobile(this.m_Serial, "Armor");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "Armor", value);
            }
        }

        public bool BigStatus
        {
            get
            {
                return (bool) Interop.GetMobile(this.m_Serial, "BigStatus");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "BigStatus", value);
            }
        }

        public int Body
        {
            get
            {
                return (short) Interop.GetMobile(this.m_Serial, "Body");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "Body", value);
            }
        }

        public int CorpseSerial
        {
            get
            {
                return (int) Interop.GetMobile(this.m_Serial, "CorpseSerial");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "CorpseSerial", value);
            }
        }

        public int Dex
        {
            get
            {
                return (int) Interop.GetMobile(this.m_Serial, "Dex");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "Dex", value);
            }
        }

        public int Direction
        {
            get
            {
                return (byte) Interop.GetMobile(this.m_Serial, "Direction");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "Direction", value);
            }
        }

        public xMobileFlags Flags
        {
            get
            {
                return new xMobileFlags(this.m_Serial, (int) Interop.GetMobile(this.m_Serial, "Flags"));
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "Flags", value.Value);
            }
        }

        public int Gender
        {
            get
            {
                return (byte) Interop.GetMobile(this.m_Serial, "Gender");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "Gender", value);
            }
        }

        public int Gold
        {
            get
            {
                return (int) Interop.GetMobile(this.m_Serial, "Gold");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "Gold", value);
            }
        }

        public int HPCur
        {
            get
            {
                return (int) Interop.GetMobile(this.m_Serial, "HPCur");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "HPCur", value);
            }
        }

        public int HPMax
        {
            get
            {
                return (int) Interop.GetMobile(this.m_Serial, "HPMax");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "HPMax", value);
            }
        }

        public int Hue
        {
            get
            {
                return (short) Interop.GetMobile(this.m_Serial, "Hue");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "Hue", value);
            }
        }

        public int Int
        {
            get
            {
                return (int) Interop.GetMobile(this.m_Serial, "Int");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "Int", value);
            }
        }

        public bool IsMoving
        {
            get
            {
                return (bool) Interop.GetMobile(this.m_Serial, "IsMoving");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "IsMoving", value);
            }
        }

        public int LastWalk
        {
            get
            {
                return (int) Interop.GetMobile(this.m_Serial, "LastWalk");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "LastWalk", value);
            }
        }

        public int ManaCur
        {
            get
            {
                return (int) Interop.GetMobile(this.m_Serial, "ManaCur");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "ManaCur", value);
            }
        }

        public int ManaMax
        {
            get
            {
                return (int) Interop.GetMobile(this.m_Serial, "ManaMax");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "ManaMax", value);
            }
        }

        public int MovedTiles
        {
            get
            {
                return (int) Interop.GetMobile(this.m_Serial, "MovedTiles");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "MovedTiles", value);
            }
        }

        public string Name
        {
            get
            {
                return (string) Interop.GetMobile(this.m_Serial, "Name");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "Name", value);
            }
        }

        public int Notoriety
        {
            get
            {
                return (int) Interop.GetMobile(this.m_Serial, "Notoriety");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "Notoriety", value);
            }
        }

        public int OldMapX
        {
            get
            {
                return (int) Interop.GetMobile(this.m_Serial, "OldMapX");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "OldMapX", value);
            }
        }

        public int OldMapY
        {
            get
            {
                return (int) Interop.GetMobile(this.m_Serial, "OldMapY");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "OldMapY", value);
            }
        }

        public bool OpenedStatus
        {
            get
            {
                return (bool) Interop.GetMobile(this.m_Serial, "OpenedStatus");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "OpenedStatus", value);
            }
        }

        public string PaperdollName
        {
            get
            {
                return (string) Interop.GetMobile(this.m_Serial, "PaperdollName");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "PaperdollName", value);
            }
        }

        public bool Refresh
        {
            get
            {
                return (bool) Interop.GetMobile(this.m_Serial, "Refresh");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "Refresh", value);
            }
        }

        public int Serial
        {
            get
            {
                return this.m_Serial;
            }
            set
            {
                this.m_Serial = value;
            }
        }

        public int StamCur
        {
            get
            {
                return (int) Interop.GetMobile(this.m_Serial, "StamCur");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "StamCur", value);
            }
        }

        public int StamMax
        {
            get
            {
                return (int) Interop.GetMobile(this.m_Serial, "StamMax");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "StamMax", value);
            }
        }

        public int Str
        {
            get
            {
                return (int) Interop.GetMobile(this.m_Serial, "Str");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "Str", value);
            }
        }

        public bool Visible
        {
            get
            {
                return (bool) Interop.GetMobile(this.m_Serial, "Visible");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "Visible", value);
            }
        }

        public int Weight
        {
            get
            {
                return (int) Interop.GetMobile(this.m_Serial, "Weight");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "Weight", value);
            }
        }

        public int X
        {
            get
            {
                return (short) Interop.GetMobile(this.m_Serial, "X");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "X", value);
            }
        }

        public int Y
        {
            get
            {
                return (short) Interop.GetMobile(this.m_Serial, "Y");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "Y", value);
            }
        }

        public int Z
        {
            get
            {
                return (short) Interop.GetMobile(this.m_Serial, "Z");
            }
            set
            {
                Interop.SetMobile(this.m_Serial, "Z", value);
            }
        }
    }
}

