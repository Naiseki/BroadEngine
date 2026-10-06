using System;

namespace MathKit
{
    /// <summary>
    /// AABB
    /// </summary>
    public struct Aabb: IEquatable<Aabb>
    {
        /// <summary>
        /// 最小値
        /// </summary>
        public Float2 Min;

        /// <summary>
        /// 最大値
        /// </summary>
        public Float2 Max;


        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        public Aabb(Float2 min, Float2 max)
        {
            Min = min;
            Max = max;
        }


        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="minX">最小値X</param>
        /// <param name="minY">最小値X</param>
        /// <param name="maxX">最大値X</param>
        /// <param name="maxY">最大値Y</param>
        public Aabb(float minX, float minY, float maxX, float maxY): 
            this(new Float2(minX, minY), new Float2(maxX, maxY)) {}


        /// <summary>
        /// 中心座標
        /// </summary>
        public Float2 Center => (Max + Min) * 0.5f;

        /// <summary>
        /// 大きさ
        /// </summary>
        public Float2 Size => Max - Min;

        /// <summary>
        /// 半分の大きさ
        /// </summary>
        public Float2 HalfSize => (Max - Min) * 0.5f;


        /// <summary>
        /// 値を設定
        /// </summary>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        public void Set(Float2 min, Float2 max)
        {
            Min = min;
            Max = max;
        }


        /// <summary>
        /// ２つの<see cref="Aabb"/>が重なっているか
        /// </summary>
        /// <param name="a">AABB1</param>
        /// <param name="b">AABB2</param>
        /// <returns>true: 重なっている　false: 重なっていない</returns>
        public static bool Collide(in Aabb a, in Aabb b)
        {
            return (a.Min.x < b.Max.x && a.Min.y < b.Max.y && b.Min.x < a.Max.x && b.Min.y < a.Max.y);
        }


        /// <summary>
        /// 点を含んでいるか
        /// </summary>
        /// <param name="point">点</param>
        /// <returns>true: 含んでいる　false: 含んでいない</returns>
        public bool Contains(Float2 point)
        {
            return Float2.InRange(point, Min, Max);
        }


        /// <summary>
        /// 平行移動
        /// </summary>
        /// <param name="transletion">移動量</param>
        public void Translate(Float2 transletion)
        {
            Min += transletion;
            Max += transletion;
        }


        /// <summary>
        /// 点に平行移動
        /// </summary>
        /// <param name="center">平行移動後の中心座標</param>
        public void TranslateTo(Float2 center)
        {
            Translate(center - Center);
        }


        /// <summary>
        /// 中心を基準に拡大/縮小
        /// </summary>
        /// <param name="scale">スケール</param>
        public void Scale(Float2 scale)
        {
            Float2 center = Center;
            Float2 halfSize = HalfSize * scale;

            Max = center + halfSize;
            Min = center - halfSize;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"Min:{Min}, Max:{Max}";


        /// <summary>
        /// ハッシュ値を取得
        /// </summary>
        public override int GetHashCode()
        {
           return HashCode.Combine(Min, Max);
        }


        /// <summary>
        /// 等価処理
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Aabb) {
                return Equals((Aabb)obj);
            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="aabb"></param>
        /// <returns></returns>
        public bool Equals(Aabb aabb)
        {
            return (Min == aabb.Min && Max == aabb.Max);
        }


        /// <summary>
        /// Min = (-1, -1) 
        /// Max = (1, 1)
        /// </summary>
        public static readonly Aabb Identity = new Aabb(-Float2.One, Float2.One);

        /// <summary>
        /// Min = (0, 0) 
        /// Max = (0, 0)
        /// </summary>
        public static readonly Aabb Zero = new Aabb(Float2.Zero, Float2.Zero);

        /// <summary>
        /// Min = (-∞, -∞) 
        /// Max = (∞, ∞)
        /// </summary>
        public static readonly Aabb Infinity = new Aabb(new Float2(float.NegativeInfinity), new Float2(float.PositiveInfinity));
    }
}