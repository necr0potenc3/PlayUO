namespace Client
{
    using System;

    public class FontImage
    {
        public int xDelta;
        public int xWidth;
        public byte[] xyPixels;
        public int yHeight;

        public FontImage(int xWidth, int yHeight)
        {
            this.xWidth = xWidth;
            this.yHeight = yHeight;
            this.xDelta = xWidth + (-xWidth & 3);
            this.xyPixels = new byte[this.xDelta * yHeight];
        }
    }
}

