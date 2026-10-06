using System;

namespace MathKit
{
    public struct Int2: IEquatable<Int2>
    {
        /// <summary>
        /// x成分
        /// </summary>
        public int x;

        /// <summary>
        /// y成分
        /// </summary>
        public int y;


        /// <param name="x">x成分</param>
        /// <param name="y">y成分</param>
        public Int2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }


        /// <param name="s">成分</param>
        public Int2(int s) => x = y = s;


        /// <param name="v">成分</param>
        /// <returns></returns>
        public Int2(Float2 v): this((int)v.x, (int)v.y) {}


        /// <summary>
        /// 値を設定
        /// </summary>
        /// <param name="newX">新しいx成分</param>
        /// <param name="newY">新しいy成分</param>
        public void Set(int newX, int newY)
        {
            x = newX;
            y = newY;
        }


        /// <summary>
        /// 値を設定
        /// </summary>
        /// <param name="s">新しい成分</param>
        public void Set(int s) => x = y = s;


        public int Added => x + y;

        public int Substructed => x - y;

        public int Multiplied => x * y;

        public int Divided => x / y;

        /// <summary>
        /// (y, x)を生成
        /// </summary>
        public Int2 yx => new Int2(y, x);

        /// <summary>
        /// (x, x)を生成
        /// </summary>
        public Int2 xx => new Int2(x);

        /// <summary>
        /// (y, y)を生成
        /// </summary>
        public Int2 yy => new Int2(y);


        public int this[int index] {
            get => index switch {
            0 => x,
            1 => y,
            _ => throw new IndexOutOfRangeException("Invalid index!"),
        };
            set {
                switch (index) {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    default: throw new IndexOutOfRangeException("Invalid index!");
                }
            }
        }
               


        /// <summary>
        /// Magnitude of this vector
        /// </summary>
        /// <returns>The magnitude</returns>
        public float Magnitude => MathF.Sqrt(MagnitudeSqr);


        /// <summary>
        /// Returns the squared magnitude of this vector
        /// </summary>
        public int MagnitudeSqr => x * x + y * y;


        /// <summary>
        /// Returns the distance between a and b
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>The distance</returns>
        public static float Distance(Int2 a, Int2 b) => MathF.Sqrt((float)DistanceSqr(a, b));


        /// <summary>
        /// Returns the squared distance between a and b
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>The squared distance</returns>
        public static int DistanceSqr(Int2 a, Int2 b) 
        {
            int dx = a.x - b.x;
            int dy = a.y - b.y;
            return dx * dx + dy * dy;
        }


        public bool IsZero => (x == 0 && y == 0); 


        public bool IsUnit => (MagnitudeSqr == 1);


        public bool IsVertical(Int2 a, Int2 b) 
        {
            if (a.IsZero || b.IsZero)
                return false;
            return (Dot(a, b) == 0);
        }


        public bool IsHorizontal(Int2 a, Int2 b)
        {
            if (a.IsZero || b.IsZero)
                return false;
            return (Cross(a, b) == 0);
        }


        //Dot product of two vectors
        public static int Dot(Int2 a, Int2 b) => a.x * b.x + a.y * b.y;


        //Cross product of two vectors
        public static int Cross(Int2 a, Int2 b) => a.x * b.y - b.x * a.y;


        public static float Angle(Int2 from, Int2 to) => MathF.Atan2((float)(to.y - from.y), (float)(to.x - from.x));


        public static bool InRange(in Int2 value, in Int2 min, in Int2 max)
        {
            return (min.x <= value.x && value.x <= max.x &&
                    min.y <= value.y && value.y <= max.y);
        }


        public static Int2 Sign(Int2 value) => new(Math.Sign(value.x), Math.Sign(value.y));


        public float Angle() => MathF.Atan2((float)y, (float)x);


        /// <summary>
        /// 最小の成分を取得
        /// </summary>
        public int MinComponent => Math.Min(x, y);

        /// <summary>
        /// 最大の成分を取得
        /// </summary>
        public int MaxComponent => Math.Max(x, y);


        public static Int2 Min(Int2 a, Int2 b) => new(Math.Min(a.x, b.x), Math.Min(a.y, b.y));
        public static Int2 Max(Int2 a, Int2 b) => new(Math.Max(a.x, b.x), Math.Max(a.y, b.y));

        public static Int2 operator +(Int2 a, Int2 b) => new(a.x + b.x, a.y + b.y);
        public static Int2 operator +(Int2 v, int s)  => new(v.x + s, v.y + s);
        public static Int2 operator -(Int2 a, Int2 b) => new(a.x - b.x, a.y - b.y);
        public static Int2 operator -(Int2 v, int s)  => new(v.x - s, v.y - s);
        public static Int2 operator -(Int2 v)         => new(-v.x, -v.y);
        public static Int2 operator *(Int2 v, int s)  => new(v.x * s, v.y * s);
        public static Int2 operator *(int s, Int2 v)  => new(v.x * s, v.y * s);
        public static Int2 operator /(Int2 v, int s)  => new(v.x / s, v.y / s);
        public static Int2 operator *(Int2 a, Int2 b) => new(a.x * b.x, a.y * b.y);
        public static Int2 operator /(Int2 a, Int2 b) => new(a.x / b.x, a.y / b.y);

        public static bool operator ==(Int2 a, Int2 b)
        {
            return (a.x == b.x && a.y == b.y);
        }
    
        public static bool operator !=(Int2 a, Int2 b) => !(a == b);

        public static Bool2 operator <(in Int2 a, in Int2 b)  => new Bool2(a.x < b.x, a.y < b.y);
        public static Bool2 operator >(in Int2 a, in Int2 b)  => new Bool2(a.x > b.x, a.y > b.y);
        public static Bool2 operator <=(in Int2 a, in Int2 b) => new Bool2(a.x >= b.x, a.y <= b.y);
        public static Bool2 operator >=(in Int2 a, in Int2 b) => new Bool2(a.x >= b.x, a.y >= b.y);

        public static explicit operator Float2(Int2 v) =>new Float2((float)v.x, (float)v.y);

        public override string ToString() => $"({x}, {y})";

        public bool Equals(Int2 v) => this == v;

        public override bool Equals(object obj)
        {
            if (obj is Int2) {
                return this == (Int2)obj;
            }
            return false;
        }

        public override int GetHashCode() 
        {
            return HashCode.Combine(x, y);
        }


        /// <summary>
        /// (0, 0)
        /// </summary>
        public static readonly Int2 Zero = new Int2(0);
        
        /// <summary>
        /// (1, 1)
        /// </summary>
        public static readonly Int2 One = new Int2(1);

        /// <summary>
        /// (1, 0)
        /// </summary>
        public static readonly Int2 UnitX = new Int2(1, 0);

        /// <summary>
        /// (0, 1)
        /// </summary>
        public static readonly Int2 UnitY = new Int2(0, 1);
    }
}
