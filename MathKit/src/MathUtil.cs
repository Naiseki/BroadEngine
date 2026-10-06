using System;

namespace MathKit
{
    public static partial class MathUtil
    {
        /// <summary>
        /// Convert degrees to radians
        /// </summary>
        /// <param name="degrees">Degrees</param>
        /// <returns>Radians</returns>
        public static float ToRadians(float degrees) => degrees * MathUtil.DegToRad;


        /// <summary>
        /// Convert radians to degrees
        /// </summary>
        /// <param name="radians">Radians</param>
        /// <returns>Degrees</returns>
        public static float ToDegrees(float radians) => radians * MathUtil.RadToDeg;


        public static int Sum(int n) 
        {
            if (n < 0) {
                throw new Exception($"Value {n} must be greater than 0");
            }
            return n * (n + 1) / 2;
        }


        public static float Lerp(float from, float to, float t)
        {
            return from + (to - from) * Clamp01(t);
        }


        public static void Increase(ref float value, float from, float to, float damping)
            => value = Increase(value, from, to, damping);


        public static float Increase(float value, float from, float to, float damping)
        {
            damping = Math.Max(damping, 1);
            float len = to - from;
            float t = (value - from) / len;
            t += 1f / damping;
            return from + len * Math.Min(t, 1f);
        }


        public static void Decrease(ref float value, float from, float to, float damping)
            => value = Decrease(value, from, to, damping);


        public static float Decrease(float value, float from, float to, float damping)
        {
            damping = Math.Max(damping, 1);
            float len = to - from;
            float t = (value - from) / len;
            t -= 1f / damping;
            return from + len * Math.Max(t, 0f);
        }


        public static int FloorToInt(float x)
        {
            return (int)(x - x % 1f + 0.1f);
        }


        /// <summary>
        /// Clamp between 0 and 1
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>Clamped value</returns>
        public static float Clamp01(float value)
        {
            if (value < 0f) return 0f;
            if (value > 1f) return 1f;
            return value;
        }


        /// <summary>
        /// Clamp between the range
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="range">Range</param>
        /// <returns>Clamped value</returns>
        public static float Clamp(float value, Float2 range)
        {
            if (value < range.x) return range.x;
            if (value < range.y) return range.y;
            return value;
        }


        /// Clamp between the range
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="range">Range</param>
        /// <returns>Clamped value</returns>
        public static int Clamp(int value, Int2 range)
        {
            if (value < range.x) return range.x;
            if (value < range.y) return range.y;
            return value;
        }


        public static float Inverse(float x) 
        {
            return (x < Epsilon) ? 0f : 1f / x;
        }


        public static int PositiveMod(int a, int b)
        {
            return (a >= 0) ? a % b : a % b + Math.Abs(b);
        }


        public static float PositiveMod(float a, float b)
        {
            return (a >= 0) ? a % b : a % b + Math.Abs(b);
        }


        public static float Max(float a, float b, float c)
        {
            float max = a;
            if (b > max) max = b;
            if (c > max) max = c;
            return max;
        }


        public static float Min(float a, float b, float c)
        {
            float min = a;
            if (b < min) min = b;
            if (c < min) min = c;
            return min;
        }


        public static float Max(float a, float b, float c, float d)
        {
            float max = a;
            if (b > max) max = b;
            if (c > max) max = c;
            if (d > max) max = d;
            return max;
        }


        public static float Min(float a, float b, float c, float d)
        {
            float min = a;
            if (b < min) min = b;
            if (c < min) min = c;
            if (d < min) min = d;
            return min;
        }


        public static byte Max(byte a, byte b, byte c, byte d)
        {
            byte max = a;
            if (b > max) max = b;
            if (c > max) max = c;
            if (d > max) max = d;
            return max;
        }


        public static byte Min(byte a, byte b, byte c, byte d)
        {
            byte min = a;
            if (b < min) min = b;
            if (c < min) min = c;
            if (d < min) min = d;
            return min;
        }


        /// <summary>
        /// 基準値から近い方の値を取得
        /// </summary>
        /// <param name="baseValue">基準となる値</param>
        /// <param name="a">値１</param>
        /// <param name="b">値２</param>
        /// <returns>近い方の値</returns>
        public static float Nearer(float baseValue, float a, float b)
        {
            float diffA = baseValue - a;
            float diffB = baseValue - b;
            return (diffA * diffA < diffB * diffB) ? a : b;
        }


