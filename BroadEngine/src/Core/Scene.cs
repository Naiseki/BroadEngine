using System;
using System.Collections.Generic;
using Broad.Physics;
using Broad.Graphics;
using MathKit;

namespace Broad
{
    /// <summary>
    /// シーン
    /// </summary>
    public class Scene: IDestroyable
    {
        List<Actor> actors = new List<Actor>();
        Stack<Actor> entries = new Stack<Actor>();
        Queue<Actor> destroyOrders = new Queue<Actor>();
        List<Scene> childScenes = new List<Scene>();
        RenderWorld renderWorld = new RenderWorld(GraphicsUtil.LayerCount);
        internal PhysicsWorld physWorld = new PhysicsWorld();
        bool isDestroyed = false;


        public bool Enabled = true;

        public bool IsDestroyed => isDestroyed;

        /// <summary>
        /// アクタの数
        /// </summary>
        public int ActorCount => actors.Count;


        /// <summary>
        /// 初期化
        /// </summary>
        protected Scene() {}


        /// <summary>
        /// アクターを追加
        /// </summary>
        /// <param name="actor">追加するアクター</param>
        public void AddActor(Actor actor) 
        {
            actor.HolderScene = this;
            ActorCommands.Spawn(actor);
        }


        /// <summary>
        /// 複数のアクターを追加
        /// </summary>
        /// <param name="actors">追加するアクター</param>
        public void AddActors(params Actor[] actors) 
        {
            foreach (var actor in actors) {
                AddActor(actor);
            }
        }


        /// <summary>
        /// シーンからアクターを削除 ただしDestroy()は呼ばれない
        /// </summary>
        /// <param name="actor">削除するアクター</param>
        public void RemoveActor(Actor actor) => actors.Remove(actor);


        internal void PushActor(Actor actor)
        {
            actors.Add(actor);
            entries.Push(actor);
        }


        /// <summary>
        /// アクターを取得
        /// </summary>
        /// <param name="actor">出力するアクター</param>
        /// <typeparam name="T">検索するアクターの型</typeparam>
        /// <returns>検索対象のアクターが存在するかどうか</returns>
        public bool TryGetActor<T>(out T actor) where T: Actor
        {
            for (int i = 0; i < actors.Count; i++) {
                if (actors[i].GetType() == typeof(T)) {
                    actor = actors[i] as T;
                    return true;
                }
            }
            actor = default;
            return false;
        }


        /// <summary>
        /// 複数のアクターを取得
        /// </summary>
        /// <param name="outputActors">出力するアクターのリスト</param>
        /// <typeparam name="T">検索するアクターの型</typeparam>
        /// <returns>検索対象のアクターが存在するかどうか</returns>
        public bool TryGetActors<T>(out List<T> outputActors) where T: Actor
        {
            outputActors = new List<T>();
            for (int i = 0; i < actors.Count; i++) {
                if (actors[i].GetType() == typeof(T)) {
                    outputActors.Add(actors[i] as T);
                }
            }
            
            return outputActors.Count > 0;
        }


        /// <summary>
        /// 子シーンを追加
        /// </summary>
        /// <param name="child">追加するシーン</param>
        public void AddChildScene(Scene child) 
        {
            child.OnStart();
            childScenes.Add(child);
        }


        /// <summary>
        /// 子シーンを削除
        /// </summary>
        /// <param name="child">削除するシーン</param>
        public void RemoveChildScene(Scene child) 
        {
            childScenes.Remove(child);
        }

        /// <summary>
        /// 全ての子シーンを削除
        /// </summary>
        public void ClearChildScenes() => childScenes.Clear();


        /// <summary>
        /// レンダラを追加
        /// </summary>
        /// <param name="renderer">追加するレンダラ</param>
        /// <param name="layer">レイヤー</param>
        internal void AddRenderer(RendererBase renderer, int layer) 
        {
            renderer.RenderWorld = renderWorld;
            renderWorld.AddRenderer(renderer, layer);
        }


        internal void RemoveRenderer(RendererBase renderer)
        {
            renderWorld.RemoveRenderer(renderer);
        }
        

        /// <summary>
        /// リジッドボディを追加
        /// </summary>
        /// <param name="body">追加するリジッドボディ</param>
        internal void AddRigidBody(RigidBody body)
        {
            physWorld.AddBody(body);
        }

        
        internal void RemoveRigidBody(RigidBody body)
        {
            physWorld.RemoveBody(body);
        }


        internal bool Hit(Float2 point) => physWorld.Hit(point);


        void UpdateActors()
        {
            for (int i = 0; i < actors.Count; i++) {
                actors[i].Update();
            }
        }


        internal void Render()
        { 
            renderWorld.Step();
        }


        void CallStart()
        {
            while (entries.Count > 0) {
                entries.Pop().Start();
            }
        }



        void UpdateChildScenes()
        {
            for (int i = 0; i < childScenes.Count; i++) {
                childScenes[i].Update();
            }
        }


        internal void Update()
        {
            if (!Enabled) return;

            CallStart();
            OnUpdate();
            UpdateActors();
            physWorld.Step();
        }


        /// <summary>
        /// シーンを破棄
        /// </summary>
        public void Destroy()
        {
            if (isDestroyed)
                return;
            OnDestroy();
            physWorld.Destroy();
            foreach (var actor in actors) {
                actor.Destroy();
            }
            isDestroyed = true;
        }


        internal void Start()
        {
            OnStart();
            ActorCommands.Execute();
            CallStart();
        }



        /// <summary>
        /// 毎フレームで呼び出される
        /// </summary>
        protected virtual void OnUpdate() {}


        /// <summary>
        /// シーンが開いた最初のフレームで呼び出される
        /// </summary>
        protected virtual void OnStart() {}

        /// <summary>
        /// このシーンがアンロードされるときに呼び出される
        /// </summary>
        protected virtual void OnDestroy() {}


        public static readonly Scene Identity = new Scene();


        ~Scene() => Destroy();
    }
}