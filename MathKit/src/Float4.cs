using System;

namespace MathKit
{
    public struct Float4: IEquatable<Float4>
    {
        public float x;
        public float y;
        public float z;
        public float w;


        public Float4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }


        public Float4(float s) 
        {
            x = y = z = w = s;
        }


        public Float4(Float3 vector, float w): this(vector.x, vector.y, vector.z, w) {}


        public Float4(Float2 vector, float z, float w): this(vector.x, vector.y, z, w) {}


        public Float4(Float2 v1, Float2 v2): this(v1.x, v1.y, v2.x, v2.y) {}


        public void Set(float newX, float newY, float newZ, float newW)
        {
            x = newX;
            y = newY;
            z = newZ;
            w = newW;
        }


        public void Set(float s) => x = y = z = w = s;


        public float Added => x + y + z + w;

        public float Substructed => x - y - z - w;

        public float Multiplied => x * y * z * w;

        public float Divided => x / y / z / w;


        public Float4 wzyx => new Float4(w, z, y, x);

        public Float3 xyz => new Float3(x, y, z);

        public Float2 xy => new Float2(x, y);

        public Float2 yz => new Float2(x, y);



        public float this[int index] {
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
        public float Magnitude { 
            get => MathF.Sqrt(MagnitudeSqr);
            set {
                if (MagnitudeSqr != 0f)
                    this *= value / Magnitude;
                else
                    this.Set(value, 0f, 0f, 0f);
            }
        }

        /// <summary>
        /// Returns the squared magnitude of this vector
        /// </summary>
        public float MagnitudeSqr => x * x + y * y + z * z + w * w;


        /// <summary>
        /// Normalize this vector
        /// </summary>
        public void Normalize() 
        {
            float mag = Magnitude;
            if (mag > 0f) {
                x /= mag;
                y /= mag;
                z /= mag;
                w /= mag;
            }
            else {
                this = UnitX;
            }
        }


        public Float4 Normalized {
            get {
                Float4 v = this;
                v.Normalize();
                return v;
            } 
        }


        /// <summary>
        /// Returns the distance between a and b
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>The distance</returns>
        public static float Distance(in Float4 a, in Float4 b) => MathF.Sqrt(DistanceSqr(a, b));


        /// <summary>
        /// Returns the squared distance between a and b
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>The squared distance</returns>
        public static float DistanceSqr(in Float4 a, in Float4 b) 
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;
            float dz = a.z - b.z;
            float dw = a.w - b.w;
            return dx * dx + dy * dy + dz * dz + dw * dw;
        }


        /// <summary>
        /// Dot product of two vectors
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>Dot product</returns>
        public static float Dot(in Float4 a, in Float4 b) => a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;


        public bool IsZero => (MagnitudeSqr < MathUtil.EpsilonSqr);

        public bool IsUnit {
            get {
                float mag = MagnitudeSqr - 1f;
                return (mag * mag < MathUtil.EpsilonSqr);
            }
        }


        public bool IsVertical(in Float4 a, in Float4 b)
        {
            if (a.IsZero || b.IsZero)
                return false;
            float dot = Dot(a, b);
            return dot * dot < MathUtil.EpsilonSqr;
        }


        /// <summary>
        /// Returns the reflection of a vector off a surface that has the specified normal
        /// </summary>
        /// <param name="vector">The vector</param>
        /// <param name="normal">The normal vector</param>
        /// <returns>The reflected vector</returns>
        public static Float4 Reflect(Float4 vector, Float4 normal)
        {
            return vector - 2f * Float4.Dot(vector, normal) * normal;
        }


        public static Float4 Lerp(Float4 a, Float4 b, float t)
        {
            t = MathUtil.Clamp01(t);
            return new Float4(
                a.x + (b.x - a.x) * t, 
                a.y + (b.y - a.y) * t, 
                a.z + (b.z - a.z) * t, 
                a.w + (b.w - a.w) * t 
            );
        }


        public static Float4 ClampMagnitude(Float4 v, float min, float max)
        {
            if (v.MagnitudeSqr < min * min) {
                v.Magnitude = min;
            }
            else if (v.MagnitudeSqr > max) {
                v.Magnitude = max;
            }
            return v;
        }


