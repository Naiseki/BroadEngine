using MathKit;

namespace Broad.Graphics
{
    /// <summary>
    /// スプライトを描画するレンダラ
    /// </summary>
    public class SpriteRenderer: RendererBase
    {
        /// <summary>
        /// スプライト
        /// </summary>
        public Sprite Sprite;

        /// <summary>
        /// 描画するテクスチャの番号
        /// </summary>
        public int UnitIndex = 0;

        /// <summary>
        /// アニメーター
        /// </summary>
        public Animator Animator;


        private static Shader shader = Shaders.Get("textureShader");
        private Aabb aabb = new Aabb();


        /// <param name="sprite">スプライト</param>
        /// <returns></returns>
        public SpriteRenderer(Sprite sprite)
        {
            Sprite = sprite;
        }


        /// <param name="spriteName">スプライト名</param>
        public SpriteRenderer(string spriteName):
            this(Assets.Load<Sprite>(spriteName)) {}


        internal override void Render()
        {
            if (!Aabb.Collide(Camera.Aabb, aabb)) {
                return;
            }
            if (Animator != null) UnitIndex = Animator.Index;
            GLProgram.UseShader(shader);
            GLProgram.UseTexture(Sprite.Texture);
            SetColor();
            shader.SetVec4("uvRect", Sprite.GetRegion(UnitIndex));
            shader.SetMatrix2("transform", Actor.Form.ToFloat2x2());
            shader.SetVec2("translation", (Actor.Position + LocalCoord).ToScreenCoord());
            GLProgram.Render();
        }




        internal override void ComputeAabb()
        {
            var halfScale = new Float2(Actor.Form.HalfScale.MaxComponent);
            var worldCood = Actor.Position + LocalCoord;
            aabb.Min = worldCood - halfScale;
            aabb.Max = worldCood + halfScale;
        }


        /// <summary>
        /// リソースを破棄
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            Sprite.Release();
        }
    }
}