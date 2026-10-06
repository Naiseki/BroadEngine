using System.Net.Mail;
using System;
using MathKit;

namespace Broad.Physics
{
    class CollisionDetector
    {
        CollisionSolver solver = new CollisionSolver();


        internal void CollideCircles(in CircleShape a, in CircleShape b)
        {
            var d = a.Center - b.Center;
            var r = a.Radius + b.Radius;

            //No collision
            if (d.MagnitudeSqr > r * r) {
                return;
            }


            var manifold = new Manifold();
            if (IsCollidable(a.Body, b.Body)) {
                float mag = d.Magnitude;            
                manifold.Depth = r - mag;
                manifold.Normal = (mag > MathUtil.Epsilon) ? d / mag : Float2.UnitX;
                manifold.RelativeContactPointA =  a.Radius * manifold.Normal;
                manifold.RelativeContactPointB = -b.Radius * manifold.Normal;
                manifold.RelativeVelocity = a.Body.VelocityAt(manifold.RelativeContactPointA) -
                                            b.Body.VelocityAt(manifold.RelativeContactPointB);
                solver.SolveCircles(a.Body, b.Body, manifold);
            }

            a.Body.Actor.OnCollide(new CollisionEventArgs(b.Body, manifold.RelativeContactPointA));
            b.Body.Actor.OnCollide(new CollisionEventArgs(a.Body, manifold.RelativeContactPointB));
        }


        internal void CollideCircleVsCapsule(in CircleShape cir, in CapsuleShape cap)
        {
            float distSqr = cap.SegmentDistanceSqr(cir.Center, out var normal);
            float r = cir.Radius + cap.Radius;
            if (distSqr > r * r) {
                return;
            }

            var manifold = new Manifold();
            if (IsCollidable(cir.Body, cap.Body)) {
                normal.Normalize();
                manifold.Normal = normal;
                manifold.Depth = r - MathF.Sqrt(distSqr);
                manifold.RelativeContactPointA = normal * cir.Radius;
                manifold.RelativeContactPointB = cir.Center + manifold.RelativeContactPointA - cap.Center;
                manifold.RelativeVelocity = cir.Body.VelocityAt(manifold.RelativeContactPointA) -
                                            cap.Body.VelocityAt(manifold.RelativeContactPointB);
                solver.SolveCircleVsCapsule(cir.Body, cap.Body, manifold);
            }

            cir.Body.Actor.OnCollide(new CollisionEventArgs(cap.Body, manifold.RelativeContactPointA));
            cap.Body.Actor.OnCollide(new CollisionEventArgs(cir.Body, manifold.RelativeContactPointB));
        }


        internal void CollideCircleVsTile(in CircleShape cir, in TileShape tile)
        {
            for (int i = 0; i < CircleShape.Vertices.Length; i++) {
                if (tile.Hit(cir.Center + CircleShape.Vertices[i] * cir.Radius, out var tilePos)) 
                {
                    var manifold = new Manifold();
                    var d = tilePos - cir.Center;

                    manifold.Normal = CircleShape.Vertices[i];
                    manifold.RelativeContactPointA = manifold.Normal * cir.Radius;
                    manifold.RelativeVelocity = cir.Body.VelocityAt(manifold.RelativeContactPointA);
                    manifold.Depth = cir.Radius + tile.HalfTileSize;
                    if (d.x * d.x < d.y * d.y) {
                        manifold.Depth -= Math.Abs(d.y);
                    }
                    else {
                        manifold.Depth -= Math.Abs(d.x);
                    }

                    if (IsCollidable(cir.Body, tile.Body)) {
                        solver.SolveCircleVsTile(cir.Body, tile.Body, manifold);
                    }

                    cir.Body.Actor.OnCollide(new CollisionEventArgs(tile.Body, manifold.RelativeContactPointA));
                    tile.Body.Actor.OnCollide(new CollisionEventArgs(cir.Body, manifold.RelativeContactPointB));
                }
            }
        }


        bool IsCollidable(RigidBody a, RigidBody b)
        {
            return !a.IsSensor && !b.IsSensor && 
                    ((a.MaskBits & b.CategoryBits) * (a.CategoryBits & b.MaskBits) != 0);
        }
    }



