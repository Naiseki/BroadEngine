using System;
using System.Collections.Generic;
using MathKit;


namespace Broad.Physics
{
    class CollisionWorld: IWorld
    {
        List<RigidBody> circleBodies = new List<RigidBody>();
        List<RigidBody> capsuleBodies = new List<RigidBody>();
        List<RigidBody> tileBodies = new List<RigidBody>();
        CollisionDetector detector = new CollisionDetector();
        bool isDestroyed = false;

        public bool IsDestroyed => isDestroyed;


        internal void Add(RigidBody body)
        {
            switch (body.ShapeType) {
                case ShapeType.Circle: circleBodies.Add(body); break;
                case ShapeType.Capsule: capsuleBodies.Add(body); break;
                case ShapeType.Tilemap: tileBodies.Add(body); break;
            }
        }


        internal void Remove(RigidBody body)
        {
            switch (body.ShapeType) {
                case ShapeType.Circle: circleBodies.Remove(body); break;
                case ShapeType.Capsule: capsuleBodies.Remove(body); break;
                case ShapeType.Tilemap: tileBodies.Remove(body); break;
            }
        }


        void CircleVsCircle()
        {
            for (int i = 0; i < circleBodies.Count; i++) {
                var bodyA = circleBodies[i];
                for (int j = i + 1; j < circleBodies.Count; j++) {
                    var bodyB = circleBodies[j];
                
                    if (bodyA.CanCollide && bodyB.CanCollide && 
                        (bodyA.IsDynamic || bodyB.IsDynamic)) 
                    {
                        //　AABBの衝突テスト
                        if (Aabb.Collide(bodyA.Shape.GetAabb(), bodyB.Shape.GetAabb())) {
                            detector.CollideCircles((CircleShape)bodyA.Shape, (CircleShape)bodyB.Shape);
                        }
                    }
                }
            }
        }


        void CircleVsCapsule()
        {
            for (int i = 0; i < circleBodies.Count; i++) {
                var cirBody = circleBodies[i];
                for (int j = 0; j < capsuleBodies.Count; j++) {
                    var capBody = capsuleBodies[j];

                    if (cirBody.CanCollide && capBody.CanCollide && 
                        (cirBody.IsDynamic || capBody.IsDynamic)) 
                    {
                        //　AABBの衝突テスト
                        if (Aabb.Collide(cirBody.Shape.GetAabb(), capBody.Shape.GetAabb())) {
                            detector.CollideCircleVsCapsule((CircleShape)cirBody.Shape, (CapsuleShape)capBody.Shape);
                        }
                    }
                }
            }
        }


        void CircleVsTile()
        {
            for (int i = 0; i < circleBodies.Count; i++) {
                var cirBody = circleBodies[i];
                for (int j = 0; j < tileBodies.Count; j++) {
                    var tileBody = tileBodies[j];

                    if (cirBody.CanCollide && tileBody.CanCollide && 
                        cirBody.IsDynamic) 
                    {
                        //　AABBの衝突テスト
                        if (Aabb.Collide(cirBody.Shape.GetAabb(), tileBody.Shape.GetAabb())) {
                            detector.CollideCircleVsTile((CircleShape)cirBody.Shape, (TileShape)tileBody.Shape);
                        }
                    }
                }
            }
        }


        void ComputeAabb()
        {
            for (int i = 0; i < circleBodies.Count; i++) {
                var body = circleBodies[i];
                if (body.CanUpdate) {
                    body.Shape.ComputeAabb();
                }
            }
            for (int i = 0; i < capsuleBodies.Count; i++) {
                var body = capsuleBodies[i];
                if (body.CanUpdate) {
                    body.Shape.ComputeAabb();
                }
            }
        }


        public void Step()
        {
            if (isDestroyed)
                return;
            ComputeAabb();
            CircleVsTile();
            CircleVsCapsule();
            CircleVsCircle();
        }


        public void Destroy() 
        {
            isDestroyed = true;
            circleBodies.Clear();
            tileBodies.Clear();
            capsuleBodies.Clear();
        }


        public bool Hit(Float2 point)
        {
            for (int i = 0; i < circleBodies.Count; i++) {
                var body = circleBodies[i];

                //　無効またはAABB外の時は判定しない
                if (body.IgnoreHitTest || !body.Shape.GetAabb().Contains(point)) { 
                    continue;
                }
                
                if (body.Shape.Hit(point)) {
                    return true;
                }
            }
            for (int i = 0; i < capsuleBodies.Count; i++) {
                var body = capsuleBodies[i];

                //　無効またはAABB外の時は判定しない
                if (body.IgnoreHitTest || !body.Shape.GetAabb().Contains(point)) { 
                    continue;
                }
                
                if (body.Shape.Hit(point)) {
                    return true;
                }
            }

            for (int i = 0; i < tileBodies.Count; i++) {
                var body = tileBodies[i];
                if (!body.IgnoreHitTest && tileBodies[i].Shape.Hit(point)) {
                    return true;
                }
            }

            return false;
        }
    }
}