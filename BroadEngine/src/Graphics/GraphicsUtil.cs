using System;
using OpenTK.Graphics.OpenGL4;

namespace Broad.Graphics
{
    public static class GraphicsUtil
    {
        /// <summary>
        /// グラフィックレイヤー数
        /// </summary>
        public static int LayerCount { get; internal set; }


        public static Texture TakeScreenShot()
        {
            Span<byte> pixels = stackalloc byte[Game.ScreenSize.Multiplied * 4];
            GL.ReadPixels(0, 0, Game.ScreenSize.x, Game.ScreenSize.y, PixelFormat.Rgba, PixelType.UnsignedByte, ref pixels[0]);
            return new Texture(pixels, Game.ScreenSize);
        }
    }
}