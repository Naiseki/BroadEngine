using System;
using System.Collections.Generic;
using GLF = OpenTK.Windowing.GraphicsLibraryFramework;


namespace Broad
{
    /// <summary>
    /// 入力情報を渡すクラス
    /// </summary>
    public static class Input
    {
        internal static bool MustUpdate = false;
        static readonly Dictionary<Keys, int> keys = new Dictionary<Keys, int>();
        static readonly Keys[] keyValues = Enum.GetValues<Keys>();


        internal static void Init()
        {
            foreach (var key in keyValues) {
                keys[key] = 0;
            }
        }


        internal static void Update(GLF.KeyboardState state)
        {
            if (!MustUpdate)
                return;

            bool mustUpdate = false;
            foreach (var key in keyValues) {
                if (key == Keys.Unknown)
                    continue;

                if (state.IsKeyDown((GLF.Keys)key)) {
                    keys[(Keys)key]++;
                    mustUpdate = true;
                }
                else if (keys[(Keys)key] > 0) {
                    keys[(Keys)key] = -1;
                    mustUpdate = true;
                }
                else {
                    keys[(Keys)key] = 0;
                }
            }

            MustUpdate = mustUpdate;
        }


        /// <summary>
        /// キーが押されたかどうか
        /// </summary>
        /// <param name="key">キーコード</param>
        /// <returns>true: 押された瞬間 false: その他</returns>
        public static bool IsPressed(Keys key) => (0 < keys[key] && keys[key] < 2); 


        /// <summary>
        /// キーが離されたかどうか
        /// </summary>
        /// <param name="key">キーコード</param>
        /// <returns>true: 離された瞬間 false: その他</returns>
        public static bool IsReleased(Keys key) => (keys[key] == -1); 



        /// <summary>
        /// キーが押されているかどうか
        /// </summary>
        /// <param name="key">キーコード</param>
        /// <returns>true: 押されている false: 押されていない</returns>
        public static bool IsKeyDown(Keys key) => (keys[key] > 0);


        /// <summary>
        /// キーが押されていないかどうか
        /// </summary>
        /// <param name="key">キーコード</param>
        /// <returns>true: 押されていない false: 押されている</returns>
        public static bool IsKeyUp(Keys key) => (keys[key] < 1);


        /// <summary>
        /// キーが押されていたかどうか
        /// </summary>
        /// <param name="key">キーコード</param>
        /// <returns>true: 前のフレームで押されていた false: 押されていなかった</returns>
        public static bool WasKeyDown(Keys key) => (keys[key] == -1);


        /// <summary>
        /// キーが押されている間のフレーム数
        /// </summary>
        /// <param name="key">キーコード</param>
        /// <returns>キーのフレーム数。0の時は押されていない、-1の時前フレームまで押されていた</returns>
        public static int GetKeyFrameCount(Keys key) => keys[key];
    }
}