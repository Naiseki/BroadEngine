using System.Collections.Generic;

namespace Broad
{
    /// <summary>
    /// アニメーションを行うクラス
    /// </summary>
    public class TileAnimator: Component
    {
        Dictionary<int, TileAnimation> anims = new Dictionary<int, TileAnimation>();
        bool playing = false;


        /// <summary>
        /// スプライトのタイルインデクス
        /// </summary>
        public int Index(int tileIndex) => anims[tileIndex].Counter.Value;


        /// <summary>
        /// 再生中かどうか
        /// </summary>
        public bool IsPlaying => playing;


        /// <summary>
        /// 再生するアニメーションを決定
        /// </summary>
        public void Play() => playing = true;


        /// <summary>
        /// アニメーションを停止
        /// </summary>
        public void Stop() => playing = false;


        /// <summary>
        /// アニメーションを追加
        /// </summary>
        /// <param name="tileIndex">タイル番号</param>
        /// <param name="data">アニメーションデータの配列</param>
        /// <param name="frequency">アニメーションの更新頻度</param>
        public void AddAnimation(int tileIndex, int[] data, int frequency) 
        {
            anims.Add(tileIndex, new TileAnimation {
                Data = data,
                Frequency = frequency,
                Counter = new LoopCounter(0, data.Length - 1)
            });
        }


        protected internal override void OnUpdate(Actor actor)
        {   
            if (playing) {
                Animate();
            }
        }


        void Animate()
        { 
            foreach (var a in anims.Values) {
                if (Time.FrameCount % a.Frequency == 0) {
                    a.Counter.Count(1);
                }
            }
        }
    }



    struct TileAnimation
    {
        public int TileIndex;
        public int Frequency;
        public int[] Data;
        public LoopCounter Counter;
    }
}