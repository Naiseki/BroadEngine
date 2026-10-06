using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Broad
{
    /// <summary>
    /// シーンを管理するマネージャー
    /// </summary>
    public static class SceneUtil
    {
        /// <summary>
        /// シーンを切り替える
        /// </summary>
        public static void ChangeScene<T>() where T: Scene
            => ChangeScene(typeof(T));


        /// <summary>
        /// シーンを切り替える
        /// </summary>
        public static void ChangeScene(Type sceneType)
        {
            var prevScene = Game.ActiveScene;
            var newScene = Activator.CreateInstance(sceneType) as Scene;
            Game.ActiveScene = newScene;
            newScene.Start();
            SceneChanged?.Invoke(prevScene, newScene);
            prevScene.Destroy();
        }


        /// <summary>
        /// 非同期でシーンを切り替える
        /// </summary>
        /// <param name="sceneType">シーンの型</param>
        /// <returns>タスク</returns>
        public static async Task ChangeSceneAsync(Type sceneType)
        {
            await Task.Run(() => ChangeScene(sceneType));
        }


        /// <summary>
        /// 非同期でシーンを切り替える
        /// </summary>
        /// <returns>タスク</returns>
        public static async Task ChangeSceneAsync<T>() where T: Scene
            => await ChangeSceneAsync(typeof(T));
        

        /// <summary>
        /// シーンが切り替わった時のイベント
        /// </summary>
        public static event Action<Scene, Scene> SceneChanged;
    }
}