using System;

namespace MathKit
{
    public struct Float2: IEquatable<Float2>
    {
        /// <summary>
        /// X成分
        /// </summary>
        public float x;

        /// <summary>
        /// Y成分
        /// </summary>
        public float y;


        public Float2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }


        public Float2(float k) 
        {
            x = y = k;
        }


        public Float2(Int2 v): this(v.x, v.y) {}


        /// <summary>
        /// 値を設定
        /// </summary>
        /// <param name="newX">新しいx成分</param>
        /// <param name="newY">新しいy成分</param>
        public void Set(float newX, float newY)
        {
            x = newX;
            y = newY;
        }


        /// <summary>
        /// 値を設定
        /// </summary>
        /// <param name="s">新しい成分</param>
        public void Set(float s) => x = y = s;


        public float Added => x + y;

        public float Substructed => x - y;

        public float Multiplied => x * y;

        public float Divided => x / y;

        public Float2 yx => new Float2(y, x);

        public Float2 xx => new Float2(x);

        public Float2 yy => new Float2(y);


        public float this[int index] { 
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
        /// Whether this vector is zero vector
        /// </summary>
        /// <returns>true: zero vector false: not</returns>
        public bool IsZero => (MagnitudeSqr < MathUtil.EpsilonSqr);

         
        /// <summary>
        /// Whether this vector is a unit vector
        /// </summary>
        /// <returns>true: unit vector false: not</returns>
        public bool IsUnit {
            get {
                float mag = MagnitudeSqr - 1f;
                return (mag * mag < MathUtil.EpsilonSqr);
            }
        }


        /// <summary>
        /// 一次独立かどうか
        /// </summary>
        /// <param name="a">Aベクトル</param>
        /// <param name="b">Bベクトル</param>
        /// <returns>true: 一次独立 false: 一次独立ではない</returns>
        public bool IsLinearlyIndependent(Float2 a, Float2 b) 
        {
            return (!a.IsZero && !b.IsZero && Cross(a, b) > MathUtil.Epsilon);
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
                    this.Set(value, 0f);
            }
        }

        /// <summary>
        /// Returns the squared magnitude of this vector
        /// </summary>
        public float MagnitudeSqr => x * x + y * y;


        /// <summary>
        /// Normalize this vector
        /// </summary>
        public void Normalize() 
        {
            if (IsUnit) return;
            
            float mag = Magnitude;
            if (mag > 0f) {
                x /= mag;
                y /= mag;
            }
            else {
                this = UnitX;
            }
        }


        /// <summary>
        /// このベクトルに垂直なベクトルを取得
        /// </summary>
        /// <param name="direction">取得するベクトルの向き true: 左向き false: 右向き(数学座標平面上の場合)</param>
        /// <returns>垂直なベクトル</returns>
        public Float2 GetNormal(bool direction) 
        {
            if (direction)
                return new Float2(-y, x);
            else
                return new Float2(y, -x);
        }


        public Float2 Normalized {
            get {
                Float2 v = this;
                v.Normalize();
                return v;
            } 
        }


        public Float2 Inverse => new Float2(MathUtil.Inverse(x), MathUtil.Inverse(y));


        /// <summary>
        /// Returns the distance between a and b
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>The distance</returns>
        public static float Distance(Float2 a, Float2 b) => MathF.Sqrt(DistanceSqr(a, b));


        /// <summary>
        /// Returns the squared distance between a and b
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>The squared distance</returns>
        public static float DistanceSqr(Float2 a, Float2 b) 
        {
            float dx = a.x - b.x;
            float dy = a.y - b.y;
            return dx * dx + dy * dy;
        }


        /// <summary>
        /// Dot product of two vectors
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>Dot product</returns>
        public static float Dot(Float2 a, Float2 b) => a.x * b.x + a.y * b.y;


        /// <summary>
        /// Cross product of two vectors
        /// </summary>
        /// <param name="a">The first vector</param>
        /// <param name="b">The second vector</param>
        /// <returns>Scalar cross product</returns>
        public static float Cross(Float2 a, Float2 b) => a.x * b.y - b.x * a.y;


        /// <summary>
        /// Cross product with a vector and a scalar
        /// </summary>
        /// <param name="v">The vector</param>
        /// <param name="s">The scalar</param>
        /// <returns>Vector cross product</returns>
        public static Float2 Cross(Float2 v, float s) => new Float2(v.y * s, v.x * -s);


