using System;
namespace MathKit
{
    /// <summary>
    /// 3x3行列
    /// </summary>
    public struct Float3x3: IEquatable<Float3x3>
    {
        /// <summary>
        /// 列1
        /// </summary>
        public Float3 C0;

        /// <summary>
        /// 列2
        /// </summary>
        public Float3 C1;

        /// <summary>
        /// 列3
        /// </summary>
        public Float3 C2;


        public Float3x3(in Float3 c0, in Float3 c1, in Float3 c2)
        {
            C0 = c0;
            C1 = c1;
            C2 = c2;
        }


        public Float3x3(
            float m00, float m10, float m20,
            float m01, float m11, float m21,
            float m02, float m12, float m22
        )
        {
            C0 = new Float3(m00, m01, m02);
            C1 = new Float3(m10, m11, m12);
            C2 = new Float3(m20, m21, m22);
        }


        public Float3 Row0 => new Float3(C0.x, C1.x, C2.x);
        public Float3 Row1 => new Float3(C0.y, C1.y, C2.y);
        public Float3 Row2 => new Float3(C0.z, C1.z, C2.z); 


        public static Float3x3 operator +(in Float3x3 a, in Float3x3 b)
            => new Float3x3(a.C0 + b.C0, a.C1 + b.C1, a.C2 + b.C2);


        public static Float3x3 operator -(in Float3x3 a, in Float3x3 b)
            => new Float3x3(a.C0 - b.C0, a.C1 - b.C1, a.C2 - b.C2);


        public static Float3x3 operator *(in Float3x3 a, in Float3x3 b)
        {
            return new Float3x3(
                a.C0 * b.C0.x + a.C1 * b.C0.y + a.C2 * b.C0.z,
                a.C0 * b.C1.x + a.C1 * b.C1.y + a.C2 * b.C1.z,
                a.C0 * b.C2.x + a.C1 * b.C2.y + a.C2 * b.C2.z
            );
        }


        public static Float3x3 operator *(in Float3x3 matrix, float value)
            => new Float3x3(matrix.C0 * value, matrix.C1 * value, matrix.C2 * value);


        public static Float3x3 operator *(float value, in Float3x3 matrix) => matrix * value;


        public static Float3 operator *(in Float3x3 matrix, in Float3 vector)
            => matrix.C0 * vector.x + matrix.C1 * vector.y + matrix.C2 * vector.z;


        public static Float3 operator *(in Float3 vector, in Float3x3 matrix) 
        {
            return new Float3(
                vector.x * matrix.C0.x + vector.y * matrix.C0.y + vector.z * matrix.C0.z,
                vector.x * matrix.C1.x + vector.y * matrix.C1.y + vector.z * matrix.C1.z,
                vector.x * matrix.C2.x + vector.y * matrix.C2.y + vector.z * matrix.C2.z
            );
        }


        public static bool operator ==(in Float3x3 a, in Float3x3 b) => (a.C0 == b.C0 && a.C1 == b.C1 && a.C2 == b.C2);


        public static bool operator !=(in Float3x3 a, in Float3x3 b) => !(a == b);


        public static Float3x3 Transpose(Float3x3 matrix)
        {
            return new Float3x3(
                matrix.Row0,
                matrix.Row1,
                matrix.Row2
            );
        }


        public void Transpose() => this = Transpose(this);


        public static Float3x3 CreateTranslation(Float2 translation)
        {
            return new Float3x3( 
                Float3.UnitX, 
                Float3.UnitY,
                new Float3(translation, 1f)
            );
        }


        public static Float3x3 CreateScale(Float2 scale)
        {
            return new Float3x3(
                scale.x, 0f, 0f,
                0f, scale.y, 0f,
                0f, 0f,      1f
            );
        }


        public static Float3x3 CreateRotation(float sin, float cos)
        {
            return new Float3x3(
                cos, -sin, 0f,
                sin, cos,  0f,
                0f,  0f,   1f
            );
        }


        public static Float3x3 CreateRotation(float rad)
        {
            return CreateRotation(MathF.Sin(rad), MathF.Cos(rad));
        }


        public override bool Equals(object obj)
        {
            if (obj is Float3x3) {
                return Equals((Float3x3)obj);
            }
            return false;
        }


        public bool Equals(Float3x3 matrix)
        {
            return (this == matrix);
        }


        /// <summary>
        /// Get this hash code
        /// </summary>
        /// <returns>The hash code</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(C0, C1, C2);
        }


        public override string ToString()
        {   
         return @$"{C0.x}, {C1.x}, {C2.x},
                   {C0.y}, {C1.y}, {C2.y},
                   {C0.z}, {C1.z}, {C2.z}";
        }


        private static Float3x3 identity = new Float3x3(Float3.UnitX, Float3.UnitY, Float3.UnitZ);


        public static Float3x3 Identity => identity;
    }
}