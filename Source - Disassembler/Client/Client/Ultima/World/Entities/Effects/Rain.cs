namespace Client
{
    using System;

    public class Rain : IParticle
    {
        private double m_Angle;
        private Texture m_Image;
        private static int m_Index;
        private bool m_Invalidated;
        private bool m_OnScreen;
        private Random m_Random;
        private int m_SliceCheck;
        private int m_Slices;
        private TimeSync m_Sync;
        private double m_xEnd;
        private double m_xStart;
        private double m_yEnd;
        private double m_yStart;

        public Rain(Random rnd)
        {
            switch ((m_Index % 8))
            {
                case 0:
                    this.m_xStart = rnd.Next(Engine.ScreenWidth) - Engine.ScreenWidth;
                    this.m_yStart = rnd.Next(Engine.ScreenHeight) - Engine.ScreenHeight;
                    break;

                case 1:
                    this.m_xStart = rnd.Next(Engine.ScreenWidth);
                    this.m_yStart = rnd.Next(Engine.ScreenHeight) - Engine.ScreenHeight;
                    break;

                case 2:
                    this.m_xStart = rnd.Next(Engine.ScreenWidth) - Engine.ScreenWidth;
                    this.m_yStart = rnd.Next(Engine.ScreenHeight);
                    break;

                case 3:
                    this.m_xStart = rnd.Next(Engine.ScreenWidth) + Engine.ScreenWidth;
                    this.m_yStart = rnd.Next(Engine.ScreenHeight) - Engine.ScreenHeight;
                    break;

                case 4:
                    this.m_xStart = rnd.Next(Engine.ScreenWidth) + Engine.ScreenWidth;
                    this.m_yStart = rnd.Next(Engine.ScreenHeight);
                    break;

                case 5:
                    this.m_xStart = rnd.Next(Engine.ScreenWidth) + Engine.ScreenWidth;
                    this.m_yStart = rnd.Next(Engine.ScreenHeight) + Engine.ScreenHeight;
                    break;

                case 6:
                    this.m_xStart = rnd.Next(Engine.ScreenWidth);
                    this.m_yStart = rnd.Next(Engine.ScreenHeight) + Engine.ScreenHeight;
                    break;

                case 7:
                    this.m_xStart = rnd.Next(Engine.ScreenWidth) - Engine.ScreenWidth;
                    this.m_yStart = rnd.Next(Engine.ScreenHeight) + Engine.ScreenHeight;
                    break;
            }
            m_Index++;
            double d = 3.1415926535897931 * (0.45 + (rnd.NextDouble() * 0.1));
            this.m_Angle = d;
            this.m_xEnd = this.m_xStart + ((Engine.ScreenWidth * 1.25) * Math.Cos(d));
            this.m_yEnd = this.m_yStart + ((Engine.ScreenHeight * 1.25) * Math.Sin(d));
            this.m_OnScreen = this.CheckOnScreen();
            this.m_Random = rnd;
            this.m_Image = Engine.m_Rain;
            float num2 = 0.5f + ((float)(rnd.NextDouble() * 0.5));
            this.m_Sync = new TimeSync((double)num2);
            this.m_SliceCheck = (int)(20f * num2);
        }

        private bool CheckOnScreen()
        {
            return ((((this.m_xEnd >= 0.0) && (this.m_yEnd >= 0.0)) && (this.m_xStart < Engine.ScreenWidth)) && (this.m_yStart < Engine.ScreenHeight));
        }

        public void Destroy()
        {
            if (!this.m_Invalidated)
            {
                Engine.Effects.Add(new Rain(this.m_Random));
            }
        }

        public void Invalidate()
        {
            this.m_Invalidated = true;
        }

        public bool Offset(int xDelta, int yDelta)
        {
            double normalized = this.m_Sync.Normalized;
            if (normalized >= 1.0)
            {
                return false;
            }
            int num2 = (int)(this.m_xStart + ((this.m_xEnd - this.m_xStart) * normalized));
            int num3 = (int)(this.m_yStart + ((this.m_yEnd - this.m_yStart) * normalized));
            this.m_xStart -= xDelta;
            this.m_yStart -= yDelta;
            this.m_xEnd -= xDelta;
            this.m_yEnd -= yDelta;
            this.m_OnScreen = this.CheckOnScreen();
            if (((num2 >= 0) && (num3 >= 0)) && ((num2 < Engine.ScreenWidth) && (num3 < Engine.ScreenHeight)))
            {
                num2 = (int)(this.m_xStart + ((this.m_xEnd - this.m_xStart) * normalized));
                num3 = (int)(this.m_yStart + ((this.m_yEnd - this.m_yStart) * normalized));
                if (((num2 < 0) || (num3 < 0)) || ((num2 >= Engine.ScreenWidth) || (num3 >= Engine.ScreenHeight)))
                {
                    if ((xDelta == 0x2c) && (yDelta == 0))
                    {
                        m_Index = 4;
                    }
                    else if ((xDelta == -44) && (yDelta == 0))
                    {
                        m_Index = 2;
                    }
                    else if ((xDelta == 0) && (yDelta == 0x2c))
                    {
                        m_Index = 6;
                    }
                    else if ((xDelta == 0) && (yDelta == -44))
                    {
                        m_Index = 1;
                    }
                    else if ((xDelta == -22) && (yDelta == -22))
                    {
                        m_Index = 0;
                    }
                    else if ((xDelta == 0x16) && (yDelta == -22))
                    {
                        m_Index = 3;
                    }
                    else if ((xDelta == 0x16) && (yDelta == 0x16))
                    {
                        m_Index = 5;
                    }
                    else
                    {
                        m_Index = 7;
                    }
                    return false;
                }
            }
            return true;
        }

        public bool Slice()
        {
            if (this.m_OnScreen || (this.m_Slices++ >= this.m_SliceCheck))
            {
                double normalized = this.m_Sync.Normalized;
                if (normalized >= 1.0)
                {
                    return false;
                }
                int x = (int)(this.m_xStart + ((this.m_xEnd - this.m_xStart) * normalized));
                int y = (int)(this.m_yStart + ((this.m_yEnd - this.m_yStart) * normalized));
                x -= Renderer.m_xScroll;
                y -= Renderer.m_yScroll;
                if (((x >= 0) && (y >= 0)) && ((x < Engine.ScreenWidth) && (y < Engine.ScreenHeight)))
                {
                    this.m_Image.DrawRotated(x, y, this.m_Angle - 1.5707963267948966, 0x8080c0);
                }
            }
            return true;
        }
    }
}