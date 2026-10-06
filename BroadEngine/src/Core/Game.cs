using System;
using MathKit;
using Broad.Graphics;


namespace Broad
{ 
    public static class Game
    {
        public static Scene ActiveScene { get; internal set; }
        public static float AspectRatio { get; private set; }
        public static Int2 ScreenSize => new Int2(window.Size.X, window.Size.Y);
        public static float Meter;
        
        static Window window;
        

        /// <summary>
        /// ゲームを起動
        /// </summary>
        /// <param name="settings">ゲームの設定</param>
        /// <param name="firstSceneType">最初に開くシーン</param>
        public static void Run(GameSettings settings, Type firstSceneType)
        {
            window = new Window(settings.WindowSize, settings.Title, settings.RenderFrequency, settings.UpdateFrequency, firstSceneType);
            GraphicsUtil.LayerCount = settings.GraphicsLayerCount;
            ActiveScene = Scene.Identity;
            AspectRatio = new Float2(settings.WindowSize).Divided;
            SceneUtil.ChangeScene(firstSceneType);   
            window.Run();
        }


        /// <summary>
        /// ゲームを終了
        /// </summary>
        public static void Quit()
        {
            window.Close();
            window.Dispose();
        }
    }


    public struct GameSettings
    {
        public Int2 WindowSize;
        public int GraphicsLayerCount;
        public string Title;
        public int RenderFrequency;
        public int UpdateFrequency;
    }
}
