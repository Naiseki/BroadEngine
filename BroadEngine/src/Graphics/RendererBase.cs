using System;
using MathKit;

namespace Broad.Graphics
{
    public abstract class RendererBase: Component, IDisposable
    {
        /// <summary>
        /// 描画処理を実行可能かどうか
        /// </summary>
        public bool CanRender => (Actor.Enabled && Enabled && !isDisposed);

        /// <summary>
        /// ローカル座標
        /// </summary>
        public Float2 LocalCoord = Float2.Zero;

        /// <summary>
        /// グラフィックレイヤー
        /// </summary>
        public int GraphicLayer => graphicLayer;

        /// <summary>
        /// カラー
        /// </summary>
        public ColorF Color = ColorF.White;
        
        /// <summary>
        /// カラーの合成モード
        /// </summary>
        public ColorBlendMode Mode = ColorBlendMode.Multiple;


        internal RenderWorld RenderWorld;

        int graphicLayer = 0;

        bool isDisposed = false;

        private protected static Shader Shader = Shaders.Get("textureShader");


        internal abstract void Render();

        internal abstract void ComputeAabb();


        /// <summary>
        /// レイヤーを設定
        /// </summary>
        /// <param name="layer">レイヤー番号</param>
        public void SetLayer(int layer)
        {
            RenderWorld?.ChangeLayer(this, layer);
            graphicLayer = layer;
        }


        private protected void SetColor()
        {
            Float4 col = Color;
            (Float4 mulColor, Float4 addColor) = Mode switch {
                ColorBlendMode.Multiple => (col, Float4.Zero),
                ColorBlendMode.Addaptive => (Float4.One, col),
                ColorBlendMode.Substractive => (Float4.One, -col),
                ColorBlendMode.Override => (Float4.Zero, col),
                _ => throw new System.Exception("No ColorBlendMode!")
            };

            Shader.SetVec4("mulColor", mulColor);
            Shader.SetVec4("addColor", addColor);
        }

        
        protected internal override void OnStart(Actor actor)
        {
            actor.HolderScene.AddRenderer(this, GraphicLayer);
        }


        protected internal override void OnDestroy(Actor actor)
        {
            RenderWorld?.RemoveRenderer(this);
            Dispose();
        }


        protected abstract void Dispose(bool disposing);


        /// <summary>
        /// レンダラを破棄
        /// </summary>
        public void Dispose() 
        {
            if (isDisposed)
                return;
            Dispose(true);
            GC.SuppressFinalize(this);
            isDisposed = true;
        }


        ~RendererBase() => Dispose(false);
    }
}