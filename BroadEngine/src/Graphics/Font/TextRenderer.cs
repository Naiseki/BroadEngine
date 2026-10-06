using System;
using MathKit;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.Fonts;

namespace Broad.Graphics
{
    /// <summary>
    /// スプライトを描画するレンダラ
    /// </summary>
    public class TextRenderer: RendererBase
    {
        /// <summary>
        /// レンダリングする文字
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// フォントの大きさ
        /// </summary>
        public int FontSize { get; private set; }

        private Texture texture;

        private Image<Rgba32> image;

        private Aabb aabb = new Aabb();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fontSize"></param>
        public TextRenderer(string text, int fontSize)
        {
            Text = text;
            FontSize = fontSize;
            //TextMeasurer.Measure(text, new RendererOptions());
            image = new Image<Rgba32>(fontSize * text.Length, fontSize);
            SetText(text);
        }

        internal override void Render()
        {
            if (!Aabb.Collide(Camera.Aabb, aabb)) {
                return;
            }
            SetColor();
            GLProgram.UseTexture(texture);
            Shader.SetVec4("uvRect", new Float4(0f, 0f, 1f, 1f));
            Shader.SetMatrix2("transform", Actor.Form.ToFloat2x2());
            Shader.SetVec2("translation", (Actor.Position + LocalCoord).ToScreenCoord());
            GLProgram.Render();
        }

        internal override void ComputeAabb()
        {
            var halfScale = new Float2(Actor.Form.HalfScale.MaxComponent);
            var worldCoord = Actor.Position + LocalCoord;
            aabb.Min = worldCoord - halfScale;
            aabb.Max = worldCoord + halfScale;
        }

        public void SetText(string text)
        {
            var family = Fonts.GetFontFamily("Gen Shin Gothic Monospace Normal");
            image.Mutate(x => x.Resize(FontSize * text.Length, FontSize));
            image.Mutate(x => 
                x.Clear(SixLabors.ImageSharp.Color.Transparent));
            image.Mutate(x => 
                x.DrawText( text, 
                            family.CreateFont(FontSize, FontStyle.Regular), 
                            SixLabors.ImageSharp.Color.White, 
                            PointF.Empty)
            );
            texture = new Texture(image);
        }


        /// <summary>
        /// リソースを破棄
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            texture.Dispose();
        }
    }
}