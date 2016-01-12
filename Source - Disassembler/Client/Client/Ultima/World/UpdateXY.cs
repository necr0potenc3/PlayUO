namespace Client
{
    public class UpdateXY : UpdateInfo
    {
        public int NewX;
        public int NewY;
        public int OldX;
        public int OldY;
        public int Serial;

        public UpdateXY(int Serial, int NewX, int NewY, int OldX, int OldY)
        {
            this.Serial = Serial;
            this.NewX = NewX;
            this.NewY = NewY;
            this.OldX = OldX;
            this.OldY = OldY;
        }
    }
}