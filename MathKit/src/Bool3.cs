using System;

namespace MathKit
{
    public struct Bool3: IEquatable<Bool3>
    {
        public bool x;
        
        public bool y;

        public bool z;


        public Bool3(bool x, bool y, bool z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }


        public Bool3(bool value) 
        {
            x = y = z = value;
        }


        public bool this[int index] => index switch {
            0 => x,
            1 => y,
            2 => z,
            _ => throw new IndexOutOfRangeException("Invalid index!")
        };


        /// <summary>
        /// 全てtrueかどうか
        /// </summary>
        public bool AllTrue => x && y && z;

        /// <summary>
        /// 全てfalseかどうか
        /// </summary>
        public bool AllFalse => !x && !y && !z;


        /// <summary>
        /// 一つでもtrueがあるかどうか
        /// </summary>
        public bool AnyTrue => x || y || z;

        /// <summary>
        /// 一つでもfalseがあるかどうか
        /// </summary>
        public bool AnyFalse => !x || !y || !z;


        public static bool operator ==(Bool3 a, Bool3 b) => a.Equals(b);

        public static bool operator !=(Bool3 a, Bool3 b) => !a.Equals(b);

        public static Bool3 operator &(Bool3 a, Bool3 b) => new(a.x & b.x, a.y & b.y, a.z & b.z);

        public static Bool3 operator &(Bool3 a, bool b) => new(a.x & b, a.y & b, a.z & b);

        public static Bool3 operator &(bool a, Bool3 b) => a & b;

        public static Bool3 operator |(Bool3 a, Bool3 b) => new(a.x | b.x, a.y | b.y, a.x | b.z);

        public static Bool3 operator |(Bool3 a, bool b) => new(a.x | b, a.y | b, a.z | b);

        public static Bool3 operator |(bool a, Bool3 b) => a | b;

        public static Bool3 operator ^(Bool3 a, Bool3 b) => new(a.x ^ b.x, a.y ^ b.y, a.z ^ b.z);

        public static Bool3 operator ^(Bool3 a, bool b) => new(a.x ^ b, a.y ^ b, a.z ^ b);

        public static Bool3 operator ^(bool a, Bool3 b) => a ^ b;

        public static Bool3 operator !(Bool3 val) => new(!val.x, !val.y, !val.z);


        public override bool Equals(object obj) => (obj is Bool3 converted && Equals(converted));

        public bool Equals(Bool3 other) 
        {
            return x == other.x && y == other.y && z == other.z;
        }


        public override string ToString() => $"({x}, {y}, {z})";


        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }


        /// <summary>
        /// (true, true, true)
        /// </summary>
        public static readonly Bool3 True = new(true);

        /// <summary>
        /// (false, false, false)
        /// </summary>
        public static readonly Bool3 False = new(false);

        /// <summary>
        /// (true, false, false)
        /// </summary>
        public static readonly Bool3 TrueX = new(true, false, false);

        /// <summary>
        /// (false, true, false)
        /// </summary>
        public static readonly Bool3 TrueY = new(false, true, false);

        /// <summary>
        /// (false, false, true)
        /// </summary>
        public static readonly Bool3 TrueZ = new(false, false, true);
    }
}
