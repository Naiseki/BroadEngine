namespace MathKit
{
    public static partial class MathUtil
    {
        /// <summary>
        /// ルート２
        /// </summary>
        public const float Sqrt2 = 1.4142135623f;

        /// <summary>
        /// ルート３
        /// </summary>
        public const float Sqrt3 = 1.73205080757f;

        /// <summary>
        /// ルート５
        /// </summary>
        public const float Sqrt5 = 2.2360679775f;

        /// <summary>
        /// sin30
        /// </summary>
        public const float Sin30 = 1f / 2f;

        /// <summary>
        /// sin45
        /// </summary>
        public const float Sin45 = Sqrt2 / 2f;

        /// <summary>
        /// sin60
        /// </summary>
        public const float Sin60 = Sqrt3 / 2f;

        /// <summary>
        /// cos30
        /// </summary>
        public const float Cos30 = Sqrt3 / 2f;

        /// <summary>
        /// cos45
        /// </summary>
        public const float Cos45 = Sqrt2 / 2f;

        /// <summary>
        /// cos60
        /// </summary>
        public const float Cos60 = 1f / 2f;

        /// <summary>
        /// tan30
        /// </summary>
        public const float Tan30 = Sqrt3;

        /// <summary>
        /// tan45
        /// </summary>
        public const float Tan45 = 1f;

        /// <summary>
        /// tan60
        /// </summary>
        public const float Tan60 = 1f / Sqrt3;
        
        /// <summary>
        /// 円周率
        /// </summary>
        public const float PI = 3.14159265358979f;

        /// <summary>
        /// 自然対数の底
        /// </summary>
        public const float E = 2.71828183f;

        /// <summary>
        /// 1回ターンのラジアン数
        /// </summary>
        public const float Tau = PI * 2f;

        /// <summary>
        /// 円周率の２乗
        /// </summary>
        public const float PISqr = PI * PI;

        /// <summary>
        /// 度数法からラジアンへの変換をする定数
        /// </summary>
        public const float DegToRad = PI / 180f;

        /// <summary>
        /// ラジアンから度数法への変換をする定数
        /// </summary>
        public const float RadToDeg = 180f / PI;

        /// <summary>
        /// ごく僅かな浮動小数点値
        /// </summary>
        public const float Epsilon = 1e-7f;

        /// <summary>
        /// ごく僅かな浮動小数点値の2乗
        /// </summary>
        public const float EpsilonSqr = Epsilon * Epsilon;
    }
}
