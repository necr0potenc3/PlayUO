namespace Client
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Point3D
    {
        public int X;
        public int Y;
        public int Z;
        public Point3D(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Point3D(IPoint3D p)
        {
            this.X = p.X;
            this.Y = p.Y;
            this.Z = p.Z;
        }

        public override bool Equals(object o)
        {
            if ((o == null) || !(o is IPoint3D))
            {
                return false;
            }
            IPoint3D pointd = (IPoint3D) o;
            return (((this.X == pointd.X) && (this.Y == pointd.Y)) && (this.Z == pointd.Z));
        }

        public override int GetHashCode()
        {
            return ((this.X ^ this.Y) ^ this.Z);
        }

        public static Point3D Parse(string value)
        {
            int index = value.IndexOf('(');
            int num2 = value.IndexOf(',', index + 1);
            string str = value.Substring(index + 1, num2 - (index + 1)).Trim();
            index = num2;
            num2 = value.IndexOf(',', index + 1);
            string str2 = value.Substring(index + 1, num2 - (index + 1)).Trim();
            index = num2;
            num2 = value.IndexOf(')', index + 1);
            string str3 = value.Substring(index + 1, num2 - (index + 1)).Trim();
            return new Point3D(Convert.ToInt32(str), Convert.ToInt32(str2), Convert.ToInt32(str3));
        }

        public static bool operator ==(Point3D l, Point3D r)
        {
            return (((l.X == r.X) && (l.Y == r.Y)) && (l.Z == r.Z));
        }

        public static bool operator !=(Point3D l, Point3D r)
        {
            return (((l.X != r.X) || (l.Y != r.Y)) || (l.Z != r.Z));
        }

        public static bool operator ==(Point3D l, IPoint3D r)
        {
            return (((l.X == r.X) && (l.Y == r.Y)) && (l.Z == r.Z));
        }

        public static bool operator !=(Point3D l, IPoint3D r)
        {
            return (((l.X != r.X) || (l.Y != r.Y)) || (l.Z != r.Z));
        }
    }
}

