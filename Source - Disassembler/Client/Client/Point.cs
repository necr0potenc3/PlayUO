namespace Client
{
    using System;
    using System.Drawing;

    public class Point : IPoint2D
    {
        private int m_X;
        private int m_Y;

        public Point(int X, int Y)
        {
            this.m_X = X;
            this.m_Y = Y;
        }

        public Point(Point p, int xOffset, int yOffset)
        {
            this.m_X = p.m_X + xOffset;
            this.m_Y = p.m_Y + yOffset;
        }

        public override bool Equals(object o)
        {
            if ((o == null) || (o.GetType() != typeof(Point)))
            {
                return false;
            }
            Point point = (Point) o;
            return ((this.m_X == point.m_X) && (this.m_Y == point.m_Y));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Point operator +(Point l, Point r)
        {
            return new Point(l.X + r.X, l.Y + r.Y);
        }

        public static Point operator /(Point l, int r)
        {
            return new Point(l.X / r, l.Y / r);
        }

        public static bool operator ==(Point p1, Point p2)
        {
            return ((p1.m_X == p2.m_X) && (p1.m_Y == p2.m_Y));
        }

        public static int operator ^(Point p1, Point p2)
        {
            int num = Math.Abs((int) (p2.X - p1.X));
            int num2 = Math.Abs((int) (p2.Y - p1.Y));
            return (int) Math.Sqrt((double) ((num * num) + (num2 * num2)));
        }

        public static implicit operator Point(Point p)
        {
            return new Point(p.m_X, p.m_Y);
        }

        public static implicit operator Point(Point p)
        {
            return new Point(p.X, p.Y);
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return ((p1.m_X != p2.m_X) || (p1.m_Y != p2.m_Y));
        }

        public static Point operator -(Point l, Point r)
        {
            return new Point(l.X - r.X, l.Y - r.Y);
        }

        public int X
        {
            get
            {
                return this.m_X;
            }
            set
            {
                this.m_X = value;
            }
        }

        public int Y
        {
            get
            {
                return this.m_Y;
            }
            set
            {
                this.m_Y = value;
            }
        }
    }
}

