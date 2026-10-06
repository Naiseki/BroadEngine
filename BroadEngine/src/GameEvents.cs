using System;
using System.Collections.Generic;

namespace Broad
{
    public static class GameEvents
    {
        private static Dictionary<string, GameEvent> events = new Dictionary<string, GameEvent>();

        /// <summary>
        /// イベントを追加
        /// </summary>
        /// <param name="name">イベントの名前</param>
        public static void AddEvent(string name)
        {
            if (events.ContainsKey(name)) {
                Console.WriteLine($"Event '{name}' has already registered");
                return;
            }
            events.Add(name, new GameEvent());
        }


        /// <summary>
        /// イベントを追加を試みる
        /// </summary>
        /// <param name="name">イベントの名前</param>
        public static bool TryAddEvent(string name)
        {
            if (events.ContainsKey(name)) {
                return false;
            }
            events.Add(name, new GameEvent());
            return true;
        }


        /// <summary>
        /// イベントリスナーを追加
        /// </summary>
        /// <param name="eventName">イベントの名前</param>
        /// <param name="handler">リスナー</param>
        public static void AddHandler(string eventName, Action handler)
        {
            if (events.ContainsKey(eventName)) {
                events[eventName].AddHandler(handler);
            }
            else {
                Console.WriteLine($"Event '{eventName}' is not registered");
            }
        }


        /// <summary>
        /// イベントリスナーを削除
        /// </summary>
        /// <param name="eventName">イベントの名前</param>
        /// <param name="handler">リスナー</param>
        public static void RemoveHnAddHandler(string eventName, Action handler)
        {
            if (events.ContainsKey(eventName)) {
                events[eventName].RemoveHandler(handler);
            }
            else {
                Console.WriteLine($"Event '{eventName}' is not registered");
            }
        }


        /// <summary>
        /// イベントを起こす
        /// </summary>
        /// <param name="eventName">イベントの名前</param>
        public static void Dispatch(string eventName)
        {
            if (events.TryGetValue(eventName, out var ev)) {
                ev.Dispatch();
                return;
            }
            Console.WriteLine($"Can't dispatch event '{eventName}'");
        }
    }
}