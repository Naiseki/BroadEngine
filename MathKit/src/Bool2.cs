using System;

namespace MathKit
{
    public struct Bool2: IEquatable<Bool2>
    {
        public bool x;
        public bool y;


        public Bool2(bool x, bool y)
        {
            this.x = x;
            this.y = y;
        }


        public Bool2(bool value) => x = y = value;


        public bool this[int index] => index switch {
            0 => x,
            1 => y,
            _ => throw new IndexOutOfRangeException("Invalid index!")
        };


        /// <summary>
        /// 全てtrueかどうか
        /// </summary>
        public bool AllTrue => (x && y);

        /// <summary>
        /// 全てfalseかどうか
        /// </summary>
        public bool AllFalse => (!x && !y);


        /// <summary>
        /// 一つでもtrueがあるかどうか
        /// </summary>
        public bool AnyTrue => (x || y);

        /// <summary>
        /// 一つでもfalseがあるかどうか
        /// </summary>
        public bool AnyFalse => (!x || !y);


        public static bool operator ==(Bool2 a, Bool2 b) => a.Equals(b);
        public static bool operator !=(Bool2 a, Bool2 b) => !a.Equals(b);
        public static Bool2 operator &(Bool2 a, Bool2 b) => new(a.x & b.x, a.y & b.y);
        public static Bool2 operator &(Bool2 a, bool b) => new(a.x & b, a.y & b);
        public static Bool2 operator &(bool a, Bool2 b) => a & b;
        public static Bool2 operator |(Bool2 a, Bool2 b) => new(a.x | b.x, a.y | b.y);
        public static Bool2 operator |(Bool2 a, bool b) => new(a.x | b, a.y | b);
        public static Bool2 operator |(bool a, Bool2 b) => a | b;
        public static Bool2 operator ^(Bool2 a, Bool2 b) => new(a.x ^ b.x, a.y ^ b.y);
        public static Bool2 operator ^(Bool2 a, bool b) => new(a.x ^ b, a.y ^ b);
        public static Bool2 operator ^(bool a, Bool2 b) => a ^ b;
        public static Bool2 operator !(Bool2 val) => new(!val.x, !val.y);


        public override bool Equals(object obj) => (obj is Bool2 converted && Equals(converted));

        public bool Equals(Bool2 other) => (x == other.x && y == other.y);


        public override string ToString() => $"({x}, {y})";


        public override int GetHashCode() => HashCode.Combine(x, y);


        /// <summary>
        /// (true, true)
        /// </summary>
        public static readonly Bool2 True = new(true);

        /// <summary>
        /// (false, false)
        /// </summary>
        public static readonly Bool2 False = new(false);

        /// <summary>
        /// (true, false)
        /// </summary>
        public static readonly Bool2 TrueX = new(true, false);

        /// <summary>
        /// (false, true)
        /// </summary>
        public static readonly Bool2 TrueY = new(false, true);
    }
}
