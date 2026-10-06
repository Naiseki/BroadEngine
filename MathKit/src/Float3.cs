using System;

namespace MathKit
{
    public struct Float3: IEquatable<Float3>
    {
        public float x;
        public float y;
        public float z;


        public Float3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }


        public Float3(float k) 
        {
            x = y = z = k;
        }


        public Float3(Float2 vector): this(vector.x, vector.y, 0f) {}


        public Float3(Float2 vector, float z): this(vector.x, vector.y, z) {}


        public void Set(float newX, float newY, float newZ)
        {
            x = newX;
            y = newY;
            z = newZ;
        }


        public void Set(float s) => x = y = z = s;


        public float Added => x + y + z;

        public float Substructed => x - y - z;

        public float Multiplied => x * y * z;

        public float Divided => x / y / z;

        public Float2 xy => new Float2(x, y);

        public Float2 yz => new Float2(y, z);

        public Float2 zx => new Float2(z, x);

        public Float3 zyx => new Float3(z, y, x);

        public Float3 xxx => new Float3(x);

        public Float3 yyy => new Float3(y);

        public Float3 zzz => new Float3(z);


        public float this[int index] => index switch {
            0 => x,
            1 => y,
            2 => z,
            _ => throw new IndexOutOfRangeException("Invalid index!"),
        };


        /// <summary>
        /// Magnitude of this vector
        /// </summary>
        public float Magnitude { 
            get => MathF.Sqrt(MagnitudeSqr);
            set {
                if (MagnitudeSqr != 0f)
                    this *= value / Magnitude;
                else
                    this.Set(value, 0f, 0f);
            }
        }

