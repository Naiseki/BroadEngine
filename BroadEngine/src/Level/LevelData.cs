using System.Collections.Generic;
using System.Text.Json;

namespace Broad
{
    public readonly struct Level
    {
        public string Name { get; init; }
        public List<ActorData> Actors { get; init; }
    }


    public readonly struct ActorData
    {
        public Vec2 Position { get; init; }
        public Vec2 Scale { get; init; }
        public float Rotation { get; init; }
        public string Name { get; init; }
        public List<ComponentData> Components { get; init; }
    }


    public readonly struct ComponentData
    {
        public string Name { get; init; }
        public Dictionary<string, JsonElement> Properties { get; init; }
    }


    public readonly struct Vec2
    {
        public float X { get; init; }
        public float Y { get; init; }


        public static implicit operator Vec2(MathKit.Float2 float2) => new Vec2 { X = float2.x, Y = float2.y };
        public static implicit operator MathKit.Float2(Vec2 vec2) => new MathKit.Float2(vec2.X, vec2.Y);
    }
}
