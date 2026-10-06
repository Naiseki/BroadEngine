using System;
using System.Collections.Generic;

namespace Broad
{
    public class GameEvent
    {
        List<Action> handlers = new List<Action>();


        public void AddHandler(Action handler)
        {
            handlers.Add(handler);
        }


        public void RemoveHandler(Action handler) 
        {
            handlers.Remove(handler);
        }


        public void ClearHandlers()
        {
            handlers.Clear();
        }


        public void Dispatch()
        {
            for (int i = 0; i < handlers.Count; i++) {
                handlers[i].Invoke();
            }
        }
    }
}