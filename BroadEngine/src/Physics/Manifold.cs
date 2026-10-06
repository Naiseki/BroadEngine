using MathKit;

namespace Broad.Physics
{
    struct Manifold
    {
        internal Float2 Normal;

        internal Float2 RelativeContactPointA;

        internal Float2 RelativeContactPointB;

        internal Float2 RelativeVelocity;
        
        internal float Depth;
    }
}