        public static bool InRange(in Float4 value, in Float4 min, in Float4 max)
        {
            return (
                min.x <= value.x && value.x <= max.x &&
                min.y <= value.y && value.y <= max.y &&
                min.z <= value.z && value.z <= max.z &&
                min.w <= value.w && value.w <= max.w
            );
        }


        public static Float4 Sign(Float4 value)
        {
            return new(Math.Sign(value.x), Math.Sign(value.y), Math.Sign(value.z), Math.Sign(value.w));
        }


        public float MinComponent => MathUtil.Min(x, y, z, w);
        public float MaxComponent => MathUtil.Max(x, y, z, w);


        public static Float4 Min(in Float4 a, in Float4 b) => new(Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z), Math.Min(a.w, b.w));
        public static Float4 Max(in Float4 a, in Float4 b) => new(Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z), Math.Max(a.w, b.w));


        public static Float4 operator +(in Float4 a, in Float4 b) => new(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        public static Float4 operator +(in Float4 v, float s)     => new(v.x + s, v.y + s, v.z + s, v.w + s);
        public static Float4 operator -(in Float4 a, in Float4 b) => new(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        public static Float4 operator -(in Float4 v, float s)     => new(v.x - s, v.y - s, v.z - s, v.w - s);
        public static Float4 operator -(in Float4 v)              => new(-v.x, -v.y, -v.z, -v.w);
        public static Float4 operator *(in Float4 v, float s)     => new(v.x * s, v.y * s, v.z * s, v.w * s);
        public static Float4 operator *(float s, in Float4 v)     => v * s;
        public static Float4 operator /(in Float4 v, float s)     => new(v.x / s, v.y / s, v.z / s, v.w / s);
        public static Float4 operator *(in Float4 a, in Float4 b) => new(a.x * b.x, a.y * b.y, a.z * b.z, a.w * b.w);
        public static Float4 operator /(in Float4 a, in Float4 b) => new(a.x / b.x, a.y / b.y, a.z / b.z, a.w / b.w);

        public static bool operator ==(in Float4 a, in Float4 b)
        {
            return (DistanceSqr(a, b) < MathUtil.EpsilonSqr);
        }
    
        public static bool operator !=(in Float4 a, in Float4 b) => !(a == b);

        public static bool operator <(in Float4 a, in Float4 b)  => a.MagnitudeSqr < b.MagnitudeSqr;
        public static bool operator >(in Float4 a, in Float4 b)  => a.MagnitudeSqr > b.MagnitudeSqr;
        public static bool operator <=(in Float4 a, in Float4 b) => a.MagnitudeSqr <= b.MagnitudeSqr;
        public static bool operator >=(in Float4 a, in Float4 b) => a.MagnitudeSqr >= b.MagnitudeSqr;

        public static explicit operator Float2(Float4 v) => new Float2(v.x, v.y);
        public static explicit operator Float3(Float4 v) => new Float3(v.x, v.y, v.z);
        public static implicit operator System.Numerics.Vector4(Float4 v) => new System.Numerics.Vector4(v.x, v.y, v.z, v.w);
        public static implicit operator Float4(System.Numerics.Vector4 v) => new Float4(v.X, v.Y, v.Z, v.W);

        public override string ToString() => $"({x}, {y}, {z}, {w})";


        public bool Equals(Float4 v) 
        {
             return (x == v.x && y == v.y && z == v.z && w == v.w);
        }


        public override bool Equals(object obj) => (obj is Float4 v && Equals(v));


        public override int GetHashCode() => HashCode.Combine(x, y, z, w);


        /// <summary>
        /// (0, 0, 0, 0)
        /// </summary>
        public static readonly Float4 Zero =  new Float4(0);

        /// <summary>
        /// (1, 1, 1, 1)
        /// </summary>
        public static readonly Float4 One = new Float4(1);

        /// <summary>
        /// (1, 0, 0, 0)
        /// </summary>
        public static readonly Float4 UnitX = new Float4(1, 0, 0, 0);
        
        /// <summary>
        /// (0, 1, 0, 0)
        /// </summary>
        public static readonly Float4 UnitY = new Float4(0, 1, 0, 0);
        
        /// <summary>
        /// (0, 0, 1, 0)
        /// </summary>
        public static readonly Float4 UnitZ = new Float4(0, 0, 1, 0);
        
        /// <summary>
        /// (0, 0, 0, 1)
        /// </summary>
        public static readonly Float4 UnitW = new Float4(0, 0, 0, 1);
    }
}
