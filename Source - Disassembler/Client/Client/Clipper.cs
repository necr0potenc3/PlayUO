namespace Client
{
    using Microsoft.DirectX.Direct3D;
    using System;

    public class Clipper
    {
        private static Clipper m_Clipper = new Clipper(0, 0, 0, 0);
        protected int m_xEnd;
        protected int m_xStart;
        protected int m_yEnd;
        protected int m_yStart;

        public Clipper(int xStart, int yStart, int xWidth, int yHeight)
        {
            this.m_xStart = xStart;
            this.m_yStart = yStart;
            this.m_xEnd = xStart + xWidth;
            this.m_yEnd = yStart + yHeight;
        }

        public bool Clip(int xStart, int yStart, int xWidth, int yHeight, CustomVertex.TransformedColoredTextured[] Vertices)
        {
            switch (this.Evaluate(xStart, yStart, xWidth, yHeight))
            {
                case ClipType.Outside:
                    return false;

                case ClipType.Inside:
                {
                    float num = -0.5f + xStart;
                    float num2 = -0.5f + yStart;
                    float num3 = num + xWidth;
                    float num4 = num2 + yHeight;
                    Vertices[0].X = Vertices[1].X = num3;
                    Vertices[0].Y = Vertices[2].Y = num4;
                    Vertices[1].Y = Vertices[3].Y = num2;
                    Vertices[2].X = Vertices[3].X = num;
                    Vertices[0].Tu = Vertices[0].Tv = Vertices[1].Tu = Vertices[2].Tv = 1f;
                    Vertices[1].Tv = Vertices[2].Tu = Vertices[3].Tu = Vertices[3].Tv = 0f;
                    return true;
                }
                case ClipType.Partial:
                {
                    int num5 = xStart;
                    int num6 = yStart;
                    int xEnd = xStart + xWidth;
                    int yEnd = yStart + yHeight;
                    if (xStart < this.m_xStart)
                    {
                        num5 = this.m_xStart;
                    }
                    if (yStart < this.m_yStart)
                    {
                        num6 = this.m_yStart;
                    }
                    if (xEnd > this.m_xEnd)
                    {
                        xEnd = this.m_xEnd;
                    }
                    if (yEnd > this.m_yEnd)
                    {
                        yEnd = this.m_yEnd;
                    }
                    Vertices[0].X = Vertices[1].X = -0.5f + xEnd;
                    Vertices[0].Y = Vertices[2].Y = -0.5f + yEnd;
                    Vertices[1].Y = Vertices[3].Y = -0.5f + num6;
                    Vertices[2].X = Vertices[3].X = -0.5f + num5;
                    double num9 = 1.0 / ((double) xWidth);
                    double num10 = 1.0 / ((double) yHeight);
                    Vertices[0].Tu = Vertices[1].Tu = (float) (num9 * (xEnd - xStart));
                    Vertices[0].Tv = Vertices[2].Tv = (float) (num10 * (yEnd - yStart));
                    Vertices[1].Tv = Vertices[3].Tv = (float) (num10 * (num6 - yStart));
                    Vertices[2].Tu = Vertices[3].Tu = (float) (num9 * (num5 - xStart));
                    return true;
                }
            }
            return false;
        }

        public override bool Equals(object Target)
        {
            if ((Target == null) || (Target.GetType() != typeof(Clipper)))
            {
                return false;
            }
            Clipper clipper = (Clipper) Target;
            return ((clipper == this) || ((((this.m_xStart == clipper.m_xStart) && (this.m_yStart == clipper.m_yStart)) && (this.m_xEnd == clipper.m_xEnd)) && (this.m_yEnd == clipper.m_yEnd)));
        }

        public bool Evaluate(Point p)
        {
            return ((((p.X >= this.m_xStart) && (p.X < this.m_xEnd)) && (p.Y >= this.m_yStart)) && (p.Y < this.m_yEnd));
        }

        public bool Evaluate(int xPoint, int yPoint)
        {
            return ((((xPoint >= this.m_xStart) && (yPoint >= this.m_yStart)) && (xPoint < this.m_xEnd)) && (yPoint < this.m_yEnd));
        }

        public ClipType Evaluate(int xStart, int yStart, int xWidth, int yHeight)
        {
            int num = xStart + xWidth;
            int num2 = yStart + yHeight;
            if (((num <= this.m_xStart) || (num2 <= this.m_yStart)) || ((xStart >= this.m_xEnd) || (yStart >= this.m_yEnd)))
            {
                return ClipType.Outside;
            }
            if (((xStart >= this.m_xStart) && (yStart >= this.m_yStart)) && ((num <= this.m_xEnd) && (num2 <= this.m_yEnd)))
            {
                return ClipType.Inside;
            }
            return ClipType.Partial;
        }

        public override int GetHashCode()
        {
            return (((this.m_xStart ^ this.m_yStart) ^ this.m_xEnd) ^ this.m_yEnd);
        }

        public static Clipper TemporaryInstance(int x, int y, int width, int height)
        {
            m_Clipper.m_xStart = x;
            m_Clipper.m_yStart = y;
            m_Clipper.m_xEnd = x + width;
            m_Clipper.m_yEnd = y + height;
            return m_Clipper;
        }

        public int xEnd
        {
            get
            {
                return this.m_xEnd;
            }
        }

        public int xStart
        {
            get
            {
                return this.m_xStart;
            }
        }

        public int yEnd
        {
            get
            {
                return this.m_yEnd;
            }
        }

        public int yStart
        {
            get
            {
                return this.m_yStart;
            }
        }
    }
}

