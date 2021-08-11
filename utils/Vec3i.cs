namespace PulseNet.utils
{
    public class Vec3i
    {
        public Vec3i(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Vec3i operator *(Vec3i a, Vec3i b)
        {
            return new Vec3i(a.x*b.x, a.y*b.y, a.z*b.z);
        }

        public static Vec3i operator +(Vec3i a, Vec3i b)
        {
            return new Vec3i(a.x+b.x, a.y+b.y, a.z+b.z);
        }
        public static Vec3i operator -(Vec3i a, Vec3i b)
        {
            return new Vec3i(a.x-b.x, a.y-b.y, a.z-b.z);
        }

        public static Vec3i zero
        {
            get
            {
                return new Vec3i( 0, 0, 0 );
            }
        }

        public int x { get; private set; }
        public int y { get; private set; }
        public int z { get; private set; }
    }
}