        /// <summary>
        /// Cross product with a scalar and a vector
        /// </summary>
        /// <param name="s">The scalar</param>
        /// <param name="v">The vector</param>
        /// <returns>Vector cross product</returns>
        public static Float2 Cross(float s, Float2 v) => new Float2(v.y * -s, v.x * s);


        /// <summary>
        /// Whether two vectors are vertical
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second Vector</param>
        /// <returns>true: vertical false: not</returns>
        public static bool IsVertical(Float2 a, Float2 b) 
        {
            if (a.IsZero || b.IsZero)
                return false;
            float dot = Dot(a, b);
            return (dot * dot < MathUtil.EpsilonSqr);
        }

        
        /// <summary>
        /// Whether two vectors are horizontal
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second Vector</param>
        /// <returns>true: horizontal false: not</returns>
        public static bool IsHorizontal(Float2 a, Float2 b)
        {
            if (a.IsZero || b.IsZero)
                return false;
            float cross = Cross(a, b);
            return (cross * cross < MathUtil.EpsilonSqr);
        }


        /// <summary>
        /// Rotate this vector 
        /// </summary>
        /// <param name="rad">Rotation radian</param>
        public void Rotate(float rad) => this = Rotate(this, rad); 


        /// <summary>
        /// Rotate this vector 
        /// </summary>
        /// <param name="sin">Sine value</param>
        /// <param name="cos">Cosine value</param>
        public void Rotate(float sin, float cos) => this = Rotate(this, sin, cos); 


        /// <summary>
        /// Rotate a vector
        /// </summary>
        /// <param name="v">The vector</param>
        /// <param name="rad">Rotation radian</param>
        /// <returns>The rotated vector</returns>
        public static Float2 Rotate(Float2 v, float rad)
        {
            return Rotate(v, MathF.Sin(rad), MathF.Cos(rad));
        }


        /// <summary>
        /// Rotate a vector
        /// </summary>
        /// <param name="v">The vector</param>
        /// <param name="sin">Sine value</param>
        /// <param name="cos">Cosine value</param>
        /// <returns>The rotated vector</returns>
        public static Float2 Rotate(Float2 v, float sin, float cos)
        {
            return new Float2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
        }


        /// <summary>
        /// Returns the reflection of a vector off a surface that has the specified normal
        /// </summary>
        /// <param name="vector">The vector</param>
        /// <param name="normal">The normal vector</param>
        /// <returns>The reflected vector</returns>
        public static Float2 Reflect(Float2 vector, Float2 normal)
        {
            return vector - 2f * Float2.Dot(vector, normal) * normal;
        }


        public static float Angle(Float2 from, Float2 to) => MathF.Atan2(to.y - from.y, to.x - from.x);


        public float Angle() => MathF.Atan2(y, x);


        //Returns x moved vector without changing direction of the vector
        public static Float2 MoveX(Float2 v, float mx)
        {
            if (v != Zero) {
                v *= (v.x + mx) / v.x;
                return v;
            }
            v.x = mx;
            return v; 
        }


        //Returns y moved vector without changing direction of the vector
        public static Float2 MoveY(Float2 v, float my)
        {
            if (v != Zero) {
                v *= (v.x + my) / v.x;
                return v;
            }

            v.x = my;
            return v; 
        }


        /// <summary>
        /// 等加速度運動を計算
        /// </summary>
        /// <param name="start">開始地点</param>
        /// <param name="v0">初速度</param>
        /// <param name="a">加速度</param>
        /// <param name="t">時間</param>
        /// <returns>位置</returns>
        public static Float2 ConstAccel(Float2 start, Float2 v0, Float2 a, float t)
        {
            return start + v0 * t + a * t * t * 0.5f;
        }


        /// <summary>
        /// 基準点から近いほうを返す
        /// </summary>
        /// <param name="basePoint">基準点</param>
        /// <param name="a">点１</param>
        /// <param name="b">点2</param>
        /// <returns>近いほうの点</returns>
        public static Float2 Nearer(Float2 basePoint, Float2 a, Float2 b)
        {
            return DistanceSqr(a, basePoint) < DistanceSqr(b, basePoint) ? a : b;
        }


