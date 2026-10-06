using MathKit;
using Broad.Graphics;

namespace Broad.Physics
{
    /// <summary>
    /// 物理演算に関するクラス
    /// </summary>
    public static class PhysicsUtil
    {
        /// <summary>
        /// ワールド全体の重力
        /// </summary>
        public static Float2 Gravity = Float2.UnitY * 9.8f;

        /// <summary>
        /// 物理エンジンの相対時間
        /// </summary>
        public static float DeltaTime { get; internal set; }


        /// <summary>
        /// 点がコリジョン内にあるかどうか
        /// </summary>
        /// <param name="point">点のワールド座標</param>
        /// <param name="scene">調べるシーン</param>
        /// <returns>点がコリジョン内にあるかどうか</returns>
        public static bool Hit(Float2 point, Scene scene) 
            => scene.Hit(point); 
        

        internal const float MaxDeltaTime = 0.1f;
    }
}