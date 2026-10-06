using System.ComponentModel;
using System;
using System.Collections.Generic;
using Broad.Physics;
using MathKit;


namespace Broad
{
    /// <summary>
    /// ゲーム内のオブジェクト
    /// </summary>
    public class Actor: IDestroyable
    {
        /// <summary>
        /// 有効かどうか
        /// </summary>
        public bool Enabled = true;

        /// <summary>
        /// アップデート処理を行うかどうか
        /// </summary>
        public bool UpdateEnabled = true;

        /// <summary>
        /// トランスフォーム
        /// </summary>
        public Transform Form = Transform.Identity;

        /// <summary>
        /// タグ
        /// </summary>
        public string Tag = "";

         /// <summary>
         /// このアクタを保持しているシーン
         /// </summary>
        public Scene? HolderScene { get; internal set; }

        /// <summary>
        /// OnStart()が呼ばれたかどうか
        /// </summary>
        public bool IsStarted { get; private set; } = false;

        /// <summary>
        /// このアクタが破棄されたかどうか
        /// </summary>
        public bool IsDestroyed => isDestroyed;

        /// <summary>
        /// 座標
        /// </summary>
        public ref Float2 Position => ref Form.Position;

        /// <summary>
        /// 座標
        /// </summary>
        public ref Float2 Scale => ref Form.Scale;
        /// <summary>
        /// 座標
        /// </summary>
        public ref float Rotation => ref Form.Rotation;

        /// <summary>
        /// 保持しているコンポーネントの数
        /// </summary>
        public int ComponentCount => components.Count;

        /// <summary>
        /// 更新可能かどうか
        /// </summary>
        public bool CanUpdate => (UpdateEnabled && Enabled && IsStarted);


        private bool isDestroyed = false;

        private List<Component> components = new List<Component>();

        private CommandBuffer componentCmdBuf = new CommandBuffer();


        /// <summary>
        /// 初期化
        /// </summary>
        protected Actor() {}


        /// <summary>
        /// 毎フレームで呼び出される
        /// </summary>
        protected virtual void OnUpdate() {}


        /// <summary>
        /// レンダリング実行時に呼び出される
        /// </summary>
        protected internal virtual void OnRender() {}


        /// <summary>
        /// 他のコライダーに衝突した時に呼び出される
        /// </summary>
        /// <param name="args">コリジョンイベント情報</param>
        protected internal virtual void OnCollide(CollisionEventArgs args) {}


        /// <summary>
        /// シーンに追加後の最初のフレームで呼び出される
        /// </summary>
        protected virtual void OnStart() {}


        /// <summary>
        /// このアクタが破棄されるときに呼び出される
        /// </summary>
        protected internal virtual void OnDestroy() {}


        /// <summary>
        /// コンポーネントを追加
        /// </summary>
        /// <param name="component">追加するコンポーネント</param>
        public void AddComponent(Component component) 
        {
            if (component.IsAttached) {
                throw new Exception("既にアタッチ済みのコンポーネントを追加することはできません");
            }
            component.Actor = this;
            componentCmdBuf.Add(new ComponentStartCommand(component));
            components.Add(component);
        }


        /// <summary>
        /// コンポーネントの追加を試みる
        /// </summary>
        /// <param name="c">追加したコンポーネント</param>
        /// <returns>追加に成功したかどうか</returns>
        public bool TryAddComponent(Component c)
        {
            if (c.IsAttached) {
                return false;
            }
            c.Actor = this;
            componentCmdBuf.Add(new ComponentStartCommand(c));
            components.Add(c);
            return true;
        }


        /// <summary>
        /// コンポーネントを追加
        /// </summary>
        /// <param name="components">追加するコンポーネント</param>
        public void AddComponents(params Component[] components)
        {
            foreach (var c in components) {
                AddComponent(c);
            }
        }


