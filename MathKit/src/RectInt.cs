using System;

namespace MathKit
{
    public struct RectInt: IEquatable<RectInt>
    {
        public Int2 Position;
        public Int2 Size;

        public int Top    => Position.y;
        public int Left   => Position.x;
        public int Right  => Position.x + Size.x;
        public int Bottom => Position.y + Size.y;

        public RectInt(Int2 position, Int2 size)
        {
            Position = position;
            Size = size;
        }

        public RectInt(int x, int y, int w, int h): 
            this(new Int2(x, y), new Int2(w, h)) {}


        public static bool Collide(Rect a, Rect b)
        {
            return (a.Left < b.Right && a.Bottom < b.Top && b.Left < a.Right && b.Bottom < a.Top);
        }

        public override bool Equals(object obj)
        {
            if (obj is RectInt) {
                return Equals((RectInt)obj);
            }
            return false;
        }

        public bool Equals(RectInt rect)
        {
            return (Position == rect.Position && Size == rect.Size);
        }
        
        public override string ToString() => $"x:{Position.x}, y:{Position.y}, width:{Size.x}, height:{Size.y}";

        public override int GetHashCode() => (Position.x, Position.y, Size.x, Size.y).GetHashCode();
    } 
}