using MathKit;
using Broad.Graphics;

namespace Broad.Physics
{
    /// <summary>
    /// コリジョンシェイプ
    /// </summary>
    public interface ICollisionShape
    {
        /// <summary>
        /// 有効/無効
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// 剛体
        /// </summary>
        RigidBody Body { get; }

        /// <summary>
        /// コリジョンのタイプ
        /// </summary>
        ShapeType Type { get; }

        /// <summary>
        /// AABBを取得
        /// </summary>
        /// <returns>AABB</returns>
        Aabb GetAabb();

        /// <summary>
        /// AABBを計算
        /// </summary>
        void ComputeAabb();

        /// <summary>
        /// 点がコリジョン内部にあるかどうか
        /// </summary>
        /// <param name="point">ワールド座標</param>
        /// <returns>ヒットしたかどうか</returns>
        bool Hit(Float2 point);
    }



    /// <summary>
    /// 円のコリジョン
    /// </summary>
    public struct CircleShape: ICollisionShape
    {
        /// <summary>
        /// 半径
        /// </summary>
        public float Radius;

        /// <summary>
        /// 有効/無効
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 剛体
        /// </summary>
        /// <value></value>
        public RigidBody Body { get; }

        /// <summary>
        /// 中心座標
        /// </summary>
        public ref readonly Float2 Center => ref Body.Position;

        /// <summary>
        /// 形状
        /// </summary>
        public ShapeType Type => ShapeType.Circle;

        Aabb Aabb;

        internal static Float2[] Vertices = {
            Float2.UnitX,
            Float2.UnitY,
            -Float2.UnitX,
            -Float2.UnitY,
            new Float2(0.5f, MathUtil.Sin60),
            new Float2(-0.5f, MathUtil.Sin60),
            new Float2(MathUtil.Sin45),
            new Float2(-MathUtil.Cos45, MathUtil.Sin45),
            new Float2(-MathUtil.Sin45),
            new Float2(MathUtil.Cos45, -MathUtil.Sin45)
        };


        internal CircleShape(RigidBody body, float radius)
        {
            Radius = radius;
            Body = body;
            Enabled = true;
            Aabb = Aabb.Identity;
        }


        /// <summary>
        /// AABBを計算
        /// </summary>
        public void ComputeAabb()
        {
            var min = new Float2(Center.x - Radius, Center.y - Radius);
            var max = new Float2(Center.x + Radius, Center.y + Radius);
            Aabb.Set(min, max);
        }


        /// <summary>
        /// 点が円の内部にあるかどうか
        /// </summary>
        /// <param name="point">ワールド座標</param>
        /// <returns>ヒットしたコリジョン</returns>
        public bool Hit(Float2 point)
        {
            var d = Center - point;
            return (d.MagnitudeSqr <= Radius * Radius);
        }


        /// <summary>
        /// AABBを取得
        /// </summary>
        /// <returns>AABB</returns>
        public Aabb GetAabb() => Aabb;
    }



    /// <summary>
    /// カプセルのコリジョン
    /// </summary>
    public struct CapsuleShape: ICollisionShape
    {
        /// <summary>
        /// 半分の長さ
        /// </summary>
        public float HalfHeight;

        /// <summary>
        /// 半径
        /// </summary>
        public float Radius;

        /// <summary>
        /// 有効/無効
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 剛体
        /// </summary>
        public RigidBody Body { get; }

        /// <summary>
        /// 形状
        /// </summary>
        public ShapeType Type => ShapeType.Capsule;

        /// <summary>
        /// 中心座標
        /// </summary>
        public ref readonly Float2 Center => ref Body.Position;
        Aabb aabb;

        /// <summary>
        /// 端から端までの半分の長さ
        /// </summary>
        public float HalfLength => HalfHeight + Radius;

        /// <summary>
        /// 端Aの座標
        /// </summary>
        public Float2 EgdeA => Center + Body.Direction * HalfHeight;

        /// <summary>
        /// 端Bの座標
        /// </summary>
        public Float2 EgdeB => Center - Body.Direction * HalfHeight; 


        internal CapsuleShape(RigidBody body, float radius, float halfHeight)
        {
            Radius = radius;
            HalfHeight = halfHeight;
            Body = body;
            Enabled = true;
            aabb = Aabb.Identity;
        }


        /// <summary>
        /// AABBを計算
        /// </summary>
        public void ComputeAabb()
        {
            var min = new Float2(Center.x - HalfLength, Center.y - HalfLength);
            var max = new Float2(Center.x + HalfLength, Center.y + HalfLength);
            aabb.Set(min, max);
        }


        /// <summary>
        /// AABBを取得
        /// </summary>
        public Aabb GetAabb() => aabb;




        /// <summary>
        /// 点がカプセル内部にあるかどうか
        /// </summary>
        /// <param name="point">ワールド座標</param>
        /// <returns>ヒットしたかどうか</returns>
        public bool Hit(Float2 point) 
        {
            return (SegmentDistanceSqr(point) <= Radius * Radius);
        }