        /// <summary>
        /// コンポーネントを削除
        /// </summary>
        /// <param name="component">削除するコンポーネント</param>
        public void RemoveComponent(Component component) 
        {
            component.OnDestroy(component.Actor);
            components.Remove(component);
        }


        /// <summary>
        /// コンポーネントを削除
        /// </summary>
        /// <typeparam name="T">削除するコンポーネントの型</typeparam>
        public void RemoveComponent<T>() where T: Component 
            => RemoveComponent(typeof(T));


        /// <summary>
        /// コンポーネントを削除
        /// </summary>
        /// <param name="componentType">削除するコンポーネントの型</param>
        /// <returns>削除に成功したかどうか</returns>
        public void RemoveComponent(Type componentType)
        {
            int index = -1;
            for (int i = 0; i < components.Count; i++) {
                if (components[i].GetType() == componentType) {
                    index = i;
                    break;
                }
            }

            if (index >= 0) { 
                components.RemoveAt(index);
            }
            else {
                throw new Exception("削除するコンポーネントが存在しません");
            }
        }


        /// <summary>
        /// コンポーネントの削除を試みる
        /// </summary>
        /// <typeparam name="T">削除するコンポーネントの型</typeparam>
        /// <returns>削除に成功したかどうか</returns>
        public bool TryRemoveComponent<T>() where T: Component
        {
            try {
                 RemoveComponent(typeof(T));
                 return true;
            }
            catch {
                return false;
            }
        }


        /// <summary>
        /// コンポーネントの削除を試みる
        /// </summary>
        /// <param name="componentType">削除するコンポーネントの型</param>
        /// <returns>削除に成功したかどうか</returns>
        public bool TryRemoveComponent(Type componentType)
        {
            try {
                 RemoveComponent(componentType);
                 return true;
            }
            catch {
                return false;
            }
        }


        /// <summary>
        /// コンポーネントの削除を試みる
        /// </summary>
        /// <param name="c">削除するコンポーネント</param>
        /// <returns>削除に成功したかどうか</returns>
        public bool TryRemoveComponent(Component c)
        {
            int x = components.IndexOf(c);
            if (x >= 0) {
                components.RemoveAt(x);
                return true;
            }
            return false;
        }


        /// <summary>
        /// コンポーネントを検索
        /// </summary>
        /// <typeparam name="T">検索するコンポーネントの型</typeparam>
        /// <returns>取得したコンポーネント</returns>
        public T GetComponent<T>() where T: Component
        {
            var type = typeof(T);
            for (int i = 0; i < components.Count; i++) {
                if (components[i].GetType() == type)
                    return components[i] as T;
            }
            throw new Exception($"{typeof(T)}型のコンポーネントはアタッチされていません");
        }


        /// <summary>
        /// コンポーネントの取得を試みる
        /// </summary>
        /// <param name="comp">取得したコンポーネント</param>
        /// <typeparam name="T">検索するコンポーネントの型</typeparam>
        /// <returns>取得に成功したかどうか</returns>
        public bool TryGetComponent<T>(out T comp) where T: Component
        {
            try {
                 comp = GetComponent<T>();
                 return true;
            }
            catch {
                comp = default;
                return false;
            }
        }


        /// <summary>
        /// アクタを破棄
        /// </summary>
        public void Destroy()
        {
            if (isDestroyed) 
                return;
            ActorCommands.Destroy(this);
            foreach (var c  in components) {
                c.OnDestroy(this);
            }
            isDestroyed = true;
        }


        internal void Start() 
        {
            if (!IsStarted) {
                OnStart();
                IsStarted = true;
            }
        }


        internal void Update() 
        {
            componentCmdBuf.Execute();

            if (!Enabled) return;
            
            if (CanUpdate) {
                OnUpdate();
            }
            for (int i = 0; i < components.Count; i++) {
                var comp = components[i];
                if (comp.Enabled)
                    comp.OnUpdate(this);
            }
        }
    }
}