using System.Collections.Generic;
using System;

namespace Broad
{
    /// <summary>
    /// モーション
    /// </summary>
    public interface IMotion
    {
        /// <summary>
        /// 更新時呼び出し
        /// </summary>
        void OnUpdate(Actor actor, MotionPlayer player);

        /// <summary>
        /// 再生開始時呼び出し
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="player"></param>
        void OnStartPlaying(Actor actor, MotionPlayer player) {}

        /// <summary>
        /// モーションを初期化する
        /// </summary>
        void Reset() {}
    }
}