        //Returns the squared magnitude of this vector
        public float MagnitudeSqr => x * x + y * y + z * z;


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
            }
            else {
                this = UnitX;
            }
        }


        public Float3 Normalized {
            get {
                Float3 v;
                float mag = Magnitude;
                if (mag > 0f) {
                    v.x = x / mag;
                    v.y = y / mag;
                    v.z = z / mag;
                }
                else {
                    v = UnitX;
                }
                return v;
            } 
        }


        /// <summary>
        /// Returns the distance between a and b
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>The distance</returns>
        public static float Distance(in Float3 a, in Float3 b) => MathF.Sqrt(DistanceSqr(a, b));


        /// <summary>
        /// Returns the squared distance between a and b
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>The squared distance</returns>
        public static float DistanceSqr(in Float3 a, in Float3 b) 
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;
            float dz = a.z - b.z;
            return dx * dx + dy * dy + dz * dz;
        }


        /// <summary>
        /// Dot product of two vectors
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>Dot product</returns>
        public static float Dot(Float3 a, Float3 b) => a.x * b.x + a.y * b.y + a.z * b.z;


        /// <summary>
        /// Cross product of two vectors
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>Cross product</returns>
        public static Float3 Cross(Float3 a, Float3 b) 
        {
            return new Float3(
                a.y * b.z - a.z * b.y,
                a.z * b.x - a.x * b.z,
                a.x * b.y - a.y * b.x
            );
        }


        public bool IsZero => (MagnitudeSqr < MathUtil.EpsilonSqr);

        public bool IsUnit {
            get {
                float mag = MagnitudeSqr - 1f;
                return (mag * mag < MathUtil.EpsilonSqr);
            }
        }


        public bool IsVertical(in Float3 a, in Float3 b)
        {
            if (a.IsZero || b.IsZero)
                return false;
            float dot = Dot(a, b);
            return dot * dot < MathUtil.EpsilonSqr;
        }


        public bool IsHorizontal(in Float3 a, in Float3 b)
        {
            if (a.IsZero || b.IsZero)
                return false;
            Float3 cross = Cross(a, b);
            return (cross.IsZero);
        }


        /// <summary>
        /// Returns the reflection of a vector off a surface that has the specified normal
        /// </summary>
        /// <param name="vector">The vector</param>
        /// <param name="normal">The normal vector</param>
        /// <returns>The reflected vector</returns>
        public static Float3 Reflect(Float3 vector, Float3 normal)
        {
            return vector - 2f * Float3.Dot(vector, normal) * normal;
        }


        public static float Angle(Float3 from, Float3 to) => MathF.Atan2(to.y - from.y, to.x - from.x);


        public float Angle() => MathF.Atan2(y, x);


        public static Float3 Lerp(Float3 a, Float3 b, float t)
        {
            t = MathUtil.Clamp01(t);
            return new Float3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
        }


        public static Float3 ClampMagnitude(Float3 v, float min, float max)
        {
            if (v.MagnitudeSqr < min * min) {
                v.Magnitude = min;
            }
            else if (v.MagnitudeSqr > max) {
                v.Magnitude = max;
            }
            return v;
        }


        public static bool InRange(in Float3 value, in Float3 min, in Float3 max)
        {
            return (min.x <= value.x && value.x <= max.x &&
                    min.y <= value.y && value.y <= max.y &&
                    min.z <= value.z && value.z <= max.z);
        }


        public static Float3 Sign(Float3 value) => new(Math.Sign(value.x), Math.Sign(value.y), Math.Sign(value.z));


        public float MinComponent => MathUtil.Min(x, y, z);
        
        public float MaxComponent => MathUtil.Max(x, y, z);


        public static Float3 Min(Float3 a, Float3 b) => new(Math.Min(a.x, b.x), Math.Min(a.y, b.y), Math.Min(a.z, b.z));
        public static Float3 Max(Float3 a, Float3 b) => new(Math.Max(a.x, b.x), Math.Max(a.y, b.y), Math.Max(a.z, b.z));


        public static Float3 operator +(in Float3 a, in Float3 b) => new(a.x + b.x, a.y + b.y, a.z + b.z);

        public static Float3 operator +(in Float3 v, float s)     => new(v.x + s, v.y + s, v.z + s);

        public static Float3 operator -(in Float3 a, in Float3 b) => new(a.x - b.x, a.y - b.y, a.z - b.z);

        public static Float3 operator -(in Float3 v, float s)     => new(v.x - s, v.y - s, v.z - s);

        public static Float3 operator -(in Float3 v)              => new(-v.x, -v.y, -v.z);

        public static Float3 operator *(in Float3 v, float s)     => new(v.x * s, v.y * s, v.z * s);

        public static Float3 operator *(float s, in Float3 v)     => v * s;

        public static Float3 operator /(in Float3 v, float s)     => new(v.x / s, v.y / s, v.z / s);

        public static Float3 operator /(float s,in Float3 v)      => new(s / v.x, s / v.y, s / v.z);

        public static Float3 operator *(in Float3 a, in Float3 b) => new(a.x * b.x, a.y * b.y, a.z * b.z);

        public static Float3 operator /(in Float3 a, in Float3 b) => new(a.x / b.x, a.y / b.y, a.z / b.z);

        public static bool operator ==(in Float3 a, in Float3 b)
        {
            return (DistanceSqr(a, b) < MathUtil.EpsilonSqr);
        }
    
        public static bool operator !=(in Float3 a, in Float3 b) => !(a == b);

        public static Bool3 operator <(in Float3 a, in Float3 b)  => new Bool3(a.x < b.x, a.y < b.y, a.z < b.z);

        public static Bool3 operator >(in Float3 a, in Float3 b)  => new Bool3(a.x > b.x, a.y > b.y, a.z > b.z);

        public static Bool3 operator <=(in Float3 a, in Float3 b) => new Bool3(a.x <= b.x, a.y <= b.y, a.z <= b.z);

        public static Bool3 operator >=(in Float3 a, in Float3 b) => new Bool3(a.x >= b.x, a.y >= b.y, a.z >= b.z);

        public static explicit operator Float2(Float3 v) => new Float2(v.x, v.y);

        public static implicit operator System.Numerics.Vector3(Float3 v) => new System.Numerics.Vector3(v.x, v.y, v.z);

        public static implicit operator Float3(System.Numerics.Vector3 v) => new Float3(v.X, v.Y, v.Z);

        public override string ToString() => $"({x}, {y}, {z})";


        public bool Equals(Float3 v) 
        {
             return x == v.x && y == v.y && z == v.z;
        }


        public override bool Equals(object obj) => (obj is Float3 v && Equals(v));


        public override int GetHashCode() => HashCode.Combine(x, y, z);


        /// <summary>
        /// (0, 0, 0)
        /// </summary>
        public static readonly Float3 Zero = new Float3(0f);

        
        /// <summary>
        /// (1, 1, 1)
        /// </summary>
        public static readonly Float3 One = new Float3(1f);

        
        /// <summary>
        /// (1, 0, 0)
        /// </summary>
        public static readonly Float3 UnitX = new Float3(1f, 0f, 0f);

        
        /// <summary>
        /// (0, 1, 0)
        /// </summary>
        public static readonly Float3 UnitY = new Float3(0f, 1f, 0f);

        
        /// <summary>
        /// (0, 0, 1)
        /// </summary>
        public static readonly Float3 UnitZ = new Float3(0f, 0f, 1f);
    }
}
