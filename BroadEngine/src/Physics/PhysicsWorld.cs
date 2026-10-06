using System;
using MathKit;
using System.Collections.Generic;


namespace Broad.Physics
{
    class PhysicsWorld: IWorld
    {
        CollisionWorld colWorld = new CollisionWorld();
        List<RigidBody> dynBodies = new List<RigidBody>();
        List<RigidBody> staBodies = new List<RigidBody>();
        bool isDestroyed = false;

        public bool IsDestroyed => isDestroyed;


        internal void AddBody(RigidBody body)
        {
            colWorld.Add(body);
            if (body.IsDynamic) dynBodies.Add(body);
            else                staBodies.Add(body);
        }


        internal void RemoveBody(RigidBody body)
        {
            colWorld.Remove(body);
            if (body.IsDynamic) dynBodies.Remove(body);
            else                staBodies.Remove(body);
        }


        void BodyQuery()
        {
            for (int i = 0; i < dynBodies.Count; i++) {
                dynBodies[i].Update();
            }
        }


        public void Step() 
        {
            if (isDestroyed)
                return;
            BodyQuery();
            colWorld.Step();
        }


        public void Destroy()
        {
            isDestroyed = true;
            colWorld.Destroy();
        }


        public bool Hit(Float2 point) => colWorld.Hit(point);
    }
}