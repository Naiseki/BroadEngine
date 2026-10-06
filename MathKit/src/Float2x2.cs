using System;

namespace MathKit
{
    public struct Float2x2: IEquatable<Float2x2>
    {
        public Float2 C0;
        public Float2 C1;


        public Float2x2(Float2 c1, Float2 c2)
        {
            C0 = c1;
            C1 = c2;
        }


        public Float2x2(float m11, float m12,
                        float m21, float m22)
        {
            C0 = new Float2(m11, m21);
            C1 = new Float2(m12, m22);
        }


        public Float2 Row0 => new Float2(C0.x, C1.x);
        public Float2 Row1 => new Float2(C0.y, C1.y);


        public float this[int row, int column] => this[column * 4 + row];

        public float this[int index] => index switch {
            0 => C0.x,
            1 => C1.x,
            2 => C0.y,
            3 => C1.y,
            _ => throw new IndexOutOfRangeException("Invalid index!")
        };


        public static Float2x2 operator *(in Float2x2 a, in Float2x2 b)
        {
            return new Float2x2(
                a.C0 * b.C0.x + a.C1 * b.C0.y,
                a.C0 * b.C1.x + a.C1 * b.C1.y
            );
        }

        public static Float2x2 operator *(in Float2x2 v, float k) => new(v.C0 * k, v.C1 * k);
        public static Float2x2 operator *(float k, in Float2x2 v) => new(v.C0 * k, v.C1 * k);

        public static Float2 operator *(in Float2 vector, in Float2x2 matrix)
        {
            return new Float2(
                vector.x * matrix.C0.x + vector.y * matrix.C0.y,
                vector.x * matrix.C1.x + vector.y * matrix.C1.y
            );
        }

        public static Float2 operator *(in Float2x2 matrix, in Float2 vector)
        {
            return matrix.C0 * vector.x + matrix.C1 * vector.y;
        }

        public static bool operator ==(in Float2x2 a, in Float2x2 b) => (a.C0 == b.C0 && a.C1 == b.C1);
        public static bool operator !=(in Float2x2 a, in Float2x2 b) => !(a == b);


        public static Float2x2 CreateScale(Float2 scale)
        {
            return new Float2x2(
                scale.x, 0f, 
                0f, scale.y   
            );
        }


        public static Float2x2 CreateRotation(float sin, float cos)
        {
            return new Float2x2(
                cos, -sin, 
                sin,  cos  
            );
        }


        public static Float2x2 CreateRotation(float rad)
        {
            return CreateRotation(MathF.Sin(rad), MathF.Cos(rad));
        }



        public static Float2x2 Transpose(Float2x2 matrix)
        {
            return new Float2x2(
                matrix.Row0,
                matrix.Row1
            );
        }


        public void Transpose() => this = Transpose(this);


        public bool Equals(Float2x2 v)
        {
            return (C0 == v.C0 && C1 == v.C1); 
        }


        public override bool Equals(object obj)
        {
            return obj is Float2x2 && Equals((Float2x2)obj);
        }


        public override string ToString() 
        {
            return @$"{C0.x}, {C1.x},
                      {C0.y}, {C1.y}";
        }


        public override int GetHashCode() => HashCode.Combine(C0, C1);


        /// <summary>
        /// (1, 0)
        /// (0, 1)
        /// </summary>
        public static Float2x2 Identity = new Float2x2(Float2.UnitX, Float2.UnitY);
    }
}