using System;

namespace MathKit
{
    /// <summary>
    /// 整数のAABB
    /// </summary>
    public struct AabbInt: IEquatable<AabbInt>
    {
        /// <summary>
        /// 左上座標
        /// </summary>
        public Int2 Min;

        /// <summary>
        /// 右下座標
        /// </summary>
        public Int2 Max;


        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="min">左上座標</param>
        /// <param name="max">右下座標</param>
        public AabbInt(Int2 min, Int2 max)
        {
            Min = min;
            Max = max;
        }


        /// <summary>
        /// 中心座標
        /// </summary>
        public Int2 Center => (Max + Min) / 2;

        /// <summary>
        /// 大きさ
        /// </summary>
        public Int2 Size => Max - Min;

        /// <summary>
        /// 半分の大きさ
        /// </summary>
        public Int2 HalfSize => (Max - Min) / 2;


        /// <summary>
        /// 値を設定
        /// </summary>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        public void Set(Int2 min, Int2 max)
        {
            Min = min;
            Max = max;
        }


        /// <summary>
        /// ２つの<see cref="AabbInt"/>が重なっているか
        /// </summary>
        /// <param name="a">AABB1</param>
        /// <param name="b">AABB2</param>
        /// <returns>true: 重なっている　false: 重なっていない</returns>
        public static bool Collide(in AabbInt a, in AabbInt b)
        {
            return (a.Min.x < b.Max.x && a.Min.y < b.Max.y && b.Min.x < a.Max.x && b.Min.y < a.Max.y);
        }


        /// <summary>
        /// 点を含んでいるか
        /// </summary>
        /// <param name="point">点</param>
        /// <returns>true: 含んでいる　false: 含んでいない</returns>
        public bool Contains(Int2 point)
        {
            return Int2.InRange(point, Min, Max);
        }


        /// <summary>
        /// 平行移動
        /// </summary>
        /// <param name="transletion">移動量</param>
        public void Translate(Int2 transletion)
        {
            Min += transletion;
            Max += transletion;
        }


        /// <summary>
        /// 点に平行移動
        /// </summary>
        /// <param name="center">平行移動後の中心座標</param>
        public void TranslateTo(Int2 center)
        {
            Translate(center - Center);
        }


        /// <summary>
        /// 拡大/縮小
        /// </summary>
        /// <param name="scale">スケール</param>
        public void Scale(Int2 scale)
        {
            Int2 center = Center;
            Int2 halfSize = HalfSize * scale;

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
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is AabbInt) {
                return Equals((AabbInt)obj);
            }
            return false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="aabb"></param>
        /// <returns></returns>
        public bool Equals(AabbInt aabb)
        {
            return (Min == aabb.Min && Max == aabb.Max);
        }


        /// <summary>
        /// Min: (-1,-1) Max: (1,1)
        /// </summary>
        public static readonly AabbInt Identity = new AabbInt(-Int2.One, Int2.One);
    }
}