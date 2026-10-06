using System;


namespace MathKit
{
    public struct Circle: IEquatable<Circle>
    {
        /// <summary>
        /// 始点
        /// </summary>
        public Float2 Center;

        /// <summary>
        /// 長さ
        /// </summary>
        public float Radius;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="center">中心座標</param>
        /// <param name="radius">半径</param>
        public Circle(Float2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }


        /// <summary>
        /// 円周の長さ
        /// </summary>
        public float Circumference => Radius * MathUtil.PI * 2f;

        /// <summary>
        /// 面積
        /// </summary>
        public float Area => Radius * Radius * MathUtil.PI; 


        public static bool Collide(Circle a, Circle b)
        {
            float distSqr = (a.Center - b.Center).MagnitudeSqr;
            float r = a.Radius + b.Radius;

            return (distSqr < r * r);
        }


        public bool Equals(Circle other)
        {
            return (Center == other.Center && MathUtil.Nearly(Radius, other.Radius));
        }


        public override bool Equals(object obj) => (obj is Circle other && Equals(other));


        public override int GetHashCode()
        {
            return HashCode.Combine(Center, Radius);
        }
    }
}
