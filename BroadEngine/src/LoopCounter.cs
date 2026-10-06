using MathKit;

namespace Broad
{
    /// <summary>
    /// 一定の範囲内をループし続けるカウンタ
    /// </summary>
    public struct LoopCounter: ICounter
    {
        /// <summary>
        /// カウンタの値（読み取り専用）
        /// </summary>
        public int Value => counter + Min;

        /// <summary>
        /// このカウンタの最小値（読み取り専用）
        /// </summary>
        public int Min { get; private set; }

        /// <summary>
        /// このカウンタの最大値（読み取り専用）
        /// </summary>
        public int Max { get; private set; }

        /// <summary>
        /// このカウンタが取りうる範囲内の数（読み取り専用）
        /// </summary>
        public int Range => Max - Min + 1;

        /// <summary>
        /// 現在の値が最大値に等しいかどうか
        /// </summary>
        /// <returns>true: 最大値に等しい　false: 等しくない</returns>
        public bool IsMax => (Value == Max);

        /// <summary>
        /// 現在の値が最小値に等しいかどうか
        /// </summary>
        /// <returns>true: 最小値に等しい　false: 等しくない</returns>
        public bool IsMin => (Value == Min);


        int counter;

        /// <summary>
        /// カウンタを初期化
        /// </summary>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        public LoopCounter(int min, int max)
        {
            Min = min; 
            Max = max;
            counter = 0;
        }


        /// <summary>
        /// カウント
        /// </summary>
        /// <param name="x">カウントする数</param>
        public void Count(int x)
        {
            counter = MathUtil.PositiveMod(counter + x, Range);
        }


        /// <summary>
        /// このカウンタをリセット
        /// </summary>
        public void Reset() => counter = 0;


        /// <summary>
        /// 範囲を設定
        /// </summary>
        /// <param name="min">最小値</param>
        /// <param name="max">最大値</param>
        public void SetRange(int min, int max)
        {
            Min = min; 
            Max = max;
            counter = 0;
        }


        /// <summary>
        /// 値を設定
        /// </summary>
        /// <param name="value">設定する値</param>
        public void SetValue(int value)
        {
            counter = System.Math.Clamp(value - Min, 0, Max - Min);
        }
    }
}