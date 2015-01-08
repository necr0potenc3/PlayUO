namespace Client
{
    using System;

    public class UpdateZ : UpdateInfo
    {
        public int Serial;
        public int X;
        public int Y;

        public UpdateZ(int Serial, int X, int Y)
        {
            this.Serial = Serial;
            this.X = X;
            this.Y = Y;
        }
    }
}

