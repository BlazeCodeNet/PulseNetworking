using System;

namespace PulseNet.utils
{
    public class Vec3f
    {
        public Vec3f(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float GetMagnitude()
        {
            return (float)Math.Sqrt( Math.Pow( x, 2 ) + Math.Pow( y, 2 ) + Math.Pow( z, 2 ) );
        }
        
        public Vec3f GetDirection( Vec3f from, Vec3f to )
        {
            Vec3f heading = to - from;

            float distance = heading.GetMagnitude( );

            return heading / distance;
        }

        public static float Distance( Vec3f a, Vec3f b )
        {
            if(a==null || b==null)
            {
                return -1;
            }

            return (float)Math.Sqrt( (float)Math.Pow( b.x - a.x, 2 ) + (float)Math.Pow( b.y - a.y, 2 ) + (float)Math.Pow( b.z - a.z, 2 ) );
        }

        public static float QuickDistance( Vec3f a, Vec3f b )
        {
            return (float)Math.Pow( b.x - a.x, 2 ) + (float)Math.Pow( b.y - a.y, 2 ) + (float)Math.Pow( b.z - a.z, 2 );
        }

        public static bool IsAligned( Vec3f a, Vec3f b, Vec3f c )
        {
            float t1 = ( ( c.x - a.x ) / ( b.x - a.x ) );
            float t2 = ( ( c.y - a.y ) / ( b.y - a.y ) );
            float t3 = ( ( c.z - a.z ) / ( b.z - a.z ) );

            return IsFloatClose( t1, t2 ) && IsFloatClose( t2, t3 ) && IsFloatClose( t3, t1 );
        }

        public static bool IsFloatClose( float a, float b)
        {
            return Math.Abs( a - b ) < 0.0001f;
        }

        public override string ToString( )
        {
            return "(" + x + ", " + y + ", " + z + ")";
        }

        public static Vec3f zero
        {
            get
            {
                return new Vec3f( 0f, 0f, 0f );
            }
        }

        public static Vec3f operator *(Vec3f a, Vec3f b)
        {
            return new Vec3f(a.x*b.x, a.y*b.y, a.z*b.z);
        }
        public static Vec3f operator /(Vec3f a, Vec3f b)
        {
            return new Vec3f(a.x/b.x, a.y/b.y, a.z/b.z);
        }
        public static Vec3f operator +(Vec3f a, Vec3f b)
        {
            return new Vec3f(a.x+b.x, a.y+b.y, a.z+b.z);
        }
        public static Vec3f operator -(Vec3f a, Vec3f b)
        {
            return new Vec3f(a.x-b.x, a.y-b.y, a.z-b.z);
        }

        public static Vec3f operator /( Vec3f vec, float input)
        {
            return new Vec3f( vec.x / input, vec.y / input, vec.z / input );
        }
        public static Vec3f operator *( Vec3f vec, float input)
        {
            return new Vec3f( vec.x * input, vec.y * input, vec.z * input );
        }

        public float x { get; private set; }
        public float y { get; private set; }
        public float z { get; private set; }
    }
}