    class CollisionSolver
    {
        internal void SolveCircles(RigidBody a, RigidBody b, in Manifold manifold)
        {   
            float velAlongNormal = Float2.Dot(manifold.RelativeVelocity, manifold.Normal);

            if (velAlongNormal > 0f) 
                return;
          
            // Caclculate impulse
            float mag = CalcMagnitude(a, b, velAlongNormal, manifold, out float m);
            var impulse = mag * manifold.Normal;
            a.ApplyImpulseTo(impulse, manifold.RelativeContactPointA);
            b.ApplyImpulseTo(-impulse, manifold.RelativeContactPointB);



            // Apply friction impulse
            var tangent = manifold.RelativeVelocity - velAlongNormal * manifold.Normal;
            tangent.Normalize();
            var frictionImpulse = CalcFrictionImpulse(a, b, mag, tangent);
            a.ApplyImpulseTo(frictionImpulse, manifold.RelativeContactPointA);
            b.ApplyImpulseTo(-frictionImpulse, manifold.RelativeContactPointB);


            //a.Position += manifold.Depth * 0.5f * Math.Sign(a.Mass) * manifold.Normal;
            //b.Position -= manifold.Depth * 0.5f * Math.Sign(b.Mass) * manifold.Normal;
            var penetVec = manifold.Depth * manifold.Normal;
            a.ResolvePosition(penetVec);
            b.ResolvePosition(-penetVec);
        }


        internal void SolveCircleVsCapsule(RigidBody cir, RigidBody cap, in Manifold manifold)
        {         
            float velAlongNormal = Float2.Dot(manifold.RelativeVelocity, manifold.Normal);
                
            if (velAlongNormal > 0f) 
                return;

            // Caclculate impulse
            float mag = CalcMagnitude(cir, cap, velAlongNormal, manifold, out float m);
            var impulse = mag * manifold.Normal;

            cir.ApplyImpulseTo(impulse, manifold.RelativeContactPointA);
            cap.ApplyImpulseTo(-impulse, manifold.RelativeContactPointB);


            // Apply friction impulse
            var tangent = manifold.RelativeVelocity - velAlongNormal * manifold.Normal;
            tangent.Normalize();
            var frictionImpulse = CalcFrictionImpulse(cir, cap, mag, tangent);
            cir.ApplyImpulseTo(-frictionImpulse, manifold.RelativeContactPointA);
            cap.ApplyImpulseTo(frictionImpulse, manifold.RelativeContactPointB);

            
            //Solve position
            //cir.Position += manifold.Depth * Math.Sign(cir.Mass) * manifold.Normal;
            var penetVec = manifold.Depth * manifold.Normal;
            cir.ResolvePosition(penetVec);
        }


        internal void SolveCircleVsTile(RigidBody cir, RigidBody tile, in Manifold manifold) 
        {
            // Apply impulse
            float e = MixRestution(cir.Restitution, tile.Restitution);
            //float m = MathUtil.Inverse(cir.InvMass);
            float velAlongNormal = Float2.Dot(cir.Velocity, manifold.Normal);
            //float j = -(e + 1f) * velAlongNormal * m;
            //var impulse = j * manifold.Normal;
            //var relativeContactPoint = manifold.Normal * cir.Shape.Radius;
            var j = CalcMagnitude(cir, tile, velAlongNormal, manifold, out float m);
            cir.ApplyImpulseTo(j * manifold.Normal, manifold.RelativeContactPointA);


            // Apply friction impulse
            var tangent = cir.Velocity - velAlongNormal * manifold.Normal;
            tangent.Normalize();
            Float2 frictionImpulse = CalcFrictionImpulse(cir, tile, j, tangent);
            cir.ApplyImpulseTo(frictionImpulse, manifold.RelativeContactPointA);


            //Solve position
            //cir.Position -= manifold.Depth * Math.Sign(cir.Mass) * manifold.Normal;
            var penetVec = -manifold.Depth * manifold.Normal;
            cir.ResolvePosition(penetVec);
            //cir.ApplyImpulse(manifold.Depth * -resolveScale * manifold.Normal);
        }


        float MixRestution(float a, float b) => a * b;
        float MixFriction(float a, float b) => a * b;


        float CalcMagnitude(RigidBody a, RigidBody b, float velAlongNormal, in Manifold manifold, out float invMassSum) 
        {
            float e = MixRestution(a.Restitution, b.Restitution);
            float r1 = Float2.Cross(manifold.RelativeContactPointA, manifold.Normal);
            float r2 = Float2.Cross(manifold.RelativeContactPointB, manifold.Normal);
            float i = r1 * r1 * a.InvInertia + r2 * r2 * b.InvInertia;
            float m = Float2.Dot((a.InvMass + b.InvMass) * manifold.Normal, manifold.Normal);
            invMassSum =  MathUtil.Inverse(m + i);
            return -(e + 1f) * velAlongNormal * invMassSum;
        }


        Float2 CalcFrictionImpulse(RigidBody a, RigidBody b, float j, Float2 tangent)
        {
            float dynFriction = MixFriction(a.Friction, b.Friction);
            return j * dynFriction * tangent;
        }
    }
}