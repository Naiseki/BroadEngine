using System;
using System.Diagnostics;
using Broad.Physics;

namespace Broad
{
    /// <summary>
    /// 時間を扱うクラス
    /// </summary>
    public static class Time
    {
        static Stopwatch watch;
        static float nowTime = 0f;       //Current frame time
        static float prevTime = 0f;      //Previous frame time
        static float deltaTime = 0f;     
        static float frameRate = 0f;     
        static int frameCount = 0;     
        static TimeSpan current;     

        /// <summary>
        /// 現在時間
        /// </summary>
        public static TimeSpan Current => current;

        /// <summary>
        /// 小数での秒単位の現在時間
        /// </summary>
        public static float Seconds => nowTime;

        /// <summary>
        /// １フレームにかかった時間
        /// </summary>
        public static float DeltaTime => deltaTime;

        /// <summary>
        /// フレームレート
        /// </summary>
        public static float FrameRate => frameRate;

        /// <summary>
        /// ゲーム開始時からの総フレーム数
        /// </summary>
        public static float FrameCount => frameCount;


        internal static void Start()
        {
            watch = Stopwatch.StartNew();
            Update();
        } 

        internal static void Update()
        {
            current = watch.Elapsed;
            nowTime = (float)current.TotalSeconds;
            deltaTime = nowTime - prevTime;
            frameRate = 1f / deltaTime;
            prevTime = nowTime;
            frameCount++;
            PhysicsUtil.DeltaTime = Math.Min(deltaTime, PhysicsUtil.MaxDeltaTime);
        }
    }
}