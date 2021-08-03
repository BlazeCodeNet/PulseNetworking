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

        public float x { get; private set; }
        public float y { get; private set; }
        public float z { get; private set; }
    }
}