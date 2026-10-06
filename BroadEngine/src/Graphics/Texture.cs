using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using OpenTK.Graphics.OpenGL4;
using MathKit;

namespace Broad.Graphics
{
    /// <summary>
    /// テクスチャ
    /// </summary>
    public struct Texture: IDisposable
    {
        private bool isDisposed;

        /// <summary>
        /// OpenGLのテクスチャハンドル
        /// </summary>
        public readonly int Handle;

        /// <summary>
        /// テクスチャのサイズ
        /// </summary>
        public readonly Int2 Size;


        internal Texture(Image<Rgba32> image)
        {
            Handle = GL.GenTexture();
            Size = new Int2(image.Width, image.Height);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
            image.Mutate(x => x.Flip(FlipMode.Vertical));
            isDisposed = false;

            var pixels = CreatePixels(image);
        
            Init(pixels.ToArray());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        public Texture(byte[] data, Int2 size)
        {
            Handle = GL.GenTexture();
            Size = size;
            isDisposed = false;
            Init(data);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Texture(Span<byte> data, Int2 size): this(data.ToArray(), size) {}


        /// <summary>
        /// テクスチャを保存
        /// </summary>
        /// <param name="path">ファイルのパス</param>
        public void Save(string path)
        {
            using var img = GetImage();
            img.SaveAsPng(path);
        }


        void Init(Span<byte> pixels)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
            
            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                Size.x,
                Size.y,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                ref pixels[0]
            );

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        }


        Image<Rgba32> GetImage()
        {
            Span<byte> pixels = stackalloc byte[Size.Multiplied * 4];
            GL.BindTexture(TextureTarget.Texture2D, Handle);
            GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ref pixels[0]);
            var img = Image.LoadPixelData<Rgba32>(pixels, Size.x, Size.y);
            img.Mutate((x) => x.Flip(FlipMode.Vertical));
            return img;
        }


        List<byte> CreatePixels(Image<Rgba32> image)
        {
            var pixels = new List<byte>(4 * image.Width * image.Height);

            for (int y = 0; y < image.Height; y++) {
                var row = image.GetPixelRowSpan(y);
                for (int x = 0; x < image.Width; x++) {
                    pixels.Add(row[x].R);
                    pixels.Add(row[x].G);
                    pixels.Add(row[x].B);
                    pixels.Add(row[x].A);
                }
            }
            return pixels;
        }
        
        
        /// <summary>
        /// テクスチャを破棄
        /// </summary>
        public void Dispose()
        {
            if (!isDisposed) {
                GL.DeleteTexture(Handle);
                isDisposed = true;
            }
        }
    }
}