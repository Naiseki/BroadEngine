using System;
using MathKit;
using Broad.Graphics;


namespace Broad.Physics
{
    /// <summary>
    /// 剛体
    /// </summary>
    public class RigidBody: Component 
    {
        /// <summary>
        /// 動的かどうか
        /// </summary>
        public readonly bool IsDynamic;

        /// <summary>
        /// 線速度
        /// </summary>
        public Float2 Velocity = Float2.Zero;   

        /// <summary>
        /// 角速度
        /// </summary>   
        public float AngularVelocity = 0f;      

        /// <summary>
        /// 重力スケール
        /// </summary>    
        public float GravityScale = 1f;

        /// <summary>
        /// 重力を適応するかどうか
        /// </summary>    
        public bool UseGravity = true;

        /// <summary>
        /// 抵抗力
        /// </summary>
        public float LiniearDamping = 0.1f;

        /// <summary>
        /// 回転抵抗力
        /// </summary>
        public float AngularDamping = 0.1f;

        /// <summary>
        /// 動摩擦力
        /// </summary>
        public float Friction = 0.5f;

        /// <summary>
        ///弾性力
        /// </summary>
        public float Restitution = 0.5f;

        /// <summary>
        /// コリジョンがセンサーかどうか
        /// </summary>
        public bool IsSensor = false;

        /// <summary>
        /// 点の検索処理を無視するかどうか
        /// </summary>
        public bool IgnoreHitTest = false;

        /// <summary>
        /// コリジョンシェイプ
        /// </summary>
        public ICollisionShape Shape;

        /// <summary>
        /// コリジョンカテゴリ
        /// </summary>
        public int CategoryBits = 0b000001;

        /// <summary>
        /// コリジョンマスク
        /// </summary>
        public int MaskBits = 0b000001;


        float mass = 1f;
        float inertia = 1f;
        internal float InvMass = 1f;               
        internal float InvInertia = 1f;    
        

        /// <summary>
        /// 質量
        /// </summary>
        public float Mass {
            get => mass;
            set {
                if (!IsDynamic) return;
                mass = value;
                InvMass = MathUtil.Inverse(value); 
            }
        }
        
        /// <summary>
        /// イナーシャ
        /// </summary>
        public float Inertia {
            get => inertia;
            set {
                if (!IsDynamic) return;
                inertia = value;
                InvInertia = MathUtil.Inverse(value); 
            }
        }

        /// <summary>
        /// コリジョンの形状
        /// </summary>
        public ShapeType ShapeType => Shape.Type;

        /// <summary>
        /// 方向
        /// </summary>
        public Float2 Direction => Actor.Form.Direction;

        /// <summary>
        /// 更新処理が可能かどうか
        /// </summary>
        public bool CanUpdate => (IsDynamic && IsActive);

        /// <summary>
        /// 衝突判定が可能かどうか
        /// </summary>
        public bool CanCollide => (IsActive && Shape.Enabled);


        internal ref Float2 Position => ref Actor.Form.Position;

        internal ref float Rotation =>  ref Actor.Form.Rotation;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isDynamic"></param>
        protected RigidBody(bool isDynamic)
        {
            if (!isDynamic) {
                SetStatic();
            }
            IsDynamic = isDynamic;
        }


        void SetStatic()
        {
            mass = 0f;
            inertia = 0f;
            InvMass = 0f;
            InvInertia = 0f;
        }


        /// <summary>
        /// 円のコリジョンを持ったリジッドボディを作成
        /// </summary>
        /// <param name="isDynamic">動的かどうか</param>
        /// <param name="radius">円の半径</param>
        /// <returns>作成したリジッドボディ</returns>
        public static RigidBody CreateCircle(bool isDynamic, float radius)
        {
            var rb = new RigidBody(isDynamic);
            rb.Shape = new CircleShape(rb, radius);
            return rb;
        }


        /// <summary>
        /// カプセルのコリジョンを持ったリジッドボディを作成
        /// </summary>
        /// <param name="isDynamic">動的かどうか</param>
        /// <param name="radius">カプセルの半径</param>
        /// <param name="halfHeight">カプセルの長さ</param>
        /// <returns>作成したリジッドボディ</returns>
        public static RigidBody CreateCapsule(bool isDynamic, float radius, float halfHeight)
        {
            var rb = new RigidBody(isDynamic);
            rb.Shape = new CapsuleShape(rb, radius, halfHeight);
            return rb;
        }


