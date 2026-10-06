using MathKit;

namespace Broad.Physics
{
    /// <summary>
    /// コリジョンイベント情報
    /// </summary>
    public readonly struct CollisionEventArgs 
    {
        /// <summary>
        /// 衝突した剛体
        /// </summary>
        public readonly RigidBody Body;

        /// <summary>
        /// 衝突したアクタ
        /// </summary>
        public readonly Actor Actor;


        /// <summary>
        /// 衝突した点の相対座標
        /// </summary>
        public readonly Float2 RelativeContactCoord;


        internal CollisionEventArgs(RigidBody body, Float2 contactCoord)
        {
            Body = body;
            Actor = body.Actor;
            RelativeContactCoord = contactCoord;
        }


        internal CollisionEventArgs(RigidBody body): 
            this(body, Float2.Zero) {}
    }
}