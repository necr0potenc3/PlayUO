namespace Client
{
    public class ExposerInterop : IInterop
    {
        public void AddTimer(Timer t)
        {
            Engine.AddTimer(t);
        }

        public object GetMobile(int Serial, string Name)
        {
            Mobile mobile = World.FindMobile(Serial);
            if (mobile != null)
            {
                switch (Name)
                {
                    case "OpenedStatus":
                        return mobile.OpenedStatus;

                    case "BigStatus":
                        return mobile.BigStatus;

                    case "Weight":
                        return mobile.Weight;

                    case "Gold":
                        return mobile.Gold;

                    case "Gender":
                        return mobile.Gender;

                    case "CorpseSerial":
                        return mobile.CorpseSerial;

                    case "Notoriety":
                        return mobile.Notoriety;

                    case "Visible":
                        return mobile.Visible;

                    case "OldMapX":
                        return mobile.OldMapX;

                    case "OldMapY":
                        return mobile.OldMapY;

                    case "Armor":
                        return mobile.Armor;

                    case "Str":
                        return mobile.Str;

                    case "HPCur":
                        return mobile.HPCur;

                    case "HPMax":
                        return mobile.HPMax;

                    case "Dex":
                        return mobile.Dex;

                    case "StamCur":
                        return mobile.StamCur;

                    case "StamMax":
                        return mobile.StamMax;

                    case "Int":
                        return mobile.Int;

                    case "ManaCur":
                        return mobile.ManaCur;

                    case "ManaMax":
                        return mobile.ManaMax;

                    case "PaperdollName":
                        return mobile.PaperdollName;

                    case "Flags":
                        return mobile.Flags.Value;

                    case "IsMoving":
                        return mobile.IsMoving;

                    case "MovedTiles":
                        return mobile.MovedTiles;

                    case "LastWalk":
                        return mobile.LastWalk;

                    case "Direction":
                        return mobile.Direction;

                    case "Body":
                        return mobile.Body;

                    case "Hue":
                        return mobile.Hue;

                    case "X":
                        return mobile.X;

                    case "Y":
                        return mobile.Y;

                    case "Z":
                        return mobile.Z;

                    case "Name":
                        return mobile.Name;

                    case "Refresh":
                        return mobile.Refresh;
                }
                return null;
            }
            return null;
        }

        public int GetTicks()
        {
            return Engine.Ticks;
        }

        public void OpenStatus(int Serial, bool Drag)
        {
            Mobile mobile = World.FindMobile(Serial);
            if (mobile != null)
            {
                mobile.OpenStatus(Drag);
            }
        }

        public void RemoveTimer(Timer t)
        {
            Engine.RemoveTimer(t);
        }

        public void SetMobile(int Serial, string Name, object Value)
        {
            Mobile mobile = World.FindMobile(Serial);
            if (mobile != null)
            {
                switch (Name)
                {
                    case "OpenedStatus":
                        mobile.OpenedStatus = (bool)Value;
                        break;

                    case "BigStatus":
                        mobile.BigStatus = (bool)Value;
                        break;

                    case "Weight":
                        mobile.Weight = (int)Value;
                        break;

                    case "Gold":
                        mobile.Gold = (int)Value;
                        break;

                    case "Gender":
                        mobile.Gender = (byte)((int)Value);
                        break;

                    case "CorpseSerial":
                        mobile.CorpseSerial = (int)Value;
                        break;

                    case "Notoriety":
                        mobile.Notoriety = (Notoriety)((byte)((int)Value));
                        break;

                    case "Visible":
                        mobile.Visible = (bool)Value;
                        break;

                    case "OldMapX":
                        mobile.OldMapX = (int)Value;
                        break;

                    case "OldMapY":
                        mobile.OldMapY = (int)Value;
                        break;

                    case "Armor":
                        mobile.Armor = (int)Value;
                        break;

                    case "Str":
                        mobile.Str = (int)Value;
                        break;

                    case "HPCur":
                        mobile.HPCur = (int)Value;
                        break;

                    case "HPMax":
                        mobile.HPMax = (int)Value;
                        break;

                    case "Dex":
                        mobile.Dex = (int)Value;
                        break;

                    case "StamCur":
                        mobile.StamCur = (int)Value;
                        break;

                    case "StamMax":
                        mobile.StamMax = (int)Value;
                        break;

                    case "Int":
                        mobile.Int = (int)Value;
                        break;

                    case "ManaCur":
                        mobile.ManaCur = (int)Value;
                        break;

                    case "ManaMax":
                        mobile.ManaMax = (int)Value;
                        break;

                    case "PaperdollName":
                        mobile.PaperdollName = (string)Value;
                        break;

                    case "Flags":
                        mobile.Flags.Value = (int)Value;
                        break;

                    case "IsMoving":
                        mobile.IsMoving = (bool)Value;
                        break;

                    case "MovedTiles":
                        mobile.MovedTiles = (int)Value;
                        break;

                    case "LastWalk":
                        mobile.LastWalk = (int)Value;
                        break;

                    case "Direction":
                        mobile.Direction = (byte)((int)Value);
                        break;

                    case "Body":
                        mobile.Body = (short)((int)Value);
                        break;

                    case "Hue":
                        mobile.Hue = (short)((int)Value);
                        break;

                    case "X":
                        mobile.X = (short)((int)Value);
                        break;

                    case "Y":
                        mobile.Y = (short)((int)Value);
                        break;

                    case "Z":
                        mobile.Z = (short)((int)Value);
                        break;

                    case "Name":
                        mobile.Name = (string)Value;
                        break;

                    case "Refresh":
                        mobile.Refresh = (bool)Value;
                        break;
                }
            }
        }
    }
}