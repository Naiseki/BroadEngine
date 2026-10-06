using Broad.Graphics;
using MathKit;
using OpenTK.Mathematics;

namespace Broad
{
    public static class Float2Extentions
    {
        public static Float2 ToScreenCoord(in this Float2 worldCoord)
        {
            Float2 viewCoord = worldCoord - Camera.Position;
            float viewScale = Camera.ViewScale * 0.5f;
            return new Float2(
                 viewCoord.x / (viewScale * Window.AspectRatio),
                -viewCoord.y / viewScale
            );
        }


        public static Float2 ToScreenScale(in this Float2 scale)
        {
            return scale / Camera.ViewScale;
        }
    }



    public static class Float2x2Extentions
    {
        public static Matrix2 ToMatrix2(in this Float2x2 matrix)
        {
            return new Matrix2(
                    matrix.C0.x, matrix.C1.x,
                    matrix.C0.y, matrix.C1.y
            );
        }
    }
}
