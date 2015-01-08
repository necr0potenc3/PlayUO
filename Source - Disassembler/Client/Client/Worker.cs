namespace Client
{
    using System;

    public class Worker
    {
        public TileMatrix Matrix;
        public int X;
        public int Y;

        public Worker(int X, int Y, TileMatrix Matrix)
        {
            this.X = X;
            this.Y = Y;
            this.Matrix = Matrix;
        }
    }
}