        /// <summary>
        /// 基準値から近い方の値を取得
        /// </summary>
        /// <param name="baseValue">基準となる値</param>
        /// <param name="a">値１</param>
        /// <param name="b">値２</param>
        /// <returns>近い方の値</returns>
        public static int Nearer(int baseValue, int a, int b)
        {
            int diffA = baseValue - a;
            int diffB = baseValue - b;
            return (diffA * diffA < diffB * diffB) ? a : b;
        }


        /// <summary>
        /// 基準値から近い方の値を取得
        /// </summary>
        /// <param name="baseValue">基準となる値</param>
        /// <param name="a">値１</param>
        /// <param name="b">値２</param>
        /// <returns>近い方の値</returns>
        public static float Farther(float baseValue, float a, float b)
        {
            float diffA = baseValue - a;
            float diffB = baseValue - b;
            return (diffA * diffA > diffB * diffB) ? a : b;
        }


        /// <summary>
        /// 基準値から近い方の値を取得
        /// </summary>
        /// <param name="baseValue">基準となる値</param>
        /// <param name="a">値１</param>
        /// <param name="b">値２</param>
        /// <returns>近い方の値</returns>
        public static int Farther(int baseValue, int a, int b)
        {
            int diffA = baseValue - a;
            int diffB = baseValue - b;
            return (diffA * diffA > diffB * diffB) ? a : b;
        }


        /// <summary>
        /// 浮動小数点の誤差を考慮した等値演算
        /// </summary>
        /// <param name="a">値1</param>
        /// <param name="b">値2</param>
        /// <returns>等しいかどうか</returns>
        public static bool Nearly(float a, float b)
        {
            float diff = a - b;
            return diff * diff < EpsilonSqr;
        }


        /// <summary>
        /// 等加速度運動を計算
        /// </summary>
        /// <param name="start">開始地点</param>
        /// <param name="v0">初速度</param>
        /// <param name="a">加速度</param>
        /// <param name="t">時間</param>
        /// <returns>位置</returns>
        public static float ConstAccel(float start, float v0, float a, float t)
        {
            return start + v0 * t + a * t * t * 0.5f;
        }


        /// <summary>
        /// 区間をリピート
        /// </summary>
        /// <param name="from">開始地点</param>
        /// <param name="to">到着地点</param>
        /// <param name="t">時間</param>
        /// <returns>リピートした値</returns>
        public static float Repeat(float from, float to, float t)
        {
            return from + (to - from) * PositiveMod(t, 1f);
        }


        public static float Bounce(float from, float to, float t)
        {
            t = PositiveMod(t, 2f);
            return Lerp(from, to, 1f - Math.Abs(1f - t));
        }


        /// <summary>
        /// 平方根の逆数
        /// </summary>
        /// <param name="x">実数</param>
        /// <returns>平方根の逆数</returns>
        public unsafe static float InverseSqrt(float x)
        {
            float halfX = x * 0.5f;
            float y = x;
            int i = *(int*)&x;
            i = 0x5f375a86 - (i >> 1);
            y = *(float*)&i;
            y *= 1.5f - halfX * y * y;
            return y;
        }


        /// <summary>
        /// 平方根
        /// </summary>
        /// <param name="x">実数</param>
        /// <returns>平方根</returns>
        public static float Sqrt(float x) => x * InverseSqrt(x);

        public static float LerpAccelerated(float from, float to, float t)
        {
            t = Clamp01(t);
            float k = (to - from) * 12f;
            return from + k * (2 - t) / 12f * t * t * t;
        }

        public static float EaseInOut(float from, float to, float t)
        {
            t = Clamp01(t);
            return from + (-2f * t * t * t + 3f * t * t) * (to - from);
        }
/*
        public static float EaseInOut(float from, float to, float t)
        {
            t = Clamp01(t);
            float half = (from + to) * 0.5f;
            if (t < 0.5f) {
                return EaseOut(from, half, t * 2f);
            }
            return EaseIn(half, to, t * 2f - 1f);
        }*/

        public static float EaseOut(float from, float to, float t)
        {
            t = Clamp01(t);
            return from + (-t * t + 2f * t) * (to - from);
        }

        public static float EaseIn(float from, float to, float t)
        {
            t = Clamp01(t);
            return from + (t * t) * (to - from);
        }
    }
}