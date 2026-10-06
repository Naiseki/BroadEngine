namespace Broad
{
    /// <summary>
    /// アクタのコンポーネント
    /// </summary>
    public class Component
    {
        protected Component() {}


        /// <summary>
        /// 有効/無効
        /// </summary>
        public bool Enabled = true;

        /// <summary>
        /// このコンポーネントがアクタにアタッチされているか
        /// </summary>
        public bool IsAttached => (Actor is not null);


        /// <summary>
        /// アクティブな状態かどうか
        /// </summary>
        public bool IsActive => (Enabled && IsAttached && Actor.Enabled);

        
        internal Actor Actor;


        /// <summary>
        /// 開始時に呼び出される
        /// </summary>
        protected internal virtual void OnStart(Actor actor) {}

        /// <summary>
        /// 毎フレームで呼び出される
        /// </summary>
        protected internal virtual void OnUpdate(Actor actor) {}

        /// <summary>
        /// 破棄されるときに呼び出される
        /// </summary>
        protected internal virtual void OnDestroy(Actor actor) {}
    }


    readonly struct ComponentStartCommand: ICommand
    {
        public readonly Component Component;

        
        public ComponentStartCommand(Component component)
        {
            Component = component;
        }


        public void Execute()
        {
            Component.OnStart(Component.Actor);
        }
    }
}
