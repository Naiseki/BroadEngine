using System;


namespace MathKit
{
    public struct Ray
    {
        /// <summary>
        /// 始点
        /// </summary>
        public Float2 Point;

        /// <summary>
        /// 方向
        /// </summary>
        public Float2 Direction;

        /// <summary>
        /// 長さ
        /// </summary>
        public float Length;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="point">始点</param>
        /// <param name="direction">方向</param>
        /// <param name="length">レイの長さ</param>
        public Ray(Float2 point, Float2 direction, float length)
        {
            Point = point;
            Direction = direction;
            Length = length;
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(Point, Direction, Length);
        }
    }
}