        /// <summary>
        /// タイルマップのコリジョンを持ったリジッドボディ
        /// </summary>
        /// <param name="tilemap">タイルマップ</param>
        /// <param name="collisionRange">コリジョンの範囲</param>
        /// <returns>作成したリジッドボディ</returns>
        public static RigidBody CreateTilemap(Tilemap tilemap, Int2 collisionRange)
        {
            var rb = new RigidBody(false);
            rb.Shape = new TileShape(rb, tilemap, collisionRange);
            return rb;
        }


        /// <summary>
        /// 力を加える
        /// </summary>
        /// <param name="force">加える力</param>
        public void ApplyForce(Float2 force)
        {
            Velocity += force * InvMass * PhysicsUtil.DeltaTime;
        }

        /// <summary>
        /// 力を加える
        /// </summary>
        /// <param name="x">x軸方向に加える力</param>
        /// <param name="y">y軸方向に加える力</param>
        public void ApplyForce(float x, float y) => ApplyForce(new Float2(x, y));


        /// <summary>
        /// ある点に力を加える
        /// </summary>
        /// <param name="force">力</param>
        /// <param name="point">加える点の相対座標</param>
        public void ApplyForceTo(Float2 force, Float2 point)
        {
            Velocity += force * InvMass * PhysicsUtil.DeltaTime;
            AngularVelocity += Float2.Cross(force, point) * InvInertia * PhysicsUtil.DeltaTime;
        }


        /// <summary>
        /// 力積を加える
        /// </summary>
        /// <param name="impulse">力積</param>
        public void ApplyImpulse(Float2 impulse)
        {
            Velocity += impulse * InvMass;
        }

        /// <summary>
        /// 力積を加える
        /// </summary>
        /// <param name="x">x軸方向に加える力積</param>
        /// <param name="y">y軸方向に加える力積</param>
        public void ApplyImpulse(float x, float y) => ApplyImpulse(new Float2(x, y));


        /// <summary>
        /// ある点に力積を加える
        /// </summary>
        /// <param name="impulse">力積</param>
        /// <param name="point">加える点の相対座標</param>
        public void ApplyImpulseTo(Float2 impulse, Float2 point)
        {
            Velocity += impulse * InvMass;
            AngularVelocity += Float2.Cross(impulse, point) * InvInertia;
        }


        /// <summary>
        /// トルクを加える
        /// </summary>
        /// <param name="torque">加えるトルク</param>
        public void ApplyTorque(float torque)
        {
            AngularVelocity += torque * InvInertia * PhysicsUtil.DeltaTime;
        }


        /// <summary>
        /// 回転の力積を加える
        /// </summary>
        /// <param name="impulse">力積</param>
        public void ApplyAngularImpulse(float impulse)
        {
            AngularVelocity += impulse * InvInertia;
        }


        /// <summary>
        /// 移動させる
        /// </summary>
        /// <param name="to">移動先の座標</param>
        public void MoveTo(Float2 to)
        {
            var v = to - Position;
            Velocity += v / PhysicsUtil.DeltaTime;
        }


        /// <summary>
        /// ある地点においての速度を取得
        /// </summary>
        /// <param name="relativePoint">点の相対座標</param>
        /// <returns>速度</returns>
        public Float2 VelocityAt(Float2 relativePoint)
        {
            return Velocity + Float2.Cross(relativePoint, AngularVelocity);
        }


        internal void Update()
        {
            if (!IsActive)
                return;
            IntegrateVelocity();
            IntegratePosition();
        }


        void IntegratePosition()
        {
            Actor.Form.Position += Velocity * PhysicsUtil.DeltaTime;
            Actor.Form.Rotation += AngularVelocity * PhysicsUtil.DeltaTime;
        }


        void IntegrateVelocity() 
        {
            if (UseGravity) ApplyGravity();
            ApplyDamping(); 
        }


        internal void ResolvePosition(Float2 positionImpulse)
        {
            if (mass * mass < MathUtil.EpsilonSqr)
                return;
            Actor.Position += positionImpulse;
        }


        void ApplyDamping()
        {
            Velocity /= 1f + LiniearDamping * PhysicsUtil.DeltaTime; 
            AngularVelocity /= 1f + AngularDamping  * PhysicsUtil.DeltaTime; 
        }


        void ApplyGravity()
        {
            if (mass < MathUtil.Epsilon)
                return;
            Velocity += GravityScale * PhysicsUtil.Gravity * PhysicsUtil.DeltaTime;
        }


        /// <summary>
        /// </summary>
        /// <param name="actor"></param>
        protected internal override void OnStart(Actor actor)
        {
            actor.HolderScene.AddRigidBody(this);
            Shape.ComputeAabb();
        }


        /// <summary>
        /// </summary>
        /// <param name="actor"></param>
        protected internal override void OnDestroy(Actor actor)
        {
            actor.HolderScene.RemoveRigidBody(this);
        }
    }
}