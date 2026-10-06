using System;

namespace MathKit
{
    public struct Rect: IEquatable<Rect>
    {
        public Float2 Position;
        public Float2 Size;

        public float Top    => Position.y;
        public float Left   => Position.x;
        public float Right  => Position.x + Size.x;
        public float Bottom => Position.y + Size.y;

        public Rect(Float2 position, Float2 size)
        {
            Position = position;
            Size = size;
        }

        public Rect(float x, float y, float w, float h): 
            this(new Float2(x, y), new Float2(w, h)) {}


        public static bool Collide(in Rect a, in Rect b)
        {
            return (a.Left < b.Right && a.Bottom > b.Top && b.Left < a.Right && b.Bottom > a.Top);
        }

        public override bool Equals(object obj)
        {
            if (obj is Rect) {
                return Equals((Rect)obj);
            }
            return false;
        }

        public bool Equals(Rect rect)
        {
            return (Position == rect.Position && Size == rect.Size);
        }
        
        
        public override int GetHashCode() => (Position.x, Position.y, Size.x, Size.y).GetHashCode();
    } 
}