using System;
using System.Collections.Generic;

namespace Broad
{
    /// <summary>
    /// インターバル
    /// </summary>
    public static class Interval
    {
        readonly static List<IntervalSystem> intervals = new List<IntervalSystem>();


        /// <summary>
        /// インターバルを設定
        /// </summary>
        /// <param name="action">インターバルで実行する処理</param>
        /// <param name="seconds">設定する秒数</param>
        public static void Set(Action action, float seconds)
        {
            intervals.Add(new IntervalSystem(action, seconds));
        }


        internal static void Update()
        {
            for (int i = 0; i < intervals.Count; i++) {
                intervals[i].Update();
            }
        }
    }



    class IntervalSystem
    {
        public readonly Action Action;
        public readonly float Frequency;

        private float nextTime;


        public IntervalSystem(Action action, float frequency)
        {
            Action = action;
            Frequency = frequency;
            nextTime = Time.Seconds + frequency;
        }


        public bool IsElapsed => (Time.Seconds >= nextTime);


        public void Update()
        {
            while (IsElapsed) {
                Action.Invoke();
                nextTime += Frequency;
            }
        }
    }
}