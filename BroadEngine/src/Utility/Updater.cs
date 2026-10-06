using System.Collections.Generic;
using System;

namespace Broad
{
    public static class Updater
    {
        static List<UpdatableAction> actions = new List<UpdatableAction>();  
        static Queue<UpdatableAction> removables = new Queue<UpdatableAction>();



        public static void Set(Action action, Func<bool> condition)
        {
            actions.Add(new UpdatableAction(action, condition));
        }



        internal static void Run()
        {
            for (int i = 0; i < actions.Count; i++) {
                actions[i].Update();
            }

            while (removables.Count > 0) {
                actions.Remove(removables.Dequeue());
            }
        }


        internal static void AddRemovables(in UpdatableAction action)
        {
            removables.Enqueue(action);
        }
    }



    readonly struct UpdatableAction
    {
        public readonly Action Action;
        public readonly Func<bool> Condition;


        public UpdatableAction(Action action, Func<bool> contidion)
        {
            Action = action;
            Condition = contidion;
        }


        public void Update()
        {
            if (Condition.Invoke()) {
                Action.Invoke();
            }
            else {
                Updater.AddRemovables(this);
            }
        }
    }
}