        /// <summary>
        /// 基準点から遠いほうを返す
        /// </summary>
        /// <param name="basePoint">基準点</param>
        /// <param name="a">点１</param>
        /// <param name="b">点2</param>
        /// <returns>遠いほうの点</returns>
        public static Float2 Farther(Float2 basePoint, Float2 a, Float2 b)
        {
            return DistanceSqr(a, basePoint) > DistanceSqr(b, basePoint) ? a : b;
        }


        public static Float2 Increase(Float2 value, Float2 from, Float2 to, float damping)
        {
            damping = Math.Max(damping, 1);
            Float2 len = to - from;
            Float2 t = (value - from) / len;
            t += 1f / damping;
            return from + len * Min(t, One);
        }


        public static void Increase(ref Float2 value, Float2 from, Float2 to, float damping)
            => value = Increase(value, from, to, damping);


        public static Float2 Decrease(Float2 value, Float2 from, Float2 to, float damping)
        {
            damping = Math.Max(damping, 1);
            Float2 len = to - from;
            Float2 t = (value - from) / len;
            t -= 1f / damping;
            return from + len * Max(t, One);
        }

        public static void Decrease(ref Float2 value, Float2 from, Float2 to, float damping)
            => value = Decrease(value, from, to, damping);


        public static Float2 Lerp(Float2 from, Float2 to, float t)
        {
            t = MathUtil.Clamp01(t);
            return new Float2(from.x + (to.x - from.x) * t, from.y + (to.y - from.y) * t);
        }


        public static Float2 LerpSqr(Float2 a, Float2 b, float t)
        {
            float t1 = MathUtil.Clamp01(t);
            float t2 = MathUtil.Clamp01(t * t);
            return new Float2(a.x + (b.x - a.x) * t1, a.y + (b.y - a.y) * t2);
        }


        public static Float2 LerpCurved(Float2 from, Float2 to, Float2 initVel, float t)
        {
            var x = to - from;
            t = MathUtil.Clamp01(t);
            return from + initVel * t + (x - initVel) * t * t;
        }


        public static Float2 SmoothAccel(Float2 from, Float2 to, Float2 initVel, float t)
        {
            return LerpCurved(from, to, initVel, MathUtil.EaseInOut(0f, 1f, t));
        }


        /// <summary>
        /// 区間をリピート
        /// </summary>
        /// <param name="from">開始地点</param>
        /// <param name="to">到着地点</param>
        /// <param name="t">時間</param>
        /// <returns>リピートした値</returns>
        public static Float2 Repeat(Float2 from, Float2 to, float t)
        {
            return from + (to - from) * MathUtil.PositiveMod(t, 1f);
        }


        public static Float2 Bounce(Float2 from, Float2 to, float t)
        {
            t = MathUtil.PositiveMod(t, 2f);
            return Lerp(from, to, 1f - Math.Abs(1f - t));
        }


        public static Float2 Clamp(Float2 v, Float2 min, Float2 max)
        {
            return new Float2(
                Math.Clamp(v.x, min.x, max.x),
                Math.Clamp(v.y, min.y, max.y)
            );
        }


        public static Float2 ClampMagnitude(Float2 v, float min, float max)
        {
            if (v.MagnitudeSqr < min * min) {
                v.Magnitude = min;
            }
            else if (v.MagnitudeSqr > max) {
                v.Magnitude = max;
            }
            return v;
        }


        public static bool InRange(in Float2 value, in Float2 min, in Float2 max)
        {
            return (min.x <= value.x && value.x <= max.x &&
                    min.y <= value.y && value.y <= max.y);
        }


        public static Float2 Sign(Float2 value) => new(Math.Sign(value.x), Math.Sign(value.y));


        public float MinComponent => Math.Min(x, y);

        public float MaxComponent => Math.Max(x, y);


        public static Float2 Min(Float2 a, Float2 b) => new(Math.Min(a.x, b.x), Math.Min(a.y, b.y));

        public static Float2 Max(Float2 a, Float2 b) => new(Math.Max(a.x, b.x), Math.Max(a.y, b.y));


        public static Float2 Floor(Float2 v)
        {
            return new Float2(MathF.Floor(v.x), MathF.Floor(v.y));
        }


        public static Float2 Round(Float2 v)
        {
            return new Float2(MathF.Round(v.x), MathF.Round(v.y));
        }


