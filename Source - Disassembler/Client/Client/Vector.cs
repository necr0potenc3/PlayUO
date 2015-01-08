namespace Client
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Vector
    {
        public float m_X;
        public float m_Y;
        public float m_Z;
        public override bool Equals(object o)
        {
            return (this == ((Vector) o));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Vector(float X, float Y, float Z)
        {
            this.m_X = X;
            this.m_Y = Y;
            this.m_Z = Z;
        }

        public static bool operator ==(Vector v1, Vector v2)
        {
            return (((v1.m_X == v2.m_X) && (v1.m_Y == v2.m_Y)) && (v1.m_Z == v2.m_Z));
        }

        public static bool operator !=(Vector v1, Vector v2)
        {
            return (((v1.m_X != v2.m_X) || (v1.m_Y != v2.m_Y)) || !(v1.m_Z == v2.m_Z));
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.m_X + v2.m_X, v1.m_Y + v2.m_Y, v1.m_Z + v2.m_Z);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.m_X - v2.m_X, v1.m_Y - v2.m_Y, v1.m_Z - v2.m_Z);
        }

        public static Vector operator *(Vector v1, Vector v2)
        {
            return new Vector(v1.m_X * v2.m_X, v1.m_Y * v2.m_Y, v1.m_Z * v2.m_Z);
        }

        public static Vector operator *(Vector v1, float f)
        {
            return new Vector(v1.m_X * f, v1.m_Y * f, v1.m_Z * f);
        }

        public static Vector operator /(Vector v1, Vector v2)
        {
            return new Vector(v1.m_X / v2.m_X, v1.m_Y / v2.m_Y, v1.m_Z / v2.m_Z);
        }

        public static Vector operator /(Vector v1, float f)
        {
            return new Vector(v1.m_X / f, v1.m_Y / f, v1.m_Z / f);
        }

        public float Dot(Vector v)
        {
            return (((this.m_X * v.m_X) + (this.m_Y * v.m_Y)) + (this.m_Z * v.m_Z));
        }

        public Vector Cross(Vector v)
        {
            return new Vector((this.m_Y * v.m_Z) - (this.m_Z * v.m_Y), (this.m_Z * v.m_X) - (this.m_X * v.m_Z), (this.m_X * v.m_Y) - (this.m_Y * v.m_X));
        }

        public Vector Normalize()
        {
            float num = (float) Math.Sqrt((double) (((this.m_X * this.m_X) + (this.m_Y * this.m_Y)) + (this.m_Z * this.m_Z)));
            return new Vector(this.m_X / num, this.m_Y / num, this.m_Z / num);
        }
    }
}

