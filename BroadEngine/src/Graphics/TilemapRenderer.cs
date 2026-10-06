using System.Runtime.InteropServices.ComTypes;
using System;
using MathKit;

namespace Broad.Graphics
{
    public class TilemapRenderer: RendererBase
    {
        public Sprite Sprite;

        public Tilemap Tilemap;

        public TileAnimator Animator;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tilemap">タイルマップ</param>
        /// <param name="sprite">スプライト</param>
        public TilemapRenderer(Tilemap tilemap, Sprite sprite)
        {
            Sprite = sprite;
            Tilemap = tilemap;
        }


        internal override void Render()
        {
            GLProgram.UseShader(Shader);
            SetColor();
            Shader.SetMatrix2("transform", Actor.Form.ToFloat2x2());
            var coord = Actor.Position + LocalCoord;
            var t = Tilemap.GetVisibleAabb(Actor.Position);

            for (int y = t.Min.y; y <= t.Max.y; y++) {
                for (int x = t.Min.x; x <= t.Max.x; x++) {
                    DrawTile(x, y, coord);
                }
            }
        }


        void DrawTile(int tx, int ty, Float2 coord)
        {
            int idx = Tilemap[tx, ty];
            if (idx < 0)
                return;
            
            GLProgram.UseTexture(Sprite.Texture);
            var pos = new Float2(tx * Tilemap.TileSize, ty * Tilemap.TileSize);
            Shader.SetVec2("translation", (coord + pos).ToScreenCoord());
            Shader.SetVec4("uvRect", Sprite.GetRegion(Animator?.Index(idx) ?? idx));
            GLProgram.Render();
        }


        internal override void ComputeAabb() {}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            Sprite.Release();
        }
    }
}