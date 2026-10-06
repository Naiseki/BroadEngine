using System;
using MathKit;
using Broad;

namespace Broad
{
    public static class Camera
    {
        public static  Float2 Position = Float2.Zero;

        public static float ViewScale = 1f;

        public static Aabb Region = Aabb.Infinity;

        public static Aabb Aabb => sightAabb;

        public static Float2 TopLeft {
            get => sightAabb.Min;
            set => Position = value + sightAabb.HalfSize;
        }

        public static Float2 BottomRight {
            get => sightAabb.Max;
            set => Position = value - sightAabb.HalfSize;
        }

        static Aabb sightAabb;


        public static void LookAt(Float2 target)
        {
            Position = target;
        }


        public static void Chase(Float2 target, float damping)
        {
            Position += (target - Position) / Math.Min(damping, Time.FrameRate) * Time.DeltaTime;
        }


        public static bool CanSee(in Aabb aabb) => Aabb.Collide(sightAabb, aabb);


        internal static void Update()
        {
            ComputeAabb();
            FollowRegion();
            ComputeAabb();
            AudioUtil.SetListenerPosition(Position);
        }


        static void FollowRegion()
        {
            if ((Region.Min > sightAabb.Min).AnyTrue) 
                TopLeft = Float2.Max(Region.Min, sightAabb.Min);
            if ((Region.Max < sightAabb.Max).AnyTrue) 
                BottomRight = Float2.Min(Region.Max, sightAabb.Max);
        }


        static void ComputeAabb()
        {
            Float2 halfViewScale = new Float2(ViewScale * 0.5f);
            halfViewScale.x *= Window.AspectRatio;
            sightAabb.Set(Position - halfViewScale, Position + halfViewScale);
        }
    }
}