        public static Float2 Ceiling(Float2 v)
        {
            return new Float2(MathF.Ceiling(v.x), MathF.Ceiling(v.y));
        }


        public static Float2 LerpAccelerated(Float2 from, Float2 to, float t)
        {
            t = MathUtil.Clamp01(t);
            Float2 k = (to - from) * 12f;
            return from + k * (2 - t) / 12f * t * t * t;
        }


        /// <summary>
        /// 成分の絶対値を取る
        /// </summary>
        /// <param name="v">ベクトル</param>
        /// <returns>絶対値を取ったベクトル</returns>
        public static Float2 Abs(Float2 v)
        {
            return new Float2(Math.Abs(v.x), Math.Abs(v.y));
        }

        /// <summary>
        /// x座標を平行移動させたベクトルを取得
        /// </summary>
        /// <param name="dx">xの移動量</param>
        /// <returns>xに平行移動したベクトル</returns>
        public Float2 TranslatedX(float dx)
        {
            var res = this;
            res.x += dx;
            return res;
        }

        /// <summary>
        /// y座標を平行移動させたベクトルを取得
        /// </summary>
        /// <param name="dy">yの移動量</param>
        /// <returns>yに平行移動したベクトル</returns>
        public Float2 TranslatedY(float dy)
        {
            var res = this;
            res.y += dy;
            return res;
        }


        public static Float2 operator +(in Float2 a, in Float2 b) => new(a.x + b.x, a.y + b.y);

        public static Float2 operator +(in Float2 v, float s)     => new(v.x + s, v.y + s);

        public static Float2 operator -(in Float2 a, in Float2 b) => new(a.x - b.x, a.y - b.y);

        public static Float2 operator -(in Float2 v, float s)     => new(v.x - s, v.y - s);

        public static Float2 operator -(in Float2 v)              => new(-v.x, -v.y);

        public static Float2 operator *(in Float2 v, float s)     => new(v.x * s, v.y * s);

        public static Float2 operator *(float s, in Float2 v)     => new(v.x * s, v.y * s);

        public static Float2 operator /(in Float2 v, float s)     => new(v.x / s, v.y / s);

        public static Float2 operator /(float s, Float2 v)        => new(s / v.x, s / v.y);

        public static Float2 operator *(in Float2 a, in Float2 b) => new(a.x * b.x, a.y * b.y);

        public static Float2 operator /(in Float2 a, in Float2 b) => new(a.x / b.x, a.y / b.y);

        public static bool operator ==(in Float2 a, in Float2 b)
        {
            return (DistanceSqr(a, b) < MathUtil.EpsilonSqr);
        }
    
        public static bool operator !=(in Float2 a, in Float2 b) => !(a == b);

        public static Bool2 operator <(in Float2 a, in Float2 b)  => new Bool2(a.x < b.x, a.y < b.y);
        public static Bool2 operator >(in Float2 a, in Float2 b)  => new Bool2(a.x > b.x, a.y > b.y);
        public static Bool2 operator <=(in Float2 a, in Float2 b) => new Bool2(a.x >= b.x, a.y <= b.y);
        public static Bool2 operator >=(in Float2 a, in Float2 b) => new Bool2(a.x >= b.x, a.y >= b.y);

        public static explicit operator Int2(Float2 v) => new Int2((int)v.x, (int)v.y);
        public static explicit operator Float3(Float2 v) => new Float3(v.x, v.y, 0f);
        public static implicit operator System.Numerics.Vector2(Float2 v) => new System.Numerics.Vector2(v.x, v.y);
        public static implicit operator Float2(System.Numerics.Vector2 v) => new Float2(v.X, v.Y);

        public override string ToString() => $"({x}, {y})";


        public bool Equals(Float2 v) 
        {
             return (x == v.x && y == v.y);
        }


        public override bool Equals(object obj)
        {
            if (obj is Float2) {
                return Equals((Float2)obj);
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
        public static readonly Float2 Zero = new Float2(0f);

        /// <summary>
        /// (1, 1)
        /// </summary>
        public static readonly Float2 One = new Float2(1f);

        /// <summary>
        /// (1, 0)
        /// </summary>
        public static readonly Float2 UnitX = new Float2(1f, 0f);

        /// <summary>
        /// (0, 1)
        /// </summary>
        public static readonly Float2 UnitY = new Float2(0f, 1f);
    }
}
