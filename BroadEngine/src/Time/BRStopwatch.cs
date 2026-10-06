using System;
using System.Collections.Generic;

namespace Broad
{
    /// <summary>
    /// ストップウォッチ
    /// </summary>
    public struct BRStopwatch
    {
        public bool IsRunning { get; private set; }

        public TimeSpan StartTime { get; private set; }

        public TimeSpan CurrentTime {
            get {
                if (IsRunning) {
                    return currentTime;
                }
                return Time.Current - StartTime;
            }
        }


        public static BRStopwatch StartNew()
        {
            var watch = new BRStopwatch();
            watch.Start();
            return watch;
        }


        public void Start()
        {
            StartTime = Time.Current;
            IsRunning = true;
        }


        public void Stop()
        {
            currentTime = Time.Current;
            IsRunning = false;
        }


        public void Reset()
        {
            currentTime = TimeSpan.Zero;
            IsRunning = false;
        }


        private TimeSpan currentTime;
    }
}