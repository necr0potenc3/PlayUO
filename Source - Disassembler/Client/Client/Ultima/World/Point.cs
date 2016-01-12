namespace Client
{
    using System;

    public class Point : IPoint2D
    {
        private int m_X;
        private int m_Y;

        public Point(int X, int Y)
        {
            this.m_X = X;
            this.m_Y = Y;
        }

        public Point(Client.Point p, int xOffset, int yOffset)
        {
            this.m_X = p.m_X + xOffset;
            this.m_Y = p.m_Y + yOffset;
        }

        public override bool Equals(object o)
        {
            if ((o == null) || (o.GetType() != typeof(Client.Point)))
            {
                return false;
            }
            Client.Point point = (Client.Point)o;
            return ((this.m_X == point.m_X) && (this.m_Y == point.m_Y));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Client.Point operator +(Client.Point l, Client.Point r)
        {
            return new Client.Point(l.X + r.X, l.Y + r.Y);
        }

        public static Client.Point operator /(Client.Point l, int r)
        {
            return new Client.Point(l.X / r, l.Y / r);
        }

        public static bool operator ==(Client.Point p1, Client.Point p2)
        {
            return ((p1.m_X == p2.m_X) && (p1.m_Y == p2.m_Y));
        }

        public static int operator ^(Client.Point p1, Client.Point p2)
        {
            int num = Math.Abs((int)(p2.X - p1.X));
            int num2 = Math.Abs((int)(p2.Y - p1.Y));
            return (int)Math.Sqrt((double)((num * num) + (num2 * num2)));
        }

        public static implicit operator System.Drawing.Point(Client.Point p)
        {
            return new System.Drawing.Point(p.m_X, p.m_Y);
        }

        public static implicit operator Client.Point(System.Drawing.Point p)
        {
            return new Client.Point(p.X, p.Y);
        }

        public static bool operator !=(Client.Point p1, Client.Point p2)
        {
            return ((p1.m_X != p2.m_X) || (p1.m_Y != p2.m_Y));
        }

        public static Client.Point operator -(Client.Point l, Client.Point r)
        {
            return new Client.Point(l.X - r.X, l.Y - r.Y);
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