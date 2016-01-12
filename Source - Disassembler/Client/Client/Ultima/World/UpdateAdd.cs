namespace Client
{
    public class UpdateAdd : UpdateInfo
    {
        public int NewX;
        public int NewY;
        public int Serial;

        public UpdateAdd(int Serial, int NewX, int NewY)
        {
            this.Serial = Serial;
            this.NewX = NewX;
            this.NewY = NewY;
        }
    }
}