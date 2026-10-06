using System;
using MathKit;

namespace Broad.Graphics
{
    public class Tilemap
    {
        int[] mapData;
        public readonly Int2 TileLength;
        public readonly Int2 MaxTileCoord;
        public readonly float TileSize;
        public readonly float HalfTileSize;
        public readonly Float2 TotalSize;


        public Tilemap(int[] data, float tileSize, Int2 tileLength) 
        {
            mapData = data;
            TileLength = tileLength;
            MaxTileCoord = tileLength - Int2.One;
            TileSize = tileSize;
            HalfTileSize = tileSize * 0.5f;
            TotalSize = (Float2)tileLength * tileSize;
        }


        public int this[Int2 point] {
            get {
                if (!Int2.InRange(point, Int2.Zero, MaxTileCoord)) {
                    throw new IndexOutOfRangeException("Invalid index!");
                }
                return mapData[point.y * Width + point.x];
            }
            set {
                if (!Int2.InRange(point, Int2.Zero, MaxTileCoord)) {
                    throw new IndexOutOfRangeException("Invalid index!");
                }
                mapData[point.y * Width + point.x] = value;
            }
        }


        public int this[int x, int y] {
            get => this[new Int2(x, y)];
            set => this[new Int2(x, y)] = value;
        }

        public int Width => TileLength.x;
        public int Height => TileLength.y;


        public AabbInt GetVisibleAabb(Float2 position)
        {
            var localPos = Camera.Aabb.Min - position + HalfTileSize;
            Float2 view = new Float2(Camera.ViewScale / TileSize);
            view.x *= Window.AspectRatio; 
            view += Float2.One;
            Int2 min = Int2.Max((Int2)(localPos / TileSize), Int2.Zero);
            Int2 max = Int2.Min((Int2)view + min, MaxTileCoord);

            return new AabbInt(min, max);
        }


        public AabbInt Clip(Float2 position, Aabb region)
        {
            var localPos = region.Min - position + HalfTileSize;
            Int2 min = Int2.Max((Int2)(localPos / TileSize), Int2.Zero);
            Int2 max = Int2.Min(min + (Int2)Float2.Ceiling(region.Max), MaxTileCoord);
            return new AabbInt(min, max);
        }
    }
}