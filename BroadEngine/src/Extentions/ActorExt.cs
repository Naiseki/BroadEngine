using System;

namespace Broad
{
    /// <summary>
    /// アクタの拡張メソッド
    /// </summary>
    public static class ActorExt
    {
        /// <summary>
        /// コンポーネントを追加して返す
        /// </summary>
        /// <param name="actor">アクタ</param>
        /// <param name="c">追加するコンポーネント</param>
        /// <returns>コンポーネントを追加済みのアクタ</returns>
        public static T With<T>(this T actor, Component c) where T: Actor
        {
            if (actor.TryAddComponent(c)) {
                return actor;
            }
            Console.WriteLine("コンポーネントを追加できませんでした");
            return actor;
        }


        /// <summary>
        /// コンポーネントを削除して返す
        /// </summary>
        /// <param name="actor">アクタ</param>
        /// <param name="c">削除するコンポーネント</param>
        /// <returns>コンポーネントを削除済みのアクタ</returns>
        public static T Without<T>(this T actor, Component c) where T: Actor
        {
            if (actor.TryRemoveComponent(c)) {
                return actor;
            }
            Console.WriteLine("コンポーネントを追加できませんでした");
            return actor;
        }


        /// <summary>
        /// コンポーネントを削除して返す
        /// </summary>
        /// <param name="actor">アクタ</param>
        /// <param name="componentType">削除するコンポーネントの型</param>
        /// <returns>コンポーネントを削除済みのアクタ</returns>
        public static T Without<T>(this T actor, Type componentType) where T: Actor
        {
            if (actor.TryRemoveComponent(componentType)) {
                return actor;
            }
            Console.WriteLine("コンポーネントを追加できませんでした");
            return actor;
        }
    }
}
