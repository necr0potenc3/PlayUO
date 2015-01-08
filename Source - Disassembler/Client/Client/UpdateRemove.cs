namespace Client
{
    using System;

    public class UpdateRemove : UpdateInfo
    {
        public int OldX;
        public int OldY;
        public int Serial;

        public UpdateRemove(int Serial, int OldX, int OldY)
        {
            this.Serial = Serial;
            this.OldX = OldX;
            this.OldY = OldY;
        }
    }
}

