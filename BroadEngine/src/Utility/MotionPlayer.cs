using System.Collections.Generic;
using System;

namespace Broad
{
    public class MotionPlayer: Component
    {
        Dictionary<string, IMotion> motions = new Dictionary<string, IMotion>();
        bool playing = false;
        string currentMotionName = "";

        /// <summary>
        /// 再生中かどうか
        /// </summary>
        public bool IsPlaying => playing;

        /// <summary>
        /// 現在のモーション
        /// </summary>
        public ref readonly IMotion? CurrentMotion => ref currentMotion;

        IMotion? currentMotion;


        /// <summary>
        /// モーションを再生
        /// </summary>
        /// <param name="name">モーションの名前</param>
        public void Play(string name)
        {
            if (!motions.ContainsKey(name)) {
                throw new Exception($"モーション{name}は登録されていません");
            }
            playing = true;
            currentMotion = motions[name];
            currentMotionName = name;
            currentMotion.OnStartPlaying(Actor, this);
        }

        /// <summary>
        /// モーションを再生
        /// </summary>
        public void Play()
        {
            if (!playing) {
                Play(currentMotionName);
            }
        }


        /// <summary>
        /// モーションを最初から再生
        /// </summary>
        /// <param name="name">モーションの名前</param>
        public void Restart(string name)
        {
            playing = true;
            currentMotion = motions[name];
            currentMotion.Reset();
        }


        /// <summary>
        /// モーションを追加
        /// </summary>
        /// <param name="name">モーションの名前</param>
        /// <param name="motion">追加するモーション</param>
        public void AddMotion(string name, IMotion motion) 
        { 
            motions.Add(name, motion);
        }

        /// <summary>
        /// モーションを削除
        /// </summary>
        /// <param name="name">モーションの名前</param>
        public void RemoveMotion(string name)
        {
            if (!motions.TryRemove(name)) {
                Console.WriteLine($"モーション{name}の削除に失敗しました");
            }
        }

        protected internal override void OnUpdate(Actor actor)
        {   
            if (!playing) 
                return;
            
            currentMotion?.OnUpdate(actor, this);
        }


        /// <summary>
        /// モーションを停止
        /// </summary>
        public void Stop() 
        {
            playing = false;
        }
    }
}
