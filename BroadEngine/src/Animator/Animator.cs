using System.Collections.Generic;

namespace Broad
{
    /// <summary>
    /// アニメーションを行うクラス
    /// </summary>
    public class Animator: Component
    {
        Dictionary<string, int[]> anims = new Dictionary<string, int[]>();
        LoopCounter counter = new LoopCounter(0, 1);
        bool playing = false;
        int[] currentAnim = new int[] {0};


        /// <summary>
        /// スプライトのタイルインデクス
        /// </summary>
        public int Index => currentAnim[counter.Value];

        /// <summary>
        /// 再生中かどうか
        /// </summary>
        public bool IsPlaying => playing;

        /// <summary>
        /// 現在のアニメ-ションの名前
        /// </summary>
        public string CurrentAnimName { get; private set; } = "";

        /// <summary>
        /// 何フレームおきに更新するか
        /// </summary>
        public int Frequency = 3;

        /// <summary>
        /// アニメーションをループ再生するかどうか
        /// </summary>
        public bool Looping = true;



        /// <summary>
        /// 再生するアニメーションを決定
        /// </summary>
        /// <param name="name">アニメーションの名前</param>
        public void Play(string name)
        {
            playing = true;
            
            //すでに再生中のアニメーションの場合は実行しない
            if (name == CurrentAnimName) {       
                return;
            }
            currentAnim = anims[name];
            CurrentAnimName = name;
            counter.SetRange(0, currentAnim.Length - 1);
            counter.Reset();
        }


        /// <summary>
        /// 再生するアニメーションを決定
        /// </summary>
        /// <param name="name">アニメーションの名前</param>
        /// <param name="frequency">何フレームおきに更新するか</param>
        public void Play(string name, int frequency)
        {
            Frequency = frequency;
            Play(name);
        }

        /// <summary>
        /// アニメーションを再生
        /// </summary>
        /// <param name="name">アニメーションの名前</param>
        /// <param name="looping">ループ再生させるかどうか</param>
        public void Play(string name, bool looping)
        {
            Looping = looping;
            Play(name);
        }

        /// <summary>
        /// アニメーションを再生
        /// </summary>
        /// <param name="name">アニメーションの名前</param>
        /// <param name="frequency">何フレームおきに更新するか</param>
        /// <param name="looping">ループ再生させるかどうか</param>
        public void Play(string name, int frequency, bool looping)
        {
            Frequency = frequency;
            Looping = looping;
            Play(name);
        }

        /// <summary>
        /// アニメーションを再生
        /// </summary>
        public void Play()
        {
            playing = true;
        }

        /// <summary>
        /// アニメーションを追加
        /// </summary>
        /// <param name="data">アニメーションデータの配列</param>
        /// <param name="name">アニメーションの名前</param>
        public void AddAnimation(int[] data, string name) => anims.Add(name, data);

        /// <summary>
        /// アニメーションのフレームを切り替える
        /// </summary>
        protected internal override void OnUpdate(Actor actor)
        {   
            if (!playing) 
                return;
            
            if (Looping)
                AnimateLoop();
            else
                AnimateOnce();
        }

        /// <summary>
        /// アニメーションを停止
        /// </summary>
        public void Stop()
        {
            playing = false;
        }

        /// <summary>
        /// 状態をリセット
        /// </summary>
        public void Reset()
        {
            Stop();
            counter.Reset();
        }

        void AnimateLoop()
        { 
            if (Time.FrameCount % Frequency == 0) {
                counter.Count(1);
            }
        }

        void AnimateOnce()
        { 
            if (Time.FrameCount % Frequency == 0) {
                counter.Count(1);
            }

            if (counter.IsMax)
                Stop();
        }
    }
}