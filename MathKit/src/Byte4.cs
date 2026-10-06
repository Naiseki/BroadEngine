using System;

namespace MathKit
{
    public struct Byte4: IEquatable<Byte4>
    {
        public byte x;
        public byte y;
        public byte z;
        public byte w;


        public Byte4(byte x, byte y, byte z, byte w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }


        public Byte4(byte s) 
        {
            x = y = z = w = s;
        }


        public void Set(byte newX, byte newY, byte newZ, byte newW)
        {
            x = newX;
            y = newY;
            z = newZ;
            w = newW;
        }


        public void Set(byte s) => x = y = z = w = s;


        public int Added => x + y + z + w;

        public int Substructed => x - y - z - w;

        public int Multiplied => x * y * z * w;

        public int Divided => x / y / z / w;


        public Byte4 wzyx => new Byte4(w, z, y, x);


        public byte this[int index] {
            get => index switch {
                0 => x,
                1 => y,
                2 => z,
                3 => z,
                _ => throw new IndexOutOfRangeException("Invalid index!"),
            };

            set {
                switch (index) {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    case 3: w = value; break;
                    default: throw new IndexOutOfRangeException("Invalid index!");
                }
            }
        }


        /// <summary>
        /// Magnitude of this vector
        /// </summary>
        public float Magnitude => MathF.Sqrt((float)MagnitudeSqr);

        /// <summary>
        /// Returns the squared magnitude of this vector
        /// </summary>
        public int MagnitudeSqr => x * x + y * y + z * z + w * w;


        /// <summary>
        /// Returns the distance between a and b
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>The distance</returns>
        public static float Distance(Byte4 a, Byte4 b) => MathF.Sqrt(DistanceSqr(a, b));


        /// <summary>
        /// Returns the squared distance between a and b
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>The squared distance</returns>
        public static int DistanceSqr(Byte4 a, Byte4 b) 
        {
            int dx = a.x - b.x;
            int dy = a.y - b.y;
            int dz = a.z - b.z;
            int dw = a.w - b.w;
            return dx * dx + dy * dy + dz * dz + dw * dw;
        }


        /// <summary>
        /// Dot product of two vectors
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>Dot product</returns>
        public static int Dot(Byte4 a, Byte4 b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;


        public bool IsZero => (MagnitudeSqr < MathUtil.EpsilonSqr);


        public bool IsVertical(in Byte4 a, in Byte4 b)
        {
            if (a.IsZero || b.IsZero)
                return false;
            return (Dot(a, b) == 0);
        }


        public static bool InRange(in Byte4 value, in Byte4 min, in Byte4 max)
        {
            return (
                min.x <= value.x && value.x <= max.x &&
                min.y <= value.y && value.y <= max.y &&
                min.z <= value.z && value.z <= max.z &&
                min.w <= value.w && value.w <= max.w
            );
        }


        public byte MinComponent => MathUtil.Min(x, y, z, w);
        public byte MaxComponent => MathUtil.Max(x, y, z, w);


        public static Byte4 Min(in Byte4 a, in Byte4 b) => new(Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z), Math.Min(a.w, b.w));
        public static Byte4 Max(in Byte4 a, in Byte4 b) => new(Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z), Math.Max(a.w, b.w));


        public static bool operator ==(in Byte4 a, in Byte4 b)
        {
            return (a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w);
        }
    
        public static bool operator !=(in Byte4 a, in Byte4 b) => !(a == b);

        public static bool operator <(in Byte4 a, in Byte4 b)  => a.MagnitudeSqr < b.MagnitudeSqr;
        public static bool operator >(in Byte4 a, in Byte4 b)  => a.MagnitudeSqr > b.MagnitudeSqr;
        public static bool operator <=(in Byte4 a, in Byte4 b) => a.MagnitudeSqr <= b.MagnitudeSqr;
        public static bool operator >=(in Byte4 a, in Byte4 b) => a.MagnitudeSqr >= b.MagnitudeSqr;


        public override string ToString() => $"({x}, {y}, {z}, {w})";


        public bool Equals(Byte4 v) 
        {
             return (x == v.x && y == v.y && z == v.z && w == v.w);
        }


        public override bool Equals(object obj) => (obj is Byte4 v && Equals(v));


        public override int GetHashCode() => HashCode.Combine(x, y, z, w);


        /// <summary>
        /// (0,0,0,0)
        /// </summary>
        public static readonly Byte4 Zero =  new Byte4(0);

        /// <summary>
        /// (1,1,1,1)
        /// </summary>
        public static readonly Byte4 One = new Byte4(1);

        /// <summary>
        /// (1,0,0,0)
        /// </summary>
        public static readonly Byte4 UnitX = new Byte4(1, 0, 0, 0);
        
        /// <summary>
        /// (0,1,0,0)
        /// </summary>
        public static readonly Byte4 UnitY = new Byte4(0, 1, 0, 0);
        
        /// <summary>
        /// (0,0,1,0)
        /// </summary>
        public static readonly Byte4 UnitZ = new Byte4(0, 0, 1, 0);
        
        /// <summary>
        /// (0,0,0,1)
        /// </summary>
        public static readonly Byte4 UnitW = new Byte4(0, 0, 0, 1);
    }
}