        /// <summary>
        /// カプセル内部の線分からの距離の2乗
        /// </summary>
        /// <param name="point">点</param>
        /// <returns>距離の2乗</returns>
        public float SegmentDistanceSqr(Float2 point) => SegmentDistanceSqr(point, out var _);


        /// <summary>
        /// カプセル内部の線分からの距離の2乗
        /// </summary>
        /// <param name="point">点</param>
        /// <param name="normal">法線ベクトル(単位ベクトルとは限らない)</param>
        /// <returns>距離の2乗</returns>
        public float SegmentDistanceSqr(Float2 point, out Float2 normal)
        {
            Float2 v = Body.Direction;
            Float2 toPoint = point - Center;
            float dot = Float2.Dot(v, toPoint);

            float distSqr;
            //　カプセルの右外側にあるとき
            if (dot > HalfHeight) {
                distSqr = Float2.DistanceSqr(EgdeA, point);
                normal = point - EgdeA;
            }
            //　カプセルの左外側にあるとき
            else if (dot < -HalfHeight) {
                distSqr = Float2.DistanceSqr(EgdeB, point);
                normal = point - EgdeB;
            }
            //　カプセルに垂直な位置にあるとき
            else {
                float cross = Float2.Cross(v, toPoint);
                distSqr = cross * cross;
                normal = (cross > 0f) ? v.GetNormal(true) : v.GetNormal(false);
            }
            return distSqr;
        }
    }



    /// <summary>
    /// タイルマップのコリジョン
    /// </summary>
    public struct TileShape: ICollisionShape
    {
        /// <summary>
        /// 有効/無効
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 剛体
        /// </summary>
        public RigidBody Body { get; }

        /// <summary>
        /// タイルサイズ
        /// </summary>
        public float TileSize => map.TileSize;

        /// <summary>
        /// 半分のタイルサイズ
        /// </summary>
        public float HalfTileSize => map.HalfTileSize;

        /// <summary>
        /// 形状
        /// </summary>
        public ShapeType Type => ShapeType.Tilemap;


        Tilemap map;
        Int2 collisionRange;
        Aabb Aabb;


        internal TileShape(RigidBody body, Tilemap map, Int2 collisionRange)
        {
            this.map = map;
            this.collisionRange = collisionRange;
            Enabled = true;
            Body = body;
            Aabb = Aabb.Identity;
        }


        /// <summary>
        /// そのタイルにコリジョンがあるかどうか
        /// </summary>
        public bool this[Int2 point] {
            get {
                int idx = map[point];
                if (collisionRange.x <= idx && idx <= collisionRange.y) {
                    return true;
                }
                return false;
            }
        }


        /// <summary>
        /// そのタイルにコリジョンがあるかどうか
        /// </summary>
        public bool this[int x, int y] => this[new Int2(x, y)];


        /// <summary>
        /// AABBを計算
        /// </summary>
        public void ComputeAabb()
        {
            Aabb.Set(   Body.Position - map.HalfTileSize, 
                        Body.Position - map.HalfTileSize + map.TotalSize 
                    );
        }


        /// <summary>
        /// AABBを取得
        /// </summary>
        /// <returns>AABB</returns>
        public Aabb GetAabb() => Aabb;


        /// <summary>
        /// ワールド座標からその場所のタイルインデックスを取得
        /// </summary>
        /// <param name="point">ワールド座標</param>
        /// <param name="tilePosition">タイル座標</param>
        /// <returns>タイルインデックス</returns>
        public int GetIndex(Float2 point, out Int2 tilePosition)
        {
            var localPos = point - Body.Position + new Float2(map.HalfTileSize);
            var tilePos = (Int2)(localPos / TileSize);

            //もしタイルマップの外だったらint.MaxValueを返す
            if (!Int2.InRange(tilePos, Int2.Zero, map.MaxTileCoord)) {
                tilePosition = Int2.Zero;
                return int.MaxValue;
            }

            tilePosition = tilePos;
            return map[tilePos];
        }


        /// <summary>
        /// ワールド座標からその場所のタイルインデックスを取得
        /// </summary>
        /// <param name="point">ワールド座標</param>
        /// <returns>タイルインデックス</returns>
        public int GetIndex(Float2 point) => GetIndex(point, out var _);



        /// <summary>
        /// 点がコリジョン内部にあるかどうか
        /// </summary>
        /// <param name="point">ワールド座標</param>
        /// <returns>ヒットしたコリジョン</returns>
        public bool Hit(Float2 point) => Hit(point, out var _);



        /// <summary>
        /// 点がコリジョン内部にあるかどうか
        /// </summary>
        /// <param name="point">ワールド座標</param>
        /// <param name="tilePosition">ヒットしたタイル座標</param>
        /// <returns>ヒットしたかどうか</returns>>
        public bool Hit(Float2 point, out Float2 tilePosition)
        {
            var idx = GetIndex(point, out var tilePos);
            if (collisionRange.x <= idx && idx <= collisionRange.y && idx != int.MinValue) {
                tilePosition = (Float2)tilePos * map.TileSize + Body.Position;
                return true;
            }
            tilePosition  = Float2.Zero;
            return false;
        }
    }
}